using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.IO;
using System.Data.SQLite;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Magazyn___projekt
{
    public partial class PanelAdmina : Window
    {
        private ObservableCollection<Produkt> ListaProduktow = null;
        public PanelAdmina()
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
                    nowyPrzycisk.Content = "M"+id;
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!Helpers.CzyOknoOtwarte<Window>("dp"))
            {
                DodajPrzedmiot dp = new DodajPrzedmiot();
                dp.Show();
            }
            else
            {
                MessageBox.Show("Dodajesz już nowy rekord!");
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!Helpers.CzyOknoOtwarte<Window>("up"))
            {
                UsunPrzedmiot up = new UsunPrzedmiot();
                up.Show();
            }
            else
            {
                MessageBox.Show("Usuwasz już rekord!");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (!Helpers.CzyOknoOtwarte<Window>("ep"))
            {
                EdytujPrzedmiot ep = new EdytujPrzedmiot();
                ep.Show();
            }
            else
            {
                MessageBox.Show("Edytujesz już rekord!");
            }
        }
        private void przygotujWiazanie()
        {
            ListaProduktow = new ObservableCollection<Produkt>();
            lstProdukty.ItemsSource = ListaProduktow;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstProdukty.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Magazyn", ListSortDirection.Ascending));

            view.Filter = FiltrUzytkownika; // Wywołujemy filtrowanie
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
        private void WczytajDaneZBazy() // czytanie danych z bazy
        {
            string connectionString = $"Data Source=magazyn.db;Version=3;";// okreslamy zrodlo danych

            using (SQLiteConnection polaczenie = new SQLiteConnection(connectionString))
            { // tworzymy polaczenie
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
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (!Helpers.CzyOknoOtwarte<Window>("dm"))
            {
                DodawanieMagazynu dm = new DodawanieMagazynu();
                dm.Show();
            }
            else
            {
                MessageBox.Show("Dodajesz już nowy magazyn!");
            }
        }
    }
    }

