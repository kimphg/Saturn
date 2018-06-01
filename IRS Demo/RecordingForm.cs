using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Novacode;
using System.Diagnostics;
using MjpegProcessor;

using System.Reflection;
using System.Threading;
using Microsoft.Office.Interop.Word;


//using IRS_Demo;

namespace IRS_Demo
{
    public partial class RecordingForm : Form
    {
        private NewSessionForm _newSessionForm;
        private FindSession _findSession;
        private bool _bReplaying;
        private Vlc.DotNet.Forms.VlcControl vlcRecorder;
                
        // class attribute
       // MjpegDecoder m_mjpeg;
        //Capture m_capture;        
        Int32 nRecTimeInSecond;

        // this.vlcPlayer.VlcLibDirectoryNeeded+=vlcPlayer_VlcLibDirectoryNeeded;
        
        //Image<Bgr, Byte> m_frame;
        //string vlcLibPath = Path.Combine(new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName,"libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64");
        public RecordingForm(NewSessionForm newSessionForm)
        {
            InitializeComponent();            
            //this.vlcControl.VlcLibDirectory = new DirectoryInfo(vlcLibPath); 
            /////////////////
            _newSessionForm = newSessionForm;

            this.StartPosition = FormStartPosition.CenterScreen;


            _bReplaying = false;

            btnStartRec.Enabled = true;
            btnStopRec.Enabled = false;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, label43.Width, label43.Height);
            label43.Region = new Region(path);
            label43.Visible = false;

            CommonParam.LoadConfig();

            
            vlcRecorder = new Vlc.DotNet.Forms.VlcControl();           

            ((System.ComponentModel.ISupportInitialize)(this.vlcRecorder)).BeginInit();
            this.vlcRecorder.Name = "vlcRecorder";
            this.vlcRecorder.Size = new System.Drawing.Size(813, 618);
            this.vlcRecorder.Spu = -1;
            this.vlcRecorder.TabIndex = 0;
            this.vlcRecorder.Text = "vlcRecorder1";
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            vlcRecorder.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            ((System.ComponentModel.ISupportInitialize)(this.vlcRecorder)).EndInit();
            //itnit vlc player
            vlcPlayer.SetMedia(CommonParam.mConfig.videoUrl );

            vlcPlayer.VlcMediaplayerOptions = new[]
            {
                "--network-caching=1000",
                "--live-caching=300",
                "--no-rtsp-tcp",
                "--video-filter=transform"                
            };

            //label42.Parent = vlcPlayer;            
            label42.BackColor = Color.Transparent;
            vlcPlayer.Play();

            setViewSessionInfo(_bReplaying);            
            
        }

        public RecordingForm(FindSession findSession)
        {
            InitializeComponent();
            _findSession = findSession;

            this.StartPosition = FormStartPosition.CenterScreen;

            _bReplaying = true;

            btnStartRec.Enabled = false;
            btnStopRec.Enabled = false;            
            label43.Visible = false;
            linkLabel1.Visible = false;
            btnAddNotes.Enabled = false;
            btnPlay.Visible = false;
            label42.Visible = false;
            setViewSessionInfo(_bReplaying);

            vlcPlayer.SetMedia(new Uri(_findSession.selectedDataPath + "\\video.mp4"));
            label42.BackColor = Color.Transparent;
            vlcPlayer.Play();

            lblVideoPath.Text = _findSession.selectedDataPath + "video.mp4"; 
            
        }

