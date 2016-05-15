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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorkWithDB.UI.Views.StructuralUnits.CurrentUnitSetter
{
    /// <summary>
    /// Interaction logic for StructuralUnitSetter.xaml
    /// </summary>
    public partial class StructuralUnitSetter : Window
    {
        public StructuralUnitSetter()
        {
            InitializeComponent();
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
