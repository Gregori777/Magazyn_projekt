using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn___projekt
{
    public class Produkt
    {
        public string TypProduktu { get; set; }
        public string Kod { get; set; }
        public string Nazwa { get; set; }
        public int LiczbaSztuk { get; set; }
        public double CenaProduktu { get; set; }

        public Produkt(string typ, string kod, string naz, int lszt, double cena)
        {
            TypProduktu = typ;
            Kod = kod;
            Nazwa = naz;
            LiczbaSztuk = lszt;
            CenaProduktu = cena;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3} {4}", TypProduktu, Kod, Nazwa, LiczbaSztuk, CenaProduktu);
        }
    }
}
