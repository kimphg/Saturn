﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Finisar.SQLite;
using System.Data.SQLite;
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
            CommonParam.GetCasesInfo();
        }

        private void loadDataUsers()
        {     
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            dataSet.Reset();
            CommonParam.sql_DataAdaptUser.Fill(dataSet);
            dataTable = dataSet.Tables[0];
            dataGridViewUser.DataSource = dataTable;

            txtId0.DataBindings.Clear();
            txtUserName.DataBindings.Clear();
            txtRole.DataBindings.Clear();
            txtPwds.DataBindings.Clear();

            txtId0.DataBindings.Add("text", dataTable, "id");
            txtUserName.DataBindings.Add("text", dataTable, "userName");
            txtRole.DataBindings.Add("text", dataTable, "role");
            txtPwds.DataBindings.Add("text", dataTable, "pwds");

            dataGridViewUser.Columns[1].HeaderText = "Tên người dùng";
            dataGridViewUser.Columns[2].HeaderText = "Mô tả";
            dataGridViewUser.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewUser.Columns[1].Width = 300;            
            dataGridViewUser.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewUser.Columns[3].Visible = false;
        }

        private void loadDataCases()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            dataSet.Reset();
            CommonParam.sql_DataAdaptCase.Fill(dataSet);
            dataTable = dataSet.Tables[0];
            dataGridViewCase.DataSource = dataTable;

            txtId4.DataBindings.Clear();
            txtCaseName.DataBindings.Clear();
            txtCaseCode.DataBindings.Clear();
            txtCaseDescription.DataBindings.Clear();

            txtId4.DataBindings.Add("text", dataTable, "id");
            txtCaseName.DataBindings.Add("text", dataTable, "caseName");
            txtCaseCode.DataBindings.Add("text", dataTable, "caseCode");
            txtCaseDescription.DataBindings.Add("text", dataTable, "caseDescrpt");

            dataGridViewCase.Columns[1].HeaderText = "Tên vụ án";
            dataGridViewCase.Columns[2].HeaderText = "Mã vụ án";
            dataGridViewCase.Columns[3].HeaderText = "Mô tả vụ án";

            dataGridViewCase.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCase.Columns[1].Width = 200;
            dataGridViewCase.Columns[2].Width = 100;
            dataGridViewCase.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void loadDataInspectors()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            dataSet.Reset();
            CommonParam.sql_DataAdaptInspector.Fill(dataSet);
            dataTable = dataSet.Tables[0];            
            dataGridViewInspector.DataSource = dataTable;

            txtId1.DataBindings.Clear();
            txtInspName.DataBindings.Clear();
            txtInspCode.DataBindings.Clear();
            txtInspUnit.DataBindings.Clear();

            txtId1.DataBindings.Add("text", dataTable, "id");
            txtInspName.DataBindings.Add("text", dataTable, "inspName");
            txtInspCode.DataBindings.Add("text", dataTable, "inspCode");
            txtInspUnit.DataBindings.Add("text", dataTable, "inspUnit");            

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

            txtId2.DataBindings.Clear();
            txtSupeName.DataBindings.Clear();
            txtSupeCode.DataBindings.Clear();
            txtSupeUnit.DataBindings.Clear();

            txtId2.DataBindings.Add("text", dataTable, "id");
            txtSupeName.DataBindings.Add("text", dataTable, "supeName");
            txtSupeCode.DataBindings.Add("text", dataTable, "supeCode");
            txtSupeUnit.DataBindings.Add("text", dataTable, "supeUnit");

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

            txtId3.DataBindings.Clear();
            txtSuspName.DataBindings.Clear();
            txtSuspHKTT.DataBindings.Clear();
            txtSuspIdenNum.DataBindings.Clear();
            txtSuspJob.DataBindings.Clear();
            txtSuspCode.DataBindings.Clear();
            txtSuspSex.DataBindings.Clear();
            txtSuspNation.DataBindings.Clear();
            txtSuspAddress.DataBindings.Clear();
            txtSuspBirthday.DataBindings.Clear();

            txtId3.DataBindings.Add("text", dataTable, "id"); ;
            txtSuspName.DataBindings.Add("text", dataTable, "suspName");
            txtSuspHKTT.DataBindings.Add("text", dataTable, "suspHKTT");
            txtSuspIdenNum.DataBindings.Add("text", dataTable, "suspIdenNum");
            txtSuspJob.DataBindings.Add("text", dataTable, "suspJob");
            txtSuspCode.DataBindings.Add("text", dataTable, "suspCode");
            txtSuspSex.DataBindings.Add("text", dataTable, "suspSex");
            txtSuspNation.DataBindings.Add("text", dataTable, "suspNation");
            txtSuspAddress.DataBindings.Add("text", dataTable, "suspAddress");
            txtSuspBirthday.DataBindings.Add("text", dataTable, "suspBirthday");


            dataGridViewSuspect.Columns[1].HeaderText = "Họ tên đối tượng";
            dataGridViewSuspect.Columns[2].HeaderText = "Mã đ.tượng";
            dataGridViewSuspect.Columns[3].HeaderText = "Giới tính";
            dataGridViewSuspect.Columns[4].HeaderText = "Ngày sinh";
            dataGridViewSuspect.Columns[5].HeaderText = "Địa chỉ";

            dataGridViewSuspect.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewSuspect.Columns[1].Width = 150;
            dataGridViewSuspect.Columns[2].Width = 100;
            dataGridViewSuspect.Columns[3].Width = 100;
            dataGridViewSuspect.Columns[4].Width = 100;
            dataGridViewSuspect.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridViewSuspect.Columns[6].Visible = false;
            dataGridViewSuspect.Columns[7].Visible = false;
            dataGridViewSuspect.Columns[8].Visible = false;
            dataGridViewSuspect.Columns[9].Visible = false;
        }


        private void ExecuteQuery(string strQuery)
        {
            CommonParam.sql_Conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(strQuery, CommonParam.sql_Conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                MessageBox.Show("Lỗi! Không thể cập nhật được CSDL!");
            }
            
            CommonParam.sql_Conn.Close();
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
            if (tabControl1.SelectedIndex == 4)
            {
                loadDataCases();
            }
        }
        private void btnAdd0_Click(object sender, EventArgs e)
        {
            txtId0.Clear();
            txtUserName.Clear();
            txtRole.Clear();
            txtPwds.Clear();
            txtUserName.Focus();
            btnSave0.Enabled = true;
        }

        private void btnAdd1_Click(object sender, EventArgs e)
        {
            txtId1.Clear();
            txtInspName.Clear();
            txtInspCode.Clear();
            txtInspUnit.Clear();
            txtInspName.Focus();
            btnSave1.Enabled = true;
        }

        private void btnSave0_Click(object sender, EventArgs e)
        {
            string user = txtUserName.Text;           
            string role = txtRole.Text;
            string pwds = txtPwds.Text;
            if (user == "")
            {
                MessageBox.Show("Lỗi! Username không thể để trống!");
                return;                    
            }
            string strInsert = string.Format("INSERT INTO userTbl(userName, role, pwds) VALUES('{0}','{1}','{2}')", user, role, pwds);
            ExecuteQuery(strInsert);
            loadDataUsers();
            btnSave0.Enabled = false;
            loadDataUsers();
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            string name = txtInspName.Text;
            string code = txtInspCode.Text;
            string unit = txtInspUnit.Text;
            if (name == "")
            {
                MessageBox.Show("Lỗi! Tên không thể để trống!");
                return;
            }
            string strInsert = string.Format("INSERT INTO inspTbl(inspName, inspCode, inspUnit) VALUES('{0}','{1}','{2}')", name, code, unit);            
            ExecuteQuery(strInsert);
            loadDataInspectors();
            btnSave1.Enabled = false;
            loadDataInspectors();
        }

        private void btnUpd1_Click(object sender, EventArgs e)
        {
            string id = txtId1.Text;
            string name = txtInspName.Text;
            string code = txtInspCode.Text;
            string unit = txtInspUnit.Text;
            string strInsert = string.Format("UPDATE inspTbl set inspName='{0}', inspCode='{1}', inspUnit='{2}' where id = {3}", name, code, unit, id);
            ExecuteQuery(strInsert);
            loadDataInspectors();
        }

        private void btnDel0_Click(object sender, EventArgs e)
        {
            string id = txtId0.Text;
            if (id == "1")
            {
                MessageBox.Show("Không thể xóa tài khoản quản trị hệ thống");
                return;
            }
            else
            {
                string strInsert = string.Format("DELETE FROM userTbl where id='{0}'", id);
                ExecuteQuery(strInsert);
                loadDataUsers();
            }
        }

        private void btnDel1_Click(object sender, EventArgs e)
        {
            string id = txtId1.Text;
            string strInsert = string.Format("DELETE FROM inspTbl where id='{0}'", id);
            ExecuteQuery(strInsert);
            loadDataInspectors();
        }

        private void btnUpd0_Click(object sender, EventArgs e)
        {
            string id = txtId0.Text;
            string user = txtUserName.Text;
            string role = txtRole.Text;
            string pwds = txtPwds.Text;

            if (id == "1")
            {
                MessageBox.Show("Không thể sửa tài khoản quản trị hệ thống");
                return;
            }

            string strInsert = string.Format("UPDATE userTbl set username='{0}', role='{1}', pwds='{2}' where id = {3}", user, role, pwds, id);
            ExecuteQuery(strInsert);
            loadDataUsers();
        }

        private void btnAdd2_Click(object sender, EventArgs e)
        {
            txtId2.Clear();
            txtSupeName.Clear();
            txtSupeCode.Clear();
            txtSupeUnit.Clear();
            txtSupeName.Focus();
            btnSave2.Enabled = true;
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            string name = txtSupeName.Text;
            string code = txtSupeCode.Text;
            string unit = txtSupeUnit.Text;
            if (name == "")
            {
                MessageBox.Show("Lỗi! Tên không thể để trống!");
                return;
            }
            string strInsert = string.Format("INSERT INTO supeTbl(supeName, supeCode, supeUnit) VALUES('{0}','{1}','{2}')", name, code, unit);
            ExecuteQuery(strInsert);
            loadDataSupervisors();
            btnSave2.Enabled = false;
            loadDataSupervisors();
        }

        private void btnUpd2_Click(object sender, EventArgs e)
        {
            string id = txtId2.Text;
            string name = txtSupeName.Text;
            string code = txtSupeCode.Text;
            string unit = txtSupeUnit.Text;
            string strInsert = string.Format("UPDATE supeTbl set supeName='{0}', supeCode='{1}', supeUnit='{2}' where id = {3}", name, code, unit, id);
            ExecuteQuery(strInsert);
            loadDataSupervisors();
        }

        private void btnDel2_Click(object sender, EventArgs e)
        {
            string id = txtId2.Text;
            string strInsert = string.Format("DELETE FROM supeTbl where id='{0}'", id);
            ExecuteQuery(strInsert);
            loadDataSupervisors();
        }

        private void dataGridViewUser_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            loadDataUsers();
        }

        private void btnAdd3_Click(object sender, EventArgs e)
        {
            txtId3.Clear();
            txtSuspName.Clear();
            txtSuspHKTT.Clear();
            txtSuspIdenNum.Clear();
            txtSuspJob.Clear();
            txtSuspCode.Clear();
            txtSuspSex.Clear();
            txtSuspNation.Clear();
            txtSuspAddress.Clear();
            txtSuspBirthday.Clear();
            txtSuspName.Focus();

            btnSave3.Enabled = true;
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            if (txtSuspName.Text == "")
            {
                MessageBox.Show("Lỗi! Tên không thể để trống!");
                return;
            }
            string strInsert = string.Format("INSERT INTO suspTbl(suspName, suspCode, suspSex, suspBirthday, suspAddress, suspHKTT, suspIdenNum, suspJob, suspNation) VALUES('{0}','{1}','{2}', '{3}','{4}','{5}', '{6}','{7}','{8}')",
                txtSuspName.Text, txtSuspCode.Text, txtSuspSex.Text, txtSuspBirthday.Text, txtSuspAddress.Text, txtSuspHKTT.Text, txtSuspIdenNum.Text, txtSuspJob.Text, txtSuspNation.Text);
            ExecuteQuery(strInsert);
            loadDataSupervisors();
            btnSave3.Enabled = false;
            loadDataSuspects();

        }

        private void btnUpd3_Click(object sender, EventArgs e)
        {            
            string strInsert = string.Format("UPDATE suspTbl set suspName='{0}', suspCode='{1}', suspSex='{2}', suspBirthday='{3}', suspAddress='{4}', suspHKTT='{5}', suspIdenNum='{6}', suspJob='{7}', suspNation='{8}' where id = '{9}'", 
                txtSuspName.Text, txtSuspCode.Text, txtSuspSex.Text, txtSuspBirthday.Text, txtSuspAddress.Text, txtSuspHKTT.Text, txtSuspIdenNum.Text, txtSuspJob.Text, txtSuspNation.Text, txtId3.Text);
            ExecuteQuery(strInsert);
            loadDataSuspects();
        }

        private void btnDel3_Click(object sender, EventArgs e)
        {
            string strInsert = string.Format("DELETE FROM suspTbl where id='{0}'", txtId3.Text);
            ExecuteQuery(strInsert);
            loadDataSuspects();
        }

        private void btnAdd4_Click(object sender, EventArgs e)
        {
            txtId4.Clear();
            txtCaseName.Clear();
            txtCaseCode.Clear();
            txtCaseDescription.Clear();
            txtCaseName.Focus();
            btnSave4.Enabled = true;
        }

        private void btnSave4_Click(object sender, EventArgs e)
        {
            string name = txtCaseName.Text;
            string code = txtCaseCode.Text;
            string descrpt = txtCaseDescription.Text;
            if (name == "")
            {
                MessageBox.Show("Lỗi! Tên vụ án không thể để trống!");
                return;
            }
            string strInsert = string.Format("INSERT INTO caseTbl(caseName, caseCode, caseDescrpt) VALUES('{0}','{1}','{2}')", name, code, descrpt);
            ExecuteQuery(strInsert);
            loadDataCases();
            btnSave4.Enabled = false;
            loadDataCases();
        }

        private void btnUpd4_Click(object sender, EventArgs e)
        {
            string id = txtId4.Text;

            if (id == "1")
            {
                MessageBox.Show("Không thể sửa thông tin vụ án này");
                return;
            }

            string name = txtCaseName.Text;
            string code = txtCaseCode.Text;
            string descrpt = txtCaseDescription.Text;
            string strInsert = string.Format("UPDATE caseTbl set caseName='{0}', caseCode='{1}', caseDescrpt='{2}' where id = {3}", name, code, descrpt, id);
            ExecuteQuery(strInsert);
            loadDataCases();
        }

        private void btnDel4_Click(object sender, EventArgs e)
        {
            string id = txtId4.Text;

            if (id == "1")
            {
                MessageBox.Show("Không thể xóa thông tin này!");
                return;
            }

            string strInsert = string.Format("DELETE FROM caseTbl where id='{0}'", id);
            ExecuteQuery(strInsert);
            loadDataCases();
        }
        
    }
}
