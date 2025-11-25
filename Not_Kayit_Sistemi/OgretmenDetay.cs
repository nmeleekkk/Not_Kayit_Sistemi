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
    public partial class OgretmenDetay : Form
    {
        public OgretmenDetay()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public string sifre;

        SqlConnection baglanti = new SqlConnection(@"Data Source=MELEK;Initial Catalog=DbNotKayit;Integrated Security=True;Encrypt=False");

        private void OgretmenDetay_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'dbNotKayitDataSet1.Tbl_Ders' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.tbl_DersTableAdapter1.Fill(this.dbNotKayitDataSet1.Tbl_Ders);
            baglanti.Open();

            SqlCommand komut = new SqlCommand("SELECT * FROM Tbl_Ogretmen WHERE OgretmenSifre = @ogrtmenSifre",baglanti);
            komut.Parameters.AddWithValue("@ogrtmenSifre", sifre);
            SqlDataReader dr = komut.ExecuteReader();
            baglanti.Close();

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO Tbl_Ders(OgrNumara,OgrAd, OgrSoyad) VALUES (@ogrNumara,@ogrAd,@ogrSoyad)", baglanti);
            komut.Parameters.AddWithValue("@ogrNumara", mskNumara.Text);
            komut.Parameters.AddWithValue("@ogrAd", txtAd.Text);
            komut.Parameters.AddWithValue("@ogrSoyad", txtSoyad.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Öğrenci Sisteme Eklendi.");
            this.tbl_DersTableAdapter1.Fill(this.dbNotKayitDataSet1.Tbl_Ders);
        }

        int secilenID;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            secilenID = Convert.ToInt32(dataGridView1.Rows[secilen].Cells[0].Value); // ID

            mskNumara.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            txtAd.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            txtSoyad.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            txtSinav1.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            txtSinav2.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();
            txtSinav3.Text = dataGridView1.Rows[secilen].Cells[6].Value.ToString();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            SqlCommand komut = new SqlCommand("UPDATE Tbl_Ders SET OgrNumara = @ogrNumara, OgrAd = @ogrAd, OgrSoyad = @ogrSoyad, OgrS1 = @ogrS1, OgrS2=@ogrS2, OgrS3=@ogrS3 WHERE OgrId=@id", baglanti);

            komut.Parameters.AddWithValue("@ogrNumara", mskNumara.Text);
            komut.Parameters.AddWithValue("@ogrAd", txtAd.Text);
            komut.Parameters.AddWithValue("@ogrSoyad", txtSoyad.Text);
            komut.Parameters.AddWithValue("@ogrS1", txtSinav1.Text);
            komut.Parameters.AddWithValue("@ogrS2", txtSinav2.Text);
            komut.Parameters.AddWithValue("@ogrS3", txtSinav3.Text);
            komut.Parameters.AddWithValue("@id", secilenID);

            komut.ExecuteNonQuery();
            baglanti.Close();

            MessageBox.Show("Öğrenci Notları Güncellendi.");

            this.tbl_DersTableAdapter1.Fill(this.dbNotKayitDataSet1.Tbl_Ders);
        }

        private void btnHesapla_Click(object sender, EventArgs e)
        {
            double ortalama, s1, s2, s3;

            s1 = Convert.ToDouble(txtSinav1.Text);
            s2 = Convert.ToDouble(txtSinav2.Text);
            s3 = Convert.ToDouble(txtSinav3.Text);

            ortalama = (s1 + s2 + s3) / 3;
            lblOrtalama.Text = ortalama.ToString("0.00");

            string durum = (ortalama >= 50) ? "True" : "False";

            // 1) Ortalama ve Durum veritabanına kaydediliyor
            baglanti.Open();

            SqlCommand komut = new SqlCommand(
                "UPDATE Tbl_Ders SET Ortalama = @ort, Durum = @durum WHERE OgrId = @id",
                baglanti
            );

            komut.Parameters.AddWithValue("@ort", ortalama);
            komut.Parameters.AddWithValue("@durum", durum);
            komut.Parameters.AddWithValue("@id", secilenID);

            komut.ExecuteNonQuery();

            // 2) Geçen sayısı
            SqlCommand komutGecen = new SqlCommand(
                "SELECT COUNT(*) FROM Tbl_Ders WHERE Durum='True'", baglanti
            );
            int gecen = (int)komutGecen.ExecuteScalar();
            lblGecen.Text = gecen.ToString();

            // 3) Kalan sayısı
            SqlCommand komutKalan = new SqlCommand(
                "SELECT COUNT(*) FROM Tbl_Ders WHERE Durum='False'", baglanti
            );
            int kalan = (int)komutKalan.ExecuteScalar();
            lblKalan.Text = kalan.ToString();

            baglanti.Close();

            // 4) Tabloyu yenile
            this.tbl_DersTableAdapter1.Fill(this.dbNotKayitDataSet1.Tbl_Ders);

            MessageBox.Show("Ortalama, Durum, Geçen-Kalan güncellendi.");
        }
    }
}
