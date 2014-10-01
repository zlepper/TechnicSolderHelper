using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace TechnicSolderHelper.SQL
{
    class excelReader
    {
        private static String permissionsheet = "https://onedrive.live.com/download.aspx?resid=96628E67B4C51B81!161&ithint=file%2cxlsx&app=Excel&authkey=!APQ4QtFrBqa1HwM";
        private static String permissionsheetFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\permissions.xlsx";

        public static void addFTBPermissions()
        {
            SQLhelper sqlhelper = new SQLhelper("ftbperms");
            sqlhelper.DropTable();
            sqlhelper = new SQLhelper("ftbperms");

            if (File.Exists(permissionsheetFile)) 
            {
                File.Delete(permissionsheetFile);
            }
            WebClient wb = new WebClient();
            wb.DownloadFile(permissionsheet, permissionsheetFile);

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet CurtainWorksheet, ModIDWorksheet;
            Excel.Range CurtainRange, ModIDRange;


            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(permissionsheetFile, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            CurtainWorksheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item("Curtain");
            ModIDWorksheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item("ModID");

            CurtainRange = CurtainWorksheet.UsedRange;
            ModIDRange = ModIDWorksheet.UsedRange;
            Debug.WriteLine(ModIDRange.Rows.Count);
            Debug.WriteLine("");

            // Read the info from ModID
            List<String> modIDs = new List<string>();
            List<String> shortNames = new List<string>();
            for (int modIdCount = 1; modIdCount <= ModIDRange.Rows.Count; modIdCount++)
            {
                String tmpid = (String)(ModIDRange.Cells[modIdCount, 1] as Excel.Range).Value2;
                String tmpshortName = (String)(ModIDRange.Cells[modIdCount, 2] as Excel.Range).Value2;
                if (!(String.IsNullOrWhiteSpace(tmpid)) && !(String.IsNullOrWhiteSpace(tmpshortName)))
                {
                    modIDs.Add(tmpid);
                    shortNames.Add(tmpshortName);
                }
            }

            List<excelPermSheet> perms = new List<excelPermSheet>();
            // Read the info from Curtain
            int rCnt = 2;
            while (!(String.IsNullOrWhiteSpace((string)(CurtainRange.Cells[rCnt, 1] as Excel.Range).Value2)))
            {
                Debug.WriteLine(rCnt);
                String Name = (string)(CurtainRange.Cells[rCnt, 1] as Excel.Range).Value2;
                String Author = (string)(CurtainRange.Cells[rCnt, 2] as Excel.Range).Value2;
                String tmpPublic = (String)(CurtainRange.Cells[rCnt, 4] as Excel.Range).Value2;
                String tmpPrivate = (String)(CurtainRange.Cells[rCnt, 5] as Excel.Range).Value2;

                String shortName = (String)(CurtainRange.Cells[rCnt, 3] as Excel.Range).Value2;


                if (Name.Contains("(") && Name.Contains(")"))
                {
                    int parentesisStartIndex = Name.IndexOf("(");
                    int parentesisEndIndex = Name.IndexOf(")");

                    char[] nameArray = Name.ToCharArray();

                    String toBeRemoved = "";
                    for (int i = parentesisStartIndex; i < parentesisEndIndex; i++)
                    {
                        if (!(Name[i].Equals('(') || Name[i].Equals(')')))
                        {
                            toBeRemoved += Name[i];
                        }
                    }
                    shortName = shortName.Replace(toBeRemoved.ToLower(), "");
                    Name = Name.Remove(parentesisStartIndex, parentesisEndIndex-parentesisStartIndex+1);
                }

                PermissionLevel Public = PermissionLevel.Unknown;
                switch (tmpPublic)
                {
                    case "Open":
                        Public = PermissionLevel.Open;
                        break;
                    case "Closed":
                        Public = PermissionLevel.Closed;
                        break;
                    case "FTB":
                        Public = PermissionLevel.FTB;
                        break;
                    case "Notify":
                        Public = PermissionLevel.Notify;
                        break;
                    case "Request":
                        Public = PermissionLevel.Request;
                        break;
                    default:
                        Public = PermissionLevel.Unknown;
                        break;
                }

                PermissionLevel Private = PermissionLevel.Unknown;
                switch (tmpPrivate)
                {
                    case "Open":
                        Private = PermissionLevel.Open;
                        break;
                    case "Closed":
                        Private = PermissionLevel.Closed;
                        break;
                    case "FTB":
                        Private = PermissionLevel.FTB;
                        break;
                    case "Notify":
                        Private = PermissionLevel.Notify;
                        break;
                    case "Request":
                        Private = PermissionLevel.Request;
                        break;
                    default:
                        Private = PermissionLevel.Unknown;
                        break;
                }

                for (int i = 0; i < modIDs.Count; i++)
			    {
                    if (shortNames[i].Equals(shortName))
	                {
                        String modID = modIDs[i];
                        sqlhelper.addFTBModPerm(Name, Author, modID, Public.ToString(), Private.ToString());
	                }
			    }

                rCnt++;
            }


            xlWorkBook.Close();
            xlApp.Quit();

            releaseObject(CurtainWorksheet);
            releaseObject(ModIDWorksheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            MessageBox.Show("DONE!!!");

        }

        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        } 
    }

    class excelPermSheet
    {
        public String ModName { get; set; }
        public String Author { get; set; }
        public List<String> ModID { get; set; }
        public PermissionLevel PublicPerm { get; set; }
        public PermissionLevel PrivatePerm { get; set; }
    }
}
