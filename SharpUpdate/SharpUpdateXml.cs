using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.Diagnostics;

namespace SharpUpdate
{
    public class SharpUpdateXml
    {
        private Version version;
        private Uri uri;
        private string fileName;
        private string md5;
        private string description;
        private string launchArgs;

        public SharpUpdateXml(Version version, Uri uri, string fileName, string md5, string description, string launchArgs)
        {
            this.version = version;
            this.uri = uri;
            this.fileName = fileName;
            this.md5 = md5;
            this.description = description;
            this.launchArgs = launchArgs;
        }

        public Version Version
        {
            get { return version; }
        }

        public Uri Uri
        {
            get { return uri; }
        }

        public string FileName
        {
            get { return fileName; }
        }

        public string MD5
        {
            get { return md5; }
        }

        public string Description
        {
            get { return description; }
        }

        public string LaunchArgs
        {
            get { return launchArgs; }
        }

        public bool InNewerThan(Version version)
        {
            return this.version > version;
        }

        public static bool ExitsOnServer(Uri location)
        {
            Debug.WriteLine("location.AbsoluteUri => " + location.AbsoluteUri);
            try
            {
                HttpWebRequest req =  (HttpWebRequest) WebRequest.Create(location.AbsoluteUri);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                return  res.StatusCode == HttpStatusCode.OK;
            }
            catch { return false; }
        }

        public static SharpUpdateXml Parse(Uri location, string appId)
        {
            Version version = null;
            string url = "", fileName = "", md5 = "", description = "", launchArgs = "";

            try
            {

                XmlDocument doc = new XmlDocument();
                doc.Load(location.AbsoluteUri);

                XmlNode node = doc.DocumentElement.SelectSingleNode("//update[@appId='" + appId + "']");
                if (node == null) return null;

                version = Version.Parse(node["version"].InnerText);
                url = node["url"].InnerText;
                fileName = node["fileName"].InnerText;
                md5 = node["md5"].InnerText;
                description = node["description"].InnerText;
                launchArgs = node["launchArgs"].InnerText;

                return new SharpUpdateXml(version, location, fileName, md5, description, launchArgs);
            }
            catch
            {
                return null;
            }
        }

    }
}
