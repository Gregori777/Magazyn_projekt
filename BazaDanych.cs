using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn___projekt
{
    public class BazaDanych
    {
        public static void WczytajDaneZBazy() // czytanie danych z bazy
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



    }
}
