using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRS_Demo
{
    public partial class NewSessionForm : Form
    {
        RecordingForm recform;
        public NewSessionForm()
        {
            InitializeComponent();
        }

        private void btnNewSession_Click(object sender, EventArgs e)
        {
            recform = new RecordingForm();
            recform.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
