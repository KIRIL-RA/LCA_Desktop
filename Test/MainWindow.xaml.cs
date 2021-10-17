using System.Windows;
using System.IO;
using Test.Classes;
using System.Diagnostics;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ReadPickAnalizeFile csvPickedFile;

        public MainWindow()
        {
            InitializeComponent();

            csvPickedFile = new ReadPickAnalizeFile();
        }

        private void Select_file()
        {
            bool? isOpened = csvPickedFile.Pick_File();

            if (isOpened == true)
            {
                Trace.WriteLine("-- File choosed succesful: " + csvPickedFile.fileName);

                File_not_selected1.Visibility = Visibility.Hidden;
                File_not_selected.Visibility = Visibility.Hidden;
                File_selected.Visibility = Visibility.Visible;
                File_name.Content = csvPickedFile.fileName;

                csvPickedFile.Read_File();
                csvPickedFile.Parse_File();
                csvPickedFile.Analize();

                Write_date.Content = csvPickedFile.write_date;
                Main_analize_page.Visibility = Visibility.Visible;
            }
            else Trace.WriteLine("-- File not choosed");

        }

        private void PickFileInfoMenu_Click(object sender, RoutedEventArgs e)
        {
            Select_file();
        }

        private void PickFileStartPage_Click(object sender, RoutedEventArgs e)
        {
            Select_file();
        }
    }
}
