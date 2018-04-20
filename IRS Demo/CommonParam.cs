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
        public string videoUrl;
    }

    public struct SuspectData
    {
        public string _Ten, _MaDT;
        public string _GioiTinh, _TenGoiKhac, _NgaySinh, _NoiSinh, _QuocTich, _DanToc, _TonGiao, _NgheNghiep;
        public string _CMND, _NgayCapCMND, _NoiCapCMND;
        public string _DiaChi;
    }
    

    public struct SessionData
    {
        public string caseName, caseCode;
        
        public string inspectorName, inspectorCode;
        public string supervisorName, supervisorCode;
        public string supervisorName2;
        public string supervisorCode2;
        public string Notes;

        public string beginSessTime;
        public string endSessTime;

        public string currentPlace;
        public string currentDate;

        public SuspectData suspectData;

        public string SessionPath {get;set;}
        public string SessionKeyText { get; set; }

        
      
    }
    class CommonParam
    {
        public static string ProgramPath = "C:\\IRS_Data\\";
        public static string ConfigFileName = "config.xml";
        public static string SessionFileName = "sessiondata.xml";
        public static SessionData mSesData;//du lieu cua session
        public static string SessionFolderName;// thu muc ghi video va luu du lieu cua session
        public static Config mConfig;
        public static string UserName;
        //public static string videoUrl = "rtsp://admin:admin@192.168.1.2/live3.sdp";
        //public static string videoUrl = "rtsp://root:root@192.168.1.218/axis-media/media.amp";
        public static string MjpegUrl = "http://root:root@192.168.1.212/mjpg/video.mjpg";
        

        public static void GetSuspectInfo()
        {   
            mSesData.suspectData._GioiTinh = "Nam"; 
            mSesData.suspectData._TenGoiKhac = "Không có";
            mSesData.suspectData._NgaySinh = "30/06/1985";
            mSesData.suspectData._NoiSinh = "Hưng Yên";            
            mSesData.suspectData._QuocTich = "Việt Nam";
            mSesData.suspectData._DanToc = "Kinh";
            mSesData.suspectData._TonGiao = "Không";
            mSesData.suspectData._NgheNghiep = "Tự do";
            mSesData.suspectData._CMND = "123456789";
            mSesData.suspectData._NgayCapCMND = "28/2/2005";
            mSesData.suspectData._NoiCapCMND = "Công an tỉnh Hưng Yên";
            mSesData.suspectData._DiaChi = "Đống Đa - Hà Nội";            
        }
        public static void LoadConfig()
        {            
            mConfig = LoadObject<Config>(ProgramPath + "\\" + ConfigFileName);
            if (mConfig.recCommand == null) LoadDefault();
        }

        public static void LoadDefault()
        {            
            mConfig.recCommand = "C://Program Files (x86)//VideoLAN//VLC//vlc.exe";
            mConfig.videoUrl = "rtsp://admin:admin@192.168.1.2/live3.sdp";
            
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
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                System.Xml.XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load( fileName);
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
