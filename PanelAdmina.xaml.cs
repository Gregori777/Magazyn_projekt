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
        private ICollectionView collectionView;
        public PanelAdmina()
        {
            InitializeComponent();
            przygotujWiazanie();
            WczytajDaneZBazy();
        }

        private void odswiez(object sender, RoutedEventArgs e)
        {
            ListaProduktow.Clear();
            WczytajDaneZBazy();
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

            collectionView = CollectionViewSource.GetDefaultView(ListaProduktow);
            collectionView.SortDescriptions.Add(new SortDescription("TypProduktu", ListSortDirection.Ascending));

        }
 
        private void WczytajDaneZBazy() // czytanie danych z bazy
        {
                string connectionString = $"Data Source=magazyn.db;Version=3;";// okreslamy zrodlo danych

                using SQLiteConnection polaczenie = new SQLiteConnection(connectionString);// tworzymy polaczenie
                polaczenie.Open();// otwieramy polaczenie z baza
                string zapytanie = "SELECT typProduktu, kodProduktu, nazwaProduktu, iloscProduktu, cenaProduktu FROM produkty";// nasze zapytanie

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

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (!Helpers.CzyOknoOtwarte<Window>("ep"))
            {
                DodawanieMagazynu dm = new DodawanieMagazynu();
                dm.Show();
            }
            else
            {
                MessageBox.Show("Dodajesz już nowy magazyn!");
            }
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

