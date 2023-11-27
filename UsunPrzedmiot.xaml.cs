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
    /// Logika interakcji dla klasy UsunPrzedmiot.xaml
    /// </summary>
    public partial class UsunPrzedmiot : Window
    {
        public UsunPrzedmiot()
        {
            InitializeComponent();
        }

        private void usuwanieRekordu(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=magazyn.db;Version=3;";

            using SQLiteConnection polaczenie = new SQLiteConnection(connectionString);
            polaczenie.Open();

            string zapytanie = "DELETE FROM produkty WHERE idProduktu = @doUsuniecia";

            using (SQLiteCommand komenda = new SQLiteCommand(zapytanie, polaczenie))
            {
                komenda.Parameters.AddWithValue("@doUsuniecia", txtKodUsun.Text);

                komenda.ExecuteNonQuery();
            }
            polaczenie.Close();

            MessageBox.Show("Usunięto rekord!");
            this.Close();
        }

     
    }
}
