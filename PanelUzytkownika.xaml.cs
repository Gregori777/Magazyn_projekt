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
        
        public PanelUzytkownika()
        {
            InitializeComponent();
            przygotujWiazanie();
            BazaDanych.WczytajDaneZBazy();
        }

        private void przygotujWiazanie() 
        {
            BazaDanych.ListaProduktow = new ObservableCollection<Produkt>();
            lstProdukty.ItemsSource = BazaDanych.ListaProduktow;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(BazaDanych.ListaProduktow);
            view.SortDescriptions.Add(new SortDescription("Cena", ListSortDirection.Ascending));

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



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
