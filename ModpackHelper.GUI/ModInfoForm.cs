using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModpackHelper.mods;
using ModpackHelper.Shared.Mods;
using ModpackHelper.UserInteraction;

namespace ModpackHelper.GUI
{
    public delegate void DoneFillingInInfoEventHandler(List<Mcmod> mods);

    public partial class ModInfoForm : Form
    {
        public event DoneFillingInInfoEventHandler DoneFillingInInfo;

        /// <summary>
        /// Called when everything is filled in, and the form closes
        /// </summary>
        public virtual void OnDoneFillingInInfo()
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

        public ModInfoForm( IFileSystem fileSystem, IMessageShower messageShower)
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
                    if (string.IsNullOrWhiteSpace(mod.Mcversion)) mod.Mcversion = currentMcVersion;
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
                OnDoneFillingInInfo();
                Close();
            }
            else
            {
                // Add all the missing info mods to the modlist
                foreach (Mcmod mod in nonFinishedMods)
                {
                    ModSelectionList.Items.Add(string.IsNullOrWhiteSpace(mod.Name) ? mod.GetPath().Name : mod.Name);
                }
                // Select the first mod on the list
                ModSelectionList.SelectedIndex = 0;
            }
        }

        private Mcmod GetSelectedMod()
        {
            int index = ModSelectionList.SelectedIndex;
            return ShowDoneCheckBox.Checked ? mods[index] : nonFinishedMods[index];
        }

        private void SetSelectedModsTextInList(string text)
        {
            ModSelectionList.Items[ModSelectionList.SelectedIndex] = text;
        }

        private void ModSelectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedMod = GetSelectedMod();
            
            // Load data back into the mod info boxes
            SkipModCheckBox.Checked = selectedMod.IsSkipping;
            ModNameTextBox.Text = selectedMod.Name ?? string.Empty;
            FileNameTextBox.Text = selectedMod.GetPath().Name;

            // We can't display lists, so we'll display a nicely formatted string
            ModAuthorTextBox.Text = selectedMod.AuthorList != null ? string.Join(", ", selectedMod.AuthorList) : string.Empty;
            ModIDTextBox.Text = selectedMod.Modid ?? string.Empty;
        }

        private void ModNameTextBox_TextChanged(object sender, EventArgs e)
        {
            // If the modname is empty, then we should display the filename in the modlist
            if (string.IsNullOrWhiteSpace(ModNameTextBox.Text))
            {
                selectedMod.Name = string.Empty;
                SetSelectedModsTextInList(selectedMod.GetPath().Name);
            }
            else
            {
                // Otherwise just display the modname
                selectedMod.Name = ModNameTextBox.Text;
                SetSelectedModsTextInList(selectedMod.Name);
            }
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
        }

        // Make sure we update the mod version
        private void ModVersionTextBox_TextChanged(object sender, EventArgs e)
        {
            selectedMod.Version = ModVersionTextBox.Text;
        }

        // Make sure we update the mod author
        private void ModAuthorTextBox_TextChanged(object sender, EventArgs e)
        {
            // replace.split.toList is done because we get a string, mod authorlist is a list of strings
            selectedMod.AuthorList = ModAuthorTextBox.Text.Replace(", ", ",").Split(',').ToList();
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
    }
}
