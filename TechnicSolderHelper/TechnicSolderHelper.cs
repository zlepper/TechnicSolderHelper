using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using System.Net;
using Newtonsoft.Json.Serialization;
using TechnicSolderHelper.SQL;
using System.Text.RegularExpressions;
using System.Configuration;

namespace TechnicSolderHelper
{
    public partial class SolderHelper : Form
    {
        public static String DirectoryWithFiles;
        public static String OutputDirectory;
        public SQLhelper ModsSQLhelper = new SQLhelper("mod");
        public SQLhelper FTBPermsSQLhelper = new SQLhelper("ftbperms");
        public SQLhelper OwnPermsSQLhelper = new SQLhelper("ownperms");
        public static String SevenZipLocation = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechnicSolderHelper\7za.exe";
        //public static String SevenZipLocation = "7za.exe";
        public static Process process = new System.Diagnostics.Process();
        public static ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        public static ManualResetEvent mre = new ManualResetEvent(false);
        public static String UserName;
        public static String path;
        public static String CurrentMCVersion;

        public SolderHelper()
        { 
            UserName = Environment.UserName;
            InitializeComponent();
            Debug.WriteLine(Properties.Settings.Default.FirstRun);
            if (Properties.Settings.Default.FirstRun)
            {
                Properties.Settings.Default.InputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\.minecraft\mods";
                Properties.Settings.Default.FirstRun = false;
                Properties.Settings.Default.Save();
            }
            InputFolder.Text = Properties.Settings.Default.InputDirectory.ToString(); //Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\.minecraft\mods" //InputFolder.Text.Replace("Users\\User", "Users\\"+Environment.UserName);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = SevenZipLocation;
        }

