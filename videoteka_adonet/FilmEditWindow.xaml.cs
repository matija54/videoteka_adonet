using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows;
using videoteka_adonet.Data;

namespace videoteka_adonet
{
    public partial class FilmEditWindow : Window
    {
        private int? _filmId;

        public FilmEditWindow()
        {
            InitializeComponent();
            Loaded += FilmEditWindow_Loaded;
        }

        public FilmEditWindow(int filmId) : this()
        {
            _filmId = filmId;
        }

        private void FilmEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGenres();
            if (_filmId.HasValue)
                LoadFilm(_filmId.Value);
        }

        private void LoadGenres()
        {
            try
            {
                var dt = Database.GetDataTable("SELECT GenreId, Name FROM Genre ORDER BY Name");
                // Allow empty selection
                var dv = dt.DefaultView;
                cbGenre.ItemsSource = dv;
                cbGenre.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju žanrova: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadFilm(int filmId)
        {
            var sql = "SELECT FilmId, Code, Name, GenreId, Description, Quantity FROM Film WHERE FilmId = @FilmId";
            var dt = Database.GetDataTable(sql, new SqlParameter("@FilmId", filmId));
            if (dt.Rows.Count == 0) return;
            var r = dt.Rows[0];
            txtCode.Text = r["Code"].ToString();
            txtName.Text = r["Name"].ToString();
            txtDescription.Text = r["Description"] == DBNull.Value ? string.Empty : r["Description"].ToString();
            txtQuantity.Text = r["Quantity"].ToString();
            if (r["GenreId"] != DBNull.Value)
                cbGenre.SelectedValue = Convert.ToInt32(r["GenreId"]);
            else
                cbGenre.SelectedIndex = -1;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // validations
            var code = txtCode.Text.Trim();
            var name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Šifra ne smije biti prazna.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Naziv ne smije biti prazan.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int qty) || qty < 0)
            {
                MessageBox.Show("Količina mora biti cijeli nenegativan broj.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            object genreVal = cbGenre.SelectedValue ?? (object)DBNull.Value;

            try
            {
                if (_filmId.HasValue)
                {
                    var sql = @"UPDATE Film SET Code=@Code, Name=@Name, GenreId=@GenreId, Description=@Description, Quantity=@Quantity
                                WHERE FilmId=@FilmId";
                    var p = new SqlParameter[] {
                        new SqlParameter("@Code", code),
                        new SqlParameter("@Name", name),
                        new SqlParameter("@GenreId", genreVal),
                        new SqlParameter("@Description", string.IsNullOrEmpty(txtDescription.Text) ? (object)DBNull.Value : txtDescription.Text),
                        new SqlParameter("@Quantity", qty),
                        new SqlParameter("@FilmId", _filmId.Value)
                    };
                    Database.ExecuteNonQuery(sql, p);
                }
                else
                {
                    var sql = @"INSERT INTO Film(Code, Name, GenreId, Description, Quantity) VALUES (@Code, @Name, @GenreId, @Description, @Quantity)";
                    var p = new SqlParameter[] {
                        new SqlParameter("@Code", code),
                        new SqlParameter("@Name", name),
                        new SqlParameter("@GenreId", genreVal),
                        new SqlParameter("@Description", string.IsNullOrEmpty(txtDescription.Text) ? (object)DBNull.Value : txtDescription.Text),
                        new SqlParameter("@Quantity", qty)
                    };
                    Database.ExecuteNonQuery(sql, p);
                }

                DialogResult = true;
                Close();
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                // unique constraint violation (Code unique)
                MessageBox.Show("Šifra mora biti jedinstvena. Već postoji film s istom šifrom.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri spremanju: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
