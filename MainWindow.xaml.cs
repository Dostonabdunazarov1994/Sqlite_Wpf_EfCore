using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MoviesDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            try
            {
                using (SqliteDbContext context = new SqliteDbContext())
                {
                    context.Database.EnsureCreated();
                    datagrid1.ItemsSource = context.Movies.ToList();
                    datagrid2.ItemsSource = context.Halls.ToList();
                }
            }
            catch(Exception ex)  {MessageBox.Show( ex.Message); }
        }

        public bool b;
        public bool b1;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            b = false;
            Window1 w1 = new Window1(this);
            w1.Show();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid1.SelectedItem != null)
                {
                    Movie m1 = (Movie)datagrid1.SelectedItem;
                    Dialog dialog = new Dialog(m1.Movie_Name);
                    if (dialog.ShowDialog() == true)
                    {
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            var mov = context.Movies.Include(c => c.Halls).ToList();
                            for(int a = 0; a< datagrid1.SelectedItems.Count; a++)
                            {
                                Movie m = (Movie)datagrid1.SelectedItems[a];
                                for (int i = 0; i < mov.Count; i++)
                                {
                                    if (m.MovieId == mov[i].MovieId)
                                    {
                                        context.Movies.Remove(mov[i]);
                                        break;
                                    }
                                }
                            }
                            
                            context.SaveChanges();
                            datagrid1.ItemsSource = context.Movies.ToList();
                        }
                    }
                }
                else
                    MessageBox.Show("   Choose item  ");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid1.SelectedItem != null)
            {
                b = true;
                Window1 win1 = new Window1(this);
                win1.Show();
            }
            else
            {
                MessageBox.Show("    Choose Movie     ");
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            b1 = false;
            Window2 win2 = new Window2(this);
            win2.Show();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid2.SelectedItem != null)
                {
                    Hall h = (Hall)datagrid2.SelectedItem;
                    Dialog dialog = new Dialog(h.Hall_Name);
                    if (dialog.ShowDialog() == true)
                    {
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            var hall = context.Halls.Include(j => j.Movies).ToList();
                            for (int i = 0; i < hall.Count; i++)
                            {
                                if (hall[i].HallId == h.HallId)
                                {
                                    context.Halls.Remove(hall[i]);
                                }
                            }
                            context.SaveChanges();
                            datagrid2.ItemsSource = context.Halls.ToList();
                        }
                    }
                }
                else
                    MessageBox.Show("   Choose item  ");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid2.SelectedItem != null)
            {
                b1 = true;
                Window2 win2 = new Window2(this);
                win2.Show();
            }
            else
            {
                MessageBox.Show("    Choose Hall    ");
            }
        }

        private void textboxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                using (SqliteDbContext context = new SqliteDbContext())
                {
                    datagrid1.ItemsSource = context.Movies.Where(b => b.Movie_Name.ToLower().Contains(textboxSearch.Text.ToLower())).ToList();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void textboxsearch2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                using (SqliteDbContext context = new SqliteDbContext())
                {
                    datagrid2.ItemsSource = context.Halls.Where(b => b.Hall_Name.ToLower().Contains(textboxsearch2.Text.ToLower())).ToList();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqliteDbContext context = new SqliteDbContext())
                {
                    var halls = context.Halls.ToList();
                    var movies = context.Movies.ToList();
                    var hms = context.HallsMovies.ToList();

                    var innerjoin = from movie in movies
                                    join hm in hms on movie.MovieId equals hm.MovieId
                                    join hall in halls on hm.HallId equals hall.HallId
                                    select new { hm.MovieId, movie.Movie_Name, hm.HallId, hall.Hall_Name };

                    datagrid3.ItemsSource = innerjoin;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void textboxsearch3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                textboxsearch4.Text = "";
                if (textboxsearch3.Text != "")
                {
                    using (SqliteDbContext context = new SqliteDbContext())
                    {
                        var halls = context.Halls.ToList();
                        var movies = context.Movies.ToList();
                        var hms = context.HallsMovies.ToList();

                        var join = from movie in movies
                                        join hm in hms on movie.MovieId equals hm.MovieId
                                        join hall in halls on hm.HallId equals hall.HallId
                                        where movie.MovieId == Convert.ToInt32(textboxsearch3.Text)
                                        select new { hm.MovieId, movie.Movie_Name, hm.HallId, hall.Hall_Name };

                        datagrid3.ItemsSource = join;
                    }
                }
                else
                {
                    refresh_Click(sender, e);
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
           
        }

        private void textboxsearch4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                textboxsearch3.Text = "";
                if (textboxsearch4.Text != "")
                {
                    using (SqliteDbContext context = new SqliteDbContext())
                    {
                        var halls = context.Halls.ToList();
                        var movies = context.Movies.ToList();
                        var hms = context.HallsMovies.ToList();

                        var join = from movie in movies
                                        join hm in hms on movie.MovieId equals hm.MovieId
                                        join hall in halls on hm.HallId equals hall.HallId
                                        where hall.HallId == Convert.ToInt32(textboxsearch4.Text)
                                        select new { hm.MovieId, movie.Movie_Name, hm.HallId, hall.Hall_Name };

                        datagrid3.ItemsSource = join;
                    }
                }
                else
                {
                    refresh_Click(sender, e);
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void datagrid2_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            button6_Click(sender, e);
        }

        private void datagrid1_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            button3_Click(sender, e);
        }

        private void textbox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                textbox6.Text = "";
                if (textbox5.Text != "")
                {
                    using (SqliteDbContext context = new SqliteDbContext())
                    {
                        var halls = context.Halls.Include(c => c.Movies).ToList();
                        var movies = context.Movies.Include(j => j.Halls).ToList();
                        var hms = context.HallsMovies.ToList();

                        var join = from movie in movies
                                   join hm in hms on movie.MovieId equals hm.MovieId
                                   join hall in halls on hm.HallId equals hall.HallId
                                   where movie.Movie_Name.ToLower().Contains(textbox5.Text.ToLower())
                                   select new { hm.MovieId, movie.Movie_Name, hm.HallId, hall.Hall_Name };

                        datagrid3.ItemsSource = join;
                    }
                }
                else
                {
                    refresh_Click(sender, e);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void textbox6_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                textbox5.Text = "";
                if (textbox6.Text != "")
                {
                    using (SqliteDbContext context = new SqliteDbContext())
                    {
                        var halls = context.Halls.ToList();
                        var movies = context.Movies.ToList();
                        var hms = context.HallsMovies.ToList();

                        var join = from movie in movies
                                   join hm in hms on movie.MovieId equals hm.MovieId
                                   join hall in halls on hm.HallId equals hall.HallId
                                   where hall.Hall_Name.ToLower().Contains(textbox6.Text.ToLower())
                                   select new { hm.MovieId, movie.Movie_Name, hm.HallId, hall.Hall_Name };

                        datagrid3.ItemsSource = join;
                    }
                }
                else
                {
                    refresh_Click(sender, e);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

    }
}
