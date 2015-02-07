using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BaseTP1
{
    /// <summary>
    /// Interaction logic for FlecheSelectionControle.xaml
    /// </summary>
    public partial class FlecheSelectionControle : UserControl
    {
        public FlecheSelectionControle()
        {
            InitializeComponent();
            btnDroite.Content = "\u2192";
            btnGauche.Content = "\u2190";
        }
    }
}
