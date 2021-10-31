using Microsoft.Win32;
using System.Diagnostics;
using System;

namespace Test.Classes
{
    struct One_record
    {
        /*
         * structure containing data from one record
         */

        public string Date;
        public int Machine_state;
        public int Laser_state;
    }

    class ReadPickAnalizeFile
    {
        /*
         * ReadPickFile class
         * Class containing functions for opening, reading, parse and analize log files
         */

        public string write_date = null;

        public string[] lines = null;
        public string[] write_dates = null;

        public int lines_count = 0;
        public int work_time_percent = 0;
        public int laser_work_time_percent = 0;
        private int work_time_ = 0;
        private int laser_work_time_ = 0;

        public TimeSpan write_time;
        public TimeSpan work_time;
        public TimeSpan laser_work_time;
        public One_record[] graph_work_hours;
        private One_record[] analized_file = null;

        public ReadPickAnalizeFile()
        {
            

            
        }

        public static string Pick_File()
        {
            /* 
             * Funtion for open dialog to choose log file
             */
            
            OpenFileDialog openFileDialogCSV = new OpenFileDialog();

            openFileDialogCSV.InitialDirectory = "c:\\";
            openFileDialogCSV.Filter = "Log files |*.csv";

            Trace.WriteLine("-- Open file dialog");
            bool? isFileOpened = openFileDialogCSV.ShowDialog();

            if(isFileOpened == true) return openFileDialogCSV.FileName;
            return null;
        }

        public bool Read_File(string fileName)
        {
            /*
             * Function for reading choosed file
             */
            try
            {
                lines = System.IO.File.ReadAllLines(fileName);
                lines_count = lines.Length;
                Trace.WriteLine("-- Readed " + lines_count + " lines");

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Parse_File()
        {
            /*
             * Function to find dates and states in a file
             */
            Trace.WriteLine("-- Parse file...");
            analized_file = new One_record[lines_count];

            for (int line = 0; line < lines_count; line++)
            {
                string[] data = lines[line].Split(',');

                analized_file[line].Date = data[0];
                try
                {
                    analized_file[line].Machine_state = int.Parse(data[1]);
                    analized_file[line].Laser_state = int.Parse(data[2]);
                }
                catch
                {
                    Trace.WriteLine("-- Error parse");
                    return false;
                }
            }


            Trace.WriteLine("-- Parsed " + analized_file.Length + " lines");
            return true;
        }

        public bool Analize()
        {
            /*
             * Function for find write date, write time, work time, laser work time
             */
            try
            {
                write_dates = new string[2] { analized_file[0].Date, analized_file[analized_file.Length - 1].Date };
                write_date = write_dates[0] + " - " + write_dates[1];
                Trace.WriteLine("-- Log dates: " + write_date);

                write_time = TimeSpan.FromSeconds(lines_count);
                Trace.WriteLine("-- Write time: " + write_time);

                laser_work_time_ = work_time_ = 0;

                foreach (One_record line in analized_file)
                {
                    if (line.Laser_state == 1) laser_work_time_++;
                    if (line.Machine_state == 1) work_time_++;
                }

                work_time = TimeSpan.FromSeconds(work_time_);
                laser_work_time = TimeSpan.FromSeconds(laser_work_time_);

                work_time_percent = (int)Math.Round((double)((double)work_time_ / (double)lines_count) * 100);
                laser_work_time_percent = (int)Math.Round((double)((double)laser_work_time_ / (double)lines_count) * 100);

                graph_work_hours = new One_record[write_time.Hours];
                for (int hour = 0; hour < graph_work_hours.Length; hour++)
                {
                    int laser_work = 0;
                    int machine_work = 0;

                    // 3600 - 1 hour in second. We iterate over all records by the hour to determine the activity for each hour
                    for (int second = hour * 3600; second < (hour + 1) * 3600; second++)
                    {
                        if (analized_file[second].Laser_state == 1) laser_work++;
                        if (analized_file[second].Machine_state == 1) machine_work++;
                    }

                    graph_work_hours[hour].Laser_state = (int)Math.Round((double)((double)laser_work / 3600) * 100);
                    graph_work_hours[hour].Machine_state = (int)Math.Round((double)((double)machine_work / 3600) * 100);
                    graph_work_hours[hour].Date = hour.ToString();
                }

                Trace.WriteLine("-- Work time: " + work_time + " (" + work_time_percent + ')');
                Trace.WriteLine("-- Laser work time: " + laser_work_time + " (" + laser_work_time_percent + ')');
                Trace.WriteLine("-- Work in hours");
                foreach (One_record hour in graph_work_hours)
                {
                    Trace.WriteLine("--- Time: " + hour.Date + " -Laser: " + hour.Laser_state + "-Work: " + hour.Machine_state);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
