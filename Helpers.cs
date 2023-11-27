using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Magazyn___projekt
{
    internal class Helpers
    {
        public static int id_mag;
        public static bool CzyOknoOtwarte<T>(string nazwa = "") where T : Window
        {
            return string.IsNullOrEmpty(nazwa)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(nazwa));
        }
    }
}
