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
                    data.SessionKeyText = data.inspectorName + " "
                                            + data.inspectorCode + " "
                                            + data.suspectData._Ten + " "
                                            + data.suspectData._MaDT + " "
                                            + data.supervisorName + " "
                                            + data.supervisorCode + " "
                                            + data.supervisorName2 + " "
                                            + data.supervisorCode2 + " "
                                            + data.Notes;
                    SessionHistory.Add(data);
                    
                }
            }
            //
            
            this.StartPosition = FormStartPosition.CenterScreen;
            comboBox1.SelectedIndex = 0;
            SearchResults = new List<SessionData>(SessionHistory);
            
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            searchParam.caseCode = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            searchParam.inspectorName = textBox2.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            searchParam.suspectName = textBox6.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            searchParam.sessionCode = textBox5.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            searchParam.supervisorName1 = textBox3.Text;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            searchParam.inspectorCode = textBox7.Text;
        }

        private void button1_Click(object sender, EventArgs e)//tim kiem
        {
            SearchResults.Clear();
            foreach (SessionData session in SessionHistory)
            {
                if (!string.IsNullOrEmpty(searchParam.inspectorName))
                {
                    if (!string.IsNullOrEmpty(session.inspectorName))
                        if (session.inspectorName.Contains(searchParam.inspectorName)) { SearchResults.Add(session); continue; }
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
                    if (!string.IsNullOrEmpty(session.supervisorName))
                        if (session.supervisorName.Contains(searchParam.supervisorName1)) { SearchResults.Add(session); continue; }
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
                    if (!string.IsNullOrEmpty(session.inspectorCode))
                        if (session.inspectorCode.Contains(searchParam.inspectorCode)) { SearchResults.Add(session); continue; }
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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            searchParam.notes = textBox4.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            searchParam.supervisorName2 = textBox8.Text;
        }

        private void listBoxSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pathToData= listBoxSearchResults.GetItemText(listBoxSearchResults.SelectedItem);
            SessionData data = CommonParam.LoadObject<SessionData>(pathToData+CommonParam.SessionFileName);
            this.textBoxInspectorCode.Text = data.inspectorCode;
            this.textBoxInsptectorName.Text = data.inspectorName;
            this.textBoxSuspectName.Text = data.suspectData._Ten;
            this.textBoxSupervisorName1.Text = data.supervisorName;
            this.textBoxSupervisorName2.Text = data.supervisorName2;
            this.textBoxCaseCode.Text = data.caseCode;
            this.txtNote_View.Text = data.Notes;

        }

        
    }
}
