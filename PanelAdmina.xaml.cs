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

namespace Magazyn___projekt
{
    /// <summary>
    /// Logika interakcji dla klasy PanelAdmina.xaml
    /// </summary>
    public partial class PanelAdmina : Window
    {
        private string NazwaPLikuDb;
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

        private void ImportBazyDanych(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Pliki bazy danych (*.db)|*.db|Wszystkie pliki (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                string sciezkaPLiku = ofd.FileName;

                try
                {
                    string sciezkaProjektu = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    NazwaPLikuDb = System.IO.Path.GetFileNameWithoutExtension(NazwaPLikuDb);
                    string sciezkaDocelowa = System.IO.Path.Combine(sciezkaProjektu, System.IO.Path.GetFileName(sciezkaPLiku));
                    if (File.Exists(sciezkaDocelowa))
                    {
                        // Usuń stary plik
                        File.Delete(sciezkaDocelowa);
                    }

                    File.Copy(sciezkaPLiku, sciezkaDocelowa, true);

                    MessageBox.Show("Baza danych została pomyślnie zaimportowana do folderu projektu.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    MessageBox.Show($"Plik bazy danych znajduje się pod ścieżką: {sciezkaDocelowa}", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas importowania bazy danych: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
