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
        int sql_offset=0;//초기 offset 0 ;
        const int offset_size = 10;
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
            string sql = "select * from log limit 10 offset " + offset_size;
            var adpt = new SQLiteDataAdapter(sql, conn);
            adpt.Fill(ds);

            return ds;

        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {   /****tempcode**절대수정할것!!***/
            Console.WriteLine("클릭됨");
            if (sql_offset > 0)
                sql_offset = sql_offset - offset_size;
            else
                Console.WriteLine("가장 최근 Log입니다");
            DataSet ds = select_Adapter();
            DataTable table = ds.Tables["log"];
            foreach (DataRow row in table.Rows)//exception 발생함 수정필요
            {
                Console.WriteLine(row["No"].ToString() + row["F_name"].ToString() + row["date"].ToString());
            }
            /******************/
            
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            sql_offset = 0;
            DataSet ds = select_Adapter();
            DataTable table = ds.Tables["log"];
            foreach (DataRow row in table.Rows)//exception 발생함 수정필요
            {
                Console.WriteLine(row["No"].ToString() + row["F_name"].ToString() + row["date"].ToString());
            }
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            sql_offset = sql_offset + offset_size;
            DataSet ds = select_Adapter();
            DataTable table = ds.Tables["log"];
            foreach (DataRow row in table.Rows)//exception 발생함 수정필요
            {
                Console.WriteLine(row["No"].ToString() + row["F_name"].ToString() + row["date"].ToString());
            }

        }
    }
}
