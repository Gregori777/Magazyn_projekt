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
    /// Logika interakcji dla klasy UsunMagazyn.xaml
    /// </summary>
    public partial class UsunMagazyn : Window
    {
        public UsunMagazyn()
        {
            InitializeComponent();
            SprawdzMagazyny();
        }
        private void SprawdzMagazyny()
        {
            string connectionString = $"Data Source=magazyn.db;Version=3;";// okreslamy zrodlo danych
            using (SQLiteConnection polaczenie = new SQLiteConnection(connectionString))
            { // tworzymy polaczenie
                polaczenie.Open();// otwieramy polaczenie z baza
                string zapytanie = "SELECT * FROM magazyny";// nasze zapytanie
                using SQLiteCommand komenda = new SQLiteCommand(zapytanie, polaczenie);// tworzymy komende ktora wysyla zapytanie do naszego polaczenia
                using SQLiteDataReader reader = komenda.ExecuteReader();// mozliwosc czytania danych(jesli dobrze zrozumialem)
                while (reader.Read())// petla czyta dane z bazy
                {
                    int id = Convert.ToInt32(reader["idMagazynu"]);
                    // Tworzymy nowy przycisk
                    ComboBoxItem nowaOpcja = new ComboBoxItem();
                    nowaOpcja.Content = $"M{id}";
                    nowaOpcja.Tag = id.ToString();
                    // Dodajemy opcję do ComboBox
                    cmbSortowanie.Items.Add(nowaOpcja);

                    // Unikatowy delegat zdarzenia dla opcji w ComboBox
                    
                }
                polaczenie.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = $"Data Source=magazyn.db;Version=3;";// okreslamy zrodlo danych
            ComboBoxItem wybranaOpcja = (ComboBoxItem)cmbSortowanie.SelectedItem;
            using SQLiteConnection polaczenie = new SQLiteConnection(connectionString);// tworzymy polaczenie
            polaczenie.Open();// otwieramy polaczenie z baza
            string zapytanie = $"DELETE FROM magazyny WHERE idMagazynu = {wybranaOpcja.Tag};";// nasze zapytanie
            using SQLiteCommand komenda = new SQLiteCommand(zapytanie, polaczenie);// tworzymy komende ktora wysyla zapytanie do naszego polaczenia
            komenda.ExecuteNonQuery();
            this.Close();
            MessageBox.Show("Pomyślnie usunięto magazyn!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
