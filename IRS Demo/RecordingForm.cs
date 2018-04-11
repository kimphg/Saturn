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

namespace IRS_Demo
{
    public partial class RecordingForm : Form
    {

        public RecordingForm()
        {
            InitializeComponent();
            CommonParam.LoadConfig();
            axVLCPlugin21.playlist.add(new Uri(CommonParam.mConfig.videoUrl).AbsoluteUri);
            axVLCPlugin21.playlist.play();             
        }

        private void btnStartRec_Click(object sender, EventArgs e)
        {
            Process.Start(CommonParam.mConfig.recCommand,CommonParam.mConfig.recParam );
        }

        private void btnStopRec_Click(object sender, EventArgs e)
        {
            // Dừng Process vlc.exe tại đây
        }
    }
}
