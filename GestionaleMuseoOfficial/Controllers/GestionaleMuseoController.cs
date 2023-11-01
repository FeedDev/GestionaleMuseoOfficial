using GestionaleMuseoOfficial.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GestionaleMuseo.Controllers
{
    [Authorize]
    public class GestionaleMuseoController : Controller
    {
        ModelDbContext dbContext = new ModelDbContext();
        // GET: GestionaleMuseo
        public ActionResult Index()
        {
            return RedirectToAction("Login");
            //return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(FormsAuthentication.DefaultUrl); // Reindirizza l'utente alla home se è già autenticato
            }

            return View(new Login());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var dbContext = new ModelDbContext())
                    {
                        Login user = dbContext.Login.Where(u => u.Username == model.Username && u.Password == model.Password).FirstOrDefault();

                        if (user != null)
                        {
                            FormsAuthentication.SetAuthCookie(model.Username, false);

                            //HttpCookie cookiePropID = new HttpCookie("USER_COOKIE");
                            //cookiePropID.Values["ID"] = user.IdLogin.ToString();
                            //Response.Cookies.Add(cookiePropID);

                            TempData["Successo"] = $"Benvenuto {model.Username}";

                            return Redirect(FormsAuthentication.DefaultUrl);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Username o password errate";
                        }
                    }
                }
                catch
                {
                    ViewBag.ErrorMessage = "Errore durante l'autenticazione";
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return Redirect(FormsAuthentication.LoginUrl);
        }


        //--------------CLIENTI------------//

        [HttpGet]
        public ActionResult ViewListaClienti()
        {
            return View(dbContext.Clienti.ToList());
        }
        [HttpGet]
        public ActionResult CreateCliente()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCliente(Clienti cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbContext.Clienti.Add(cliente); // Aggiungi il nuovo cliente al contesto del database
                    dbContext.SaveChanges(); // Salva le modifiche nel database

                    TempData["Successo"] = "Cliente creato con successo!";
                    return RedirectToAction("ViewListaClienti"); //Reindirizza alla pagina di visualizzazione dei clienti 
                }
            }
            catch (Exception ex)
            {
                TempData["Errore"] = "Si è verificato un errore durante la creazione del cliente: " + ex.Message;
            }

            return View(cliente); // Se si verifica un errore, torna alla vista del form di creazione del cliente
        }

        [HttpGet]
        public ActionResult EditCliente(int id)
        {
            return View(dbContext.Clienti.Find(id));
        }

        [HttpPost]
        public ActionResult EditCliente(Clienti c)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    dbContext.Entry(c).State = System.Data.Entity.EntityState.Modified;
                    dbContext.SaveChanges();
                    return RedirectToAction("ViewListaClienti");
                }
                else
                {
                    ViewBag.Errore = "Errore nella compilazione dei dati";
                    return View(c);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Errore = ex.Message;
                return View(c);
            }
        }


        [HttpGet]
        public ActionResult DeleteCliente(int id)
        {
            return View(dbContext.Clienti.Find(id));
        }

        [HttpPost]
        [ActionName("DeleteCliente")]
        public ActionResult ConfirmDeleteCliente(int id)
        {
            try
            {
                dbContext.Clienti.Remove(dbContext.Clienti.Find(id));
                dbContext.SaveChanges();
                return RedirectToAction("ViewListaClienti"); //Reindirizza alla pagina di visualizzazione dei clienti 
            }
            catch (Exception ex)
            {
                ViewBag.Errore = ex.Message;
                return View();
            }
        }


        //--------------VISITE------------//

        [HttpGet]
        public ActionResult ViewListaVisite()
        {
            return View(dbContext.Visite.ToList());
        }
        [HttpGet]
        public ActionResult CreateVisita()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateVisita(Visite v)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (v.Image != null)
                    {
                        if (v.Image.ContentLength > 0)
                        {
                            string fileName = v.Image.FileName; //image è il nuovo parametro aggiunto nella classe visita sotto ImmagineLocandina
                            string path = Server.MapPath("~/Content/image"); //image è il nome della cartella

                            v.Image.SaveAs($"{path}/{fileName}");
                        }

                    }
                    v.ImmagineLocandina = v.Image.FileName;
                    //v.DataInizio = v.DataInizio.ToShortDateString();
                    dbContext.Visite.Add(v); // Aggiungi la nuova visita al contesto del database
                    dbContext.SaveChanges(); // Salva le modifiche nel database

                    TempData["Successo"] = "Visita creata con successo!";
                    return RedirectToAction("ViewListaVisite"); // Reindirizza alla pagina di visualizzazione delle visite
                }
            }
            catch (Exception ex)
            {
                TempData["Errore"] = "Si è verificato un errore durante la creazione della visita: " + ex.Message;
            }

            return View(v); // Se si verifica un errore, torna alla vista del form di creazione della visita
        }

        [HttpGet]
        public ActionResult EditVisita(int id)
        {
            Visite visita = dbContext.Visite.Find(id);
            return View(visita);
            //return View(dbContext.Clienti.Find(id));
        }

        [HttpPost]
        public ActionResult EditVisita(Visite v)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (v.Image != null)
                    {
                        if (v.Image.ContentLength > 0)
                        {
                            string fileName = v.Image.FileName; //image è il nuovo parametro aggiunto nella classe visita sotto ImmagineLocandina
                            string path = Server.MapPath("~/Content/image"); //image è il nome della cartella

                            v.Image.SaveAs($"{path}/{fileName}");
                        }
                        v.ImmagineLocandina = v.Image.FileName;
                    }
                    else
                    {
                        int id = v.IdVisita;
                        string img = dbContext.Visite.Find(id).ImmagineLocandina;
                        v.ImmagineLocandina = img;
                    }

                    ModelDbContext dbContext2 = new ModelDbContext();
                    dbContext2.Entry(v).State = System.Data.Entity.EntityState.Modified;
                    dbContext2.SaveChanges();
                    return RedirectToAction("ViewListaVisite");
                }
                else
                {
                    ViewBag.Errore = "Errore nella compilazione dei dati";
                    return View(v);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Errore = ex.Message;
                return View(v);
            }
        }

        [HttpGet]
        public ActionResult DeleteVisita(int id)
        {
            return View(dbContext.Visite.Find(id));
        }

        [HttpPost]
        [ActionName("DeleteVisita")]
        public ActionResult ConfirmDeleteVisita(int id)
        {
            try
            {
                dbContext.Visite.Remove(dbContext.Visite.Find(id));
                dbContext.SaveChanges();
                return RedirectToAction("ViewListaVisite");
            }
            catch (Exception ex)
            {
                ViewBag.Errore = ex.Message;
                return View();
            }
        }


        //-----------CATEGORIA VISITATORI---------//

        [HttpGet]
        public ActionResult ViewListaCategoriaVisitatori()
        {
            return View(dbContext.CategorieVisitatori.ToList());
        }
        [HttpGet]
        public ActionResult CreateCategoriaVisitatore()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategoriaVisitatore(CategorieVisitatori cv)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbContext.CategorieVisitatori.Add(cv); // Aggiungi la nuova categoria visitatore al contesto del database
                    dbContext.SaveChanges(); // Salva le modifiche nel database

                    TempData["Successo"] = "Categoria creata con successo!";
                    return RedirectToAction("ViewListaCategoriaVisitatori"); 
                }
            }
            catch (Exception ex)
            {
                TempData["Errore"] = "Si è verificato un errore durante la creazione della categoria: " + ex.Message;
            }

            return View(cv); // Se si verifica un errore, torna alla vista del form di creazione della visita
        }

        [HttpGet]
        public ActionResult EditCategoriaVisitatore(int id)
        {
            return View(dbContext.CategorieVisitatori.Find(id));
        }

        [HttpPost]
        public ActionResult EditCategoriaVisitatore(CategorieVisitatori cv)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    dbContext.Entry(cv).State = System.Data.Entity.EntityState.Modified;
                    dbContext.SaveChanges();
                    return RedirectToAction("ViewListaCategoriaVisitatori");
                }
                else
                {
                    ViewBag.Errore = "Errore nella compilazione dei dati";
                    return View(cv);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Errore = ex.Message;
                return View(cv);
            }
        }

        [HttpGet]
        public ActionResult DeleteCategoriaVisitatore(int id)
        {
            return View(dbContext.CategorieVisitatori.Find(id));
        }

        [HttpPost]
        [ActionName("DeleteCategoriaVisitatore")]
        public ActionResult ConfirmDeleteCategoriaVisitatore(int id)
        {
            try
            {
                dbContext.CategorieVisitatori.Remove(dbContext.CategorieVisitatori.Find(id));
                dbContext.SaveChanges();
                return RedirectToAction("ViewListaCategoriaVisitatori");
            }
            catch (Exception ex)
            {
                ViewBag.Errore = ex.Message;
                return View();
            }
        }


        //--------------BIGLIETTI--------------//

        [HttpGet]
        public ActionResult ViewListaBiglietti()
        {
            ViewBag.Visite = dbContext.Visite.ToList();
            return View("ViewListaBiglietti", dbContext.Biglietti.ToList());
        }

        [HttpGet]
        public ActionResult CreateBiglietto()
        {
            ViewBag.IdVisita = new SelectList(dbContext.Visite, "IdVisita", "Titolo");
            ViewBag.IdCategorieVisitatori = new SelectList(dbContext.CategorieVisitatori, "IdCategorieVisitatori", "Descrizione");

            var clienti = dbContext.Clienti.Select(c => new
            {
                IdCliente = c.IdCliente,
                NomeCognome = c.Nome + " " + c.Cognome // Unisce il nome e il cognome del cliente per utilizzarlo nella DropDownList
            }).ToList();

            ViewBag.IdCliente = new SelectList(clienti, "IdCliente", "NomeCognome");

            return View();
        }

        [HttpPost]
        public ActionResult CreateBiglietto(Biglietti b)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int numeroBigliettiVenduti = dbContext.Biglietti.Where(m=>m.IdVisita == b.IdVisita).Count();

                    Visite visita = dbContext.Visite.FirstOrDefault(v => v.IdVisita == b.IdVisita);

                    if (numeroBigliettiVenduti < visita.NumeroMaxBiglietti)
                    {
                        decimal prezzoOrdinario = visita.TariffaOrdinaria;
                        decimal prezzoScontato = CalcolaScontoCategoria(b.IdCategorieVisitatori, prezzoOrdinario);
                        b.PrezzoTotale = prezzoScontato;

                        dbContext.Biglietti.Add(b);
                        dbContext.SaveChanges();

                        TempData["Successo"] = "Biglietto creato con successo!";
                        return RedirectToAction("ViewListaBiglietti");
                    }
                    else
                    {
                        TempData["Errore"] = "Numero massimo di biglietti venduti raggiunto per questa visita.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Errore"] = "Si è verificato un errore durante la creazione del biglietto: " + ex.Message;
            }

            ViewBag.IdVisita = new SelectList(dbContext.Visite, "IdVisita", "Titolo", b.IdVisita);
            ViewBag.IdCategorieVisitatori = new SelectList(dbContext.CategorieVisitatori, "IdCategorieVisitatori", "Descrizione", b.IdCategorieVisitatori);

            var clienti = dbContext.Clienti.Select(c => new
            {
                IdCliente = c.IdCliente,
                NomeCognome = c.Nome + " " + c.Cognome // Combina il nome e il cognome del cliente
            }).ToList();

            ViewBag.IdCliente = new SelectList(clienti, "IdCliente", "NomeCognome", b.IdCliente);

            return View(b);
        }


        //[HttpPost]
        //public ActionResult CreateBiglietto(Biglietti b)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            int numeroBigliettiVenduti = dbContext.Biglietti.Count(biglietto => b.IdVisita == b.IdVisita);

        //            Visite visita = dbContext.Visite.FirstOrDefault(v => v.IdVisita == b.IdVisita);

        //            if (numeroBigliettiVenduti < visita.NumeroMaxBiglietti)
        //            {

        //                decimal prezzoOrdinario = visita.TariffaOrdinaria;
        //                decimal prezzoScontato = CalcolaScontoCategoria(b.IdCategorieVisitatori, prezzoOrdinario);
        //                b.PrezzoTotale = prezzoScontato;

        //                dbContext.Biglietti.Add(b);
        //                dbContext.SaveChanges();

        //                TempData["Successo"] = "Biglietto creato con successo!";

        //                return RedirectToAction("ViewListaBiglietti");                       
        //            }
        //            else
        //            {
        //                TempData["Errore"] = "Numero massimo di biglietti venduti raggiunto per questa visita.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Errore"] = "Si è verificato un errore durante la creazione del biglietto: " + ex.Message;
        //    }

        //    ViewBag.IdVisita = new SelectList(dbContext.Visite, "IdVisita", "Titolo", b.IdVisita);
        //    ViewBag.IdCategorieVisitatori = new SelectList(dbContext.CategorieVisitatori, "IdCategorieVisitatori", "Descrizione", b.IdCategorieVisitatori);

        //    var clienti = dbContext.Clienti.Select(c => new
        //    {
        //        IdCliente = c.IdCliente,
        //        NomeCognome = c.Nome + " " + c.Cognome // Combina il nome e il cognome del cliente
        //    }).ToList();

        //    ViewBag.IdCliente = new SelectList(clienti, "IdCliente", "NomeCognome", b.IdCliente);

        //    return View(b);
        //}

        [HttpGet]
        public JsonResult GetTariffaOrdinaria(int id)
        {
            decimal tariffa = 0;
            List<Visite> listVisite = dbContext.Visite.ToList();
            foreach (Visite visite in listVisite)
            {
                if(visite.IdVisita == id)
                {
                    tariffa = visite.TariffaOrdinaria;
                    break;
                }
            }
            // Restituisce la tariffa ordinaria come risultato JSON
            return Json(tariffa, JsonRequestBehavior.AllowGet);
        }

        private decimal CalcolaScontoCategoria(int idCategoria, decimal prezzoOrdinario)
        {
            decimal prezzoScontato = prezzoOrdinario;

            CategorieVisitatori categoria = dbContext.CategorieVisitatori.FirstOrDefault(c => c.IdCategorieVisitatori == idCategoria);

            if (categoria != null)
            {
                prezzoScontato = prezzoOrdinario - (prezzoOrdinario * categoria.PercentualeSconto / 100);
            }

            return prezzoScontato;
        }

        //FUNZIONALE MA SENZA PREZZO ORDINARIO GIA' ASSEGNATO

        //[HttpGet]
        //public ActionResult CreateBiglietto()
        //{
        //    ViewBag.IdVisita = new SelectList(dbContext.Visite, "IdVisita", "Titolo");
        //    ViewBag.IdCategorieVisitatori = new SelectList(dbContext.CategorieVisitatori, "IdCategorieVisitatori", "Descrizione");

        //    var clienti = dbContext.Clienti.Select(c => new
        //    {
        //        IdCliente = c.IdCliente,
        //        NomeCognome = c.Nome + " " + c.Cognome // Unisce il nome e il cognome del cliente per utilzzarlo nella DropDown
        //    }).ToList();

        //    ViewBag.IdCliente = new SelectList(clienti, "IdCliente", "NomeCognome");

        //    return View();
        //}

        //[HttpPost]
        //public ActionResult CreateBiglietto(Biglietti b)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            int numeroBigliettiVenduti = dbContext.Biglietti.Count(biglietto => b.IdVisita == b.IdVisita);

        //            Visite visita = dbContext.Visite.FirstOrDefault(v => v.IdVisita == b.IdVisita);

        //            if (numeroBigliettiVenduti < visita.NumeroMaxBiglietti)
        //            {
        //                decimal prezzoOrdinario = visita.TariffaOrdinaria;
        //                decimal prezzoScontato = CalcolaScontoCategoria(b.IdCategorieVisitatori, prezzoOrdinario);
        //                b.PrezzoTotale = prezzoScontato;

        //                dbContext.Biglietti.Add(b);
        //                dbContext.SaveChanges();

        //                TempData["Successo"] = "Biglietto creato con successo!";
        //                return RedirectToAction("ViewListaBiglietti");
        //            }
        //            else
        //            {
        //                TempData["Errore"] = "Numero massimo di biglietti venduti raggiunto per questa visita.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Errore"] = "Si è verificato un errore durante la creazione del biglietto: " + ex.Message;
        //    }

        //    ViewBag.IdVisita = new SelectList(dbContext.Visite, "IdVisita", "Titolo", b.IdVisita);
        //    ViewBag.IdCategorieVisitatori = new SelectList(dbContext.CategorieVisitatori, "IdCategorieVisitatori", "Descrizione", b.IdCategorieVisitatori);

        //    var clienti = dbContext.Clienti.Select(c => new
        //    {
        //        IdCliente = c.IdCliente,
        //        NomeCognome = c.Nome + " " + c.Cognome // Combina il nome e il cognome del cliente
        //    }).ToList();

        //    ViewBag.IdCliente = new SelectList(clienti, "IdCliente", "NomeCognome", b.IdCliente);

        //    return View(b);
        //}

        //[HttpGet]
        //public ActionResult EditBiglietto(int id)
        //{
        //    return View(dbContext.Biglietti.Find(id));
        //}

        //[HttpPost]
        //public ActionResult EditBiglietto(Biglietti b)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {

        //            dbContext.Entry(b).State = System.Data.Entity.EntityState.Modified;
        //            dbContext.SaveChanges();
        //            return RedirectToAction("ViewListaBiglietti"); // RITORNA ALLA VIEW DELLA LISTA DEI BIGLIETTI
        //        }
        //        else
        //        {
        //            ViewBag.Errore = "Errore nella compilazione dei dati";
        //            return View(b);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Errore = ex.Message;
        //        return View(b);
        //    }
        //}

        [HttpGet]
        public ActionResult EditBiglietto(int id)
        {
            var biglietto = dbContext.Biglietti.Find(id);

            if (biglietto == null)
            {
                ViewBag.Errore = "Il biglietto non è stato trovato.";
                return View("Errore");
            }

            ViewBag.IdVisita = new SelectList(dbContext.Visite, "IdVisita", "Titolo", biglietto.IdVisita);
            ViewBag.IdCategorieVisitatori = new SelectList(dbContext.CategorieVisitatori, "IdCategorieVisitatori", "Descrizione", biglietto.IdCategorieVisitatori);
            ViewBag.IdCliente = new SelectList(dbContext.Clienti.Select(c => new { IdCliente = c.IdCliente, NomeCognome = c.Nome + " " + c.Cognome }), "IdCliente", "NomeCognome", biglietto.IdCliente);
            ViewBag.IdBiglietto = biglietto.DataValidita;
            return View(biglietto);
        }

        [HttpPost]
        public ActionResult EditBiglietto(Biglietti b)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbContext.Entry(b).State = System.Data.Entity.EntityState.Modified;
                    dbContext.SaveChanges();
                    return RedirectToAction("ViewListaBiglietti");
                }
                else
                {
                    ViewBag.Errore = "Errore nella compilazione dei dati";
                    ViewBag.IdVisita = new SelectList(dbContext.Visite, "IdVisita", "Titolo", b.IdVisita);
                    ViewBag.IdCategorieVisitatori = new SelectList(dbContext.CategorieVisitatori, "IdCategorieVisitatori", "Descrizione", b.IdCategorieVisitatori);
                    ViewBag.IdCliente = new SelectList(dbContext.Clienti.Select(c => new { IdCliente = c.IdCliente, NomeCognome = c.Nome + " " + c.Cognome }), "IdCliente", "NomeCognome", b.IdCliente);
                    return View(b);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Errore = ex.Message;
                return View(b);
            }
        }


        [HttpGet]
        public ActionResult DeleteBiglietto(int id)
        {
            return View(dbContext.Biglietti.Find(id));
        }

        [HttpPost]
        [ActionName("DeleteBiglietto")]
        public ActionResult ConfirmDeleteBiglietto(int id)
        {
            try
            {
                dbContext.Biglietti.Remove(dbContext.Biglietti.Find(id));
                dbContext.SaveChanges();
                return RedirectToAction("ViewListaBiglietti"); // RITORNA ALLA VIEW DELLA LISTA DEI BIGLIETTI
            }
            catch (Exception ex)
            {
                ViewBag.Errore = ex.Message;
                return View();
            }
        }




        //-------------------METODI_JSON-----------------------//

        [HttpGet]
        public JsonResult FiltroPerData(DateTime data)
        {
            try
            {
                var biglietti = dbContext.Biglietti
                    .Where(b => b.DataValidita == data)
                    .Select(b => b.IdBiglietto)
                    .ToList();

                return Json(biglietti, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Gestisci l'errore in base alle tue esigenze
                return Json(new { error = "Si è verificato un errore durante il filtro per data." });
            }
        }

        //[HttpGet]
        //public JsonResult ElencoBigliettiEmessi(DateTime data)
        //{
        //    try
        //    {
        //        var biglietti = dbContext.Biglietti.Where(b => b.DataValidita.Date == data.Date).Select(b => b.IdBiglietto).ToList();

        //        return Json(biglietti, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Gestisci l'errore in base alle tue esigenze
        //        return Json(new { error = "Si è verificato un errore durante l'elenco dei biglietti emessi." });
        //    }
        //}


        //public JsonResult NumeroBigliettiEmessi(int visitaEventoId)
        //{
        //    try
        //    {
        //        int numeroBiglietti = dbContext.Biglietti
        //            .Count(b => b.IdVisita == visitaEventoId);

        //        return Json(new { numeroBiglietti, visite = ViewBag.Visite }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { error = "Si è verificato un errore durante il calcolo del numero di biglietti emessi." });
        //        //return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpGet]
        public JsonResult GetNumeroBigliettiEmessi(int visitaEventoId)
        {
            try
            {
                int numeroBiglietti = dbContext.Biglietti
                    .Count(b => b.IdVisita == visitaEventoId);

                return Json(new { numeroBiglietti }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Si è verificato un errore durante il recupero del numero di biglietti emessi." });
            }
        }

        [HttpGet]
        public JsonResult CalcolaRicavato(int visitaEventoId)
        {
            try
            {
                decimal ricavato = dbContext.Biglietti
                    .Where(b => b.IdVisita == visitaEventoId)
                    .Sum(b => b.PrezzoTotale);

                return Json(ricavato, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Si è verificato un errore durante il calcolo del ricavato." });
                //return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
























        //[HttpPost]
        //public ActionResult CreateBiglietto(Biglietti b)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            dbContext.Biglietti.Add(b); // Aggiungi il nuovo biglietto al contesto del database
        //            dbContext.SaveChanges(); // Salva le modifiche nel database

        //            TempData["Successo"] = "Biglietto creato con successo!";
        //            return RedirectToAction("Index", "Biglietto"); //METTERE VIEW DELLA LISTA DEI BIGLIETTI// Reindirizza alla pagina di visualizzazione del biglietto
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Errore"] = "Si è verificato un errore durante la creazione della visita: " + ex.Message;
        //    }

        //    return View(b); // Se si verifica un errore, torna alla vista del form di creazione del biglietto
        //}

        //
        //[HttpPost]
        //public ActionResult CreateBiglietto(Biglietti b)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            int numeroBigliettiVenduti = dbContext.Biglietti.Count(biglietto => b.IdVisita == b.IdVisita);

        //            Visite visita = dbContext.Visite.FirstOrDefault(v => v.IdVisita == b.IdVisita);

        //            if (numeroBigliettiVenduti < visita.NumeroMaxBiglietti) // Controlla se il numero di biglietti venduti è inferiore al numero massimo consentito
        //            {
        //                dbContext.Biglietti.Add(b); // Aggiungi il nuovo biglietto al contesto del database
        //                dbContext.SaveChanges(); // Salva le modifiche nel database

        //                TempData["Successo"] = "Biglietto creato con successo!";
        //                return RedirectToAction("Index", "Biglietto"); //INSERIRE VIEW LISTA BIGLIETTI // Reindirizza alla pagina di visualizzazione dei biglietti
        //            }
        //            else
        //            {
        //                TempData["Errore"] = "Numero massimo di biglietti venduti raggiunto per questa visita.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Errore"] = "Si è verificato un errore durante la creazione del biglietto: " + ex.Message;
        //    }

        //    return View(b); // Se si verifica un errore o il numero di biglietti venduti supera il limite, torna alla vista del form di creazione del biglietto
        //}








    }

}