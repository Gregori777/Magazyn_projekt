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
        }

        private void przygotujWiazanie()
        {
            ListaProduktow = new ObservableCollection<Produkt>();
            ListaProduktow.Add(new Produkt("Procesor", "AM376D", "Ryzen 5 2600", 143, 232.00));
            lstProdukty.ItemsSource = ListaProduktow;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstProdukty.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Magazyn", ListSortDirection.Ascending));

            view.Filter = FiltrUzytkownika;
        }

        private bool FiltrUzytkownika(object item)
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
