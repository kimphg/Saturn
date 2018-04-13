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

//using IRS_Demo;

namespace IRS_Demo
{
    public partial class RecordingForm : Form
    {
        private NewSessionForm _newSessionForm;
        // class attribute
        MjpegDecoder m_mjpeg;
        Capture m_capture;
        bool m_bSession;
        Image<Bgr, Byte> m_frame;
        public RecordingForm(NewSessionForm newSessionForm)
        {
            InitializeComponent();

            

            _newSessionForm = newSessionForm;

            this.StartPosition = FormStartPosition.CenterScreen;

            m_bSession = true;

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
                axVLCPlugin21.playlist.add(new Uri(CommonParam.videoUrl).AbsoluteUri);
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
            String command =  "\"" 
                + CommonParam.videoUrl
                + "\" --qt-start-minimized --sout=#transcode{vcodec=theo,vb=800,acodec=flac,ab=128,channels=2,samplerate=44100}:file{dst=" 
                + CommonParam.ProgramPath 
                + CommonParam.SessionFolderName 
                + "\\video.ogg"
                + ",no-overwrite"
                + "}\"";

            //"C://Program Files (x86)//VideoLAN//VLC//vlc.exe", "\"rtsp://admin:admin@192.168.1.2/live3.sdp\" --qt-start-minimized --sout=#transcode{vcodec=theo,vb=800,acodec=flac,ab=128,channels=2,samplerate=44100}:file{dst=D:\\abc.ogg,no-overwrite}"
            Process.Start(CommonParam.mConfig.recCommand, command);  
            //Process.Start("C://Program Files (x86)//VideoLAN//VLC//vlc.exe", textBox20.Text);//vcodec=theo,vb=800,
            //Process.Start("C://Program Files (x86)//VideoLAN//VLC//vlc.exe", "\"rtsp://root:root@192.168.1.218/axis-media/media.amp\" --sout=#std{access=file,mux=mpeg1,vcodec=theo, acodec=vorb, dst=d:/go.ogg}");
            //rtsp://root:root@192.168.1.218/axis-media/media.amp --sout="#std{access=file,mux=mpeg1,dst=d:/go.mpg}" --ghi duoc hinh chua co tieng
            //C://Program Files (x86)//VideoLAN//VLC//vlc.exe "rtsp://root:root@192.168.1.218/axis-media/media.amp" --sout=#std{access=file,mux=mpeg1,vcodec=theo, acodec=vorb, dst=d:/go.ogg}
        }
        
        private void stopRecording()
        {
            foreach (var _process in Process.GetProcessesByName("vlc"))
            {
                _process.Kill();
            }
        }

        private void btnStopRec_Click(object sender, EventArgs e)
        {
            DialogResult stopRecDialogResult = MessageBox.Show("Bạn có muốn dừng ghi lại phiên hỏi cung?", "Dừng ghi", MessageBoxButtons.YesNo);
            if (stopRecDialogResult == DialogResult.Yes)
            {
                stopRecording();
                MessageBox.Show("Dữ liệu video đã được ghi tại ...");
            }
            else if (stopRecDialogResult == DialogResult.No)
            {
                //do something else
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this.DialogResult = DialogResult.Cancel;            
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
            DialogResult stopRecDialogResult = MessageBox.Show("Bạn có muốn thoát chương trình?", "Thoát chương trình", MessageBoxButtons.YesNo);
            if (stopRecDialogResult == DialogResult.Yes)
            {
                stopRecording();
                axVLCPlugin21.playlist.stop();
                m_bSession = false;                
            }
            else if (stopRecDialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }            

        }

    }
}
