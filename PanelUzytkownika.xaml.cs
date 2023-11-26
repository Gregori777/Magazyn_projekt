using System;
using System.Collections.Generic;
using System.Data;
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
using System.Data.SQLite;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Magazyn___projekt
{
    public partial class PanelUzytkownika : Window
    {
        public static ObservableCollection<Produkt> ListaProduktow = null;

        public PanelUzytkownika()
        {
            InitializeComponent();
            SprawdzNazweMagazynu();
            SprawdzMagazyny();
            przygotujWiazanie();
            WczytajDaneZBazy();
        }
        private void SprawdzNazweMagazynu()
        {
            string connectionString = $"Data Source=magazyn.db;Version=3;";// okreslamy zrodlo danych
            using (SQLiteConnection polaczenie = new SQLiteConnection(connectionString))
            { // tworzymy polaczenie
                polaczenie.Open();// otwieramy polaczenie z baza
                string zapytanie = "SELECT * FROM magazyny WHERE idMagazynu=1";// nasze zapytanie
                using SQLiteCommand komenda = new SQLiteCommand(zapytanie, polaczenie);// tworzymy komende ktora wysyla zapytanie do naszego polaczenia
                using SQLiteDataReader reader = komenda.ExecuteReader();// mozliwosc czytania danych(jesli dobrze zrozumialem)
                while (reader.Read())// petla czyta dane z bazy
                {
                    string pelnaNazwa;
                    string nazwa = reader["nazwaMagazynu"] as string;
                    string lokalizacja = reader["lokalizacjaMagazynu"] as string;

                    pelnaNazwa = nazwa + " - " + lokalizacja;
                    nazwaMagazynu.Text = pelnaNazwa;
                }
                polaczenie.Close();
            }
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
                    string pelnaNazwa;
                    int id = Convert.ToInt32(reader["idMagazynu"]);
                    string nazwa = reader["nazwaMagazynu"] as string;
                    string lokalizacja = reader["lokalizacjaMagazynu"] as string;
                    // Tworzymy nowy przycisk
                    Button nowyPrzycisk = new Button();

                    // Ustawiamy właściwości przycisku zgodnie z Twoim szablonem
                    nowyPrzycisk.FontSize = 20;
                    nowyPrzycisk.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1666BA"));
                    nowyPrzycisk.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DEECFB"));
                    nowyPrzycisk.BorderThickness = new Thickness(0);
                    nowyPrzycisk.Content = "M" + id;
                    nowyPrzycisk.Width = 75;
                    nowyPrzycisk.Height = 75;
                    nowyPrzycisk.Margin = new Thickness(20);
                    pelnaNazwa = nazwa + " - " + lokalizacja;
                    // Dodajemy szablon do nowego przycisku
                    nowyPrzycisk.Template = (ControlTemplate)Resources["MyButtonTemplate"]; // "MyButtonTemplate" to klucz szablonu zasobu XAML

                    // Dodajemy obsługę zdarzenia dla nowego przycisku
                    nowyPrzycisk.Click += (s, args) => ObslugaZdarzenia(id, pelnaNazwa);

                    // Dodajemy nowy przycisk do istniejącego kontenera (np. Grid)
                    stackPanel.Children.Add(nowyPrzycisk);
                }
                polaczenie.Close();
            }
        }
        private void ObslugaZdarzenia(int id, string pelnanazwa)
        {
            nazwaMagazynu.Text = pelnanazwa;
            string connectionString = $"Data Source=magazyn.db;Version=3;";// okreslamy zrodlo danych

            using (SQLiteConnection polaczenie = new SQLiteConnection(connectionString))
            { // tworzymy polaczenie
                polaczenie.Open();// otwieramy polaczenie z baza
                string zapytanie = $"SELECT typProduktu, kodProduktu, nazwaProduktu, iloscProduktu, cenaProduktu FROM produkty WHERE idMagazynu = {id}";// nasze zapytanie

                using SQLiteCommand komenda = new SQLiteCommand(zapytanie, polaczenie);// tworzymy komende ktora wysyla zapytanie do naszego polaczenia
                using SQLiteDataReader reader = komenda.ExecuteReader();// mozliwosc czytania danych(jesli dobrze zrozumialem)
                ListaProduktow.Clear();
                while (reader.Read())// petla czyta dane z bazy i dodaje je do kolekcji
                {
                    string typ = reader["typProduktu"] as string;
                    string kod = reader["kodProduktu"] as string;
                    string nazwa = reader["nazwaProduktu"] as string;
                    int ilosc = Convert.ToInt32(reader["iloscProduktu"]);
                    double cena = Convert.ToDouble(reader["cenaProduktu"]);

                    ListaProduktow.Add(new Produkt(typ, kod, nazwa, ilosc, cena));
                }

                polaczenie.Close();
            }
        }
        private void przygotujWiazanie() 
        {
            ListaProduktow = new ObservableCollection<Produkt>();
            lstProdukty.ItemsSource = ListaProduktow;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListaProduktow);
            view.SortDescriptions.Add(new SortDescription("Cena", ListSortDirection.Ascending));

            view.Filter = FiltrUzytkownika; // Wywołujemy filtrowanie
        }

        private void WczytajDaneZBazy() // czytanie danych z bazy
        {
            string connectionString = $"Data Source=magazyn.db;Version=3;";// okreslamy zrodlo danych

            using SQLiteConnection polaczenie = new SQLiteConnection(connectionString);// tworzymy polaczenie
            polaczenie.Open();// otwieramy polaczenie z baza
            string zapytanie = "SELECT typProduktu, kodProduktu, nazwaProduktu, iloscProduktu, cenaProduktu FROM produkty WHERE idMagazynu = 1";// nasze zapytanie

            using SQLiteCommand komenda = new SQLiteCommand(zapytanie, polaczenie);// tworzymy komende ktora wysyla zapytanie do naszego polaczenia
            using SQLiteDataReader reader = komenda.ExecuteReader();// mozliwosc czytania danych(jesli dobrze zrozumialem)
            while (reader.Read())// petla czyta dane z bazy i dodaje je do kolekcji
            {
                string typ = reader["typProduktu"] as string;
                string kod = reader["kodProduktu"] as string;
                string nazwa = reader["nazwaProduktu"] as string;
                int ilosc = Convert.ToInt32(reader["iloscProduktu"]);
                double cena = Convert.ToDouble(reader["cenaProduktu"]);

                ListaProduktow.Add(new Produkt(typ, kod, nazwa, ilosc, cena));
            }
            polaczenie.Close();
        }

        private bool FiltrUzytkownika(object item) // Funkcja odpowiedzialna za filtrowanie
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as Produkt).Nazwa.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lstProdukty.ItemsSource).Refresh();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
