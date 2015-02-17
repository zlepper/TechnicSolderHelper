using System;
using System.IO;
using System.Windows.Forms;
using TechnicSolderHelper.SQL;

namespace TechnicSolderHelper
{
    public partial class SolderHelper
    {
        private void CreateOwnPermissionInfo(String modname, String modid, String modauthor, String linkToPermission, String modLink)
        {
            String output = String.Format("{0}({1}) by {2} {3}Permission: {4} {3}Link to mod: {5}{3}{3}", modname, modid, modauthor, Environment.NewLine, linkToPermission, modLink);
            File.AppendAllText(_ftbOwnPermissionList, output);
        }

        private void CreateTableRow(String firstColumn, String secondColumn, String thirdColumn)
        {
            String addedMod = "<tr>";
            addedMod += String.Format("<td><input readonly class=\"containsInfo\" value=\"{0}\"></td>", firstColumn);
            addedMod += String.Format("<td><input readonly class=\"containsInfo\" value=\"{0}\"></td>", secondColumn);
            addedMod += String.Format("<td><input readonly class=\"containsInfo\" value=\"{0}\"></td>", thirdColumn);
            addedMod += "<td><button class=\"Hide\" type=\"button\">Hide</button></td></tr>";
            File.AppendAllText(_path, addedMod + Environment.NewLine);
        }

        private void CreateConfigZip()
        {
            if (SolderPack.Checked)
            {
                String inputDirectory = InputFolder.Text;
                inputDirectory = inputDirectory.Replace(Globalfunctions.PathSeperator + "mods", "");

                _outputDirectory = OutputFolder.Text;
                String configFileName;
                if (_modpackName == null)
                    configFileName =
                        MakeUrlFriendly(
                        Prompt.ShowDialog(
                            "What do you want the file name of the config " + Environment.NewLine + "folder to be?",
                            "Config FileInfo Name"));
                else
                    configFileName = MakeUrlFriendly(_modpackName) + "-configs";
                var configVersion = _modpackVersion ?? Prompt.ShowDialog("What is the config version?", "Config Version");
                String configFileZipName = configFileName + "-" + configVersion;
                if (!(configFileZipName.EndsWith(".zip")))
                {
                    configFileZipName = configFileZipName.ToLower().Replace(" ", "-") + ".zip";
                }
                if (Globalfunctions.IsUnix())
                {
                    _startInfo.FileName = "zip";
                    Directory.CreateDirectory(_outputDirectory + "/mods/" + configFileName.ToLower());
                    Environment.CurrentDirectory = inputDirectory;
                    _startInfo.Arguments = string.Format("-r \"{0}/mods/{1}/{2}\" \"config\" -x config/YAMPST.nbt", _outputDirectory, configFileName.ToLower(), configFileZipName.ToLower());
                }
                else
                {
                    _startInfo.Arguments = string.Format("a -x!config\\YAMPST.nbt -y \"{0}\\mods\\{1}\\{2}\" \"{3}\\config" + "\"", _outputDirectory, configFileName.ToLower(), configFileZipName.ToLower(), inputDirectory);
                }
                _process.StartInfo = _startInfo;
                _process.Start();

                CreateTableRow(configFileName, configFileName.ToLower(), configVersion.ToLower());

                _process.WaitForExit();

                if (!useSolder.Checked)
                    return;
                int id = _solderSqlHandler.GetModId(configFileName.ToLower());
                if (id == -1)
                {
                    _solderSqlHandler.AddModToSolder(configFileName.ToLower(), null, null, null, configFileName);
                    id = _solderSqlHandler.GetModId(configFileName.ToLower());
                }
                String outputFile = Path.Combine(_outputDirectory, "mods", configFileName.ToLower(), configFileZipName.ToLower());
                _solderSqlHandler.AddNewModversionToSolder(id, _modpackVersion, SqlHelper.CalculateMd5(outputFile).ToLower());

                int modVersionId = _solderSqlHandler.GetModversionId(_solderSqlHandler.GetModId(configFileName.ToLower()), _modpackVersion);
                _solderSqlHandler.AddModversionToBuild(_buildId, modVersionId);
            }
            else
            {
                if (Globalfunctions.IsUnix())
                {
                    Environment.CurrentDirectory = _inputDirectory.Remove(_inputDirectory.LastIndexOf(Globalfunctions.PathSeperator));
                    _startInfo.FileName = "zip";
                    _startInfo.Arguments = String.Format("-r \"{0}\" \"config\" -x config/YAMPST.nbt", _modpackArchive);
                }
                else
                {
                    var input = _inputDirectory.Replace("\\mods", "\\config");
                    _startInfo.Arguments = "a -x!config\\YAMPST.nbt -y \"" + _modpackArchive + "\" \"" + input + "\"";
                }
                _process.StartInfo = _startInfo;
                _process.Start();
                _process.WaitForExit();
            }


        }

