using DevcadeGame.States;
using System;
using System.IO;

namespace DevcadeGame.GameManager
{
    /* 
    Class ChartReader:
        ChartReader Constructor: Gets File path and plays the song
            - contains each button by making a new button class
        @ GetFilePath Method
    */
    public class ChartReader
    {
        // Attributes
        private string currentFile;
        private string currentDirectory;
        private string currentFilePath;
        private string[] files;
        private int gameMode;
        private int difficulty;

        // Chart Reader Constructor
        public ChartReader(string fileName)
        {
            gameMode = MenuState.GetGameID();
            difficulty = MenuState.GetDifficultyID();

            // Initialize the variables and return the path of the file
            GetFilePath(fileName);

        } // chart reader constructor

        private string GetFilePath(string fileName)
        {
            // Check to see if the file is real
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            // Get path
            currentDirectory = Directory.GetCurrentDirectory();
            currentFilePath = Path.Combine(currentDirectory, "Charts");

            // Get all of the .chart files in the chart directory
            files = Directory.GetFiles(currentFilePath, fileName + ".chart", SearchOption.TopDirectoryOnly);

            // Go through all the files and find the one we need
            for (int i = 0; i < files.Length; i++)
            {
                currentFile = files[i];
                if (currentFile.Contains(fileName))
                {
                    // The correct file has been found
                    // Debug.WriteLine(currentFile); 
                    break;
                }
                else
                {
                    // File not found
                    throw new FileNotFoundException(nameof(fileName));
                }
            } // for loop

            return currentFile;
        } // Get File Path Method

        public void GetNotes(string fileName)
        {
            
        }

    } // public class Chart Reader

} // name space
