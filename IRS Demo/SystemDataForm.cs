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
        // We use these three SQLite objects:
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private SQLiteDataAdapter DB;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();

        private SQLiteDataReader sql_datareader;

        public SystemDataForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            loadData();
            /*
            // create a new database connection:
            sql_con = new SQLiteConnection("Data Source=IRS.db;Version=3;New=True;Compress=True;");
            // open the connection:
            sql_con.Open();
            // create a new SQL command:
            sql_cmd = sql_con.CreateCommand();
            // Let the SQLiteCommand object know our SQL-Query:
            sql_cmd.CommandText = "CREATE TABLE userTbl (id integer primary key, userName varchar(100), role varchar(100));";
            // Now lets execute the SQL ;D
            sql_cmd.ExecuteNonQuery();
            // Lets insert something into our new table:
            sql_cmd.CommandText = "INSERT INTO userTbl (id, userName, role) VALUES (1, 'Admin', 'Quan tri vien');";
            // And execute this again ;D
            sql_cmd.ExecuteNonQuery();
            // ...and inserting another line:
            sql_cmd.CommandText = "INSERT INTO userTbl (id, userName, role) VALUES (2, 'Operator', 'Nguoi dung');";
            // And execute this again ;D
            sql_cmd.ExecuteNonQuery();
            */
            //// First lets build a SQL-Query again:
            //sqlite_cmd.CommandText = "SELECT * FROM userTbl";
            //// Now the SQLiteCommand object can give us a DataReader-Object:
            //sqlite_datareader = sqlite_cmd.ExecuteReader();

            //while (sqlite_datareader.Read())
            //{
            //    dataGridView1.Rows.Add(new object[] { 
            //sqlite_datareader.GetValue(0),  // U can use column index
            //sqlite_datareader.GetValue(sqlite_datareader.GetOrdinal("id")),  // Or column name like this
            //sqlite_datareader.GetValue(sqlite_datareader.GetOrdinal("userName")),
            //sqlite_datareader.GetValue(sqlite_datareader.GetOrdinal("role"))
            //});
            //}
        }

        private void loadData()
        {
            setConnection();
            sql_con.Open();

            sql_cmd = sql_con.CreateCommand();
            string CommandText = "select id, userName, role from  userTbl";
            DB = new SQLiteDataAdapter(CommandText, sql_con);
            DS.Reset();
            DB.Fill(DS);
            DT = DS.Tables[0];
            dataGridView1.DataSource = DT;
            sql_con.Close();
        }

        private void setConnection()
        {
            sql_con = new SQLiteConnection("Data Source=IRS.db;Version=3;New=False;Compress=True;");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        

        
    }
}
