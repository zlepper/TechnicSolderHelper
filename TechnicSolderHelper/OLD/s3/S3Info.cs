using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TechnicSolderHelper.OLD.confighandler;
using TechnicSolderHelper.OLD.cryptography;

namespace TechnicSolderHelper.OLD.s3
{
    public partial class S3Info : Form
    {
        private S3 _service;

        public S3Info()
        {
            InitializeComponent();
        }

        private void test_Click(object sender, EventArgs e)
        {
            if (!(serviceURL.Text.StartsWith("http://") || serviceURL.Text.StartsWith("https://")))
            {
                serviceURL.Text = "http://" + serviceURL.Text;
            }
            if (IsEveryFilledIn())
            {
                if (!Uri.IsWellFormedUriString(serviceURL.Text, UriKind.Absolute))
                {
                    MessageBox.Show("Service url is not valid");
                    return;
                }

                _service = new S3(accessKey.Text, secretKey.Text, serviceURL.Text);
                try
                {
                    _service.GetBucketList();
                    if (sender != null)
                    {
                        MessageBox.Show("Connection Succesful.");
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(string.Format("Connection not succesful.\n{0}", exception.Message));
                }
                GetBuckets();
            }
            else
            {
                MessageBox.Show("Please fill in everything.");
            }
        }

        private bool test_Click()
        {
            if (!(serviceURL.Text.StartsWith("http://") ||serviceURL.Text.StartsWith("https://")))
            {
                serviceURL.Text = "http://" + serviceURL.Text;
            }
            if (IsEveryFilledIn())
            {
                if (!Uri.IsWellFormedUriString(serviceURL.Text, UriKind.Absolute))
                {
                    MessageBox.Show("Service url is not valid");
                    return false;
                }

                _service = new S3(accessKey.Text, secretKey.Text, serviceURL.Text);
                try
                {
                    _service.GetBucketList();
                    GetBuckets();
                    return true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(string.Format("Connection not succesful.\n{0}", exception.Message));
                }
                
            }
            else
            {
                MessageBox.Show("Please fill in everything.");
            }
            return false;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GetBuckets()
        {
            List<String> bucketList = _service.GetBucketList();
            buckets.Items.Clear();
            if (bucketList != null)
            {
                foreach (string bucket in bucketList)
                {
                    buckets.Items.Add(bucket);
                }
            }
        }

        private bool IsEveryFilledIn()
        {
            if (String.IsNullOrWhiteSpace(accessKey.Text) || String.IsNullOrWhiteSpace(serviceURL.Text) ||String.IsNullOrWhiteSpace(secretKey.Text))
            {
                return false;
            }
            return true;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (IsEveryFilledIn())
            {
                if (buckets.SelectedItem != null)
                {
                    String bucket = buckets.SelectedItem.ToString();
                    ConfigHandler ch = new ConfigHandler();
                    Crypto crypto = new Crypto();
                    ch.SetConfig("S3url", serviceURL.Text);
                    ch.SetConfig("S3accessKey", crypto.EncryptToString(accessKey.Text));
                    ch.SetConfig("S3secretKey", crypto.EncryptToString(secretKey.Text));
                    ch.SetConfig("S3Bucket", bucket);
                    Close();
                }
                else
                {
                    MessageBox.Show("Please select a bucket");
                }
            }
        }

        private void S3Info_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigHandler ch = new ConfigHandler();
                Crypto crypto = new Crypto();
                serviceURL.Text = ch.GetConfig("S3url");
                accessKey.Text = crypto.DecryptString(ch.GetConfig("S3accessKey"));
                secretKey.Text = crypto.DecryptString(ch.GetConfig("S3secretKey"));
            }
            catch
            {
                // ignored
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_service != null)
            {
                if (String.IsNullOrWhiteSpace(newBucketName.Text))
                {
                    MessageBox.Show("You need to enter a name of the new bucket.");
                }
                else
                {
                    _service.CreateNewBucket(newBucketName.Text);
                    test_Click();
                    buckets.SelectedItem = newBucketName.Text;
                }
            }
            else
            {
                test_Click();
            }
        }
    }
}
