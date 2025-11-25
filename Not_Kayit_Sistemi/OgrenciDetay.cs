using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Not_Kayit_Sistemi
{
    public partial class OgrenciDetay : Form
    {
        public OgrenciDetay()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            OgrenciGiris ogrenciGiris = new OgrenciGiris();
            this.Hide();
        }
        public string numara;
        SqlConnection baglanti = new SqlConnection(@"Data Source=MELEK;Initial Catalog=DbNotKayit;Integrated Security=True;Encrypt=False");

        private void OgrenciDetay_Load(object sender, EventArgs e)
        {
            lblNumara.Text = numara;

            baglanti.Open();

            SqlCommand komut = new SqlCommand("SELECT * FROM Tbl_Ders WHERE OgrNumara = @ogrNumara", baglanti);
            komut.Parameters.AddWithValue("@ogrNumara", numara);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read()) 
            {
                lblAdSoyad.Text = dr[2].ToString() + " " + dr[3].ToString();
                lblSinav1.Text = dr[4].ToString();
                lblSinav2.Text = dr[5].ToString();
                lblSinav3.Text = dr[6].ToString();
                lblOrtalama.Text = dr[7].ToString();
                lblDurum.Text = dr[8].ToString();
            }
            baglanti.Close();
        }
    }
}
