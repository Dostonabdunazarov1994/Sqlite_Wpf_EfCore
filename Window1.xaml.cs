using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MoviesDB
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        MainWindow win;
        public Window1(MainWindow w1)
        {
            InitializeComponent();
            win = w1;
            try
            {
                if (win.b)
                {
                    buttonAdd.Content = "Edit";
                    Movie movie = (Movie)win.datagrid1.SelectedItem;
                    textbox1.Text = movie.Movie_Name;
                    startdatatime.Text = movie.start_datetime.ToString();

                        Movie m = (Movie)win.datagrid1.SelectedItem;
                    using (SqliteDbContext context = new SqliteDbContext())
                    {
                        var mov = context.Movies.Include(c => c.Halls).ToList();
                        for (int i = 0; i < mov.Count; i++)
                        {
                            if (mov[i].MovieId == m.MovieId)
                            {
                                datagrid1.ItemsSource = mov[i].Halls.ToList();
                                break;
                            }
                        }
                    }
                }
                else
                {
                    buttonAdd.Content = "Save";
                    textbox1.Text = "";
                    startdatatime.Text = "";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!win.b)
                {
                    if (textbox1.Text == "" || startdatatime.Text == "")
                    {
                        throw new Exception("Заполните поля ");
                    }
                    using (SqliteDbContext context = new SqliteDbContext())
                    {
                        Movie m = new Movie() { Movie_Name = textbox1.Text, start_datetime = (DateTime)startdatatime.Value };
                        var mov = context.Halls.Include(c => c.Movies).ToList();
                        for (int i = 0; i < mov.Count; i++)
                        {
                            for (int j = 0; j < datagrid1.Items.Count; j++)
                            {
                                Hall h = (Hall)datagrid1.Items[j];
                                if (mov[i].HallId == h.HallId)
                                {
                                    m.Halls.Add(mov[i]);
                                }
                            }
                        }
                        context.Movies.Add(m);
                        context.SaveChanges();
                        win.datagrid1.ItemsSource = context.Movies.ToList();
                        win.datagrid2.ItemsSource = context.Halls.ToList();
                    }
                    Close();
                }
                else
                {
                    if (win.datagrid1.SelectedItem != null)
                    {
                        if (textbox1.Text == "" || startdatatime.Text == "")
                        {
                            throw new Exception("Заполните поля ");
                        }
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            Movie m = (Movie)win.datagrid1.SelectedItem;
                            var hall = context.Halls.Include(c => c.Movies).ToList();
                            var mov = context.Movies.Include(c => c.Halls).ToList();

                            for (int i = 0; i < mov.Count; i++)
                            {
                                if (mov[i].MovieId == m.MovieId)
                                {
                                    mov[i].Movie_Name = textbox1.Text;
                                    mov[i].start_datetime = (DateTime)startdatatime.Value;
                                    mov[i].Halls.Clear();
                                    for (int p = 0; p < hall.Count; p++)
                                    {
                                        for (int j = 0; j < datagrid1.Items.Count; j++)
                                        {
                                            Hall h = (Hall)datagrid1.Items[j];
                                            if (hall[p].HallId == h.HallId)
                                            {
                                                mov[i].Halls.Add(hall[p]);
                                            }
                                        }
                                    }
                                    context.Movies.Update(mov[i]);
                                    break;
                                }
                            }
                            context.SaveChanges();
                            win.datagrid1.ItemsSource = context.Movies.ToList();
                            win.datagrid2.ItemsSource = context.Halls.ToList();
                        }
                        Close();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void addhall_Click(object sender, RoutedEventArgs e)
        {
            Window3 win3 = new Window3(win, this);
            win3.Show();
        }

        private void delhall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid1.SelectedItem != null)
                {
                    Dialog d = new Dialog(((Hall)datagrid1.SelectedItem).Hall_Name);
                    if (d.ShowDialog() == true)
                    {
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            Movie m = (Movie)win.datagrid1.SelectedItem;
                            var hall = context.Halls.Include(c => c.Movies).ToList();
                            var mov = context.Movies.Include(c => c.Halls).ToList();

                            for (int i = 0; i < mov.Count; i++)
                            {
                                if (mov[i].MovieId == m.MovieId)
                                {
                                    for (int p = 0; p < hall.Count; p++)
                                    {
                                        if (hall[p].HallId == ((Hall)datagrid1.SelectedItem).HallId)
                                        {
                                            mov[i].Halls.Remove(hall[p]);
                                        }
                                    }
                                    context.Movies.Update(mov[i]);
                                    datagrid1.ItemsSource = mov[i].Halls.ToList();
                                    break;
                                }
                            }
                            context.SaveChanges();
                        }
                    }
                }
                else 
                    MessageBox.Show("  Choose Item!  ");
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
