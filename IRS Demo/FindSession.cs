using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRS_Demo
{
    
    public partial class FindSession : Form
    {
        public struct searchParam
        {
            public static string caseCode = "";
            public static string inspectorName = "";
            public static string suspectName = "";
            public static string sessionCode;
            public static string supervisorName1;
            public static string supervisorName2;
            public static string notes;
            public static string inspectorCode;
        }
        
        
        List<SessionData> SessionHistory = new List<SessionData>();
        List<SessionData> SearchResults ;
        public string selectedDataPath = "";

        public RecordingForm replayForm;
        public FindSession( Form parent)
        {
            InitializeComponent();
            // get list of sub folder
            var directories = Directory.GetDirectories(CommonParam.ProgramPath);
            
            foreach (var subDir in directories)
            {
                if(File.Exists(subDir+ "\\" + CommonParam.SessionFileName))
                {
                    SessionData data = CommonParam.LoadObject<SessionData>(subDir + "\\" + CommonParam.SessionFileName);
                    data.SessionPath = subDir + "\\";
                    data.SessionKeyText =       data.inspectData._Ten + " "
                                            +   data.inspectData._maDTV + " "
                                            +   data.suspectData._Ten + " "
                                            +   data.suspectData._MaDT + " "
                                            +   data.supervisorData1._Ten + " "
                                            +   data.supervisorData1._maGSV + " "
                                            +   data.supervisorName2 + " "
                                            +   data.supervisorCode2 + " "
                                            +   data.Notes;
                    SessionHistory.Add(data);
                    
                }
            }
            //
            
            this.StartPosition = FormStartPosition.CenterScreen;
            comboBox1.SelectedIndex = 0;
            SearchResults = new List<SessionData>(SessionHistory);

            GetUSBRemovable();
            if(comboBox2.Items.Count != 0)
                comboBox2.SelectedIndex = 0;
            
            UpdateSearchResults();
        }

        private void UpdateSearchResults()
        {
            var bindingSource = new BindingSource();
            // Bind BindingSource1 to the list of states.
            bindingSource.DataSource = SearchResults;
            this.listBoxSearchResults.DataSource = bindingSource;
            listBoxSearchResults.BindingContext = this.BindingContext;
            listBoxSearchResults.DisplayMember = "SessionPath";
            //listBoxSearchResults.ValueMember = "suspectName";
            listBoxSearchResults.Refresh();
        }

        private void FindSession_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void getSearchInfo()
        {
            searchParam.caseCode = textBox1.Text;
            searchParam.inspectorName = textBox2.Text;
            searchParam.suspectName = textBox6.Text;
            searchParam.sessionCode = textBox5.Text;
            searchParam.supervisorName1 = textBox3.Text;
            searchParam.inspectorCode = textBox7.Text;
            searchParam.notes = textBox4.Text;
            searchParam.supervisorName2 = textBox8.Text;

        }

        private void button1_Click(object sender, EventArgs e)//tim kiem
        {
            SearchResults.Clear();

            getSearchInfo();

            foreach (SessionData session in SessionHistory)
            {
                if (!string.IsNullOrEmpty(searchParam.inspectorName))
                {
                    if (!string.IsNullOrEmpty(session.inspectData._Ten))
                        if (session.inspectData._Ten.Contains(searchParam.inspectorName)) { SearchResults.Add(session); continue; }
                }
                if (!string.IsNullOrEmpty(searchParam.caseCode))
                {
                    if (!string.IsNullOrEmpty(session.caseCode))
                        if (session.caseCode.Contains(searchParam.caseCode)) { SearchResults.Add(session); continue; }
                }
                if (!string.IsNullOrEmpty(searchParam.suspectName))
                {
                    if (!string.IsNullOrEmpty(session.suspectData._Ten))
                        if (session.suspectData._Ten.Contains(searchParam.suspectName)) { SearchResults.Add(session); continue; }
                }
                if (!string.IsNullOrEmpty(searchParam.supervisorName1))
                {
                    if (!string.IsNullOrEmpty(session.supervisorData1._Ten))
                        if (session.supervisorData1._Ten.Contains(searchParam.supervisorName1)) { SearchResults.Add(session); continue; }
                }
                if (!string.IsNullOrEmpty(searchParam.supervisorName2))
                {
                    if (!string.IsNullOrEmpty(session.supervisorName2))
                        if (session.supervisorName2.Contains(searchParam.supervisorName2)) { SearchResults.Add(session); continue; }
                }
                if (!string.IsNullOrEmpty(searchParam.notes))
                {
                    if (!string.IsNullOrEmpty(session.Notes))
                        if (session.Notes.Contains(searchParam.notes)) { SearchResults.Add(session); continue; }
                }
                if (!string.IsNullOrEmpty(searchParam.inspectorCode))
                {
                    if (!string.IsNullOrEmpty(session.inspectData._maDTV))
                        if (session.inspectData._maDTV.Contains(searchParam.inspectorCode)) { SearchResults.Add(session); continue; }
                }
                
            }
            UpdateSearchResults();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string keyWord = textBox9.Text;
            SearchResults.Clear();
            foreach (SessionData session in SessionHistory)
            {
                if (session.SessionKeyText.Contains(keyWord)) SearchResults.Add(session);
            }
            UpdateSearchResults();
        }
        

        private void listBoxSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDataPath = listBoxSearchResults.GetItemText(listBoxSearchResults.SelectedItem);
            SessionData data = CommonParam.LoadObject<SessionData>(selectedDataPath + CommonParam.SessionFileName);
            
            this.textBoxInspectorCode.Text = data.inspectData._maDTV;
            this.textBoxInsptectorName.Text = data.inspectData._Ten;
            this.textBoxSuspectName.Text = data.suspectData._Ten;
            this.textBoxSupervisorName1.Text = data.supervisorData1._Ten;
            this.textBoxSupervisorName2.Text = data.supervisorName2;
            this.textBoxCaseCode.Text = data.caseCode;
            this.txtNote_View.Text = data.Notes;

            getReplayInfo(data);
           
        }

        private void btnCopyUSB_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("Chưa chọn ổ đĩa USB để lưu!");
                return;
            }
            string destPath = selectedDataPath.Replace("C:\\", comboBox2.Text);
            CommonParam.DirectoryCopy(selectedDataPath, destPath, true);
        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            GetUSBRemovable();

        }

        private void getReplayInfo(SessionData sessData)
        {
            CommonParam.mSesData.caseName = sessData.caseName;
            CommonParam.mSesData.suspectData._Ten = sessData.suspectData._Ten;
            CommonParam.mSesData.inspectData._Ten = sessData.inspectData._Ten;
            CommonParam.mSesData.supervisorData1._Ten = sessData.supervisorData1._Ten;
            CommonParam.mSesData.supervisorName2 = sessData.supervisorName2;
            CommonParam.mSesData.caseCode = sessData.caseCode;
            CommonParam.mSesData.suspectData._MaDT = sessData.suspectData._MaDT;
            CommonParam.mSesData.inspectData._maDTV = sessData.inspectData._maDTV;
            CommonParam.mSesData.supervisorData1._maGSV = sessData.supervisorData1._maGSV;
            CommonParam.mSesData.supervisorCode2 = sessData.supervisorCode2;
            CommonParam.mSesData.currentPlace = sessData.currentPlace;
            CommonParam.mSesData.Notes = sessData.Notes;
        }

        private void GetUSBRemovable()
        {            
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Removable)
                {
                    comboBox2.Items.Add(drive.Name);                                     
                }
            }
            
        }

        private void btnReplay_Click(object sender, EventArgs e)
        {
            if (selectedDataPath == "")
            {
                MessageBox.Show("Chưa chọn phiên hỏi cung!");
                return;
            }
                
            replayForm = new RecordingForm(this);
            
            this.Hide();
            replayForm.ShowDialog();
            
            this.Show();            
        }

        private void btnCopyDVD_Click(object sender, EventArgs e)
        {
            string driveNameCD = ""; 
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.CDRom)
                {
                    driveNameCD = drive.Name;
                }
            }
            string destPath = selectedDataPath.Replace("C:\\", driveNameCD);
            CommonParam.DirectoryCopy(selectedDataPath, destPath, true);
        }

        
    }
}
