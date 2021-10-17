using Microsoft.Win32;
using System.Diagnostics;
using System;

namespace Test.Classes
{
    struct Analized_line
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

        public OpenFileDialog openFileDialogCSV;

        public string fileName = null;
        public string write_date = null;

        public string[] lines = null;
        public string[] write_dates = null;

        public int lines_count = 0;

        TimeSpan write_time;
        public Analized_line[] analized_file = null;

        public ReadPickAnalizeFile()
        {
            openFileDialogCSV = new OpenFileDialog();

            openFileDialogCSV.InitialDirectory = "c:\\";
            openFileDialogCSV.Filter = "Log files |*.csv";
        }

        public bool? Pick_File()
        {
            /* 
             * Funtion for open dialog to choose log file
             */

            Trace.WriteLine("-- Open file dialog");
            bool? isFileOpened = openFileDialogCSV.ShowDialog();
            fileName = openFileDialogCSV.FileName;

            return isFileOpened;
        }

        public bool Read_File()
        {
            /*
             * Function for reading choosed file
             */

            if (fileName != null)
            {
                lines = System.IO.File.ReadAllLines(fileName);
                lines_count = lines.Length;
                Trace.WriteLine("-- Readed " + lines_count + " lines");

                return true;
            }
            else
            {
                Trace.WriteLine("-- File not readed");
                return false;
            }
        }

        public bool Parse_File()
        {
            if (lines != null)
            {
                Trace.WriteLine("-- Parse file...");
                analized_file = new Analized_line[lines_count];

                for (int line = 0; line < lines_count; line++)
                {
                    string[] data = lines[line].Split(',');

                    analized_file[line].Date = data[0];
                    try
                    {
                        analized_file[line].Machine_state = Int32.Parse(data[1]);
                        analized_file[line].Laser_state = Int32.Parse(data[2]);
                    }
                    catch(FormatException e)
                    {
                        Trace.WriteLine("-- Error parse: " + e.Message);
                        return false;
                    }
                }

                
                Trace.WriteLine("-- Parsed " + analized_file.Length + " lines");
                return true;
            }
            else
            {
                Trace.WriteLine("--Parse not started");
                return false;
            }
        }

        public void Analize()
        {
            write_dates = new string[2] {analized_file[0].Date,  analized_file[analized_file.Length-1].Date};
            write_date = write_dates[0] + " - " + write_dates[1];
            Trace.WriteLine("-- Log dates: " + write_date);

            write_time = TimeSpan.FromSeconds(lines_count);
            Trace.WriteLine("-- Write time: " + write_time);
        }
    }
}
