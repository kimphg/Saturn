using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace IRS_Demo
{
	public partial class fLoginForm : Form
	{
		#region Combobox - Picturebox *******************************************
        public class User
        {
            public string EmployeeId { get; set; }
            public string EmployeeName { get; set; }
            public string EmployeePass { get; set; }
        }
        public List<User> Employees { get; set; }

        #endregion *******************************************
        //==================================================================================//
		public fLoginForm()
		{
			InitializeComponent();
            LoadDataForConbobox();
		}
                
        private DataTable m_DataTable = new DataTable();

		void LoadDataForConbobox()
		{
            DataSet dataSet = new DataSet();
            CommonParam.GetUsersInfo();
            dataSet.Reset();
            CommonParam.sql_DataAdaptUser.Fill(dataSet);
            m_DataTable = dataSet.Tables[0];
            cmbNameLogin.DataSource = m_DataTable;
            cmbNameLogin.DisplayMember = m_DataTable.Columns[1].ToString();            
		}

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string strUser = cmbNameLogin.GetItemText(cmbNameLogin.SelectedItem);
            string filterExpression = "";
            filterExpression = "userName=" + "'" + strUser + "'";
            DataRow[] rows = m_DataTable.Select(filterExpression);
            string strPass = rows[0].ItemArray[3].ToString();

            if (txbPassWord.Text != strPass)
            {
                MessageBox.Show("Mật khẩu không đúng, hãy thử lại!");
                return;
            }
            CommonParam.UserName = strUser;
            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                if (MessageBox.Show("Bạn có muốn thoát không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    e.Cancel = true;
        }

    }

}
