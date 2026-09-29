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
// removed DB-init helpers and related usings per user request


namespace videoteka_adonet
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

        private void OpenFilms_Click(object sender, RoutedEventArgs e)
        {
            var w = new FilmsWindow();
            w.Owner = this;
            w.ShowDialog();
        }

        private void OpenCustomers_Click(object sender, RoutedEventArgs e)
        {
            var w = new CustomersWindow();
            w.Owner = this;
            w.ShowDialog();
        }

        private void OpenLoans_Click(object sender, RoutedEventArgs e)
        {
            var w = new LoansWindow();
            w.Owner = this;
            w.ShowDialog();
        }

        private void OpenReports_Click(object sender, RoutedEventArgs e)
        {
            var w = new ReportsWindow();
            w.Owner = this;
            w.ShowDialog();
        }

        // Init DB functionality removed
    }
}