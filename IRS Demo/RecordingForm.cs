using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MjpegProcessor;

using Emgu;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Reflection;
using System.IO;

//using IRS_Demo;

namespace IRS_Demo
{
    public partial class RecordingForm : Form
    {
        private NewSessionForm _newSessionForm;
        //private Vlc.DotNet.Forms.VlcControl vlcControl;
        // class attribute
        MjpegDecoder m_mjpeg;
        Capture m_capture;
        bool m_bSession;
        Image<Bgr, Byte> m_frame;
        private Vlc.DotNet.Forms.VlcControl vlcControl;
        public RecordingForm(NewSessionForm newSessionForm)
        {
            InitializeComponent();
            vlcControl = new Vlc.DotNet.Forms.VlcControl();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).BeginInit();
            // vlcControl
            
            this.vlcControl.BackColor = System.Drawing.Color.Black;
            this.vlcControl.Location = new System.Drawing.Point(0, 0);
            this.vlcControl.Name = "vlcControl";
            this.vlcControl.Size = new System.Drawing.Size(200, 179);
            this.vlcControl.Spu = -1;
            this.vlcControl.TabIndex = 0;
            this.vlcControl.Text = "vlcControl1";
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            this.vlcControl.VlcLibDirectory =  new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64")); ;
            this.vlcControl.VlcMediaplayerOptions = null;
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).EndInit();
            /////////////////
            _newSessionForm = newSessionForm;

            this.StartPosition = FormStartPosition.CenterScreen;

            m_bSession = true;

            btnStartRec.Enabled = true;
            btnStopRec.Enabled = false;

            CommonParam.LoadConfig();
                        
            tabControl1.SelectedIndex = 1;

            tabControl1.Selecting += tabControl1_Selecting;


            if (tabControl1.SelectedIndex == 0)
            {
                m_mjpeg = new MjpegDecoder();
                m_mjpeg.imageSizeBytes = 1024 * 1024;
                m_mjpeg.FrameReady += mjpeg_FrameReady;
                m_mjpeg.ParseStream(new Uri(CommonParam.MjpegUrl));
            }

            if (tabControl1.SelectedIndex == 1)
            {
                axVLCPlugin21.playlist.add(new Uri(CommonParam.mConfig.videoUrl).AbsoluteUri);
                axVLCPlugin21.playlist.play();
            }

            if (tabControl1.SelectedIndex == 2)
            {
                if (m_capture == null)
                   // m_capture = new Emgu.CV.Capture("rtsp://admin:admin@192.168.1.2/live3.sdp");
                    m_capture = new Emgu.CV.Capture("rtsp://root:root@192.168.1.218/axis-media/media.amp");

                m_capture.ImageGrabbed += m_capture_ImageGrabbed;
                m_capture.Start();
            }

