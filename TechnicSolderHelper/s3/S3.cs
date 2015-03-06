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
        private readonly AmazonS3Client _client;
        private readonly String _bucket;

        public S3()
        {
            ConfigHandler ch = new ConfigHandler();
            Crypto crypto = new Crypto();
            var accessKey = crypto.DecryptString(ch.GetConfig("S3accessKey"));
            var secretKey = crypto.DecryptString(ch.GetConfig("S3secretKey"));
            String url = ch.GetConfig("S3url");
            _bucket = ch.GetConfig("S3Bucket");
            var config = new AmazonS3Config {ServiceURL = url};
            _client = AWSClientFactory.CreateAmazonS3Client(accessKey, secretKey, config) as AmazonS3Client;
        }

        public S3(string accessKey, string secretKey, String serviceUrl)
        {
            var config = new AmazonS3Config(){ServiceURL = serviceUrl};
            _client = AWSClientFactory.CreateAmazonS3Client(accessKey, secretKey, config) as AmazonS3Client;
        }

        public List<String> GetBucketList()
        {
            try
            {
                
                ListBucketsResponse responce = _client.ListBuckets();
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
                _client.PutBucket(request);
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
                BucketName = _bucket,
                Directory = folderPath,
                SearchOption = SearchOption.AllDirectories,
                SearchPattern = "*.zip",
                KeyPrefix = "mods"
            };
            TransferUtility directorytTransferUtility = new TransferUtility(_client);
            directorytTransferUtility.UploadDirectory(request);
            MessageBox.Show("Done uploading files to s3");
        }
    }
}
