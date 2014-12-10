using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using System.Security.Cryptography;

namespace TechnicSolderHelper
{
    public class Ftp
    {
        private String userName;
        private String password;
        private String url;

        public Ftp()
        {
            if (globalfunctions.isUnix())
            {
                ConfigHandler ch = new ConfigHandler();
                url = ch.getConfig("ftpUrl");
                password = ch.getConfig("ftpPassword");
                userName = ch.getConfig("ftpUserName");
            }
            else
            {
                url = Properties.Settings.Default.ftpUrl;
                userName = Properties.Settings.Default.ftpUserName;
                password = Properties.Settings.Default.ftpPassword;
            }
            if (url.EndsWith("/"))
            {
                url.Remove(url.Length - 1);
            }
            if (!url.StartsWith("ftp://"))
            {
                url = "ftp://" + url;
            }
        }

        public void uploadFolder(String folderPath)
        {
            String[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

            foreach (String file in files)
            {
                Debug.WriteLine("");
                Debug.WriteLine(file);
                Debug.WriteLine(file.Replace(folderPath + globalfunctions.pathSeperator, ""));
                uploadFile(file, file.Replace(folderPath + globalfunctions.pathSeperator, ""), "mods");
            }

        }

        public void uploadFile(String FullyQualifiedPathName, String destinationOnServer, String constant)
        {
            if (constant != null)
            {
                destinationOnServer = constant + globalfunctions.pathSeperator + destinationOnServer;
            }
            String[] tmp = destinationOnServer.Split(globalfunctions.pathSeperator);
            List<String> folders = new List<String>(tmp);
            String fileToUpload = folders[folders.Count - 1];
            folders.Remove(folders[folders.Count - 1]);
            for (int i = 0; i < folders.Count; i++)
            {
                folders[i] = folders[i].Replace(globalfunctions.pathSeperator.ToString(), "");
            }
            FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
            request.Credentials = new NetworkCredential(this.userName, this.password);
            Debug.WriteLine(request.RequestUri);
            foreach (String folder in folders)
            {
                Debug.WriteLine(folder);
                if (request.RequestUri.ToString().EndsWith("/"))
                {
                    request = WebRequest.Create(request.RequestUri + folder) as FtpWebRequest;
                }
                else
                {
                    request = WebRequest.Create(request.RequestUri + "/" + folder) as FtpWebRequest;
                }
                Debug.WriteLine(request.RequestUri);
                request.Credentials = new NetworkCredential(this.userName, this.password);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                try
                {
                    FtpWebResponse responce = (FtpWebResponse)request.GetResponse();
                    Debug.WriteLine(responce.StatusDescription);
                }
                catch (System.Net.WebException e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            try
            {
                Debug.WriteLine("Uploading file: " + fileToUpload);
                request = WebRequest.Create(request.RequestUri + "/" + fileToUpload) as FtpWebRequest;
                request.Credentials = new NetworkCredential(this.userName, this.password);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = true;
                using (FileStream fs = File.OpenRead(FullyQualifiedPathName))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Close();
                    requestStream.Flush();
                }
            }
            catch (System.Net.WebException e)
            {
                //Console.WriteLine("error getting responce");
                Debug.WriteLine(e.Message);
            }
        }

    }
}

