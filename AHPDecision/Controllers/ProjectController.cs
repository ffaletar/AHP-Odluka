using AHPDecision.Helpers;
using AHPDecision.Models;
using AHPDecision.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace AHPDecision.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult Index(int id)
        {
            AHPEntities4 db = new AHPEntities4();
            CustomAuthorization customAuthorization = new CustomAuthorization();
            bool userAuthorized = customAuthorization.AuthorizeUserProject(User.Identity.GetUserId(), id);
            if (userAuthorized == true)
            {
                Projekt project = db.Projekts.Where(x => x.id == id).SingleOrDefault();
                List<Alternativa> alternatives = db.Alternativas.Where(x => ( x.projekt == id && x.obrisana != true)).ToList();
                List<Kriterij> criteria = db.Kriterijs.Where(x => (x.projekt == id && x.Kriterij2 == null && x.obrisan != true)).ToList();
                List<Dnevnik> logs = db.Dnevniks.Where(x => x.projekt == id).OrderByDescending(y => y.vrijeme).ToList();

                ProjectViewModel projektViewModel = new ProjectViewModel(project, criteria, alternatives, logs);
                return View(projektViewModel);
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }

        public ActionResult Result(int id)
        {
            EvaluationController.CalculateResults(id);

            AHPEntities4 db = new AHPEntities4();
            List<Alternativa> alternative = db.Alternativas.Where(x => (x.projekt == id && x.obrisana != true)).OrderByDescending(y => y.vrijednost).ToList();

            return View(alternative);
        }

        [HttpPost]
        public ActionResult AddProject(string naziv, string opis)
        {
            DateTime date = DateTime.Now;
            var idKorisnika = User.Identity.GetUserId();

            AHPEntities4 db = new AHPEntities4();
            Projekt projekt = new Projekt { naziv = naziv, opis = opis, korisnik = idKorisnika, datum = date, zadnjaPromjena = date, konzistentno = false, fazaProjekta = 3 };

            var projekti = db.Set<Projekt>();
            projekti.Add(projekt);

            if (db.SaveChanges() != 0)
            {
                int idProjekta = projekt.id;
                return Redirect(Url.Action("Index", "Project", new { id = idProjekta }));
            }else
            {
                return null;
            }

        }

        public ActionResult DeleteProject(int id)
        {
            AHPEntities4 db = new AHPEntities4();
            Projekt projekt = db.Projekts.Where(x => x.id == id).SingleOrDefault();
            projekt.obrisan = true;

            if (db.SaveChanges() != 0)
            {
                return Redirect(Url.Action("Index", "Home"));
            }
            else
            {
                return null;
            }

        }


        [HttpPost]
        public ActionResult AddAlternative(int projectId, string name, string info)
        {
            AHPEntities4 db = new AHPEntities4();
            Alternativa alternativa = new Alternativa { naziv = name, opis = info, projekt = projectId, obrisana = false };
            var alternative = db.Set<Alternativa>();
            alternative.Add(alternativa);
            Projekt projekt = db.Projekts.SingleOrDefault(b => b.id == projectId);

            if (db.SaveChanges() != 0)
            {
                DateTime trenutnoVrijeme = DateTime.Now;
                

                List<Kriterij> kriteriji = db.Kriterijs.Where(x => (x.projekt == projectId && x.obrisan != true && x.Kriterij1.Count == 0)).ToList();
                foreach (Kriterij kriterij in kriteriji)
                {
                    AddOrChangeAlternativesValuesForCriteria(alternativa, kriterij, null);
                }

                //UpdateAlternativesComparison(projectId);

                var dnevnik = db.Set<Dnevnik>();
                dnevnik.Add(new Dnevnik { tipZapisa = 4, vrijeme = trenutnoVrijeme, dodatneInformacije = name, korisnik = projekt.korisnik, status = 1, projekt = projectId });

                UpdateProjectsAlternativesConsistency(projectId);

                db.SaveChanges();

                return Redirect(Url.Action("Index", "Project", new { id = projectId }));
            }

            return Redirect(Url.Action("Index", "Project", new { id = projectId }));
        }

        [HttpPost]
        public ActionResult AddCriteria(int projectId, int? parentId, string name, string info)
        {
            AHPEntities4 db = new AHPEntities4();
            Kriterij kriterij = new Kriterij { naziv = name, opis = info, projekt = projectId, idRoditelja = parentId, konzistentno = true, obrisan = false };
            var kriteriji = db.Set<Kriterij>();
            kriteriji.Add(kriterij);

            if (db.SaveChanges() != 0)
            {

                List<Alternativa> alternative = db.Alternativas.Where(x => (x.projekt == projectId && x.obrisana != true)).ToList();
                foreach(Alternativa alternativa in alternative)
                {
                    AddOrChangeAlternativesValuesForCriteria(alternativa, kriterij, null);
                }
                
                Projekt projekt = db.Projekts.Where(x => x.id == projectId).SingleOrDefault();
                projekt.konzistentno = false;

                if (parentId != null)
                {
                    Kriterij roditelj = db.Kriterijs.Where(x => x.id == parentId).SingleOrDefault();
                    roditelj.konzistentno = false;
                }

                db.SaveChanges();

                DBOperationsHelper.AddNewCriteriaComparisons(projectId);
                UpdateProjectsCriteriaConsistency(projectId);
                UpdateProjectsAlternativesConsistency(projectId);


                return Redirect(Url.Action("Index", "Project", new { id = projectId }));
            }

            return Redirect(Url.Action("Index", "Project", new { id = projectId }));
        }

        [HttpPost]
        public ActionResult DeleteCriteria(int id)
        {
            AHPEntities4 db = new AHPEntities4();
            Kriterij kriterij = db.Kriterijs.Where(x => x.id == id).SingleOrDefault();
            kriterij.obrisan = true;

            if (db.SaveChanges() != 0)
            {
                UpdateProjectsCriteriaConsistency(id);
                UpdateProjectsAlternativesConsistency(id);
                return Redirect(Url.Action("Index", "Project", new { id = kriterij.projekt}));
            }
            else
            {
                return null;
            }
        }

        public void AddOrChangeAlternativesValuesForCriteria(Alternativa alternativa, Kriterij kriterij, string stvarnaVrijednost)
        {
            AHPEntities4 db = new AHPEntities4();
            if(alternativa != null && kriterij != null)
            {
                AlternativaKriterij alternativaKriterij = db.AlternativaKriterijs.Where(x => (x.alternativa == alternativa.id && x.kriterij == kriterij.id)).SingleOrDefault();

                if(alternativaKriterij != null)
                {
                    alternativaKriterij.stvarnaVrijednost = stvarnaVrijednost;
                    db.SaveChanges();
                }else
                {
                    AlternativaKriterij novaAlternativaKriterij = new AlternativaKriterij();
                    novaAlternativaKriterij.alternativa = alternativa.id;
                    novaAlternativaKriterij.kriterij = kriterij.id;
                    novaAlternativaKriterij.stvarnaVrijednost = stvarnaVrijednost;
                    var alternativaKriterijList = db.Set<AlternativaKriterij>();
                    alternativaKriterijList.Add(novaAlternativaKriterij);
                    int a = db.SaveChanges();
                }
            }
        }

        [HttpGet]
        public void ChangeCriteriaComparisonFactor(int criteria1, int criteria2, double factor)
        {
            AHPEntities4 db = new AHPEntities4();
            UsporedbaKriterija usporedbaKriterija = db.UsporedbaKriterijas.Where(x => (x.kriterij1 == criteria1 && x.kriterij2 == criteria2)).SingleOrDefault();
            usporedbaKriterija.vrijednost = factor;

            int a = db.SaveChanges();
        }

        [HttpGet]
        public void ChangeAlternativeComparisonFactor(int alternative1, int alternative2, int criteria, float factor)
        {
            AHPEntities4 db = new AHPEntities4();
            UsporedbaAlternativaPremaKriteriju usporedbaAlternativaPremaKriteriju = db.UsporedbaAlternativaPremaKriterijus.Where(x => (x.alternativa1 == alternative1 && x.alternativa2 == alternative2 && x.kriterij == criteria)).SingleOrDefault();
            if(usporedbaAlternativaPremaKriteriju != null)
            {
                usporedbaAlternativaPremaKriteriju.vrijednost = factor;
            }else
            {
                var usporedbaAlternativaPremaKriterijuLista = db.Set<UsporedbaAlternativaPremaKriteriju>();
                UsporedbaAlternativaPremaKriteriju uapk = new UsporedbaAlternativaPremaKriteriju();
                uapk.alternativa1 = alternative1;
                uapk.alternativa2 = alternative2;
                uapk.kriterij = criteria;
                uapk.vrijednost = factor;
                usporedbaAlternativaPremaKriterijuLista.Add(uapk);
            }
            if(db.SaveChanges() > 0)
            {
                //UpdateProjectsAlternativesConsistency(usporedbaAlternativaPremaKriteriju.Kriterij1.projekt);
            }
        }

        public void UpdateProjectsCriteriaConsistency(int id)
        {
            AHPEntities4 db = new AHPEntities4();
            int maxLevel = 0;
            List<Kriterij> listaRoditelja = db.Kriterijs.Where(x => x.projekt == id && x.Kriterij2.obrisan != true).Select(x => x.Kriterij2).Distinct().ToList();

            List<CriteriaParentsGroupedByLevelsViewModel> listOfCriteriaParentsGroupedByLevels = new List<CriteriaParentsGroupedByLevelsViewModel>();

            foreach (Kriterij roditelj in listaRoditelja)
            {
                int razina = 0;

                CriteriaParentsGroupedByLevelsViewModel criteriaParentsGroupedByLevelsViewModel = null;
                if (roditelj == null)
                {
                    Kriterij kr = new Kriterij();
                    kr.Kriterij1 = db.Kriterijs.Where(x => (x.Kriterij2 == null && x.projekt == id)).ToList();
                    criteriaParentsGroupedByLevelsViewModel = new CriteriaParentsGroupedByLevelsViewModel(razina, kr);
                }
                else
                {
                    Kriterij rod = roditelj.Kriterij2;

                    razina++;

                    if (rod == null)
                    {
                        criteriaParentsGroupedByLevelsViewModel = new CriteriaParentsGroupedByLevelsViewModel(razina, roditelj);
                        if (razina > maxLevel) maxLevel = razina;
                    }
                    else
                    {
                        while (rod != null)
                        {
                            razina++;

                            rod = rod.Kriterij2;
                            if (rod == null)
                            {
                                criteriaParentsGroupedByLevelsViewModel = new CriteriaParentsGroupedByLevelsViewModel(razina, roditelj);
                                if (razina > maxLevel) maxLevel = razina;
                                break;
                            }
                        }
                    }
                }
                listOfCriteriaParentsGroupedByLevels.Add(criteriaParentsGroupedByLevelsViewModel);
            }

            int lev = maxLevel;

            while (lev >= 0)
            {
                List<Kriterij> listaRoditeljaPremaLevelima = listOfCriteriaParentsGroupedByLevels.Where(x => x.Level == lev).Select(x => x.Roditelj).ToList();

                foreach (Kriterij roditelj in listaRoditeljaPremaLevelima)
                {
                    List<Kriterij> listaKriterijaRoditelja = roditelj.Kriterij1.Where(x => x.obrisan != true).ToList();
                    Tuple<bool, List<double?>> konzistentnostIVrijednostKriterija = DBOperationsHelper.GetConsistencyFactorAndCriteriaValues(listaKriterijaRoditelja, roditelj.id, id);

                    if (roditelj.id == 0)
                    {
                        Projekt projekt = db.Projekts.Where(x => x.id == id).SingleOrDefault();
                        if (projekt.konzistentno != konzistentnostIVrijednostKriterija.Item1)
                        {
                            projekt.konzistentno = konzistentnostIVrijednostKriterija.Item1;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        Kriterij kriterijZaUpdate = db.Kriterijs.Where(x => x.id == roditelj.id).SingleOrDefault();
                        if (kriterijZaUpdate.konzistentno != konzistentnostIVrijednostKriterija.Item1)
                        {
                            kriterijZaUpdate.konzistentno = konzistentnostIVrijednostKriterija.Item1;
                            db.SaveChanges();
                        }
                    }

                    for (int i = 0; i < listaKriterijaRoditelja.Count(); i++)
                    {
                        if (konzistentnostIVrijednostKriterija.Item2 != null)
                        {
                            if (listaKriterijaRoditelja[i].vrijednost != konzistentnostIVrijednostKriterija.Item2[i])
                            {
                                listaKriterijaRoditelja[i].vrijednost = konzistentnostIVrijednostKriterija.Item2[i].Value;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (listaKriterijaRoditelja[i].vrijednost != null)
                            {
                                listaKriterijaRoditelja[i].vrijednost = null;
                                db.SaveChanges();
                            }
                        }
                    }

                }
                lev--;
            }
        }

        public void UpdateProjectsAlternativesConsistency(int id)
        {
            
            AHPEntities4 db = new AHPEntities4();
            List<Kriterij> kriteriji = db.Kriterijs.Where(x => (x.projekt == id && x.obrisan != true && x.Kriterij2.obrisan != true && x.Kriterij1.Count() == 0)).ToList();
            List<Alternativa> alternative = db.Alternativas.Where(x => (x.projekt == id && x.obrisana != true)).ToList();
            List<UsporedbaAlternativaPremaKriteriju> usporedbe = db.UsporedbaAlternativaPremaKriterijus.Where(x => (x.Kriterij1.projekt == id && x.Alternativa.obrisana != true && x.Alternativa3.obrisana != true)).ToList();
            foreach (var kriterij in kriteriji)
            {
                int a = 0;
                List<UsporedbaAlternativaPremaKriteriju> usporedbeAlternativaPremaKriteriju = usporedbe.Where(x => x.kriterij == kriterij.id).ToList();
                
                foreach (var alternativa in alternative)
                {
                    int broj = usporedbe.Where(x => (x.kriterij == kriterij.id && (x.alternativa1 == alternativa.id || x.alternativa2 == alternativa.id))).Count();

                    if(broj != alternative.Count() - 1)
                    {
                        kriterij.konzistentneAlternative = false;
                        db.SaveChanges();
                        a = 1;
                        break;
                    }
                }

                if(a == 1)
                {
                    foreach(var alternativa1 in alternative)
                    {
                        alternativa1.vrijednost = null;
                        db.SaveChanges();
                    }
                }else if (a == 0)
                {
                    Tuple<bool, List<double?>> konzistentnostIVrijednost = DBOperationsHelper.GetAlternativesConsistencyFactorAndValues(alternative, usporedbeAlternativaPremaKriteriju, kriterij.id);


                    for (int i = 0; i < alternative.Count(); i++)
                    {
                        if (konzistentnostIVrijednost.Item2 != null)
                        {
                            int b = alternative[i].id;
                            List<AlternativaKriterij> alternativaKriterij = db.AlternativaKriterijs.Where(x => x.alternativa == b && x.kriterij == kriterij.id).ToList();
                            if(alternativaKriterij != null)
                            {
                                if (alternativaKriterij[0].izracunataVrijednost != konzistentnostIVrijednost.Item2[i])
                                {
                                    alternativaKriterij[0].izracunataVrijednost = konzistentnostIVrijednost.Item2[i].Value;
                                    db.SaveChanges();
                                }
                            }else
                            {
                                AlternativaKriterij alternativariterijNova = new AlternativaKriterij();
                                alternativariterijNova.alternativa = alternative[i].id;
                                alternativariterijNova.kriterij = kriterij.id;
                                alternativariterijNova.izracunataVrijednost = konzistentnostIVrijednost.Item2[i].Value;

                                var alternativeKriteriji = db.Set<AlternativaKriterij>();
                                alternativeKriteriji.Add(alternativariterijNova);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (alternative[i].vrijednost != null)
                            {
                                alternative[i].vrijednost = null;
                                db.SaveChanges();
                            }
                        }
                    }
                    kriterij.konzistentneAlternative = konzistentnostIVrijednost.Item1;
                    db.SaveChanges();
                }
            }
        }
    }
}