            label23.Text = CommonParam.mSesData.caseCode;
            label24.Text = "Phòng 1";
            label25.Text = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            label26.Text = ""; // time ket thuc
            label27.Text = ""; // Thoi diem dien ra
            label28.Text = ""; // Ma phien
            label29.Text = ""; // Thoi gian thuc hien
            label30.Text = CommonParam.mSesData.inspectorName;
            label31.Text = CommonParam.mSesData.inspectorCode;


        }

        void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //if (tabControl1.SelectedIndex == 0)
            //{
            //    axVLCPlugin21.playlist.stop();
            //    m_capture.Stop();
            //}
            //if (tabControl1.SelectedIndex == 1)
            //{
            //    m_capture.Stop();
            //    axVLCPlugin21.playlist.play();
            //}
            //if (tabControl1.SelectedIndex == 2)
            //{
            //    axVLCPlugin21.playlist.stop();
            //    m_capture.Start();
            //}
        }

        private void m_capture_ImageGrabbed(object sender, EventArgs e)
        {            
                try
                {
                    m_frame = m_capture.RetrieveBgrFrame();

                    m_frame = m_frame.Resize(pictureBox1.Width, pictureBox1.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);

                    //pictureBox1.Image = m_frame.ToBitmap(pictureBox1.Width, pictureBox1.Height);
                    pictureBox1.Image = m_frame.ToBitmap();

                }
                catch (Exception)
                {

                }                        
        }
        private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            if(tabControl1.SelectedIndex==0) pictureBoxVideo.Image = e.Bitmap;
        }
        private void btnStartRec_Click(object sender, EventArgs e)
        {
            
            vlcControl.SetMedia(CommonParam.mConfig.videoUrl,
                ":sout=#transcode{vcodec=theo,vb=1000,scale=1,acodec=flac,ab=128,channels=2,samplerate=44100}:std{access=file,mux=ogg,dst="+CommonParam.ProgramPath + CommonParam.SessionFolderName + "\\video.ogg}");
            vlcControl.Play();
            btnStartRec.Enabled = false;
            btnStopRec.Enabled = true;
            /*
            String command =  "\"" 
                + CommonParam.mConfig.videoUrl
                + "\" --qt-start-minimized --sout=#transcode{vcodec=theo,vb=800,acodec=flac,ab=128,channels=2,samplerate=44100}:file{dst="
                + CommonParam.ProgramPath 
                + CommonParam.SessionFolderName 
                + "\\video.ogg"
                + ",overwrite"
                + "}\"";

            //"C://Program Files (x86)//VideoLAN//VLC//vlc.exe", "\"rtsp://admin:admin@192.168.1.2/live3.sdp\" --qt-start-minimized --sout=#transcode{vcodec=theo,vb=800,acodec=flac,ab=128,channels=2,samplerate=44100}:file{dst=D:\\abc.ogg,no-overwrite}"
            Process.Start(CommonParam.mConfig.recCommand, command);  
            //Process.Start("C://Program Files (x86)//VideoLAN//VLC//vlc.exe", textBox20.Text);//vcodec=theo,vb=800,
            //Process.Start("C://Program Files (x86)//VideoLAN//VLC//vlc.exe", "\"rtsp://root:root@192.168.1.218/axis-media/media.amp\" --sout=#std{access=file,mux=mpeg1,vcodec=theo, acodec=vorb, dst=d:/go.ogg}");
            //rtsp://root:root@192.168.1.218/axis-media/media.amp --sout="#std{access=file,mux=mpeg1,dst=d:/go.mpg}" --ghi duoc hinh chua co tieng
            //C://Program Files (x86)//VideoLAN//VLC//vlc.exe "rtsp://root:root@192.168.1.218/axis-media/media.amp" --sout=#std{access=file,mux=mpeg1,vcodec=theo, acodec=vorb, dst=d:/go.ogg}
            btnStartRec.Enabled = false;
            btnStopRec.Enabled = true;
            lblVideoPath.Text = CommonParam.ProgramPath + CommonParam.SessionFolderName + "\\video.ogg";*/
        }
        
        private void stopRecording()
        {
            vlcControl.Stop();
            /*foreach (var _process in Process.GetProcessesByName("vlc"))
            {
                _process.Kill();
            }*/
        }

        private void btnStopRec_Click(object sender, EventArgs e)
        {
            DialogResult stopRecDialogResult = MessageBox.Show("Bạn có muốn dừng ghi lại phiên hỏi cung?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (stopRecDialogResult == DialogResult.Yes)
            {
                stopRecording();                
                //btnStartRec.Enabled = true;
                btnStopRec.Enabled = false;
                btnExport.Enabled = true;
                btnFinish.Enabled = true;
                MessageBox.Show("Dữ liệu video đã được ghi tại " + CommonParam.ProgramPath + CommonParam.SessionFolderName, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (!m_bSession) return; 
            DialogResult stopRecDialogResult = MessageBox.Show("Bạn có muốn thoát chương trình?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (stopRecDialogResult == DialogResult.Yes)
            {
                stopRecording();
                axVLCPlugin21.playlist.stop();
                axVLCPlugin21.playlist.items.clear();
                m_bSession = false;                
            }
            else if (stopRecDialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }            

        }

        private void btnFinish_Click(object sender, EventArgs e)
        {            
            axVLCPlugin21.playlist.stop();
            btnPlay.Enabled = true;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            axVLCPlugin21.playlist.items.clear();
            axVLCPlugin21.playlist.add(new Uri(CommonParam.ProgramPath + CommonParam.SessionFolderName + "\\video.ogg").AbsoluteUri);
            axVLCPlugin21.playlist.play();
        }

    }
}
