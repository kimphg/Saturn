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
    public partial class FindSession : Form
    {
        private NewSessionForm _newSessionForm;
        public FindSession(NewSessionForm newSessionForm)
        {
            InitializeComponent();
            _newSessionForm = newSessionForm;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void FindSession_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }
        
    }
}
