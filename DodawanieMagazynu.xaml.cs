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
    /// Logika interakcji dla klasy DodawanieMagazynu.xaml
    /// </summary>
    public partial class DodawanieMagazynu : Window
    {
        public DodawanieMagazynu()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = $"Data Source=magazyn.db;Version=3;";// okreslamy zrodlo danych

            using SQLiteConnection polaczenie = new SQLiteConnection(connectionString);// tworzymy polaczenie
            polaczenie.Open();// otwieramy polaczenie z baza
            string zapytanie = $"INSERT INTO magazyny(nazwaMagazynu, lokalizacjaMagazynu) VALUES ('{nazwaMagazynu.Text}','{lokalizacjaMagazynu.Text}');";// nasze zapytanie
            using SQLiteCommand komenda = new SQLiteCommand(zapytanie, polaczenie);// tworzymy komende ktora wysyla zapytanie do naszego polaczenia
            komenda.ExecuteNonQuery();
            this.Close();
            MessageBox.Show("Pomyślnie dodano magazyn!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
