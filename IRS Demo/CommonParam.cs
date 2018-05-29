using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
//using Finisar.SQLite;
using System.Data.SQLite;

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
        public string _GioiTinh, _TenGoiKhac, _NgaySinh, _NoiSinh, _QuocTich, _DanToc, _TonGiao, _NgheNghiep, _HKTT;
        public string _CMND, _NgayCapCMND, _NoiCapCMND;
        public string _DiaChi;
    }

    public struct InspectData
    {
        public string _Ten, _maDTV, _DonVi;
    }

    public struct SupervisorData
    {
        public string _Ten, _maGSV, _DonVi;
    }

    public struct CaseData
    {
        public string _Ten, _maVuAn, _Mota;
    }


    public struct SessionData
    {
       // public string caseName, caseCode;

        public string sessNotes;

        public string sessBeginTime;
        public string sessEndTime;

        public string sessPlace;
        public string sessCurrDate;

        public string sessionCode;

        public SuspectData      suspectData;
        public InspectData      inspectData;
        public SupervisorData   supervisorData1, supervisorData2;
        public CaseData         caseData;
        

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

        public  static SQLiteConnection sql_Conn;
        private static SQLiteCommand sql_Cmd;
        public static SQLiteDataAdapter sql_DataAdaptUser;
        public static SQLiteDataAdapter sql_DataAdaptInspector;
        public static SQLiteDataAdapter sql_DataAdaptSupervisor;
        public static SQLiteDataAdapter sql_DataAdaptSuspect;
        public static SQLiteDataAdapter sql_DataAdaptCase;
        

        public static void GetSessSuspectInfo(string strSuspectCode)
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

        public static void GetSuspectsInfo()
        {
            setConnection();
            sql_Conn.Open();

            sql_Cmd = sql_Conn.CreateCommand();
            string CommandText = "select * from  suspTbl";
            sql_DataAdaptSuspect = new SQLiteDataAdapter(CommandText, sql_Conn);
            sql_Conn.Close();
        }

        private static void setConnection()
        {
            sql_Conn = new SQLiteConnection("Data Source=IRS.db;Version=3;New=False;Compress=True;");
        }

        public static void GetSupervisorsInfo()
        {
            setConnection();
            sql_Conn.Open();

            sql_Cmd = sql_Conn.CreateCommand();
            string CommandText = "select * from  supeTbl";
            sql_DataAdaptSupervisor = new SQLiteDataAdapter(CommandText, sql_Conn);
            sql_Conn.Close();
        }

        public static void GetInspectorsInfo()
        {
            setConnection();
            sql_Conn.Open();

            sql_Cmd = sql_Conn.CreateCommand();
            string CommandText = "select * from  inspTbl";
            sql_DataAdaptInspector = new SQLiteDataAdapter(CommandText, sql_Conn);
            sql_Conn.Close();
        }

        public static void GetCasesInfo() // lay thong tin vu an
        {
            setConnection();
            sql_Conn.Open();

            sql_Cmd = sql_Conn.CreateCommand();
            string CommandText = "select * from  caseTbl";
            sql_DataAdaptCase = new SQLiteDataAdapter(CommandText, sql_Conn);
            sql_Conn.Close();
        }

        public static void GetUsersInfo()
        {
            setConnection();
            sql_Conn.Open();

            sql_Cmd = sql_Conn.CreateCommand();
            string CommandText = "select * from  userTbl";
            sql_DataAdaptUser = new SQLiteDataAdapter(CommandText, sql_Conn);
            sql_Conn.Close();
        }        

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!Directory.Exists(destDirName))
            {
                try
                {
                    Directory.CreateDirectory(destDirName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    return;
                }
                
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                
                string temppath = Path.Combine(destDirName, file.Name);
                try
                {
                    file.CopyTo(temppath, false);
                }

                // Catch exception if the file was already copied.
                catch (IOException copyError)
                {
                    MessageBox.Show(copyError.Message);
                }

                
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
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
