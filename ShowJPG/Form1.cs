using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Data.SQLite;
using System.Windows.Forms;



namespace ShowJPG
{
    public partial class Form1 : Form
    { 
        int Rwidth;
        int Rheight;
        int Pwidth;
        int Pheight;
        string connStr = @"Data Source=.\mydb.db";
        SQLiteConnection sqlconn;
        public logForm2 logform2;
        Rectangle[] Rect = new Rectangle[3];
       //string strDir = "..\\..\\..\\..\\JPG_files\\";
       //string[] fnames = { "S1.jpg", "S2.jpg", "S3.jpg" };
        Bitmap PhotoImage;
        
        string ipaddr;


        public Form1()
        {
            InitializeComponent();
            logform2 = new logForm2(this);
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
        private void SqlInsert(string f_name,string date)
        {
            sqlconn = Sqlconnect();

            //sqlconn.Open();

            string strSQL = "INSERT INTO log (F_name,date) VALUES ('" + f_name + "','" + date + "');";
            SQLiteCommand sqlcmd = new SQLiteCommand(strSQL, sqlconn);
            sqlcmd.ExecuteNonQuery();
            sqlcmd.Dispose();

            //return sqlconn;


        }
        private void DrawGrid()
        {
            Rwidth = pictureBox1.ClientSize.Width;
            Rheight = pictureBox1.ClientSize.Height;

            Pwidth = pictureBox2.ClientSize.Width;
            Pheight = pictureBox2.ClientSize.Height;

            double pi = Math.PI;
            int x = Rheight - (int)(Rheight * Math.Cos(pi * 50 / 180));
            int y = Rheight - (int)(Rheight * Math.Sin(pi * 30 / 180)) + 20;
            Rect[0] = new Rectangle(x, y, 20, 20);
            Rect[1] = new Rectangle(Rwidth / 2 - 10, Rheight / 3, 20, 20);
            Rect[2] = new Rectangle(Rwidth - x - 20, y, 20, 20);


            Graphics formGraphics = pictureBox1.CreateGraphics();
            formGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            formGraphics.Clear(Color.Black);

            Pen pen1 = new Pen(Color.FromArgb(255, 0, 100, 0), 2);
            Pen pen2 = new Pen(Color.FromArgb(255, 0, 220, 100), 2);

            formGraphics.DrawPie(pen1, 0, 0, 2 * Rheight, 2 * Rheight, 180, 130);
            formGraphics.DrawPie(pen2, Rwidth - 2 * Rheight, 0, 2 * Rheight, 2 * Rheight, 230, 130);

            pen1.Dispose();
            pen2.Dispose();
            formGraphics.Dispose();
        }//감지부분 출력부

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            button4.Enabled = false;
            DrawGrid();
            ipaddr ="http://"+textBox1.Text.ToString()+":81/snapshot.cgi?user=admin&pwd=888888";
            //ipaddr = "https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcT-1s4Jm1mYuRBufxyVI23ylhGpSoubvgWd8p5UkZtvwKgFRaDJ6w";
           // ipaddr = "http://uwins.ulsan.ac.kr/COMM/StudPhoto.aspx?hagbeon=20102489"; 
            serialPort1.PortName = comboBox1.SelectedItem.ToString();
            serialPort1.BaudRate = Int32.Parse(comboBox2.SelectedItem.ToString());
            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                Console.Write("포트연결성공");
            }
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            this.Dispose();
            button4.Enabled = true;
            serialPort1.Close();
        }

