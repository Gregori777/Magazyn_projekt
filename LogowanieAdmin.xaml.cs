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
    /// Logika interakcji dla klasy LogowanieAdmin.xaml
    /// </summary>
    public partial class LogowanieAdmin : Window
    {

        public LogowanieAdmin()
        {
            InitializeComponent();
        }

        private void Zaloguj(object sender, RoutedEventArgs e)
        {
            string log = login.Text;
            string pass = haslo.Text;
            if (log == "admin" && pass == "password")
            {
                PanelAdmina pa = new PanelAdmina();
                pa.Show();
                this.Close();
            }
            else
            {
                komunikat.Text = "Błędne hasło lub login!";
            }
        }
    }
}
