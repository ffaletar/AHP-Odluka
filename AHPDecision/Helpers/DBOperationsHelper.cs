using AHPDecision.Models;
using AHPDecision.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHPDecision.Helpers
{
    public static class DBOperationsHelper
    {
        public static void AddNewCriteriaComparisons(int idProjekta)
        {
            AHPEntities4 db = new AHPEntities4();
            List<Kriterij> kriteriji = db.Kriterijs.Where(x => (x.Projekt1.id == idProjekta && x.obrisan != true)).ToList();
            List<UsporedbaKriterija> usporedbe = db.UsporedbaKriterijas.ToList();

            for (int i = 0; i < kriteriji.Count; i++)
            {
                for (int j = 0; j < kriteriji.Count; j++)
                {
                    int kriterij1 = kriteriji[i].id;
                    int kriterij2 = kriteriji[j].id;

                    if (kriterij1 != kriterij2 && kriteriji[i].idRoditelja == kriteriji[j].idRoditelja)
                    {
                        bool usporedbePostoje = (db.UsporedbaKriterijas.Any(x => (((x.kriterij1 == kriterij1 && x.kriterij2 == kriterij2) || (x.kriterij1 == kriterij2 && x.kriterij2 == kriterij1) && x.Kriterij.obrisan != true && x.Kriterij3.obrisan != true))));
                        if (!usporedbePostoje)
                        {
                            var usporedbaKriterija = db.Set<UsporedbaKriterija>();
                            usporedbaKriterija.Add(new UsporedbaKriterija { kriterij1 = kriteriji[i].id, kriterij2 = kriteriji[j].id, vrijednost = null });
                            db.SaveChanges();
                        }
                    }
                }
            }
        }


        public static Tuple<bool, List<double?>> GetConsistencyFactorAndCriteriaValues(List<Kriterij> kriteriji, int roditelj, int projektId)
        {
            AHPEntities4 db = new AHPEntities4();
            List<UsporedbaKriterija> listaUsporedaba = new List<UsporedbaKriterija>();

            if (kriteriji.Count() >= 2)
            {
                if (kriteriji.Where(x => x.konzistentno != true).Count() != 0)
                {
                    return new Tuple<bool, List<double?>>(false, null);
                }
                else
                {
                    if (roditelj == 0)
                    {
                        listaUsporedaba = db.UsporedbaKriterijas.Where(x => ((x.Kriterij.Kriterij2 == null || x.Kriterij3.Kriterij2 == null) && (x.Kriterij.obrisan != true && x.Kriterij3.obrisan != true) && (x.Kriterij.projekt == projektId || x.Kriterij3.projekt == projektId))).ToList();
                        return Executor.CreateCriteriaMatrix(kriteriji, listaUsporedaba);
                    }
                    else
                    {
                        listaUsporedaba = db.UsporedbaKriterijas.Where(x => ((x.Kriterij.idRoditelja == roditelj || x.Kriterij3.idRoditelja == roditelj) && (x.Kriterij.obrisan != true && x.Kriterij3.obrisan != true))).ToList();
                        if (listaUsporedaba.Where(x => x.vrijednost == null).Count() > 0)
                        {
                            return new Tuple<bool, List<double?>>(false, null);
                        }
                        return Executor.CreateCriteriaMatrix(kriteriji, listaUsporedaba);
                    }
                }
            }
            else
            {
                return new Tuple<bool, List<double?>>(false, null);
            }
        }


        public static Tuple<bool, List<double?>> GetAlternativesConsistencyFactorAndValues(List<Alternativa> alternative,List<UsporedbaAlternativaPremaKriteriju> usporedbe, int kriterij)
        {
            AHPEntities4 db = new AHPEntities4();
            Tuple<bool, List<double?>>  aa = Executor.CreateAlternativeMatrix(alternative, usporedbe);

            return aa;
        }

    }
}