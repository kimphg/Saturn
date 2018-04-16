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
        public RecordingForm recform;
        public FindSession findForm;    
        public NewSessionForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnNewSession_Click(object sender, EventArgs e)
        {
            CommonParam.SessionFolderName = "SS_" + DateTime.Now.ToString(@"MM_dd_yyyy.h_mm_tt");
            System.IO.Directory.CreateDirectory(CommonParam.ProgramPath + "\\" + CommonParam.SessionFolderName);
            recform = new RecordingForm(this);
            this.Hide();
            recform.ShowDialog();
            CommonParam.saveSession();            
            Application.Exit();            
        }

        private void button1_Click(object sender, EventArgs e)//tra cuu du lieu
        {

        }

        private void tbCaseName_TextChanged(object sender, EventArgs e)
        {
            CommonParam.mSesData.caseName = tbCaseName.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CommonParam.mSesData.suspectName = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            CommonParam.mSesData.inspectorName = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            CommonParam.mSesData.supervisorName = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            CommonParam.mSesData.supervisorName2 = textBox4.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            CommonParam.mSesData.caseCode = textBox6.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            CommonParam.mSesData.suspectCode = textBox8.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            CommonParam.mSesData.inspectorCode = textBox5.Text;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            CommonParam.mSesData.supervisorCode = textBox7.Text;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            CommonParam.mSesData.supervisorCode2 = textBox9.Text;
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            CommonParam.mSesData.Notes = txtNotes.Text;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            findForm = new FindSession(this);
            this.Hide();            
            findForm.ShowDialog();

            this.Show();
        }

        private void NewSessionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void txtNotes_TextChanged(object sender, EventArgs e)
        {            
            CommonParam.mSesData.Notes = txtNotes.Text;
        }

    }
}
