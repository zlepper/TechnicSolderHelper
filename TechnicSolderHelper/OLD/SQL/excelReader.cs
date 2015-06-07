using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using Excel;

namespace TechnicSolderHelper.OLD.SQL
{
    static class ExcelReader
    {
        private const String Permissionsheet = "https://onedrive.live.com/download.aspx?resid=96628E67B4C51B81!161&ithint=file%2cxlsx&app=Excel&authkey=!APQ4QtFrBqa1HwM";
        private static readonly String PermissionsheetFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "permissions.xlsx");

        public static void AddFtbPermissions()
        {
            FtbPermissionsSqlHelper sqlhelper = new FtbPermissionsSqlHelper();
            sqlhelper.ResetTable();

            if (File.Exists(PermissionsheetFile))
            {
                File.Delete(PermissionsheetFile);
            }
            WebClient wb = new WebClient();
            wb.DownloadFile(Permissionsheet, PermissionsheetFile);


            FileStream stream = File.Open(PermissionsheetFile, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet result = excelReader.AsDataSet();
            DataTable curtain = result.Tables["Curtain"];
            DataTable modId = result.Tables["ModID"];

            List<String> modIDs = new List<string>();
            List<String> shortNames = new List<string>();
            for (int modIdCount = 0; modIdCount < modId.Rows.Count; modIdCount++)
            {
                String tmpid = modId.Rows[modIdCount]["column1"].ToString();
                Debug.WriteLine(tmpid);
                String tmpshortName = modId.Rows[modIdCount]["column2"].ToString();
                if (String.IsNullOrWhiteSpace(tmpid) || String.IsNullOrWhiteSpace(tmpshortName)) continue;
                modIDs.Add(tmpid);
                shortNames.Add(tmpshortName);
                sqlhelper.AddFtbModPerm(tmpid, tmpshortName);
            }

            // Read the info from Curtain
            int rCnt = 1;
            while (!(String.IsNullOrWhiteSpace(curtain.Rows[rCnt]["column1"].ToString())))
            {
                Debug.WriteLine(rCnt);
                String name = curtain.Rows[rCnt]["column1"].ToString();
                String author = curtain.Rows[rCnt]["column2"].ToString();
                String Public = curtain.Rows[rCnt]["column4"].ToString();
                String Private = curtain.Rows[rCnt]["column5"].ToString();
                String shortName = curtain.Rows[rCnt]["column3"].ToString();
                String modLink = curtain.Rows[rCnt]["column6"].ToString();
                String permLink = curtain.Rows[rCnt]["column7"].ToString();
                String custPrivate = curtain.Rows[rCnt]["column8"].ToString();
                String custFtb = curtain.Rows[rCnt]["column9"].ToString();

                if (name.Contains("(") && name.Contains(")"))
                {
                    int parentesisStartIndex = name.IndexOf("(", StringComparison.Ordinal);
                    int parentesisEndIndex = name.IndexOf(")", StringComparison.Ordinal);

                    String toBeRemoved = "";
                    for (int i = parentesisStartIndex; i < parentesisEndIndex; i++)
                    {
                        if (!(name[i].Equals('(') || name[i].Equals(')')))
                        {
                            toBeRemoved += name[i];
                        }
                    }
                    shortName = shortName.Replace(toBeRemoved.ToLower(), "");
                    name = name.Remove(parentesisStartIndex, parentesisEndIndex - parentesisStartIndex + 1);
                }

                for (int i = 0; i < modIDs.Count; i++)
                {
                    if (!shortNames[i].Equals(shortName)) continue;
                    String modID = modIDs[i];
                    sqlhelper.AddFtbModPerm(name, author, modID, Public, Private, modLink, permLink, custPrivate, custFtb, shortName);
                }

                rCnt++;
            }

            excelReader.Close();
        }
    }
}
