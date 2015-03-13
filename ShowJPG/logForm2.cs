using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace ShowJPG
{
    public partial class logForm2 : Form
    {
        Form1 mainForm;
        SQLiteConnection sqlconn;
        string connStr = @"Data Source=.\mydb.db";
        public logForm2()
        {
            InitializeComponent();
        }
        public logForm2( Form1 mForm)
        {
            InitializeComponent();
            mainForm = mForm;
        }
        private SQLiteConnection Sqlconnect()
        {
            var conn = new SQLiteConnection(connStr);
            return conn.OpenAndReturn();
            

        }
        private void Sqldiscon(SQLiteConnection conn)
        {
            conn.Close();
        }
        private DataSet select_Adapter()
        {
            SQLiteConnection conn = Sqlconnect();
            DataSet ds = new DataSet();
            string sql = "select * from log";
            var adpt = new SQLiteDataAdapter(sql, conn);
            adpt.Fill(ds);

            return ds;

        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("클릭됨");
            DataSet ds = select_Adapter();
            DataTable table = ds.Tables["log"];
            foreach (DataRow row in table.Rows)//exception 발생함 수정필요

            {
                Console.WriteLine(row["No"].ToString() + row["F_name"].ToString() + row["date"].ToString());
            }
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}
