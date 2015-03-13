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
    public partial class log_show : Form
    {
        SQLiteConnection conn;
        string strConn = @"Data Source=\mydb.db";
        public log_show()
        {
            InitializeComponent();
        }
        private void create_table()
        {
            using(SQLiteConnection conn = new SQLiteConnection(strConn)){
                conn.Open();
                
            }
        }
    }
}
