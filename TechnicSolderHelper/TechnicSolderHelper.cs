﻿using System;
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


namespace TechnicSolderHelper
{
    public partial class SolderHelper : Form
    {
        #region Application Wide Variables

        public static String DirectoryWithFiles;
        public static String OutputDirectory;
        public ModListSQLHelper ModsSQLhelper = new ModListSQLHelper();
        public FTBPermissionsSQLHelper FTBPermsSQLhelper = new FTBPermissionsSQLHelper();
        public OwnPermissionsSQLHelper OwnPermsSQLhelper = new OwnPermissionsSQLHelper();
        public ForgeSQLHelper forgesqlhelper = new ForgeSQLHelper();
        public liteloaderSQLHelper liteloadersqlhelper = new liteloaderSQLHelper();
        public static String SevenZipLocation = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechnicSolderHelper\7za.exe";
        public static Process process = new System.Diagnostics.Process();
        public static ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        public static String UserName, path, CurrentMCVersion, ModpackVersion, ModpackName, ModpackArchive, FTBModpackArchive;
        public static ConfigHandler confighandler = new ConfigHandler();
        public static String modlistTextFile = "", technicPermissionList = "", FTBPermissionList = "", FTBOwnPermissionList = "";
        public static short totalMods = 0, currentMod = 0;

        #endregion

