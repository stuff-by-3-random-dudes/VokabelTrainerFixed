using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EnglischAbfrage.Windows
{
    /// <summary>
    /// Interaktionslogik für KapitelWindow.xaml
    /// </summary>
    public partial class KapitelWindow : Window
    {
        List<int> kapitelIds = new List<int>();
        List<string> names = new List<string>();
        public KapitelWindow()
        {
           
            InitializeComponent();
            getKapitel();
            
        }

        private void listView_Click(object sender, MouseButtonEventArgs e)
        {
            TypeWindow tw = new TypeWindow(kapitelIds[(sender as ListView).SelectedIndex]);
            tw.Show();
            this.Close();
        }

        private async void getKapitel()
        {
            Tuple<List<int>, List<string>> tpl = await PersistenzDB.GetKapitel();
            names = tpl.Item2;
            kapitelIds = tpl.Item1;
            KapitelListView.ItemsSource = names;
        }
    }
}
