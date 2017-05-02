using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
    /// Interaction logic for PageLintOnline.xaml
    /// </summary>
    public partial class PageLintOnline : Page
    {
        private WincoalaCore wincoalaCore;
        public PageLintOnline()
        {
            InitializeComponent();
            wincoalaCore = WincoalaCore.Instance;
            Combobox_SelectedBears.ItemsSource = wincoalaCore.getBearList();
            // 58 is the index of PEP8Bear in the list.
            // I can't set it with SelectedItem nor SelectedValue.
            // TODO Fix this hack
            Combobox_SelectedBears.SelectedIndex = 58;

            CodeEditor.Text = "# example code\n# press \"Run coala\"\nprint ( a + b )";
        }

        private void Combobox_SelectedBears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO change CodeEditor syntax highlighting mode based
            // on selected bears.
        }

        private void Button_RunCoala_Click(object sender, RoutedEventArgs e)
        {
            LintRequest request = new LintRequest();
            request.bears = ((BearMetadata) Combobox_SelectedBears.SelectedItem).Name;
            request.file_data = CodeEditor.Text;
            List<Result> results = wincoalaCore.lintOnline(request);
            ListView_Results.ItemsSource = results;
        }
    }

    [ValueConversion(typeof(List), typeof(Visibility))]
    public class ListToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((List<String>)value) != null && ((List<String>)value).Count == 0)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    [ValueConversion(typeof(Dictionary<object, object>), typeof(Visibility))]
    public class DictionaryToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((Dictionary<object, object>)value) == null ||
                ((Dictionary<object, object>)value).Keys.ToList().Count == 0)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