        public SolderHelper()
        {
            UserName = Environment.UserName;
            InitializeComponent();
            bool firstRun = true;
            if (globalfunctions.isUnix())
            {
                try
                {
                    firstRun = Convert.ToBoolean(confighandler.getConfig("FirstRun"));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
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
                    Debug.WriteLine("Adding mcversion: " + mcversion);
                    MCversion.Items.Add(mcversion);
                }
                Debug.WriteLine("Done adding versions");

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
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.Assert(false);
                    InputFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/mods";
                }

                try
                {
                    OutputFolder.Text = confighandler.getConfig("OutputDirectory");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.Assert(false);
                    OutputFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/SolderHelper";
                }

                try
                {
                    CreateTechnicPack.Checked = Convert.ToBoolean(confighandler.getConfig("CreateTechnicSolderFiles"));
                    SolderPackType.Visible = CreateTechnicPack.Checked;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.Assert(false);
                    CreateTechnicPack.Checked = false;
                }

                try
                {
                    CreateFTBPack.Checked = Convert.ToBoolean(confighandler.getConfig("CreateFTBPack"));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.Assert(false);
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

            Boolean CSP = true, CPFP = true, TPP = true, IFV = false, ICZ = true, CP = false;
            if (globalfunctions.isUnix())
            {
                try
                {
                    CSP = Convert.ToBoolean(confighandler.getConfig("CreateSolderPack"));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                try
                {
                    CPFP = Convert.ToBoolean(confighandler.getConfig("CreatePrivateFTBPack"));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                try
                {
                    TPP = Convert.ToBoolean(confighandler.getConfig("TechnicPrivatePermissionsLevel"));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                try
                {
                    IFV = Convert.ToBoolean(confighandler.getConfig("IncludeForgeVersion"));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                try
                {
                    ICZ = Convert.ToBoolean(confighandler.getConfig("CreateTechnicConfigZip"));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                try
                {
                    CP = Convert.ToBoolean(confighandler.getConfig("CheckTecnicPermissions"));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
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
                Debug.WriteLine("Adding mcversion: " + mcversion);
                MCversion.Items.Add(mcversion);
            }
            Debug.WriteLine("Done adding versions");

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
        }

        public void createFTBPermissionInfo(String modname, String modid, String modauthor, String linkToPermission)
        {
            String output = String.Format("{0}({1}) by {2} {3}Permission: {4} {3}{3}", modname, modid, modauthor, Environment.NewLine, linkToPermission, Environment.NewLine);
            Debug.WriteLine(output);
            File.AppendAllText(FTBPermissionList, output);
        }

        public void createOwnPermissionInfo(String modname, String modid, String modauthor, String linkToPermission, String modLink)
        {
            String output = String.Format("{0}({1}) by {2} {3}Permission: {4} {3}Link to mod: {5}{3}{3}", modname, modid, modauthor, Environment.NewLine, linkToPermission, modLink);
            Debug.WriteLine(output);
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
                            authorString = Prompt.ShowDialog("Who is the author of " + mod.name + "?" + Environment.NewLine + "If you leave this empty the author list in the output will also be empty.", "Mod Author");

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
            String FileName = modfile.Replace(DirectoryWithFiles, "").Replace("1.6.4\\", "").Replace("1.7.2\\", "").Replace("1.7.10\\", "").Replace("1.5.2\\", "").Replace("\\", "").Trim();
            FileName = modfile.Replace(DirectoryWithFiles, "").Replace("1.6.4/", "").Replace("1.7.2/", "").Replace("1.7.10/", "").Replace("1.5.2/", "").Replace("/", "").Trim();
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
                    PermissionLevel PermLevel = FTBPermsSQLhelper.doFTBHavePermission(mod.modid, PublicFTBPack.Checked, mod.useShortName);

                    Debug.WriteLine(PermLevel.ToString());
                    String overwritelink = "";
                    ownPermissions op;
                    switch (PermLevel)
                    {
                        case PermissionLevel.Open:
                            Debug.WriteLine("Open Permissions");
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
                                        overwritelink = Prompt.ShowDialog(mod.name + " requires that you notify the author of inclusion." + Environment.NewLine + "Please provide proof that you have done this:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true).Replace(" ", "");
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
                            Debug.WriteLine("FTB Permissions");
                            break;
                        case PermissionLevel.Request:
                            op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                            if (!op.hasPermission)
                            {
                                overwritelink = Prompt.ShowDialog("This mod requires that you request permissions from the Mod Author of " + mod.name + Environment.NewLine + "Please provide proof that you have this permission:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true).Replace(" ", "");
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
                                        overwritelink = Prompt.ShowDialog("This mod requires that you request permissions from the Mod Author of " + mod.name + Environment.NewLine + "Please provide proof that you have this permission:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true).Replace(" ", "");
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
                                overwritelink = Prompt.ShowDialog("The FTB permissionsheet states that permissions for " + mod.name + " is closed." + Environment.NewLine + "Please provide proof that this is not the case:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true).Replace(" ", "");
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
                                        overwritelink = Prompt.ShowDialog("The FTB permissionsheet states that permissions for " + mod.name + " is closed." + Environment.NewLine + "Please provide proof that this is not the case:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true).Replace(" ", "");
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
                                overwritelink = Prompt.ShowDialog("Permissions for " + mod.name + " is unknown" + Environment.NewLine + "Please provide proof of permissions:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true).Replace(" ", "");
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
                                        overwritelink = Prompt.ShowDialog("Permissions for " + mod.name + " is unknown" + Environment.NewLine + "Please provide proof of permissions:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true).Replace(" ", "");
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
                                        modLink = Prompt.ShowDialog("Please provide a link to " + mod.name + ":" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true).Replace(" ", "");
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
                            Debug.WriteLine("WELLP, something went wrong!!");
                            break;
                    }
                    #endregion
                }
            }

            //Debug.WriteLine("Creating big zip file");
            if (String.IsNullOrWhiteSpace(FTBModpackArchive))
            {
                while (String.IsNullOrWhiteSpace(ModpackName))
                {
                    ModpackName = Prompt.ShowDialog("What is the Modpack Name?", "Modpack Name");
                    Debug.WriteLine(ModpackName);
                }
                while (String.IsNullOrWhiteSpace(ModpackVersion))
                {
                    ModpackVersion = Prompt.ShowDialog("What Version is the modpack?", "Modpack Version");
                }
                if (String.IsNullOrWhiteSpace(ModpackArchive))
                {
                    ModpackArchive = String.Format("{0}\\{1}-{2}.zip", OutputDirectory, ModpackName, ModpackVersion);
                }
                if (globalfunctions.isUnix())
                {
                    ModpackArchive = ModpackArchive.Replace("\\", "/");
                }
                FTBModpackArchive = Path.Combine(OutputDirectory, ModpackName + "-" + ModpackVersion + "-FTB" + ".zip");

            }


            String tempModDirectory = String.Format("{0}\\minecraft\\mods", OutputDirectory);
            if (globalfunctions.isUnix())
            {
                tempModDirectory = tempModDirectory.Replace("\\", "/");
            }
            Directory.CreateDirectory(tempModDirectory);
            String tempFile = String.Format("{0}\\{1}", tempModDirectory, FileName);
            if (globalfunctions.isUnix())
            {
                tempFile = tempFile.Replace("\\", "/");
            }
            tempFile = tempFile.Replace("\\\\", "\\");
            int index = 0;
            if (globalfunctions.isUnix())
            {
                index = tempFile.LastIndexOf("/");
            }
            else
            {
                index = tempFile.LastIndexOf("\\");
            }
            String tempFileDirectory = tempFile.Remove(index);
            if (globalfunctions.isUnix())
            {
                tempFileDirectory = tempFileDirectory.Replace("\\", "/");
            }
            Debug.WriteLine(tempFileDirectory);
            Directory.CreateDirectory(tempFileDirectory);
            File.Copy(modfile, tempFile, true);
            Debug.WriteLine("Copying " + modfile + " to " + tempFile);

            //ModpackArchive = String.Format("{0}\\{1}-{2}.zip", OutputDirectory, ModpackName, ModpackVersion);
            if (globalfunctions.isUnix())
            {
                FTBModpackArchive = FTBModpackArchive.Replace("\\", "/");
                Environment.CurrentDirectory = OutputDirectory;
                startInfo.Arguments = String.Format("-r \"{0}\" \"{1}\"", FTBModpackArchive, "minecraft");
                Debug.WriteLine(startInfo.Arguments);
            }
            else
            {
                Environment.CurrentDirectory = OutputDirectory;
                startInfo.Arguments = String.Format("a -y \"{0}\" \"{1}\"", FTBModpackArchive, "minecraft");
            }
            if (globalfunctions.isUnix())
            {
                startInfo.FileName = "zip";
                startInfo.Arguments = startInfo.Arguments.Replace("\\", "/");
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
            DirectoryWithFiles = InputFolder.Text;
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
                Debug.WriteLine("Erasing!!!");
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
            ModpackVersion = ModpackVersionInput.Text;

            ModpackVersion = null;
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

            path = OutputDirectory + @"\mods.html";
            if (globalfunctions.isUnix())
            {
                path = path.Replace("\\", "/");
            }


            Directory.CreateDirectory(OutputDirectory);
            Environment.CurrentDirectory = OutputDirectory;
            Debug.WriteLine(Environment.CurrentDirectory);
            if (globalfunctions.isUnix())
            {
                modlistTextFile = OutputDirectory + "/modlist.txt";
            }
            else
            {
                modlistTextFile = OutputDirectory + "\\modlist.txt";
            }
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
                                  "<style type=\"text/css\" rel=\"stylesheet\" href=\"http://cloud.zlepper.dk/technicsolderhelper.css\"></style>" +
                                  "</head>" + Environment.NewLine + "<body><table border='1'><tr><th>Modname</th><th>Modslug</th><th>Version</th></tr>" + Environment.NewLine;
                File.WriteAllText(path, htmlfile);
            }


            // Create array with all the mod locations
            List<String> files = new List<String>();

            // Add the different mod files to the files array
            foreach (String file in Directory.GetFiles(DirectoryWithFiles, "*.zip", SearchOption.AllDirectories))
            {
                files.Add(file);
                totalMods++;
                //Debug.WriteLine(file);
            }
            foreach (String file in Directory.GetFiles(DirectoryWithFiles, "*.jar", SearchOption.AllDirectories))
            {
                files.Add(file);
                totalMods++;
                //Debug.WriteLine(file);
            }
            foreach (String file in Directory.GetFiles(DirectoryWithFiles, "*.litemod", SearchOption.AllDirectories))
            {
                files.Add(file);
                totalMods++;
                //Debug.WriteLine(file);
            }
            foreach (String file in Directory.GetFiles(DirectoryWithFiles, "*.disabled", SearchOption.AllDirectories))
            {
                files.Add(file);
                totalMods++;
                //Debug.WriteLine(file);
            }
            Debug.WriteLine(totalMods);
            //Check if files have already been added
            foreach (String file in files)
            {
                if (IsWierdMod(file) == 0)
                {
                    continue;
                }
                String FileName = file.Replace(DirectoryWithFiles, "");
                ProgressLabel.Text = FileName;
                //Check for mcmod.info
                Directory.CreateDirectory(OutputDirectory);
                String Arguments = "";
                if (globalfunctions.isUnix())
                {
                    startInfo.FileName = "unzip";
                    Arguments = "-o \"" + file + "\" \"*.info\" \"*.json\" -d \"" + OutputDirectory + "\"";
                    Debug.WriteLine(Arguments);
                }
                else
                {
                    Arguments = "e " + "-y -o\"" + OutputDirectory + "\" \"" + file + "\" *.info litemod.json";
                }
                //Debug.WriteLine(Arguments);
                startInfo.Arguments = Arguments;

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                String mcmodfile = Path.Combine(OutputDirectory, "mcmod.info");//OutputDirectory + @"\mcmod.info";
                String litemodfile = Path.Combine(OutputDirectory, "litemod.json");//OutputDirectory + @"\litemod.json";
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
                        Debug.WriteLine(modinfofile);
                        if (modinfofile.ToLower().Contains("dependancies") || modinfofile.ToLower().Contains("dependencies"))
                        {
                            Debug.WriteLine("Found dependancy file");
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
                            Debug.WriteLine(modinfofile);
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

                            if (modinfo2.modListVersion == 2)
                            {
                                //Debug.WriteLine("Is version 2");
                                mod.mcversion = modinfo2.modList[0].mcversion.ToString();
                                mod.modid = modinfo2.modList[0].modid.ToString();
                                mod.name = modinfo2.modList[0].name.ToString();
                                mod.version = modinfo2.modList[0].version.ToString();
                                requireUserInfo(mod, file);
                            }
                            else
                            {
                                throw new JsonSerializationException();
                            }
                        }
                        catch (Newtonsoft.Json.JsonSerializationException)
                        {
                            Debug.Write("");
                            try
                            {
                                mcmod mod = new mcmod();
                                //Debug.WriteLine("Maybe version 1?");
                                List<mcmod> modinfo = null;
                                try
                                {
                                    modinfo = JsonConvert.DeserializeObject<List<mcmod>>(json);
                                    //Debug.WriteLine("Version 1");
                                }
                                catch (Newtonsoft.Json.JsonReaderException)
                                {
                                    MessageBox.Show("Something is wrong with the Json in " + FileName);
                                    throw new JsonSerializationException("Invalid Json in file" + FileName);
                                }
                                mod = modinfo[0];

                                if (file.ToLower().Contains("mekanism"))
                                {
                                    //Debug.WriteLine("Found mekanism");
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
                            catch (Newtonsoft.Json.JsonSerializationException e)
                            {
                                Debug.WriteLineIf(e.InnerException != null, e.InnerException);
                                Debug.WriteLineIf(e.StackTrace != null, e.StackTrace);
                                Debug.WriteLineIf(e.Message != null, e.Message);
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
                    catch (Newtonsoft.Json.JsonSerializationException e)
                    {
                        Debug.WriteLine(e.Message);
                        requireUserInfo(file);
                    }
                    Debug.WriteLine("");
                    File.Delete(mcmodfile);
                }
                else
                {
                    String fileName = file.Replace(DirectoryWithFiles, "").Replace("1.6.4\\", "").Replace("1.7.2\\", "").Replace("1.7.10\\", "").Replace("1.5.2\\", "").Replace("\\", "").Trim();
                    fileName = file.Replace(DirectoryWithFiles, "").Replace("1.6.4/", "").Replace("1.7.2/", "").Replace("1.7.10/", "").Replace("1.5.2/", "").Replace("/", "").Trim();

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
                                    mod.version = llversion.version.Substring(llversion.version.LastIndexOf("_") + 1);
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
                            mod.name = FTBPermsSQLhelper.getInfoFromModID(shortname, FTBPermissionsSQLHelper.InfoType.ModName);
                            mod.authors = new List<string>();
                            mod.authors.Add(FTBPermsSQLhelper.getInfoFromModID(shortname, FTBPermissionsSQLHelper.InfoType.ModAuthor));
                            mod.authorList = mod.authors;
                            mod.privatePerms = FTBPermsSQLhelper.doFTBHavePermission(shortname, false, true);
                            mod.publicPerms = FTBPermsSQLhelper.doFTBHavePermission(shortname, true, true);
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

            if (CreateTechnicPack.Checked && IncludeConfigZip.Checked)
            {
                createConfigZip();
            }

            //FTB pack configs
            if (CreateFTBPack.Checked)
            {
                String tmpConfigDirectory = Path.Combine(OutputDirectory, Path.Combine("minecraft", "config"));
                Debug.WriteLine(tmpConfigDirectory);
                Directory.CreateDirectory(tmpConfigDirectory);

                String sourceConfigDirectory = InputFolder.Text;
                if (globalfunctions.isUnix())
                {
                    sourceConfigDirectory = sourceConfigDirectory.Replace("/mods", "/config");
                }
                else
                {
                    sourceConfigDirectory = sourceConfigDirectory.Replace("\\mods", "\\config");
                }
                DirectoryCopy(sourceConfigDirectory, tmpConfigDirectory, true);

                Environment.CurrentDirectory = OutputDirectory;
                if (globalfunctions.isUnix())
                {
                    startInfo.FileName = "zip";
                    startInfo.Arguments = String.Format("-r \"{0}\" \"minecraft\"", FTBModpackArchive);
                }
                else
                {
                    startInfo.Arguments = "a -y \"" + FTBModpackArchive + "\" \"minecraft\"";
                }

                Debug.WriteLine(startInfo.Arguments.ToString());
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                Directory.Delete(Path.Combine(OutputDirectory, "minecraft"), true);
            }

            if (IncludeForgeVersion.Checked)
            {
                string selectedBuild = ForgeBuild.SelectedItem.ToString();
                Debug.WriteLine(selectedBuild);
                Number forgeinfo = forgesqlhelper.getForgeInfo(selectedBuild);
                String tmpdir = OutputDirectory + "\\bin";
                if (globalfunctions.isUnix())
                {
                    tmpdir = tmpdir.Replace("\\", "/");
                }
                Directory.CreateDirectory(tmpdir);
                String tempfile = tmpdir + "\\modpack.jar";
                if (globalfunctions.isUnix())
                {
                    tempfile = tempfile.Replace("\\", "/");
                }
                WebClient wb = new WebClient();
                Debug.WriteLine("Downloading: " + forgeinfo.downloadurl);
                wb.DownloadFile(forgeinfo.downloadurl, tempfile);
                if (globalfunctions.isUnix())
                {
                    Directory.CreateDirectory(OutputDirectory + "/forge");
                }
                else
                {
                    Directory.CreateDirectory(OutputDirectory + "\\forge");
                }
                if (SolderPack.Checked)
                {
                    String outputfile = OutputDirectory + "\\forge\\forge-" + forgeinfo.version + ".zip";
                    if (globalfunctions.isUnix())
                    {
                        outputfile = outputfile.Replace("\\", "/");
                        startInfo.FileName = "zip";
                        Environment.CurrentDirectory = OutputDirectory;
                        startInfo.Arguments = "-r \"" + outputfile + "\" \"bin\"";
                    }
                    else
                    {
                        startInfo.Arguments = "a -y \"" + outputfile + "\" \"" + tmpdir + "\"";
                    }
                    String AddedMod = "<tr>";
                    File.AppendAllText(path, AddedMod);
                    AddedMod = "<td><input class=\"containsInfo\" value=\"Minecraft Forge\"></input></td>";
                    File.AppendAllText(path, AddedMod);
                    AddedMod = "<td><input class=\"containsInfo\" value=\"forge\"></input></td>";
                    File.AppendAllText(path, AddedMod);
                    AddedMod = "<td><input class=\"containsInfo\" value=\"" + forgeinfo.version + "\"></input></td>";
                    File.AppendAllText(path, AddedMod.ToLower());
                    File.AppendAllText(path, "<td><button class=\"Hide\" type=\"button\">Hide</button></td>");
                    File.AppendAllText(path, "</tr>" + Environment.NewLine);
                }
                else
                {
                    startInfo.Arguments = "a -y \"" + ModpackArchive + "\" \"" + tmpdir + "\"";
                }
                Debug.WriteLine(startInfo.Arguments.ToString());
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                Directory.Delete(tmpdir, true);
            }
            if (globalfunctions.isUnix())
            {
                if (Directory.Exists(OutputDirectory + "/assets"))
                {
                    Directory.Delete(OutputDirectory + "/assets", true);
                }
                if (Directory.Exists(OutputDirectory + "/example"))
                {
                    Directory.Delete(OutputDirectory + "/example", true);
                }
            }
            else
            {
                if (Directory.Exists(OutputDirectory + "\\assets"))
                {
                    Directory.Delete(OutputDirectory + "\\assets", true);
                }
                if (Directory.Exists(OutputDirectory + "\\example"))
                {
                    Directory.Delete(OutputDirectory + "\\example", true);
                }
            }
            Debug.WriteLine(path);
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
                        Debug.WriteLine("First try");
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
            ProgressLabel.Text = "Waiting...";

        }

        #region Technic Pack Function

        /// <summary>
        /// Checks if the mod is on the list of mods which has custom support.
        /// </summary>
        /// <param name="modFileName">The mod file name.</param>
        /// <returns>Returns the number of the method to call, if no match is found, returns zero</returns>
        private static int IsWierdMod(String modFileName)
        {
            Debug.WriteLine(modFileName);
            String[] skipMods =
                {"CarpentersBlocksCachedResources", 
                    "CodeChickenLib", 
                    "ForgeMultipart", 
                    "ejml-"
                };
            for (int i = 0; i < skipMods.Length; i++)
            {
                if (modFileName.ToLower().Contains(skipMods[i].ToLower()))
                {
                    Debug.WriteLine("Found something");
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
                InputDirectory = InputDirectory.Replace("\\mods", "");
                if (globalfunctions.isUnix())
                {
                    InputDirectory = InputDirectory.Replace("/mods", "").Replace("\\", "/");
                }
                OutputDirectory = OutputFolder.Text;
                String ConfigFileName = Prompt.ShowDialog("What do you want the file name of the config " + Environment.NewLine + "folder to be?", "Config FileInfo Name");
                String ConfigVersion = Prompt.ShowDialog("What is the config version?", "Config Version");
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
                    startInfo.Arguments = "-r \"" + OutputDirectory + "/" + ConfigFileName + "/" + ConfigFileZipName + "\" \"config\"";
                }
                else
                {
                    startInfo.Arguments = "a -y \"" + OutputDirectory + "\\" + ConfigFileName + "\\" + ConfigFileZipName + "\" \"" + InputDirectory + "\\config" + "\"";
                }
                startInfo.Arguments = startInfo.Arguments;
                process.StartInfo = startInfo;
                process.Start();

                String AddedMod = "<tr>";
                File.AppendAllText(path, AddedMod);
                AddedMod = "<td><input class=\"containsInfo\" value=\"" + ConfigFileName + "\"></input></td>";
                File.AppendAllText(path, AddedMod);
                AddedMod = "<td><input class=\"containsInfo\" value=\"" + ConfigFileName + "\"></input></td>";
                File.AppendAllText(path, AddedMod);
                AddedMod = "<td><input class=\"containsInfo\" value=\"" + ConfigVersion + "\"></input></td>";
                File.AppendAllText(path, AddedMod.ToLower());
                File.AppendAllText(path, "<td><button class=\"Hide\" type=\"button\">Hide</button></td>");
                File.AppendAllText(path, "</tr>" + Environment.NewLine);

                process.WaitForExit();


            }
            else
            {
                if (globalfunctions.isUnix())
                {
                    String tmpConfigDirectory = Path.Combine(OutputDirectory, "config");
                    Debug.WriteLine(tmpConfigDirectory);
                    Directory.CreateDirectory(tmpConfigDirectory);

                    String sourceConfigDirectory = InputFolder.Text;
                    sourceConfigDirectory = sourceConfigDirectory.Replace("/mods", "/config");

                    DirectoryCopy(sourceConfigDirectory, tmpConfigDirectory, true);

                    Environment.CurrentDirectory = OutputDirectory;
                    startInfo.FileName = "zip";
                    startInfo.Arguments = String.Format("-r \"{0}\" \"config\"", ModpackArchive).Replace("\\", "/");
                }
                else
                {
                    String InputDirectory = InputFolder.Text;
                    InputDirectory = InputDirectory.Replace("\\mods", "\\config");
                    startInfo.Arguments = "a -y \"" + ModpackArchive + "\" \"" + InputDirectory + "\"";
                }
                Debug.WriteLine(startInfo.Arguments);
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
                Debug.WriteLine("FileInfo is not in the database");
            }

            String FileName = File.Replace(DirectoryWithFiles, "").Replace("1.6.4\\", "").Replace("1.7.2\\", "").Replace("1.7.10\\", "").Replace("1.5.2\\", "").Replace("\\", "").Replace(".jar", "").Replace(".zip", "").Replace(".litemod", "").Replace(".disabled", "").Trim();
            FileName = File.Replace(DirectoryWithFiles, "").Replace("1.6.4/", "").Replace("1.7.2/", "").Replace("1.7.10/", "").Replace("1.5.2/", "").Replace("/", "").Replace(".jar", "").Replace(".zip", "").Replace(".litemod", "").Replace(".disabled", "").Trim();
            //Debug.WriteLine(FileName);
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
                    //Debug.WriteLine("What is the mod name of: " + FileName);
                    String a = "Mod name of " + FileName + Environment.NewLine + "Go bug the mod author to include an mcmod.info file!";
                    mod.name = Prompt.ShowDialog(a, "Mod Name");
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
                    //Debug.WriteLine("What is the mod version of: " + FileName);
                    String a = String.Format("Mod version of {0}" + Environment.NewLine + "Go bug the mod author to include an mcmod.info file!", FileName);
                    mod.version = Prompt.ShowDialog(a, "Mod Version");
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
                        //Debug.WriteLine("What is the Minecraft version of: " + FileName);
                        String a = String.Format("Minecraft Version of {0}" + Environment.NewLine + "Go bug the mod author to include an mcmod.info file!", FileName);
                        mod.mcversion = Prompt.ShowDialog(a, "Minecraft Version");
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
                modlink = Prompt.ShowDialog("What is the link to " + mod.name + "?", "Mod link");
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
            String FileName = modfile.Replace(DirectoryWithFiles, "").Replace("1.6.4\\", "").Replace("1.7.2\\", "").Replace("1.7.10\\", "").Replace("1.5.2\\", "").Replace("\\", "").Trim();
            FileName = modfile.Replace(DirectoryWithFiles, "").Replace("1.6.4/", "").Replace("1.7.2/", "").Replace("1.7.10/", "").Replace("1.5.2/", "").Replace("/", "").Trim();
            String modMD5 = SQLHelper.calculateMD5(modfile);
            ModsSQLhelper.addMod(mod.name, mod.modid, mod.version, mod.mcversion, FileName, modMD5, false);
            if (CheckPermissions.Checked)
            {
                PermissionLevel PermLevel = FTBPermsSQLhelper.doFTBHavePermission(mod.modid, PublicFTBPack.Checked, mod.useShortName);
                String overwritelink = "";
                String modLink = "";
                ownPermissions op;
                String customPermissionText = "";
                switch (PermLevel)
                {
                    case PermissionLevel.Open:
                        Debug.WriteLine("Open Permissions for " + mod.name);
                        createTechnicPermissionInfo(mod, PermLevel, null);
                        break;
                    case PermissionLevel.Notify:
                        op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                        if (!op.hasPermission)
                        {
                            overwritelink = Prompt.ShowDialog(mod.name + " requires that you notify the author of inclusion." + Environment.NewLine + "Please provide proof that you have done this:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true);
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
                                    overwritelink = Prompt.ShowDialog(mod.name + " requires that you notify the author of inclusion." + Environment.NewLine + "Please provide proof that you have done this:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true);
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
                            overwritelink = Prompt.ShowDialog("Permissions for " + mod.name + " is FTB exclusive" + Environment.NewLine + "Please provide proof of things being otherwise:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true);
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
                                    overwritelink = Prompt.ShowDialog("Permissions for " + mod.name + " is FTB exclusive" + Environment.NewLine + "Please provide proof of things being otherwise:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true);
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
                                    overwritelink = Prompt.ShowDialog("This mod requires that you request permissions from the Mod Author of " + mod.name + Environment.NewLine + "Please provide proof that you have this permission:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true);
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
                            overwritelink = Prompt.ShowDialog("The FTB permissionsheet states that permissions for " + mod.name + " is closed." + Environment.NewLine + "Please provide proof that this is not the case:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true);
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
                            overwritelink = Prompt.ShowDialog("Permissions for " + mod.name + " is unknown" + Environment.NewLine + "Please provide proof of permissions:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true);
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
                                    overwritelink = Prompt.ShowDialog("Permissions for " + mod.name + " is unknown" + Environment.NewLine + "Please provide proof of permissions:" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true);
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
                                    modLink = Prompt.ShowDialog("Please provide a link to " + mod.name + ":" + Environment.NewLine + "Enter \"skip\" to skip the mod.", mod.name, true);
                                }
                            }
                            String a = getAuthors(mod);
                            createOwnPermissionInfo(mod.name, mod.modid, a, overwritelink, modLink);

                        }
                        op = OwnPermsSQLhelper.doUserHavePermission(mod.modid);
                        customPermissionText = getAuthors(mod) + " has given permission as seen here: " + op.PermissionLink;
                        createTechnicPermissionInfo(mod, PermLevel, customPermissionText, op.ModLink);
                        break;
                    default:
                        Debug.WriteLine("WELLP, something went wrong!!");
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
                        modDir = OutputDirectory + "\\" + mod.modid.Remove(mod.modid.LastIndexOf("|")).Replace(".", string.Empty).ToLower() + "\\mods";
                    }
                    else
                    {
                        modDir = OutputDirectory + "\\" + mod.modid.Replace(".", string.Empty).ToLower() + "\\mods";
                    }
                    if (globalfunctions.isUnix())
                    {
                        modDir = modDir.Replace("\\", "/");
                    }
                    Directory.CreateDirectory(modDir);

                    String tempModFile = modDir + "\\" + FileName;
                    if (globalfunctions.isUnix())
                    {
                        tempModFile = tempModFile.Replace("\\", "/");
                    }
                    tempModFile = tempModFile.Replace("\\\\", "\\");
                    int index = 0;
                    if (globalfunctions.isUnix())
                    {
                        index = tempModFile.LastIndexOf("/");
                    }
                    else
                    {
                        index = tempModFile.LastIndexOf("\\");
                    }
                    String tempFileDirectory = tempModFile.Remove(index);
                    if (globalfunctions.isUnix())
                    {
                        tempFileDirectory = tempFileDirectory.Replace("\\", "/");
                    }
                    Debug.WriteLine(tempFileDirectory);
                    Directory.CreateDirectory(tempFileDirectory);
                    File.Copy(modfile, tempModFile, true);

                    String modArchive = "";
                    if (mod.modid.Contains("|"))
                    {
                        modArchive = OutputDirectory + "\\" + mod.modid.Remove(mod.modid.LastIndexOf("|")).Replace(".", string.Empty).ToLower() + "\\" + mod.modid.Remove(mod.modid.LastIndexOf("|")).Replace(".", string.Empty).ToLower() + "-" + mod.mcversion.ToLower() + "-" + mod.version.ToLower() + ".zip";
                    }
                    else
                    {
                        modArchive = OutputDirectory + "\\" + mod.modid.Replace(".", string.Empty).ToLower() + "\\" + mod.modid.Replace(".", string.Empty).ToLower() + "-" + mod.mcversion.ToLower() + "-" + mod.version.ToLower() + ".zip";
                    }
                    if (globalfunctions.isUnix())
                    {
                        if (mod.modid.Contains("|"))
                        {
                            Environment.CurrentDirectory = OutputDirectory + "/" + mod.modid.Remove(mod.modid.LastIndexOf("|")).Replace(".", string.Empty).ToLower();
                        }
                        else
                        {
                            Environment.CurrentDirectory = OutputDirectory + "/" + mod.modid.Replace(".", string.Empty).ToLower();
                        }
                        modDir = "mods";
                        modArchive.Replace("\\", "/");
                        startInfo.FileName = "zip";
                        startInfo.Arguments = "-r \"" + modArchive + "\" \"" + modDir + "\" ";
                        startInfo.Arguments = startInfo.Arguments.Replace("\\", "/");
                        Debug.WriteLine(startInfo.Arguments);
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
                    String AddedMod = "<tr>";
                    File.AppendAllText(path, AddedMod);
                    AddedMod = "<td><input class=\"containsInfo\" value=\"" + mod.name.Replace("|", "") + "\"></input></td>";
                    File.AppendAllText(path, AddedMod);
                    if (mod.modid.Contains("|"))
                    {
                        AddedMod = "<td><input class=\"containsInfo\" value=\"" + mod.modid.Remove(mod.modid.LastIndexOf("|")).Replace(".", String.Empty).ToLower() + "\"></input></td>";
                    }
                    else
                    {
                        AddedMod = "<td><input class=\"containsInfo\" value=\"" + mod.modid.Replace(".", string.Empty).ToLower() + "\"></input></td>";
                    }
                    File.AppendAllText(path, AddedMod);
                    AddedMod = "<td><input class=\"containsInfo\" value=\"" + mod.mcversion.ToLower() + "-" + mod.version.ToLower() + "\"></input></td>";
                    File.AppendAllText(path, AddedMod);
                    File.AppendAllText(path, "<td><button class=\"Hide\" type=\"button\">Hide</button></td>");
                    File.AppendAllText(path, "</tr>" + Environment.NewLine);

                    process.WaitForExit();

                    Directory.Delete(modDir, true);
                }
                else
                {
                    Debug.WriteLine(mod.name + " is already in the database. skipping..");
                }
            }
            else
            {
                ModsSQLhelper.addMod(mod.name, mod.modid, mod.version, mod.mcversion, FileName, modMD5, false);
                //Debug.WriteLine("Creating big zip file");
                while (String.IsNullOrWhiteSpace(ModpackName))
                {
                    ModpackName = Prompt.ShowDialog("What is the Modpack Name?", "Modpack Name");
                }
                while (String.IsNullOrWhiteSpace(ModpackVersion))
                {
                    ModpackVersion = Prompt.ShowDialog("What Version is the modpack?", "Modpack Version");
                }

                String tempDirectory = String.Format("{0}\\tmp", OutputDirectory);
                String tempModDirectory = String.Format("{0}\\mods", tempDirectory);
                if (globalfunctions.isUnix())
                {
                    tempDirectory = tempDirectory.Replace("\\", "/");
                    tempModDirectory = tempModDirectory.Replace("\\", "/");
                }
                Directory.CreateDirectory(tempModDirectory);
                String tempFile = String.Format("{0}\\{1}", tempModDirectory, FileName);
                if (globalfunctions.isUnix())
                {
                    tempFile = tempFile.Replace("\\", "/");
                }
                tempFile = tempFile.Replace("\\\\", "\\");
                int index = 0;
                if (globalfunctions.isUnix())
                {
                    index = tempFile.LastIndexOf("/");
                }
                else
                {
                    index = tempFile.LastIndexOf("\\");
                }
                String tempFileDirectory = tempFile.Remove(index);
                if (globalfunctions.isUnix())
                {
                    tempFileDirectory = tempFileDirectory.Replace("\\", "/");
                }
                Debug.WriteLine(tempFileDirectory);
                Directory.CreateDirectory(tempFileDirectory);
                File.Copy(modfile, tempFile, true);

                ModpackArchive = String.Format("{0}\\{1}-{2}.zip", OutputDirectory, ModpackName, ModpackVersion);
                if (globalfunctions.isUnix())
                {
                    ModpackArchive.Replace("\\", "/");
                }
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
                    startInfo.Arguments = startInfo.Arguments.Replace("\\", "/");
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
                Debug.WriteLine("Adding mcversion: " + mcversion);
                MCversion.Items.Add(mcversion);
            }
            Debug.WriteLine("Done adding versions");

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
            Debug.WriteLine("DONE");

            foreach (KeyValuePair<String, versions> item in liteloader.versions)
            {
                //Debug.WriteLine(item.Value.artefacts["com.mumfrey:liteloader"].Count);
                foreach (versionclass it in item.Value.artefacts["com.mumfrey:liteloader"].Values)
                {
                    /*Debug.WriteLine("");
                    Debug.WriteLine(it.file);
                    Debug.WriteLine(it.version);
                    Debug.WriteLine(it.md5);
                    Debug.WriteLine(item.Key);*/
                    liteloadersqlhelper.addVersion(it.file, it.version, it.md5, item.Key, it.tweakClass);
                }

            }
        }
    }
}
