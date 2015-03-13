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
        public log_show()
        {
            InitializeComponent();
        }
    }
}
