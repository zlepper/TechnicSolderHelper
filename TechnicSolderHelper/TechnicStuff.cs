using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TechnicSolderHelper.SQL;

namespace TechnicSolderHelper
{
    public partial class SolderHelper
    {
        public StringBuilder AddedModStringBuilder = new StringBuilder();
        private readonly Dictionary<string, int> _processesUsingModID = new Dictionary<string, int>();

        private void CreateOwnPermissionInfo(string modname, string modid, string modauthor, string linkToPermission, string modLink)
        {
            string output = string.Format("{0}({1}) by {2} {3}Permission: {4} {3}Link to mod: {5}{3}{3}", modname, modid, modauthor, Environment.NewLine, linkToPermission, modLink);
            File.AppendAllText(_ftbOwnPermissionList, output);
        }

        private void CreateTableRow(string firstColumn, string secondColumn, string thirdColumn)
        {
            string addedMod = "<tr>";
            addedMod += string.Format("<td><input readonly class=\"containsInfo\" value=\"{0}\"></td>", firstColumn);
            addedMod += string.Format("<td><input readonly class=\"containsInfo\" value=\"{0}\"></td>", secondColumn);
            addedMod += string.Format("<td><input readonly class=\"containsInfo\" value=\"{0}\"></td>", thirdColumn);
            addedMod += "<td><button class=\"Hide\" type=\"button\">Hide</button></td></tr>" + Environment.NewLine;
            AddedModStringBuilder.Append(addedMod);
            //File.AppendAllText(_path, addedMod + Environment.NewLine);
        }

