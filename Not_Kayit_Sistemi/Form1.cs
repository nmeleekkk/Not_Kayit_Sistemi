using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Not_Kayit_Sistemi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OgrenciGiris ogrenciGiris = new OgrenciGiris();
            ogrenciGiris.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OgretmenGiris ogretmenGiris = new OgretmenGiris();
            ogretmenGiris.Show();
            this.Hide();
        }
    }
}
