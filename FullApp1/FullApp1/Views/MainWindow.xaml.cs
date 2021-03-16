using FullApp1.Core;
using System.Windows;

namespace FullApp1.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            LocalizedLangExtension.SetLocalLanguage("en-US");

        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            LocalizedLangExtension.SetLocalLanguage("zh-CN");

        }
    }
}
