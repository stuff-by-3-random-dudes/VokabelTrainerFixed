using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EnglischAbfrage
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Aufgabe_VOK> aufgaben = new List<Aufgabe_VOK>();
        private Aufgabe_VOK aufgabe = null;
        private Random random = new Random();
        private double prozentFuerLadebalken = 0.0;
        private bool notchecked = false;

        public MainWindow()
        {
            try
            {
                this.Visibility = Visibility.Collapsed;
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler aufgetretetn, bitte an Entwickler wenden, falls sich das Prolem nicht von selbst löst.\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
            }
        }
        private async void NeueFrageDB()
        {
            var index = 0;
            if (aufgaben.Count > 0)
            {
                if (allval.IsChecked == true)
                {
                    notchecked = false;
                }
                else
                {
                    notchecked = true;
                }
                await Task.Run(() => Thread.Sleep(350));

                do
                {
                    // index für eine zufällige Frage auswählen
                    index = random.Next(0, aufgaben.Count);

                    // solange es mehr als eine Frage gibt und es soll nicht dieselbe Frage nacheinander gefragt werden
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

        public void SetupEmptyInputFields(int anz)
        {
            antwortBox.Items.Clear();
            if (notchecked)
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
                    ((TextBox)sender).Background = Brushes.LightGreen;

                    if (aufgabe.AllesGefragt() || notchecked)
                    {
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
            await Task.Run(() => Thread.Sleep(200));
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
            await Task.Run(() => Thread.Sleep(2000));
            aufgabe.Reset();
            NeueFrageDB();
        }
        private void UpdateProgressbar()
        {
            progBar.Value += prozentFuerLadebalken;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            try
            {
                aufgaben = await PersistenzDB.GetVokabeln();
                prozentFuerLadebalken = progBar.Maximum / aufgaben.Count();
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
    }
}
