using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MoviesDB
{
    /// <summary>
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        MainWindow win1;
        Window1 win2;
        public Window3(MainWindow w, Window1 w1)
        {
            InitializeComponent();
            win1 = w;
            win2 = w1;
            using (SqliteDbContext context = new SqliteDbContext())
            {
                datagrid1.ItemsSource = context.Halls.ToList();
            }
        }

        private void buttonadd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid1.SelectedItems.Count != 0)
                {
                    List<Hall> halls = new List<Hall>();
                    for(int i = 0; i < win2.datagrid1.Items.Count; i++)
                    {
                        halls.Add((Hall)win2.datagrid1.Items[i]);
                    }
                    for (int i = 0; i < datagrid1.SelectedItems.Count; i++)
                    {
                        halls.Add((Hall)datagrid1.SelectedItems[i]);
                    }
                    win2.datagrid1.ItemsSource = halls;
                }
                Close();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
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
                                   where hall.Hall_Name.ToLower().Contains(textbox1.Text.ToLower())
                                   select new { hm.HallId, hall.Hall_Name };

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
                                   select new { hm.HallId, hall.Hall_Name };

                        datagrid1.ItemsSource = join;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
