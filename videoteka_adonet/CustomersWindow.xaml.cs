using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows;
using videoteka_adonet.Data;

namespace videoteka_adonet
{
    public partial class CustomersWindow : Window
    {
        public CustomersWindow()
        {
            InitializeComponent();
            Loaded += CustomersWindow_Loaded;
        }

        private void CustomersWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                var sql = "SELECT KupacId, FirstName, LastName, MembershipNumber FROM Kupac ORDER BY LastName, FirstName";
                var dt = Database.GetDataTable(sql);
                dgCustomers.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju kupaca: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var w = new CustomerEditWindow();
            w.Owner = this;
            if (w.ShowDialog() == true)
                LoadCustomers();
        }

        private void EditSelectedCustomer()
        {
            if (dgCustomers.SelectedItem is DataRowView drv)
            {
                var id = Convert.ToInt32(drv["KupacId"]);
                var w = new CustomerEditWindow(id);
                w.Owner = this;
                if (w.ShowDialog() == true)
                    LoadCustomers();
            }
            else
            {
                MessageBox.Show("Odaberite kupca za uređivanje.", "Obavijest", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedCustomer();
        }

        private void dgCustomers_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditSelectedCustomer();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem is DataRowView drv)
            {
                var id = Convert.ToInt32(drv["KupacId"]);
                var name = drv["FirstName"].ToString() + " " + drv["LastName"].ToString();
                var res = MessageBox.Show($"Obrisati kupca '{name}' i sve njegove posudbe?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        var sql = "DELETE FROM Kupac WHERE KupacId = @KupacId";
                        Database.ExecuteNonQuery(sql, new SqlParameter("@KupacId", id));
                        LoadCustomers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greška pri brisanju kupca: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Odaberite kupca za brisanje.", "Obavijest", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
