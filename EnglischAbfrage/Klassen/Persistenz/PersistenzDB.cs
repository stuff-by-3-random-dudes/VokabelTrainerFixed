using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EnglischAbfrage
{
    class PersistenzDB
    {
        public async static Task<List<Aufgabe_VOK>> GetVokabeln()
        {
            List<Aufgabe_VOK> aufgaben = new List<Aufgabe_VOK>();

            string deutsch = String.Empty;
            List<string> englisch = new List<string>();

            string zeile;
            string[] felder;

            using (StreamReader reader = new StreamReader("daten.csv"))
            {
                zeile = reader.ReadLine();
                while (!String.IsNullOrEmpty(zeile))
                {
                    felder = zeile.Split(';');
                    deutsch = felder[0];

                    englisch = felder.ToList();
                    englisch.RemoveAt(0);

                    aufgaben.Add(new Aufgabe_VOK(deutsch, englisch));

                    zeile = reader.ReadLine();
                }
            }

            return aufgaben;
        }
    }
}
