using System;
using System.IO;
using System.Windows.Forms;
using TechnicSolderHelper.SQL;

namespace TechnicSolderHelper
{
    public partial class SolderHelper : Form
    {
        private void CreateFtbPermissionInfo(string modname, string modid, string modauthor, string linkToPermission)
        {
            string output = string.Format("{0}({1}) by {2} {3}Permission: {4} {3}{3}", modname, modid, modauthor, Environment.NewLine, linkToPermission);
            File.AppendAllText(_ftbPermissionList, output);
        }

        private void CreateFtbPermissionInfo(Mcmod mod, PermissionPolicy pl, string customPermissionText = null)
        {
            string modlink = _ftbPermsSqLhelper.GetPermissionFromModId(mod.Modid).modLink;
            while (string.IsNullOrWhiteSpace(modlink) || !Uri.IsWellFormedUriString(modlink, UriKind.Absolute))
            {
                modlink = Prompt.ShowDialog("What is the link to " + mod.Name + "?", "Mod link", false, Prompt.ModsLeftString(_totalMods, _currentMod));
                if (!Uri.IsWellFormedUriString(modlink, UriKind.Absolute))
                {
                    MessageBox.Show("Not a proper url");
                }
            }
            CreateFtbPermissionInfo(mod, pl, customPermissionText, modlink);
        }

        private void CreateFtbPermissionInfo(Mcmod mod, PermissionPolicy pl, string customPermissionText, string modlink)
        {
            string ps = string.Format("{0}({1}) by {2}{3}At {4}{3}Permissions are {5}{3}", mod.Name, mod.Modid, GetAuthors(mod), Environment.NewLine, modlink, pl);
            if (!string.IsNullOrWhiteSpace(customPermissionText))
            {
                ps += customPermissionText + Environment.NewLine;
            }
            File.AppendAllText(_ftbPermissionList, ps + Environment.NewLine);
        }

