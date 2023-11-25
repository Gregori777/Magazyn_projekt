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

namespace Magazyn___projekt
{
    /// <summary>
    /// Logika interakcji dla klasy PanelAdmina.xaml
    /// </summary>
    public partial class PanelAdmina : Window
    {
        public PanelAdmina()
        {
            InitializeComponent();
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
    }
}
