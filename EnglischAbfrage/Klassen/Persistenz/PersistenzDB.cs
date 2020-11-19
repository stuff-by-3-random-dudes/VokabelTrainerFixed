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

            // Reader erstellen, um die Datei zu lesen
            // durch using wird er am Ende des Blocks automatisch geschlossen
            using (StreamReader reader = new StreamReader("daten.csv"))
            {
                // erste Zeile vorlesen
                zeile = reader.ReadLine();

                // solange die eingelesene Zeile nicht leer ist
                while (!String.IsNullOrEmpty(zeile))
                {
                    // zeile an ';' aufteilen
                    felder = zeile.Split(';');

                    // ersten wert in variable 'deutsch' speichern
                    deutsch = felder[0];

                    // restliche werte sind englische Lösungen
                    englisch = felder.ToList();
                    // natürlich ersten Wert (deutsch) entfernen
                    englisch.RemoveAt(0);

                    // der liste ein neues Objekt hinzufügen
                    aufgaben.Add(new Aufgabe_VOK(deutsch, englisch));

                    // nächste Zeile lesen
                    zeile = reader.ReadLine();
                }
            }

            return aufgaben;
        }
    }
}
