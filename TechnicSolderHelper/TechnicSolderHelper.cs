using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TechnicSolderHelper.SQL;
using TechnicSolderHelper.forge;
using System.Security.Cryptography;
using System.Security;


namespace TechnicSolderHelper
{
    public partial class SolderHelper : Form
    {
        #region Application Wide Variables

        public String InputDirectory;
        public String OutputDirectory;
        public ModListSQLHelper ModsSQLhelper = new ModListSQLHelper();
        public FTBPermissionsSQLHelper FTBPermsSQLhelper = new FTBPermissionsSQLHelper();
        public OwnPermissionsSQLHelper OwnPermsSQLhelper = new OwnPermissionsSQLHelper();
        public ForgeSQLHelper forgesqlhelper = new ForgeSQLHelper();
        public liteloaderSQLHelper liteloadersqlhelper = new liteloaderSQLHelper();
        public String SevenZipLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TechnicSolderHelper", "7za.exe");
        public Process process = new System.Diagnostics.Process();
        public ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        public String UserName, path, CurrentMCVersion, ModpackVersion, ModpackName, ModpackArchive, FTBModpackArchive;
        public ConfigHandler confighandler = new ConfigHandler();
        public String modlistTextFile = "", technicPermissionList = "", FTBPermissionList = "", FTBOwnPermissionList = "";
        public short totalMods = 0, currentMod = 0;
        public String modpacksJsonFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "modpacks.json");
        public modpacks modpacks = new modpacks();
        public Dictionary<String, CheckBox> additionalDirectories = new Dictionary<string, CheckBox>();
        public Ftp ftp;

        #endregion