        private void CreateTechnicPermissionInfo(Mcmod mod, PermissionLevel pl, String customPermissionText = null)
        {
            String modlink = _ftbPermsSqLhelper.GetInfoFromModId(mod.Modid, FtbPermissionsSqlHelper.InfoType.ModLink);
            while (String.IsNullOrWhiteSpace(modlink) || !Uri.IsWellFormedUriString(modlink, UriKind.Absolute))
            {
                modlink = Prompt.ShowDialog("What is the link to " + mod.Name + "?", "Mod link", false, Prompt.ModsLeftString(_totalMods, _currentMod));
                if (!Uri.IsWellFormedUriString(modlink, UriKind.Absolute))
                {
                    MessageBox.Show("Not a proper url");
                }
            }
            CreateTechnicPermissionInfo(mod, pl, customPermissionText, modlink);
        }

        private void CreateTechnicPermissionInfo(Mcmod mod, PermissionLevel pl, String customPermissionText, String modlink)
        {
            String ps = String.Format("{0}({1}) by {2}{3}At {4}{3}Permissions are {5}{3}", mod.Name, mod.Modid, GetAuthors(mod), Environment.NewLine, modlink, pl);
            if (!String.IsNullOrWhiteSpace(customPermissionText))
            {
                ps += customPermissionText + Environment.NewLine;
            }
            File.AppendAllText(_technicPermissionList, ps + Environment.NewLine);
        }

