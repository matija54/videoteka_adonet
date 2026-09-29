using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.Data.SqlClient;
using videoteka_adonet.Data;

namespace videoteka_adonet
{
    public partial class CustomerCardWindow : Window
    {
        public CustomerCardWindow()
        {
            InitializeComponent();
            Loaded += CustomerCardWindow_Loaded;
        }

        private void CustomerCardWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCustomers();
            dpFrom.SelectedDate = DateTime.Now.AddMonths(-1);
            dpTo.SelectedDate = DateTime.Now;
        }

        private void LoadCustomers()
        {
            try
            {
                var dt = Database.GetDataTable("SELECT KupacId, FirstName, LastName FROM Kupac ORDER BY LastName, FirstName");
                var table = new DataTable();
                table.Columns.Add("KupacId", typeof(int));
                table.Columns.Add("Display", typeof(string));
                foreach (DataRow r in dt.Rows)
                {
                    table.Rows.Add((int)r["KupacId"], r["FirstName"].ToString() + " " + r["LastName"].ToString());
                }
                cbCustomers.ItemsSource = table.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju kupaca: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if (cbCustomers.SelectedValue == null)
            {
                MessageBox.Show("Odaberite kupca.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var kupacId = Convert.ToInt32(cbCustomers.SelectedValue);
            var from = dpFrom.SelectedDate ?? DateTime.MinValue;
            var to = dpTo.SelectedDate ?? DateTime.MaxValue;

            try
            {
                var sql = @"SELECT f.Code AS Sifra, f.Name AS Naziv, (k.FirstName + ' ' + k.LastName) AS Kupac, p.DatumPosudbe, p.DatumVracanja,
                                    g.Name AS Zanr,
                                    CASE WHEN p.DatumVracanja IS NULL THEN 'Posuđeno' ELSE 'Vraćeno' END AS Status
                             FROM Posudba p
                             INNER JOIN Film f ON p.FilmId = f.FilmId
                             INNER JOIN Kupac k ON p.KupacId = k.KupacId
                             LEFT JOIN Genre g ON f.GenreId = g.GenreId
                             WHERE p.KupacId = @KupacId AND p.DatumPosudbe BETWEEN @From AND @To
                             ORDER BY p.DatumPosudbe DESC";

                var p = new SqlParameter[] {
                    new SqlParameter("@KupacId", kupacId),
                    new SqlParameter("@From", from),
                    new SqlParameter("@To", to)
                };

                var dt = Database.GetDataTable(sql, p);

                // Create view and group by Status then Zanr
                var dv = dt.DefaultView;
                var lcv = new ListCollectionView(dv.ToTable().DefaultView);
                lcv.GroupDescriptions.Add(new PropertyGroupDescription("Status"));
                lcv.GroupDescriptions.Add(new PropertyGroupDescription("Zanr"));
                lcv.SortDescriptions.Add(new SortDescription("DatumPosudbe", ListSortDirection.Descending));

                dgCard.ItemsSource = lcv;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri izvođenju izvještaja: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
