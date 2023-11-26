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
        private ObservableCollection<Produkt> ListaProduktow = null;
        public PanelUzytkownika()
        {
            InitializeComponent();
            przygotujWiazanie();
            WczytajDaneZBazy();
        }

        private void przygotujWiazanie() 
        {
            ListaProduktow = new ObservableCollection<Produkt>();
            lstProdukty.ItemsSource = ListaProduktow;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstProdukty.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Magazyn", ListSortDirection.Ascending));

            view.Filter = FiltrUzytkownika; // Wywołujemy filtrowanie
        }

        private void WczytajDaneZBazy() // czytanie danych z bazy
        {
            string connectionString = "Data Source=magazyn.db;Version=3;";// okreslamy zrodlo danych

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
