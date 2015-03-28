using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using TechnicSolderHelper.SQL;
using TechnicSolderHelper.SQL.workTogether;

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
                if (!mcmod.Aredone)
                {
                    ModListSqlHelper modListSqlHelper = new ModListSqlHelper();
                    Mcmod m = modListSqlHelper.GetModInfo(SqlHelper.CalculateMd5(mcmod.Path));
                    if (m == null)
                    {
                        if (mcmod.Authors == null || mcmod.AuthorList == null)
                        {
                            String a = _solderHelper.GetAuthors(mcmod, true);
                            if (!String.IsNullOrWhiteSpace(a))
                            {
                                List<String> s = a.Replace(" ", "").Split(',').ToList();
                                mcmod.Authors = s;
                            }
                        }
                        if (!IsValid(mcmod.Mcversion))
                        {
                            mcmod.Mcversion = _solderHelper._currentMcVersion;
                        }
                        if (AreModDone(mcmod))
                        {
                            mcmod.Aredone = true;
                        }
                        else
                        {
                            DataSuggest ds = new DataSuggest();
                            m = ds.GetMcmod(SqlHelper.CalculateMd5(mcmod.Path));
                            if (m != null)
                            {
                                if (!IsValid(mcmod.Name))
                                {
                                    mcmod.Name = m.Name;
                                }
                                if (!IsValid(mcmod.Modid))
                                {
                                    mcmod.Modid = m.Modid;
                                }
                                if (!IsValid(mcmod.Version))
                                {
                                    mcmod.Version = m.Version;
                                }
                                mcmod.FromSuggestion = true;
                            }
                            if (AreModDone(mcmod))
                            {
                                mcmod.Aredone = true;
                            }
                            else
                            {
                                _nonFinishedMods.Add(mcmod);
                                modlist.Items.Add(String.IsNullOrWhiteSpace(mcmod.Name) ? mcmod.Filename : mcmod.Name);
                            }
                        }
                    }
                    if (!mcmod.Aredone)
                    {

                        if (m != null)
                        {
                            if (!IsValid(mcmod.Name) &&
                                !String.IsNullOrWhiteSpace(m.Name))
                            {
                                mcmod.Name = m.Name;
                            }
                            if (!IsValid(mcmod.Modid) && !String.IsNullOrWhiteSpace(m.Modid))
                            {
                                mcmod.Modid = m.Modid;
                            }
                            if (!IsValid(mcmod.Version) && !String.IsNullOrWhiteSpace(m.Version))
                            {
                                mcmod.Version = m.Version;
                            }
                        }
                        if (!IsValid(mcmod.Mcversion))
                        {
                            mcmod.Mcversion = _solderHelper._currentMcVersion;
                        }
                        if (mcmod.Authors == null || mcmod.AuthorList == null)
                        {
                            String a = _solderHelper.GetAuthors(mcmod, true);
                            if (a != null)
                            {
                                List<String> s = a.Replace(" ", "").Split(',').ToList();
                                mcmod.Authors = s;
                            }
                        }
                        if (AreModDone(mcmod))
                        {
                            mcmod.Aredone = true;
                        }
                        else
                        {
                            DataSuggest ds = new DataSuggest();
                            m = ds.GetMcmod(SqlHelper.CalculateMd5(mcmod.Path));
                            if (m != null)
                            {
                                if (!IsValid(mcmod.Name))
                                {
                                    mcmod.Name = m.Name;
                                }
                                if (!IsValid(mcmod.Modid))
                                {
                                    mcmod.Modid = m.Modid;
                                }
                                if (!IsValid(mcmod.Version))
                                {
                                    mcmod.Version = m.Version;
                                }
                                mcmod.FromSuggestion = true;
                            }
                            if (AreModDone(mcmod))
                            {
                                mcmod.Aredone = true;
                            }
                            else
                            {
                                if (!_nonFinishedMods.Contains(mcmod))
                                {
                                    _nonFinishedMods.Add(mcmod);
                                    modlist.Items.Add(String.IsNullOrWhiteSpace(mcmod.Name) ? mcmod.Filename : mcmod.Name);
                                }
                            }
                        }
                    }
                }
            }
            if (modlist.Items.Count <= 0)
            {
            }
            else
            {
                modlist.SelectedIndex = 0;
            }
        }

        private Boolean IsValid(String s)
        {
            if (String.IsNullOrWhiteSpace(s) || s.Contains("@") || s.Contains("${") || s.ToLower().Contains("example"))
            {
                return false;
            }
            return true;
        }

        private static Boolean IsFullyInformed(Mcmod mod)
        {
            if (String.IsNullOrWhiteSpace(mod.Name) || String.IsNullOrWhiteSpace(mod.Version) ||
                String.IsNullOrWhiteSpace(mod.Mcversion) || String.IsNullOrWhiteSpace(mod.Modid) || (mod.AuthorList == null && mod.Authors == null))
                return false;
            Debug.WriteLine(mod.Version);
            if (mod.Name.Contains("${") || mod.Version.Contains("${") || mod.Mcversion.Contains("${") || mod.Modid.Contains("${") || mod.Version.ToLower().Contains("@version@") || mod.Modid.ToLower().Contains("example") || mod.Version.ToLower().Contains("example") || mod.Name.ToLower().Contains("example"))
            {
                return false;
            }
            return true;
        }

        private Boolean AreModDone(Mcmod mod)
        {
            if (!IsFullyInformed(mod)) return false;
            OwnPermissionsSqlHelper ownPermissionsSqlHelper = new OwnPermissionsSqlHelper();
            bool b = ownPermissionsSqlHelper.DoUserHavePermission(mod.Modid).HasPermission;
            if (b)
            {
                return true;
            }
            if (_solderHelper.CreateTechnicPack.Checked && _solderHelper.TechnicPermissions.Checked)
            {
                if (_ftbPermissionsSqlHelper.DoFtbHavePermission(mod.Modid,
                    _solderHelper.TechnicPublicPermissions.Checked) != PermissionLevel.Open)
                {
                    return false;
                }
            }
            if (!_solderHelper.CreateFTBPack.Checked) return true;
            PermissionLevel p = _ftbPermissionsSqlHelper.DoFtbHavePermission(mod.Modid,
                _solderHelper.PublicFTBPack.Checked);
            if (p == PermissionLevel.Open || p == PermissionLevel.Ftb)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void modlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = modlist.SelectedIndex;
            if (index < 0) return;
            Mcmod m = showDone.Checked ? _mods[index] : _nonFinishedMods[index];
            skipmod.Checked = m.IsSkipping;
            textBoxFileName.Text = m.Filename;
            textBoxAuthor.Text = m.Authors != null ? _solderHelper.GetAuthors(m) : String.Empty;
            textBoxModName.Text = m.Name ?? String.Empty;

            textBoxModID.Text = m.Modid ?? String.Empty;
            //_updatingModID = String.IsNullOrWhiteSpace(textBoxModID.Text);
            //textBoxModID.ReadOnly = !String.IsNullOrWhiteSpace(textBoxModID.Text);

            textBoxModVersion.Text = m.Version ?? String.Empty;

            ShowPermissions();
        }

        private void textBoxModName_TextChanged(object sender, EventArgs e)
        {
            int index = modlist.SelectedIndex;
            var mod = showDone.Checked ? _mods[index] : _nonFinishedMods[index];
            mod.FromUserInput = true;
            if (String.IsNullOrWhiteSpace(textBoxModName.Text))
            {
                mod.Name = String.Empty;
                modlist.Items[index] = mod.Filename;
            }
            else
            {
                mod.Name = textBoxModName.Text;
                modlist.Items[index] = mod.Name;
            }
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
                Width -= technicPermissions.Width;
            }
            else
            {
                if (!FTBPermissions.Visible)
                {
                    Width -= FTBPermissions.Width;
                }
            }
        }

        private void ShowPermissions()
        {
            textBoxTechnicLicenseLink.Text = String.Empty;
            textBoxTechnicModLink.Text = String.Empty;
            textBoxTechnicPermissionLink.Text = String.Empty;
            textBoxFTBLicenseLink.Text = String.Empty;
            textBoxFTBModLink.Text = String.Empty;
            textBoxFTBPermissionLink.Text = String.Empty;
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
            if (FTBPermissions.Visible)
            {
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
            OwnPermissionsSqlHelper ownPermissionsSqlHelper = new OwnPermissionsSqlHelper();
            OwnPermissions permissions = ownPermissionsSqlHelper.DoUserHavePermission(textBoxModID.Text);
            if (!permissions.HasPermission)
            {
                textBoxTechnicPermissionLink.Text = String.Empty;
                textBoxFTBPermissionLink.Text = String.Empty;
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(permissions.PermissionLink))
                {
                    textBoxTechnicPermissionLink.Text = permissions.PermissionLink;
                    textBoxFTBPermissionLink.Text = permissions.PermissionLink;
                }
                if (!String.IsNullOrWhiteSpace(permissions.ModLink))
                {
                    textBoxTechnicModLink.Text = permissions.ModLink;
                    textBoxFTBModLink.Text = permissions.ModLink;
                }
                if (!String.IsNullOrWhiteSpace(permissions.LicenseLink))
                {
                    textBoxTechnicLicenseLink.Text = permissions.LicenseLink;
                    textBoxFTBLicenseLink.Text = permissions.LicenseLink;
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
            mod.FromUserInput = true;
        }

        private void textBoxAuthor_TextChanged(object sender, EventArgs e)
        {
            int index = modlist.SelectedIndex;
            Mcmod mod = showDone.Checked ? _mods[index] : _nonFinishedMods[index];
            if (String.IsNullOrWhiteSpace(textBoxAuthor.Text))
            {
                mod.Authors = null;
            }
            else
            {
                String a = textBoxAuthor.Text;
                List<String> s = a.Replace(" ", "").Split(',').ToList();
                mod.Authors = s;
                if (!String.IsNullOrWhiteSpace(textBoxModID.Text))
                {
                    OwnPermissionsSqlHelper ownPermissionsSqlHelper = new OwnPermissionsSqlHelper();
                    ownPermissionsSqlHelper.AddAuthor(textBoxModID.Text, a);
                }
            }
            mod.FromUserInput = true;
        }

        private void modinfo_Closing(object sender, CancelEventArgs e)
        {
            foreach (Mcmod mcmod in _mods)
            {
                if (mcmod.IsSkipping)
                    continue;
                if (String.IsNullOrWhiteSpace(mcmod.Name))
                {
                    e.Cancel = true;
                    MessageBox.Show("Please check all mods and make sure the info is filled in." +
                                    Environment.NewLine + "Issue with mod: " + mcmod.Filename);
                    return;
                }
                if (String.IsNullOrWhiteSpace(mcmod.Modid))
                {
                    mcmod.Modid = mcmod.Name.Replace(" ", "").ToLower();
                }
                if (!AreModDone(mcmod))
                {
                    e.Cancel = true;
                    MessageBox.Show("Please check all mods and make sure the info is filled in." +
                                    Environment.NewLine + "Issue with mod: " + mcmod.Filename);
                    return;
                }
                mcmod.Aredone = true;
            }
            if (!e.Cancel)
            {
                foreach (Mcmod mcmod in _mods)
                {
                    if (mcmod.FromUserInput && !mcmod.FromSuggestion)
                    {
                        Debug.WriteLine(mcmod.Modid);
                        DataSuggest ds = new DataSuggest();
                        String a = _solderHelper.GetAuthors(mcmod, true);
                        ds.Suggest(mcmod.Filename, mcmod.Mcversion, mcmod.Version,
                            SqlHelper.CalculateMd5(mcmod.Path), mcmod.Modid, mcmod.Name, a);
                    }
                    if (_solderHelper.CreateFTBPack.Checked)
                    {
                        _solderHelper.CreateFtbPackZip(mcmod, mcmod.Path);
                    }
                    if (_solderHelper.CreateTechnicPack.Checked)
                    {
                        _solderHelper.CreateTechnicModZip(mcmod, mcmod.Path);
                    }
                }
            }
        }

        private void textBoxModID_TextChanged(object sender, EventArgs e)
        {
            int index = modlist.SelectedIndex;
            Mcmod mod = showDone.Checked ? _mods[index] : _nonFinishedMods[index];
            mod.Modid = !String.IsNullOrWhiteSpace(textBoxModID.Text) ? textBoxModID.Text : String.Empty;
            mod.FromUserInput = true;

        }

        private void Done_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBoxPermissionLink_TextChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(e);
            var box = sender as TextBox;
            if (box != null)
            {
                if (!String.IsNullOrWhiteSpace(box.Text))
                {
                    if (Uri.IsWellFormedUriString(box.Text, UriKind.Absolute))
                    {
                        String modid = textBoxModID.Text;
                        String modname = textBoxModName.Text;
                        OwnPermissionsSqlHelper ownPermissionsSqlHelper = new OwnPermissionsSqlHelper();
                        ownPermissionsSqlHelper.AddOwnModPerm(modname, modid, box.Text);

                    }
                }
            }
        }

        private void textBoxModLink_TextChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(e);
            var box = sender as TextBox;
            if (box != null)
            {
                if (!String.IsNullOrWhiteSpace(box.Text))
                {
                    if (Uri.IsWellFormedUriString(box.Text, UriKind.Absolute))
                    {
                        String modid = textBoxModID.Text;
                        OwnPermissionsSqlHelper ownPermissionsSqlHelper = new OwnPermissionsSqlHelper();
                        String modname = textBoxModName.Text;
                        ownPermissionsSqlHelper.AddOwnModLink(modname, modid, box.Text);
                    }
                }
            }
        }

        private void textBoxLicenseLink_TextChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(e);
            var box = sender as TextBox;
            if (box != null)
            {
                if (!String.IsNullOrWhiteSpace(box.Text))
                {
                    if (Uri.IsWellFormedUriString(box.Text, UriKind.Absolute))
                    {
                        String modid = textBoxModID.Text;
                        String modname = textBoxModName.Text;
                        OwnPermissionsSqlHelper ownPermissionsSqlHelper = new OwnPermissionsSqlHelper();
                        ownPermissionsSqlHelper.AddOwnModLicense(modname, modid, box.Text);

                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int index = modlist.SelectedIndex;
            Mcmod mod = showDone.Checked ? _mods[index] : _nonFinishedMods[index];
            mod.IsSkipping = skipmod.Checked;
        }
    }
}
