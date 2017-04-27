using System;
using System.Collections;
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

namespace wincoala
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WincoalaCore wincoalaCore = WincoalaCore.Instance;
            MainFrame.Content = new PageLintOnline();
        }

        private void ButtonLintOnline_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new PageLintOnline();
        }

        private void ButtonBearList_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new PageBearList();
        }
    }
}