        private void CreateConfigZip()
        {
            if (SolderPack.Checked)
            {
                string inputDirectory = InputFolder.Text;
                inputDirectory = inputDirectory.Replace(Globalfunctions.PathSeperator + "mods", "");

                _outputDirectory = OutputFolder.Text;
                string configFileName;
                if (_modpackName == null)
                    configFileName =
                        MakeUrlFriendly(
                        Prompt.ShowDialog(
                            "What do you want the file name of the config " + Environment.NewLine + "folder to be?",
                            "Config FileInfo Name"));
                else
                    configFileName = MakeUrlFriendly(_modpackName) + "-configs";
                var configVersion = _modpackVersion ?? Prompt.ShowDialog("What is the config version?", "Config Version");
                string configFileZipName = configFileName + "-" + configVersion;
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
                string outputFile = Path.Combine(_outputDirectory, "mods", configFileName.ToLower(), configFileZipName.ToLower());
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
                    _startInfo.Arguments = string.Format("-r \"{0}\" \"config\" -x config/YAMPST.nbt", _modpackArchive);
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

        private void CreateTechnicPermissionInfo(Mcmod mod, PermissionPolicy pl, string customPermissionText = null)
        {
            string modlink = _ftbPermsSqLhelper.GetPermissionFromModId(mod.Modid).modAuthors;
            while (string.IsNullOrWhiteSpace(modlink) || !Uri.IsWellFormedUriString(modlink, UriKind.Absolute))
            {
                modlink = Prompt.ShowDialog("What is the link to " + mod.Name + "?", "Mod link", false, Prompt.ModsLeftString(_totalMods, _currentMod));
                if (!Uri.IsWellFormedUriString(modlink, UriKind.Absolute))
                {
                    MessageBox.Show("Not a proper url");
                }
            }
            CreateTechnicPermissionInfo(mod, pl, customPermissionText, modlink);
        }

        private void CreateTechnicPermissionInfo(Mcmod mod, PermissionPolicy pl, string customPermissionText, string modlink)
        {
            string ps = string.Format("{0}({1}) by {2}{3}At {4}{3}Permissions are {5}{3}", mod.Name, mod.Modid, GetAuthors(mod), Environment.NewLine, modlink, pl);
            if (!string.IsNullOrWhiteSpace(customPermissionText))
            {
                ps += customPermissionText + Environment.NewLine;
            }
            File.AppendAllText(_technicPermissionList, ps + Environment.NewLine);
        }

        public void CreateTechnicModZip(Mcmod mod, string modfile)
        {
            if (mod.IsSkipping)
            {
                return;
            }
            string fileName = modfile.Substring(modfile.LastIndexOf(Globalfunctions.PathSeperator) + 1);
            string modMd5 = SqlHelper.CalculateMd5(modfile);
            _modsSqLhelper.AddMod(mod.Name, mod.Modid, mod.Version, mod.Mcversion, fileName, modMd5, false);
            # region permissions
            if (TechnicPermissions.Checked)
            {
                PermissionPolicy permissionPolicy = _ftbPermsSqLhelper.FindPermissionPolicy(mod.Modid, TechnicPublicPermissions.Checked);
                string overwritelink;
                OwnPermissions ownPermissions;
                string customPermissionText;
                switch (permissionPolicy)
                {
                    case PermissionPolicy.Open:
                        CreateTechnicPermissionInfo(mod, permissionPolicy);
                        break;
                    case PermissionPolicy.Notify:
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
                                        CreateTechnicPermissionInfo(mod, permissionPolicy, customPermissionText, _ftbPermsSqLhelper.GetPermissionFromModId(mod.Modid).modLink);
                                        break;
                                    }
                                }
                                overwritelink = Prompt.ShowDialog(mod.Name + " requires that you notify the author of inclusion." + Environment.NewLine + "Please provide proof(an imgur link) that you have done this:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod));
                            }
                        }
                        else
                        {
                            customPermissionText = "Proof of notitification: " + ownPermissions.PermissionLink;
                            CreateTechnicPermissionInfo(mod, permissionPolicy, customPermissionText);
                        }
                        break;
                    case PermissionPolicy.FTB:
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
                                        _ownPermsSqLhelper.AddOwnModPerm(mod.Name, mod.Modid, overwritelink, _ftbPermsSqLhelper.GetPermissionFromModId(mod.Modid).modLink);
                                        break;
                                    }
                                }
                                overwritelink = Prompt.ShowDialog("Permissions for " + mod.Name + " is FTB exclusive" + Environment.NewLine + "Please provide proof(an imgur link) of things being otherwise:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod));
                            }
                        }
                        ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                        customPermissionText = "Proof of permission outside of FTB: " + ownPermissions.PermissionLink;
                        CreateTechnicPermissionInfo(mod, permissionPolicy, customPermissionText, ownPermissions.ModLink);
                        break;
                    case PermissionPolicy.Request:
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
                                        _ownPermsSqLhelper.AddOwnModPerm(mod.Name, mod.Modid, overwritelink, _ftbPermsSqLhelper.GetPermissionFromModId(mod.Modid).modLink);
                                        break;
                                    }
                                }
                                overwritelink = Prompt.ShowDialog("This mod requires that you request permissions from the Mod Author of " + mod.Name + Environment.NewLine + "Please provide proof(an imgur link) that you have this permission:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name, true, Prompt.ModsLeftString(_totalMods, _currentMod));
                            }
                        }
                        ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                        customPermissionText = GetAuthors(mod) + " has given permission as seen here: " + ownPermissions.PermissionLink;
                        CreateTechnicPermissionInfo(mod, permissionPolicy, customPermissionText, ownPermissions.ModLink);
                        break;
                    case PermissionPolicy.Closed:
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
                                        _ownPermsSqLhelper.AddOwnModPerm(mod.Name, mod.Modid, overwritelink, _ftbPermsSqLhelper.GetPermissionFromModId(mod.Modid).modLink);
                                        break;
                                    }
                                }
                                overwritelink = Prompt.ShowDialog("The FTB permissionsheet states that permissions for " + mod.Name + " is closed." + Environment.NewLine + "Please provide proof(an imgur link) that this is not the case:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.Name);
                            }
                        }
                        ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                        customPermissionText = GetAuthors(mod) + " has given permission as seen here: " + ownPermissions.PermissionLink;
                        CreateTechnicPermissionInfo(mod, permissionPolicy, customPermissionText, ownPermissions.ModLink);
                        break;
                    case PermissionPolicy.Unknown:
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
                            while (string.IsNullOrWhiteSpace(modLink))
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
                            string a = GetAuthors(mod);
                            _ownPermsSqLhelper.AddOwnModPerm(mod.Name, mod.Modid, overwritelink, modLink);
                            CreateOwnPermissionInfo(mod.Name, mod.Modid, a, overwritelink, modLink);

                        }
                        ownPermissions = _ownPermsSqLhelper.DoUserHavePermission(mod.Modid);
                        customPermissionText = GetAuthors(mod) + " has given permission as seen here: " + ownPermissions.PermissionLink;
                        CreateTechnicPermissionInfo(mod, permissionPolicy, customPermissionText, ownPermissions.ModLink);
                        break;
                }
            }
            # endregion 
            if (SolderPack.Checked)
            {
                bool force = forcesolder.Checked;
                bool useSolderBool = useSolder.Checked;
                BackgroundWorker bw = new BackgroundWorker();
                _runningProcess++;
                bw.DoWork += (o, arg) =>
                {

                    var modid = mod.Modid.Contains("|")
                        ? mod.Modid.Replace("|", string.Empty)
                            .Replace(".", string.Empty)
                            .ToLower()
                        : mod.Modid.Replace(".", string.Empty).ToLower();
                    string modversion = mod.Mcversion.ToLower() + "-" + mod.Version.ToLower();
                    if (useSolderBool && !force)
                    {
                        if (_solderSqlHandler.IsModversionOnline(modid, modversion))
                        {
                            Debug.WriteLine(modid + " is already online with version " + modversion);
                            int id = _solderSqlHandler.GetModId(modid);
                            int modVersionId = _solderSqlHandler.GetModversionId(id, modversion);
                            _solderSqlHandler.AddModversionToBuild(_buildId, modVersionId);
                            _runningProcess--;
                            return;
                        }
                    }

                    while (true)
                    {
                        if (_processesUsingModID.ContainsKey(mod.Modid))
                        {
                            Debug.WriteLine("Sleeping with id: " + mod.Modid);
                            Thread.Sleep(100);
                        }
                        else
                        {
                            _processesUsingModID.Add(mod.Modid, 1);
                            break;
                        }
                    }
                    
                    if (!_modsSqLhelper.IsFileInSolder(modfile) || force)
                    {
                        var modDir = Path.Combine(_outputDirectory, "mods",
                            mod.Modid.Contains("|")
                                ? mod.Modid.Replace("|", string.Empty)
                                    .Replace(".", string.Empty)
                                    .ToLower()
                                    .Replace(Globalfunctions.PathSeperator.ToString(), string.Empty)
                                : mod.Modid.Replace(".", string.Empty)
                                    .ToLower()
                                    .Replace(Globalfunctions.PathSeperator.ToString(), string.Empty), "mods");
                        Directory.CreateDirectory(modDir);
                        if (_processesUsingFolder.ContainsKey(modDir))
                        {
                            _processesUsingFolder[modDir]++;
                        }
                        else
                        {
                            _processesUsingFolder.Add(modDir, 1);
                        }
                        string tempModFile = Path.Combine(modDir, fileName);

                        string tempFileDirectory =
                            tempModFile.Remove(tempModFile.LastIndexOf(Globalfunctions.PathSeperator));

                        Directory.CreateDirectory(tempFileDirectory);
                        File.Copy(modfile, tempModFile, true);

                        var modArchive = mod.Modid.Contains("|")
                            ? Path.Combine(_outputDirectory, "mods",
                                mod.Modid.Replace("|", string.Empty)
                                    .Replace(".", string.Empty)
                                    .ToLower(),
                                mod.Modid.Replace("|", string.Empty)
                                    .Replace(".", string.Empty)
                                    .ToLower() + "-" + mod.Mcversion.ToLower() + "-" + mod.Version.ToLower() + ".zip")
                            : Path.Combine(_outputDirectory, "mods", mod.Modid.Replace(".", string.Empty).ToLower(),
                                mod.Modid.Replace(".", string.Empty).ToLower() + "-" + mod.Mcversion.ToLower() + "-" +
                                mod.Version.ToLower() + ".zip");
                        if (Globalfunctions.IsUnix())
                        {
                            Environment.CurrentDirectory = Path.Combine(_outputDirectory, "mods",
                                mod.Modid.Contains("|")
                                    ? mod.Modid.Replace("|", string.Empty)
                                        .Replace(".", string.Empty)
                                        .ToLower()
                                    : mod.Modid.Replace(".", string.Empty).ToLower());
                            _startInfo.FileName = "zip";
                            _startInfo.Arguments = "-r \"" + modArchive + "\" \"mods\" ";
                        }
                        else
                        {
                            _startInfo.Arguments = "a -y \"" + modArchive + "\" \"" + modDir + "\" ";
                        }
                        Process process = new Process { StartInfo = _startInfo };

                        process.Start();

                        //Save mod to database
                        _modsSqLhelper.AddMod(mod.Name, mod.Modid, mod.Version, mod.Mcversion, fileName, modMd5, true);

                        // Add mod info to a html file
                        CreateTableRow(mod.Name.Replace("|", string.Empty), modid, modversion);

                        process.WaitForExit();

                        if (useSolderBool)
                        {
                            int id = -1;
                            string archive = Path.Combine(_outputDirectory, "mods", modArchive);
                            SolderSqlHandler sqh = new SolderSqlHandler();
                            string md5Value = SqlHelper.CalculateMd5(archive).ToLower();
                            if (sqh.IsModversionOnline(modid, modversion))
                            {
                                Debug.WriteLine(string.Format("Updating mod on solder with Modid: {0} modversion: {1} md5value: {2}", modid, modversion, md5Value), doDebug.Checked);
                                sqh.UpdateModversionMd5(modid, modversion, md5Value);
								Debug.WriteLine(string.Format("Done updating mod on solder with modid: {0}", modid));
                            }
                            else
                            {
                                id = sqh.GetModId(modid);
                                if (id == -1)
                                {
                                    sqh.AddModToSolder(modid, mod.Description, GetAuthors(mod), mod.Url,
                                        mod.Name);
                                }
                                Debug.WriteLine(string.Format("Adding mod to solder with Modid: {0} modversion: {1} md5value: {2}", modid, modversion, md5Value), doDebug.Checked);
                                sqh.AddNewModversionToSolder(modid, modversion, md5Value);
                            }
                            id = sqh.GetModId(modid);
                            int modVersionId = sqh.GetModversionId(id, modversion);
                            sqh.AddModversionToBuild(_buildId, modVersionId);
							Debug.WriteLine(string.Format("Done addong mod {0} to build", modid));
                        }
						Debug.WriteLine("Decrementing " + modDir);
                        _processesUsingFolder[modDir]--;
						Debug.WriteLine("Decremented " + modDir);
                        if (Directory.Exists(modDir) && _processesUsingFolder[modDir] == 0)
                        {
                            while (true)
                            {
                                try
                                {
									Debug.WriteLine("Attempting to delete directory " + modDir);
                                    Directory.Delete(modDir, true);
									Debug.WriteLine("Done deleting directory " + modDir);
                                    break;
                                }
                                catch (IOException e)
                                {
                                    Debug.WriteLine(e.Message);
                                    Thread.Sleep(100);
                                }
                            }
                            _processesUsingFolder.Remove(modDir);
                        }
                    }
                    if (_processesUsingModID.ContainsKey(mod.Modid))
                        if (_processesUsingModID[mod.Modid] > 1)
                        {
                            _processesUsingModID[mod.Modid]--;
                        }
                        else
                        {
                            _processesUsingModID.Remove(mod.Modid);
                        }
                    _runningProcess--;
					Debug.WriteLine("Decremented _runningProcess");
                };
                bw.RunWorkerAsync();
            }
            else
            {
                _modsSqLhelper.AddMod(mod.Name, mod.Modid, mod.Version, mod.Mcversion, fileName, modMd5, false);
                while (string.IsNullOrWhiteSpace(_modpackName))
                {
                    _modpackName = Prompt.ShowDialog("What is the Modpack Name?", "Modpack Name");
                }
                while (string.IsNullOrWhiteSpace(_modpackVersion))
                {
                    _modpackVersion = Prompt.ShowDialog("What Version is the modpack?", "Modpack Version");
                }

                string tempDirectory = Path.Combine(_outputDirectory, "tmp");
                string tempModDirectory = Path.Combine(tempDirectory, "mods");
                Directory.CreateDirectory(tempModDirectory);
                string tempFile = Path.Combine(tempModDirectory, fileName);

                int index = tempFile.LastIndexOf(Globalfunctions.PathSeperator);

                string tempFileDirectory = tempFile.Remove(index);
                Directory.CreateDirectory(tempFileDirectory);
                File.Copy(modfile, tempFile, true);

                _modpackArchive = Path.Combine(_outputDirectory, string.Format("{0}-{1}.zip", _modpackName, _modpackVersion));

                if (Globalfunctions.IsUnix())
                {
                    Environment.CurrentDirectory = tempDirectory;
                    _startInfo.Arguments = string.Format("-r \"{0}\" \"{1}\"", _modpackArchive, "mods");
                }
                else
                {
                    _startInfo.Arguments = string.Format("a -y \"{0}\" \"{1}\"", _modpackArchive, tempModDirectory);
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