        public void setViewSessionInfo(bool bReplay)
        {
            lblCaseCode.Text = CommonParam.mSesData.caseData._maVuAn;
            lblSessCode.Text = CommonParam.mSesData.sessionCode;
            lblRoom.Text = "Phòng 1";
            if (bReplay)
            {
                lblTimeBegin.Text = CommonParam.mSesData.sessBeginTime;
                lblTimeCase.Text = CommonParam.mSesData.sessCurrDate; // Thoi diem dien ra
                lblTimeEnd.Text = CommonParam.mSesData.sessEndTime; // time ket thuc                
            }     
            else
            {
                lblTimeBegin.Text = DateTime.Now.ToString("dd/MM/yyyy h:mm tt");
                lblTimeCase.Text = DateTime.Now.ToString("dd/MM/yyyy"); ; // Thoi diem dien ra
                lblTimeEnd.Text = ""; // time ket thuc
            }            
            
            lblTimePerform.Text = ""; // Thoi gian thuc hien

            lblInspectName.Text = CommonParam.mSesData.inspectData._Ten;
            lblInspectUnit.Text = CommonParam.mSesData.inspectData._DonVi;

            lblSupevName1.Text = CommonParam.mSesData.supervisorData1._Ten;
            lblSupevName2.Text = CommonParam.mSesData.supervisorData2._Ten;

            lblSuspectName.Text = CommonParam.mSesData.suspectData._Ten;
            lblSuspectSex.Text = CommonParam.mSesData.suspectData._GioiTinh;
            lblSuspectOtherName.Text = CommonParam.mSesData.suspectData._TenGoiKhac;
            lblSuspectBirthday.Text = CommonParam.mSesData.suspectData._NgaySinh;
            lblSuspectHKTT.Text = CommonParam.mSesData.suspectData._HKTT;
            lblSuspectAddr.Text = CommonParam.mSesData.suspectData._DiaChi;

            lblSupevUnit1.Text = CommonParam.mSesData.supervisorData1._DonVi;
            lblSupevUnit2.Text = CommonParam.mSesData.supervisorData2._DonVi;

            textBox19.Text = CommonParam.mSesData.sessNotes;           
        }

