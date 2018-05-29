using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
        DataTable sessDataTable = new DataTable();

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
                                            +   data.supervisorData2._Ten + " "
                                            +   data.supervisorData2._maGSV + " "
                                            +   data.sessNotes;
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

        DataTable getResultsTableFromList(List<SessionData> resultsList)
        {
            DataTable resultsTable = new DataTable();            

            resultsTable.Columns.Add("caseCode", typeof(String));
            resultsTable.Columns.Add("sessionCode", typeof(String));
            resultsTable.Columns.Add("suspectName", typeof(String));
            resultsTable.Columns.Add("inspectorName", typeof(String));
            resultsTable.Columns.Add("inspectorCode", typeof(String));
            resultsTable.Columns.Add("supervisorName1", typeof(String));
            resultsTable.Columns.Add("supervisorName2", typeof(String));
            resultsTable.Columns.Add("sessionNotes", typeof(String));
            resultsTable.Columns.Add("sessionPath", typeof(String));
            
            foreach (SessionData session in resultsList)
            {
                resultsTable.Rows.Add(session.caseData._maVuAn, session.sessionCode, session.suspectData._Ten, session.inspectData._Ten, session.inspectData._maDTV, session.supervisorData1._Ten, session.supervisorData2._Ten,
                    session.sessNotes, session.SessionPath);
            }
            return resultsTable;
        }

        private void UpdateSearchResults()
        {
            //var bindingSource = new BindingSource();
            //// Bind BindingSource1 to the list of states.            
            //bindingSource.DataSource = SearchResults;
            //this.listBoxSearchResults.DataSource = bindingSource;
            //listBoxSearchResults.BindingContext = this.BindingContext;
            //listBoxSearchResults.DisplayMember = "SessionPath";
            ////listBoxSearchResults.ValueMember = "suspectName";
            //listBoxSearchResults.Refresh();

            //////////////////////////////////////////////////////

            sessDataTable = getResultsTableFromList(SearchResults);
            dataGridViewResults.DataSource = sessDataTable;

            txtCaseCode.DataBindings.Clear();
            txtSuspectName.DataBindings.Clear();
            txtInsptectorName.DataBindings.Clear();
            txtInspectorCode.DataBindings.Clear();
            txtSupervisorName1.DataBindings.Clear();
            txtSupervisorName2.DataBindings.Clear();
            txtNoteView.DataBindings.Clear();

            txtCaseCode.DataBindings.Add("text", sessDataTable, "caseCode"); ;
            txtSuspectName.DataBindings.Add("text", sessDataTable, "suspectName");
            txtInsptectorName.DataBindings.Add("text", sessDataTable, "inspectorName");
            txtInspectorCode.DataBindings.Add("text", sessDataTable, "inspectorCode");
            txtSupervisorName1.DataBindings.Add("text", sessDataTable, "supervisorName1");
            txtSupervisorName2.DataBindings.Add("text", sessDataTable, "supervisorName2");
            txtNoteView.DataBindings.Add("text", sessDataTable, "sessionNotes");

            dataGridViewResults.Columns[0].HeaderText = "Mã vụ án";
            dataGridViewResults.Columns[1].HeaderText = "Mã PHC";
            dataGridViewResults.Columns[2].HeaderText = "Đối tượng";
            dataGridViewResults.Columns[3].HeaderText = "Điều tra viên";           
            dataGridViewResults.Columns[8].HeaderText = "Đường dẫn";

            dataGridViewResults.Columns[0].Width = 80;
            dataGridViewResults.Columns[1].Width = 80;
            dataGridViewResults.Columns[2].Width = 120;
            dataGridViewResults.Columns[3].Width = 120;            
            dataGridViewResults.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridViewResults.Columns[4].Visible = false;
            dataGridViewResults.Columns[5].Visible = false;
            dataGridViewResults.Columns[6].Visible = false;
            dataGridViewResults.Columns[7].Visible = false;            
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
                    if (!string.IsNullOrEmpty(session.caseData._maVuAn))
                        if (session.caseData._maVuAn.Contains(searchParam.caseCode)) { SearchResults.Add(session); continue; }
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
                    if (!string.IsNullOrEmpty(session.supervisorData2._Ten))
                        if (session.supervisorData2._Ten.Contains(searchParam.supervisorName2)) { SearchResults.Add(session); continue; }
                }
                if (!string.IsNullOrEmpty(searchParam.notes))
                {
                    if (!string.IsNullOrEmpty(session.sessNotes))
                        if (session.sessNotes.Contains(searchParam.notes)) { SearchResults.Add(session); continue; }
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
            CommonParam.mSesData.caseData._Ten = sessData.caseData._Ten;
            CommonParam.mSesData.suspectData._Ten = sessData.suspectData._Ten;
            CommonParam.mSesData.inspectData._Ten = sessData.inspectData._Ten;
            CommonParam.mSesData.supervisorData1._Ten = sessData.supervisorData1._Ten;
            CommonParam.mSesData.supervisorData2._Ten = sessData.supervisorData2._Ten;
            CommonParam.mSesData.caseData._maVuAn = sessData.caseData._maVuAn;
            CommonParam.mSesData.suspectData._MaDT = sessData.suspectData._MaDT;
            CommonParam.mSesData.inspectData._maDTV = sessData.inspectData._maDTV;
            CommonParam.mSesData.supervisorData1._maGSV = sessData.supervisorData1._maGSV;
            CommonParam.mSesData.supervisorData2._maGSV = sessData.supervisorData2._maGSV;
            CommonParam.mSesData.sessPlace = sessData.sessPlace;
            CommonParam.mSesData.sessNotes = sessData.sessNotes;
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

        private void dataGridViewResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;// get the Row Index
            DataGridViewRow selectedRow = dataGridViewResults.Rows[index];
            selectedDataPath = selectedRow.Cells[8].Value.ToString();
            SessionData data = CommonParam.LoadObject<SessionData>(selectedDataPath + CommonParam.SessionFileName);
            getReplayInfo(data);
            //MessageBox.Show(selectedRow.Cells[7].Value.ToString());
        }

        
    }
}
