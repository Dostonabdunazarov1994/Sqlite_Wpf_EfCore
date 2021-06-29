using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MoviesDB
{
    /// <summary>
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        MainWindow win1;
        Window2 win2;
        public Window4(MainWindow w, Window2 w2)
        {
            InitializeComponent();
            win1 = w;
            win2 = w2;
            using (SqliteDbContext context = new SqliteDbContext())
            {
                datagrid1.ItemsSource = context.Movies.ToList();
            }
        }

        private void buttonadd_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid1.SelectedItems.Count != 0)
            {
                List<Movie> movie = new List<Movie>();
                for(int i = 0; i < win2.datagrid1.Items.Count; i++)
                {
                    movie.Add((Movie)win2.datagrid1.Items[i]);
                }
                for (int i = 0; i < datagrid1.SelectedItems.Count; i++)
                {
                    movie.Add((Movie)datagrid1.SelectedItems[i]);
                }
                win2.datagrid1.ItemsSource = movie;
            }
            Close();
        }

        private void buttoncancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void textbox1_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (textbox1.Text != "")
                {
                    using (SqliteDbContext context = new SqliteDbContext())
                    {
                        var halls = context.Halls.ToList();
                        var movies = context.Movies.ToList();
                        var hms = context.HallsMovies.ToList();

                        var join = from movie in movies
                                   join hm in hms on movie.MovieId equals hm.MovieId
                                   join hall in halls on hm.HallId equals hall.HallId
                                   where movie.Movie_Name.ToLower().Contains(textbox1.Text.ToLower())
                                   select new { hm.MovieId, movie.Movie_Name};

                        datagrid1.ItemsSource = join;
                    }
                }
                else
                {
                    using (SqliteDbContext context = new SqliteDbContext())
                    {
                        var halls = context.Halls.ToList();
                        var movies = context.Movies.ToList();
                        var hms = context.HallsMovies.ToList();

                        var join = from movie in movies
                                   join hm in hms on movie.MovieId equals hm.MovieId
                                   join hall in halls on hm.HallId equals hall.HallId
                                   select new { hm.MovieId, movie.Movie_Name};

                        datagrid1.ItemsSource = join;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
