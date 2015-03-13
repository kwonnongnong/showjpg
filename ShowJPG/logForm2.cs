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
        SQLiteConnection sqlconn;
        string connStr = @"Data Source=\mydb.db";
        public logForm2()
        {
            InitializeComponent();
        }
        private SQLiteConnection Sqlconnect()
        {
            using (var conn = new SQLiteConnection(connStr))
            {
                return conn.OpenAndReturn();
            }

        }
        private void Sqldiscon(SQLiteConnection conn)
        {
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSet ds = select_Adapter();
            DataTable table = ds.Tables["log"];
            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine(row["No"].ToString() + row["F_name"].ToString() + row["date"].ToString());
            }
            

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

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
