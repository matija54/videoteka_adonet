using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Data.SqlClient;
using videoteka_adonet.Data;

namespace videoteka_adonet
{
    public partial class StockReportWindow : Window
    {
        public StockReportWindow()
        {
            InitializeComponent();
            Loaded += StockReportWindow_Loaded;
        }

        private void StockReportWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGenres();
        }

        private void LoadGenres()
        {
            try
            {
                var dt = Database.GetDataTable("SELECT GenreId, Name FROM Genre ORDER BY Name");
                lbGenres.ItemsSource = dt.DefaultView;
                lbGenres.DisplayMemberPath = "Name";
                lbGenres.SelectedValuePath = "GenreId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju žanrova: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selected = lbGenres.SelectedItems.Cast<DataRowView>().Select(r => r["GenreId"].ToString()).ToList();
                string genreFilter = "";
                if (selected.Any())
                {
                    genreFilter = "WHERE f.GenreId IN (" + string.Join(",", selected) + ")";
                }

                var sql = $@"SELECT f.Code AS Sifra, f.Name AS Naziv, g.Name AS Zanr,
                                    f.Quantity - ISNULL((SELECT COUNT(*) FROM Posudba p WHERE p.FilmId = f.FilmId AND p.DatumVracanja IS NULL), 0) AS Kolicina
                              FROM Film f
                              LEFT JOIN Genre g ON f.GenreId = g.GenreId
                              {genreFilter}
                              ORDER BY f.Name";

                var dt = Database.GetDataTable(sql);
                dgStock.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri izvođenju izvještaja: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
