using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Linq;
using videoteka_adonet.Data;

namespace videoteka_adonet
{
    public partial class LoansWindow : Window
    {
        public LoansWindow()
        {
            InitializeComponent();
            Loaded += LoansWindow_Loaded;
        }

        private void LoansWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLoans();
        }

        private void LoadLoans()
        {
            try
            {
                var sql = @"SELECT p.FilmId, p.KupacId, f.Code, f.Name AS FilmName, (k.FirstName + ' ' + k.LastName) AS CustomerName,
                                    p.DatumPosudbe, p.DatumVracanja
                             FROM Posudba p
                             INNER JOIN Film f ON p.FilmId = f.FilmId
                             INNER JOIN Kupac k ON p.KupacId = k.KupacId
                             ORDER BY p.DatumPosudbe DESC";
                var dt = Database.GetDataTable(sql);
                dgLoans.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju posudbi: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var w = new LoanEditWindow();
            w.Owner = this;
            if (w.ShowDialog() == true)
                LoadLoans();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (dgLoans.SelectedItem is DataRowView drv)
            {
                if (drv["DatumVracanja"] != DBNull.Value)
                {
                    MessageBox.Show("Ova posudba je već vraćena.", "Obavijest", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var filmId = Convert.ToInt32(drv["FilmId"]);
                var kupacId = Convert.ToInt32(drv["KupacId"]);
                var datumPosudbe = Convert.ToDateTime(drv["DatumPosudbe"]);

                var res = MessageBox.Show("Označiti datum vraćanja kao sadašnji?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        var sql = @"UPDATE Posudba SET DatumVracanja = @Now WHERE FilmId=@FilmId AND KupacId=@KupacId AND DatumPosudbe=@DatumPosudbe";
                        var p = new SqlParameter[] {
                            new SqlParameter("@Now", DateTime.Now),
                            new SqlParameter("@FilmId", filmId),
                            new SqlParameter("@KupacId", kupacId),
                            new SqlParameter("@DatumPosudbe", datumPosudbe)
                        };
                        Database.ExecuteNonQuery(sql, p);
                        LoadLoans();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greška pri označavanju vraćanja: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Odaberite posudbu.", "Obavijest", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgLoans.SelectedItem is DataRowView drv)
            {
                var filmId = Convert.ToInt32(drv["FilmId"]);
                var kupacId = Convert.ToInt32(drv["KupacId"]);
                var datumPosudbe = Convert.ToDateTime(drv["DatumPosudbe"]);

                var res = MessageBox.Show("Obrisati odabranu posudbu?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        var sql = "DELETE FROM Posudba WHERE FilmId=@FilmId AND KupacId=@KupacId AND DatumPosudbe=@DatumPosudbe";
                        var p = new SqlParameter[] {
                            new SqlParameter("@FilmId", filmId),
                            new SqlParameter("@KupacId", kupacId),
                            new SqlParameter("@DatumPosudbe", datumPosudbe)
                        };
                        Database.ExecuteNonQuery(sql, p);
                        LoadLoans();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greška pri brisanju posudbe: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Odaberite posudbu.", "Obavijest", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void dgLoans_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // double-click to toggle return if not returned
            if (dgLoans.SelectedItem is DataRowView drv && drv["DatumVracanja"] == DBNull.Value)
            {
                btnReturn_Click(sender, null);
            }
        }
    }
}
