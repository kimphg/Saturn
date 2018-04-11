using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace IRS_Demo
{
    public struct Config
    {
        public string recCommand;
        public string recParam;
    }
    public struct SessionData
    {
        public string caseName,caseCode;
        public string suspectName, suspectCode;
        public string inspectorName, inspectorCode;
        public string supervisorName, supervisorCode;
        public string supervisorName2;
        public string supervisorCode2;
    }
    class CommonParam
    {
        public static string ProgramPath = "C:\\Recorder\\";
        public static string ConfigFileName = "config.xml";
        public static string SessionFileName = "sessiondata.xml";
        public static SessionData mSesData;//du lieu cua session
        public static string SessionFolderName;// thu muc ghi video va luu du lieu cua session
        public static Config mConfig;
        public static string UserName;
        public static string videoUrl = "rtsp://admin:admin@192.168.1.2/live3.sdp";
        public static string MjpegUrl = "http://root:root@192.168.1.212/mjpg/video.mjpg";
        
        public static void LoadConfig()
        {
            mConfig = LoadObject<Config>(ConfigFileName);
            if (mConfig.recCommand == null) LoadDefault();
        }

        public static void LoadDefault()
        {
            
            mConfig.recCommand = "C://Program Files (x86)//VideoLAN//VLC//vlc.exe";
            mConfig.recParam = "\""+videoUrl+ "\" --qt-start-minimized --sout=#transcode{vcodec=theo,vb=800,acodec=flac,ab=128,channels=2,samplerate=44100}:file{dst=";
            saveConfig();
        }
        public static void saveSession()
        {
            SaveObject<SessionData>(mSesData, SessionFolderName +"\\"+ SessionFileName);
        }
        public static void saveConfig()
        {
            SaveObject<Config>(mConfig, ConfigFileName);
        }
        public static void SaveObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(ProgramPath + "\\" + fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
                MessageBox.Show(ex.ToString());
            }
        }
        public static T LoadObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(ProgramPath + "\\" + fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                System.Xml.XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(ProgramPath + "\\" + fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                return (T)Activator.CreateInstance(typeof(T));
            }

            return objectOut;
        }
    }
}
