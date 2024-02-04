using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;


namespace Kalkulator.Models
{
    public class Zarobki
    {
        public decimal PrzecietneWyn { get; set; } = 6935;
        public decimal OgraniczenieNettoMies = 10541.83m;
        [Range(0,double.MaxValue)]
        public decimal KwotaInit { get; set; }
        public decimal Kwota { get; set; }
        public string Typ { get; set; }
        public bool Pow26 { get; set; }
        public bool Miejsce { get; set; }
        
       
        [Range(0,double.MaxValue)]
        public decimal Darowizny { get; set; } = 0;

        public void BruttoNetto()
        {
            if (Typ == "brutto")
            {
                Kwota = KwotaInit;
            }
            else
            {
                decimal KwotaNetto = KwotaInit;
                decimal StalaEmRen = (PrzecietneWyn * 30 / 12) * 0.1045m;

                if (Pow26)
                {
                    if (KwotaNetto <= 27300 / 12 + 0.91m / 12 * Koszty())
                    {
                        Kwota = KwotaNetto / (0.7979m * 0.91m);
                    }
                    else if (KwotaNetto > 27300 / 12 + 0.91m / 12 * Koszty() / 12 
                            && KwotaNetto <= 98400 / 12 + 0.91m / 12 * Koszty() / 12)
                    {
                        Kwota = (KwotaNetto - Koszty() * 0.12m - 300) / (0.7979m * 0.79m);
                    }
                    else if (KwotaNetto > 98400 / 12 + 0.91m / 12 * Koszty() / 12
                            && KwotaNetto <= OgraniczenieNettoMies)
                    {
                        Kwota = (KwotaNetto - 2300 - 0.32m * Koszty()) / (0.7979m * 0.59m);
                    }
                    else if (KwotaNetto > OgraniczenieNettoMies 
                            && KwotaNetto <= Math.Round(617600 / 12m + 0.91m * Koszty(), 2))
                    {
                        Kwota = (KwotaNetto - 2300 + 0.59m * StalaEmRen - 0.32m * Koszty()) / (0.9024m * 0.59m);
                    }
                    else
                    {
                        Kwota = (KwotaNetto - Math.Round(67600 / 12m, 2) - 0.36m * Koszty() + 0.55m * StalaEmRen) 
                                / (0.9024m * 0.55m);
                    }
                }
                else
                {
                    if (KwotaNetto <= 12588.57m)
                    {
                        Kwota = KwotaNetto / 0.91m / 0.7979m;
                    }
                    else
                    {
                        Kwota = ((KwotaNetto / 0.91m) + StalaEmRen) / 0.9024m;
                    }
                }
            }
        }
        public decimal Koszty ()
        {
            if (Miejsce == true)
            { 
                 return 250;
            }
            else { return 300; }
        }

        public decimal Chorobowe { get => Kwota * 0.0976m; } 
        public decimal Emerytalne { get => (Kwota > PrzecietneWyn * 30 / 12) ? PrzecietneWyn * 30 / 12 * 0.08m : Kwota * 0.08m;}
        public decimal Rentowe { get => (Kwota > PrzecietneWyn * 30 / 12) ? PrzecietneWyn * 30 / 12 * 0.0245m : Kwota * 0.0245m; }
        
        public decimal PoZUS { get => Kwota - Chorobowe - Emerytalne - Rentowe; }
        public decimal Podstawa { get => (PoZUS >= Koszty()) ? PoZUS - Koszty() : 0; }
        
        public decimal Zdrowotne { get => PoZUS * 0.09m; }

       public decimal PodatekDochodowy()
        {
            if (Pow26)
            {
                if (Podstawa * 12 < 30000)
                {
                    return 0;
                }
                else if (Podstawa * 12 > 30000 && Podstawa * 12 < 120000)
                {
                    return Math.Round((Podstawa - 30000 / 12) * 0.12m, 0);
                }
                else if (Podstawa * 12 > 120000 && Podstawa * 12 < 1000000)
                {
                    return Math.Round((Podstawa - 120000 / 12) * 0.32m + 10800 / 12, 0);
                }
                else
                {
                    return Math.Round((Podstawa - Math.Round(1000000 / 12m, 2)) * 0.36m + Math.Round(2281600 / 12m, 2) + 10800, 0);
                }
            }
            else
            {   return 0; }
           
        }
        public decimal Darowizna()
        {
            if (PodatekDochodowy() != 0)
            {
                if (Darowizny <= Podstawa * 12 * 0.06m)
                {
                    return PodatekDochodowy() - Darowizny / 12;
                }
                else { return PodatekDochodowy() - Podstawa * 0.06m; }
            }
            else { return PodatekDochodowy(); }
        }

        public decimal Result()
        {
            if (Typ == "brutto")
            {
                return Kwota - Rentowe - Chorobowe - Emerytalne - Zdrowotne - Darowizna();
            }
            else
            {
                return Kwota;
            }
        }

        public decimal NettoChart { get; set; }
        public decimal EmChart { get; set; }
        public decimal RenChart { get; set; }
        public decimal ChorChart { get; set; }
        public decimal ZdrowChart { get; set; }
        public decimal PodChart { get; set; }


        public void ChartInfo()
        {
            if (KwotaInit!=0)
            {
                if (Typ == "brutto")
                {
                    NettoChart = Result() / KwotaInit * 100;
                    EmChart = Emerytalne / KwotaInit * 100;
                    RenChart = Rentowe / KwotaInit * 100;
                    ChorChart = Chorobowe / KwotaInit * 100;
                    ZdrowChart = Zdrowotne / KwotaInit * 100;
                    PodChart = Darowizna() / KwotaInit * 100;
                }

                else
                {
                    NettoChart = KwotaInit / Result() * 100;
                    EmChart = Emerytalne / Result() * 100;
                    RenChart = Rentowe / Result() * 100;
                    ChorChart = Chorobowe / Result() * 100;
                    ZdrowChart = Zdrowotne / Result() * 100;
                    PodChart = Darowizna() / Result() * 100;
                }
            }
            else
            {
                NettoChart = 0;
                EmChart = 0;
                RenChart = 0;
                ChorChart = 0;
                ZdrowChart = 0;
                PodChart = 0;
            }
            
        }
    }          
}
