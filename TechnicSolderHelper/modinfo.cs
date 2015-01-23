using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TechnicSolderHelper.SQL;

namespace TechnicSolderHelper
{
    public partial class Modinfo : Form
    {
        private readonly SolderHelper _solderHelper;
        private readonly List<Mcmod> _mods;
        private readonly List<Mcmod> _nonFinishedMods; 
        private readonly FtbPermissionsSqlHelper _ftbPermissionsSqlHelper = new FtbPermissionsSqlHelper();

        public Modinfo(SolderHelper solderHelper)
        {
            _solderHelper = solderHelper;
            InitializeComponent();
        }

        public Modinfo(List<Mcmod> modsList, SolderHelper solderHelper)
        {
            _nonFinishedMods = new List<Mcmod>();
            _solderHelper = solderHelper;
            var tmp = from mcmod1 in modsList
                where mcmod1.Name != null
                orderby mcmod1.Name
                select mcmod1;
            _mods = new List<Mcmod>();
            _mods.AddRange(tmp.ToList());
            tmp = from mcmod1 in modsList
                where mcmod1.Name == null
                orderby mcmod1.Filename
                select mcmod1;
            _mods.AddRange(tmp.ToList());
            InitializeComponent();
            foreach (Mcmod mcmod in _mods)
            {
                if (String.IsNullOrWhiteSpace(mcmod.Mcversion))
                    mcmod.Mcversion = solderHelper._currentMcVersion;
                mcmod.Aredone = AreModDone(mcmod);
                if (mcmod.Aredone) continue;
                _nonFinishedMods.Add(mcmod);
                modlist.Items.Add(String.IsNullOrWhiteSpace(mcmod.Name) ? mcmod.Filename : mcmod.Name);
            }
            modlist.SelectedIndex = 0;
        }

