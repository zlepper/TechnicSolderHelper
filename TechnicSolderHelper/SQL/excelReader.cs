using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using System.IO;
using Excel;
using System.Diagnostics;
using System.Data;
using TechnicSolderHelper;

namespace TechnicSolderHelper.SQL
{
    class excelReader
    {
        private static String permissionsheet = "https://onedrive.live.com/download.aspx?resid=96628E67B4C51B81!161&ithint=file%2cxlsx&app=Excel&authkey=!APQ4QtFrBqa1HwM";
        private static String permissionsheetFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SolderHelper\permissions.xlsx";

        public static void addFTBPermissions()
        {
            if (globalfunctions.isUnix())
            {
                permissionsheetFile.Replace("\\", "/");
            }
            FTBPermissionsSQLHelper sqlhelper = new FTBPermissionsSQLHelper();
            sqlhelper.resetTable();

            if (File.Exists(permissionsheetFile))
            {
                File.Delete(permissionsheetFile);
            }
            WebClient wb = new WebClient();
            wb.DownloadFile(permissionsheet, permissionsheetFile);


            FileStream stream = File.Open(permissionsheetFile, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet result = excelReader.AsDataSet();
            DataTable Curtain = result.Tables["Curtain"];
            DataTable ModID = result.Tables["ModID"];

            List<String> modIDs = new List<string>();
            List<String> shortNames = new List<string>();
            for (int modIdCount = 0; modIdCount < ModID.Rows.Count; modIdCount++)
            {
                String tmpid = ModID.Rows[modIdCount]["column1"].ToString();
                Debug.WriteLine(tmpid);
                String tmpshortName = ModID.Rows[modIdCount]["column2"].ToString();
                if (!(String.IsNullOrWhiteSpace(tmpid)) && !(String.IsNullOrWhiteSpace(tmpshortName)))
                {
                    modIDs.Add(tmpid);
                    shortNames.Add(tmpshortName);
                }
            }

            // Read the info from Curtain
            int rCnt = 1;
            while (!(String.IsNullOrWhiteSpace(Curtain.Rows[rCnt]["column1"].ToString())))
            {
                Debug.WriteLine(rCnt);
                String Name = Curtain.Rows[rCnt]["column1"].ToString();
                String Author = Curtain.Rows[rCnt]["column2"].ToString();
                String Public = Curtain.Rows[rCnt]["column4"].ToString();
                String Private = Curtain.Rows[rCnt]["column5"].ToString();
                String shortName = Curtain.Rows[rCnt]["column3"].ToString();
                String modLink = Curtain.Rows[rCnt]["column6"].ToString();
                String permLink = Curtain.Rows[rCnt]["column7"].ToString();
                String CustPrivate = Curtain.Rows[rCnt]["column8"].ToString();
                String CustFTB = Curtain.Rows[rCnt]["column9"].ToString();

                if (Name.Contains("(") && Name.Contains(")"))
                {
                    int parentesisStartIndex = Name.IndexOf("(");
                    int parentesisEndIndex = Name.IndexOf(")");

                    String toBeRemoved = "";
                    for (int i = parentesisStartIndex; i < parentesisEndIndex; i++)
                    {
                        if (!(Name[i].Equals('(') || Name[i].Equals(')')))
                        {
                            toBeRemoved += Name[i];
                        }
                    }
                    shortName = shortName.Replace(toBeRemoved.ToLower(), "");
                    Name = Name.Remove(parentesisStartIndex, parentesisEndIndex - parentesisStartIndex + 1);
                }

                for (int i = 0; i < modIDs.Count; i++)
                {
                    if (shortNames[i].Equals(shortName))
                    {
                        String modID = modIDs[i];
                        sqlhelper.addFTBModPerm(Name, Author, modID, Public, Private, modLink, permLink, CustPrivate, CustFTB);
                    }
                }

                rCnt++;
            }

            excelReader.Close();
            //MessageBox.Show("DONE!!!");
        }
    }
}
