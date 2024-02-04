using Kalkulator.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace Kalkulator.Controllers
{
    public class ZarobkiController : Controller
    {
        public IActionResult Index(Zarobki z)
        {
            //z.Ograniczenie();
            z.BruttoNetto();

            return View(z);
        }
        [HttpPost]
        public IActionResult Wyniki(IFormCollection collection)
        {
            //Console.WriteLine(collection.ToJson());
            try
            {
                Zarobki z = new Zarobki();
                z.KwotaInit = Convert.ToDecimal(collection["KwotaInit"]);
                if (collection["Miejsce"][0] == "True")
                {
                    z.Miejsce = true;
                }
                else
                {
                    z.Miejsce = false;
                }
                if (collection["Pow26"][0] == "True")
                {
                    z.Pow26 = true;
                }
                else
                {
                    z.Pow26 = false;
                }
                z.Typ = Convert.ToString(collection["Typ"]);
                z.Darowizny = Convert.ToDecimal(collection["Darowizny"]);
                //z.Emerytalne = Convert.ToDecimal(collection["Emerytalne"]);
                z.BruttoNetto();
                //z.Ograniczenie();
                z.ChartInfo();
                
                ViewData["KwotaInit"] = z.KwotaInit;
                ViewData["Miejsce"] = z.Miejsce;
                ViewData["Pow26"] = z.Pow26;
                ViewData["Typ"] = z.Typ;
                ViewData["Darowizny"] = z.Darowizny;
                ViewData["Chorobowe"] = z.Chorobowe;
                ViewData["Emerytalne"] = z.Emerytalne;
                ViewData["Rentowe"] = z.Rentowe;
                ViewData["PoZUS"] = z.PoZUS;
                ViewData["Zdrowotne"] = z.Zdrowotne;
                ViewData["Koszty"] = z.Koszty();
                ViewData["PodatekDochodowy"] = z.Darowizna();
                ViewData["Result"] = z.Result();

                ViewData["NettoChart"] = z.NettoChart;
                ViewData["EmChart"] = z.EmChart;
                ViewData["RenChart"] = z.RenChart;
                ViewData["ChorChart"] = z.ChorChart;
                ViewData["ZdrowChart"] = z.ZdrowChart;
                ViewData["PodChart"] = z.PodChart;
                //ViewData["Emerytalne"] = z.Emerytalne;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("Wyniki");
        }
           
            
        
    }
}
