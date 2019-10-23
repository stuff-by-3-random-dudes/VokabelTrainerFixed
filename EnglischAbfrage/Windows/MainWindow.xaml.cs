using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EnglischAbfrage
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path = "daten.csv";
        private int KID { get; set; }
        private List<Aufgabe_VOK> aufgaben = new List<Aufgabe_VOK>();
        private List<int> ausstehendId = new List<int>();
        private Aufgabe_VOK aufgabe = null;
        private Random random = new Random();
        private double incval = 0.0;
        private bool notchecked = false;
        // ToDo wenn man nicht mehr mag, irgendeinen Knopf drücken oder so, der einem die richtige Antwort zeigt, und diese wieder mit aufgabe.Reset resettet
        public MainWindow()
        {
            try
            {
                this.Visibility = Visibility.Collapsed;
                InitializeComponent();
                //SetupAufgabenDB();
                //SetupAufgabenCSV();
                //NeueFrageCSV();
                //NeueFrageDB();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler aufgetretetn, bitte an Entwickler wenden, falls sich das Prolem nicht von selbst löst.\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
            }
        }
        public MainWindow(int KapitelID)
        {
           
            KID = KapitelID;
            InitializeComponent();
           
        }
        private void SetupAufgabenCSV()
        {
            List<String[]> daten = new PersistenzCsv(path, ';').AllRowValues();
            for (int i = 0; i < daten.Count; i++)
            {
                string frage = daten[i][0];
                var antworten = daten[i].ToList<string>();
                antworten.RemoveAt(0);
                aufgaben.Add(new Aufgabe_VOK(frage, antworten));
            }
        }
        private void SetupAufgabenDB(int kapitel)
        {
            ausstehendId = PersistenzDB.GetIdList(kapitel);
        }
        private async void NeueFrageDB()
        {
            var index = 0;
            if (aufgaben.Count > 0)
            {
                if(allval.IsChecked == true)
                {
                    notchecked = false;
                }
                else
                {
                    notchecked = true;
                }
                //aufgabe = await PersistenzDB.GetVokabeln(PersistenzDB.GetIdList(KID),KID);
                //aufgabe = await PersistenzDB.GetVokabeln(ausstehendId, KID);
                await Task.Run(() => Thread.Sleep(350));
               
                do
                {
                    index = random.Next(0, aufgaben.Count);
                } while (index == aufgaben.IndexOf(aufgabe) && aufgaben.Count > 1);
                aufgabe = aufgaben[index];
                SetupEmptyInputFields(aufgabe.GetAnzahlAntworten());
                frageBox.Text = aufgabe.GetFrage();
                
                FocusEmptyOrFalseElement();

            }
            else
            {
                skipBtn.IsEnabled = false;
            }
        }
        private void NeueFrageCSV()
        {
            if (aufgaben.Count > 0)
            {
                aufgabe = aufgaben[random.Next(0, aufgaben.Count)];
                SetupEmptyInputFields(aufgabe.GetAnzahlAntworten());
                frageBox.Text = aufgabe.GetFrage();
                //MessageBox.Show("Irgendeinen Knopf drücken.");
                //MessageBox.Show($"Antwortbox: {antwortBox.Items.Count}");
                //MessageBox.Show("Beliebige Taste drücken. WARUM GEHT DAS OHNE MSGBOX NICHT?");
                FocusEmptyOrFalseElement();
                // ToDo progressbar updaten
            }
            else
            {
                MessageBox.Show("Ende");
            }
        }
        
        public void SetupEmptyInputFields(int anz)
        {
            antwortBox.Items.Clear();
            if(notchecked)
            {
                
                anz = 1;
            }
            for (int i = 0; i < anz; i++)
            {
               
                TextBox awFeld = new TextBox();
                awFeld.TextAlignment = TextAlignment.Center;
                awFeld.MinWidth = 300;
                awFeld.FontSize = 20;
                awFeld.TextChanged += new TextChangedEventHandler(CheckInput);
                Grid.SetColumn(awFeld, i);
                antwortBox.Items.Add(awFeld);
            }
        }


        private void CheckInput(object sender, TextChangedEventArgs e)
        {
            try
            {
                aufgabe.PruefeAntwort(((TextBox)sender).Text);
                if (aufgabe.Richtig)
                {
                    ((TextBox)sender).IsReadOnly = true;
                    ((TextBox)sender).Background = Brushes.LightGreen;//yellowgreen
                    
                    if (aufgabe.AllesGefragt() || notchecked)
                    {

                        //aufgaben.Remove(aufgabe);
                        // ausstehendId.Remove(aufgabe.CurrentID);
                        aufgaben.Remove(aufgabe);
                        UpdateProgressbar();
                        NeueFrageDB();
                    }
                    else
                    {
                        FocusEmptyOrFalseElement();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler aufgetretetn, bitte an Entwickler wenden, falls sich das Prolem nicht von selbst löst.\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
            }
        }

        private async void FocusEmptyOrFalseElement()
        {
            await Task.Run(()=> Thread.Sleep(200));
            foreach (UIElement answer in antwortBox.Items)
            {
                if (!((TextBox)answer).IsReadOnly)
                {
                    ((TextBox)answer).Focus();
                    break;
                }
            }
        }

        private async void skipBtn_Click(object sender, RoutedEventArgs e)
        {
            string antwort = string.Empty;
            foreach (string s in aufgabe.GetAntwort())
            {
                antwort += "\n" + s;
            }
            List<string> loesungen = aufgabe.GetAlleAntworten();
            for (int i = 0; i < loesungen.Count; i++)
            {
                ((TextBox)antwortBox.Items[i]).IsReadOnly = true;
                ((TextBox)antwortBox.Items[i]).TextChanged -= CheckInput;
                ((TextBox)antwortBox.Items[i]).Text = loesungen[i];
                
            }
            await Task.Run(()=>Thread.Sleep(2000));
            aufgabe.Reset();
            NeueFrageDB();
        }
        private void UpdateProgressbar()
        {
            //in initialisieren maxvalue setzten und hier value = ausstehend.count
            progBar.Value += incval;
            //progBar.Value = progBar.Maximum - ausstehendId.Count;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            try
            {
                
                ///await methode hier => sobald diese fertig ist ist dieses fenster sichtbar
                ///meanwhile ladefenster
                aufgaben = await PersistenzDB.GetVokabeln(PersistenzDB.GetIdList(KID), KID);
                incval = progBar.Maximum / aufgaben.Count();
                NeueFrageDB();
                await Task.Run(() => Thread.Sleep(500));
                this.Visibility = Visibility.Visible;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler aufgetretetn, bitte an Entwickler wenden, falls sich das Prolem nicht von selbst löst.\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
            }
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EnglischAbfrage.Windows.KapitelWindow kapitelWindow = new Windows.KapitelWindow();
            kapitelWindow.Show();
        }

        private void Window_Loaded(object sender, EventArgs e)
        {

        }
    }
    
}
