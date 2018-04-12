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
namespace IRS_Demo
{
    public partial class RecordingForm : Form
    {
        // class attribute
        MjpegDecoder m_mjpeg;
        public RecordingForm()
        {
            InitializeComponent();
            CommonParam.LoadConfig();

            tabControl1.SelectedIndex = 1;

            axVLCPlugin21.playlist.add(new Uri(CommonParam.videoUrl).AbsoluteUri);
            axVLCPlugin21.playlist.play();



            m_mjpeg = new MjpegDecoder();
            m_mjpeg.imageSizeBytes = 1024*1024;
            m_mjpeg.FrameReady += mjpeg_FrameReady;
            m_mjpeg.ParseStream(new Uri(CommonParam.MjpegUrl));

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
        private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            if(tabControl1.SelectedIndex==0) pictureBoxVideo.Image = e.Bitmap;
        }
        private void btnStartRec_Click(object sender, EventArgs e)
        {
            Process.Start(CommonParam.mConfig.recCommand, CommonParam.mConfig.recParam + CommonParam.ProgramPath + CommonParam.SessionFolderName + "\\video.ogg" + ",no-overwrite}");  
        }

        private void btnStopRec_Click(object sender, EventArgs e)
        {
            foreach (var _process in Process.GetProcessesByName("vlc"))
            {
                _process.Kill();
            }
            // Dừng Process vlc.exe tại đây
        }

       
    }
}
