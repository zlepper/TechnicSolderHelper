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
        public static String SevenZipLocation = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechnicSolderHelper\7za.exe";
        public static Process process = new System.Diagnostics.Process();
        public static ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        public static String UserName, path, CurrentMCVersion, ModpackVersion, ModpackName, ModpackArchive;
		public static ConfigHandler confighandler = new ConfigHandler();
        public static String modlistTextFile = "";

        #endregion

        public SolderHelper()
        { 
            UserName = Environment.UserName;
            InitializeComponent();
			bool firstRun = true;
			if (globalfunctions.isUnix ()) {
				try {
					firstRun = Convert.ToBoolean(confighandler.getConfig("FirstRun"));
				} catch (Exception e) {
					Debug.WriteLine (e.Message);
					firstRun = true;
				}
			} else {
				firstRun = Properties.Settings.Default.FirstRun;
			}
			if (firstRun)
            {
				if (globalfunctions.isUnix ()) {
					confighandler.setConfig ("InputDirectory", Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData) + "/.minecraft/mods");
					confighandler.setConfig ("OutputDirectory", Environment.GetFolderPath (Environment.SpecialFolder.DesktopDirectory) + "/SolderHelper");
					confighandler.setConfig ("FirstRun", "false");
				} else {
					Properties.Settings.Default.InputDirectory = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData) + @"\.minecraft\mods";
					Properties.Settings.Default.OutputDirectory = Environment.GetFolderPath (Environment.SpecialFolder.DesktopDirectory) + @"\SolderHelper";
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
			if (globalfunctions.isUnix ()) {
				try {
				InputFolder.Text = confighandler.getConfig ("InputDirectory");
				} catch(Exception e) {
					Debug.WriteLine (e.Message);
					Debug.Assert(false);
					InputFolder.Text = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData) + "/.minecraft/mods";
				}

				try {
					OutputFolder.Text = confighandler.getConfig ("OutputDirectory");
				} catch (Exception e) {
					Debug.WriteLine(e.Message);
					Debug.Assert(false);
					OutputFolder.Text = Environment.GetFolderPath (Environment.SpecialFolder.DesktopDirectory) + "/SolderHelper";
				}

				try {
					CreateTechnicPack.Checked = Convert.ToBoolean(confighandler.getConfig ("CreateTechnicSolderFiles"));
					SolderPackType.Visible = CreateTechnicPack.Checked;
				} catch (Exception e) {
					Debug.WriteLine(e.Message);
					Debug.Assert(false);
					CreateTechnicPack.Checked = false;
				}

				try {
					CreateFTBPack.Checked = Convert.ToBoolean(confighandler.getConfig ("CreateFTBPack"));
				} catch (Exception e) {
					Debug.WriteLine(e.Message);
					Debug.Assert(false);
					CreateFTBPack.Checked = false;
				}
			} else {
				InputFolder.Text = Properties.Settings.Default.InputDirectory.ToString ();
				OutputFolder.Text = Properties.Settings.Default.OutputDirectory.ToString ();
				CreateTechnicPack.Checked = Properties.Settings.Default.CreateTechnicSolderFiles;
				CreateFTBPack.Checked = Properties.Settings.Default.CreateFTBPack;
			}
            

            #region Reload Interface
			Boolean CSP = true, CPFP = true, TPP = true, IFV = false, ICZ = true, CP = false;
			if(globalfunctions.isUnix()) {
				try {
					CSP = Convert.ToBoolean(confighandler.getConfig("CreateSolderPack"));
				} catch (Exception e) {
					Debug.WriteLine(e.Message);
				}
				try {
					CPFP = Convert.ToBoolean(confighandler.getConfig("CreatePrivateFTBPack"));
				} catch (Exception e) {
					Debug.WriteLine(e.Message);
				}
				try {
					TPP = Convert.ToBoolean(confighandler.getConfig("TechnicPrivatePermissionsLevel"));
				} catch (Exception e) {
					Debug.WriteLine(e.Message);
				}
				try {
					IFV = Convert.ToBoolean(confighandler.getConfig("IncludeForgeVersion"));
				} catch (Exception e) {
					Debug.WriteLine(e.Message);
				}
				try {
					ICZ = Convert.ToBoolean(confighandler.getConfig("CreateTechnicConfigZip"));
				} catch (Exception e) {
					Debug.WriteLine(e.Message);
				}
				try {
					CP = Convert.ToBoolean(confighandler.getConfig("CheckTecnicPermissions"));
				} catch (Exception e) {
					Debug.WriteLine(e.Message);
				}
			} else {
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
			if (globalfunctions.isUnix ()) {
				startInfo.FileName = "unzip";
			} else {
				startInfo.FileName = SevenZipLocation;
			}
        }

        public void CreateFTBPackZip(mcmod mod, string modfile)
        {
            if (false)
            {
                #region Permissions checking
                PermissionLevel PermLevel = FTBPermsSQLhelper.doFTBHavePermission(mod.modid, PublicFTBPack.Checked);
                String ov = "";
                switch (PermLevel)
                {
                    case PermissionLevel.Open:
                        Debug.WriteLine("Open Permissions");
                        break;
                    case PermissionLevel.Notify:
                        if (!OwnPermsSQLhelper.doUserHavePermission(mod.modid).hasPermission)
                        {
                            ov = Prompt.ShowDialog(mod.name + " requires that you notify the author of inclusion." + Environment.NewLine + "Please provide proof that you have done this:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                            while (true)
                            {
                                if (String.IsNullOrWhiteSpace(ov))
                                {
                                    return;
                                }
                                else
                                {
                                    if (Uri.IsWellFormedUriString(ov, UriKind.Absolute))
                                    {
                                        if (ov.ToLower().Contains("imgur"))
                                        {
                                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, ov);
                                            break;
                                        }
                                    }
                                    ov = Prompt.ShowDialog(mod.name + " requires that you notify the author of inclusion." + Environment.NewLine + "Please provide proof that you have done this:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                                }
                            }
                        }
                        break;
                    case PermissionLevel.FTB:
                        Debug.WriteLine("FTB Permissions");
                        break;
                    case PermissionLevel.Request:
                        if (!OwnPermsSQLhelper.doUserHavePermission(mod.modid).hasPermission)
                        {
                            ov = Prompt.ShowDialog("This mod requires that you request permissions from the Mod Author of " + mod.name + Environment.NewLine + "Please provide proof that you have this permission:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                            while (true)
                            {
                                if (String.IsNullOrWhiteSpace(ov))
                                {
                                    return;
                                }
                                else
                                {
                                    if (Uri.IsWellFormedUriString(ov, UriKind.Absolute))
                                    {
                                        if (ov.ToLower().Contains("imgur"))
                                        {
                                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, ov);
                                            break;
                                        }
                                    }
                                    ov = Prompt.ShowDialog("This mod requires that you request permissions from the Mod Author of " + mod.name + Environment.NewLine + "Please provide proof that you have this permission:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                                }
                            }
                        }
                        break;
                    case PermissionLevel.Closed:
                        if (!OwnPermsSQLhelper.doUserHavePermission(mod.modid).hasPermission)
                        {
                            ov = Prompt.ShowDialog("The FTB permissionsheet states that permissions for " + mod.name + " is closed." + Environment.NewLine + "Please provide proof that this is not the case:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                            while (true)
                            {
                                if (String.IsNullOrWhiteSpace(ov))
                                {
                                    return;
                                }
                                else
                                {
                                    if (Uri.IsWellFormedUriString(ov, UriKind.Absolute))
                                    {
                                        if (ov.ToLower().Contains("imgur"))
                                        {
                                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, ov);
                                            break;
                                        }
                                    }
                                    ov = Prompt.ShowDialog("The FTB permissionsheet states that permissions for " + mod.name + " is closed." + Environment.NewLine + "Please provide proof that this is not the case:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                                }
                            }
                        }
                        break;
                    case PermissionLevel.Unknown:
                        if (!OwnPermsSQLhelper.doUserHavePermission(mod.modid).hasPermission)
                        {
                            ov = Prompt.ShowDialog("Permissions for " + mod.name + " is unknown" + Environment.NewLine + "Please provide proof of permissions:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                            while (true)
                            {
                                if (String.IsNullOrWhiteSpace(ov))
                                {
                                    return;
                                }
                                else
                                {
                                    if (Uri.IsWellFormedUriString(ov, UriKind.Absolute))
                                    {
                                        if (ov.ToLower().Contains("imgur"))
                                        {
                                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, ov);
                                            break;
                                        }
                                    }
                                    ov = Prompt.ShowDialog("Permissions for " + mod.name + " is unknown" + Environment.NewLine + "Please provide proof of permissions:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                                }
                            }
                        }
                        break;
                    default:
                        Debug.WriteLine("WELLP, something went wrong!!");
                        break;
                }
                #endregion
            }

            String FileName = modfile.Replace(DirectoryWithFiles, "").Replace("1.6.4\\", "").Replace("1.7.2\\", "").Replace("1.7.10\\", "").Replace("1.5.2\\", "").Replace("\\", "").Trim();
            FileName = modfile.Replace(DirectoryWithFiles, "").Replace("1.6.4/", "").Replace("1.7.2/", "").Replace("1.7.10/", "").Replace("1.5.2/", "").Replace("/", "").Trim();
            String modMD5 = SQLHelper.calculateMD5(modfile);

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

            //String tempDirectory = String.Format("{0}\\minecraft\\tmp", OutputDirectory);
            String tempModDirectory = String.Format("{0}\\minecraft\\mods", OutputDirectory);
            if (globalfunctions.isUnix ()) {
                tempModDirectory = tempModDirectory.Replace ("\\", "/");
            }
            Directory.CreateDirectory(tempModDirectory);
            String tempFile = String.Format("{0}\\{1}", tempModDirectory, FileName);
            if (globalfunctions.isUnix ()) {
                tempFile = tempFile.Replace ("\\", "/");
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

            ModpackArchive = String.Format("{0}\\{1}-{2}.zip", OutputDirectory, ModpackName, ModpackVersion);
            if (globalfunctions.isUnix ()) {
                ModpackArchive.Replace ("\\", "/");
            }
            if (globalfunctions.isUnix())
            {
                Environment.CurrentDirectory = OutputDirectory;
                startInfo.Arguments = String.Format("-r \"{0}\" \"{1}\"", ModpackArchive, "minecraft");
                Debug.WriteLine(startInfo.Arguments);
            }
            else
            {
                startInfo.Arguments = String.Format("a -y \"{0}\" \"{1}\"", ModpackArchive, tempModDirectory);
            }
            if (globalfunctions.isUnix ()) {
                startInfo.FileName = "zip";
                startInfo.Arguments = startInfo.Arguments.Replace ("\\", "/");
            }
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            Directory.Delete(tempModDirectory, true);


            File.AppendAllText(modlistTextFile, mod.name + Environment.NewLine);
        }

        public void Start()
        {
            DirectoryWithFiles = InputFolder.Text;
            OutputDirectory = OutputFolder.Text;
            if (globalfunctions.isUnix())
            {
                Environment.CurrentDirectory = "/";
            }
            else
            {
                Environment.CurrentDirectory = "C:\\";
            }
			if (!Directory.Exists (InputFolder.Text)) {
				MessageBox.Show ("Input directory does not exist!");
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
            
            ModpackVersion = null;
            //Download 7zip dependancy
			if (!globalfunctions.isUnix ()) {
				if (!(Directory.Exists (Environment.GetFolderPath (Environment.SpecialFolder.CommonApplicationData) + @"\TechnicSolderHelper"))) {
					Directory.CreateDirectory (Environment.GetFolderPath (Environment.SpecialFolder.CommonApplicationData) + @"\TechnicSolderHelper");
				}
			}
			if (!(globalfunctions.isUnix() || File.Exists(SevenZipLocation)))
            {
                WebClient wb = new WebClient();
                System.Uri SevenWeb = new Uri("http://cloud.zlepper.dk/7za.exe");
                wb.DownloadFile(SevenWeb, SevenZipLocation);
            }

			if (globalfunctions.isUnix ()) {
				confighandler.setConfig ("InputDirectory", InputFolder.Text);
				confighandler.setConfig ("OutputDirectory", OutputFolder.Text);
			} else {
				Properties.Settings.Default.InputDirectory = InputFolder.Text;
				Properties.Settings.Default.OutputDirectory = OutputFolder.Text;
				Properties.Settings.Default.Save ();
			}

            path = OutputDirectory + @"\mods.html";
			if (globalfunctions.isUnix ()) {
				path = path.Replace ("\\", "/");
			}
            
            
            Directory.CreateDirectory(OutputDirectory);
			Environment.CurrentDirectory = OutputDirectory;
			Debug.WriteLine (Environment.CurrentDirectory);
            if(globalfunctions.isUnix()) {
                modlistTextFile = OutputDirectory + "/modlist.txt";
            } else {
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
                         "</head>" + Environment.NewLine + "<body><table border='1'><tr><th>Modname</th><th>Modslug</th><th>Version</th></tr>" + Environment.NewLine;
                File.WriteAllText(path, htmlfile);
            }
            

            // Create array with all the mod locations
            List<String> files = new List<String>();

            // Add the different mod files to the files array
            foreach (String file in Directory.GetFiles(DirectoryWithFiles, "*.zip", SearchOption.AllDirectories))
            {
                files.Add(file);
                //Debug.WriteLine(file);
            }
            foreach (String file in Directory.GetFiles(DirectoryWithFiles, "*.jar", SearchOption.AllDirectories))
            {
                files.Add(file);
                //Debug.WriteLine(file);
            }
            foreach (String file in Directory.GetFiles(DirectoryWithFiles, "*.litemod", SearchOption.AllDirectories))
            {
                files.Add(file);
                //Debug.WriteLine(file);
            }

            //Check if files have already been added
            foreach (String file in files)
            {
                String FileName = file.Replace(DirectoryWithFiles, "");
                ProgressLabel.Text = FileName;
                //Check for mcmod.info
                Directory.CreateDirectory(OutputDirectory);
				String Arguments = "";
				if (globalfunctions.isUnix ()) {
					startInfo.FileName = "unzip";
					Arguments = "-o \"" + file + "\" \"*.info\" \"*.json\" -d \"" + OutputDirectory + "\"";
					Debug.WriteLine (Arguments);
				} else {
					Arguments = "e " + "-y -o\"" + OutputDirectory + "\" \"" + file + "\" *.info litemod.json";
				}
                //Debug.WriteLine(Arguments);
                startInfo.Arguments = Arguments;

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                String mcmodfile = OutputDirectory + @"\mcmod.info";
                String litemodfile = OutputDirectory + @"\litemod.json";
				if (globalfunctions.isUnix ()) {
					litemodfile = litemodfile.Replace ("\\", "/");
					mcmodfile = mcmodfile.Replace ("\\", "");
				}
                if (File.Exists(litemodfile))
                {
                    if (File.Exists(mcmodfile))
                    {
                        File.Delete(mcmodfile);
                    }
                    File.Move(litemodfile, mcmodfile);
                }
                foreach (String mcmodfiles in Directory.GetFiles(OutputDirectory, "*.info"))
                {
                    if (mcmodfiles.Contains("dependencies"))
                    {
                        File.Delete(mcmodfiles);
                    }
                    else
                    {
                        if (mcmodfiles.Equals(mcmodfile))
                        {
                            File.Move(mcmodfiles, mcmodfile);
                        }
                        else
                        {
                            File.Delete(mcmodfile);
                            File.Move(mcmodfiles, mcmodfile);

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
                            mcmod2 modinfo2 = JsonConvert.DeserializeObject<mcmod2>(json);
                                    
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
                                catch (Exception)
                                {
                                    //Debug.WriteLine(e.Message);
                                    //Debug.WriteLine(e.InnerException);
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
                                        if(mod.modid.Contains("|")) {
                                            mod.modid = mod.modid.Remove(mod.modid.LastIndexOf("|"));
                                        }
                                        if (isFullyInformed(mod))
                                        {
                                            if (CreateTechnicPack.Checked) {
                                                CreateTechnicModZip(mod, file);
                                            }
                                            if (CreateFTBPack.Checked) {
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
                            catch (Exception e)
                            {
                                Debug.WriteLine(e.Message);
                                litemod liteloadermod = JsonConvert.DeserializeObject<litemod>(json);

                                //Convert into mcmod
                                mcmod mod = new mcmod();
                                mod.mcversion = liteloadermod.mcversion;
                                mod.modid = liteloadermod.name.ToLower().Replace(" ", "");
                                mod.name = liteloadermod.name;

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
                    catch (Exception)
                    {
                        requireUserInfo(file);
                    }
                    File.Delete(mcmodfile);
                }
                else
                {
					String fileName = file.Replace(DirectoryWithFiles, "").Replace("1.6.4\\", "").Replace("1.7.2\\", "").Replace("1.7.10\\", "").Replace("1.5.2\\", "").Replace("\\", "").Trim();
					fileName = file.Replace(DirectoryWithFiles, "").Replace("1.6.4/", "").Replace("1.7.2/", "").Replace("1.7.10/", "").Replace("1.5.2/", "").Replace("\\", "").Trim();
                    int fixNr = IsWierdMod(fileName);
                    if ( fixNr != 0 )
                    {
                        mcmod mod;
                        switch (fixNr)
                        {
                                //Not enough items
                            case 1:
                                mod = ModHelper.NotEnoughItems(fileName);
                                requireUserInfo(mod, file);
                                break;
                                //CoFHLib
                            case 5:
                                mod = ModHelper.CoFHLib(fileName);
                                requireUserInfo(mod, file);
                                break;
                                //Code chicken core
                            case 6:
                                mod = ModHelper.CodeChickenCore(fileName);
                                requireUserInfo(mod, file);
                                break;
                                //Liteloader
                            case 7:
                                mod = ModHelper.Liteloader(fileName);
                                requireUserInfo(mod, file);
                                break;
                            case 9:
                                mod = ModHelper.GoodVersioning(fileName);
                                requireUserInfo(mod, file);
                                break;
                            case 10:
                            case 11:
                                mod = ModHelper.iChunMod(fileName);
                                requireUserInfo(mod, file);
                                break;
                            case 12:
                                mod = ModHelper.waila(fileName);
                                requireUserInfo(mod, file);
                                break;
                            case 13:
                                mod = ModHelper.ReikasMods(fileName);
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
            }

            if (IncludeConfigZip.Checked)
            {
                createConfigZip();
            }
            if (IncludeForgeVersion.Checked)
            {
                string selectedBuild = ForgeBuild.SelectedItem.ToString();
                Debug.WriteLine(selectedBuild);
                Number forgeinfo = forgesqlhelper.getForgeInfo(selectedBuild);
                String tmpdir = OutputDirectory + "\\bin";
				if (globalfunctions.isUnix ()) {
					tmpdir = tmpdir.Replace ("\\", "/");
				}
                Directory.CreateDirectory(tmpdir);
                String tempfile = tmpdir + "\\modpack.jar";
				if (globalfunctions.isUnix ()) {
					tempfile = tempfile.Replace ("\\", "/");
				}
                WebClient wb = new WebClient();
				Debug.WriteLine ("Downloading: " + forgeinfo.downloadurl);
                wb.DownloadFile(forgeinfo.downloadurl, tempfile);
				if (globalfunctions.isUnix ()) {
					Directory.CreateDirectory (OutputDirectory + "/forge");
				} else {
					Directory.CreateDirectory (OutputDirectory + "\\forge");
				}
                if (SolderPack.Checked)
                {
                    String outputfile = OutputDirectory + "\\forge\\forge-" + forgeinfo.version + ".zip";
					if (globalfunctions.isUnix ()) {
						outputfile = outputfile.Replace ("\\", "/");
						startInfo.FileName = "zip";
						Environment.CurrentDirectory = OutputDirectory;
						startInfo.Arguments = "-r \"" + outputfile + "\" \"bin\"";
					} else {
						startInfo.Arguments = "a -y \"" + outputfile + "\" \"" + tmpdir + "\"";
					}
                    String AddedMod = "<tr>";
                    File.AppendAllText(path, AddedMod);
                    AddedMod = "<td>Minecraft Forge</td>";
                    File.AppendAllText(path, AddedMod);
                    AddedMod = "<td>forge</td>";
                    File.AppendAllText(path, AddedMod);
                    AddedMod = "<td>" + forgeinfo.version + "</td>";
                    File.AppendAllText(path, AddedMod.ToLower());
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
            if (SolderPack.Checked)
            {
                File.AppendAllText(path, "</table><p>List autogenerated by TechnicSolderHelper &copy; 2014 - Rasmus Hansen</p></body></html>");
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
            ProgressLabel.Text = "Waiting...";
            
        }

        #region Technic Pack Function

        /// <summary>
        /// Checks if the mod is on the list of mods which has custom support.
        /// </summary>
        /// <param name="modFileName">The mod file name.</param>
        /// <returns>Returns the number of the method to call, if no match is found, returns zero</returns>
        private static int IsWierdMod(String modFileName) {
            String[] wierdMods = {"NotEnoughItems", 
                                     "CarpentersBlocksCachedResources", 
                                     "CodeChickenLib", 
                                     "ForgeMultipart", 
                                     "CoFHLib", 
                                     "CodeChickenCore", 
                                     "liteloader",
                                     "bspkrsCore-IsNowNeeded",
                                     "Morpheus",
                                     "Morph", 
                                     "PiP",
                                     "Waila",
                                     "Reaikas mods hereyadaytaad",
                                     "INpureProject"};
            for (int i = 0; i < wierdMods.Length; i++)
            {
                if (modFileName.ToLower().Contains(wierdMods[i].ToLower()))
                {
                    //Return the number we are on, plus 1 to make sure we call the right function
                    return i + 1;
                }
            }
            String ReikasModsPattern = @"[a-z]+ 1.[0-9].[0-9]* V[0-9]*[a-z]*";

            if (Regex.IsMatch(modFileName, ReikasModsPattern, RegexOptions.IgnoreCase))
            {
                return 13;
            }
            return 0;
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
				if (globalfunctions.isUnix ()) {
					InputDirectory = InputDirectory.Replace("/mods", "").Replace ("\\", "/");
				}
                OutputDirectory = OutputFolder.Text;
                String ConfigFileName = Prompt.ShowDialog("What do you want the file name of the config " + Environment.NewLine + "folder to be?", "Config FileInfo Name");
                if (ConfigFileName.EndsWith(".zip"))
                {
                    ConfigFileName = ConfigFileName.Replace(".zip", "");
                }
                String ConfigVersion = Prompt.ShowDialog("What is the config version?", "Config Version");
                String ConfigFileZipName = ConfigFileName + "-" + ConfigVersion;
                if (!(ConfigFileZipName.EndsWith(".zip")))
                {
                    ConfigFileZipName = ConfigFileName + ".zip";
                }
				if (globalfunctions.isUnix ()) {
					startInfo.FileName = "zip";
					Directory.CreateDirectory (OutputDirectory + "/" + ConfigFileName);
					Environment.CurrentDirectory = InputDirectory;
					startInfo.Arguments = "-r \"" + OutputDirectory + "/" + ConfigFileName + "/" + ConfigFileZipName + "\" \"config\"";
				} else {
					startInfo.Arguments = "a -y \"" + OutputDirectory + "\\" + ConfigFileName + "\\" + ConfigFileZipName + "\" \"" + InputDirectory + "\\config" + "\"";
				}
				startInfo.Arguments = startInfo.Arguments.Replace ("\\", "/");
                process.StartInfo = startInfo;
                process.Start();

                String AddedMod = "<tr>";
                File.AppendAllText(path, AddedMod);
                AddedMod = "<td>" + ConfigFileName + "</td>";
                File.AppendAllText(path, AddedMod);
                AddedMod = "<td>" + ConfigFileName + "</td>";
                File.AppendAllText(path, AddedMod);
                AddedMod = "<td>" + ConfigVersion + "</td>";
                File.AppendAllText(path, AddedMod.ToLower());
                File.AppendAllText(path, "</tr>" + Environment.NewLine);

                process.WaitForExit();


            }
            else
            {
                String InputDirectory = InputFolder.Text;
                InputDirectory = InputDirectory.Replace("\\mods", "\\config");
				if (globalfunctions.isUnix ()) {
					InputDirectory.Replace ("\\", "/");
				}
                
                startInfo.Arguments = "a -y \"" + ModpackArchive + "\" \"" + InputDirectory + "\"";
                Debug.WriteLine(startInfo.Arguments);
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
            

        }

        public void requireUserInfo(mcmod currentData, String File)
        {

            mcmod mod = new mcmod();

            try
            {
                mod = ModsSQLhelper.getModInfo(SQLHelper.calculateMD5(File));
            }
            catch (Exception)
            {
                Debug.WriteLine("FileInfo is not in the database");
            }

			String FileName = File.Replace(DirectoryWithFiles, "").Replace("1.6.4\\", "").Replace("1.7.2\\", "").Replace("1.7.10\\", "").Replace("1.5.2\\", "").Replace("\\", "").Trim();
			FileName = File.Replace(DirectoryWithFiles, "").Replace("1.6.4/", "").Replace("1.7.2/", "").Replace("1.7.10/", "").Replace("1.5.2/", "").Replace("\\", "").Trim();
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
                mod.modid = currentData.modid.ToLower();
            }
            else
            {
                mod.modid = mod.name.Replace(" ", "").ToLower();
            }

            if (mod.modid.Contains('|'))
            {
                mod.modid = mod.modid.Substring(0, mod.modid.IndexOf('|'));
            }

            if (CreateTechnicPack.Checked) {
                CreateTechnicModZip(mod, File);
            }
            if (CreateFTBPack.Checked) {
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

        public void CreateTechnicModZip(mcmod mod, String modfile)
        {
            if (CheckPermissions.Checked)
            {
                #region Permissions checking
                PermissionLevel PermLevel = FTBPermsSQLhelper.doFTBHavePermission(mod.modid, TechnicPublicPermissions.Checked);
                String ov = "";
                switch (PermLevel)
                {
                    case PermissionLevel.Open:
                        Debug.WriteLine("Open Permissions");
                        break;
                    case PermissionLevel.Notify:
                        if (!OwnPermsSQLhelper.doUserHavePermission(mod.modid).hasPermission)
                        {
                            ov = Prompt.ShowDialog(mod.name + " requires that you notify the author of inclusion." + Environment.NewLine + "Please provide proof that you have done this:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                            while (true)
                            {
                                if (String.IsNullOrWhiteSpace(ov))
                                {
                                    return;
                                }
                                else
                                {
                                    if (Uri.IsWellFormedUriString(ov, UriKind.Absolute))
                                    {
                                        if (ov.ToLower().Contains("imgur"))
                                        {
                                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, ov);
                                            break;
                                        }
                                    }
                                    ov = Prompt.ShowDialog(mod.name + " requires that you notify the author of inclusion." + Environment.NewLine + "Please provide proof that you have done this:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                                }
                            }
                        }
                        break;
                    case PermissionLevel.FTB:
                        if (!OwnPermsSQLhelper.doUserHavePermission(mod.modid).hasPermission)
                        {
                            ov = Prompt.ShowDialog("Currently only FTB has permissions to distribute " + mod.name + Environment.NewLine + "Please provide proof that this is not the case:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                            while (true)
                            {
                                if (String.IsNullOrWhiteSpace(ov))
                                {
                                    return;
                                }
                                else
                                {
                                    if (Uri.IsWellFormedUriString(ov, UriKind.Absolute))
                                    {
                                        if (ov.ToLower().Contains("imgur"))
                                        {
                                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, ov);
                                            break;
                                        }
                                    }
                                    ov = Prompt.ShowDialog("Currently only FTB has permissions to distribute " + mod.name + Environment.NewLine + "Please provide proof that this is not the case:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                                }
                            }
                        }
                        break;
                    case PermissionLevel.Request:
                        if (!OwnPermsSQLhelper.doUserHavePermission(mod.modid).hasPermission)
                        {
                            ov = Prompt.ShowDialog("This mod requires that you request permissions from the Mod Author of " + mod.name + Environment.NewLine + "Please provide proof that you have this permission:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                            while (true)
                            {
                                if (String.IsNullOrWhiteSpace(ov))
                                {
                                    return;
                                }
                                else
                                {
                                    if (Uri.IsWellFormedUriString(ov, UriKind.Absolute))
                                    {
                                        if (ov.ToLower().Contains("imgur"))
                                        {
                                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, ov);
                                            break;
                                        }
                                    }
                                    ov = Prompt.ShowDialog("This mod requires that you request permissions from the Mod Author of " + mod.name + Environment.NewLine + "Please provide proof that you have this permission:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                                }
                            }
                        }
                        break;
                    case PermissionLevel.Closed:
                        if (!OwnPermsSQLhelper.doUserHavePermission(mod.modid).hasPermission)
                        {
                            ov = Prompt.ShowDialog("The FTB permissionsheet states that permissions for " + mod.name + " is closed." + Environment.NewLine + "Please provide proof that this is not the case:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                            while (true)
                            {
                                if (String.IsNullOrWhiteSpace(ov))
                                {
                                    return;
                                }
                                else
                                {
                                    if (Uri.IsWellFormedUriString(ov, UriKind.Absolute))
                                    {
                                        if (ov.ToLower().Contains("imgur"))
                                        {
                                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, ov);
                                            break;
                                        }
                                    }
                                    ov = Prompt.ShowDialog("The FTB permissionsheet states that permissions for " + mod.name + " is closed." + Environment.NewLine + "Please provide proof that this is not the case:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                                }
                            }
                        }
                        break;
                    case PermissionLevel.Unknown:
                        if (!OwnPermsSQLhelper.doUserHavePermission(mod.modid).hasPermission)
                        {
                            ov = Prompt.ShowDialog("Permissions for " + mod.name + " is unknown" + Environment.NewLine + "Please provide proof of permissions:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                            while (true)
                            {
                                if (String.IsNullOrWhiteSpace(ov))
                                {
                                    return;
                                }
                                else
                                {
                                    if (Uri.IsWellFormedUriString(ov, UriKind.Absolute))
                                    {
                                        if (ov.ToLower().Contains("imgur"))
                                        {
                                            OwnPermsSQLhelper.addOwnModPerm(mod.name, mod.modid, ov);
                                            break;
                                        }
                                    }
                                    ov = Prompt.ShowDialog("Permissions for " + mod.name + " is unknown" + Environment.NewLine + "Please provide proof of permissions:" + Environment.NewLine + "Leave this line empty to skip the mod.", mod.name);
                                }
                            }
                        }
                        break;
                    default:
                        Debug.WriteLine("WELLP, something went wrong!!");
                        break;
                }
                #endregion
            }
			String FileName = modfile.Replace(DirectoryWithFiles, "").Replace("1.6.4\\", "").Replace("1.7.2\\", "").Replace("1.7.10\\", "").Replace("1.5.2\\", "").Replace("\\", "").Trim();
			FileName = modfile.Replace(DirectoryWithFiles, "").Replace("1.6.4/", "").Replace("1.7.2/", "").Replace("1.7.10/", "").Replace("1.5.2/", "").Replace("/", "").Trim();
            String modMD5 = SQLHelper.calculateMD5(modfile);
            if (SolderPack.Checked)
            {
                if (!ModsSQLhelper.IsFileInSolder(modfile))
                {
                    String modDir = OutputDirectory + "\\" + mod.modid.ToLower().Replace("|", "") + "\\mods";
					if (globalfunctions.isUnix ()) {
						modDir = modDir.Replace ("\\", "/");
					}
                    Directory.CreateDirectory(modDir);

					String tempModFile = modDir + "\\" + FileName;
					if (globalfunctions.isUnix ()) {
						tempModFile = tempModFile.Replace ("\\", "/");
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

                    String modArchive = OutputDirectory + "\\" + mod.modid.ToLower() + "\\" + mod.modid.ToLower() + "-" + mod.mcversion.ToLower() + "-" + mod.version.ToLower() + ".zip";
					if (globalfunctions.isUnix ()) {
						Environment.CurrentDirectory = OutputDirectory + "/" + mod.modid.ToLower().Replace("|", "");
						modDir = "mods";
						modArchive.Replace ("\\", "/");
						startInfo.FileName = "zip";
						startInfo.Arguments = "-r \"" + modArchive + "\" \"" + modDir + "\" ";
						startInfo.Arguments = startInfo.Arguments.Replace("\\", "/");
						Debug.WriteLine (startInfo.Arguments);
					} else {
						startInfo.Arguments = "a -y \"" + modArchive + "\" \"" + modDir + "\" ";
					}
                    process.StartInfo = startInfo;
                    process.Start();

                    //Save mod to database
                    ModsSQLhelper.addMod(mod.name, mod.modid, mod.version, mod.mcversion, FileName, modMD5, true);

                    // Add mod info to a html file
                    String AddedMod = "<tr>";
                    File.AppendAllText(path, AddedMod);
                    AddedMod = "<td>" + mod.name.Replace("|", "") + "</td>";
                    File.AppendAllText(path, AddedMod);
                    AddedMod = "<td>" + mod.modid.ToLower().Replace("|", "") + "</td>";
                    File.AppendAllText(path, AddedMod);
                    AddedMod = "<td>" + mod.mcversion + "-" + mod.version + "</td>";
                    File.AppendAllText(path, AddedMod.ToLower());
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
				if (globalfunctions.isUnix ()) {
					tempDirectory = tempDirectory.Replace ("\\", "/");
					tempModDirectory = tempModDirectory.Replace ("\\", "/");
				}
                Directory.CreateDirectory(tempModDirectory);
                String tempFile = String.Format("{0}\\{1}", tempModDirectory, FileName);
				if (globalfunctions.isUnix ()) {
					tempFile = tempFile.Replace ("\\", "/");
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
				if (globalfunctions.isUnix ()) {
					ModpackArchive.Replace ("\\", "/");
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
				if (globalfunctions.isUnix ()) {
                    startInfo.FileName = "zip";
					startInfo.Arguments = startInfo.Arguments.Replace ("\\", "/");
				}
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                Directory.Delete(tempDirectory, true);
            }

            File.AppendAllText(modlistTextFile, mod.name + Environment.NewLine);

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
				if (globalfunctions.isUnix ()) {
					confighandler.setConfig ("InputDirectory", InputFolder.Text);
				} else {
					Properties.Settings.Default.InputDirectory = InputFolder.Text;
					Properties.Settings.Default.Save ();
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
				if (globalfunctions.isUnix ()) {
					confighandler.setConfig ("OutputDirectory", OutputFolder.Text);
				} else {
					Properties.Settings.Default.OutputDirectory = OutputFolder.Text;
					Properties.Settings.Default.Save ();
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
			if (globalfunctions.isUnix ()) {
				confighandler.setConfig ("InputDirectory", InputFolder.Text);
			} else {
				Properties.Settings.Default.InputDirectory = InputFolder.Text;
				Properties.Settings.Default.Save ();
			}
        }

        private void OutputFolder_TextChanged(object sender, EventArgs e)
		{
			if (globalfunctions.isUnix ()) {
				confighandler.setConfig ("OutputDirectory", OutputFolder.Text);
			} else {
				Properties.Settings.Default.OutputDirectory = OutputFolder.Text;
				Properties.Settings.Default.Save ();
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
			if (globalfunctions.isUnix ()) {
				confighandler.setConfig ("CreateFTBPack", CreateFTBPack.Checked);
			} else {
				Properties.Settings.Default.CreateFTBPack = CreateFTBPack.Checked;
				Properties.Settings.Default.Save ();
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
			if (globalfunctions.isUnix ()) {
				confighandler.setConfig ("CreatePrivateFTBPack", PrivateFTBPack.Checked);
			} else {
				Properties.Settings.Default.CreatePrivateFTBPack = PrivateFTBPack.Checked;
				Properties.Settings.Default.Save ();
			}
        }

        #endregion

        #region Technic Packs

        private void CreateTechnicPack_CheckedChanged(object sender, EventArgs e)
        {
			if (globalfunctions.isUnix ()) {
				confighandler.setConfig ("CreateTechnicSolderFiles", CreateTechnicPack.Checked);
			} else {
				Properties.Settings.Default.CreateTechnicSolderFiles = CreateTechnicPack.Checked;
				Properties.Settings.Default.Save ();
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
            }
            else
            {
                SolderPackType.Hide();
                DistributionLevel.Location = new Point(DistributionLevel.Location.X, DistributionLevel.Location.Y - SolderPackType.Height);
                CreateFTBPack.Location = new Point(CreateFTBPack.Location.X, CreateFTBPack.Location.Y - SolderPackType.Height);
                TechnicDistributionLevel.Hide();
            }
        }

        private void IncludeConfigZip_CheckedChanged(object sender, EventArgs e)
        {
			if (globalfunctions.isUnix ()) {
				confighandler.setConfig ("CreateTechnicConfigZip", IncludeConfigZip.Checked);
			} else {
				Properties.Settings.Default.CreateTechnicConfigZip = IncludeConfigZip.Checked;
				Properties.Settings.Default.Save ();
			}
        }

        private void SolderPack_CheckedChanged(object sender, EventArgs e)
        {
			if (globalfunctions.isUnix ()) {
				confighandler.setConfig ("CreateSolderPack", SolderPack.Checked);
			} else {
				Properties.Settings.Default.CreateSolderPack = SolderPack.Checked;
				Properties.Settings.Default.Save ();
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
			if (globalfunctions.isUnix ()) {
				confighandler.setConfig ("CheckTecnicPermissions", CheckPermissions.Checked);
			} else {
				Properties.Settings.Default.CheckTecnicPermissions = CheckPermissions.Checked;
				Properties.Settings.Default.Save ();
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
			if (globalfunctions.isUnix ()) {
				confighandler.setConfig ("TechnicPrivatePermissionsLevel", TechnicPrivatePermissions.Checked);
			} else {
				Properties.Settings.Default.TechnicPrivatePermissionsLevel = TechnicPrivatePermissions.Checked;
				Properties.Settings.Default.Save ();
			}
        }

        #endregion

        private void UploadToFTPServer_CheckedChanged(object sender, EventArgs e)
        {
			if (globalfunctions.isUnix ()) {
				confighandler.setConfig ("UploadToFTPServer", UploadToFTPServer.Checked);
			} else {
				Properties.Settings.Default.UploadToFTPServer = UploadToFTPServer.Checked;
				Properties.Settings.Default.Save ();
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
			if (globalfunctions.isUnix ()) {
				confighandler.setConfig ("IncludeForgeVersion", IncludeForgeVersion.Checked);
			} else {
				Properties.Settings.Default.IncludeForgeVersion = IncludeForgeVersion.Checked;
				Properties.Settings.Default.Save ();
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
    }
}
