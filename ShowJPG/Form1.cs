﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;


namespace ShowJPG
{
    public partial class Form1 : Form
    {
        int Rwidth;
        int Rheight;
        int Pwidth;
        int Pheight;
        Rectangle[] Rect = new Rectangle[3];
        string strDir = "..\\..\\..\\..\\JPG_files\\";
        string[] fnames = { "S1.jpg", "S2.jpg", "S3.jpg" };
        Bitmap PhotoImage;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            int i, n=0;
            Graphics formGraphics = pictureBox1.CreateGraphics();
            for(i=0; i<3; i++) formGraphics.FillRectangle(Brushes.Black, Rect[i]);
           // serialPort1;

            if (sender == this.button1) { progressBar1.Value = progressBar1.Value + 10; }
            else if (sender == this.button2) { progressBar1.Value = progressBar1.Value + 10; progressBar2.Value = progressBar2.Value + 10; }
            else if (sender == this.button3) { progressBar2.Value = progressBar2.Value + 10; }
            else return;
            
            
            if (progressBar1.Value == 100 && progressBar2.Value == 100)
            {
                progressBar1.Value = 0;
                progressBar2.Value = 0;
                n = 1;
            }

            else if (progressBar1.Value == 100){
                progressBar1.Value = 0;
                progressBar2.Value = 0;
                n = 0;
            }
            else if (progressBar2.Value == 100) {
                progressBar1.Value = 0;
                progressBar2.Value = 0;
                n = 2;
            }

            formGraphics.FillRectangle(Brushes.Red, Rect[n]);
            DownloadRemoteImageFile("https://z.enha.kr/http://rigvedawiki.net/r1/pds/_ec_95_84_ec_9d_b4_ec_82_ac_ec_b9_b4_20_ed_83_80_ec_9d_b4_ea_b0_80/aisaka_taiga.jpg");
            showPhoto(PhotoImage);
            formGraphics.Dispose();
        }

        private void DrawGrid()
        {
            Rwidth = pictureBox1.ClientSize.Width;
            Rheight = pictureBox1.ClientSize.Height;

            Pwidth = pictureBox2.ClientSize.Width;
            Pheight = pictureBox2.ClientSize.Height;

            double pi = Math.PI;
            int x = Rheight - (int)(Rheight * Math.Cos(pi * 50 / 180));
            int y = Rheight - (int)(Rheight * Math.Sin(pi * 30 / 180))+20;
            Rect[0] = new Rectangle(x, y, 20, 20);
            Rect[1] = new Rectangle(Rwidth / 2-10, Rheight/3, 20, 20);
            Rect[2] = new Rectangle(Rwidth - x-20, y, 20, 20);


            Graphics formGraphics = pictureBox1.CreateGraphics();
            formGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            formGraphics.Clear(Color.Black);

            Pen pen1 = new Pen(Color.FromArgb(255, 0, 100, 0),2);
            Pen pen2 = new Pen(Color.FromArgb(255, 0, 220, 100),2);

            formGraphics.DrawPie(pen1, 0, 0, 2 * Rheight, 2 * Rheight, 180, 130);
            formGraphics.DrawPie(pen2, Rwidth - 2 * Rheight, 0, 2 * Rheight, 2 * Rheight, 230, 130);

            pen1.Dispose();
            pen2.Dispose();
            formGraphics.Dispose();
        }//감지부분 출력부

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            DrawGrid();
            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                Console.Write("포트연결성공");
            }
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            this.Dispose();
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
                MessageBox.Show("Error!! : filename error");
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
                    using (Stream inputStream = response.GetResponseStream())//respon된 stream INputStream에 삽입
                    PhotoImage = new Bitmap(inputStream);
                   // return true;
                }
                else
                {
                   // return false;
                }
            }
            catch (WebException e)
            {
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
            int i, n = 0;
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();
            Graphics formGraphics = pictureBox1.CreateGraphics();
            for (i = 0; i < 3; i++) formGraphics.FillRectangle(Brushes.Black, Rect[i]);
            // serialPort1;
            this.Invoke(new Action(delegate()
            {
                if (indata == "100\r") progressBar1.Value = progressBar1.Value + 10;
                else if (indata == "010\r") { progressBar1.Value = progressBar1.Value + 10; progressBar2.Value = progressBar2.Value + 10; }
                else if (indata == "001\r") { progressBar2.Value = progressBar2.Value + 10; }
                else Console.WriteLine(indata);

                if (progressBar1.Value == 100 && progressBar2.Value == 100)
                {
                    progressBar1.Value = 0;
                    progressBar2.Value = 0;
                    n = 1;
                }

                else if (progressBar1.Value == 100)
                {
                    progressBar1.Value = 0;
                    progressBar2.Value = 0;
                    n = 0;
                }
                else if (progressBar2.Value == 100)
                {
                    progressBar1.Value = 0;
                    progressBar2.Value = 0;
                    n = 2;
                }
            }
                ));
           


           

            formGraphics.FillRectangle(Brushes.Red, Rect[n]);
            DownloadRemoteImageFile("https://z.enha.kr/http://rigvedawiki.net/r1/pds/_ec_95_84_ec_9d_b4_ec_82_ac_ec_b9_b4_20_ed_83_80_ec_9d_b4_ea_b0_80/aisaka_taiga.jpg");
            showPhoto(PhotoImage);
            formGraphics.Dispose();
        }   
       //****//
    }
}