        public SolderHelper()
        {
            if (globalfunctions.isUnix())
            {
                globalfunctions.pathSeperator = '/';
            }
            else
            {
                globalfunctions.pathSeperator = '\\';
            }
            UserName = Environment.UserName;
            InitializeComponent();
            bool firstRun = true;
            if (globalfunctions.isUnix())
            {
                try
                {
                    firstRun = Convert.ToBoolean(confighandler.getConfig("FirstRun"));
                }
                catch (Exception)
                {
                    firstRun = true;
                }
            }
            else
            {
                firstRun = Properties.Settings.Default.FirstRun;
            }
            if (firstRun)
            {
                messageToUser m = new messageToUser();
                Thread startingThread = new Thread(new ThreadStart(m.firstTimeRun));
                startingThread.Start();
                getliteloaderversions_Click(null, null);
                if (globalfunctions.isUnix())
                {
                    confighandler.setConfig("InputDirectory", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/mods");
                    confighandler.setConfig("OutputDirectory", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/SolderHelper");
                    confighandler.setConfig("FirstRun", "false");
                }
                else
                {
                    Properties.Settings.Default.InputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\.minecraft\mods";
                    Properties.Settings.Default.OutputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\SolderHelper";
                    Properties.Settings.Default.FirstRun = false;
                    Properties.Settings.Default.Save();
                }

                #region Find MC versions

                MCversion.Items.Clear();

                forgesqlhelper.FindAllForgeVersion();
                List<String> mcversions = forgesqlhelper.getMCVersions();
                foreach (String mcversion in mcversions)
                {
                    MCversion.Items.Add(mcversion);
                }

                #endregion
                excelReader.addFTBPermissions();

            }
            #region Reload Interface
            if (globalfunctions.isUnix())
            {
                try
                {
                    InputFolder.Text = confighandler.getConfig("InputDirectory");
                }
                catch (Exception)
                {
                    InputFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/mods";
                }

                try
                {
                    OutputFolder.Text = confighandler.getConfig("OutputDirectory");
                }
                catch (Exception)
                {
                    OutputFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/SolderHelper";
                }

                try
                {
                    CreateTechnicPack.Checked = Convert.ToBoolean(confighandler.getConfig("CreateTechnicSolderFiles"));
                    SolderPackType.Visible = CreateTechnicPack.Checked;
                }
                catch (Exception)
                {
                    CreateTechnicPack.Checked = false;
                }

                try
                {
                    CreateFTBPack.Checked = Convert.ToBoolean(confighandler.getConfig("CreateFTBPack"));
                }
                catch (Exception)
                {
                    CreateFTBPack.Checked = false;
                }
            }
            else
            {
                InputFolder.Text = Properties.Settings.Default.InputDirectory.ToString();
                OutputFolder.Text = Properties.Settings.Default.OutputDirectory.ToString();
                CreateTechnicPack.Checked = Properties.Settings.Default.CreateTechnicSolderFiles;
                CreateFTBPack.Checked = Properties.Settings.Default.CreateFTBPack;
            }

            Boolean CSP = true, CPFP = true, TPP = true, IFV = false, ICZ = true, CP = false, UFTP = false;
            if (globalfunctions.isUnix())
            {
                try
                {
                    CSP = Convert.ToBoolean(confighandler.getConfig("CreateSolderPack"));
                }
                catch (Exception)
                {
                }
                try
                {
                    CPFP = Convert.ToBoolean(confighandler.getConfig("CreatePrivateFTBPack"));
                }
                catch (Exception)
                {
                }
                try
                {
                    TPP = Convert.ToBoolean(confighandler.getConfig("TechnicPrivatePermissionsLevel"));
                }
                catch (Exception)
                {
                }
                try
                {
                    IFV = Convert.ToBoolean(confighandler.getConfig("IncludeForgeVersion"));
                }
                catch (Exception)
                {
                }
                try
                {
                    ICZ = Convert.ToBoolean(confighandler.getConfig("CreateTechnicConfigZip"));
                }
                catch (Exception)
                {
                }
                try
                {
                    CP = Convert.ToBoolean(confighandler.getConfig("CheckTecnicPermissions"));
                }
                catch (Exception)
                {
                }
                try
                {
                    UFTP = Convert.ToBoolean(confighandler.getConfig("UploadToFTPServer"));
                }
                catch
                {
                }
            }
            else
            {
                CSP = Properties.Settings.Default.CreateSolderPack;
                CPFP = Properties.Settings.Default.CreatePrivateFTBPack;
                TPP = Properties.Settings.Default.TechnicPrivatePermissionsLevel;
                IFV = Properties.Settings.Default.IncludeForgeVersion;
                ICZ = Properties.Settings.Default.CreateTechnicConfigZip;
                CP = Properties.Settings.Default.CheckTecnicPermissions;
                UFTP = Properties.Settings.Default.UploadToFTPServer;
            }

            if (CSP)
            {
                ZipPack.Checked = false;
                SolderPack.Checked = true;
            }
            else
            {
                SolderPack.Checked = false;
                ZipPack.Checked = true;
            }

            if (CPFP)
            {
                PublicFTBPack.Checked = false;
                PrivateFTBPack.Checked = true;
            }
            else
            {
                PrivateFTBPack.Checked = false;
                PublicFTBPack.Checked = true;
            }
            if (TPP)
            {
                TechnicPublicPermissions.Checked = false;
                TechnicPrivatePermissions.Checked = true;
            }
            else
            {
                TechnicPrivatePermissions.Checked = false;
                TechnicPublicPermissions.Checked = true;
            }
            UploadToFTPServer.Checked = UFTP;
            if (UploadToFTPServer.Checked)
            {
                configureFTP.Show();
            }
            else
            {
                configureFTP.Hide();
            }
            IncludeForgeVersion.Checked = IFV;
            IncludeConfigZip.Checked = ICZ;
            CheckPermissions.Checked = CP;
            if (CP && CreateTechnicPack.Checked)
            {
                TechnicDistributionLevel.Visible = true;
            }
            else
            {
                TechnicDistributionLevel.Visible = false;
            }

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
            List<String> minecraftversions = forgesqlhelper.getMCVersions();
            foreach (String mcversion in minecraftversions)
            {
                MCversion.Items.Add(mcversion);
            }

            if (!CreateTechnicPack.Checked)
            {
                MCversion.Hide();
                ForgeBuild.Hide();
                labelmcversion.Hide();
                labelforgeversion.Hide();
            }

            #endregion

            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            if (globalfunctions.isUnix())
            {
                startInfo.FileName = "unzip";
            }
            else
            {
                startInfo.FileName = SevenZipLocation;
            }

            if (File.Exists(modpacksJsonFile))
            {
                String modpackJson = "";
                using (StreamReader reader = new StreamReader(modpacksJsonFile))
                {
                    modpackJson = reader.ReadToEnd();
                }
                modpacks = JsonConvert.DeserializeObject<modpacks>(modpackJson);
                foreach (String item in modpacks.modpack.Keys)
                {
                    ModpackNameInput.Items.Add(item);
                }
            }
        }

        public void createFTBPermissionInfo(String modname, String modid, String modauthor, String linkToPermission)
        {
            String output = String.Format("{0}({1}) by {2} {3}Permission: {4} {3}{3}", modname, modid, modauthor, Environment.NewLine, linkToPermission, Environment.NewLine);
            File.AppendAllText(FTBPermissionList, output);
        }

        public void createOwnPermissionInfo(String modname, String modid, String modauthor, String linkToPermission, String modLink)
        {
            String output = String.Format("{0}({1}) by {2} {3}Permission: {4} {3}Link to mod: {5}{3}{3}", modname, modid, modauthor, Environment.NewLine, linkToPermission, modLink);
            File.AppendAllText(FTBOwnPermissionList, output);
        }

        private string getAuthors(mcmod mod)
        {
            string authorString = "";
            bool isFirst = true;
            if (mod.authors != null && mod.authors.Count != 0)
            {
                foreach (string author in mod.authors)
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
                if (mod.authorList != null && mod.authorList.Count != 0)
                {
                    foreach (string author in mod.authorList)
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
                    authorString = FTBPermsSQLhelper.getInfoFromModID(mod.modid, FTBPermissionsSQLHelper.InfoType.ModAuthor);

                    if (String.IsNullOrWhiteSpace(authorString))
                    {
                        authorString = OwnPermsSQLhelper.getAuthor(mod.modid);
                        if (String.IsNullOrWhiteSpace(authorString))
                        {
                            authorString = Prompt.ShowDialog("Who is the author of " + mod.name + "?" + Environment.NewLine + "If you leave this empty the author list in the output will also be empty.", "Mod Author", false, Prompt.modsLeftString(totalMods, currentMod));

                        }
                    }
                }
            }
            OwnPermsSQLhelper.addAuthor(mod.modid, authorString);
            return authorString;
        }

        public void CreateFTBPackZip(mcmod mod, string modfile)
        {
            if (mod.isSkipping)
            {
                return;
            }
            String FileName = modfile.Substring(modfile.LastIndexOf(globalfunctions.pathSeperator) + 1);
            if (IsWierdMod(FileName) == 0)
            {
                return;
            }
            String modMD5 = SQLHelper.calculateMD5(modfile);
            if (!mod.isIgnore)
            {
                if (!mod.useShortName)
                {
                    ModsSQLhelper.addMod(mod.name, mod.modid, mod.version, mod.mcversion, FileName, modMD5, false);
                }
                if (true)
                {
                    #region Permissions checking
                    PermissionLevel PermLevel = FTBPermsSQLhelper.doFTBHavePermission(mod.modid, PublicFTBPack.Checked);

                    String overwritelink = "";
                    ownPermissions op;
                    switch (PermLevel)
                    {
                        case PermissionLevel.Open:
                            break;
                        case PermissionLevel.Notify:
                            op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                            if (!op.hasPermission)
                            {
                                overwritelink = Prompt.ShowDialog(mod.name + " requires that you notify the author of inclusion." + Environment.NewLine + "Please provide proof that you have done this:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true).Replace(" ", "");
                                while (true)
                                {
                                    if (overwritelink.ToLower().Equals("skip".ToLower()))
                                    {
                                        mod.isSkipping = true;
                                        return;
                                    }
                                    else
                                    {
                                        if (Uri.IsWellFormedUriString(overwritelink, UriKind.Absolute))
                                        {
                                            if (overwritelink.ToLower().Contains("imgur"))
                                            {
                                                OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, overwritelink);
                                                //Get Author
                                                String a = FTBPermsSQLhelper.getInfoFromModID(mod.modid, FTBPermissionsSQLHelper.InfoType.ModAuthor);
                                                createFTBPermissionInfo(mod.name, mod.modid, a, overwritelink);
                                                break;
                                            }
                                            else
                                            {
                                                MessageBox.Show("Not an imgur link");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Invalid url");
                                        }
                                        overwritelink = Prompt.ShowDialog(mod.name + " requires that you notify the author of inclusion." + Environment.NewLine + "Please provide proof that you have done this:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod)).Replace(" ", "");
                                    }
                                }
                            }
                            else
                            {
                                //Get Author
                                String a = FTBPermsSQLhelper.getInfoFromModID(mod.modid, FTBPermissionsSQLHelper.InfoType.ModAuthor);
                                createFTBPermissionInfo(mod.name, mod.modid, a, op.PermissionLink);
                            }
                            break;
                        case PermissionLevel.FTB:
                            break;
                        case PermissionLevel.Request:
                            op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                            if (!op.hasPermission)
                            {
                                overwritelink = Prompt.ShowDialog("This mod requires that you request permissions from the Mod Author of " + mod.name + Environment.NewLine + "Please provide proof that you have this permission:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod)).Replace(" ", "");
                                while (true)
                                {
                                    if (overwritelink.ToLower().Equals("skip".ToLower()))
                                    {
                                        mod.isSkipping = true;
                                        return;
                                    }
                                    else
                                    {
                                        if (Uri.IsWellFormedUriString(overwritelink, UriKind.Absolute))
                                        {
                                            if (overwritelink.ToLower().Contains("imgur"))
                                            {
                                                OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, overwritelink);
                                                //Get Author
                                                String a = FTBPermsSQLhelper.getInfoFromModID(mod.modid, FTBPermissionsSQLHelper.InfoType.ModAuthor);
                                                createFTBPermissionInfo(mod.name, mod.modid, a, overwritelink);
                                                break;
                                            }
                                            else
                                            {
                                                MessageBox.Show("Not an imgur link");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Invalid url");
                                        }
                                        overwritelink = Prompt.ShowDialog("This mod requires that you request permissions from the Mod Author of " + mod.name + Environment.NewLine + "Please provide proof that you have this permission:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod)).Replace(" ", "");
                                    }
                                }
                            }
                            else
                            {
                                //Get Author
                                String a = FTBPermsSQLhelper.getInfoFromModID(mod.modid, FTBPermissionsSQLHelper.InfoType.ModAuthor);
                                createFTBPermissionInfo(mod.name, mod.modid, a, op.PermissionLink);
                            }
                            break;
                        case PermissionLevel.Closed:
                            op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                            if (!op.hasPermission)
                            {
                                overwritelink = Prompt.ShowDialog("The FTB permissionsheet states that permissions for " + mod.name + " is closed." + Environment.NewLine + "Please provide proof that this is not the case:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod)).Replace(" ", "");
                                while (true)
                                {
                                    if (overwritelink.ToLower().Equals("skip".ToLower()))
                                    {
                                        mod.isSkipping = true;
                                        return;
                                    }
                                    else
                                    {
                                        if (Uri.IsWellFormedUriString(overwritelink, UriKind.Absolute))
                                        {
                                            if (overwritelink.ToLower().Contains("imgur"))
                                            {
                                                OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, overwritelink);
                                                //Get Author
                                                String a = FTBPermsSQLhelper.getInfoFromModID(mod.modid, FTBPermissionsSQLHelper.InfoType.ModAuthor);
                                                createFTBPermissionInfo(mod.name, mod.modid, a, overwritelink);
                                                break;
                                            }
                                            else
                                            {
                                                MessageBox.Show("Not an imgur link");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Invalid url");
                                        }
                                        overwritelink = Prompt.ShowDialog("The FTB permissionsheet states that permissions for " + mod.name + " is closed." + Environment.NewLine + "Please provide proof that this is not the case:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod)).Replace(" ", "");
                                    }
                                }
                            }
                            else
                            {
                                //Get Author
                                String a = FTBPermsSQLhelper.getInfoFromModID(mod.modid, FTBPermissionsSQLHelper.InfoType.ModAuthor);
                                createFTBPermissionInfo(mod.name, mod.modid, a, op.PermissionLink);
                            }
                            break;
                        case PermissionLevel.Unknown:
                            String modLink = "";
                            op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                            if (!op.hasPermission)
                            {
                                overwritelink = Prompt.ShowDialog("Permissions for " + mod.name + " is unknown" + Environment.NewLine + "Please provide proof of permissions:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod)).Replace(" ", "");
                                while (true)
                                {
                                    if (overwritelink.ToLower().Equals("skip".ToLower()))
                                    {
                                        mod.isSkipping = true;
                                        return;
                                    }
                                    else
                                    {
                                        if (Uri.IsWellFormedUriString(overwritelink, UriKind.Absolute))
                                        {
                                            if (overwritelink.ToLower().Contains("imgur"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                MessageBox.Show("Not an imgur link");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Invalid url");
                                        }
                                        overwritelink = Prompt.ShowDialog("Permissions for " + mod.name + " is unknown" + Environment.NewLine + "Please provide proof of permissions:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod)).Replace(" ", "");
                                    }
                                }
                                while (true)
                                {
                                    modLink = Prompt.ShowDialog("Please provide a link to " + mod.name + ":" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true).Replace(" ", "");
                                    if (modLink.ToLower().Equals("skip".ToLower()))
                                    {
                                        mod.isSkipping = true;
                                        return;
                                    }
                                    else
                                    {
                                        if (Uri.IsWellFormedUriString(modLink, UriKind.Absolute))
                                        {
                                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, overwritelink, modLink);
                                            break;

                                        }
                                        else
                                        {
                                            MessageBox.Show("Invalid url");
                                        }
                                        modLink = Prompt.ShowDialog("Please provide a link to " + mod.name + ":" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod)).Replace(" ", "");
                                    }
                                }
                                String a = getAuthors(mod);
                                createOwnPermissionInfo(mod.name, mod.modid, a, overwritelink, modLink);

                            }
                            else
                            {
                                String a = getAuthors(mod);
                                createOwnPermissionInfo(mod.name, mod.modid, a, op.PermissionLink, op.ModLink);
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
            }

            if (String.IsNullOrWhiteSpace(FTBModpackArchive))
            {
                while (String.IsNullOrWhiteSpace(ModpackName))
                {
                    ModpackName = Prompt.ShowDialog("What is the Modpack Name?", "Modpack Name");
                }
                while (String.IsNullOrWhiteSpace(ModpackVersion))
                {
                    ModpackVersion = Prompt.ShowDialog("What Version is the modpack?", "Modpack Version");
                }
                if (String.IsNullOrWhiteSpace(ModpackArchive))
                {
                    ModpackArchive = Path.Combine(OutputDirectory, String.Format("{0}-{1}.zip", ModpackName, ModpackVersion));
                    FTBModpackArchive = Path.Combine(OutputDirectory, ModpackName + "-" + ModpackVersion + "-FTB" + ".zip");
                }

            }


            String tempModDirectory = Path.Combine(OutputDirectory, "minecraft", "mods");
            Directory.CreateDirectory(tempModDirectory);
            String tempFile = Path.Combine(tempModDirectory, FileName);
            int index = tempFile.LastIndexOf(globalfunctions.pathSeperator);
            String tempFileDirectory = tempFile.Remove(index);
            Directory.CreateDirectory(tempFileDirectory);
            File.Copy(modfile, tempFile, true);

            if (globalfunctions.isUnix())
            {
                Environment.CurrentDirectory = OutputDirectory;
                startInfo.FileName = "zip";
                startInfo.Arguments = String.Format("-r \"{0}\" \"{1}\"", FTBModpackArchive, "minecraft");
            }
            else
            {
                Environment.CurrentDirectory = OutputDirectory;
                startInfo.Arguments = String.Format("a -y \"{0}\" \"{1}\"", FTBModpackArchive, "minecraft");
            }

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            Directory.Delete(tempModDirectory, true);

            if (!mod.hasBeenWritenToModlist)
            {
                File.AppendAllText(modlistTextFile, mod.name + Environment.NewLine);
                mod.hasBeenWritenToModlist = true;
            }
        }

        public void Start()
        {
            InputDirectory = InputFolder.Text;
            OutputDirectory = OutputFolder.Text;
            FTBOwnPermissionList = Path.Combine(OutputDirectory, "Own Permission List.txt");
            FTBPermissionList = Path.Combine(OutputDirectory, "FTB Permission List.txt");
            technicPermissionList = Path.Combine(OutputDirectory, "Technic Permission List.txt");

            if (globalfunctions.isUnix())
            {
                Environment.CurrentDirectory = "/";
            }
            else
            {
                Environment.CurrentDirectory = "C:\\";
            }
            if (!Directory.Exists(InputFolder.Text))
            {
                MessageBox.Show("Input directory does not exist!");
                return;
            }
            if (checkBox1.Checked)
            {
                if (Directory.Exists(OutputDirectory))
                {
                    try
                    {
                        Directory.Delete(OutputDirectory, true);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unable to clear solderDirectory." + Environment.NewLine + "Please restart the process when the directory is no longer in use.");
                        return;
                    }
                }
            }
            if (UploadToFTPServer.Checked)
            {
                String test = "";
                if (globalfunctions.isUnix())
                {
                    test = confighandler.getConfig("ftpUrl");
                }
                else
                {
                    test = Properties.Settings.Default.ftpUrl;
                }
                if (String.IsNullOrWhiteSpace(test))
                {
                    MessageBox.Show("You do not have an uploadurl set for your FTP server.");
                    return;
                }
                if (globalfunctions.isUnix())
                {
                    test = confighandler.getConfig("ftpUserName");
                }
                else
                {
                    test = Properties.Settings.Default.ftpUserName;
                }
                if (String.IsNullOrWhiteSpace(test))
                {
                    MessageBox.Show("You do not have an username set for your FTP server.");
                    return;
                }
                if (globalfunctions.isUnix())
                {
                    test = confighandler.getConfig("ftpPassword");
                }
                else
                {
                    test = Properties.Settings.Default.ftpPassword;
                }
                if (String.IsNullOrWhiteSpace(test))
                {
                    MessageBox.Show("You do not have an password set for your FTP server.");
                    return;
                }

            }

            if (CreateTechnicPack.Checked && IncludeForgeVersion.Checked)
            {
                if (MCversion.SelectedItem == null)
                {
                    MessageBox.Show("You have choosen to include Minecraft Forge, but you haven't selected a Minecraft Version.");
                    return;
                }
                if (ForgeBuild.SelectedItem == null)
                {
                    MessageBox.Show("You have choosen to include Minecraft Forge, but you haven't selected a Forge build to include.");
                    return;
                }
            }

            if (File.Exists(FTBOwnPermissionList))
            {
                File.Delete(FTBOwnPermissionList);
            }
            if (File.Exists(FTBPermissionList))
            {
                File.Delete(FTBPermissionList);
            }
            if (File.Exists(technicPermissionList))
            {
                File.Delete(technicPermissionList);
            }

            ModpackName = ModpackNameInput.Text;
            ModpackVersion = null;
            ModpackVersion = ModpackVersionInput.Text;

            if (!String.IsNullOrWhiteSpace(ModpackName))
            {
                modpacks = new modpacks();
                if (!ModpackNameInput.Items.Contains(ModpackName))
                {
                    ModpackNameInput.Items.Add(ModpackName);
                }
                foreach (String item in ModpackNameInput.Items)
                {
                    if (modpacks.modpack == null)
                    {
                        modpacks.modpack = new Dictionary<string, List<string>>();
                    }
                    if (!modpacks.modpack.ContainsKey(item))
                    {
                        modpacks.modpack.Add(item, null);
                    }
                }
                String tmpJson = JsonConvert.SerializeObject(modpacks, Formatting.Indented);
                File.WriteAllText(modpacksJsonFile, tmpJson);
            }


            //Download 7zip dependancy
            if (!globalfunctions.isUnix())
            {
                if (!(Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechnicSolderHelper")))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechnicSolderHelper");
                }
            }
            if (!(globalfunctions.isUnix() || File.Exists(SevenZipLocation)))
            {
                WebClient wb = new WebClient();
                System.Uri SevenWeb = new Uri("http://cloud.zlepper.dk/7za.exe");
                wb.DownloadFile(SevenWeb, SevenZipLocation);
            }

            if (globalfunctions.isUnix())
            {
                confighandler.setConfig("InputDirectory", InputFolder.Text);
                confighandler.setConfig("OutputDirectory", OutputFolder.Text);
            }
            else
            {
                Properties.Settings.Default.InputDirectory = InputFolder.Text;
                Properties.Settings.Default.OutputDirectory = OutputFolder.Text;
                Properties.Settings.Default.Save();
            }

            path = Path.Combine(OutputDirectory, "mods.html");


            Directory.CreateDirectory(OutputDirectory);
            Environment.CurrentDirectory = OutputDirectory;
            modlistTextFile = Path.Combine(OutputDirectory, "modlist.txt");
            if (File.Exists(modlistTextFile))
            {
                File.Delete(modlistTextFile);
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
                File.WriteAllText(path, htmlfile);
            }


            // Create array with all the mod locations
            List<String> files = new List<String>();
            totalMods = 0;
            currentMod = 0;
            // Add the different mod files to the files array
            foreach (String file in Directory.GetFiles(InputDirectory, "*.zip", SearchOption.AllDirectories))
            {
                files.Add(file);
                totalMods++;
            }
            foreach (String file in Directory.GetFiles(InputDirectory, "*.jar", SearchOption.AllDirectories))
            {
                files.Add(file);
                totalMods++;
            }
            foreach (String file in Directory.GetFiles(InputDirectory, "*.litemod", SearchOption.AllDirectories))
            {
                files.Add(file);
                totalMods++;
            }
            foreach (String file in Directory.GetFiles(InputDirectory, "*.disabled", SearchOption.AllDirectories))
            {
                files.Add(file);
                totalMods++;
            }
            while (String.IsNullOrWhiteSpace(ModpackName))
            {
                ModpackName = Prompt.ShowDialog("What is the Modpack Name?", "Modpack Name");
            }
            while (String.IsNullOrWhiteSpace(ModpackVersion))
            {
                ModpackVersion = Prompt.ShowDialog("What Version is the modpack?", "Modpack Version");
            }
            if (String.IsNullOrWhiteSpace(ModpackArchive))
            {
                ModpackArchive = Path.Combine(OutputDirectory, String.Format("{0}-{1}.zip", ModpackName, ModpackVersion));
            }
            FTBModpackArchive = Path.Combine(OutputDirectory, ModpackName + "-" + ModpackVersion + "-FTB" + ".zip");

            //Check if files have already been added
            foreach (String file in files)
            {
                currentMod++;
                if (IsWierdMod(file) == 0)
                {
                    continue;
                }
                String FileName = file.Substring(file.LastIndexOf(globalfunctions.pathSeperator) + 1);
                ProgressLabel.Text = FileName;
                //Check for mcmod.info
                Directory.CreateDirectory(OutputDirectory);
                String Arguments = "";
                if (globalfunctions.isUnix())
                {
                    startInfo.FileName = "unzip";
                    Arguments = "-o \"" + file + "\" \"*.info\" \"*.json\" -d \"" + OutputDirectory + "\"";
                }
                else
                {
                    Arguments = "e " + "-y -o\"" + OutputDirectory + "\" \"" + file + "\" *.info litemod.json";
                }
                startInfo.Arguments = Arguments;

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                String mcmodfile = Path.Combine(OutputDirectory, "mcmod.info");
                String litemodfile = Path.Combine(OutputDirectory, "litemod.json");
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
                    foreach (String modinfofile in Directory.GetFiles(OutputDirectory, "*.info"))
                    {
                        if (modinfofile.ToLower().Contains("dependancies") || modinfofile.ToLower().Contains("dependencies"))
                        {
                            File.Delete(modinfofile);
                        }
                        else
                        {
                            if (!File.Exists(mcmodfile))
                            {
                                File.Move(modinfofile, mcmodfile);
                            }
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
                    String json = "";
                    using (StreamReader r = new StreamReader(mcmodfile))
                    {
                        json = r.ReadToEnd();
                    }
                    try
                    {
                        try
                        {
                            mcmod2 modinfo2 = null;
                            try
                            {
                                modinfo2 = JsonConvert.DeserializeObject<mcmod2>(json);
                            }
                            catch (Newtonsoft.Json.JsonReaderException)
                            {
                                MessageBox.Show("Something is wrong with the Json in " + FileName);
                                throw new JsonSerializationException("Invalid Json in file" + FileName);
                            }

                            mcmod mod = new mcmod();

                            if (modinfo2.modinfoversion != 0 && modinfo2.modinfoversion == 2 || modinfo2.modListVersion != 0 && modinfo2.modListVersion == 2)
                            {
                                mod.mcversion = modinfo2.modlist[0].mcversion.ToString();
                                mod.modid = modinfo2.modlist[0].modid.ToString();
                                mod.name = modinfo2.modlist[0].name.ToString();
                                mod.version = modinfo2.modlist[0].version.ToString();
                                requireUserInfo(mod, file);
                            }
                            else
                            {
                                throw new JsonSerializationException();
                            }
                        }
                        catch (Newtonsoft.Json.JsonSerializationException)
                        {
                            try
                            {
                                mcmod mod = new mcmod();
                                List<mcmod> modinfo = null;
                                try
                                {
                                    modinfo = JsonConvert.DeserializeObject<List<mcmod>>(json);
                                }
                                catch (Newtonsoft.Json.JsonReaderException)
                                {
                                    MessageBox.Show("Something is wrong with the Json in " + FileName);
                                    throw new JsonSerializationException("Invalid Json in file" + FileName);
                                }
                                mod = modinfo[0];

                                if (file.ToLower().Contains("mekanism"))
                                {
                                    mod = ModHelper.GoodVersioning(FileName);
                                    requireUserInfo(mod, file);
                                }
                                else
                                {
                                    if (mod.modid.ToLower().StartsWith("mystcraft"))
                                    {
                                        mod = ModHelper.GoodVersioning(FileName);
                                        requireUserInfo(mod, file);
                                    }
                                    else
                                    {
                                        if (isFullyInformed(mod))
                                        {
                                            if (CreateTechnicPack.Checked)
                                            {
                                                CreateTechnicModZip(mod, file);
                                            }
                                            if (CreateFTBPack.Checked)
                                            {
                                                CreateFTBPackZip(mod, file);
                                            }
                                        }
                                        else
                                        {
                                            requireUserInfo(mod, file);
                                        }
                                    }
                                }
                            }
                            catch (Newtonsoft.Json.JsonSerializationException)
                            {
                                litemod liteloadermod = null;

                                try
                                {
                                    liteloadermod = JsonConvert.DeserializeObject<litemod>(json);
                                }
                                catch (Newtonsoft.Json.JsonReaderException)
                                {
                                    MessageBox.Show("Something is wrong with the Json in " + FileName);
                                    throw new JsonSerializationException("Invalid Json in file" + FileName);
                                }
                                //Convert into mcmod
                                mcmod mod = new mcmod();
                                mod.mcversion = liteloadermod.mcversion;
                                mod.modid = liteloadermod.name.Replace(" ", "");
                                mod.name = liteloadermod.name;
                                mod.authors = new List<string>();
                                mod.authors.Add(liteloadermod.author);

                                if (String.IsNullOrEmpty(liteloadermod.version) || String.IsNullOrEmpty(liteloadermod.revision))
                                {
                                    if (!(String.IsNullOrEmpty(liteloadermod.version)))
                                    {
                                        mod.version = liteloadermod.version;
                                    }
                                    else
                                    {
                                        if (!(String.IsNullOrEmpty(liteloadermod.revision)))
                                        {
                                            mod.version = liteloadermod.revision;
                                        }
                                    }
                                }
                                else
                                {
                                    mod.version = liteloadermod.version + "-" + liteloadermod.revision;
                                }

                                requireUserInfo(mod, file);

                            }
                        }

                    }
                    catch (Newtonsoft.Json.JsonSerializationException)
                    {
                        requireUserInfo(file);
                    }
                    File.Delete(mcmodfile);
                }
                else
                {
                    String fileName = file.Substring(file.LastIndexOf(globalfunctions.pathSeperator) + 1);

                    //Check the FTB permission sheet for info before doing anything else
                    String shortname = FTBPermsSQLhelper.getShortName(SQLHelper.calculateMD5(file));
                    if (String.IsNullOrWhiteSpace(shortname))
                    {
                        int fixNr = IsWierdMod(fileName);
                        if (fixNr != Int32.MaxValue)
                        {
                            mcmod mod;
                            switch (fixNr)
                            {
                            //Reikas Pattern
                                case 1:
                                    mod = ModHelper.ReikasMods(fileName);
                                    requireUserInfo(mod, file);
                                    break;
                                case 2:
                                    mod = ModHelper.wailaPattern(fileName);
                                    requireUserInfo(mod, file);
                                    break;
                                case 3:
                                    liteloaderversion llversion = liteloadersqlhelper.getInfo(SQLHelper.calculateMD5(file));
                                    mod = new mcmod();
                                    mod.mcversion = llversion.mcversion;
                                    mod.name = "Liteloader";
                                    mod.modid = llversion.tweakClass;
                                    try
                                    {
                                        mod.version = llversion.version.Substring(llversion.version.LastIndexOf("_") + 1);
                                    }
                                    catch (System.NullReferenceException)
                                    {
                                        mod.version = 1.ToString();
                                    }
                                    requireUserInfo(mod, file);
                                    break;
                                case 0:
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            requireUserInfo(file);
                        }
                    }
                    else
                    {
                        mcmod mod = new mcmod();
                        if (shortname.Equals("ignore"))
                        {
                            mod.isIgnore = true;
                        }
                        else
                        {
                            mod.isIgnore = false;
                            mod.useShortName = true;
                            mod.modid = shortname;
                            mod.name = FTBPermsSQLhelper.getInfoFromShortName(shortname, FTBPermissionsSQLHelper.InfoType.ModName);
                            mod.authors = new List<string>();
                            mod.authors.Add(FTBPermsSQLhelper.getInfoFromModID(shortname, FTBPermissionsSQLHelper.InfoType.ModAuthor));
                            mod.authorList = mod.authors;
                            mod.privatePerms = FTBPermsSQLhelper.doFTBHavePermission(shortname, false);
                            mod.publicPerms = FTBPermsSQLhelper.doFTBHavePermission(shortname, true);
                        }

                        if (CreateFTBPack.Checked && !CreateTechnicPack.Checked)
                        {
                            CreateFTBPackZip(mod, file);
                        }
                        else
                        {
                            if (CreateFTBPack.Checked || CreateTechnicPack.Checked)
                            {
                                requireUserInfo(mod, file);
                            }
                        }
                    }
                }
            }

            Environment.CurrentDirectory = InputDirectory;
            String[] Directories = Directory.GetDirectories(InputDirectory);
            String minecraftVersionPattern = @"^[0-9]{1}\.[0-9]{1}\.[0-9]{1,2}$";
            foreach (String dir in Directories)
            {
                String dirName = dir.Substring(dir.LastIndexOf(globalfunctions.pathSeperator) + 1);
                if (Regex.IsMatch(dirName, minecraftVersionPattern, RegexOptions.Multiline))
                {
                    continue;
                }
                String[] jarFiles = Directory.GetFiles(dir, "*.jar", SearchOption.AllDirectories);
                if (jarFiles.Length != 0)
                {
                    continue;
                }
                String levelOverInputDirectory = InputDirectory.Remove(InputDirectory.LastIndexOf(globalfunctions.pathSeperator));
				
                DialogResult confirmInclude = MessageBox.Show("Do you want to include " + dirName + "?",
                                                  "Additional folder found", MessageBoxButtons.YesNo);
                if (confirmInclude == DialogResult.Yes)
                {
                    Environment.CurrentDirectory = levelOverInputDirectory;
                    if (CreateTechnicPack.Checked)
                    {
                        //Create Technic Pack
                        if (SolderPack.Checked)
                        {
                            List<String> md5values = new List<string>();
                            List<String> oldmd5values = new List<string>();
                            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "unarchievedFiles"));
                            String md5valuesFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "unarchievedFiles", dirName + ".txt");
                            if (File.Exists(md5valuesFile))
                            {
                                using (StreamReader reader = new StreamReader(md5valuesFile))
                                {
                                    while (true)
                                    {
                                        String tmp = reader.ReadLine();
                                        if (String.IsNullOrWhiteSpace(tmp))
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            oldmd5values.Add(tmp);
                                        }
                                    }
                                }
                            }

                            bool same = true;
                            foreach (String f in Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories))
                            {
                                if (!oldmd5values.Contains(SQLHelper.calculateMD5(f)))
                                {
                                    same = false;
                                }
                                md5values.Add(SQLHelper.calculateMD5(f));
                            }
                            if (same)
                            {
                                continue;
                            }
                            else
                            {
                                while (String.IsNullOrWhiteSpace(ModpackVersion))
                                {
                                    ModpackVersion = Prompt.ShowDialog("What Version is the modpack?", "Modpack Version");
                                }
                                //So we need to include this folder
                                Environment.CurrentDirectory = levelOverInputDirectory;
                                String outputfile = Path.Combine(OutputDirectory, dirName.ToLower(), dirName.ToLower() + "-" + ModpackName + "-" + ModpackVersion + ".zip");
                                Directory.CreateDirectory(Path.Combine(OutputDirectory, dirName.ToLower()));
                                if (globalfunctions.isUnix())
                                {
                                    startInfo.FileName = "zip";
                                    startInfo.Arguments = String.Format("-r \"{0}\" mods/{1}", outputfile, dirName);
                                }
                                else
                                {
                                    startInfo.FileName = SevenZipLocation;
                                    startInfo.Arguments = String.Format("a -y \"{0}\" \"mods\\{1}\"", outputfile, dirName);
                                }
                                process.StartInfo = startInfo;
                                process.Start();
                                createTableRow(dirName, dirName.ToLower(), ModpackName.ToLower() + "-" + ModpackVersion.ToLower());
                                process.WaitForExit();

                            }
                            if (File.Exists(md5valuesFile))
                            {
                                File.Delete(md5valuesFile);
                            }
                            foreach (String md5value in md5values)
                            {
                                File.AppendAllText(md5valuesFile, md5value + Environment.NewLine);
                            }
                        }
                        else
                        {
                            if (globalfunctions.isUnix())
                            {
                                startInfo.FileName = "zip";
                                startInfo.Arguments = String.Format("-r \"{0}\" \"./mods/{1}\"", ModpackArchive, dirName);
                            }
                            else
                            {
                                startInfo.FileName = SevenZipLocation;
                                startInfo.Arguments = String.Format("a -y \"{0}\" \"mods\\{1}\"", ModpackArchive, dirName);
                            }
                            process.StartInfo = startInfo;
                            process.Start();
                            process.WaitForExit();
                        }
                    } 
                    if (CreateFTBPack.Checked)
                    {
                        // Create FTB Pack

                        String tmpDir = Path.Combine(OutputDirectory, "minecraft", "mods");
                        Directory.CreateDirectory(tmpDir);
                        if (globalfunctions.isUnix())
                        {
                            startInfo.FileName = "cp";
                            startInfo.Arguments = String.Format("-r \"./mods/{0}\" \"{1}\"", dirName, tmpDir);
                            process.StartInfo = startInfo;
                            process.Start();
                            process.WaitForExit();
                        }
                        else
                        {
                            String input = Path.Combine(InputDirectory, dirName);
                            DirectoryCopy(input, tmpDir, true);
                        }
                    }
                }
				
            }

            // Pack additional folders if they are marked
            if (CreateTechnicPack.Checked)
            {
                Environment.CurrentDirectory = InputDirectory.Remove(InputDirectory.LastIndexOf(globalfunctions.pathSeperator));
                foreach (KeyValuePair<String, CheckBox> cb in additionalDirectories)
                {
                    if (cb.Value.Checked)
                    {
                        String folderName = cb.Key.Substring(cb.Key.LastIndexOf(globalfunctions.pathSeperator) + 1);
                        if (SolderPack.Checked)
                        {
                            String of = Path.Combine(OutputDirectory, folderName);
                            Directory.CreateDirectory(of);
                            String outputfile = Path.Combine(of, folderName + "-" + ModpackName + "-" + ModpackVersion + ".zip");
                            if (globalfunctions.isUnix())
                            {
                                startInfo.FileName = "zip";
                                startInfo.Arguments = String.Format("-r \"{0}\" {1}", outputfile, folderName);
                            }
                            else
                            {
                                startInfo.FileName = SevenZipLocation;
                                startInfo.Arguments = String.Format("a -y \"{0}\" \"{1}\"", outputfile, folderName);
                            }
                            process.StartInfo = startInfo;
                            process.Start();
                            process.WaitForExit();
                            createTableRow(folderName, folderName.ToLower(), ModpackName + "-" + ModpackVersion);
                        }
                        else
                        {

                            if (globalfunctions.isUnix())
                            {
                                startInfo.FileName = "zip";
                                startInfo.Arguments = String.Format("-r \"{0}\" \"{1}\"", ModpackArchive, folderName);
                            }
                            else
                            {
                                startInfo.FileName = SevenZipLocation;
                                startInfo.Arguments = String.Format("a -y \"{0}\" \"{1}\"", ModpackArchive, folderName);
                            }
                            process.StartInfo = startInfo;
                            process.Start();
                            process.WaitForExit();
                        }
                    }
                }
            }



            if (CreateTechnicPack.Checked && IncludeConfigZip.Checked)
            {
                createConfigZip();
            }

            //FTB pack configs
            if (CreateFTBPack.Checked)
            {
                foreach (KeyValuePair<String, CheckBox> cb in additionalDirectories)
                {
                    if (cb.Value.Checked)
                    {
                        String dirName = cb.Key.Substring(cb.Key.LastIndexOf(globalfunctions.pathSeperator) + 1);
                        String tmpDir = Path.Combine(OutputDirectory, "minecraft");

                        Directory.CreateDirectory(tmpDir);
                        if (globalfunctions.isUnix())
                        {
                            startInfo.FileName = "cp";
                            startInfo.Arguments = String.Format("-r \"{0}\" \"{1}\"", dirName, tmpDir);
                            process.StartInfo = startInfo;
                            process.Start();
                            process.WaitForExit();
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

                String tmpConfigDirectory = Path.Combine(OutputDirectory, Path.Combine("minecraft", "config"));
                Directory.CreateDirectory(tmpConfigDirectory);

                String sourceConfigDirectory = InputFolder.Text.Replace(globalfunctions.pathSeperator + "mods", globalfunctions.pathSeperator + "config");
                try
                {
                    DirectoryCopy(sourceConfigDirectory, tmpConfigDirectory, true);
                }
                catch (System.IO.DirectoryNotFoundException)
                {
                    MessageBox.Show("I can't seem to find a config directory for the FTB pack.");
                }

                Environment.CurrentDirectory = OutputDirectory;
                if (globalfunctions.isUnix())
                {
                    startInfo.FileName = "zip";
                    startInfo.Arguments = String.Format("-r \"{0}\" \"minecraft\" -x minecraft/config/YAMPST.nbt", FTBModpackArchive);
                }
                else
                {
                    startInfo.Arguments = "a -x YAMPST.nbt -y \"" + FTBModpackArchive + "\" \"minecraft\" ";
                }

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                Directory.Delete(Path.Combine(OutputDirectory, "minecraft"), true);
            }

            if (CreateTechnicPack.Checked && IncludeForgeVersion.Checked)
            {
                string selectedBuild = ForgeBuild.SelectedItem.ToString();
                Number forgeinfo = forgesqlhelper.getForgeInfo(selectedBuild);
                String tmpdir = Path.Combine(OutputDirectory, "bin");
                Directory.CreateDirectory(tmpdir);
                String tempfile = Path.Combine(tmpdir, "modpack.jar");

                WebClient wb = new WebClient();
                wb.DownloadFile(forgeinfo.downloadurl, tempfile);
                if (SolderPack.Checked)
                {
                    Directory.CreateDirectory(Path.Combine(OutputDirectory, "forge"));
                    String outputfile = Path.Combine(OutputDirectory, "forge", "forge-" + forgeinfo.version + ".zip");
                    if (globalfunctions.isUnix())
                    {
                        startInfo.FileName = "zip";
                        Environment.CurrentDirectory = OutputDirectory;
                        startInfo.Arguments = "-r \"" + outputfile + "\" \"bin\"";
                    }
                    else
                    {
                        startInfo.Arguments = "a -y \"" + outputfile + "\" \"" + tmpdir + "\"";
                    }
                    createTableRow("Minecraft Forge", "forge", forgeinfo.version.ToLower());
                }
                else
                {
                    if (globalfunctions.isUnix())
                    {
                        Environment.CurrentDirectory = OutputDirectory;
                        startInfo.FileName = "zip";
                        startInfo.Arguments = "-r \"" + ModpackArchive + "\" \"bin\"";
                    }
                    else
                    {
                        startInfo.Arguments = "a -y \"" + ModpackArchive + "\" \"" + tmpdir + "\"";
                    }
                }
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                Directory.Delete(tmpdir, true);
            }

            if (Directory.Exists(Path.Combine(OutputDirectory, "assets")))
            {
                Directory.Delete(Path.Combine(OutputDirectory, "assets"), true);
            }
            if (Directory.Exists(Path.Combine(OutputDirectory, "example")))
            {
                Directory.Delete(Path.Combine(OutputDirectory, "example"), true);
            }

            if (CreateTechnicPack.Checked && SolderPack.Checked)
            {
                File.AppendAllText(path, "</table><button id=\"Reshow\" type=\"button\">Unhide Everything</button><p>List autogenerated by TechnicSolderHelper &copy; 2014 - Rasmus Hansen</p></body></html>");
                if (globalfunctions.isUnix())
                {
                    Process.Start(path);
                }
                else
                {
                    try
                    {
                        Process.Start("chrome.exe", path);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            Process.Start("iexplore", path);
                        }
                        catch (Exception)
                        {
                            try
                            {
                                Process.Start("firefox.exe", path);
                            }
                            catch (Exception)
                            {
                                Process.Start(path);
                            }
                        }
                    }
                }

            }
            if (CreateTechnicPack.Checked && SolderPack.Checked && UploadToFTPServer.Checked)
            {
                ProgressLabel.Text = "Uploading to FTP Server";
                if (ftp == null)
                {
                    ftp = new Ftp();
                }

                messageToUser m = new messageToUser();
                Thread startingThread = new Thread(new ThreadStart(m.uploadingToFTP));
                startingThread.Start();

                ftp.uploadFolder(Path.Combine(OutputDirectory, "mods"));

            }
            ProgressLabel.Text = "Waiting...";

        }

        #region Technic Pack Function

        private void createTableRow(String firstColumn, String secondColumn, String thirdColumn)
        {
            String AddedMod = "<tr>";
            AddedMod += String.Format("<td><input readonly class=\"containsInfo\" value=\"{0}\"></td>", firstColumn);
            AddedMod += String.Format("<td><input readonly class=\"containsInfo\" value=\"{0}\"></td>", secondColumn);
            AddedMod += String.Format("<td><input readonly class=\"containsInfo\" value=\"{0}\"></td>", thirdColumn);
            AddedMod += "<td><button class=\"Hide\" type=\"button\">Hide</button></td></tr>";
            File.AppendAllText(path, AddedMod + Environment.NewLine);
        }

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
            for (int i = 0; i < skipMods.Length; i++)
            {
                if (modFileName.ToLower().Contains(skipMods[i].ToLower()))
                {
                    //Return zero to indicate a mod that needs to be skipped
                    return 0;
                }
            }
            String[] ModPatterns =
                {@"[a-z]+ 1.[0-9].[0-9]* V[0-9]*[a-z]*",
                    @"[a-z]+-[0-9.]+_[0-9.]+",
                    @"liteloader"
                };
            for (int i = 0; i < ModPatterns.Length; i++)
            {
                if (Regex.IsMatch(modFileName, ModPatterns[i], RegexOptions.IgnoreCase))
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
        private static bool isFullyInformed(mcmod mod)
        {
            if (String.IsNullOrWhiteSpace(mod.name))
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(mod.version))
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(mod.mcversion))
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(mod.modid))
            {
                return false;
            }
            return true;
        }

        public void createConfigZip()
        {
            if (SolderPack.Checked)
            {
                String InputDirectory = InputFolder.Text;
                InputDirectory = InputDirectory.Replace(globalfunctions.pathSeperator + "mods", "");

                OutputDirectory = OutputFolder.Text;
                String ConfigFileName = "";
                if (ModpackName == null)
                {
                    ConfigFileName = Prompt.ShowDialog("What do you want the file name of the config " + Environment.NewLine + "folder to be?", "Config FileInfo Name");
                }
                else
                {
                    ConfigFileName = ModpackName + "-configs";
                }
                String ConfigVersion = "";
                if (ModpackVersion == null)
                {
                    ConfigVersion = Prompt.ShowDialog("What is the config version?", "Config Version");
                }
                else
                {
                    ConfigVersion = ModpackVersion;  
                }
                String ConfigFileZipName = ConfigFileName + "-" + ConfigVersion;
                if (!(ConfigFileZipName.EndsWith(".zip")))
                {
                    ConfigFileZipName = ConfigFileZipName + ".zip";
                }
                if (globalfunctions.isUnix())
                {
                    startInfo.FileName = "zip";
                    Directory.CreateDirectory(OutputDirectory + "/" + ConfigFileName);
                    Environment.CurrentDirectory = InputDirectory;
                    startInfo.Arguments = "-r \"" + OutputDirectory + "/mods/" + ConfigFileName + "/" + ConfigFileZipName + "\" \"config\" -x config/YAMPST.nbt";
                }
                else
                {
                    startInfo.Arguments = "a -x config\\YAMPST.nbt -y \"" + OutputDirectory + "\\mods\\" + ConfigFileName + "\\" + ConfigFileZipName + "\" \"" + InputDirectory + "\\config" + "\"";
                }
                startInfo.Arguments = startInfo.Arguments;
                process.StartInfo = startInfo;
                process.Start();

                createTableRow(ConfigFileName, ConfigFileName, ConfigVersion.ToLower());

                process.WaitForExit();


            }
            else
            {
                if (globalfunctions.isUnix())
                {
                    Environment.CurrentDirectory = InputDirectory.Remove(InputDirectory.LastIndexOf(globalfunctions.pathSeperator));
                    startInfo.FileName = "zip";
                    startInfo.Arguments = String.Format("-r \"{0}\" \"config\" -x config/YAMPST.nbt", ModpackArchive);
                }
                else
                {
                    String Input = InputFolder.Text;
                    Input = InputDirectory.Replace("\\mods", "\\config");
                    startInfo.Arguments = "a -x config\\YAMPST.nbt -y \"" + ModpackArchive + "\" \"" + Input + "\"";
                }
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }


        }

        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                Directory.CreateDirectory(destDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            System.IO.FileInfo[] files = dir.GetFiles();
            foreach (System.IO.FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
            
        }

        public void requireUserInfo(mcmod currentData, String File)
        {

            mcmod mod = new mcmod();
            mod.useShortName = false;
            try
            {
                mod = ModsSQLhelper.getModInfo(SQLHelper.calculateMD5(File));
            }
            catch (Exception)
            {
            }

            String FileName = File.Substring(File.LastIndexOf(globalfunctions.pathSeperator) + 1);
            FileName = FileName.Remove(FileName.LastIndexOf("."));

            if (currentData.name != null)
            {
                if (currentData.name.Equals("Mystcraft"))
                {
                    mod.version = ModHelper.GoodVersioning(FileName).version;
                    mod.mcversion = ModHelper.GoodVersioning(FileName).mcversion;
                }
            }


            if (currentData.name != null)
            {
                mod.name = currentData.name;

            }
            else
            {
                if (mod.name == null)
                {
                    String a = "Mod name of " + FileName + Environment.NewLine + "Go bug the mod author to include an mcmod.info file!";
                    mod.name = Prompt.ShowDialog(a, "Mod Name", false, Prompt.modsLeftString(totalMods, currentMod));
                    if (mod.name.Equals(""))
                    {
                        return;
                    }
                }

            }

            if (currentData.version != null)
            {
                mod.version = currentData.version.Replace(" ", "+").ToLower();
            }
            else
            {
                if (mod.version == null)
                {
                    String a = String.Format("Mod version of {0}" + Environment.NewLine + "Go bug the mod author to include an mcmod.info file!", FileName);
                    mod.version = Prompt.ShowDialog(a, "Mod Version", false, Prompt.modsLeftString(totalMods, currentMod));
                    mod.version = mod.version.Replace(" ", "+").ToLower();
                }

            }

            if (currentData.mcversion != null)
            {
                mod.mcversion = currentData.mcversion;
            }
            else
            {
                if (mod.mcversion == null)
                {
                    if (CurrentMCVersion == null)
                    {
                        String a = String.Format("Minecraft Version of {0}" + Environment.NewLine + "Go bug the mod author to include an mcmod.info file!", FileName);
                        mod.mcversion = Prompt.ShowDialog(a, "Minecraft Version", false, Prompt.modsLeftString(totalMods, currentMod));
                        CurrentMCVersion = mod.mcversion;
                    }
                    else
                    {
                        mod.mcversion = CurrentMCVersion;
                    }
                }
            }


            if (currentData.modid != null)
            {
                mod.modid = currentData.modid;
            }
            else
            {
                mod.modid = mod.name.Replace(" ", "").ToLower();
            }

            if (CreateTechnicPack.Checked)
            {
                CreateTechnicModZip(mod, File);
            }
            if (CreateFTBPack.Checked)
            {
                CreateFTBPackZip(mod, File);
            }
        }

        public void requireUserInfo(String file)
        {
            mcmod mod = new mcmod();
            mod.mcversion = null;
            mod.modid = null;
            mod.name = null;
            mod.version = null;

            requireUserInfo(mod, file);
        }

        public void createTechnicPermissionInfo(mcmod mod, PermissionLevel pl)
        {
            createTechnicPermissionInfo(mod, pl, null);
        }

        public void createTechnicPermissionInfo(mcmod mod, PermissionLevel pl, String customPermissionText)
        {
            String modlink = FTBPermsSQLhelper.getInfoFromModID(mod.modid, FTBPermissionsSQLHelper.InfoType.ModLink);
            while (String.IsNullOrWhiteSpace(modlink) || !Uri.IsWellFormedUriString(modlink, UriKind.Absolute))
            {
                modlink = Prompt.ShowDialog("What is the link to " + mod.name + "?", "Mod link", false, Prompt.modsLeftString(totalMods, currentMod));
            }
            createTechnicPermissionInfo(mod, pl, customPermissionText, modlink);
        }

        public void createTechnicPermissionInfo(mcmod mod, PermissionLevel pl, String customPermissionText, String modlink)
        {
            String ps = String.Format("{0}({1}) by {2}{3}At {4}{3}Permissions are {5}{3}", mod.name, mod.modid, getAuthors(mod), Environment.NewLine, modlink, pl.ToString());
            if (!String.IsNullOrWhiteSpace(customPermissionText))
            {
                ps += customPermissionText + Environment.NewLine;
            }
            File.AppendAllText(technicPermissionList, ps + Environment.NewLine);
        }

        public void CreateTechnicModZip(mcmod mod, String modfile)
        {
            if (mod.isSkipping)
            {
                return;
            }
            String FileName = modfile.Substring(modfile.LastIndexOf(globalfunctions.pathSeperator) + 1);
            String modMD5 = SQLHelper.calculateMD5(modfile);
            ModsSQLhelper.addMod(mod.name, mod.modid, mod.version, mod.mcversion, FileName, modMD5, false);
            if (CheckPermissions.Checked)
            {
                PermissionLevel PermLevel = FTBPermsSQLhelper.doFTBHavePermission(mod.modid, PublicFTBPack.Checked);
                String overwritelink = "";
                String modLink = "";
                ownPermissions op;
                String customPermissionText = "";
                switch (PermLevel)
                {
                    case PermissionLevel.Open:
                        createTechnicPermissionInfo(mod, PermLevel, null);
                        break;
                    case PermissionLevel.Notify:
                        op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                        if (!op.hasPermission)
                        {
                            overwritelink = Prompt.ShowDialog(mod.name + " requires that you notify the author of inclusion." + Environment.NewLine + "Please provide proof that you have done this:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod));
                            while (true)
                            {
                                if (overwritelink.ToLower().Equals("skip".ToLower()))
                                {
                                    mod.isSkipping = true;
                                    return;
                                }
                                else
                                {
                                    if (Uri.IsWellFormedUriString(overwritelink, UriKind.Absolute))
                                    {
                                        if (overwritelink.ToLower().Contains("imgur"))
                                        {
                                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, overwritelink);
                                            customPermissionText = "Proof of notitification: " + overwritelink;
                                            createTechnicPermissionInfo(mod, PermLevel, customPermissionText, FTBPermsSQLhelper.getInfoFromModID(mod.modid, FTBPermissionsSQLHelper.InfoType.ModLink));
                                            break;
                                        }
                                    }
                                    overwritelink = Prompt.ShowDialog(mod.name + " requires that you notify the author of inclusion." + Environment.NewLine + "Please provide proof that you have done this:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod));
                                }
                            }
                        }
                        else
                        {
                            customPermissionText = "Proof of notitification: " + op.PermissionLink;
                            createTechnicPermissionInfo(mod, PermLevel, customPermissionText);
                        }
                        break;
                    case PermissionLevel.FTB:
                        op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                        if (!op.hasPermission)
                        {
                            overwritelink = Prompt.ShowDialog("Permissions for " + mod.name + " is FTB exclusive" + Environment.NewLine + "Please provide proof of things being otherwise:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod));
                            while (true)
                            {
                                if (overwritelink.ToLower().Equals("skip".ToLower()))
                                {
                                    mod.isSkipping = true;
                                    return;
                                }
                                else
                                {
                                    if (Uri.IsWellFormedUriString(overwritelink, UriKind.Absolute))
                                    {
                                        if (overwritelink.ToLower().Contains("imgur"))
                                        {
                                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, overwritelink, FTBPermsSQLhelper.getInfoFromModID(mod.modid, FTBPermissionsSQLHelper.InfoType.ModLink));
                                            break;
                                        }
                                    }
                                    overwritelink = Prompt.ShowDialog("Permissions for " + mod.name + " is FTB exclusive" + Environment.NewLine + "Please provide proof of things being otherwise:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod));
                                }
                            }
                        }
                        op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                        customPermissionText = "Proof of permission outside of FTB: " + op.PermissionLink;
                        createTechnicPermissionInfo(mod, PermLevel, customPermissionText, op.ModLink);
                        break;
                    case PermissionLevel.Request:
                        op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                        if (!op.hasPermission)
                        {
                            overwritelink = Prompt.ShowDialog("This mod requires that you request permissions from the Mod Author of " + mod.name + Environment.NewLine + "Please provide proof that you have this permission:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true);
                            while (true)
                            {
                                if (overwritelink.ToLower().Equals("skip".ToLower()))
                                {
                                    mod.isSkipping = true;
                                    return;
                                }
                                else
                                {
                                    if (Uri.IsWellFormedUriString(overwritelink, UriKind.Absolute))
                                    {
                                        if (overwritelink.ToLower().Contains("imgur"))
                                        {
                                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, overwritelink, FTBPermsSQLhelper.getInfoFromModID(mod.modid, FTBPermissionsSQLHelper.InfoType.ModLink));
                                            break;
                                        }
                                    }
                                    overwritelink = Prompt.ShowDialog("This mod requires that you request permissions from the Mod Author of " + mod.name + Environment.NewLine + "Please provide proof that you have this permission:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod));
                                }
                            }
                        }
                        op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                        customPermissionText = getAuthors(mod) + " has given permission as seen here: " + op.PermissionLink;
                        createTechnicPermissionInfo(mod, PermLevel, customPermissionText, op.ModLink);
                        break;
                    case PermissionLevel.Closed:
                        op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                        if (!op.hasPermission)
                        {
                            overwritelink = Prompt.ShowDialog("The FTB permissionsheet states that permissions for " + mod.name + " is closed." + Environment.NewLine + "Please provide proof that this is not the case:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod));
                            while (true)
                            {
                                if (overwritelink.ToLower().Equals("skip".ToLower()))
                                {
                                    mod.isSkipping = true;
                                    return;
                                }
                                else
                                {
                                    if (Uri.IsWellFormedUriString(overwritelink, UriKind.Absolute))
                                    {
                                        if (overwritelink.ToLower().Contains("imgur"))
                                        {
                                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, overwritelink, FTBPermsSQLhelper.getInfoFromModID(mod.modid, FTBPermissionsSQLHelper.InfoType.ModLink));
                                            break;
                                        }
                                    }
                                    overwritelink = Prompt.ShowDialog("The FTB permissionsheet states that permissions for " + mod.name + " is closed." + Environment.NewLine + "Please provide proof that this is not the case:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true);
                                }
                            }
                        }
                        op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                        customPermissionText = getAuthors(mod) + " has given permission as seen here: " + op.PermissionLink;
                        createTechnicPermissionInfo(mod, PermLevel, customPermissionText, op.ModLink);
                        break;
                    case PermissionLevel.Unknown:
                        op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                        modLink = op.ModLink;
                        if (!op.hasPermission)
                        {
                            overwritelink = Prompt.ShowDialog("Permissions for " + mod.name + " is unknown" + Environment.NewLine + "Please provide proof of permissions:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod));
                            while (true)
                            {
                                if (overwritelink.ToLower().Equals("skip".ToLower()))
                                {
                                    mod.isSkipping = true;
                                    return;
                                }
                                else
                                {
                                    if (Uri.IsWellFormedUriString(overwritelink, UriKind.Absolute))
                                    {
                                        if (overwritelink.ToLower().Contains("imgur"))
                                        {
                                            break;
                                        }
                                    }
                                    overwritelink = Prompt.ShowDialog("Permissions for " + mod.name + " is unknown" + Environment.NewLine + "Please provide proof of permissions:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod));
                                }
                            }
                            while (String.IsNullOrWhiteSpace(modLink))
                            {
                                if (modLink != null && modLink.ToLower().Equals("skip".ToLower()))
                                {
                                    mod.isSkipping = true;
                                    return;
                                }
                                else
                                {
                                    if (modLink != null && Uri.IsWellFormedUriString(modLink, UriKind.Absolute))
                                    {
                                        OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, overwritelink, modLink);
                                        break;

                                    }
                                    modLink = Prompt.ShowDialog("Please provide a link to " + mod.name + ":" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true, Prompt.modsLeftString(totalMods, currentMod));
                                }
                            }
                            String a = getAuthors(mod);
                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, overwritelink, modLink);
                            createOwnPermissionInfo(mod.name, mod.modid, a, overwritelink, modLink);

                        }
                        op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                        customPermissionText = getAuthors(mod) + " has given permission as seen here: " + op.PermissionLink;
                        createTechnicPermissionInfo(mod, PermLevel, customPermissionText, op.ModLink);
                        break;
                    default:
                        break;
                }
            }
            if (SolderPack.Checked)
            {
                if (!ModsSQLhelper.IsFileInSolder(modfile))
                {
                    String modDir = "";
                    if (mod.modid.Contains("|"))
                    {
                        modDir = Path.Combine(OutputDirectory, "mods", mod.modid.Remove(mod.modid.LastIndexOf("|")).Replace(".", string.Empty).ToLower().Replace(globalfunctions.pathSeperator.ToString(), String.Empty), "mods");
                    }
                    else
                    {
                        modDir = Path.Combine(OutputDirectory, "mods", mod.modid.Replace(".", string.Empty).ToLower().Replace(globalfunctions.pathSeperator.ToString(), String.Empty), "mods");
                    }
                    Directory.CreateDirectory(modDir);

                    String tempModFile = Path.Combine(modDir, FileName);

                    String tempFileDirectory = tempModFile.Remove(tempModFile.LastIndexOf(globalfunctions.pathSeperator));

                    Directory.CreateDirectory(tempFileDirectory);
                    File.Copy(modfile, tempModFile, true);

                    String modArchive = "";
                    if (mod.modid.Contains("|"))
                    {
                        modArchive = Path.Combine(OutputDirectory, "mods", mod.modid.Remove(mod.modid.LastIndexOf("|")).Replace(".", string.Empty).ToLower(), mod.modid.Remove(mod.modid.LastIndexOf("|")).Replace(".", string.Empty).ToLower() + "-" + mod.mcversion.ToLower() + "-" + mod.version.ToLower() + ".zip");
                    }
                    else
                    {
                        modArchive = Path.Combine(OutputDirectory, "mods", mod.modid.Replace(".", string.Empty).ToLower(), mod.modid.Replace(".", string.Empty).ToLower() + "-" + mod.mcversion.ToLower() + "-" + mod.version.ToLower() + ".zip");
                    }
                    if (globalfunctions.isUnix())
                    {
                        if (mod.modid.Contains("|"))
                        {
                            Environment.CurrentDirectory = Path.Combine(OutputDirectory, "mods", mod.modid.Remove(mod.modid.LastIndexOf("|")).Replace(".", string.Empty).ToLower());
                        }
                        else
                        {
                            Environment.CurrentDirectory = Path.Combine(OutputDirectory, "mods", mod.modid.Replace(".", string.Empty).ToLower());
                        }
                        modDir = "mods";
                        startInfo.FileName = "zip";
                        startInfo.Arguments = "-r \"" + modArchive + "\" \"" + modDir + "\" ";
                    }
                    else
                    {
                        startInfo.Arguments = "a -y \"" + modArchive + "\" \"" + modDir + "\" ";
                    }
                    process.StartInfo = startInfo;
                    process.Start();

                    //Save mod to database
                    ModsSQLhelper.addMod(mod.name, mod.modid, mod.version, mod.mcversion, FileName, modMD5, true);

                    // Add mod info to a html file
                    string s = "";
                    if (mod.modid.Contains("|"))
                    {
                        s = mod.modid.Remove(mod.modid.LastIndexOf("|")).Replace(".", String.Empty).ToLower();
                    }
                    else
                    {
                        s = mod.modid.Replace(".", string.Empty).ToLower();
                    }
                    createTableRow(mod.name.Replace("|", ""), s, mod.mcversion.ToLower() + "-" + mod.version.ToLower());

                    process.WaitForExit();

                    Directory.Delete(modDir, true);
                }
                else
                {
                }
            }
            else
            {
                ModsSQLhelper.addMod(mod.name, mod.modid, mod.version, mod.mcversion, FileName, modMD5, false);
                while (String.IsNullOrWhiteSpace(ModpackName))
                {
                    ModpackName = Prompt.ShowDialog("What is the Modpack Name?", "Modpack Name");
                }
                while (String.IsNullOrWhiteSpace(ModpackVersion))
                {
                    ModpackVersion = Prompt.ShowDialog("What Version is the modpack?", "Modpack Version");
                }

                String tempDirectory = Path.Combine(OutputDirectory, "tmp");
                String tempModDirectory = Path.Combine(tempDirectory, "mods");
                Directory.CreateDirectory(tempModDirectory);
                String tempFile = Path.Combine(tempModDirectory, FileName);

                int index = tempFile.LastIndexOf(globalfunctions.pathSeperator);

                String tempFileDirectory = tempFile.Remove(index);
                Directory.CreateDirectory(tempFileDirectory);
                File.Copy(modfile, tempFile, true);

                ModpackArchive = Path.Combine(OutputDirectory, String.Format("{0}-{1}.zip", ModpackName, ModpackVersion));

                if (globalfunctions.isUnix())
                {
                    Environment.CurrentDirectory = tempDirectory;
                    startInfo.Arguments = String.Format("-r \"{0}\" \"{1}\"", ModpackArchive, "mods");
                }
                else
                {
                    startInfo.Arguments = String.Format("a -y \"{0}\" \"{1}\"", ModpackArchive, tempModDirectory);
                }
                if (globalfunctions.isUnix())
                {
                    startInfo.FileName = "zip";
                }
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                Directory.Delete(tempDirectory, true);
            }

            if (!mod.hasBeenWritenToModlist)
            {
                File.AppendAllText(modlistTextFile, mod.name + Environment.NewLine);
                mod.hasBeenWritenToModlist = true;
            }

        }

        #endregion

        #region Interface buttons

        private void InputDirectoryBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowser.SelectedPath = InputFolder.Text;
            DialogResult result = FolderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                InputFolder.Text = FolderBrowser.SelectedPath;
                if (globalfunctions.isUnix())
                {
                    confighandler.setConfig("InputDirectory", InputFolder.Text);
                }
                else
                {
                    Properties.Settings.Default.InputDirectory = InputFolder.Text;
                    Properties.Settings.Default.Save();
                }
            }
            InputFolder_TextChanged(null, null);

        }

        private void OutputDirectoryBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowser.SelectedPath = OutputFolder.Text;
            DialogResult result = FolderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                OutputFolder.Text = FolderBrowser.SelectedPath;
                if (globalfunctions.isUnix())
                {
                    confighandler.setConfig("OutputDirectory", OutputFolder.Text);
                }
                else
                {
                    Properties.Settings.Default.OutputDirectory = OutputFolder.Text;
                    Properties.Settings.Default.Save();
                }
            }

        }

        public void button1_Click(object sender, EventArgs e)
        {
            Start();
        }

        public void button2_Click(object sender, EventArgs e)
        {
            ModsSQLhelper.resetTable();
            String s = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "unarchievedFiles");
            if (Directory.Exists(s))
            {
                Directory.Delete(s, true);
            }
        }

        private void InputFolder_TextChanged(object sender, EventArgs e)
        {
            if (globalfunctions.isUnix())
            {
                confighandler.setConfig("InputDirectory", InputFolder.Text);
            }
            else
            {
                Properties.Settings.Default.InputDirectory = InputFolder.Text;
                Properties.Settings.Default.Save();
            }

            String superDirectory = InputFolder.Text.Remove(InputFolder.Text.LastIndexOf(globalfunctions.pathSeperator));


            List<String> dirs = new List<string>();
            foreach (String dir in Directory.GetDirectories(superDirectory))
            {
                if (dir.EndsWith("mods") || dir.EndsWith("config"))
                {
                    continue;
                }
                else
                {
                    dirs.Add(dir);
                }
            }
            additionalDirectories.Clear();
            int c = 0;
            for (int i = 23; i < dirs.Count * 23 + 23; i += 23)
            {
                if (!additionalDirectories.ContainsKey(dirs[c]))
                {
                    String dirname = dirs[c].Substring(dirs[c].LastIndexOf(globalfunctions.pathSeperator) + 1);
                    additionalDirectories.Add(dirs[c], new CheckBox()
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
                additionalDirectories.Add(serversDat, new CheckBox()
                    {
                        Left = 20,
                        Top = c * 23 + 23,
                        Height = 20,
                        Text = "Servers.dat file"
                    });
            }
            groupBox1.Controls.Clear();
            foreach (CheckBox cb in additionalDirectories.Values)
            {
                groupBox1.Controls.Add(cb);
            }
        }

        private void OutputFolder_TextChanged(object sender, EventArgs e)
        {
            if (globalfunctions.isUnix())
            {
                confighandler.setConfig("OutputDirectory", OutputFolder.Text);
            }
            else
            {
                Properties.Settings.Default.OutputDirectory = OutputFolder.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            excelReader.addFTBPermissions();
        }

        #endregion

        #region Feed The Beast Packs

        private void CreateFTBPack_CheckedChanged(object sender, EventArgs e)
        {
            if (globalfunctions.isUnix())
            {
                confighandler.setConfig("CreateFTBPack", CreateFTBPack.Checked);
            }
            else
            {
                Properties.Settings.Default.CreateFTBPack = CreateFTBPack.Checked;
                Properties.Settings.Default.Save();
            }

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
            if (globalfunctions.isUnix())
            {
                confighandler.setConfig("CreatePrivateFTBPack", PrivateFTBPack.Checked);
            }
            else
            {
                Properties.Settings.Default.CreatePrivateFTBPack = PrivateFTBPack.Checked;
                Properties.Settings.Default.Save();
            }
        }

        #endregion

        #region Technic Packs

        private void CreateTechnicPack_CheckedChanged(object sender, EventArgs e)
        {
            if (globalfunctions.isUnix())
            {
                confighandler.setConfig("CreateTechnicSolderFiles", CreateTechnicPack.Checked);
            }
            else
            {
                Properties.Settings.Default.CreateTechnicSolderFiles = CreateTechnicPack.Checked;
                Properties.Settings.Default.Save();
            }
            if (CreateTechnicPack.Checked)
            {
                SolderPackType.Show();
                DistributionLevel.Location = new Point(DistributionLevel.Location.X, DistributionLevel.Location.Y + SolderPackType.Height);
                CreateFTBPack.Location = new Point(CreateFTBPack.Location.X, CreateFTBPack.Location.Y + SolderPackType.Height);
                if (CheckPermissions.Checked)
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
            if (globalfunctions.isUnix())
            {
                confighandler.setConfig("CreateTechnicConfigZip", IncludeConfigZip.Checked);
            }
            else
            {
                Properties.Settings.Default.CreateTechnicConfigZip = IncludeConfigZip.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void SolderPack_CheckedChanged(object sender, EventArgs e)
        {
            if (globalfunctions.isUnix())
            {
                confighandler.setConfig("CreateSolderPack", SolderPack.Checked);
            }
            else
            {
                Properties.Settings.Default.CreateSolderPack = SolderPack.Checked;
                Properties.Settings.Default.Save();
            }

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
            if (globalfunctions.isUnix())
            {
                confighandler.setConfig("CheckTecnicPermissions", CheckPermissions.Checked);
            }
            else
            {
                Properties.Settings.Default.CheckTecnicPermissions = CheckPermissions.Checked;
                Properties.Settings.Default.Save();
            }

            if (CheckPermissions.Checked)
            {
                TechnicDistributionLevel.Show();
            }
            else
            {
                TechnicDistributionLevel.Hide();
            }
        }

        private void TechnicPrivatePermissions_CheckedChanged(object sender, EventArgs e)
        {
            if (globalfunctions.isUnix())
            {
                confighandler.setConfig("TechnicPrivatePermissionsLevel", TechnicPrivatePermissions.Checked);
            }
            else
            {
                Properties.Settings.Default.TechnicPrivatePermissionsLevel = TechnicPrivatePermissions.Checked;
                Properties.Settings.Default.Save();
            }
        }

        #endregion

        private void UploadToFTPServer_CheckedChanged(object sender, EventArgs e)
        {
            if (globalfunctions.isUnix())
            {
                confighandler.setConfig("UploadToFTPServer", UploadToFTPServer.Checked);
            }
            else
            {
                Properties.Settings.Default.UploadToFTPServer = UploadToFTPServer.Checked;
                Properties.Settings.Default.Save();
            }

            if (UploadToFTPServer.Checked)
            {
                bool hasbeenwarned = false;
                if (globalfunctions.isUnix())
                {
                    try
                    {
                        hasbeenwarned = Convert.ToBoolean(confighandler.getConfig("HasBeenWarnedAboutLongFTPTimes"));
                    }
                    catch
                    {
                    }
                }
                else
                {
                    hasbeenwarned = Properties.Settings.Default.HasBeenWarnedAboutLongFTBTimes;
                }
                if (!hasbeenwarned)
                {
                    if (globalfunctions.isUnix())
                    {
                        confighandler.setConfig("HasBeenWarnedAboutLongFTPTimes", true);
                    }
                    else
                    {
                        Properties.Settings.Default.HasBeenWarnedAboutLongFTBTimes = true;
                    }
                    var responce = MessageBox.Show("Uploading to FTP can take a very long time. Do you still want to upload to FTP?", "FTP upload", MessageBoxButtons.YesNo);
                    if (responce == System.Windows.Forms.DialogResult.Yes)
                    {
                        configureFTP.Show();
                    }
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
            if (ftp == null)
            {
                ftp = new Ftp();
            }

        }

        private void MCversion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ForgeBuild.Items.Clear();
            String selectedMcversion = MCversion.SelectedItem.ToString();
            List<String> ForgeVersions = forgesqlhelper.getForgeVersions(selectedMcversion);

            foreach (String build in ForgeVersions)
            {
                ForgeBuild.Items.Add(build);
            }
        }

        private void GetForgeVersions_Click(object sender, EventArgs e)
        {
            #region Find MC versions

            MCversion.Items.Clear();

            forgesqlhelper.FindAllForgeVersion();
            List<String> mcversions = forgesqlhelper.getMCVersions();
            foreach (String mcversion in mcversions)
            {
                MCversion.Items.Add(mcversion);
            }

            #endregion
        }

        private void IncludeForgeVersion_CheckedChanged(object sender, EventArgs e)
        {
            if (globalfunctions.isUnix())
            {
                confighandler.setConfig("IncludeForgeVersion", IncludeForgeVersion.Checked);
            }
            else
            {
                Properties.Settings.Default.IncludeForgeVersion = IncludeForgeVersion.Checked;
                Properties.Settings.Default.Save();
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

        private void getliteloaderversions_Click(object sender, EventArgs e)
        {
            String liteloaderjsonfile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            liteloaderjsonfile = Path.Combine(liteloaderjsonfile, "liteloader.json");
            WebClient wb = new WebClient();
            System.Uri webfile = new Uri("http://dl.liteloader.com/versions/versions.json");
            wb.DownloadFile(webfile, liteloaderjsonfile);

            String json = "";
            using (StreamReader r = new StreamReader(liteloaderjsonfile))
            {
                json = r.ReadToEnd();
            }
            liteloader liteloader = JsonConvert.DeserializeObject<liteloader>(json);

            foreach (KeyValuePair<String, versions> item in liteloader.versions)
            {
                foreach (versionclass it in item.Value.artefacts["com.mumfrey:liteloader"].Values)
                {
                    liteloadersqlhelper.addVersion(it.file, it.version, it.md5, item.Key, it.tweakClass);
                }

            }
        }

        private void testInterface_Click(object sender, EventArgs e)
        {
            if (this.Width == 657)
            {
                this.Width = 800;
            }
            else
            {
                if (this.Width == 750)
                {
                    this.Width = 800;
                }
            }
        }

        private void configureFTP_Click(object sender, EventArgs e)
        {
            Form f = new ftp.ftpInfo();
            f.ShowDialog();

        }

    }
}
