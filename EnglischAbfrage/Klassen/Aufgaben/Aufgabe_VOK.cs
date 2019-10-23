using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglischAbfrage
{
    class Aufgabe_VOK
    {
        private string frage;
        private List<string> antwort;
        public int CurrentID { get; set; }
        //private string[] aBackup;
        private List<string> beantwortet = new List<string>();
        public bool Richtig { get; set; } = false;

        public Aufgabe_VOK(string frage, List<string> antwort)
        {
            this.frage = frage;
            this.antwort = antwort;
            //aBackup = new string[antwort.Count];
            //antwort.CopyTo(aBackup);
        }
        public Aufgabe_VOK(string frage, List<string> antwort, int id)
        {
            this.frage = frage;
            this.antwort = antwort;
            CurrentID = id;
            //aBackup = new string[antwort.Count];
            //antwort.CopyTo(aBackup);
        }
        public string GetFrage()
        {
            return frage;
        }
        public List<string> GetAntwort()
        {
            return antwort;
        }
        public List<string> GetAlleAntworten()
        {
            List<string> alleaw = new List<string>();
            beantwortet.ForEach(item => { alleaw.Add(item); });
            antwort.ForEach(item => { alleaw.Add(item); });
            return alleaw;
        }
        public List<string> GetBeantwortet()
        {
            return beantwortet;
        }
        public bool AllesGefragt()
        {
            bool allesGefragt = false;
            if (antwort.Count == 0)
            {
                allesGefragt = true;
            }
            return allesGefragt;
        }
        public void PruefeAntwort(string eingabe)
        {
            bool richtig = false;
            string remove = string.Empty;
            foreach (string aw in antwort)
            {
                if (eingabe.Trim() == aw.Trim() || RemoveTo(aw) == RemoveTo(eingabe) && RemoveTo(aw) != aw)
                {
                    richtig = true;
                    remove = aw;
                }
            }
            if (richtig)
            {
                antwort.Remove(remove);
                beantwortet.Add(remove);
            }
            this.Richtig = richtig;
        }
        private string RemoveTo(string text)
        {
            //wenn aw verb ist, ignoriere to in eingabe
            //wenn aw kein verb ist, ignoriere to nicht in eingabe
            string[] signatur = { "to ", "(to) " };
            text = text.Trim();
            foreach (string s in signatur)
            {
                if (text.Length > s.Length && text.StartsWith(s))
                {
                    text = text.Substring(s.Length, text.Length - s.Length);
                }
            }
            return text;
        }
        public void Reset()
        {
            //antwort = aBackup.ToList();
            foreach (string s in beantwortet)
            {
                antwort.Add(s);
            }
            beantwortet.Clear();
        }
        public string GetRightAnswer()
        {
            string ausgabe = $"Frage: {frage}\n\nAntworten:";
            foreach (string aw in antwort)
            {
                ausgabe += $"\n\t{aw}";
            }
            return ausgabe;
        }
        public int GetAnzahlAntworten()
        {
            return antwort.Count() + beantwortet.Count();
        }
    }
}
