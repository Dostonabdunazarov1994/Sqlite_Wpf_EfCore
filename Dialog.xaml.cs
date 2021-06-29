using System;
using System.Windows;

namespace MoviesDB
{
    /// <summary>
    /// Логика взаимодействия для Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        string str;
        public Dialog(string s)
        {
            InitializeComponent();
            str = s;
            label1.Text = $"Вы действительно хотите удалить {s}?";
        }

        private void but1_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
