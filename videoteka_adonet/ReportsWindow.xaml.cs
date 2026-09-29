using System.Windows;

namespace videoteka_adonet
{
    public partial class ReportsWindow : Window
    {
        public ReportsWindow()
        {
            InitializeComponent();
        }

        private void btnStock_Click(object sender, RoutedEventArgs e)
        {
            var w = new StockReportWindow();
            w.Owner = this;
            w.ShowDialog();
        }

        private void btnCustomerCard_Click(object sender, RoutedEventArgs e)
        {
            var w = new CustomerCardWindow();
            w.Owner = this;
            w.ShowDialog();
        }
    }
}
