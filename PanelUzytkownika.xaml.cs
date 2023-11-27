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
        private ICollectionView collectionView;
        private GridViewColumnHeader ostatniaKolumna = null;
        private ListSortDirection ostatniKierunek = ListSortDirection.Ascending;

        public PanelUzytkownika()
        {
            InitializeComponent();
            SprawdzNazweMagazynu();
            SprawdzMagazyny();
            przygotujWiazanie();
            WczytajDaneZBazy();
        }

        private void Header_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader kolumna = (GridViewColumnHeader)sender;

            // Czyścimy sortowanie
            lstProdukty.Items.SortDescriptions.Clear();

            // Określenie pola do sortowania na podstawie treści nagłówka kolumny
            string sprawdz = kolumna.Content.ToString();
            string sortowanie = "";
            switch (sprawdz)
            {
                case "ID produktu":
                    sortowanie = "IDProduktu";
                    break;
                case "Typ produktu":
                    sortowanie = "TypProduktu";
                    break;
                case "Kod":
                    sortowanie = "Kod";
                    break;
                case "Nazwa":
                    sortowanie = "Nazwa";
                    break;
                case "Liczba sztuk":
                    sortowanie = "LiczbaSztuk";
                    break;
                case "Cena":
                    sortowanie = "CenaProduktu";
                    break;
                default:
                    break;
            }

            ListSortDirection kierunekSortowania;

            // Sprawdzenie, czy kolumna jest już posortowana
            if (ostatniaKolumna == kolumna && ostatniKierunek == ListSortDirection.Ascending)
            {
                kierunekSortowania = ListSortDirection.Descending;
            }
            else
            {
                kierunekSortowania = ListSortDirection.Ascending;
            }

            // Zastosowanie sortowania
            lstProdukty.Items.SortDescriptions.Add(new SortDescription(sortowanie, kierunekSortowania));

            // Przechowanie bieżącego nagłówka i kierunku sortowania dla następnego kliknięcia
            ostatniaKolumna = kolumna;
            ostatniKierunek = kierunekSortowania;
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
                string zapytanie = $"SELECT idProduktu, typProduktu, kodProduktu, nazwaProduktu, iloscProduktu, cenaProduktu FROM produkty WHERE idMagazynu = {id}";// nasze zapytanie

                using SQLiteCommand komenda = new SQLiteCommand(zapytanie, polaczenie);// tworzymy komende ktora wysyla zapytanie do naszego polaczenia
                using SQLiteDataReader reader = komenda.ExecuteReader();// mozliwosc czytania danych(jesli dobrze zrozumialem)
                ListaProduktow.Clear();
                while (reader.Read())// petla czyta dane z bazy i dodaje je do kolekcji
                {
                    int idProduktu = Convert.ToInt32(reader["idProduktu"]);
                    string typ = reader["typProduktu"] as string;
                    string kod = reader["kodProduktu"] as string;
                    string nazwa = reader["nazwaProduktu"] as string;
                    int ilosc = Convert.ToInt32(reader["iloscProduktu"]);
                    double cena = Convert.ToDouble(reader["cenaProduktu"]);

                    ListaProduktow.Add(new Produkt(idProduktu, typ, kod, nazwa, ilosc, cena));
                }

                polaczenie.Close();
            }
        }
        private void przygotujWiazanie() 
        {
            ListaProduktow = new ObservableCollection<Produkt>();
            lstProdukty.ItemsSource = ListaProduktow;
            collectionView = CollectionViewSource.GetDefaultView(ListaProduktow);

            

        }

        private void WczytajDaneZBazy() // czytanie danych z bazy
        {
            string connectionString = $"Data Source=magazyn.db;Version=3;";// okreslamy zrodlo danych

            using SQLiteConnection polaczenie = new SQLiteConnection(connectionString);// tworzymy polaczenie
            polaczenie.Open();// otwieramy polaczenie z baza
            string zapytanie = "SELECT idProduktu, typProduktu, kodProduktu, nazwaProduktu, iloscProduktu, cenaProduktu FROM produkty WHERE idMagazynu = 1";// nasze zapytanie

            using SQLiteCommand komenda = new SQLiteCommand(zapytanie, polaczenie);// tworzymy komende ktora wysyla zapytanie do naszego polaczenia
            using SQLiteDataReader reader = komenda.ExecuteReader();// mozliwosc czytania danych(jesli dobrze zrozumialem)
            while (reader.Read())// petla czyta dane z bazy i dodaje je do kolekcji
            {
                int idProduktu = Convert.ToInt32(reader["idProduktu"]);
                string typ = reader["typProduktu"] as string;
                string kod = reader["kodProduktu"] as string;
                string nazwa = reader["nazwaProduktu"] as string;
                int ilosc = Convert.ToInt32(reader["iloscProduktu"]);
                double cena = Convert.ToDouble(reader["cenaProduktu"]);

                ListaProduktow.Add(new Produkt(idProduktu, typ, kod, nazwa, ilosc, cena));
            }
            polaczenie.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterProducts();
        }

        private void cmbSortowanie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterProducts();
        }

        private void FilterProducts()
        {
            string filterText = txtFilter.Text.ToLower();
            string selectedCategory = (cmbSortowanie.SelectedItem as ComboBoxItem)?.Content.ToString();

            ObservableCollection<Produkt> filteredProducts = new ObservableCollection<Produkt>();

            foreach (Produkt product in ListaProduktow)
            {
                string propertyValue = null;

                switch (selectedCategory)
                {
                    case "ID produktu":
                        propertyValue = product.IDProduktu.ToString();
                        break;
                    case "Typ produktu":
                        propertyValue = product.TypProduktu;
                        break;
                    case "Kod":
                        propertyValue = product.Kod;
                        break;
                    case "Nazwa":
                        propertyValue = product.Nazwa;
                        break;
                    case "Liczba sztuk":
                        propertyValue = product.LiczbaSztuk.ToString();
                        break;
                    case "Cena":
                        propertyValue = product.CenaProduktu.ToString();
                        break;
                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(propertyValue) && propertyValue.ToLower().Contains(filterText))
                {
                    filteredProducts.Add(product);
                }
            }

            collectionView.Filter = item =>
            {
                Produkt product = item as Produkt;

                string propertyValue = null;

                switch (selectedCategory)
                {
                    case "ID produktu":
                        propertyValue = product.IDProduktu.ToString();
                        break;
                    case "Typ produktu":
                        propertyValue = product.TypProduktu;
                        break;
                    case "Kod":
                        propertyValue = product.Kod;
                        break;
                    case "Nazwa":
                        propertyValue = product.Nazwa;
                        break;
                    case "Liczba sztuk":
                        propertyValue = product.LiczbaSztuk.ToString();
                        break;
                    case "Cena":
                        propertyValue = product.CenaProduktu.ToString();
                        break;
                    default:
                        break;
                }

                return !string.IsNullOrEmpty(propertyValue) && propertyValue.ToLower().Contains(filterText);
            };
        }
    }
}