        public void CreateTechnicModZip(Mcmod mod, String modfile)
        {
            if (mod.IsSkipping)
            {
                return;
            }
            String fileName = modfile.Substring(modfile.LastIndexOf(Globalfunctions.PathSeperator) + 1);
            String modMd5 = SqlHelper.CalculateMd5(modfile);
            _modsSqLhelper.AddMod(mod.Name, mod.Modid, mod.Version, mod.Mcversion, fileName, modMd5, false);
            if (TechnicPermissions.Checked)
            {
                PermissionLevel permissionLevel = _ftbPermsSqLhelper.DoFtbHavePermission(mod.Modid, TechnicPublicPermissions.Checked);
                String overwritelink;
                OwnPermissions ownPermissions;
                String customPermissionText;
                switch (permissionLevel)
                {
                    case PermissionLevel.Open:
                        CreateTechnicPermissionInfo(mod, permissionLevel);
                        break;
                    case PermissionLevel.Notify:
                        ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                        if (!ownPermissions.HasPermission)
                        {
                            overwritelink = Prompt.ShowDialog(mod.Name + " requires that you notify the author of inclusion." + Environment.NewLine + "Please provide proof(an imgur link) that you have done this:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod));
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
                                        customPermissionText = "Proof of notitification: " + overwritelink;
                                        CreateTechnicPermissionInfo(mod, permissionLevel, customPermissionText, _ftbPermsSqLhelper.GetInfoFromModId(mod.Modid, FtbPermissionsSqlHelper.InfoType.ModLink));
                                        break;
                                    }
                                }
                                overwritelink = Prompt.ShowDialog(mod.Name + " requires that you notify the author of inclusion." + Environment.NewLine + "Please provide proof(an imgur link) that you have done this:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod));
                            }
                        }
                        else
                        {
                            customPermissionText = "Proof of notitification: " + ownPermissions.PermissionLink;
                            CreateTechnicPermissionInfo(mod, permissionLevel, customPermissionText);
                        }
                        break;
                    case PermissionLevel.Ftb:
                        ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                        if (!ownPermissions.HasPermission)
                        {
                            overwritelink = Prompt.ShowDialog("Permissions for " + mod.Name + " is FTB exclusive" + Environment.NewLine + "Please provide proof(an imgur link) of things being otherwise:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod));
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
                                        _ownPermsSqLhelper.AddOwnModPerm(mod.Name, mod.Modid, overwritelink, _ftbPermsSqLhelper.GetInfoFromModId(mod.Modid, FtbPermissionsSqlHelper.InfoType.ModLink));
                                        break;
                                    }
                                }
                                overwritelink = Prompt.ShowDialog("Permissions for " + mod.Name + " is FTB exclusive" + Environment.NewLine + "Please provide proof(an imgur link) of things being otherwise:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod));
                            }
                        }
                        ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                        customPermissionText = "Proof of permission outside of FTB: " + ownPermissions.PermissionLink;
                        CreateTechnicPermissionInfo(mod, permissionLevel, customPermissionText, ownPermissions.ModLink);
                        break;
                    case PermissionLevel.Request:
                        ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                        if (!ownPermissions.HasPermission)
                        {
                            overwritelink = Prompt.ShowDialog("This mod requires that you request permissions from the Mod Author of " + mod.Name + Environment.NewLine + "Please provide proof(an imgur link) that you have this permission:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name);
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
                                        _ownPermsSqLhelper.AddOwnModPerm(mod.Name, mod.Modid, overwritelink, _ftbPermsSqLhelper.GetInfoFromModId(mod.Modid, FtbPermissionsSqlHelper.InfoType.ModLink));
                                        break;
                                    }
                                }
                                overwritelink = Prompt.ShowDialog("This mod requires that you request permissions from the Mod Author of " + mod.Name + Environment.NewLine + "Please provide proof(an imgur link) that you have this permission:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod));
                            }
                        }
                        ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                        customPermissionText = GetAuthors(mod) + " has given permission as seen here: " + ownPermissions.PermissionLink;
                        CreateTechnicPermissionInfo(mod, permissionLevel, customPermissionText, ownPermissions.ModLink);
                        break;
                    case PermissionLevel.Closed:
                        ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                        if (!ownPermissions.HasPermission)
                        {
                            overwritelink = Prompt.ShowDialog("The FTB permissionsheet states that permissions for " + mod.Name + " is closed." + Environment.NewLine + "Please provide proof(an imgur link) that this is not the case:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod));
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
                                        _ownPermsSqLhelper.AddOwnModPerm(mod.Name, mod.Modid, overwritelink, _ftbPermsSqLhelper.GetInfoFromModId(mod.Modid, FtbPermissionsSqlHelper.InfoType.ModLink));
                                        break;
                                    }
                                }
                                overwritelink = Prompt.ShowDialog("The FTB permissionsheet states that permissions for " + mod.Name + " is closed." + Environment.NewLine + "Please provide proof(an imgur link) that this is not the case:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name);
                            }
                        }
                        ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                        customPermissionText = GetAuthors(mod) + " has given permission as seen here: " + ownPermissions.PermissionLink;
                        CreateTechnicPermissionInfo(mod, permissionLevel, customPermissionText, ownPermissions.ModLink);
                        break;
                    case PermissionLevel.Unknown:
                        ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                        var modLink = ownPermissions.ModLink;
                        if (!ownPermissions.HasPermission)
                        {
                            overwritelink = Prompt.ShowDialog("Permissions for " + mod.Name + " is unknown" + Environment.NewLine + "Please provide proof(an imgur link) of permissions:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod));
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
                                }
                                overwritelink = Prompt.ShowDialog("Permissions for " + mod.Name + " is unknown" + Environment.NewLine + "Please provide proof(an imgur link) of permissions:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod));
                            }
                            while (String.IsNullOrWhiteSpace(modLink))
                            {
                                if (modLink != null && modLink.ToLower().Equals("skip".ToLower()))
                                {
                                    mod.IsSkipping = true;
                                    return;
                                }
                                if (modLink != null && Uri.IsWellFormedUriString(modLink, UriKind.Absolute))
                                {
                                    _ownPermsSqLhelper.AddOwnModPerm(mod.Name, mod.Modid, overwritelink, modLink);
                                    break;

                                }
                                modLink = Prompt.ShowDialog("Please provide a link to " + mod.Name + ":" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod));
                            }
                            String a = GetAuthors(mod);
                            _ownPermsSqLhelper.AddOwnModPerm(mod.Name, mod.Modid, overwritelink, modLink);
                            CreateOwnPermissionInfo(mod.Name, mod.Modid, a, overwritelink, modLink);

                        }
                        ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                        customPermissionText = GetAuthors(mod) + " has given permission as seen here: " + ownPermissions.PermissionLink;
                        CreateTechnicPermissionInfo(mod, permissionLevel, customPermissionText, ownPermissions.ModLink);
                        break;
                }
            }
            if (SolderPack.Checked)
            {
                var modid = mod.Modid.Contains("|") ? mod.Modid.Remove(mod.Modid.LastIndexOf("|", StringComparison.Ordinal)).Replace(".", String.Empty).ToLower() : mod.Modid.Replace(".", string.Empty).ToLower();
                if (useSolder.Checked)
                {
                    if (_solderSqlHandler.IsModversionOnline(modid, mod.Mcversion.ToLower() + "-" + mod.Version.ToLower()))
                    {
                        int id = _solderSqlHandler.GetModId(modid);
                        int modVersionId = _solderSqlHandler.GetModversionId(id, mod.Mcversion.ToLower() + "-" + mod.Version.ToLower());
                        _solderSqlHandler.AddModversionToBuild(_buildId, modVersionId);
                    }
                }
                if (!_modsSqLhelper.IsFileInSolder(modfile))
                {
                    var modDir = Path.Combine(_outputDirectory, "mods", mod.Modid.Contains("|") ? mod.Modid.Remove(mod.Modid.LastIndexOf("|", StringComparison.Ordinal)).Replace(".", string.Empty).ToLower().Replace(Globalfunctions.PathSeperator.ToString(), String.Empty) : mod.Modid.Replace(".", string.Empty).ToLower().Replace(Globalfunctions.PathSeperator.ToString(), String.Empty), "mods");
                    Directory.CreateDirectory(modDir);

                    String tempModFile = Path.Combine(modDir, fileName);

                    String tempFileDirectory = tempModFile.Remove(tempModFile.LastIndexOf(Globalfunctions.PathSeperator));

                    Directory.CreateDirectory(tempFileDirectory);
                    File.Copy(modfile, tempModFile, true);

                    var modArchive = mod.Modid.Contains("|") ? Path.Combine(_outputDirectory, "mods", mod.Modid.Remove(mod.Modid.LastIndexOf("|", StringComparison.Ordinal)).Replace(".", string.Empty).ToLower(), mod.Modid.Remove(mod.Modid.LastIndexOf("|", StringComparison.Ordinal)).Replace(".", string.Empty).ToLower() + "-" + mod.Mcversion.ToLower() + "-" + mod.Version.ToLower() + ".zip") : Path.Combine(_outputDirectory, "mods", mod.Modid.Replace(".", string.Empty).ToLower(), mod.Modid.Replace(".", string.Empty).ToLower() + "-" + mod.Mcversion.ToLower() + "-" + mod.Version.ToLower() + ".zip");
                    if (Globalfunctions.IsUnix())
                    {
                        Environment.CurrentDirectory = Path.Combine(_outputDirectory, "mods", mod.Modid.Contains("|") ? mod.Modid.Remove(mod.Modid.LastIndexOf("|", StringComparison.Ordinal)).Replace(".", string.Empty).ToLower() : mod.Modid.Replace(".", string.Empty).ToLower());
                        modDir = "mods";
                        _startInfo.FileName = "zip";
                        _startInfo.Arguments = "-r \"" + modArchive + "\" \"" + modDir + "\" ";
                    }
                    else
                    {
                        _startInfo.Arguments = "a -y \"" + modArchive + "\" \"" + modDir + "\" ";
                    }
                    _process.StartInfo = _startInfo;
                    _process.Start();

                    //Save mod to database
                    _modsSqLhelper.AddMod(mod.Name, mod.Modid, mod.Version, mod.Mcversion, fileName, modMd5, true);

                    // Add mod info to a html file
                    CreateTableRow(mod.Name.Replace("|", ""), modid, mod.Mcversion.ToLower() + "-" + mod.Version.ToLower());

                    _process.WaitForExit();

                    if (useSolder.Checked)
                    {
                        string archive = Path.Combine(_outputDirectory, "mods", modArchive);
                        if (_solderSqlHandler.IsModversionOnline(modid,
                            mod.Mcversion.ToLower() + "-" + mod.Version.ToLower()))
                        {
                            _solderSqlHandler.UpdateModversionMd5(modid, mod.Mcversion.ToLower() + "-" + mod.Version.ToLower(), SqlHelper.CalculateMd5(archive).ToLower());
                        }
                        else
                        {
                            int id = _solderSqlHandler.GetModId(modid);
                            if (id == -1)
                            {
                                _solderSqlHandler.AddModToSolder(modid, mod.Description, GetAuthors(mod), mod.Url,
                                    mod.Name);
                                // ReSharper disable once RedundantAssignment
                                id = _solderSqlHandler.GetModId(modid);
                            }
                            _solderSqlHandler.AddNewModversionToSolder(modid,
                                mod.Mcversion.ToLower() + "-" + mod.Version.ToLower(),
                                SqlHelper.CalculateMd5(archive).ToLower());

                            id = _solderSqlHandler.GetModId(modid);
                            int modVersionId = _solderSqlHandler.GetModversionId(id,
                                mod.Mcversion.ToLower() + "-" + mod.Version.ToLower());
                            _solderSqlHandler.AddModversionToBuild(_buildId, modVersionId);
                        }
                    }

                    Directory.Delete(modDir, true);
                }
            }
            else
            {
                _modsSqLhelper.AddMod(mod.Name, mod.Modid, mod.Version, mod.Mcversion, fileName, modMd5, false);
                while (String.IsNullOrWhiteSpace(_modpackName))
                {
                    _modpackName = Prompt.ShowDialog("What is the Modpack Name?", "Modpack Name");
                }
                while (String.IsNullOrWhiteSpace(_modpackVersion))
                {
                    _modpackVersion = Prompt.ShowDialog("What Version is the modpack?", "Modpack Version");
                }

                String tempDirectory = Path.Combine(_outputDirectory, "tmp");
                String tempModDirectory = Path.Combine(tempDirectory, "mods");
                Directory.CreateDirectory(tempModDirectory);
                String tempFile = Path.Combine(tempModDirectory, fileName);

                int index = tempFile.LastIndexOf(Globalfunctions.PathSeperator);

                String tempFileDirectory = tempFile.Remove(index);
                Directory.CreateDirectory(tempFileDirectory);
                File.Copy(modfile, tempFile, true);

                _modpackArchive = Path.Combine(_outputDirectory, String.Format("{0}-{1}.zip", _modpackName, _modpackVersion));

                if (Globalfunctions.IsUnix())
                {
                    Environment.CurrentDirectory = tempDirectory;
                    _startInfo.Arguments = String.Format("-r \"{0}\" \"{1}\"", _modpackArchive, "mods");
                }
                else
                {
                    _startInfo.Arguments = String.Format("a -y \"{0}\" \"{1}\"", _modpackArchive, tempModDirectory);
                }
                if (Globalfunctions.IsUnix())
                {
                    _startInfo.FileName = "zip";
                }
                _process.StartInfo = _startInfo;
                _process.Start();
                _process.WaitForExit();
                Directory.Delete(tempDirectory, true);
            }

            if (mod.HasBeenWritenToModlist)
                return;
            File.AppendAllText(_modlistTextFile, mod.Name + Environment.NewLine);
            mod.HasBeenWritenToModlist = true;
        }
    }
}

