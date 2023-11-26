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

        private void DodaRekord()
        {
            string connectionString = "Data Source=magazyn.db;Version=3;";

            using (SQLiteConnection polaczenie = new SQLiteConnection(connectionString))
            {
                polaczenie.Open();

                // Assuming your table has columns: typProduktu, kodProduktu, nazwaProduktu, iloscProduktu, cenaProduktu
                string zapytanie = "INSERT INTO produkty (typProduktu, kodProduktu, nazwaProduktu, iloscProduktu, cenaProduktu) VALUES (@Typ, @Kod, @Nazwa, @Ilosc, @Cena)";

                using (SQLiteCommand komenda = new SQLiteCommand(zapytanie, polaczenie))
                {
                    // Assuming you have variables with the values you want to insert
                    komenda.Parameters.AddWithValue("@Typ", "NowyTyp");
                    komenda.Parameters.AddWithValue("@Kod", "NowyKod");
                    komenda.Parameters.AddWithValue("@Nazwa", "NowaNazwa");
                    komenda.Parameters.AddWithValue("@Ilosc", 10);
                    komenda.Parameters.AddWithValue("@Cena", 99.99);

                    komenda.ExecuteNonQuery();
                }
            }
        }
    }
}
