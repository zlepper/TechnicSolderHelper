using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using TechnicSolderHelper.Confighandler;
using TechnicSolderHelper.cryptography;

namespace TechnicSolderHelper.s3
{
    class S3
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly AmazonS3Client client;
        public String Bucket;

        public S3()
        {
            ConfigHandler ch = new ConfigHandler();
            Crypto crypto = new Crypto();
            _accessKey = crypto.DecryptString(ch.GetConfig("S3accessKey"));
            _secretKey = crypto.DecryptString(ch.GetConfig("S3secretKey"));
            String url = ch.GetConfig("S3url");
            Bucket = ch.GetConfig("S3Bucket");
            var config = new AmazonS3Config {ServiceURL = url};
            client = AWSClientFactory.CreateAmazonS3Client(_accessKey, _secretKey, config) as AmazonS3Client;
        }

        public S3(string accessKey, string secretKey, String serviceUrl)
        {
            _accessKey = accessKey;
            _secretKey = secretKey;
            var config = new AmazonS3Config(){ServiceURL = serviceUrl};
            client = AWSClientFactory.CreateAmazonS3Client(_accessKey, _secretKey, config) as AmazonS3Client;
        }

        public List<String> GetBucketList()
        {
            try
            {
                ListBucketsResponse responce = client.ListBuckets();
                return responce.Buckets.Select(bucket => bucket.BucketName).ToList();
            }
            catch
            {
                return null;
            }
        }

        public void CreateNewBucket(String bucketName)
        {
            try
            {
                PutBucketRequest request = new PutBucketRequest {BucketName = bucketName};
                client.PutBucket(request);
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format("Could not create bucket:\n{0}", exception.Message));
            }
        }

        public void UploadFolder(String folderPath)
        {
            MessageToUser m = new MessageToUser();
            Thread startingThread = new Thread(m.UploadToS3);
            startingThread.Start();
            TransferUtilityUploadDirectoryRequest request = new TransferUtilityUploadDirectoryRequest()
            {
                BucketName = Bucket,
                Directory = folderPath,
                SearchOption = SearchOption.AllDirectories,
                SearchPattern = "*.zip",
                KeyPrefix = "mods"
            };
            TransferUtility directorytTransferUtility = new TransferUtility(client);
            directorytTransferUtility.UploadDirectory(request);
            MessageBox.Show("Done uploading files to s3");
        }
    }
}
