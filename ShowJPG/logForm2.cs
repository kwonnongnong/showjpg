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
        string strDir = "C:/device_testpic/";
        public logForm2()
        {
            InitializeComponent();
        }
        public logForm2( Form1 mForm)
        {
            InitializeComponent();
            mainForm = mForm;
            list_renew();
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
        private SQLiteDataReader select_log()
        {
            sqlconn = Sqlconnect();

            string strSQL_sel = "select * from log  order by No desc limit 10 offset " + sql_offset + ";";
            SQLiteCommand sqlcmd = new SQLiteCommand(strSQL_sel, sqlconn);
            SQLiteDataReader rd = sqlcmd.ExecuteReader();
            sqlcmd.Dispose();
           // Sqldiscon(conn);
            return rd;

        }
        private void list_renew()
        {
            sql_offset = 0;

            SQLiteDataReader rd = select_log();

            listBox1.Items.Clear();
            while (rd.Read())
            {
                Console.Write(rd["F_name"] + " ");
                Console.WriteLine(rd["date"]);
                listBox1.Items.Add(rd["date"]);
            }
            rd.Close();
            sqlconn.Close();
        }
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {   /****tempcode**절대수정할것!!***/
            Console.WriteLine("클릭됨");
            if (sql_offset > 0)
                sql_offset = sql_offset - offset_size;
            else
                Console.WriteLine("가장 최근 Log입니다");
            SQLiteDataReader rd = select_log();

            listBox1.Items.Clear();
            while (rd.Read())
            {
                Console.Write(rd["F_name"] + " ");
                Console.WriteLine(rd["date"]);
                listBox1.Items.Add(rd["date"]);
            }
            rd.Close();
            sqlconn.Close();
        
            
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            list_renew();
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            sql_offset = sql_offset + offset_size;
            SQLiteDataReader rd = select_log();

            //수정필요
            listBox1.Items.Clear();
            while (rd.Read())
            {
                Console.Write(rd["F_name"] + " ");
                Console.WriteLine(rd["date"]);
                listBox1.Items.Add(rd["date"]);
            }
            rd.Close();
            sqlconn.Close();

        }

        private void showPhoto(string f_name)
        {
            string P_fname = strDir +f_name+".jpg" ;
            Console.WriteLine(P_fname);
            try
            {
                Bitmap PhotoImage = new Bitmap(P_fname);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = (Image)PhotoImage;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "Error!! : filename error");
            }
        }//image 출력부


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Console.WriteLine("as"+listBox1.SelectedValue.ToString());
           // Console.WriteLine("asdsa"+listBox1.SelectedItems.ToString());
            showPhoto(listBox1.SelectedItem.ToString());

        }
    }
}
