using AHPDecision.Models;
using AHPDecision.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AHPDecision.Controllers
{
    public class EvaluationController : Controller
    {
        // GET: Evaluation
        public ActionResult Index(int id, int? kriterij)
        {
            AHPEntities4 db = new AHPEntities4();
            List<UsporedbaKriterija> criteriaComparison = new List<UsporedbaKriterija>();

            Projekt project = db.Projekts.Where(x => x.id == id).SingleOrDefault();
            Kriterij parent = null;
            if (kriterij != null)
            {
                parent = db.Kriterijs.Where(x => x.id == kriterij).SingleOrDefault();
                foreach (Kriterij podkriterij in parent.Kriterij1.Where(x => x.obrisan != true).ToList())
                {
                    List<UsporedbaKriterija> a = podkriterij.UsporedbaKriterijas.Where(x => (x.Kriterij.obrisan != true && x.Kriterij3.obrisan != true)).ToList();

                    //Je li dovoljno jednostrano ili je potrebno obostrano?? ispitati
                    //List<UsporedbaKriterija> b = podkriterij.UsporedbaKriterijas1.Where(x => (x.Kriterij.obrisan != true && x.Kriterij3.obrisan != true)).ToList();

                    criteriaComparison.AddRange(a);
                }
            }else
            {
                criteriaComparison = db.UsporedbaKriterijas.Where(x => (x.Kriterij.Projekt1.id == id && x.Kriterij3.Projekt1.id == id && x.Kriterij.Kriterij2 == null && x.Kriterij3.Kriterij2 == null && x.Kriterij.obrisan != true && x.Kriterij3.obrisan != true)).ToList();
            }

            EvaluationCriteriaViewModel evaluationCriteriaViewModel = new EvaluationCriteriaViewModel(project, parent, criteriaComparison, GetListOfCriteria(id));

            return View(evaluationCriteriaViewModel);
        }

        public ActionResult Alternatives(int projekt, int kriterij)
        {
            AHPEntities4 db = new AHPEntities4();
            Projekt odabraniProjekt = db.Projekts.Where(x => x.id == projekt).SingleOrDefault();

            if (kriterij != -1)
            {

                List<Kriterij> kriteriji = GetListOfCriteria(projekt);
                List<Alternativa> alternative = db.Alternativas.Where(x => (x.projekt == projekt && x.obrisana != true)).ToList();
                List<AlternativaKriterij> alternativaKriterij = db.AlternativaKriterijs.Where(x => (x.kriterij == kriterij && x.Alternativa1.obrisana != false)).ToList();


                Kriterij odabraniKriterij = db.Kriterijs.Where(x => x.id == kriterij).SingleOrDefault();

                List<AlternativesComparedViewModel> alternativesComparedViewModel = new List<AlternativesComparedViewModel>();

                foreach (var alternativa1 in alternative)
                {
                    foreach (var alternativa2 in alternative)
                    {
                        if (alternativa1.id != alternativa2.id)
                        {
                            if (alternativesComparedViewModel.Where(x => ((x.Alternativa1.id == alternativa1.id && x.Alternativa2.id == alternativa2.id) || (x.Alternativa2.id == alternativa1.id && x.Alternativa1.id == alternativa2.id))).Count() == 0)
                            {
                                alternativesComparedViewModel.Add(new AlternativesComparedViewModel(alternativa1, alternativa2));
                            }
                        }
                    }
                }



                List<UsporedbaAlternativaPremaKriteriju> usporedbaAlternativaPremaKriteriju = odabraniKriterij.UsporedbaAlternativaPremaKriterijus.Where(x => (x.Kriterij1.id == kriterij)).ToList();

                EvaluationAlternativesViewModel evaluationAlternativesViewModel = new EvaluationAlternativesViewModel();
                evaluationAlternativesViewModel.Kriterij = odabraniKriterij;
                evaluationAlternativesViewModel.Projekt = odabraniProjekt;
                evaluationAlternativesViewModel.AlternativaKriterij = alternativaKriterij;
                evaluationAlternativesViewModel.Kriteriji = kriteriji;
                evaluationAlternativesViewModel.Alternative = alternative;
                evaluationAlternativesViewModel.ListaKombinacijaAlternativa = alternativesComparedViewModel;
                evaluationAlternativesViewModel.UsporedbaAlternativaPremaKriteriju = usporedbaAlternativaPremaKriteriju;
                return View(evaluationAlternativesViewModel);

            }
            else
            {

                List<Kriterij> kriteriji = GetListOfCriteria(projekt);

                kriterij = kriteriji[0].id;
                List<Alternativa> alternative = db.Alternativas.Where(x => (x.projekt == projekt && x.obrisana != true)).ToList();
                List<AlternativaKriterij> alternativaKriterij = db.AlternativaKriterijs.Where(x => (x.kriterij == kriterij && x.Alternativa1.obrisana != false)).ToList();


                Kriterij odabraniKriterij = db.Kriterijs.Where(x => x.id == kriterij).SingleOrDefault();

                List<AlternativesComparedViewModel> alternativesComparedViewModel = new List<AlternativesComparedViewModel>();

                foreach (var alternativa1 in alternative)
                {
                    foreach (var alternativa2 in alternative)
                    {
                        if (alternativa1.id != alternativa2.id)
                        {
                            if (alternativesComparedViewModel.Where(x => ((x.Alternativa1.id == alternativa1.id && x.Alternativa2.id == alternativa2.id) || (x.Alternativa2.id == alternativa1.id && x.Alternativa1.id == alternativa2.id))).Count() == 0)
                            {
                                alternativesComparedViewModel.Add(new AlternativesComparedViewModel(alternativa1, alternativa2));
                            }
                        }
                    }
                }



                List<UsporedbaAlternativaPremaKriteriju> usporedbaAlternativaPremaKriteriju = odabraniKriterij.UsporedbaAlternativaPremaKriterijus.Where(x => (x.Kriterij1.id == kriterij)).ToList();

                EvaluationAlternativesViewModel evaluationAlternativesViewModel = new EvaluationAlternativesViewModel();
                evaluationAlternativesViewModel.Kriterij = odabraniKriterij;
                evaluationAlternativesViewModel.Projekt = odabraniProjekt;
                evaluationAlternativesViewModel.AlternativaKriterij = alternativaKriterij;
                evaluationAlternativesViewModel.Kriteriji = kriteriji;
                evaluationAlternativesViewModel.Alternative = alternative;
                evaluationAlternativesViewModel.ListaKombinacijaAlternativa = alternativesComparedViewModel;
                evaluationAlternativesViewModel.UsporedbaAlternativaPremaKriteriju = usporedbaAlternativaPremaKriteriju;
                return View(evaluationAlternativesViewModel);
            }

        }


        [HttpGet]
        public PartialViewResult GetCriteriaMenuFullPartialView(int id)
        {
            AHPEntities4 db = new AHPEntities4();

            Projekt projekt = db.Projekts.Where(x => x.id == id).SingleOrDefault();

            EvaluationCriteriaViewModel evaluationCriteriaViewModel = new EvaluationCriteriaViewModel();
            evaluationCriteriaViewModel.Projekt = projekt;
            evaluationCriteriaViewModel.ListaKriterija = GetListOfCriteria(id);

            return PartialView("_CriteriaMenuPartial", evaluationCriteriaViewModel);
        }

        [HttpGet]
        public PartialViewResult GetCriteriaAlternativesMenuFullPartialView(int id, int kriterij)
        {
            AHPEntities4 db = new AHPEntities4();

            List<Kriterij> kriteriji = GetListOfCriteria(id);
            Projekt odabraniProjekt = db.Projekts.Where(x => x.id == id).SingleOrDefault();
            List<Alternativa> alternative = db.Alternativas.Where(x => (x.projekt == id && x.obrisana != true)).ToList();
            List<AlternativaKriterij> alternativaKriterij = db.AlternativaKriterijs.Where(x => (x.kriterij == kriterij && x.Alternativa1.obrisana != true)).ToList();
            Kriterij odabraniKriterij = db.Kriterijs.Where(x => x.id == kriterij).SingleOrDefault();
            List<AlternativesComparedViewModel> alternativesComparedViewModel = new List<AlternativesComparedViewModel>();

            foreach (var alternativa1 in alternative)
            {
                foreach (var alternativa2 in alternative)
                {
                    if (alternativa1.id != alternativa2.id)
                    {
                        if (alternativesComparedViewModel.Where(x => ((x.Alternativa1.id == alternativa1.id && x.Alternativa2.id == alternativa2.id) || (x.Alternativa2.id == alternativa1.id && x.Alternativa1.id == alternativa2.id))).Count() == 0)
                        {
                            alternativesComparedViewModel.Add(new AlternativesComparedViewModel(alternativa1, alternativa2));
                        }
                    }
                }
            }

            List<UsporedbaAlternativaPremaKriteriju> usporedbaAlternativaPremaKriteriju = odabraniKriterij.UsporedbaAlternativaPremaKriterijus.Where(x => (x.Kriterij1.id == kriterij)).ToList();


            EvaluationAlternativesViewModel evaluationAlternativesViewModel = new EvaluationAlternativesViewModel();
            evaluationAlternativesViewModel.Projekt = odabraniProjekt;
            evaluationAlternativesViewModel.AlternativaKriterij = alternativaKriterij;
            evaluationAlternativesViewModel.Kriteriji = kriteriji;
            evaluationAlternativesViewModel.Kriterij = odabraniKriterij;
            evaluationAlternativesViewModel.Alternative = alternative;
            evaluationAlternativesViewModel.ListaKombinacijaAlternativa = alternativesComparedViewModel;
            evaluationAlternativesViewModel.UsporedbaAlternativaPremaKriteriju = usporedbaAlternativaPremaKriteriju;

            return PartialView("_CriteriaMenuAlternativesPartial", evaluationAlternativesViewModel);
        }

        [HttpGet]
        public List<Kriterij> GetListOfCriteria(int id)
        {
            AHPEntities4 db = new AHPEntities4();
            List<Kriterij> criteria = db.Kriterijs.Where(x => (x.projekt == id && x.Kriterij2 == null && x.obrisan != true)).ToList();
            
            return criteria;
        }
        [HttpGet]
        public List<Alternativa> GetListOfAlternatives(int id)
        {
            AHPEntities4 db = new AHPEntities4();
            List<Alternativa> alternatives = db.Alternativas.Where(x => (x.projekt == id && x.obrisana != true)).ToList();

            return alternatives;
        }


        public static void CalculateResults(int id)
        {
            AHPEntities4 db = new AHPEntities4();
            List<Alternativa> alternative = db.Alternativas.Where(x => (x.projekt == id && x.obrisana != true)).ToList();
            List<Kriterij> kriteriji = db.Kriterijs.Where(x => (x.projekt == id && x.obrisan != true && x.Kriterij1.Count() == 0)).ToList();
            List<CalculatedCriteriaValuesViewModel> listaKalkuliranihVrijednostiKriterija = new List<CalculatedCriteriaValuesViewModel>();

            foreach(var kriterij in kriteriji)
            {
                double? vrijednost = 0;
                if(kriterij.Kriterij2 != null)
                {
                    if(kriterij.Kriterij2.Kriterij2 != null)
                    {
                        if(kriterij.Kriterij2.Kriterij2.Kriterij2 != null)
                        {

                        }else
                        {
                            vrijednost = kriterij.Kriterij2.Kriterij2.vrijednost * kriterij.Kriterij2.vrijednost * kriterij.vrijednost;
                        }
                    }else
                    {
                        vrijednost = kriterij.Kriterij2.vrijednost * kriterij.vrijednost;
                    }
                }else
                {
                    vrijednost = kriterij.vrijednost;
                }

                CalculatedCriteriaValuesViewModel calculatedCriteriaValuesViewModel = new CalculatedCriteriaValuesViewModel();
                calculatedCriteriaValuesViewModel.Kriterij = kriterij;
                calculatedCriteriaValuesViewModel.vrijednost = vrijednost;

                listaKalkuliranihVrijednostiKriterija.Add(calculatedCriteriaValuesViewModel);
            }

            List<AlternativaKriterij> alternativaKriteriji = db.AlternativaKriterijs.Where(x => x.Kriterij1.projekt == id).ToList();

            foreach(var alternativa in alternative)
            {
                double? rezultat = 0;
                foreach(var kriterijZaUsporedbu in kriteriji)
                {
                    AlternativaKriterij alternativaKriterij = alternativaKriteriji.Where(x => (x.alternativa == alternativa.id && x.kriterij == kriterijZaUsporedbu.id)).SingleOrDefault();

                    if(alternativaKriterij != null)
                    {
                        rezultat += kriterijZaUsporedbu.vrijednost * alternativaKriterij.izracunataVrijednost;
                    }
                }

                alternativa.vrijednost = rezultat;

                db.SaveChanges();
            }
        }
    }
}