        private Boolean AreModDone(Mcmod mod)
        {
            if (SolderHelper.IsFullyInformed(mod))
            {
                if (_solderHelper.TechnicPermissions.Checked)
                {
                    if (_ftbPermissionsSqlHelper.DoFtbHavePermission(mod.Modid,
                        _solderHelper.TechnicPublicPermissions.Checked) != PermissionLevel.Open)
                    {
                        return false;
                    }
                }
                if (_solderHelper.CreateFTBPack.Checked)
                {
                    PermissionLevel p = _ftbPermissionsSqlHelper.DoFtbHavePermission(mod.Modid,
                        _solderHelper.PublicFTBPack.Checked);
                    if (p != PermissionLevel.Open && p != PermissionLevel.Ftb)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        } 

        private void modlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = modlist.SelectedIndex;
            if (index >= 0)
            {
                Mcmod m = showDone.Checked ? _mods[index] : _nonFinishedMods[index];
                textBoxFileName.Text = m.Filename;

                textBoxAuthor.Text = m.Authors != null ? _solderHelper.GetAuthors(m) : String.Empty;

                textBoxModName.Text = m.Name ?? String.Empty;

                textBoxModID.Text = m.Modid ?? String.Empty;
                textBoxModID.ReadOnly = !String.IsNullOrWhiteSpace(textBoxModID.Text);

                textBoxModVersion.Text = m.Version ?? String.Empty;

                ShowPermissions();
            }
        }

        private void textBoxModName_TextChanged(object sender, EventArgs e)
        {
            int index = modlist.SelectedIndex;
            var mod = showDone.Checked ? _mods[index] : _nonFinishedMods[index];

            if (!String.IsNullOrWhiteSpace(textBoxModName.Text))
            {
                mod.Name = textBoxModName.Text;
                modlist.Items[index] = mod.Name;
            }
            else
            {
                mod.Name = String.Empty;
                modlist.Items[index] = mod.Filename;
            }

            if (textBoxModID.ReadOnly)
                return;
            textBoxModID.Text = textBoxModName.Text.Replace(" ", "").ToLower();
        }

        private void showDone_CheckedChanged(object sender, EventArgs e)
        {
            if (showDone.Checked)
            {
                modlist.Items.Clear();
                foreach (Mcmod mcmod in _mods)
                    modlist.Items.Add(mcmod.Name ?? mcmod.Filename);
                toolTip1.SetToolTip(showDone,
                    "Only show items that are missing information or have other issues." + Environment.NewLine + "Currently showing all files.");
            }
            else
            {
                modlist.Items.Clear();
                foreach (Mcmod mod in _nonFinishedMods)
                    modlist.Items.Add(mod.Name ?? mod.Filename);
                toolTip1.SetToolTip(showDone, "Show all items, even the once without any issues." + Environment.NewLine + "Currently only showing files with issues.");
            }
        }

        private void Modinfo_Load(object sender, EventArgs e)
        {
            FTBPermissions.Visible = _solderHelper.CreateFTBPack.Checked;
            technicPermissions.Visible = _solderHelper.TechnicPermissions.Checked;

            if (!technicPermissions.Visible)
            {
                FTBPermissions.Location = technicPermissions.Location;
                this.Width -= technicPermissions.Width;
            }
            else
            {
                if (!FTBPermissions.Visible)
                {
                    this.Width -= FTBPermissions.Width;
                }
            }
        }

        private void ShowPermissions()
        {
            if (String.IsNullOrWhiteSpace(textBoxModID.Text)) return;
            if (technicPermissions.Visible)
            {
                PermissionLevel technicPermissionLevel =
                    _ftbPermissionsSqlHelper.DoFtbHavePermission(textBoxModID.Text,
                        _solderHelper.TechnicPublicPermissions.Checked);
                Debug.WriteLine(technicPermissionLevel);
                switch (technicPermissionLevel)
                {
                    case PermissionLevel.Open:
                        permissionTechnicOpen.Checked = true;
                        break;
                    case PermissionLevel.Notify:
                        permissionTechnicNotify.Checked = true;
                        break;
                    case PermissionLevel.Ftb:
                        permissionTechnicFTBExclusive.Checked = true;
                        break;
                    case PermissionLevel.Request:
                        permissionTechnicRequest.Checked = true;
                        break;
                    case PermissionLevel.Closed:
                        permissionTechnicClosed.Checked = true;
                        break;
                    case PermissionLevel.Unknown:
                        permissionTechnicUnknown.Checked = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if (FTBPermissions.Visible) {
                PermissionLevel ftbPermissionLevel = _ftbPermissionsSqlHelper.DoFtbHavePermission(textBoxModID.Text,
                    _solderHelper.PublicFTBPack.Checked);
                Debug.WriteLine(ftbPermissionLevel);
                switch (ftbPermissionLevel)
                {
                    case PermissionLevel.Open:
                        permissionFTBOpen.Checked = true;
                        break;
                    case PermissionLevel.Notify:
                        permissionFTBNotify.Checked = true;
                        break;
                    case PermissionLevel.Ftb:
                        permissionFTBFTBExclusive.Checked = true;
                        break;
                    case PermissionLevel.Request:
                        permissionFTBRequest.Checked = true;
                        break;
                    case PermissionLevel.Closed:
                        permissionFTBClosed.Checked = true;
                        break;
                    case PermissionLevel.Unknown:
                        permssionFTBUnknown.Checked = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            String modlink = _ftbPermissionsSqlHelper.GetInfoFromModId(textBoxModID.Text,
                FtbPermissionsSqlHelper.InfoType.ModLink);
            if (String.IsNullOrWhiteSpace(modlink))
            {
                if (technicPermissions.Visible)
                {
                    textBoxTechnicModLink.Text = String.Empty;
                }
                if (FTBPermissions.Visible)
                {
                    textBoxFTBModLink.Text = String.Empty;
                }
            }
            else
            {
                if (technicPermissions.Visible)
                {
                    textBoxTechnicModLink.Text = modlink;
                }
                if (FTBPermissions.Visible)
                {
                    textBoxFTBModLink.Text = modlink;
                }
            }
            String licenseLink = _ftbPermissionsSqlHelper.GetInfoFromModId(textBoxModID.Text,
                FtbPermissionsSqlHelper.InfoType.PermLink);
            if (String.IsNullOrWhiteSpace(licenseLink))
            {
                if (technicPermissions.Visible)
                {
                    textBoxTechnicLicenseLink.Text = String.Empty;
                }
                if (FTBPermissions.Visible)
                {
                    textBoxFTBLicenseLink.Text = String.Empty;
                }
            }
            else
            {
                if (technicPermissions.Visible)
                {
                    textBoxTechnicLicenseLink.Text = licenseLink;
                }
                if (FTBPermissions.Visible)
                {
                    textBoxFTBLicenseLink.Text = licenseLink;
                }
            }
        }
        

        private void getPermissions_Click(object sender, EventArgs e)
        {
            ShowPermissions();
        }

        private void textBoxModVersion_TextChanged(object sender, EventArgs e)
        {
            int index = modlist.SelectedIndex;
            Mcmod mod = showDone.Checked ? _mods[index] : _nonFinishedMods[index];
            mod.Version = !String.IsNullOrWhiteSpace(textBoxModVersion.Text) ? textBoxModVersion.Text : String.Empty;
        }

        private void textBoxAuthor_TextChanged(object sender, EventArgs e)
        {
            int index = modlist.SelectedIndex;
            Mcmod mod = showDone.Checked ? _mods[index] : _nonFinishedMods[index];
            if (!String.IsNullOrWhiteSpace(textBoxAuthor.Text))
            {
                String a = textBoxAuthor.Text;
                List<String> s = a.Split(',').ToList();
                mod.Authors = s;
            }
            else
            {
                mod.Authors = null;
            }
        }
    }
}
