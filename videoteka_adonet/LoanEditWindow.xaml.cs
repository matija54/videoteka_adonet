using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows;
using videoteka_adonet.Data;

namespace videoteka_adonet
{
    public partial class LoanEditWindow : Window
    {
        public LoanEditWindow()
        {
            InitializeComponent();
            Loaded += LoanEditWindow_Loaded;
        }

        private void LoanEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFilms();
            LoadCustomers();
            dpDate.SelectedDate = DateTime.Now;
        }

        private void LoadFilms()
        {
            try
            {
                var sql = "SELECT FilmId, Code, Name, Quantity FROM Film ORDER BY Name";
                var dt = Database.GetDataTable(sql);
                var table = new DataTable();
                table.Columns.Add("FilmId", typeof(int));
                table.Columns.Add("Display", typeof(string));

                foreach (DataRow r in dt.Rows)
                {
                    var id = (int)r["FilmId"];
                    var code = r["Code"].ToString();
                    var name = r["Name"].ToString();
                    table.Rows.Add(id, $"[{code}] {name}");
                }

                cbFilms.ItemsSource = table.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju filmova: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadCustomers()
        {
            try
            {
                var sql = "SELECT KupacId, FirstName, LastName FROM Kupac ORDER BY LastName, FirstName";
                var dt = Database.GetDataTable(sql);
                var table = new DataTable();
                table.Columns.Add("KupacId", typeof(int));
                table.Columns.Add("Display", typeof(string));

                foreach (DataRow r in dt.Rows)
                {
                    var id = (int)r["KupacId"];
                    var name = r["FirstName"].ToString() + " " + r["LastName"].ToString();
                    table.Rows.Add(id, name);
                }

                cbCustomers.ItemsSource = table.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju kupaca: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbFilms.SelectedValue == null)
            {
                MessageBox.Show("Odaberite film.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (cbCustomers.SelectedValue == null)
            {
                MessageBox.Show("Odaberite kupca.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var filmId = Convert.ToInt32(cbFilms.SelectedValue);
            var kupacId = Convert.ToInt32(cbCustomers.SelectedValue);
            var datum = dpDate.SelectedDate ?? DateTime.Now;

            try
            {
                // check availability
                var sqlCount = "SELECT Quantity - (SELECT COUNT(*) FROM Posudba WHERE FilmId=@FilmId AND DatumVracanja IS NULL) AS Available FROM Film WHERE FilmId=@FilmId";
                var availObj = Database.ExecuteScalar(sqlCount, new SqlParameter("@FilmId", filmId));
                int available = 0;
                if (availObj != null && availObj != DBNull.Value)
                    available = Convert.ToInt32(availObj);

                if (available <= 0)
                {
                    MessageBox.Show("Ne može se izdati film kojeg nema na skladištu.", "Nedostupno", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var insertSql = "INSERT INTO Posudba(FilmId, KupacId, DatumPosudbe) VALUES(@FilmId, @KupacId, @Datum)";
                var p = new SqlParameter[] {
                    new SqlParameter("@FilmId", filmId),
                    new SqlParameter("@KupacId", kupacId),
                    new SqlParameter("@Datum", datum)
                };
                Database.ExecuteNonQuery(insertSql, p);

                DialogResult = true;
                Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Greška pri dodavanju posudbe: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
