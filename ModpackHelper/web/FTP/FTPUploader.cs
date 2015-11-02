using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Net;
using ModpackHelper.Shared.Utils.Config;

namespace ModpackHelper.Shared.Web.FTP
{
    public delegate void CurrentFileProgressChangedEventHandler(double progress);

    public delegate void CurrentWorkingFileChangedEventHandler(string filename);

    public class FTPUploader
    {
        /// <summary>
        /// Happens when the upload progress changes.
        /// </summary>
        public event CurrentFileProgressChangedEventHandler FileProgressChanged;

        /// <summary>
        /// Happens when a new upload is started
        /// </summary>
        public event CurrentWorkingFileChangedEventHandler WorkingFileChanged;

        protected virtual void OnFileProgressChanged(double progress)
        {
            FileProgressChanged?.Invoke(progress);
        }

        protected virtual void OnWorkingFileChanged(string filename)
        {
            WorkingFileChanged?.Invoke(filename);
        }

        /// <summary>
        /// The username to used to login to the FTP service
        /// </summary>
        private readonly string username;
        /// <summary>
        /// The password used to login
        /// </summary>
        private readonly string password;
        /// <summary>
        /// The url to the service
        /// </summary>
        private readonly string url;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Creates a new ftp file uploader
        /// </summary>
        /// <param name="ftpLoginInfo">The login info for the server</param>
        public FTPUploader(FTPLoginInfo ftpLoginInfo) : this(ftpLoginInfo, new FileSystem()) { }

        /// <summary>
        /// Creates a new ftp file uploader
        /// </summary>
        /// <param name="ftpLoginInfo">The login info for the server</param>
        /// <param name="fileSystem">The filesystem the FTPUploader should work against</param>
        public FTPUploader(FTPLoginInfo ftpLoginInfo, IFileSystem fileSystem)
        {
            if (!url.ToLower().StartsWith("ftp://"))
            {
                throw new ArgumentException("url has to be a ftp:// url");
            }
            username = ftpLoginInfo.Username;
            password = ftpLoginInfo.Password;
            url = ftpLoginInfo.Address;
            this.fileSystem = fileSystem;
        }

        /// <summary>
        /// Uploads a specific folder to the FTP service
        /// </summary>
        /// <param name="folder"></param>
        public void UploadFolder(DirectoryInfoBase folder)
        {
            if (!folder.Exists) return;
            FileInfoBase[] files = folder.GetFiles("*.*", SearchOption.AllDirectories);

            foreach (FileInfoBase file in files)
            {
                UploadFile(file.FullName, file.FullName.Replace(folder.FullName + Path.DirectorySeparatorChar, ""), "mods");
            }

        }

        /// <summary>
        /// Gets the content of a directory on the FTP service
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private List<string> GetDirectoryContent(string location)
        {
            List<string> folderContent = new List<string>();
            FtpWebRequest request;
            if (location == null)
            {
                request = (FtpWebRequest)WebRequest.Create(url);
            }
            else
            {
                request = (FtpWebRequest)WebRequest.Create(location);
            }
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            request.Credentials = new NetworkCredential(username, password);

            using (FtpWebResponse responce = (FtpWebResponse)request.GetResponse())
            {
                Stream responceStream = responce.GetResponseStream();
                if (responceStream == null) return folderContent;
                using (StreamReader reader = new StreamReader(responceStream))
                {
                    while (true)
                    {
                        try
                        {
                            string s = reader.ReadLine();
                            if (s == null) continue;
                            if (s.Contains("/"))
                            {
                                s = s.Substring(s.LastIndexOf("/", StringComparison.Ordinal) + 1);
                            }
                            folderContent.Add(s);
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                }
            }

            return folderContent;
        }

        private void UploadFile(string fullyQualifiedPathName, string destinationOnServer, string subdir)
        {
            // Ensures we can put the files in subdirs
            if (subdir != null)
            {
                destinationOnServer = subdir + Path.DirectorySeparatorChar + destinationOnServer;
            }
            // All the subfolders we should create for the file to be correctly uploaded
            List<string> folders = destinationOnServer.Split(Path.DirectorySeparatorChar).ToList();
            string fileToUpload = folders[folders.Count - 1];
            // The last string is the file itself, which we don't want
            folders.Remove(folders[folders.Count - 1]);
            // Remove any "/" chars
            for (int i = 0; i < folders.Count; i++)
            {
                folders[i] = folders[i].Replace(Path.DirectorySeparatorChar.ToString(), "");
            }

            // Create a requst to the FTP server
            FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
            if (request != null)
            {
                // Login to the server
                request.Credentials = new NetworkCredential(username, password);
                // Create all the subfolders up to the file
                foreach (string folder in folders)
                {
                    Debug.Assert(request != null, "request != null");
                    List<string> directoryContent = GetDirectoryContent(request.RequestUri.ToString());
                    if (directoryContent.Contains(folder))
                    {
                        if (request.RequestUri.ToString().EndsWith("/"))
                        {
                            request = WebRequest.Create(request.RequestUri + folder) as FtpWebRequest;
                        }
                        else
                        {
                            request = WebRequest.Create(request.RequestUri + "/" + folder) as FtpWebRequest;
                        }
                    }
                    else
                    {
                        if (request.RequestUri.ToString().EndsWith("/"))
                        {
                            request = WebRequest.Create(request.RequestUri + folder) as FtpWebRequest;
                        }
                        else
                        {
                            request = WebRequest.Create(request.RequestUri + "/" + folder) as FtpWebRequest;
                        }
                        Debug.Assert(request != null, "request != null");
                        // Create the directory since it doesn't exist
                        request.Credentials = new NetworkCredential(username, password);
                        request.Method = WebRequestMethods.Ftp.MakeDirectory;
                        try
                        {
                            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        }
                        catch (WebException)
                        {

                        }
                    }
                }

                try
                {
                    // Try to upload the file itself
                    Debug.Assert(request != null, "request != null");
                    request = WebRequest.Create(request.RequestUri + "/" + fileToUpload) as FtpWebRequest;
                    Debug.Assert(request != null, "request != null");
                    request.Credentials = new NetworkCredential(username, password);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.UseBinary = true;
                    request.UsePassive = true;
                    request.KeepAlive = true;
                    using (Stream fs = fileSystem.File.OpenRead(fullyQualifiedPathName))
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        // Ensured that we can get upload progress
                        byte[] buffer = new byte[1024 * 100];
                        int totalReadBytesCount = 0;
                        int readBytesCount;
                        while ((readBytesCount = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            requestStream.Write(buffer, 0, readBytesCount);
                            // Calculate progress
                            totalReadBytesCount += readBytesCount;
                            double progress = totalReadBytesCount * 100.0 / requestStream.Length;
                            OnFileProgressChanged(progress);
                        }

                    }
                }
                catch (WebException)
                {
                    //Console.WriteLine("error getting responce");
                }
            }
        }
    }
}