        public void CreateFtbPackZip(Mcmod mod, string modfile)
        {
            if (mod.IsSkipping)
            {
                return;
            }
            string fileName = modfile.Substring(modfile.LastIndexOf(Globalfunctions.PathSeperator) + 1);
            if (IsWierdMod(fileName) == 0)
            {
                return;
            }
            string modMd5 = SqlHelper.CalculateMd5(modfile);
            if (!mod.IsIgnore)
            {
                if (!mod.UseShortName)
                {
                    _modsSqLhelper.AddMod(mod.Name, mod.Modid, mod.Version, mod.Mcversion, fileName, modMd5, false);
                }
                if (true)
                {
                    #region Permissions checking
                    PermissionPolicy permLevel = _ftbPermsSqLhelper.FindPermissionPolicy(mod.Modid, PublicFTBPack.Checked);

                    string overwritelink;
                    OwnPermissions ownPermissions;
                    switch (permLevel)
                    {
                        case PermissionPolicy.Open:
                            break;
                        case PermissionPolicy.Notify:
                            ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                            if (!ownPermissions.HasPermission)
                            {
                                overwritelink = Prompt.ShowDialog(string.Format("{0} requires that you notify the author of inclusion.{1}Please provide proof(an imgur link) that you have done this:{1}Enter \"skip\" to skip the mod.", mod.Name, Environment.NewLine), mod.Name).Replace(" ", "");
                                while (true)
                                {
                                    if (overwritelink.ToLower().Equals("skip".ToLower()))
                                    {
                                        mod.IsSkipping = true;
                                        return;
                                    }
                                    if (Uri.IsWellFormedUriString(overwritelink, UriKind.Absolute))
                                    {
                                        if (overwritelink.ToLower().Contains("imgur"))
                                        {
                                            _ownPermsSqLhelper.AddOwnModPerm(mod.Name, mod.Modid, overwritelink);
                                            //Get Author
                                            string a = _ftbPermsSqLhelper.GetPermissionFromModId(mod.Modid).modAuthors;
                                            CreateFtbPermissionInfo(mod.Name, mod.Modid, a, overwritelink);
                                            break;
                                        }
                                        MessageBox.Show("Not an imgur link");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid url");
                                    }
                                    overwritelink = Prompt.ShowDialog(string.Format("{0} requires that you notify the author of inclusion.{1}Please provide proof(an imgur link) that you have done this:{1}Enter \"skip\" to skip the mod.", mod.Name, Environment.NewLine), mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod)).Replace(" ", "");
                                }
                            }
                            else
                            {
                                //Get Author
                                string a = _ftbPermsSqLhelper.GetPermissionFromModId(mod.Modid).modAuthors;
                                CreateFtbPermissionInfo(mod.Name, mod.Modid, a, ownPermissions.PermissionLink);
                            }
                            break;
                        case PermissionPolicy.FTB:
                            break;
                        case PermissionPolicy.Request:
                            ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                            if (!ownPermissions.HasPermission)
                            {
                                overwritelink = Prompt.ShowDialog(string.Format("This mod requires that you request permissions from the Mod Author of {0}{1}Please provide proof(an imgur link) that you have this permission:{1}Enter \"skip\" to skip the mod.", mod.Name, Environment.NewLine), mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod)).Replace(" ", "");
                                while (true)
                                {
                                    if (overwritelink.ToLower().Equals("skip".ToLower()))
                                    {
                                        mod.IsSkipping = true;
                                        return;
                                    }
                                    if (Uri.IsWellFormedUriString(overwritelink, UriKind.Absolute))
                                    {
                                        if (overwritelink.ToLower().Contains("imgur"))
                                        {
                                            _ownPermsSqLhelper.AddOwnModPerm(mod.Name, mod.Modid, overwritelink);
                                            //Get Author
                                            string a = _ftbPermsSqLhelper.GetPermissionFromModId(mod.Modid).modAuthors;
                                            CreateFtbPermissionInfo(mod.Name, mod.Modid, a, overwritelink);
                                            break;
                                        }
                                        MessageBox.Show("Not an imgur link");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid url");
                                    }
                                    overwritelink = Prompt.ShowDialog(string.Format("This mod requires that you request permissions from the Mod Author of {0}{1}Please provide proof(an imgur link) that you have this permission:{1}Enter \"skip\" to skip the mod.", mod.Name, Environment.NewLine), mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod)).Replace(" ", "");
                                }
                            }
                            else
                            {
                                //Get Author
                                string a = _ftbPermsSqLhelper.GetPermissionFromModId(mod.Modid).modAuthors;
                                CreateFtbPermissionInfo(mod.Name, mod.Modid, a, ownPermissions.PermissionLink);
                            }
                            break;
                        case PermissionPolicy.Closed:
                            ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                            if (!ownPermissions.HasPermission)
                            {
                                overwritelink = Prompt.ShowDialog(string.Format("The FTB permissionsheet states that permissions for {0} is closed.{1}Please provide proof(an imgur link) that this is not the case:{1}Enter \"skip\" to skip the mod.", mod.Name, Environment.NewLine), mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod)).Replace(" ", "");
                                while (true)
                                {
                                    if (overwritelink.ToLower().Equals("skip".ToLower()))
                                    {
                                        mod.IsSkipping = true;
                                        return;
                                    }
                                    if (Uri.IsWellFormedUriString(overwritelink, UriKind.Absolute))
                                    {
                                        if (overwritelink.ToLower().Contains("imgur"))
                                        {
                                            _ownPermsSqLhelper.AddOwnModPerm(mod.Name, mod.Modid, overwritelink);
                                            //Get Author
                                            string a = _ftbPermsSqLhelper.GetPermissionFromModId(mod.Modid).modAuthors;
                                            CreateFtbPermissionInfo(mod.Name, mod.Modid, a, overwritelink);
                                            break;
                                        }
                                        MessageBox.Show("Not an imgur link");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid url");
                                    }
                                    overwritelink = Prompt.ShowDialog(string.Format("The FTB permissionsheet states that permissions for {0} is closed.{1}Please provide proof(an imgur link) that this is not the case:{1}Enter \"skip\" to skip the mod.", mod.Name, Environment.NewLine), mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod)).Replace(" ", "");
                                }
                            }
                            else
                            {
                                //Get Author
                                string a = _ftbPermsSqLhelper.GetPermissionFromModId(mod.Modid).modAuthors;
                                CreateFtbPermissionInfo(mod.Name, mod.Modid, a, ownPermissions.PermissionLink);
                            }
                            break;
                        case PermissionPolicy.Unknown:
                            ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                            if (!ownPermissions.HasPermission)
                            {
                                overwritelink = Prompt.ShowDialog(string.Format("Permissions for {0} is unknown{1}Please provide proof(an imgur link) of permissions:{1}Enter \"skip\" to skip the mod.", mod.Name, Environment.NewLine), mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod)).Replace(" ", "");
                                while (true)
                                {
                                    if (overwritelink.ToLower().Equals("skip".ToLower()))
                                    {
                                        mod.IsSkipping = true;
                                        return;
                                    }
                                    if (Uri.IsWellFormedUriString(overwritelink, UriKind.Absolute))
                                    {
                                        if (overwritelink.ToLower().Contains("imgur"))
                                        {
                                            break;
                                        }
                                        MessageBox.Show("Not an imgur link");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid url");
                                    }
                                    overwritelink = Prompt.ShowDialog(string.Format("Permissions for {0} is unknown{1}Please provide proof(an imgur link) of permissions:{1}Enter \"skip\" to skip the mod.", mod.Name, Environment.NewLine), mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod)).Replace(" ", "");
                                }
                                string modLink;
                                while (true)
                                {
                                    modLink = Prompt.ShowDialog(string.Format("Please provide a link to {0}:{1}Enter \"skip\" to skip the mod.", mod.Name, Environment.NewLine), mod.Name).Replace(" ", "");
                                    if (modLink.ToLower().Equals("skip".ToLower()))
                                    {
                                        mod.IsSkipping = true;
                                        return;
                                    }
                                    if (Uri.IsWellFormedUriString(modLink, UriKind.Absolute))
                                    {
                                        _ownPermsSqLhelper.AddOwnModPerm(mod.Name, mod.Modid, overwritelink, modLink);
                                        break;

                                    }
                                    MessageBox.Show("Invalid url");

                                }
                                string a = GetAuthors(mod);
                                CreateOwnPermissionInfo(mod.Name, mod.Modid, a, overwritelink, modLink);

                            }
                            else
                            {
                                string a = GetAuthors(mod);
                                CreateOwnPermissionInfo(mod.Name, mod.Modid, a, ownPermissions.PermissionLink, ownPermissions.ModLink);
                            }
                            break;
                    }
                    #endregion
                }
            }

            if (string.IsNullOrWhiteSpace(_ftbModpackArchive))
            {
                while (string.IsNullOrWhiteSpace(_modpackName))
                {
                    _modpackName = Prompt.ShowDialog("What is the Modpack Name?", "Modpack Name");
                }
                while (string.IsNullOrWhiteSpace(_modpackVersion))
                {
                    _modpackVersion = Prompt.ShowDialog("What Version is the modpack?", "Modpack Version");
                }
                if (string.IsNullOrWhiteSpace(_modpackArchive))
                {
                    _modpackArchive = Path.Combine(_outputDirectory, string.Format("{0}-{1}.zip", _modpackName, _modpackVersion));
                    _ftbModpackArchive = Path.Combine(_outputDirectory, _modpackName + "-" + _modpackVersion + "-FTB" + ".zip");
                }

            }


            string tempModDirectory = Path.Combine(_outputDirectory, "minecraft", "mods");
            Directory.CreateDirectory(tempModDirectory);
            string tempFile = Path.Combine(tempModDirectory, fileName);
            int index = tempFile.LastIndexOf(Globalfunctions.PathSeperator);
            string tempFileDirectory = tempFile.Remove(index);
            Directory.CreateDirectory(tempFileDirectory);
            File.Copy(modfile, tempFile, true);

            if (Globalfunctions.IsUnix())
            {
                Environment.CurrentDirectory = _outputDirectory;
                _startInfo.FileName = "zip";
                _startInfo.Arguments = string.Format("-r \"{0}\" \"{1}\"", _ftbModpackArchive, "minecraft");
            }
            else
            {
                Environment.CurrentDirectory = _outputDirectory;
                _startInfo.Arguments = string.Format("a -y \"{0}\" \"{1}\"", _ftbModpackArchive, "minecraft");
            }

            _process.StartInfo = _startInfo;
            _process.Start();
            _process.WaitForExit();
            Directory.Delete(tempModDirectory, true);

            if (mod.HasBeenWritenToModlist)
                return;
            File.AppendAllText(_modlistTextFile, mod.Name + Environment.NewLine);
            mod.HasBeenWritenToModlist = true;
        }
    }
}

