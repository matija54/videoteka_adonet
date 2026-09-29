using System.Windows;
using System.Data;
using videoteka_adonet.Data;
using Microsoft.Data.SqlClient;
using System;

namespace videoteka_adonet
{
    public partial class FilmsWindow : Window
    {
        public FilmsWindow()
        {
            InitializeComponent();
            Loaded += FilmsWindow_Loaded;
        }

        private void FilmsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFilms();
        }

        private void LoadFilms()
        {
            try
            {
                var sql = @"SELECT f.FilmId, f.Code, f.Name, g.Name AS GenreName, f.Quantity
                            FROM Film f
                            LEFT JOIN Genre g ON f.GenreId = g.GenreId
                            ORDER BY f.Name";
                var dt = Database.GetDataTable(sql);
                dgFilms.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju filmova: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var w = new FilmEditWindow();
            w.Owner = this;
            if (w.ShowDialog() == true)
                LoadFilms();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedFilm();
        }

        private void dgFilms_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditSelectedFilm();
        }

        private void EditSelectedFilm()
        {
            if (dgFilms.SelectedItem is DataRowView drv)
            {
                var filmId = Convert.ToInt32(drv["FilmId"]);
                var w = new FilmEditWindow(filmId);
                w.Owner = this;
                if (w.ShowDialog() == true)
                    LoadFilms();
            }
            else
            {
                MessageBox.Show("Odaberite film za uređivanje.", "Obavijest", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgFilms.SelectedItem is DataRowView drv)
            {
                var filmId = Convert.ToInt32(drv["FilmId"]);
                var name = drv["Name"].ToString();
                var res = MessageBox.Show($"Obrisati film '{name}'?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        var sql = "DELETE FROM Film WHERE FilmId = @FilmId";
                        var p = new SqlParameter("@FilmId", filmId);
                        Database.ExecuteNonQuery(sql, p);
                        LoadFilms();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Greška pri brisanju filma. Provjerite ima li povezanih posudbi ili drugih ograničenja.\n" + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greška: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Odaberite film za brisanje.", "Obavijest", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
