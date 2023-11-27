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
    /// Logika interakcji dla klasy EdytujPrzedmiot.xaml
    /// </summary>
    public partial class EdytujPrzedmiot : Window
    {
        public EdytujPrzedmiot()
        {
            InitializeComponent();
        }

        private void EdytujRekord(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=magazyn.db;Version=3;";

            using (SQLiteConnection polaczenie = new SQLiteConnection(connectionString))
            {
                polaczenie.Open();


                string zapytanie = "UPDATE produkty SET typProduktu = @Typ, kodProduktu = @Kod, nazwaProduktu = @Nazwa, iloscProduktu = @Ilosc, cenaProduktu = @Cena WHERE idProduktu = @ID;";

                using SQLiteCommand komenda = new SQLiteCommand(zapytanie, polaczenie);

                komenda.Parameters.AddWithValue("@ID", txtID.Text);
                komenda.Parameters.AddWithValue("@Typ", txtTyp.Text);
                komenda.Parameters.AddWithValue("@Kod", txtKod.Text);
                komenda.Parameters.AddWithValue("@Nazwa", txtNazwa.Text);
                komenda.Parameters.AddWithValue("@Ilosc", txtIlosc.Text);
                komenda.Parameters.AddWithValue("@Cena", txtCena.Text);

                komenda.ExecuteNonQuery();
                polaczenie.Close();
                MessageBox.Show("Zmodyfikowano rekord!");
                this.Close();
            }
        }
    }
}
