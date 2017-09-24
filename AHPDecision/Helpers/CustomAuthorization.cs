using AHPDecision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHPDecision.Helpers
{
    public class CustomAuthorization
    {

        public bool AuthorizeUserProject(string UID, int id)
        {
            AHPEntities4 db = new AHPEntities4();

            Projekt projekt = db.Projekts.Where(x => (x.id == id && x.korisnik == UID)).SingleOrDefault();

            if (projekt != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

