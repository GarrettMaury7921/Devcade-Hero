using DevcadeHero.States;
using System;
using System.Collections.Generic;
using System.IO;

// CHART FILE SPECIFICATIONS
// https://docs.google.com/document/d/1v2v0U-9HQ5qHeccpExDOLJ5CMPZZ3QytPmAG5WF0Kzs/edit#heading=h.uhxt6eceagq9

namespace DevcadeHero.GameManager
{
    /* 
    Class ChartReader:
        ChartReader Constructor: Gets File path and plays the song
            - contains each button by making a new button class
        @ Initialize Fields Method
        @ GetFilePath Method
        @ Parse Notes Method
    */
    public class ChartReader
    {
        // Attributes
        private List<String> bpm_events_and_notes;
        private StreamReader reader;
        private string currentFile;
        private string currentDirectory;
        private string currentFilePath;
        private string[] files;
        private readonly int difficulty;
        private string difficultyStr;
        private string fileContents;
        private string[] lines;
        private bool body_field;
        private readonly String[] fields;

        // Chart Reader Constructor
        public ChartReader(string fileName)
        {
            // Attributes
            difficulty = MenuState.GetDifficultyID();
            bpm_events_and_notes = new List<String>();
            fields = new string[3];

            // Initialize Fields
            Initialize_fields(difficulty);

            // Initialize the variables and return the path of the file
            GetFilePath(fileName);

            // Read in the file and get the important information from it
            ParseNotes(currentFile);

        } // chart reader constructor

        private void Initialize_fields(int difficulty)
        {
            // Make the difficulty
            switch (difficulty)
            {
                case 0:
                    difficultyStr = "[EasyGHLGuitar]";
                    break;
                case 1:
                    difficultyStr = "[MediumGHLGuitar]";
                    break;
                case 2:
                    difficultyStr = "[HardGHLGuitar]";
                    break;
                case 3:
                    difficultyStr = "[ExpertGHLGuitar]";
                    break;
                default:
                    break;
            }

            fields[0] = "[SyncTrack]";
            fields[1] = "[Events]";
            fields[2] = difficultyStr;

        } // Initialize Fields method

        public List<String> GetNotes()
        {
            return bpm_events_and_notes;
        }

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

        public void ParseNotes(string filePath)
        {
            // Make the stream reader
            reader = new StreamReader(filePath);

            // Read the entire file and then close the reader
            fileContents = reader.ReadToEnd();
            reader.Close();

            // Break up the file into lines
            lines = fileContents.Split('\n');

            // This is for incrementing the fields array manually
            // fields[] contains all the important fields we need to extract from the .chart file
            int j = 0;
            // Go through each line
            for (int i = 0; i < lines.Length; i++)
            {
                // Break the loop if J is at it's limit!
                if (j >= fields.Length)
                {
                    break;
                }

                // Process the line and extract note chart data as needed
                // Debug.WriteLine(lines[i]);

                // Record IMPORTANT BPM/EVENTS/DIFFICULTY NOTE INFORMATION
                if (lines[i].Contains(fields[j]))
                {
                    bpm_events_and_notes.Add(lines[i]);
                    body_field = true;
                    // Make it skip this iteration
                    continue;
                }
                if (body_field)
                {
                    // If there are non bpm, don't add them
                    if (lines[i].Contains('{'))
                    {
                        // Skip
                        continue;
                    }
                    if (lines[i].Contains('}'))
                    {
                        body_field = false;
                        j++;
                        continue;
                    }
                    else
                    {
                        // Add a bpm to the notes
                        bpm_events_and_notes.Add(lines[i]);
                    }
                } // syncTrack if statement

            } // for loop for going through each line in the chart file

            // For checking if it's reading in the files correctly
            /*foreach (string element in bpm_events_and_notes)
            {
                Debug.WriteLine(element);
            }*/
            


        } // Get notes method

    } // public class Chart Reader

} // name space
