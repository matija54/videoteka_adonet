using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows;
using videoteka_adonet.Data;

namespace videoteka_adonet
{
    public partial class CustomerEditWindow : Window
    {
        private int? _kupacId;

        public CustomerEditWindow()
        {
            InitializeComponent();
            Loaded += CustomerEditWindow_Loaded;
        }

        public CustomerEditWindow(int kupacId) : this()
        {
            _kupacId = kupacId;
        }

        private void CustomerEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (_kupacId.HasValue)
                LoadKupac(_kupacId.Value);
        }

        private void LoadKupac(int id)
        {
            var sql = "SELECT KupacId, FirstName, LastName, Address, MembershipNumber FROM Kupac WHERE KupacId = @Id";
            var dt = Database.GetDataTable(sql, new SqlParameter("@Id", id));
            if (dt.Rows.Count == 0) return;
            var r = dt.Rows[0];
            txtFirstName.Text = r["FirstName"].ToString();
            txtLastName.Text = r["LastName"].ToString();
            txtAddress.Text = r["Address"] == DBNull.Value ? string.Empty : r["Address"].ToString();
            txtMembership.Text = r["MembershipNumber"].ToString();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var first = txtFirstName.Text.Trim();
            var last = txtLastName.Text.Trim();
            var membership = txtMembership.Text.Trim();

            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(last))
            {
                MessageBox.Show("Ime i prezime su obavezni.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (membership.Length != 8 || !long.TryParse(membership, out _))
            {
                MessageBox.Show("Broj članske iskaznice mora biti osmeroznamenkasti broj.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_kupacId.HasValue)
                {
                    var sql = @"UPDATE Kupac SET FirstName=@FirstName, LastName=@LastName, Address=@Address, MembershipNumber=@Membership WHERE KupacId=@Id";
                    var p = new SqlParameter[] {
                        new SqlParameter("@FirstName", first),
                        new SqlParameter("@LastName", last),
                        new SqlParameter("@Address", string.IsNullOrEmpty(txtAddress.Text)? (object)DBNull.Value : txtAddress.Text),
                        new SqlParameter("@Membership", membership),
                        new SqlParameter("@Id", _kupacId.Value)
                    };
                    Database.ExecuteNonQuery(sql, p);
                }
                else
                {
                    var sql = @"INSERT INTO Kupac(FirstName, LastName, Address, MembershipNumber) VALUES(@FirstName, @LastName, @Address, @Membership)";
                    var p = new SqlParameter[] {
                        new SqlParameter("@FirstName", first),
                        new SqlParameter("@LastName", last),
                        new SqlParameter("@Address", string.IsNullOrEmpty(txtAddress.Text)? (object)DBNull.Value : txtAddress.Text),
                        new SqlParameter("@Membership", membership)
                    };
                    Database.ExecuteNonQuery(sql, p);
                }

                DialogResult = true;
                Close();
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                MessageBox.Show("Članski broj mora biti jedinstven. Već postoji kupac s istim članskim brojem.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri spremanju kupca: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
