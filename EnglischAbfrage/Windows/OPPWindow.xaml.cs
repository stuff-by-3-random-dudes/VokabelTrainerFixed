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

namespace EnglischAbfrage
{
    /// <summary>
    /// Interaktionslogik für OPPWindow.xaml
    /// </summary>
    public partial class OPPWindow : Window
    {
        //To-Do: Alles hier drinnen
        private int KapitelID { get; set; } = 0;
        public OPPWindow()
        {
            InitializeComponent();
        }
        public OPPWindow(int kapitelID)
        {
            
            KapitelID = kapitelID;
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }
    }
}
