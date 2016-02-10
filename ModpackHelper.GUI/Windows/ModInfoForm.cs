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
using ModpackHelper.GUI.Helpers;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Permissions;
using ModpackHelper.Shared.Permissions.FTB;
using ModpackHelper.Shared.UserInteraction;
using ModpackHelper.Shared.Utils.Config;
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
        private Modpack modpack;
        private PermissionDB permissionDB;

        public ModInfoForm(IFileSystem fileSystem, IMessageShower messageShower)
        {
            this.fileSystem = fileSystem;
            this.messageShower = messageShower;
            permissionDB = new PermissionDB(fileSystem);
            InitializeComponent();
        }

        /// <summary>
        /// Creates all the content in the form
        /// </summary>
        public void InitializeContent(List<Mcmod> modsList, string mcv, Modpack modpack)
        {
            this.modpack = modpack;

            technicPermissionsGroupBox.Visible = (modpack.CreateTechnicPack && modpack.CheckTechnicPermissions);

            // TODO create ftp permissions interaction
            ftbPermissionsGroupBox.Visible = false;

            mods = modsList;
            currentMcVersion = mcv;
            // Find all the mods that still needs info
            nonFinishedMods = mods.Where(mcmod => !IsValid(mcmod)).ToList();


            // No need to run over everything again if every mod if valid already
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
                nonFinishedMods = nonFinishedMods.Where(mcmod => !IsValid(mcmod)).ToList();
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
            }
            Notifier.FlashWindow(this);
        }

        private bool IsValid(Mcmod m)
        {
            bool valid = m.IsValid();
            if (valid && modpack.CreateTechnicPack && modpack.CheckTechnicPermissions)
            {
                var permission = permissionDB.CanModBeDestributed(m, !modpack.TechnicPermissionsPrivate, false);
                var userPermission = GetPermission(m.GetSafeModId(), Launcher.Technic);
                valid = permission == PermissionPolicy.Open || userPermission.IsDone();
            }
            // TODO Handle ftb permissions
            return valid;
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

            HandleTechnicPermissions(selectedMod);
            // TODO Handle ftb permissions
        }

        private void HandleTechnicPermissions(Mcmod mod)
        {
            // We don't have to worry about permissions if we aren't supposed to check them
            if (modpack.CreateTechnicPack && modpack.CheckTechnicPermissions)
            {
                var permission = permissionDB.CanModBeDestributed(mod, !modpack.TechnicPermissionsPrivate, false);
                if (permission == PermissionPolicy.Open)
                {
                    technicPermissionsGroupBox.Visible = false;
                    return;
                }
                technicPermissionsGroupBox.Visible = true;
                string message = "";
                switch (permission)
                {
                    case PermissionPolicy.Notify:
                        message = "This mod requires that you notify the author of inclusion.";
                        break;
                    case PermissionPolicy.Request:
                        message = "This mod requires that you request permission to include it in the modpack.";
                        break;
                    case PermissionPolicy.Unknown:
                        message = "Nothing is known about the permissions for this mod, please provide necesary links.";
                        break;
                    case PermissionPolicy.Closed:
                        message =
                            "Acording to the permission list i have you cannot destribute this mod. Please provide proof if this is not the case, or mark the mod as skipping. ";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                technicPermissionTextLabel.Text = message;
            }
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
            if (mods.Any(m => !m.IsSkipping && !IsValid(m)))
            {
                messageShower.ShowMessageAsync("You are not done entering info. Please finish!");
            }
            else
            {
                OnDoneFillingInInfo();
                permissionDB.SavePermissions();
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
                    selectedMod, showMessage ? messageShower : null
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
            mod.GetModInfoFromApi(messageShower: param[1] as IMessageShower).Wait();
        }

        private UserPermission GetPermission(string modid, Launcher launcher)
        {
            UserPermission permission =
                permissionDB.UserPermissions.FirstOrDefault(
                    up => up.Launcher == launcher && up.ModId.Equals(modid));
            if (permission != null) return permission;
            permission = new UserPermission { ModId = modid, Launcher = launcher };
            permissionDB.UserPermissions.Add(permission);
            return permission;
        }

        private void technicLinkToPermissionListingTextBox_TextChanged(object sender, EventArgs e)
        {
            var permission = GetPermission(selectedMod.GetSafeModId(), Launcher.Technic);
            permission.LinkToPermissionListing = technicLinkToPermissionListingTextBox.Text;
        }

        private void technicLinkToProofOfPermissionsTextBox_TextChanged(object sender, EventArgs e)
        {
            var permission = GetPermission(selectedMod.GetSafeModId(), Launcher.Technic);
            permission.LinkToProofOfPermissions = technicLinkToProofOfPermissionsTextBox.Text;
        }

        private void ftbLinkToPermissionListingTextBox_TextChanged(object sender, EventArgs e)
        {
            var permission = GetPermission(selectedMod.GetSafeModId(), Launcher.FTB);
            permission.LinkToPermissionListing = ftbLinkToPermissionListingTextBox.Text;
        }

        private void ftbLinkToProofOfPermissionTextBox_TextChanged(object sender, EventArgs e)
        {
            var permission = GetPermission(selectedMod.GetSafeModId(), Launcher.FTB);
            permission.LinkToProofOfPermissions = ftbLinkToProofOfPermissionTextBox.Text;
        }

        private void GetPermissionButton_Click(object sender, EventArgs e)
        {
            HandleTechnicPermissions(selectedMod);
        }
    }
}
