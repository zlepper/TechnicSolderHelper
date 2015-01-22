using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using TechnicSolderHelper.SQL;
using TechnicSolderHelper.Confighandler;
using TechnicSolderHelper.SQL.forge;
using TechnicSolderHelper.FileUpload;
using TechnicSolderHelper.s3;
using FileInfo = System.IO.FileInfo;
using TechnicSolderHelper.SQL.liteloader;
using TechnicSolderHelper.SQL.workTogether;

namespace TechnicSolderHelper
{
    public partial class SolderHelper
    {
        #region Application Wide Variables

        private String _inputDirectory;
        private String _outputDirectory;
        private readonly ModListSqlHelper _modsSqLhelper = new ModListSqlHelper();
        private readonly FtbPermissionsSqlHelper _ftbPermsSqLhelper = new FtbPermissionsSqlHelper();
        private readonly OwnPermissionsSqlHelper _ownPermsSqLhelper = new OwnPermissionsSqlHelper();
        private readonly ForgeSqlHelper _forgeSqlHelper = new ForgeSqlHelper();
        private readonly LiteloaderSqlHelper _liteloaderSqlHelper = new LiteloaderSqlHelper();
        private SolderSqlHandler _solderSqlHandler = new SolderSqlHandler();
        private readonly String _sevenZipLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "7za.exe");
        private readonly Process _process = new Process();
        private readonly ProcessStartInfo _startInfo = new ProcessStartInfo();
        public String _path, _currentMcVersion, _modpackVersion, _modpackName, _modpackArchive, _ftbModpackArchive;
        private readonly ConfigHandler _confighandler = new ConfigHandler();
        private String _modlistTextFile = "", _technicPermissionList = "", _ftbPermissionList = "", _ftbOwnPermissionList = "";
        private short _totalMods, _currentMod;
        private readonly String _modpacksJsonFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "modpacks.json");
        private Modpacks _modpacks = new Modpacks();
        private readonly Dictionary<String, CheckBox> _additionalDirectories = new Dictionary<string, CheckBox>();
        private Ftp _ftp;
        private readonly List<String> _inputDirectories = new List<String>();
        private int _buildId, _modpackId;

        #endregion

        private void form_resize(object sender, EventArgs e)
        {
            if ((Globalfunctions.IsUnix() && Width > 923) || (!Globalfunctions.IsUnix() && Width > 800))
            {
                //Debug.WriteLine(Width - 923 + 159);
                if (Globalfunctions.IsUnix())
                {
                    groupBox1.Width = Width - 923 + 159;
                }
                else
                {
                    groupBox1.Width = Width - 800 + 136;
                }
                Debug.WriteLine(groupBox1.Width);
            }
        }

        public string GetAuthors(Mcmod mod)
        {
            string authorString = "";
            bool isFirst = true;
            if (mod.Authors != null && mod.Authors.Count != 0)
            {
                foreach (string author in mod.Authors)
                {
                    if (isFirst)
                    {
                        authorString = author;
                        isFirst = false;
                    }
                    else
                    {
                        authorString += ", " + author;
                    }
                }
            }
            else
            {
                if (mod.AuthorList != null && mod.AuthorList.Count != 0)
                {
                    foreach (string author in mod.AuthorList)
                    {
                        if (isFirst)
                        {
                            authorString = author;
                            isFirst = false;
                        }
                        else
                        {
                            authorString += ", " + author;
                        }
                    }
                }
                else
                {
                    authorString = _ftbPermsSqLhelper.GetInfoFromModId(mod.Modid, FtbPermissionsSqlHelper.InfoType.ModAuthor);

                    if (String.IsNullOrWhiteSpace(authorString))
                    {
                        authorString = _ownPermsSqLhelper.GetAuthor(mod.Modid);
                        if (String.IsNullOrWhiteSpace(authorString))
                        {
                            authorString = Prompt.ShowDialog("Who is the author of " + mod.Name + "?" + Environment.NewLine + "If you leave this empty the author list in the output will also be empty.", "Mod Author", false, Prompt.ModsLeftString(_totalMods, _currentMod));

                        }
                    }
                }
            }
            _ownPermsSqLhelper.AddAuthor(mod.Modid, authorString);
            return authorString;
        }

        private Boolean Prepare()
        {
            _inputDirectory = InputFolder.Text;
            _outputDirectory = OutputFolder.Text;
            _ftbOwnPermissionList = Path.Combine(_outputDirectory, "Own Permission List.txt");
            _ftbPermissionList = Path.Combine(_outputDirectory, "FTB Permission List.txt");
            _technicPermissionList = Path.Combine(_outputDirectory, "Technic Permission List.txt");
            //_sqlCommandPath = Path.Combine(_outputDirectory, "commands.sql");
            _currentMcVersion = string.IsNullOrEmpty(MCversion.Text) ? null : MCversion.SelectedItem.ToString();

            Environment.CurrentDirectory = Globalfunctions.IsUnix() ? "/" : "C:\\";
            if (!Directory.Exists(InputFolder.Text))
            {
                MessageBox.Show("Input directory does not exist.");
                return true;
            }
            if (!_inputDirectories.Contains(_inputDirectory))
            {
                _inputDirectories.Add(_inputDirectory);
            }
            if (checkBox1.Checked)
            {
                if (Directory.Exists(_outputDirectory))
                {
                    try
                    {
                        Directory.Delete(_outputDirectory, true);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unable to clear solderDirectory." + Environment.NewLine + "Please restart the process when the directory is no longer in use.");
                        return true;
                    }
                }
            }
            if (UploadToFTPServer.Checked)
            {
                var tmp = _confighandler.GetConfig("ftpUrl");
                if (String.IsNullOrWhiteSpace(tmp))
                {
                    MessageBox.Show("You do not have an uploadurl set for your FTP address.");
                    return true;
                }
                tmp = _confighandler.GetConfig("ftpUserName");
                if (String.IsNullOrWhiteSpace(tmp))
                {
                    MessageBox.Show("You do not have an username set for your FTP address.");
                    return true;
                }
                tmp = _confighandler.GetConfig("ftpPassword");
                if (String.IsNullOrWhiteSpace(tmp))
                {
                    MessageBox.Show("You do not have an password set for your FTP address.");
                    return true;
                }

            }

            if (CreateTechnicPack.Checked && IncludeForgeVersion.Checked)
            {
                if (MCversion.SelectedItem == null)
                {
                    MessageBox.Show("You have choosen to include Minecraft Forge, but you haven't selected a Minecraft Version.");
                    return true;
                }
                if (ForgeBuild.SelectedItem == null)
                {
                    MessageBox.Show("You have choosen to include Minecraft Forge, but you haven't selected a Forge build to include.");
                    return true;
                }
            }

            if (File.Exists(_ftbOwnPermissionList))
                File.Delete(_ftbOwnPermissionList);
            if (File.Exists(_ftbPermissionList))
                File.Delete(_ftbPermissionList);
            if (File.Exists(_technicPermissionList))
                File.Delete(_technicPermissionList);

            _modpackName = ModpackNameInput.Text;
            _modpackVersion = null;
            _modpackVersion = ModpackVersionInput.Text;

            if (!String.IsNullOrWhiteSpace(_modpackName))
            {
                _modpacks = new Modpacks();
                if (!ModpackNameInput.Items.Contains(_modpackName))
                {
                    ModpackNameInput.Items.Add(_modpackName);
                }
                foreach (String item in ModpackNameInput.Items)
                {
                    if (_modpacks.Modpack == null)
                    {
                        _modpacks.Modpack = new Dictionary<string, List<string>>();
                    }
                    if (!_modpacks.Modpack.ContainsKey(item))
                    {
                        _modpacks.Modpack.Add(item, null);
                    }
                }
                String tmpJson = JsonConvert.SerializeObject(_modpacks, Formatting.Indented);
                File.WriteAllText(_modpacksJsonFile, tmpJson);
            }


            //Download 7zip dependancy
            if (!Globalfunctions.IsUnix())
            {
                if (!(Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechnicSolderHelper")))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechnicSolderHelper");
                }
            }
            if (!(Globalfunctions.IsUnix() || File.Exists(_sevenZipLocation)))
            {
                WebClient wb = new WebClient();
                Uri sevenWeb = new Uri("http://cloud.zlepper.dk/7za.exe");
                wb.DownloadFile(sevenWeb, _sevenZipLocation);
            }
            _confighandler.SetConfig("InputDirectory", InputFolder.Text);
            _confighandler.SetConfig("OutputDirectory", OutputFolder.Text);

            _path = Path.Combine(_outputDirectory, "mods.html");


            Directory.CreateDirectory(_outputDirectory);
            Environment.CurrentDirectory = _outputDirectory;
            _modlistTextFile = Path.Combine(_outputDirectory, "modlist.txt");
            if (File.Exists(_modlistTextFile))
            {
                File.Delete(_modlistTextFile);
            }

            // The start of the output html file for Technic Solder.
            if (SolderPack.Checked)
            {
                String htmlfile = "<!DOCTYPE html> \n <html> <head>" + Environment.NewLine +
                                  "<title>Mods</title>" + Environment.NewLine +
                                  "<meta charset=\"utf-8\" />" + Environment.NewLine +
                                  "<script src=\"https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js\"></script>" + Environment.NewLine +
                                  "<script src=\"http://cloud.zlepper.dk/technicsolderhelper.js\"></script>" + Environment.NewLine +
                                  "<link type=\"text/css\" rel=\"stylesheet\" href=\"http://cloud.zlepper.dk/technicsolderhelper.css\">" +
                                  "</head>" + Environment.NewLine + "<body><table border='1'><tr><th>Modname</th><th>Modslug</th><th>Version</th></tr>" + Environment.NewLine;
                File.WriteAllText(_path, htmlfile);
            }

            return false;
        }

        private List<String> GetModFiles()
        {
            // Create array with all the mod locations
            List<String> files = new List<String>();
            _totalMods = 0;
            _currentMod = 0;
            // Add the different mod files to the files array
            foreach (String file in Directory.GetFiles(_inputDirectory, "*.zip", SearchOption.AllDirectories))
            {
                files.Add(file);
                _totalMods++;
            }
            foreach (String file in Directory.GetFiles(_inputDirectory, "*.jar", SearchOption.AllDirectories))
            {
                files.Add(file);
                _totalMods++;
            }
            foreach (String file in Directory.GetFiles(_inputDirectory, "*.litemod", SearchOption.AllDirectories))
            {
                files.Add(file);
                _totalMods++;
            }
            foreach (String file in Directory.GetFiles(_inputDirectory, "*.disabled", SearchOption.AllDirectories))
            {
                files.Add(file);
                _totalMods++;
            }
            return files;
        }

        private void initializeSolderSqlHandler()
        {
            _solderSqlHandler = new SolderSqlHandler();
            _modpackId = _solderSqlHandler.GetModpackId(_modpackName);
            if (_modpackId == -1)
            {
                _solderSqlHandler.CreateNewModpack(_modpackName, _modpackName.ToLower().Replace(" ", "-"));
                _modpackId = _solderSqlHandler.GetModpackId(_modpackName);
            }
            _buildId = _solderSqlHandler.GetBuildId(_modpackId, _modpackVersion);
            if (_buildId == -1)
            {
                _solderSqlHandler.CreateModpackBuild(_modpackId, _modpackVersion, _currentMcVersion);
                _buildId = _solderSqlHandler.GetBuildId(_modpackId, _modpackVersion);
            }
        }

        private void Start()
        {
            if (Prepare())
            {
                return;
            }

            List<String> files = GetModFiles();

            while (String.IsNullOrWhiteSpace(_modpackName))
            {
                _modpackName = Prompt.ShowDialog("What is the Modpack Name?", "Modpack Name");
            }
            while (String.IsNullOrWhiteSpace(_modpackVersion))
            {
                _modpackVersion = Prompt.ShowDialog("What Version is the modpack?", "Modpack Version");
            }
            if (String.IsNullOrWhiteSpace(_modpackArchive))
            {
                _modpackArchive = Path.Combine(_outputDirectory, String.Format("{0}-{1}.zip", _modpackName, _modpackVersion));
            }
            _ftbModpackArchive = Path.Combine(_outputDirectory, _modpackName + "-" + _modpackVersion + "-FTB" + ".zip");

            if (String.IsNullOrWhiteSpace(_currentMcVersion))
            {
                _currentMcVersion = Prompt.ShowDialog("What is the Minecraft Version for the modpack?", "Minecraft Version");
            }

            if (useSolder.Checked)
            {
                initializeSolderSqlHandler();
            }

            List<Mcmod> modsList = new List<Mcmod>(_totalMods);
            
            //Check if files have already been added
            foreach (String file in files)
            {
                Debug.WriteLine("");
                _currentMod++;
                if (IsWierdMod(file) == 0)
                {
                    continue;
                }
                // ReSharper disable once InconsistentNaming
                var FileName = file.Substring(file.LastIndexOf(Globalfunctions.PathSeperator) + 1);
                ProgressLabel.Text = FileName;
                //Check for mcmod.info
                Directory.CreateDirectory(_outputDirectory);
                String arguments;
                if (Globalfunctions.IsUnix())
                {
                    _startInfo.FileName = "unzip";
                    arguments = string.Format("-o \"{0}\" \"*.info\" \"*.json\" -d \"{1}\"", file, _outputDirectory);
                }
                else
                {
                    arguments = string.Format("e -y -o\"{0}\" \"{1}\" *.info litemod.json", _outputDirectory, file);
                }
                _startInfo.Arguments = arguments;

                _process.StartInfo = _startInfo;
                _process.Start();
                _process.WaitForExit();
                String mcmodfile = Path.Combine(_outputDirectory, "mcmod.info");
                String litemodfile = Path.Combine(_outputDirectory, "litemod.json");
                if (File.Exists(litemodfile))
                {
                    if (File.Exists(mcmodfile))
                    {
                        File.Delete(mcmodfile);
                    }
                    File.Move(litemodfile, mcmodfile);
                }
                if (!File.Exists(mcmodfile))
                {
                    foreach (String modinfofile in Directory.GetFiles(_outputDirectory, "*.info"))
                    {
                        if (modinfofile.ToLower().Contains("dependancies") ||
                            modinfofile.ToLower().Contains("dependencies"))
                            File.Delete(modinfofile);
                        else
                        {
                            if (!File.Exists(mcmodfile))
                                File.Move(modinfofile, mcmodfile);
                            else
                            {
                                File.Delete(mcmodfile);
                                File.Move(modinfofile, mcmodfile);
                            }
                        }
                    }
                }

                if (File.Exists(mcmodfile))
                {

                    //If exist, then read info and make zip file
                    String json;
                    using (StreamReader r = new StreamReader(mcmodfile))
                    {
                        json = r.ReadToEnd();
                    }
                    try
                    {
                        try
                        {
                            Mcmod2 modinfo2;
                            try
                            {
                                modinfo2 = JsonConvert.DeserializeObject<Mcmod2>(json);
                            }
                            catch (JsonReaderException)
                            {
                                MessageBox.Show(string.Format("Something is wrong with the Json in {0}", FileName));
                                throw new JsonSerializationException("Invalid Json in file" + FileName);
                            }

                            Mcmod mod = new Mcmod();

                            if (modinfo2.Modinfoversion != 0 && modinfo2.Modinfoversion == 2 || modinfo2.ModListVersion != 0 && modinfo2.ModListVersion == 2)
                            {
                                mod.Mcversion = modinfo2.Modlist[0].Mcversion;
                                mod.Modid = modinfo2.Modlist[0].Modid;
                                mod.Name = modinfo2.Modlist[0].Name;
                                mod.Version = modinfo2.Modlist[0].Version;
                                mod.Authors = modinfo2.Modlist[0].Authors;
                                mod.Description = modinfo2.Modlist[0].Description;
                                mod.Url = modinfo2.Modlist[0].Url;
                                if (missingInfoActionOnTheRun.Checked)
                                {
                                    RequireUserInfo(mod, file);
                                }
                                else
                                {
                                    mod.Filename = FileName;
                                    mod.Path = file;
                                    modsList.Add(mod);
                                }
                            }
                            else
                            {
                                throw new JsonSerializationException();
                            }
                        }
                        catch (JsonSerializationException)
                        {
                            try
                            {
                                List<Mcmod> modinfo;
                                try
                                {
                                    modinfo = JsonConvert.DeserializeObject<List<Mcmod>>(json);
                                }
                                catch (JsonReaderException)
                                {
                                    MessageBox.Show(string.Format("Something is wrong with the Json in {0}", FileName));
                                    throw new JsonSerializationException("Invalid Json in file" + FileName);
                                }
                                var mod = modinfo[0];
                                if (missingInfoActionOnTheRun.Checked)
                                {
                                    if (IsFullyInformed(mod))
                                    {
                                        if (CreateTechnicPack.Checked)
                                        {
                                            CreateTechnicModZip(mod, file);
                                        }
                                        if (CreateFTBPack.Checked)
                                        {
                                            CreateFtbPackZip(mod, file);
                                        }
                                    }
                                    else
                                    {
                                        RequireUserInfo(mod, file);
                                    }
                                }
                                else
                                {
                                    mod.Filename = FileName;
                                    mod.Path = file;
                                    modsList.Add(mod);
                                }

                            }
                            catch (JsonSerializationException)
                            {
                                Litemod liteloadermod;

                                try
                                {
                                    liteloadermod = JsonConvert.DeserializeObject<Litemod>(json);
                                }
                                catch (JsonReaderException)
                                {
                                    MessageBox.Show(string.Format("Something is wrong with the Json in {0}", FileName));
                                    throw new JsonSerializationException("Invalid Json in file" + FileName);
                                }
                                //Convert into mcmod
                                Mcmod mod = new Mcmod
                                {
                                    Mcversion = liteloadermod.Mcversion,
                                    Modid = liteloadermod.Name.Replace(" ", ""),
                                    Name = liteloadermod.Name,
                                    Description = liteloadermod.Description,
                                    Authors = new List<string> { liteloadermod.Author }
                                };

                                if (String.IsNullOrEmpty(liteloadermod.Version) || String.IsNullOrEmpty(liteloadermod.Revision))
                                {
                                    if (!(String.IsNullOrEmpty(liteloadermod.Version)))
                                    {
                                        mod.Version = liteloadermod.Version;
                                    }
                                    else
                                    {
                                        if (!(String.IsNullOrEmpty(liteloadermod.Revision)))
                                        {
                                            mod.Version = liteloadermod.Revision;
                                        }
                                    }
                                }
                                else
                                {
                                    mod.Version = liteloadermod.Version + "-" + liteloadermod.Revision;
                                }
                                if (missingInfoActionOnTheRun.Checked)
                                {
                                    RequireUserInfo(mod, file);
                                }
                                else
                                {
                                    mod.Filename = FileName;
                                    mod.Path = file;
                                    modsList.Add(mod);
                                }

                            }
                        }

                    }
                    catch (JsonSerializationException)
                    {
                        RequireUserInfo(file);
                    }
                    File.Delete(mcmodfile);
                }
                else
                {
                    
                    
                    //Check the FTB permission sheet for info before doing anything else
                    String shortname = _ftbPermsSqLhelper.GetShortName(SqlHelper.CalculateMd5(file));
                    if (String.IsNullOrWhiteSpace(shortname))
                    {
                        int fixNr = IsWierdMod(FileName);
                        if (fixNr != Int32.MaxValue)
                        {
                            Mcmod mod;
                            switch (fixNr)
                            {
                            //Reikas Pattern
                                case 1:
                                    mod = ModHelper.ReikasMods(FileName);
                                    RequireUserInfo(mod, file);
                                    break;
                                case 2:
                                    mod = ModHelper.WailaPattern(FileName);
                                    RequireUserInfo(mod, file);
                                    break;
                                case 3:
                                    Liteloaderversion llversion = _liteloaderSqlHelper.GetInfo(SqlHelper.CalculateMd5(file));
                                    mod = new Mcmod
                                    {
                                        Mcversion = llversion.Mcversion,
                                        Name = "Liteloader",
                                        Modid = llversion.TweakClass
                                    };
                                    try
                                    {
                                        mod.Version = llversion.Version.Substring(llversion.Version.LastIndexOf("_", StringComparison.Ordinal) + 1);
                                    }
                                    catch (NullReferenceException)
                                    {
                                        mod.Version = 1.ToString();
                                    }
                                    if (missingInfoActionOnTheRun.Checked)
                                    {
                                        RequireUserInfo(mod, file);
                                    }
                                    else
                                    {
                                        mod.Filename = FileName;
                                        mod.Path = file;
                                        modsList.Add(mod);
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            if (missingInfoActionOnTheRun.Checked)
                            {
                                RequireUserInfo(file);
                            }
                            else
                            {
                                modsList.Add(new Mcmod
                                {
                                    Path = file,
                                    Filename = FileName
                                });
                            }
                        }
                    }
                    else
                    {
                        Mcmod mod = new Mcmod();
                        if (shortname.Equals("ignore"))
                        {
                            mod.IsIgnore = true;
                        }
                        else
                        {
                            mod.IsIgnore = false;
                            mod.UseShortName = true;
                            mod.Modid = shortname;
                            mod.Name = _ftbPermsSqLhelper.GetInfoFromShortName(shortname, FtbPermissionsSqlHelper.InfoType.ModName);
                            mod.Authors = new List<string>
                            {
                                _ftbPermsSqLhelper.GetInfoFromModId(shortname, FtbPermissionsSqlHelper.InfoType.ModAuthor)
                            };
                            mod.AuthorList = mod.Authors;
                            mod.PrivatePerms = _ftbPermsSqLhelper.DoFtbHavePermission(shortname, false);
                            mod.PublicPerms = _ftbPermsSqLhelper.DoFtbHavePermission(shortname, true);
                        }

                        if (missingInfoActionOnTheRun.Checked)
                        {
                            if (CreateFTBPack.Checked && !CreateTechnicPack.Checked)
                                CreateFtbPackZip(mod, file);
                            else
                            {
                                if (CreateFTBPack.Checked || CreateTechnicPack.Checked)
                                    RequireUserInfo(mod, file);
                            }
                        }
                        else
                        {
                            mod.Filename = FileName;
                            mod.Path = file;
                            modsList.Add(mod);
                        }
                    }
                }
            }
            if (missingInfoActionCreateList.Checked)
            {
                Form modinfo = new Modinfo(modsList, this);
                modinfo.ShowDialog();
            }

            Environment.CurrentDirectory = _inputDirectory;
            String[] directories = Directory.GetDirectories(_inputDirectory);
            const string minecraftVersionPattern = @"^[0-9]{1}\.[0-9]{1}\.[0-9]{1,2}$";
            foreach (String dir in directories)
            {
                String dirName = dir.Substring(dir.LastIndexOf(Globalfunctions.PathSeperator) + 1);
                if (Regex.IsMatch(dirName, minecraftVersionPattern, RegexOptions.Multiline))
                {
                    continue;
                }
                String[] jarFiles = Directory.GetFiles(dir, "*.jar", SearchOption.AllDirectories);
                if (jarFiles.Length != 0)
                {
                    continue;
                }
                String levelOverInputDirectory = _inputDirectory.Remove(_inputDirectory.LastIndexOf(Globalfunctions.PathSeperator));
				
                DialogResult confirmInclude = MessageBox.Show(string.Format("Do you want to include {0}?", dirName),
                                                  @"Additional folder found", MessageBoxButtons.YesNo);
                if (confirmInclude == DialogResult.Yes)
                {
                    Environment.CurrentDirectory = levelOverInputDirectory;
                    if (CreateTechnicPack.Checked)
                    {
                        //Create Technic Pack
                        if (SolderPack.Checked)
                        {
                            List<String> md5Values = new List<string>();
                            List<String> oldmd5Values = new List<string>();
                            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "unarchievedFiles"));
                            String md5ValuesFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "unarchievedFiles", dirName + ".txt");
                            if (File.Exists(md5ValuesFile))
                            {
                                using (StreamReader reader = new StreamReader(md5ValuesFile))
                                {
                                    while (true)
                                    {
                                        String tmp = reader.ReadLine();
                                        if (String.IsNullOrWhiteSpace(tmp))
                                            break;
                                        oldmd5Values.Add(tmp);
                                    }
                                }
                            }

                            bool same = true;
                            foreach (String f in Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories))
                            {
                                if (!oldmd5Values.Contains(SqlHelper.CalculateMd5(f)))
                                {
                                    same = false;
                                }
                                md5Values.Add(SqlHelper.CalculateMd5(f));
                            }
                            if (same)
                            {
                                continue;
                            }
                            while (String.IsNullOrWhiteSpace(_modpackVersion))
                            {
                                _modpackVersion = Prompt.ShowDialog("What Version is the modpack?", "Modpack Version");
                            }
                            //So we need to include this folder
                            Environment.CurrentDirectory = levelOverInputDirectory;
                            String outputfile = Path.Combine(_outputDirectory, "mods", dirName.ToLower(), dirName.ToLower() + "-" + MakeUrlFriendly(_modpackName + "-" + _modpackVersion) + ".zip");
                            Directory.CreateDirectory(Path.Combine(_outputDirectory, "mods", dirName.ToLower()));
                            if (Globalfunctions.IsUnix())
                            {
                                _startInfo.FileName = "zip";
                                _startInfo.Arguments = String.Format("-r \"{0}\" mods/{1}", outputfile, dirName);
                            }
                            else
                            {
                                _startInfo.FileName = _sevenZipLocation;
                                _startInfo.Arguments = String.Format("a -y \"{0}\" \"mods\\{1}\"", outputfile, dirName);
                            }
                            _process.StartInfo = _startInfo;
                            _process.Start();
                            CreateTableRow(dirName, dirName.ToLower(), MakeUrlFriendly(_modpackName + "-" + _modpackVersion));
                            _process.WaitForExit();

                            if (useSolder.Checked)
                            {
                                int id = _solderSqlHandler.GetModId(dirName.ToLower());
                                if (id == -1)
                                {
                                    _solderSqlHandler.AddModToSolder(dirName.ToLower(), null, null, null, dirName);
                                    id = _solderSqlHandler.GetModId(dirName.ToLower());
                                }
                                String modversion = MakeUrlFriendly(_modpackName + "-" + _modpackVersion);
                                String md5 = SqlHelper.CalculateMd5(outputfile).ToLower();
                                if (_solderSqlHandler.IsModversionOnline(dirName.ToLower(), modversion))
                                    _solderSqlHandler.UpdateModversionMd5(dirName.ToLower(), modversion, md5);
                                else
                                    _solderSqlHandler.AddNewModversionToSolder(id, modversion, md5);

                                id = _solderSqlHandler.GetModId(dirName.ToLower());
                                int modVersionId = _solderSqlHandler.GetModversionId(id, modversion);
                                _solderSqlHandler.AddModversionToBuild(_buildId, modVersionId);
                            }
                            if (File.Exists(md5ValuesFile))
                            {
                                File.Delete(md5ValuesFile);
                            }
                            foreach (String md5Value in md5Values)
                            {
                                File.AppendAllText(md5ValuesFile, md5Value + Environment.NewLine);
                            }
                        }
                        else
                        {
                            if (Globalfunctions.IsUnix())
                            {
                                _startInfo.FileName = "zip";
                                _startInfo.Arguments = String.Format("-r \"{0}\" \"./mods/{1}\"", _modpackArchive, dirName);
                            }
                            else
                            {
                                _startInfo.FileName = _sevenZipLocation;
                                _startInfo.Arguments = String.Format("a -y \"{0}\" \"mods\\{1}\"", _modpackArchive, dirName);
                            }
                            _process.StartInfo = _startInfo;
                            _process.Start();
                            _process.WaitForExit();
                        }
                    } 
                    if (CreateFTBPack.Checked)
                    {
                        // Create FTB Pack

                        String tmpDir = Path.Combine(_outputDirectory, "minecraft", "mods");
                        Directory.CreateDirectory(tmpDir);
                        if (Globalfunctions.IsUnix())
                        {
                            _startInfo.FileName = "cp";
                            _startInfo.Arguments = String.Format("-r \"./mods/{0}\" \"{1}\"", dirName, tmpDir);
                            _process.StartInfo = _startInfo;
                            _process.Start();
                            _process.WaitForExit();
                        }
                        else
                        {
                            String input = Path.Combine(_inputDirectory, dirName);
                            DirectoryCopy(input, tmpDir, true);
                        }
                    }
                }
				
            }

            // Pack additional folders if they are marked
            if (CreateTechnicPack.Checked)
            {
                Environment.CurrentDirectory = _inputDirectory.Remove(_inputDirectory.LastIndexOf(Globalfunctions.PathSeperator));
                foreach (KeyValuePair<String, CheckBox> cb in _additionalDirectories)
                {
                    if (cb.Value.Checked)
                    {
                        String folderName = cb.Key.Substring(cb.Key.LastIndexOf(Globalfunctions.PathSeperator) + 1).ToLower();
                        if (SolderPack.Checked)
                        {
                            String mpname = _modpackName.Replace(" ", "-").ToLower();
                            String of = Path.Combine(_outputDirectory, "mods", folderName);
                            Directory.CreateDirectory(of);
                            String outputfile = Path.Combine(of, folderName.ToLower() + "-" + mpname + "-" + _modpackVersion + ".zip");
                            if (Globalfunctions.IsUnix())
                            {
                                _startInfo.FileName = "zip";
                                _startInfo.Arguments = String.Format("-r \"{0}\" {1}", outputfile, folderName);
                            }
                            else
                            {
                                _startInfo.FileName = _sevenZipLocation;
                                _startInfo.Arguments = String.Format("a -y \"{0}\" \"{1}\"", outputfile, folderName);
                            }
                            _process.StartInfo = _startInfo;
                            _process.Start();
                            _process.WaitForExit();

                            CreateTableRow(folderName, folderName.ToLower(), mpname + "-" + _modpackVersion);

                            if (useSolder.Checked)
                            {
                                int id = _solderSqlHandler.GetModId(folderName.ToLower());
                                if (id == -1)
                                {
                                    _solderSqlHandler.AddModToSolder(folderName.ToLower(), null, null, null, folderName);
                                    id = _solderSqlHandler.GetModId(folderName.ToLower());
                                }
                                String md5 = SqlHelper.CalculateMd5(outputfile).ToLower();
                                if (_solderSqlHandler.IsModversionOnline(folderName.ToLower(),
                                        mpname + "-" + _modpackVersion))
                                    _solderSqlHandler.UpdateModversionMd5(folderName.ToLower(),
                                        mpname + "-" + _modpackVersion, md5);
                                else
                                    _solderSqlHandler.AddNewModversionToSolder(id, mpname + "-" + _modpackVersion,
                                        md5);
                                int modVersionId = _solderSqlHandler.GetModversionId(_solderSqlHandler.GetModId(folderName.ToLower()), mpname + "-" + _modpackVersion);
                                _solderSqlHandler.AddModversionToBuild(_buildId, modVersionId);
                            }
                        }
                        else
                        {

                            if (Globalfunctions.IsUnix())
                            {
                                _startInfo.FileName = "zip";
                                _startInfo.Arguments = String.Format("-r \"{0}\" \"{1}\"", _modpackArchive, folderName);
                            }
                            else
                            {
                                _startInfo.FileName = _sevenZipLocation;
                                _startInfo.Arguments = String.Format("a -y \"{0}\" \"{1}\"", _modpackArchive, folderName);
                            }
                            _process.StartInfo = _startInfo;
                            _process.Start();
                            _process.WaitForExit();

                        }
                    }
                }
            }


            if (CreateTechnicPack.Checked && IncludeConfigZip.Checked)
                CreateConfigZip();

            //FTB pack configs
            if (CreateFTBPack.Checked)
            {
                foreach (KeyValuePair<String, CheckBox> cb in _additionalDirectories)
                {
                    if (cb.Value.Checked)
                    {
                        String dirName = cb.Key.Substring(cb.Key.LastIndexOf(Globalfunctions.PathSeperator) + 1);
                        String tmpDir = Path.Combine(_outputDirectory, "minecraft");

                        Directory.CreateDirectory(tmpDir);
                        if (Globalfunctions.IsUnix())
                        {
                            _startInfo.FileName = "cp";
                            _startInfo.Arguments = String.Format("-r \"{0}\" \"{1}\"", dirName, tmpDir);
                            _process.StartInfo = _startInfo;
                            _process.Start();
                            _process.WaitForExit();
                        }
                        else
                        {
                            FileAttributes attr = File.GetAttributes(cb.Key);
                            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                tmpDir = Path.Combine(tmpDir, dirName);
                                DirectoryCopy(cb.Key, tmpDir, true);
                            }
                            else
                            {
                                String outputFile = Path.Combine(tmpDir, dirName);
                                File.Copy(cb.Key, outputFile);
                            }
                        }
                    }
                }

                String tmpConfigDirectory = Path.Combine(_outputDirectory, Path.Combine("minecraft", "config"));
                Directory.CreateDirectory(tmpConfigDirectory);

                String sourceConfigDirectory = InputFolder.Text.Replace(Globalfunctions.PathSeperator + "mods", Globalfunctions.PathSeperator + "config");
                try
                {
                    DirectoryCopy(sourceConfigDirectory, tmpConfigDirectory, true);
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("I can't seem to find a config directory for the FTB pack.");
                }

                Environment.CurrentDirectory = _outputDirectory;
                if (Globalfunctions.IsUnix())
                {
                    _startInfo.FileName = "zip";
                    _startInfo.Arguments = String.Format("-r \"{0}\" \"minecraft\" -x minecraft/config/YAMPST.nbt",
                        _ftbModpackArchive);
                }
                else
                    _startInfo.Arguments = string.Format("a -x!minecraft\\config\\YAMPST.nbt -y \"{0}\" \"minecraft\" ", _ftbModpackArchive);

                _process.StartInfo = _startInfo;
                _process.Start();
                _process.WaitForExit();
                Directory.Delete(Path.Combine(_outputDirectory, "minecraft"), true);
            }

            if (CreateTechnicPack.Checked && IncludeForgeVersion.Checked)
            {
                string selectedBuild = ForgeBuild.SelectedItem.ToString();
                Number forgeinfo = _forgeSqlHelper.GetForgeInfo(selectedBuild);
                String tmpdir = Path.Combine(_outputDirectory, "bin");
                Directory.CreateDirectory(tmpdir);
                String tempfile = Path.Combine(tmpdir, "modpack.jar");

                WebClient wb = new WebClient();
                wb.DownloadFile(forgeinfo.Downloadurl, tempfile);
                if (SolderPack.Checked)
                {
                    Directory.CreateDirectory(Path.Combine(_outputDirectory, "mods", "forge"));
                    String outputfile = Path.Combine(_outputDirectory, "mods", "forge", "forge-" + forgeinfo.Version + ".zip");
                    if (Globalfunctions.IsUnix())
                    {
                        _startInfo.FileName = "zip";
                        Environment.CurrentDirectory = _outputDirectory;
                        _startInfo.Arguments = "-r \"" + outputfile + "\" \"bin\"";
                    }
                    else
                    {
                        _startInfo.Arguments = "a -y \"" + outputfile + "\" \"" + tmpdir + "\"";
                    }
                    CreateTableRow("Minecraft Forge", "forge", forgeinfo.Version.ToLower());

                    
                }
                else
                {
                    if (Globalfunctions.IsUnix())
                    {
                        Environment.CurrentDirectory = _outputDirectory;
                        _startInfo.FileName = "zip";
                        _startInfo.Arguments = "-r \"" + _modpackArchive + "\" \"bin\"";
                    }
                    else
                    {
                        _startInfo.Arguments = "a -y \"" + _modpackArchive + "\" \"" + tmpdir + "\"";
                    }
                }
                _process.StartInfo = _startInfo;
                _process.Start();
                _process.WaitForExit();

                if (useSolder.Checked && SolderPack.Checked)
                {
                    int id = _solderSqlHandler.GetModId("forge");
                    if (id == -1)
                    {
                        _solderSqlHandler.AddModToSolder("forge", "Minecraft Forge is a common open source API allowing a broad range of mods to work cooperatively together. It allows many mods to be created without them editing the main Minecraft code.", "LexManos, Eloram, Spacetoad", "http://MinecraftForge.net", "Minecraft Forge");
                        id = _solderSqlHandler.GetModId("forge");
                    }
                    String outputfile = Path.Combine(_outputDirectory, "mods", "forge", "forge-" + forgeinfo.Version + ".zip");
                    String md5 = SqlHelper.CalculateMd5(outputfile).ToLower();
                    if (_solderSqlHandler.IsModversionOnline("forge", forgeinfo.Version))
                        _solderSqlHandler.UpdateModversionMd5("forge", forgeinfo.Version, md5);
                    else
                        _solderSqlHandler.AddNewModversionToSolder(id, forgeinfo.Version.ToLower(), md5);

                    int modVersionId = _solderSqlHandler.GetModversionId(_solderSqlHandler.GetModId("forge"), forgeinfo.Version.ToLower());
                    _solderSqlHandler.AddModversionToBuild(_buildId, modVersionId);
                }

                Directory.Delete(tmpdir, true);
            }

            if (Directory.Exists(Path.Combine(_outputDirectory, "assets")))
                Directory.Delete(Path.Combine(_outputDirectory, "assets"), true);
            if (Directory.Exists(Path.Combine(_outputDirectory, "example")))
                Directory.Delete(Path.Combine(_outputDirectory, "example"), true);

            if (CreateTechnicPack.Checked && SolderPack.Checked && !useSolder.Checked)
            {
                File.AppendAllText(_path, @"</table><button id=""Reshow"" type=""button"">Unhide Everything</button><p>List autogenerated by TechnicSolderHelper &copy; 2014 - Rasmus Hansen</p></body></html>");
                if (Globalfunctions.IsUnix())
                {
                    Process.Start(_path);
                }
                else
                {
                    try
                    {
                        Process.Start("chrome.exe", _path);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            Process.Start("iexplore", _path);
                        }
                        catch (Exception)
                        {
                            try
                            {
                                Process.Start("firefox.exe", _path);
                            }
                            catch (Exception)
                            {
                                Process.Start(_path);
                            }
                        }
                    }
                }

            }
            if (CreateTechnicPack.Checked && SolderPack.Checked && UploadToFTPServer.Checked)
            {
                ProgressLabel.Text = "Start Uploading to FTP Server";
                if (_ftp == null)
                {
                    _ftp = new Ftp();
                }

                MessageToUser m = new MessageToUser();
                Thread startingThread = new Thread(m.UploadingToFtp);
                startingThread.Start();

                _ftp.UploadFolder(Path.Combine(_outputDirectory, "mods"));

            }

            if (CreateTechnicPack.Checked && SolderPack.Checked && UseS3.Checked)
            {
                ProgressLabel.Text = "Uploading to s3.";
                S3 s3Client = new S3();
                s3Client.UploadFolder(Path.Combine(_outputDirectory, "mods"));
            }


            ProgressLabel.Text = @"Waiting...";

            InputFolder.Items.Clear();
            try
            {
                InputFolder.Items.AddRange(_inputDirectories.ToArray());
            }
            catch
            {
                // ignored
            }
        }

        #region Technic Pack Function



        /// <summary>
        /// Checks if the mod is on the list of mods which has custom support.
        /// </summary>
        /// <param name="modFileName">The mod file name.</param>
        /// <returns>Returns the number of the method to call, if no match is found, returns zero</returns>
        private static int IsWierdMod(String modFileName)
        {
            String[] skipMods =
                {"CarpentersBlocksCachedResources", 
                    "CodeChickenLib", 
                    "ForgeMultipart", 
                    "ejml-",
                    "commons-codec",
                    "commons-compress",
                    "Cleanup"
                };
            if (skipMods.Any(t => modFileName.ToLower().Contains(t.ToLower())))
            {
                return 0;
            }
            String[] modPatterns =
                {@"[a-z]+ 1.[0-9].[0-9]* V[0-9]*[a-z]*",
                    @"[a-z]+-[0-9.]+_[0-9.]+",
                    @"liteloader"
                };
            for (int i = 0; i < modPatterns.Length; i++)
            {
                if (Regex.IsMatch(modFileName, modPatterns[i], RegexOptions.IgnoreCase))
                {
                    return i + 1;
                }
            }

            return Int32.MaxValue;
        }

        /// <summary>
        /// Check is the mcmod.info file has all the info we need to produce a zip file
        /// </summary>
        /// <param name="mod"></param>
        /// <returns>
        /// Returns true if everything is alright</returns>
        public static bool IsFullyInformed(Mcmod mod)
        {
            if (String.IsNullOrWhiteSpace(mod.Name) || String.IsNullOrWhiteSpace(mod.Version) ||
                String.IsNullOrWhiteSpace(mod.Mcversion) || String.IsNullOrWhiteSpace(mod.Modid))
                return false;
            Debug.WriteLine(mod.Version);
            if (mod.Name.Contains("${") || mod.Version.Contains("${") || mod.Mcversion.Contains("${") || mod.Modid.Contains("${") || mod.Version.ToLower().Contains("@version@"))
            {
                return false;
            }
            return true;
        }

        private String MakeUrlFriendly(String value)
        {
            value = value.ToLower();
            return Regex.Replace(value, @"[^A-Za-z0-9_\.~]+", "-");
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
                Directory.CreateDirectory(destDirName);

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (!copySubDirs)
                return;
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }

        private void RequireUserInfo(Mcmod currentData, String file)
        {
            try
            {
                Mcmod mod;
                String s = SqlHelper.CalculateMd5(file);
                Debug.WriteLine(s);
                DataSuggest suggest = new DataSuggest();

                try
                {
                    mod = _modsSqLhelper.GetModInfo(s);
                    Debug.WriteLine("Got from local database");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    mod = suggest.GetMcmod(s);
                    if (mod == null)
                    {
                        mod = new Mcmod();
                    }
                    else
                    {
                        mod.FromSuggestion = true;
                        Debug.WriteLine("Got from remove database");
                    }
                }
                if (mod == null)
                {
                    Debug.WriteLine("didn't get anything from local database");
                    mod = suggest.GetMcmod(SqlHelper.CalculateMd5(file));
                    if (mod == null)
                    {
                        mod = new Mcmod();
                    }
                    else
                    {
                        mod.FromSuggestion = true;
                        Debug.WriteLine("Got from remove database");
                    }
                }

                String fileName = file.Substring(file.LastIndexOf(Globalfunctions.PathSeperator) + 1);
                fileName = fileName.Remove(fileName.LastIndexOf(".", StringComparison.Ordinal));


                if (currentData.Name != null && !currentData.Name.Contains("${"))
                {
                    mod.Name = currentData.Name;

                }
                else
                {
                    if (mod.Name == null && (String.IsNullOrWhiteSpace(currentData.Name) || currentData.Name.Contains("${")))
                    {
                        String a =
                            string.Format("Mod name of {0}{1}Go bug the mod author to include an mcmod.info file!",
                                fileName, Environment.NewLine);
                        mod.Name = Prompt.ShowDialog(a, "Mod Name", false,
                            Prompt.ModsLeftString(_totalMods, _currentMod));
                        currentData.Name = mod.Name;
                        if (mod.Name.Equals(""))
                            return;
                        mod.FromUserInput = true;
                    }

                }
                if (currentData.Version != null && !currentData.Version.Contains("${") && !currentData.Version.ToLower().Contains("@version@"))
                    mod.Version = currentData.Version.Replace(" ", "+").ToLower();
                else
                {
                    if (mod.Version == null && (String.IsNullOrWhiteSpace(currentData.Version) || currentData.Version.Contains("${") || currentData.Version.ToLower().Contains("@version@")))
                    {
                        String a =
                            String.Format(
                                "Mod version of {0}" + Environment.NewLine +
                                "Go bug the mod author to include an mcmod.info file!", fileName);
                        mod.Version = Prompt.ShowDialog(a, "Mod Version", false,
                            Prompt.ModsLeftString(_totalMods, _currentMod));
                        mod.Version = mod.Version.Replace(" ", "+").ToLower();
                        if (mod.Version.Equals(""))
                            return;
                        mod.FromUserInput = true;
                    }
                }

                if (currentData.Mcversion != null && !currentData.Mcversion.Contains("${"))
                    mod.Mcversion = currentData.Mcversion;
                else if (mod.Mcversion == null && (String.IsNullOrWhiteSpace(currentData.Mcversion) || currentData.Mcversion.Contains("${")))
                if (_currentMcVersion == null)
                {
                    String a =
                        String.Format(
                            "Minecraft Version of {0}" + Environment.NewLine +
                            "Go bug the mod author to include an mcmod.info file!", fileName);
                    mod.Mcversion = Prompt.ShowDialog(a, "Minecraft Version", false,
                        Prompt.ModsLeftString(_totalMods, _currentMod));
                    _currentMcVersion = mod.Mcversion;
                    currentData.Mcversion = _currentMcVersion;
                }
                else
                {
                    mod.Mcversion = _currentMcVersion;
                    currentData.Mcversion = _currentMcVersion;
                }

                if (!String.IsNullOrWhiteSpace(currentData.Modid))
                {
                    mod.Modid = currentData.Modid;
                }
                //mod.Modid = currentData.Modid ?? mod.Name.Replace(" ", "").ToLower();
                if (mod.Name != null && (String.IsNullOrWhiteSpace(mod.Modid) || mod.Modid.Contains("${")))
                {
                    mod.Modid = mod.Name.Replace(" ", "").ToLower();
                }

                string md5 = SqlHelper.CalculateMd5(file);
                if (mod.FromUserInput && !mod.FromSuggestion && !suggest.IsModSuggested(md5))
                {
                    suggest.Suggest(fileName, mod.Mcversion, mod.Version, md5, mod.Modid, mod.Name);
                }

                if (CreateTechnicPack.Checked)
                    CreateTechnicModZip(mod, file);
                if (CreateFTBPack.Checked)
                    CreateFtbPackZip(mod, file);
            }
            catch (NullReferenceException ex)
            {
                String error = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "error.txt");
                File.AppendAllText(error, ex.Message);
                File.AppendAllText(error, ex.StackTrace);
                MessageBox.Show("Please check the error.txt file on your desktop, and send it to the developer.");
                RequireUserInfo(file);
            }
        }

        private void RequireUserInfo(String file)
        {
            Mcmod mod = new Mcmod { Mcversion = null, Modid = null, Name = null, Version = null };

            RequireUserInfo(mod, file);
        }

        private void InputDirectoryBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowser.SelectedPath = InputFolder.SelectedText;
            DialogResult result = FolderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                InputFolder.Text = FolderBrowser.SelectedPath;
                _confighandler.SetConfig("InputDirectory", InputFolder.Text);
            }
            InputFolder_TextChanged(null, null);

        }

        private void OutputDirectoryBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowser.SelectedPath = OutputFolder.Text;
            DialogResult result = FolderBrowser.ShowDialog();
            if (result != DialogResult.OK)
                return;
            OutputFolder.Text = FolderBrowser.SelectedPath;
            _confighandler.SetConfig("OutputDirectory", OutputFolder.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _modsSqLhelper.ResetTable();
            String s = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "unarchievedFiles");
            if (Directory.Exists(s))
            {
                Directory.Delete(s, true);
            }
        }

        private void InputFolder_TextChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("InputDirectory", InputFolder.Text);

            String superDirectory = InputFolder.Text.Remove(InputFolder.Text.LastIndexOf(Globalfunctions.PathSeperator));


            if (!Directory.Exists(superDirectory))
                return;
            List<String> dirs = Directory.GetDirectories(superDirectory).Where(dir => !dir.EndsWith("mods") && !dir.EndsWith("config")).ToList();
            _additionalDirectories.Clear();
            int c = 0;
            for (int i = 23; i < dirs.Count * 23 + 23; i += 23)
            {
                if (!_additionalDirectories.ContainsKey(dirs[c]))
                {
                    String dirname = dirs[c].Substring(dirs[c].LastIndexOf(Globalfunctions.PathSeperator) + 1);
                    _additionalDirectories.Add(dirs[c], new CheckBox
                        {
                            Left = 20,
                            Top = i,
                            Height = 20,
                            Text = dirname
                        });
                }
                c++;
            }


            String serversDat = Path.Combine(superDirectory, "servers.dat");
            if (File.Exists(serversDat))
            {
                _additionalDirectories.Add(serversDat, new CheckBox
                    {
                        Left = 20,
                        Top = c * 23 + 23,
                        Height = 20,
                        Text = @"Servers.dat file"
                    });
            }
            groupBox1.Controls.Clear();
            foreach (CheckBox cb in _additionalDirectories.Values)
            {
                groupBox1.Controls.Add(cb);
            }
        }

        private void OutputFolder_TextChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("OutputDirectory", OutputFolder.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ExcelReader.AddFtbPermissions();
        }

        private void CreateFTBPack_CheckedChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("CreateFTBPack", CreateFTBPack.Checked);

            if (CreateFTBPack.Checked)
            {
                DistributionLevel.Show();
            }
            else
            {
                DistributionLevel.Hide();
            }
        }

        private void PrivateFTBPack_CheckedChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("CreatePrivateFTBPack", PrivateFTBPack.Checked);
        }

        private void CreateTechnicPack_CheckedChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("CreateTechnicSolderFiles", CreateTechnicPack.Checked);
            if (CreateTechnicPack.Checked)
            {
                SolderPackType.Show();
                DistributionLevel.Location = new Point(DistributionLevel.Location.X, DistributionLevel.Location.Y + SolderPackType.Height);
                CreateFTBPack.Location = new Point(CreateFTBPack.Location.X, CreateFTBPack.Location.Y + SolderPackType.Height);
                if (TechnicPermissions.Checked)
                {
                    TechnicDistributionLevel.Show();
                }
                else
                {
                    TechnicDistributionLevel.Hide();
                }
                if (IncludeForgeVersion.Checked)
                {
                    labelforgeversion.Show();
                    labelmcversion.Show();
                    ForgeBuild.Show();
                    MCversion.Show();
                }
                else
                {
                    labelforgeversion.Hide();
                    labelmcversion.Hide();
                    ForgeBuild.Hide();
                    MCversion.Hide();
                }
            }
            else
            {
                SolderPackType.Hide();
                DistributionLevel.Location = new Point(DistributionLevel.Location.X, DistributionLevel.Location.Y - SolderPackType.Height);
                CreateFTBPack.Location = new Point(CreateFTBPack.Location.X, CreateFTBPack.Location.Y - SolderPackType.Height);
                TechnicDistributionLevel.Hide();
                labelforgeversion.Hide();
                labelmcversion.Hide();
                ForgeBuild.Hide();
                MCversion.Hide();
            }
        }

        private void IncludeConfigZip_CheckedChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("CreateTechnicConfigZip", IncludeConfigZip.Checked);
        }

        private void SolderPack_CheckedChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("CreateSolderPack", SolderPack.Checked);

            if (SolderPack.Checked)
            {
                IncludeForgeVersion.Text = "Create Forge zip";
                IncludeConfigZip.Text = "Create Config zip";
            }
            else
            {
                IncludeForgeVersion.Text = "Include Forge in zip";
                IncludeConfigZip.Text = "Include Configs in zip";
            }
        }

        private void CheckPermissions_CheckedChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("CheckTecnicPermissions", TechnicPermissions.Checked);

            if (TechnicPermissions.Checked)
                TechnicDistributionLevel.Show();
            else
                TechnicDistributionLevel.Hide();
        }

        private void TechnicPrivatePermissions_CheckedChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("TechnicPrivatePermissionsLevel", TechnicPrivatePermissions.Checked);
        }

        #endregion

        private void UploadToFTPServer_CheckedChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("UploadToFTPServer", UploadToFTPServer.Checked);

            if (UploadToFTPServer.Checked)
            {
                bool hasbeenwarned = false;
                try
                {
                    hasbeenwarned = Convert.ToBoolean(_confighandler.GetConfig("HasBeenWarnedAboutLongFTPTimes"));
                }
                catch
                {
                    // ignored
                }
                if (!hasbeenwarned)
                {
                    _confighandler.SetConfig("HasBeenWarnedAboutLongFTPTimes", true);
                    var responce = MessageBox.Show(@"Uploading to FTP can take a very long time. Do you still want to upload to FTP?", @"FTP upload", MessageBoxButtons.YesNo);
                    if (responce == DialogResult.Yes)
                        configureFTP.Show();
                    else
                    {
                        UploadToFTPServer.Checked = false;
                        return;
                    }
                }
                configureFTP.Show();
            }
            else
            {
                configureFTP.Hide();
                return;
            }
            if (_ftp == null)
                _ftp = new Ftp();
        }

        private void MCversion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ForgeBuild.Items.Clear();
            String selectedMcversion = MCversion.SelectedItem.ToString();
            List<String> forgeVersions = _forgeSqlHelper.GetForgeVersions(selectedMcversion);

            foreach (String build in forgeVersions)
                ForgeBuild.Items.Add(build);
        }

        private void GetForgeVersions_Click(object sender, EventArgs e)
        {
            MCversion.Items.Clear();

            _forgeSqlHelper.FindAllForgeVersion();
            List<String> mcversions = _forgeSqlHelper.GetMcVersions();
            foreach (String mcversion in mcversions)
            {
                MCversion.Items.Add(mcversion);
            }
        }

        private void IncludeForgeVersion_CheckedChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("IncludeForgeVersion", IncludeForgeVersion.Checked);

            if (IncludeForgeVersion.Checked)
            {
                labelforgeversion.Show();
                labelmcversion.Show();
                ForgeBuild.Show();
                MCversion.Show();
            }
            else
            {
                labelforgeversion.Hide();
                labelmcversion.Hide();
                ForgeBuild.Hide();
                MCversion.Hide();
            }
        }

        private void getliteloaderversions_Click(object sender, EventArgs e)
        {
            String liteloaderjsonfile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            liteloaderjsonfile = Path.Combine(liteloaderjsonfile, "liteloader.json");
            WebClient wb = new WebClient();
            Uri webfile = new Uri("http://dl.liteloader.com/versions/versions.json");
            wb.DownloadFile(webfile, liteloaderjsonfile);

            String json;
            using (StreamReader r = new StreamReader(liteloaderjsonfile))
                json = r.ReadToEnd();
            Liteloader liteloader = JsonConvert.DeserializeObject<Liteloader>(json);

            foreach (KeyValuePair<String, Versions> item in liteloader.Versions)
                foreach (Versionclass it in item.Value.Artefacts["com.mumfrey:liteloader"].Values)
                    _liteloaderSqlHelper.AddVersion(it.File, it.Version, it.Md5, item.Key, it.TweakClass);
        }

        private void configureFTP_Click(object sender, EventArgs e)
        {
            Form f = new FtpInfo();
            f.ShowDialog();
            _ftp = new Ftp();
        }

        private void OnApplicationClosing(object sender, EventArgs e)
        {
            //MessageBox.Show("Exiting");
            String json = JsonConvert.SerializeObject(_inputDirectories);
            FileInfo inputDirectoriesFile = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "inputDirectories.json"));
            File.WriteAllText(inputDirectoriesFile.ToString(), json);
        }

        private void testmysql_Click(object sender, EventArgs e)
        {
            Form f = new Modinfo(this);
            f.Show();
        }

        private void configureSolder_Click(object sender, EventArgs e)
        {
            Form f = new SqlInfo();
            f.ShowDialog();
            _solderSqlHandler = new SolderSqlHandler();
        }

        private void useSolder_CheckedChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("useSolder", useSolder.Checked);
            if (useSolder.Checked)
            {
                configureSolder.Show();
            }
            else
            {
                configureSolder.Hide();
            }
        }

        private void savesqlcommands_CheckedChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("saveSQL", savesqlcommands.Checked);
        }

        private void UseS3_CheckedChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("useS3", UseS3.Checked);
            ConfigureS3.Visible = UseS3.Checked;
        }

        private void ConfigureS3_Click(object sender, EventArgs e)
        {
            Form f = new S3Info();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form f = new DatabaseEditor();
            f.Show();
        }

        private void MCversionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ForgeBuild.Items.Clear();
            String selectedMcversion = MCversion.SelectedItem.ToString();
            List<String> forgeVersions = _forgeSqlHelper.GetForgeVersions(selectedMcversion);
            foreach (String build in forgeVersions)
                ForgeBuild.Items.Add(build);
            ForgeBuild.SelectedIndex = ForgeBuild.Items.Count - 1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void labelmcversion_Click(object sender, EventArgs e)
        {

        }

        private void missingInfoActionOnTheRun_CheckedChanged(object sender, EventArgs e)
        {
            _confighandler.SetConfig("missingInfoAction", missingInfoActionOnTheRun.Checked);
        }
    }
}
