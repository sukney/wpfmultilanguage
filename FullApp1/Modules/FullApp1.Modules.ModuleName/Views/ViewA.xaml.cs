using FullApp1.Core;
using Prism.Regions;
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

namespace FullApp1.Modules.ModuleName.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class ViewA : UserControl
    {
        IRegionManager regionManager;
        public ViewA(IRegionManager regionManager)
        {
           this. regionManager = regionManager;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            regionManager.RequestNavigate(RegionNames.ContentRegion, "ViewB");
        }
    }
}
