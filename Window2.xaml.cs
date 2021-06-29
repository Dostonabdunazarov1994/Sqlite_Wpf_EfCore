using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace MoviesDB
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        MainWindow win;
        public Window2(MainWindow w)
        {
            InitializeComponent();
            win = w;
            try
            {
                if (win.b1)
                {
                    buttonAdd.Content = "Edit";
                    Hall h = (Hall)win.datagrid2.SelectedItem;
                    textbox1.Text = h.Hall_Name;
                    textbox2.Text = h.Place_count.ToString();

                    Hall h1 = (Hall)win.datagrid2.SelectedItem;
                    using (SqliteDbContext context = new SqliteDbContext())
                    {
                        var mov = context.Halls.Include(c => c.Movies).ToList();
                        for (int i = 0; i < mov.Count; i++)
                        {
                            if (mov[i].HallId == h1.HallId)
                            {
                                datagrid1.ItemsSource = mov[i].Movies.ToList();
                                break;
                            }
                        }
                    }
                }
                else
                {
                    buttonAdd.Content = "Save";
                    textbox1.Text = "";
                    textbox2.Text = "";
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!win.b1)
                {
                    if (textbox1.Text == "" || textbox2.Text == "")
                    {
                        //MessageBox.Show("    Заполните поля    ");
                        throw new Exception("Заполните поля");
                    }
                    if(Convert.ToInt32(textbox2.Text) < 0)
                    {
                        throw new Exception("Кол-во мест должно быть больше 0");
                    }
                    using (SqliteDbContext context = new SqliteDbContext())
                    {
                        Hall h = new Hall() { Hall_Name = textbox1.Text, Place_count = Convert.ToInt32(textbox2.Text) };
                        var mov = context.Movies.Include(c => c.Halls).ToList();
                        for (int i = 0; i < mov.Count; i++)
                        {
                            for (int j = 0; j < datagrid1.Items.Count; j++)
                            {
                                Movie m = (Movie)datagrid1.Items[j];
                                if (mov[i].MovieId == m.MovieId)
                                {
                                    h.Movies.Add(mov[i]);
                                }
                            }
                        }
                        context.Halls.Add(h);
                        context.SaveChanges();
                        win.datagrid2.ItemsSource = context.Halls.ToList();
                        win.datagrid1.ItemsSource = context.Movies.ToList();
                    }
                    Close();
                }
                else
                {
                    if (win.datagrid2.SelectedItem != null)
                    {
                        if (textbox1.Text == "" || textbox2.Text == "")
                        {
                            //MessageBox.Show("    Заполните поля    ");
                            throw new Exception("Заполните поля");
                        }
                        if (Convert.ToInt32(textbox2.Text) < 0)
                        {
                            throw new Exception("Кол-во мест должно быть больше 0");
                        }
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            Hall h = (Hall)win.datagrid2.SelectedItem;
                            var hall = context.Halls.Include(c => c.Movies).ToList();
                            var mov = context.Movies.Include(c => c.Halls).ToList();

                            for (int i = 0; i < hall.Count; i++)
                            {
                                if (hall[i].HallId == h.HallId)
                                {
                                    hall[i].Hall_Name = textbox1.Text;
                                    hall[i].Place_count = Convert.ToInt32(textbox2.Text);
                                    hall[i].Movies.Clear();
                                    for (int p = 0; p < mov.Count; p++)
                                    {
                                        for (int j = 0; j < datagrid1.Items.Count; j++)
                                        {
                                            Movie m1 = (Movie)datagrid1.Items[j];
                                            if (mov[p].MovieId == m1.MovieId)
                                            {
                                                hall[i].Movies.Add(mov[p]);
                                            }
                                        }
                                    }
                                    context.Halls.Update(hall[i]);
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

        private void addmovie_Click(object sender, RoutedEventArgs e)
        {
            Window4 win4 = new Window4(win, this);
            win4.Show();
        }

        private void delmovie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid1.SelectedItem != null)
                {
                    Dialog d = new Dialog(((Movie)datagrid1.SelectedItem).Movie_Name);
                    if (d.ShowDialog() == true)
                    {
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            Hall h = (Hall)win.datagrid2.SelectedItem;
                            var hall = context.Halls.Include(c => c.Movies).ToList();
                            var mov = context.Movies.Include(c => c.Halls).ToList();

                            for (int i = 0; i < hall.Count; i++)
                            {
                                if (hall[i].HallId == h.HallId)
                                {
                                    for (int p = 0; p < mov.Count; p++)
                                    {
                                        if (mov[p].MovieId == ((Movie)datagrid1.SelectedItem).MovieId)
                                        {
                                            hall[i].Movies.Remove(mov[p]);
                                        }
                                    }
                                    context.Halls.Update(hall[i]);
                                    datagrid1.ItemsSource = hall[i].Movies.ToList();
                                    break;
                                }
                            }
                            context.SaveChanges();
                        }
                    }
                }
                else MessageBox.Show("  Choose Item!  ");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
