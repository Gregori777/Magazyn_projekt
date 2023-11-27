using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Magazyn___projekt
{
    /// <summary>
    /// Logika interakcji dla klasy DodajPrzedmiot.xaml
    /// </summary>
    public partial class DodajPrzedmiot : Window
    {
        public DodajPrzedmiot()
        {
            InitializeComponent();
        }

        private void DodajRekord(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=magazyn.db;Version=3;";

            using (SQLiteConnection polaczenie = new SQLiteConnection(connectionString))
            {
                polaczenie.Open();

                
                string zapytanie = $"INSERT INTO produkty (idMagazynu, typProduktu, kodProduktu, nazwaProduktu, iloscProduktu, cenaProduktu) VALUES (@ID, @Typ, @Kod, @Nazwa, @Ilosc, @Cena)";

                using SQLiteCommand komenda = new SQLiteCommand(zapytanie, polaczenie);

                komenda.Parameters.AddWithValue("@ID", txtID.Text);
                komenda.Parameters.AddWithValue("@Typ", txtTyp.Text);
                komenda.Parameters.AddWithValue("@Kod", txtKod.Text);
                komenda.Parameters.AddWithValue("@Nazwa", txtNazwa.Text);
                komenda.Parameters.AddWithValue("@Ilosc", txtIlosc.Text);
                komenda.Parameters.AddWithValue("@Cena", txtCena.Text);

                komenda.ExecuteNonQuery();
                polaczenie.Close();
                MessageBox.Show("Dodano rekord!");
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
