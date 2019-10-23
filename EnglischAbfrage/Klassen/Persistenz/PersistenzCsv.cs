using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglischAbfrage
{
    class PersistenzCsv
    {
        private bool endOfFile = false;

        private string pfad = "daten.csv";
        private char separator;
        private bool ueberschrift = false;

        private StreamReader rd;

        public PersistenzCsv(string pfad, char separator = ';')
        {
            if (pfad != string.Empty)
            {
                this.pfad = pfad;
            }
            this.separator = separator;
            if (!File.Exists(pfad))
            {
                throw new Exception($"Die Datei '{pfad}' existiert nicht, ist im falschen Ordner oder es sind keine Leserechte vorhanden.");
            }
            rd = new StreamReader(pfad);
        }
        public PersistenzCsv(string pfad, char separator, bool ueberschrift) : this(pfad, separator)
        {
            this.ueberschrift = ueberschrift;
            if (ueberschrift)
            {
                ReadLine();
            }
        }
        private string ReadLine()
        {
            string row = null;
            if (!endOfFile)
            {
                row = rd.ReadLine();
                if (row == null)
                {
                    row = String.Empty;
                    endOfFile = true;
                }
            }
            return row;
        }
        public string[] RowValues()
        {
            string[] fields = null;
            fields = ReadLine().Split(separator);
            return fields;
        }
        public List<string[]> AllRowValues()
        {
            List<string[]> liste = new List<string[]>();
            string[] element;
            while (!endOfFile)
            {
                //liste.Add(RowValues());
                element = RowValues();
                if (!endOfFile)
                {
                    liste.Add(element);
                }
            }
            Close();
            return liste;
        }
        public bool EndOfFile()
        {
            return endOfFile;
        }
        public void Close()
        {
            rd.Close();
        }
    }
}