        private void showPhoto(Bitmap inputBitmap)
        {
            //string fname = strDir + fnames[n];
            try
            {
                // PhotoImage = new Bitmap(inputstream);
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox2.Image = (Image)PhotoImage;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message+"Error!! : filename error");
            }
        }//image 출력부


        private void DownloadRemoteImageFile(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                bool bImage = response.ContentType.StartsWith("image",
                StringComparison.OrdinalIgnoreCase);//request된 패킷의 contenType이 image 형태인지 확인
                if ((response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Moved ||
                    response.StatusCode == HttpStatusCode.Redirect) &&
                    bImage)
                {
                    using (Stream inputStream = response.GetResponseStream()){//respon된 stream INputStream에 삽입
                        try
                        {
                            PhotoImage = new Bitmap(inputStream);//stream to bitmap
                            string date = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                            String fileName = "c:/device_testpic/" + date  + ".jpg";
                            PhotoImage.Save(fileName);//사진저장
                            SqlInsert(date + ".jpg", date);
                            sqlconn.Close();
                            System.Threading.Thread.Sleep(1000);
                        }
                        catch (ArgumentException e)
                        {
                            Console.Write("스트림이 NULL" + e.Message);//stream 이 비어있을때
                        }
                    }
                    Console.Write("사진");
                    // return true;
                }
                else
                {
                    Console.Write("사진안찎힘");
                    // return false;
                }
            }
            catch (WebException e)
            {
                Console.Write("??");
                Console.Write("에러 메시지" + e.Message);
                // return false;
            }
        }
        //**********************************************************************//
        //ipcamera 이미지 획득부//
        /*
        private static bool DownloadRemoteImageFile(string url, string fileName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                bool bImage = response.ContentType.StartsWith("image",
                StringComparison.OrdinalIgnoreCase);//request된 패킷의 contenType이 image 형태인지 확인
                if ((response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Moved ||
                    response.StatusCode == HttpStatusCode.Redirect) &&
                    bImage)
                {
                    using (Stream inputStream = response.GetResponseStream())//respon된 stream INputStream에 삽입
                    using (Stream inputStream2 = response.GetResponseStream())//respon된 stream INputStream에 삽입
                    
                       
                    using (Stream outputStream = File.OpenWrite(fileName))//outputStream은 fileopen과 연결
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        do
                        {

                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);//input stream의 크기를 알아내서
                            outputStream.Write(buffer, 0, bytesRead);//파일로 쓰기
                        } while (bytesRead != 0);
                    }


                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (WebException e)
            {
                Console.Write("에러 메시지" + e.Message);
                return false;
            }
        }*/
        //**********************************************************************//
        //시리얼 입력시 작동부분//
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int i, n=2 ;
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();
            Graphics formGraphics = pictureBox1.CreateGraphics();
            for (i = 0; i < 3; i++) formGraphics.FillRectangle(Brushes.Black, Rect[i]);
            // serialPort1;
            this.Invoke(new Action(delegate()
            {
                if (indata == "100\r") { progressBar1.Value = progressBar1.Value + 10; n = 0; }
                else if (indata == "010\r") { progressBar3.Value = progressBar3.Value + 10; n = 1; }
                else if (indata == "001\r") { progressBar2.Value = progressBar2.Value + 10; n = 2; }
                //else Console.WriteLine(indata);
                Console.WriteLine(indata);

                if (progressBar3.Value == 100 )
                {
                    progressBar3.Value = 0;
                    
                }
                else if (progressBar1.Value == 100)
                {
                    progressBar1.Value = 0;
                    progressBar2.Value = 0;
                    
                }
                else if (progressBar2.Value == 100)
                {
                    progressBar1.Value = 0;
                    progressBar2.Value = 0;
                   
                }
            }
                ));

            formGraphics.FillRectangle(Brushes.Red, Rect[n]);
            //Console.WriteLine(ipaddr);
            if (indata == "pic\r") {
                DownloadRemoteImageFile(ipaddr);
            }
            showPhoto(PhotoImage);
            formGraphics.Dispose();
        }

        private void comboBox1_MouseDown(object sender, MouseEventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (string name in System.IO.Ports.SerialPort.GetPortNames())
                comboBox1.Items.Add(name);


        }

        private void comboBox2_MouseDown(object sender, MouseEventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Items.Add("9600");
            comboBox2.Items.Add("12800");
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            logform2.ShowDialog();
        }
        //****//
        private static string SendGCMNotification(string apiKey, string postData)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            //create request
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
            Request.Method = "POST";
            Request.KeepAlive = false;
            Request.ContentType = "application/json";
            Request.Headers.Add(string.Format("Authorization: key={0}", apiKey));
            Request.ContentLength = byteArray.Length;

            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //send Message
            try
            {
                WebResponse Response = Request.GetResponse();
                HttpStatusCode Responsecode = ((HttpWebResponse)Response).StatusCode;
                if (Responsecode.Equals(HttpStatusCode.Unauthorized) || Responsecode.Equals(HttpStatusCode.Forbidden))
                {
                    //var text = "unauthorize - need new token";
                }
                else if (!Responsecode.Equals(HttpStatusCode.OK))
                {
                    //var text = "response from web service isn't OK";
                }
                StreamReader Reader = new StreamReader(Response.GetResponseStream());
                string responseLine = Reader.ReadToEnd();
                Reader.Close();

                return responseLine;

            }
            catch (Exception e)
            {
                return "error";

            }
        }
        static string apikey = "AIzaSyDwEaU1yQ0nJfA0IW9BS3v-V_Y17yLtnOQ";
        static public string SendGCM(string regid, string ticker, string title, string message)
        {
            string postData =
                "{ \"registration_ids\":[\"" + regid + "\"], " +
                "\"data\": {\"ticker\":\"" + ticker + "\", " +
                "\"title\":\"" + title + "\", " +
                "\"message\": \"" + message + "\"}}";

            return SendGCMNotification(apikey, postData);
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine(SendGCM("APA91bFOjxsp-wC-hvyHhi0sluHoLASzOItiyea3Xkbek6db62Hh8PX_Er3B-2_N2NgGjXpFJZlkqyIV8xwxiFBezGXn-_ZWolXUROOxh3sQyvJk463mEZ_EEW8sb1p2EGE4moDAwO9I",
                "push", "title", "testest"));

        }
    }
}
