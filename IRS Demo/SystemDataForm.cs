using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Finisar.SQLite;

namespace IRS_Demo
{
    public partial class SystemDataForm : Form
    {                
        

        public SystemDataForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            tabControl1.SelectedIndex = 0;
            loadDataUsers();
            CommonParam.GetInspectorsInfo();
            CommonParam.GetSupervisorsInfo();
            CommonParam.GetSuspectsInfo();
        }

        private void loadDataUsers()
        {     
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            dataSet.Reset();
            CommonParam.sql_DataAdaptUser.Fill(dataSet);
            dataTable = dataSet.Tables[0];
            dataGridViewUser.DataSource = dataTable;            
            dataGridViewUser.Columns[1].HeaderText = "Tên người dùng";
            dataGridViewUser.Columns[2].HeaderText = "Vai trò";
            dataGridViewUser.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewUser.Columns[1].Width = 300;            
            dataGridViewUser.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewUser.Columns[3].Visible = false;
        }

        private void loadDataInspectors()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            dataSet.Reset();
            CommonParam.sql_DataAdaptInspector.Fill(dataSet);
            dataTable = dataSet.Tables[0];
            dataGridViewInspector.DataSource = dataTable;
            dataGridViewInspector.Columns[1].HeaderText = "Tên điều tra viên";
            dataGridViewInspector.Columns[2].HeaderText = "Mã số điều tra viên";
            dataGridViewInspector.Columns[3].HeaderText = "Đơn vị công tác";

            dataGridViewInspector.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewInspector.Columns[1].Width = 200;
            dataGridViewInspector.Columns[2].Width = 200;
            dataGridViewInspector.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        private void loadDataSupervisors()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            dataSet.Reset();
            CommonParam.sql_DataAdaptSupervisor.Fill(dataSet);
            dataTable = dataSet.Tables[0];
            dataGridViewSupervisor.DataSource = dataTable;
            dataGridViewSupervisor.Columns[1].HeaderText = "Tên giám sát viên";
            dataGridViewSupervisor.Columns[2].HeaderText = "Mã số giám sát viên";
            dataGridViewSupervisor.Columns[3].HeaderText = "Đơn vị công tác";

            dataGridViewSupervisor.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewSupervisor.Columns[1].Width = 200;
            dataGridViewSupervisor.Columns[2].Width = 200;
            dataGridViewSupervisor.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void loadDataSuspects()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            dataSet.Reset();
            CommonParam.sql_DataAdaptSuspect.Fill(dataSet);
            dataTable = dataSet.Tables[0];
            dataGridViewSuspect.DataSource = dataTable;
            dataGridViewSuspect.Columns[1].HeaderText = "Họ tên đối tượng";
            dataGridViewSuspect.Columns[2].HeaderText = "Tên gọi khác";
            dataGridViewSuspect.Columns[3].HeaderText = "Giới tính";
            dataGridViewSuspect.Columns[4].HeaderText = "Ngày sinh";
            dataGridViewSuspect.Columns[5].HeaderText = "Địa chỉ";

            dataGridViewSuspect.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewSuspect.Columns[1].Width = 150;
            dataGridViewSuspect.Columns[2].Width = 100;
            dataGridViewSuspect.Columns[3].Width = 100;
            dataGridViewSuspect.Columns[4].Width = 100;
            dataGridViewSuspect.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void ExecuteQuery(string txtQuery)
        {
            CommonParam.sql_Conn.Open();
		    SQLiteCommand sql_cmd = CommonParam.sql_Conn.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            sql_cmd.ExecuteNonQuery();
            CommonParam.sql_Conn.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //string txtSQLQuery = "insert into  inspTbl (id, inspName, inspCode) values (4, 'Hoàng Minh Thái', 'DTV123')";
            //ExecuteQuery(txtSQLQuery);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                loadDataUsers();
            }
            if (tabControl1.SelectedIndex == 1)
            {                
                loadDataInspectors();
            }
            if (tabControl1.SelectedIndex == 2)
            {
                loadDataSupervisors();
            }
            if (tabControl1.SelectedIndex == 3)
            {
                loadDataSuspects();
            }
        }

       
    }
}