        public void Start()
        {
            //Download 7zip dependancy
            if (!(Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechnicSolderHelper")))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechnicSolderHelper");
            }
            if (!File.Exists(SevenZipLocation))
            {
                WebClient wb = new WebClient();
                System.Uri SevenWeb = new Uri("http://cloud.zlepper.dk/7za.exe");
                wb.DownloadFile(SevenWeb, SevenZipLocation);
            }
            

            

            DirectoryWithFiles = InputFolder.Text;
            OutputDirectory = OutputFolder.Text;
            Properties.Settings.Default.InputDirectory = InputFolder.Text;
            Properties.Settings.Default.OutputDirectory = OutputFolder.Text;
            path = OutputDirectory + @"\mods.html";
            if (checkBox1.Checked)
            {
                if (Directory.Exists(OutputDirectory))
                {
                    Directory.Delete(OutputDirectory, true);
                }
            }
            
            Directory.CreateDirectory(OutputDirectory);

            String htmlfile = "<!DOCTYPE html> \n <html> <head>"+ Environment.NewLine +
                         "<title>Mods</title>"+ Environment.NewLine+
	                     "<meta charset=\"utf-8\" />" + Environment.NewLine+
                         "<script src=\"https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js\"></script>"+ Environment.NewLine +
                         "<script src=\"http://cloud.zlepper.dk/technicsolderhelper.js\"></script>"+ Environment.NewLine +
                         //"<script src=\"http://rasmus.phpwizard.dk/jquery.min.js\"></script>" + Environment.NewLine +
                         //"<script src=\"http://rasmus.phpwizard.dk/technicsolderhelper.js\"></script>" + Environment.NewLine +
                         "</head>" + Environment.NewLine + "<body><table border='1'><tr><th>Modname</th><th>Modslug</th><th>Version</th></tr>" + Environment.NewLine;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.AppendAllText(path, htmlfile);

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
                if (!ModsSQLhelper.IsFileInDatabase(SQLhelper.calculateMD5(file)))
                {
                    //Check for mcmod.info
                    Directory.CreateDirectory(OutputDirectory);
                    String Arguments = "e " + "-y -o\"" + OutputDirectory + "\" \"" + file + "\" mcmod.info litemod.json";
                    //Debug.WriteLine(Arguments);
                    startInfo.Arguments = Arguments;

                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                    String mcmodfile = OutputDirectory + @"\mcmod.info";
                    String litemodfile = OutputDirectory + @"\litemod.json";
                    if (File.Exists(litemodfile))
                    {
                        if (File.Exists(mcmodfile))
                        {
                            File.Delete(mcmodfile);
                        }
                        File.Move(litemodfile, mcmodfile);
                    }
                    if (File.Exists(mcmodfile))
                    {
                        if (file.Contains("bspkrsCore"))
                        {
                            //Debug.WriteLine("skipping file: " + file);
                        }
                        else
                        {
                            //If exist, then read info and make zip file
                            using (StreamReader r = new StreamReader(mcmodfile))
                            {
                                String json = r.ReadToEnd();
                                //modlist modlistversion;
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
                                            //Debug.WriteLine("Deffenetly not version 2");
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

                                                    if (isFullyInformed(mod))
                                                    {
                                                        CreateTechnicModZip(mod, file);
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
                                            //Debug.WriteLine("Maybe litemod?");
                                            litemod liteloadermod = JsonConvert.DeserializeObject<litemod>(json);
                                            //Debug.WriteLine("litemod.");

                                            //Convert into mcmod
                                            mcmod mod = new mcmod();
                                            mod.mcversion = liteloadermod.mcversion;
                                            mod.modid = liteloadermod.name.ToLower().Replace(" ", "");
                                            mod.name = liteloadermod.name;
                                            //mod.version = liteloadermod.version + "-" + liteloadermod.revision;

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
                            }
                        }
                        File.Delete(mcmodfile);
                    }
                    else
                    {
                        String fileName = file.Replace(DirectoryWithFiles, "").Replace("1.6.4\\", "").Replace("1.7.2\\", "").Replace("1.7.10\\", "").Replace("1.5.2\\", "").Replace("\\", "").Trim();
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
                else
                {
                    //Debug.WriteLine(FileName + " is already in the database. skipping..");
                }



            }

            if (IncludeConfigZip.Checked)
            {
                createConfigZip();
            }
            File.AppendAllText(path, "</table><p>List autogenerated by TechnicSolderHelper &copy; 2014 - Rasmus Hansen</p></body></html>");
            MessageBox.Show("Done");
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
            String InputDirectory = InputFolder.Text;
            InputDirectory = InputDirectory.Replace("\\mods", "");
            OutputDirectory = OutputFolder.Text;
            String ConfigFileName = Prompt.ShowDialog("What do you want the file name of the config " + Environment.NewLine + "folder to be?", "Config File Name");
            if (!(ConfigFileName.EndsWith(".zip")))
	        {
                ConfigFileName = ConfigFileName + ".zip";
            }
            startInfo.Arguments = "a -y \"" + OutputDirectory + "\\" + ConfigFileName + "\" \"" + InputDirectory + "\\config" + "\"";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

        }

        public void requireUserInfo(mcmod currentData, String File)
        {

            mcmod mod = new mcmod();

            String FileName = File.Replace(DirectoryWithFiles, "").Replace("1.6.4\\", "").Replace("1.7.2\\", "").Replace("1.7.10\\", "").Replace("1.5.2\\", "").Replace("\\", "").Trim();
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
                //Debug.WriteLine("What is the mod name of: " + FileName);
                String a = "Mod name of " + FileName + Environment.NewLine + "Go bug the mod author to include an mcmod.info file!";
                mod.name = Prompt.ShowDialog(a, "Mod Name");
                if (mod.name.Equals(""))
                {
                    return;
                }
            }

            if (currentData.version != null)
            {
                mod.version = currentData.version.Replace(" ", "+").ToLower();
            }
            else
            {
                //Debug.WriteLine("What is the mod version of: " + FileName);
                String a = String.Format("Mod version of {0}" + Environment.NewLine + "Go bug the mod author to include an mcmod.info file!", FileName);
                mod.version = Prompt.ShowDialog(a, "Mod Version");
                mod.version = mod.version.Replace(" ", "+").ToLower();
            }

            if (currentData.mcversion != null)
            {
                mod.mcversion = currentData.mcversion;
            }
            else
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

            CreateTechnicModZip(mod, File);
        }

        public void requireUserInfo(String FileName)
        {
            mcmod mod = new mcmod();
            mod.mcversion = null;
            mod.modid = null;
            mod.name = null;
            mod.version = null;
            requireUserInfo(mod, FileName);
        }

        public void CreateTechnicModZip(mcmod mod, String modfile)
        {

            String FileName = modfile.Replace(DirectoryWithFiles, "").Replace("1.6.4\\", "").Replace("1.7.2\\", "").Replace("1.7.10\\", "").Replace("1.5.2\\", "").Replace("\\", "").Trim();
            String modDir = OutputDirectory + "\\" + mod.modid.ToLower().Replace("|", "") + "\\mods";
            Directory.CreateDirectory(modDir);

            String tempModFile = modDir + "\\" + FileName;
            //Debug.WriteLine("Copying " + modfile + " to " + tempModFile);
            File.Copy(modfile, tempModFile, true);


            String modArchive = OutputDirectory + "\\" + mod.modid.ToLower() + "\\" + mod.modid.ToLower() + "-" + mod.mcversion.ToLower() + "-" + mod.version.ToLower() + ".zip";
            startInfo.Arguments = "a -y \"" + modArchive + "\" \"" + modDir + "\" "/* + ">> C:\\Users\\Rasmus\\Desktop\\error.txt"*/;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();


            Directory.Delete(modDir, true);

            //Save mod to database
            String modMD5 = SQLhelper.calculateMD5(modfile);
            ModsSQLhelper.addDoneMod(mod.name, mod.modid, mod.version, mod.mcversion, FileName, modMD5);

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

        }

        private void InputDirectoryBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = InputFolder.Text;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                InputFolder.Text = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.InputDirectory = InputFolder.Text;
                //Debug.WriteLine("Set InputDirectory to: " + Properties.Settings.Default.InputDirectory);
                Properties.Settings.Default.Save();
            }

        }

        private void OutputDirectoryBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = OutputFolder.Text;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                OutputFolder.Text = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.OutputDirectory = OutputFolder.Text;
                //Debug.WriteLine("Set OutputDirectory to: " + Properties.Settings.Default.OutputDirectory);
                Properties.Settings.Default.Save();
            }
            
        }

        public void button1_Click(object sender, EventArgs e)
        {
            Start();
        }

        public void button2_Click(object sender, EventArgs e)
        {
            ModsSQLhelper.DropTable();
            ModsSQLhelper = new SQLhelper("Mod");
        }

        private void InputFolder_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.InputDirectory = InputFolder.Text;
            Properties.Settings.Default.Save();
        }

        private void OutputFolder_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.OutputDirectory = OutputFolder.Text;
            Properties.Settings.Default.Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            excelReader.addFTBPermissions();
        }

        private void CreateFTBPack_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

}