        private void vlcPlayer_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            // Default installation path of VideoLAN.LibVLC.Windows
            e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
        }       

        private void btnStartRec_Click(object sender, EventArgs e)
        {
            vlcRecorder.SetMedia(CommonParam.mConfig.videoUrl,
                ":sout=#transcode{vcodec=theo,vb=1000,scale=1,acodec=flac,ab=128,channels=2,samplerate=44100}:std{access=file,mux=ogg,dst="
                + CommonParam.ProgramPath + CommonParam.SessionFolderName + "\\video.mp4}");
            vlcRecorder.Play();


            timer1.Start();
            timer1.Interval = 500;
            nRecTimeInSecond = 0;

            label43.Visible = true;

            btnStartRec.Enabled = false;
            btnStopRec.Enabled = true;
            btnPause.Enabled = true;
        }


        private void btnStopRec_Click(object sender, EventArgs e)
        {
            DialogResult stopRecDialogResult = MessageBox.Show("Bạn có muốn dừng ghi lại phiên hỏi cung?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (stopRecDialogResult == DialogResult.Yes)
            {
                vlcRecorder.Stop();
                vlcRecorder.Dispose();
                timer1.Stop();
                label43.Visible = false;
                //btnStartRec.Enabled = true;
                btnStopRec.Enabled = false;
                btnPause.Enabled = false;                
                btnFinish.Enabled = true;
                if(MessageBox.Show("Dữ liệu video đã được ghi tại " + CommonParam.ProgramPath + CommonParam.SessionFolderName + ". Mở thư mục ghi lưu?", 
                "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                ==DialogResult.Yes)
                    Process.Start("explorer.exe", CommonParam.ProgramPath + CommonParam.SessionFolderName);

                lblVideoPath.Text = CommonParam.ProgramPath + CommonParam.SessionFolderName;                
            }
            else if (stopRecDialogResult == DialogResult.No)
            {
                //do something else
            }
            
            
        }
                
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            if (_newSessionForm.findForm == null)
            {
                _newSessionForm.findForm = new FindSession(_newSessionForm);
                this.Hide();
                _newSessionForm.findForm.ShowDialog();
                this.Show();
            }
            else
            {
                this.Hide();
                _newSessionForm.findForm.ShowDialog();
                this.Show();
            }
        }

        private void RecordingForm_FormClosing(object sender, FormClosingEventArgs e)
        {            
            
            DialogResult stopRecDialogResult = MessageBox.Show("Bạn có muốn thoát khỏi giao diện phiên hỏi cung?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (stopRecDialogResult == DialogResult.Yes)
            { 
                try
                {
                    vlcPlayer.Stop();
                    vlcRecorder.Stop();
                    vlcPlayer.Dispose();
                    vlcRecorder.Dispose();
                }
                catch(Exception exp)
                {
                    e.Cancel = false; ;
                }          
               

                e.Cancel = false;                            
            }
            else if (stopRecDialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }            

        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            vlcPlayer.Stop();
            vlcPlayer.Dispose();
            label42.Text = "00:00:00";
            btnPlay.Enabled = true;
            btnExport.Enabled = true;
            CommonParam.mSesData.sessEndTime = DateTime.Now.Hour.ToString() + " giờ " + DateTime.Now.Minute.ToString() + " phút";
            CommonParam.mSesData.sessCurrDate = DateTime.Now.ToString("dd/MM/yyyy");
            CommonParam.saveSession();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            vlcPlayer.SetMedia(new Uri(CommonParam.ProgramPath + CommonParam.SessionFolderName + "\\video.mp4"));
            vlcPlayer.Play();            
        }

        private void btnVideoPause_Click(object sender, EventArgs e)
        {
            vlcPlayer.Pause();
        }

        private void btnVideoContinue_Click(object sender, EventArgs e)
        {
            if (!_bReplaying)
                vlcPlayer.SetMedia(CommonParam.mConfig.videoUrl);

            vlcPlayer.Play();
        }

        private void btnVideoReload_Click(object sender, EventArgs e)
        {
            try
            {
                //itnit vlc player
                if (_bReplaying)
                    vlcPlayer.SetMedia(new Uri(_findSession.selectedDataPath + "\\video.mp4"));                    
                else
                    vlcPlayer.SetMedia(CommonParam.mConfig.videoUrl);                
                vlcPlayer.Play();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Lỗi video: " + exp.ToString());
                return;
            }
            
        }

        private void btnVideoStop_Click(object sender, EventArgs e)
        {
            vlcPlayer.Stop();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DocX gDoc;

            try
            {
                if (File.Exists(@"Template.docx"))
                {
                    gDoc = CreateInvoiceFromTemplate(DocX.Load(@"Template.docx"));                    
                    gDoc.SaveAs(CommonParam.ProgramPath + CommonParam.SessionFolderName + "\\BienBan.docx");                    
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "WINWORD.EXE";
                    startInfo.Arguments = CommonParam.ProgramPath + CommonParam.SessionFolderName + "\\BienBan.docx";
                    Process.Start(startInfo);
                }
                else
                {
                    MessageBox.Show("Không có file Template.docx");
                }
            }
            catch (Exception)
            {

                throw;
            }  
            /*
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    //string[] files = Directory.GetFiles(fbd.SelectedPath);
                    
                    //copy file ra folder da chon
                    string[] originalFiles = Directory.GetFiles(CommonParam.ProgramPath+CommonParam.SessionFolderName, "*", SearchOption.AllDirectories);

                    // Dealing with a string array, so let's use the actionable Array.ForEach() with a anonymous method
                    Array.ForEach(originalFiles, (originalFileLocation) =>
                    {
                        // Get the FileInfo for both of our files
                        FileInfo originalFile = new FileInfo(originalFileLocation);
                        FileInfo destFile = new FileInfo(originalFileLocation.Replace(CommonParam.ProgramPath + CommonParam.SessionFolderName, fbd.SelectedPath));
                        // ^^ We can fill the FileInfo() constructor with files that don't exist...

                        // ... because we check it here
                        if (destFile.Exists)
                        {
                            // Logic for files that exist applied here; if the original is larger, replace the updated files...
                            if (originalFile.Length > destFile.Length)
                            {
                                originalFile.CopyTo(destFile.FullName, true);
                            }
                        }
                        else // ... otherwise create any missing directories and copy the folder over
                        {
                            Directory.CreateDirectory(destFile.DirectoryName); // Does nothing on directories that already exist
                            originalFile.CopyTo(destFile.FullName, false); // Copy but don't over-write  
                        }

                    });
                }
            }
            */
        }

        private DocX CreateInvoiceFromTemplate(DocX template)
        {
            
            template.AddCustomProperty(new Novacode.CustomProperty("DieuTraVien", CommonParam.mSesData.inspectData._Ten));
            template.AddCustomProperty(new Novacode.CustomProperty("GiamSatVien1", CommonParam.mSesData.supervisorData1._Ten));
            template.AddCustomProperty(new Novacode.CustomProperty("GiamSatVien2", CommonParam.mSesData.supervisorData2._Ten));
            template.AddCustomProperty(new Novacode.CustomProperty("ThoiGianDienRa", CommonParam.mSesData.sessBeginTime));
            template.AddCustomProperty(new Novacode.CustomProperty("BC_Ten", CommonParam.mSesData.suspectData._Ten));
            template.AddCustomProperty(new Novacode.CustomProperty("BC_GioiTinh", CommonParam.mSesData.suspectData._GioiTinh));
            template.AddCustomProperty(new Novacode.CustomProperty("BC_QuocTich", CommonParam.mSesData.suspectData._QuocTich));
            template.AddCustomProperty(new Novacode.CustomProperty("BC_TenGoiKhac", CommonParam.mSesData.suspectData._TenGoiKhac));
            template.AddCustomProperty(new Novacode.CustomProperty("BC_NgaySinh", CommonParam.mSesData.suspectData._NgaySinh));
            template.AddCustomProperty(new Novacode.CustomProperty("BC_NoiSinh", CommonParam.mSesData.suspectData._NoiSinh));
            template.AddCustomProperty(new Novacode.CustomProperty("BC_DanToc", CommonParam.mSesData.suspectData._DanToc));
            template.AddCustomProperty(new Novacode.CustomProperty("BC_TonGiao", CommonParam.mSesData.suspectData._TonGiao));
            template.AddCustomProperty(new Novacode.CustomProperty("BC_NgheNghiep", CommonParam.mSesData.suspectData._NgheNghiep));
            template.AddCustomProperty(new Novacode.CustomProperty("BC_SoCMND", CommonParam.mSesData.suspectData._CMND));
            template.AddCustomProperty(new Novacode.CustomProperty("BC_NgayCapCMND", CommonParam.mSesData.suspectData._NgayCapCMND));
            template.AddCustomProperty(new Novacode.CustomProperty("BC_NoiCuTru", CommonParam.mSesData.suspectData._DiaChi));
            template.AddCustomProperty(new Novacode.CustomProperty("BC_NoiCapCMND", CommonParam.mSesData.suspectData._NoiCapCMND));
            template.AddCustomProperty(new Novacode.CustomProperty("DiaDiemHoiCung", CommonParam.mSesData.sessPlace));
            template.AddCustomProperty(new Novacode.CustomProperty("ThoiGianKetThuc", CommonParam.mSesData.sessEndTime));
            template.AddCustomProperty(new Novacode.CustomProperty("NgayHoiCung", CommonParam.mSesData.sessCurrDate));
            template.AddCustomProperty(new Novacode.CustomProperty("DiaDiemHoiCung", CommonParam.mSesData.sessPlace));
            template.AddCustomProperty(new Novacode.CustomProperty("GhiChu", CommonParam.mSesData.sessNotes));
            
            return template;
        }

         
        private void timer1_Tick(object sender, EventArgs e)
        {
            nRecTimeInSecond++;            

            if ((nRecTimeInSecond % 2) == 0)
            {
                label43.Visible = false;
                TimeSpan time = TimeSpan.FromSeconds(nRecTimeInSecond/2);
                string recordTime = time.ToString(@"hh\:mm\:ss");
                label42.Text = recordTime;
            }
                
            else
                label43.Visible = true;
            
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (btnPause.Text == "Tạm dừng")
            {
                vlcRecorder.Pause();                 
                btnPause.Text = "Tiếp tục ghi";
                timer1.Stop();
            }
                
            else if (btnPause.Text == "Tiếp tục ghi")
            {   
                vlcRecorder.SetMedia(CommonParam.mConfig.videoUrl,
                ":sout=#transcode{vcodec=theo,vb=1000,scale=1,acodec=flac,ab=128,channels=2,samplerate=44100}:std{access=file,mux=ogg,dst="
                + CommonParam.ProgramPath + CommonParam.SessionFolderName + "\\video.mp4}");                

                vlcRecorder.Play();

                                               
                btnPause.Text = "Tạm dừng";
                timer1.Start();
            }
                
        }

        private void btnAddNotes_Click(object sender, EventArgs e)
        {
            CommonParam.mSesData.sessNotes += "\r\n";
            CommonParam.mSesData.sessNotes += txtAddNotes.Text;
            txtAddNotes.Clear();
            textBox19.Text = CommonParam.mSesData.sessNotes;
        }

    }
}
