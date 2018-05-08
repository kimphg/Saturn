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
            loadInspectorCbBox();
        }

        public void getNewSessionInfo()
        {
            CommonParam.mSesData.caseName = tbCaseName.Text;
            CommonParam.mSesData.suspectData._Ten = textBox1.Text;
            CommonParam.mSesData.inspectorName = cbInspectorName.Text;
            CommonParam.mSesData.supervisorName = textBox3.Text;
            CommonParam.mSesData.supervisorName2 = textBox4.Text;
            CommonParam.mSesData.caseCode = textBox6.Text;
            CommonParam.mSesData.suspectData._MaDT = textBox8.Text;
            CommonParam.mSesData.inspectorCode = txtInspectCode.Text;
            CommonParam.mSesData.supervisorCode = textBox7.Text;
            CommonParam.mSesData.supervisorCode2 = textBox9.Text;
            CommonParam.mSesData.currentPlace = textBox10.Text;
            CommonParam.mSesData.Notes = txtNotes.Text;            
        }

        private void btnNewSession_Click(object sender, EventArgs e)
        {
            getNewSessionInfo();
            CommonParam.GetSessSuspectInfo();

            CommonParam.SessionFolderName = "SS_" + DateTime.Now.ToString(@"MM_dd_yyyy.h_mm_tt");
            System.IO.Directory.CreateDirectory(CommonParam.ProgramPath + "\\" + CommonParam.SessionFolderName);
            recform = new RecordingForm(this);
            
            CommonParam.mSesData.beginSessTime = DateTime.Now.Hour.ToString() + " giờ " + DateTime.Now.Minute.ToString() + " phút";            
            this.Hide();
            recform.ShowDialog();
            CommonParam.saveSession();
            this.Show();            
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

        private DataTable m_InspectDataTable = new DataTable();

        void loadInspectorCbBox()
        {
            DataSet dataSet = new DataSet();
            CommonParam.GetInspectorsInfo();
            dataSet.Reset();
            CommonParam.sql_DataAdaptInspector.Fill(dataSet);
            m_InspectDataTable = dataSet.Tables[0];
            cbInspectorName.DataSource = m_InspectDataTable;
            cbInspectorName.DisplayMember = m_InspectDataTable.Columns[1].ToString();

            loadInspecCodeTxtBox();
        }

        private string getInspectCode(string inspectName)
        {
            string filterExpression = "inspName=" + "'" + inspectName + "'";
            DataRow[] rows = m_InspectDataTable.Select(filterExpression);
            string strInspecCode = rows[0].ItemArray[2].ToString();
            return strInspecCode;
        }

        private void loadInspecCodeTxtBox()
        {
            string strInspecName = cbInspectorName.GetItemText(cbInspectorName.SelectedItem);
            string strInspecCode = getInspectCode(strInspecName);
            txtInspectCode.Text = strInspecCode;
        }

        private void NewSessionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void cbInspectorName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            loadInspecCodeTxtBox();
        }


    }
}
