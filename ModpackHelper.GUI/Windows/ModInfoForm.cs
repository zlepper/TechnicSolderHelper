using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Abstractions;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.UserInteraction;
using ModpackHelper.Shared.Web.Api;

namespace ModpackHelper.GUI.Windows
{
    public delegate void DoneFillingInInfoEventHandler(List<Mcmod> mods);

    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public partial class ModInfoForm : Form
    {
        public event DoneFillingInInfoEventHandler DoneFillingInInfo;

        /// <summary>
        /// Called when everything is filled in, and the form closes
        /// </summary>
        protected virtual void OnDoneFillingInInfo()
        {
            DoneFillingInInfo?.Invoke(mods.Where(m => !m.IsSkipping).ToList());
        }

        /// <summary>
        /// The list of mods that are being packed
        /// </summary>
        private List<Mcmod> mods;
        /// <summary>
        /// A list of mods which needs some info before they can be packed properly
        /// </summary>
        private List<Mcmod> nonFinishedMods;
        /// <summary>
        /// The current Minecraft version we are packing against
        /// </summary>
        private string currentMcVersion;
        private readonly IFileSystem fileSystem;
        /// <summary>
        /// The currently selected mod in the modSelectionList
        /// </summary>
        private Mcmod selectedMod;
        private readonly IMessageShower messageShower;

        public ModInfoForm(IFileSystem fileSystem, IMessageShower messageShower)
        {
            this.fileSystem = fileSystem;
            this.messageShower = messageShower;
            InitializeComponent();
        }

        /// <summary>
        /// Creates all the content in the form
        /// </summary>
        public void InitializeContent(List<Mcmod> modsList, string mcv)
        {
            mods = modsList;
            currentMcVersion = mcv;
            // Find all the mods that still needs info
            nonFinishedMods = mods.Where(m => !m.IsValid()).ToList();
            // No need to run over everything again is every mod is valid already
            if (nonFinishedMods.Count > 0)
            {
                foreach (Mcmod mod in nonFinishedMods)
                {
                    // Set the minecraft version if it's missing
                    if (string.IsNullOrWhiteSpace(mod.Mcversion))
                        mod.Mcversion = currentMcVersion;
                    // Set the authorlist if it's missing
                    if (mod.AuthorList == null || mod.AuthorList.Count == 0)
                    {
                        mod.GetAuthors(fileSystem);
                    }
                }
                // Find all the mods that still misses info
                nonFinishedMods = nonFinishedMods.Where(m => !m.IsValid()).ToList();
            }
            // If all the mods have info then just close the form and continue
            if (nonFinishedMods.Count == 0)
            {
                Debug.WriteLine("Nothing to do!");
            }
            else
            {
                // Add all the missing info mods to the modlist
                foreach (Mcmod mod in nonFinishedMods)
                {
                    ModSelectionList.Items.Add(mod.GetPath().Name);
                }
                // Select the first mod on the list
                ModSelectionList.SelectedIndex = 0;
                foreach (Mcmod mod in nonFinishedMods)
                {
                    using (BackgroundWorker bw = new BackgroundWorker())
                    {
                        bw.DoWork += BwFetchModInfoFromApi;
                        List<object> param = new List<object>()
                            {
                                mod,
                                null
                            };
                        bw.RunWorkerAsync(param);
                    }
                }
            }
        }

        private Mcmod GetSelectedMod()
        {
            int index = ModSelectionList.SelectedIndex;
            if (index < 0)
            {
                ModSelectionList.SelectedIndex = 0;
                return null;
            }
            return ShowDoneCheckBox.Checked ? mods[index] : nonFinishedMods[index];
        }
        

        private void ModSelectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedMod = GetSelectedMod();
            if (selectedMod == null) return;
            // Load data back into the mod info boxes
            SkipModCheckBox.Checked = selectedMod.IsSkipping;
            ModNameTextBox.Text = selectedMod.Name ?? string.Empty;
            ModVersionTextBox.Text = selectedMod.Version ?? string.Empty;
            FileNameTextBox.Text = selectedMod.GetPath().Name;

            // We can't display lists, so we'll display a nicely formatted string
            ModAuthorTextBox.Text = selectedMod.AuthorList != null ? string.Join(", ", selectedMod.AuthorList) : string.Empty;
            ModIDTextBox.Text = selectedMod.Modid ?? string.Empty;
        }

