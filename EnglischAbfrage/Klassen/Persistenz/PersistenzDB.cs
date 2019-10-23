using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace EnglischAbfrage
{
    class PersistenzDB
    {
        static Random rdm = new Random();

              // string = Deutsches wort, List<string> = Englische Wörter
        public async static Task<Tuple<List<int>,List<string>>> GetKapitel()
        {
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://vokabelapi.azurewebsites.net/api/kapitel/kapitel");
            var httpResponse = (HttpWebResponse)(await httpWebRequest.GetResponseAsync());
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var resfix = FixResponse(result);
                // keine ahnung ob des mit einem kapitel funktioniert, wir werden es nie erfahren
                var kapsplit = resfix.Split('|');
                foreach(string s in kapsplit)
                {
                    var splitkap = s.Split(';');
                    ids.Add(Convert.ToInt32(splitkap[0]));
                    names.Add(splitkap[1]);
                }
            }
            return new Tuple<List<int>, List<string>>(ids, names);
        }


        public async static Task<List<Aufgabe_VOK>> GetVokabeln(List<int> ids, int kap)
        {
            List<Aufgabe_VOK> retval = new List<Aufgabe_VOK>();
            
            foreach(int id in ids)
            {
                var deutsch = string.Empty;
                var id2 = string.Empty;
                List<string> englisch = new List<string>();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://vokabelapi.azurewebsites.net/api/de_vok/deutschvokkap?vok=" + id + "&kap=" + kap);
                var httpResponse = (HttpWebResponse)(await httpWebRequest.GetResponseAsync());


                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var resfix = FixResponse(result);
                    deutsch = resfix.Split(',')[1];
                    id2 = resfix.Split(',')[0];
                }
                httpWebRequest = (HttpWebRequest)WebRequest.Create("https://vokabelapi.azurewebsites.net/api/en_vok/englischvok?vok=" + id2);
                httpResponse = (HttpWebResponse)(await httpWebRequest.GetResponseAsync());
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var fixr = FixResponse(result).Split(';');
                    foreach (string s in fixr)
                    {
                        englisch.Add(s);
                    }
                }
                retval.Add(new Aufgabe_VOK(deutsch, englisch, id));
            }
            

            //return new Aufgabe_VOK(deutsch,englisch, id);
            return retval;
        }
        public async static Task<Aufgabe_VOK> GetVokabeln(List<int> ids)
        {
            var id = RandomID(ids);
            var deutsch = string.Empty;
            var id2 = string.Empty;
            List<string> englisch = new List<string>();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://vokabelapi.azurewebsites.net/api/de_vok/deutschvokkap?vok=" + id);
            var httpResponse = (HttpWebResponse)(await httpWebRequest.GetResponseAsync());


            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                deutsch = FixResponse(result);
            }
            httpWebRequest = (HttpWebRequest)WebRequest.Create("https://vokabelapi.azurewebsites.net/api/en_vok/englischvok?vok=" + id);
            httpResponse = (HttpWebResponse)(await httpWebRequest.GetResponseAsync());
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var fixr = FixResponse(result).Split(';');
                foreach (string s in fixr)
                {
                    englisch.Add(s);
                }
            }

            return new Aufgabe_VOK(deutsch, englisch, id);
        }
        private static string FixResponse(string response)
        {
            return response.Replace("\"", "").Replace("\\","").Trim();
        }

        private static int RandomID(List<int> ids)
        {
           return ids[rdm.Next(0, ids.Count)];
        }

       public static List<int> GetIdList()
        {
            List<int> ids = new List<int>();
            int anz = 0;
            //Id anzahl über api bekommen
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://vokabelapi.azurewebsites.net/api/anz_vok/anzvok");
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            //api antwort in anzahl umwandeln
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var splitres = FixResponse(result);
                anz = Convert.ToInt32(splitres);
                
            }



            //id's "berechnen"

            for (int i = 1; i <= anz; i++)
            {
                ids.Add(i);
            }
            return ids;
        }
        public static List<int> GetIdList(int Kapitel)
        {
            List<int> ids = new List<int>();
            int anz = 0;
            //Id anzahl über api bekommen
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://vokabelapi.azurewebsites.net/api/anz_vok/anzvokkap?kapt="+Kapitel);
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            //api antwort in anzahl umwandeln
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var splitres = FixResponse(result);
                anz = Convert.ToInt32(splitres);

            }



            //id's "berechnen"

            for (int i = 1; i <= anz; i++)
            {
                ids.Add(i);
            }
            return ids;
        }
    }
}
