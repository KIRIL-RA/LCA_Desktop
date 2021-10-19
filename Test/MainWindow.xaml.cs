using System.Windows;
using System.IO;
using LiveCharts;
using LiveCharts.Wpf;
using Test.Classes;
using System.Diagnostics;
using System.Collections.Generic;

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

        private void Show_data()
        {
            Write_date.Content = csvPickedFile.write_date;
            Main_analize_page.Visibility = Visibility.Visible;

            Write_time.Content = csvPickedFile.write_time;
            Work_time_percent.Content = csvPickedFile.work_time_percent.ToString() + '%';
            Laser_work_time_percent.Content = csvPickedFile.laser_work_time_percent.ToString() + '%';

            Work_time.Content = csvPickedFile.work_time;
            Laser_work_time.Content = csvPickedFile.laser_work_time;

            LineSeries graph_data_work = new LineSeries() { Values = new ChartValues<int>(), Title = "Work time" };
            LineSeries graph_data_laser = new LineSeries() { Values = new ChartValues<int>(), Title = "Laser time" };

            // In zero hour add zero work state
            graph_data_laser.Values.Add(0);
            graph_data_work.Values.Add(0);

            foreach (One_record hour in csvPickedFile.graph_work_hours)
            {
                graph_data_laser.Values.Add(hour.Laser_state);
                graph_data_work.Values.Add(hour.Machine_state);
            }

            WorkGraph.Series.Clear();
            WorkGraph.Series.Add(graph_data_laser);
            WorkGraph.Series.Add(graph_data_work);
        }

        private void Work(string fileName)
        {
            /*
             * Work after choose file
             */
            Trace.WriteLine("-- File choosed succesful: " + fileName);

            File_not_selected1.Visibility = Visibility.Hidden;
            File_not_selected.Visibility = Visibility.Hidden;
            File_selected.Visibility = Visibility.Visible;
            File_name.Content = fileName;

            csvPickedFile.Read_File(fileName);
            csvPickedFile.Parse_File();
            csvPickedFile.Analize();

            Show_data();
        }

        private void Select_file()
        {
            string fileName = ReadPickAnalizeFile.Pick_File();

            if (fileName != null) Work(fileName);
            else Trace.WriteLine("-- File not choosed");
        }

        private void PickFile_Click(object sender, RoutedEventArgs e)
        {
            Select_file();
        }

        private void Drop_file(object sender, DragEventArgs e)
        {
            File_drop.Visibility = Visibility.Hidden;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            Trace.WriteLine("-- File dropped");

            if (Path.GetExtension(files[0]).ToUpperInvariant() == ".CSV") Work(files[0]);
            else Trace.WriteLine("-- File extension not supported!");
        }

        private void Drop_enter(object sender, DragEventArgs e)
        {
            File_drop.Visibility = Visibility.Visible;
        }

        private void Drop_over(object sender, DragEventArgs e)
        {
            File_drop.Visibility = Visibility.Hidden;
        }
    }
}