        private void ModNameTextBox_TextChanged(object sender, EventArgs e)
        {
            // If the modname is empty, then we should display the filename in the modlist
            selectedMod.Name = string.IsNullOrWhiteSpace(ModNameTextBox.Text) ? string.Empty : ModNameTextBox.Text;
            selectedMod.FromUser = true;
        }

        // Make sure we update the modid
        private void ModIDTextBox_TextChanged(object sender, EventArgs e)
        {
            selectedMod.Modid = ModIDTextBox.Text;

            // Check if the new modid returns a mod author
            List<string> a = selectedMod.GetAuthors(fileSystem);
            if (a.Count > 0)
            {
                ModAuthorTextBox.Text = string.Join(", ", a);
            }
            selectedMod.FromUser = true;
        }

        // Make sure we update the mod version
        private void ModVersionTextBox_TextChanged(object sender, EventArgs e)
        {
            selectedMod.Version = ModVersionTextBox.Text;
            selectedMod.FromUser = true;
        }

        // Make sure we update the mod author
        private void ModAuthorTextBox_TextChanged(object sender, EventArgs e)
        {
            // replace.split.toList is done because we get a string, mod authorlist is a list of strings
            selectedMod.AuthorList = ModAuthorTextBox.Text.Replace(", ", ",").Split(',').ToList();
            selectedMod.FromUser = true;
        }

        // Make sure we update the skipping
        private void SkipModCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            selectedMod.IsSkipping = SkipModCheckBox.Checked;
        }

        // Close the form and cancel packing mods
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Close the form if the user is done entering info
        private void DoneButton_Click(object sender, EventArgs e)
        {
            if (mods.Any(m => !m.IsSkipping && !m.IsValid()))
            {
                messageShower.ShowMessageAsync("You are not done entering info. Please finish!");
            }
            else
            {
                OnDoneFillingInInfo();
                Close();
            }
        }

        private void ShowDoneCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // Empty the list so we don't mess up some ordering
            ModSelectionList.Items.Clear();
            if (ShowDoneCheckBox.Checked)
            {
                // Put all mods into the list
                foreach (Mcmod mod in mods)
                {
                    ModSelectionList.Items.Add(string.IsNullOrWhiteSpace(mod.Name) ? mod.GetPath().Name : mod.Name);
                }
            }
            else
            {
                // Only put the non-finished mods into the list
                foreach (Mcmod mod in nonFinishedMods)
                {
                    ModSelectionList.Items.Add(string.IsNullOrWhiteSpace(mod.Name) ? mod.GetPath().Name : mod.Name);
                }
            }
        }

        private void SkipAllButton_Click(object sender, EventArgs e)
        {
            foreach (Mcmod mod in nonFinishedMods)
            {
                mod.IsSkipping = true;
            }
        }

        private void CheckOnlineButton_Click(object sender, EventArgs e)
        {
            CheckOnline(true);
        }

        private void CheckOnline(bool showMessage = false)
        {
            using (BackgroundWorker bw = new BackgroundWorker())
            {
                bw.DoWork += BwFetchModInfoFromApi;
                List<object> param = new List<object>()
                {
                    selectedMod,
                    showMessage ? messageShower : null
                };
                bw.RunWorkerAsync(param);
            }
        }

        private void BwFetchModInfoFromApi(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            List<object> param = doWorkEventArgs.Argument as List<object>;
            Mcmod mod = param?[0] as Mcmod;

            if (mod == null) return;
            mod.DoneWithApi += () =>
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => ModSelectionList_SelectedIndexChanged(null, EventArgs.Empty)));
                }
            };
            mod.GetModInfoFromApi(param[1] as IMessageShower).Wait();
        }
    }
}
