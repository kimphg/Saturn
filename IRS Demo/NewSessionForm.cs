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
            CommonParam.GetSuspectInfo();
            CommonParam.mSesData.beginSessTime = DateTime.Now.Hour.ToString() + " giờ " + DateTime.Now.Minute.ToString() + " phút";            
            this.Hide();
            recform.ShowDialog();
            CommonParam.saveSession();
            this.Show();            
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
            CommonParam.mSesData.suspectData._Ten = textBox1.Text;
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
            CommonParam.mSesData.suspectData._MaDT = textBox8.Text;
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

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            CommonParam.mSesData.currentPlace = textBox10.Text;
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
            DialogResult exitAppResult = MessageBox.Show("Bạn có muốn thoát khỏi chương trình?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (exitAppResult == DialogResult.No)
            {
                e.Cancel = true;
            }            
            
        }

        private void txtNotes_TextChanged(object sender, EventArgs e)
        {            
            CommonParam.mSesData.Notes = txtNotes.Text;
        }

        private void NewSessionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        

    }
}
