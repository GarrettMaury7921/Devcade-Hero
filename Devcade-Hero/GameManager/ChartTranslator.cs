using Devcade;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DevcadeGame.GameManager
{
    public class ChartTranslator
    {
        // Attributes
        private List<String> _notes;
        private long default_bpm = 120000; // default BPM value
        private int _resolution = 192; // default resolution value
        private int tsValue;
        private List<int> _bpms;
        private List<int> _bpm_time;
        private List<int> _note_ticks;
        private List<int> _note_color;
        private List<int> _note_length;

        public ChartTranslator(List<String> notes)
        {
            // Attributes
            _notes = notes;
            _bpms = new List<int>();
            _bpm_time = new List<int>();
            _note_ticks = new List<int>();
            _note_color = new List<int>();
            _note_length = new List<int>();

            /*foreach (string _notes in notes)
            {
                Debug.WriteLine(_notes);
            }*/

            // Parse through the formatted .chart file
            foreach (string _notes in notes)
            {
                /* Parse through the TS and extract the number
                 *
                 *   The “TS” key represents a Time Signature event. Time signatures in various Guitar Hero-style games are often used to draw beat lines on the highway, 
                 *   or to determine how long the Starpower mechanic will last for.
	             *   Time Signatures must contain at least 1 value, but may contain up to 2. 
	             *   The first value represents the numerator value of a given time signature. 
	             *   The optional second parameter is stored as the logarithm of the denominator in base 2. 
	             *   If there is no denominator defined, the value is 4 by default.
	             *   For example, if you wanted to store a time signature of 3/8, it would be written as [0 = TS 3 3].
                 *
                 *   SECOND NUMBER NOT IMPLEMENTED YET
                 */
                if (_notes.Contains("= TS"))
                {
                    int index = _notes.IndexOf("TS ");
                    if (index != -1)
                    {
                        string numberString = _notes.Substring(index + 3);
                        int.TryParse(numberString, out tsValue);
                    }
                    // DEBUGGING
                    // Debug.WriteLine(tsValue);
                } // TS 



                /* Parse through the BPM and get all the BPM events
                 *
                 *   The “B” key for an event is used to define a BPM marker, or beats per minute. 
                 *   It has 1 value, a positive integer which is equal to the bpm rate * 1000. 
                 *   For example, 120 bpm will be stored as [0 = B 120000]. 
                 *   This means that bpm is limited to a decimal precision of 3 with this format. 
                 *
                 */
                else if (_notes.Contains("= B"))
                {
                    int firstValue = 0;
                    int lastValue = 0;

                    string[] parts = _notes.Split(' ');
                    if (parts.Length >= 3)
                    {
                        int.TryParse(parts[0], out firstValue);
                        int.TryParse(parts[parts.Length - 1], out lastValue);

                        // Add the values to the lists
                        _bpm_time.Add(firstValue);
                        _bpms.Add(lastValue);
                    }
                    // DEBUGGING
                    /*foreach (int num in _bpm_time)
                    {
                        Debug.WriteLine(num);
                    }
                    foreach (int num in _bpms)
                    {
                        Debug.WriteLine(num);
                    }*/


                } // BPM if statement

                /* Parse through the Events and get all of the regular note events
                 *
                 *   E - Global Events
                 *   Text Events
	             *       These events are simply a string to indicate something at a certain tick.
	             *       They always contain 1 value, and that value is always surrounded by quotes “”. 
	             *       Any quote characters within the event phrase will result in the event loading incorrectly, 
	             *       depending on the program loading it. 
                 *   Sections
	             *       Sections are a subtype of text events and are commonly used by games in practice modes to 
	             *       outline a certain section of notes to play over and over.
	             *       It’s value is always prefixed with the string “section”, followed by the title of the section. 
	             *       For example, a section labeled “Solo 1” would be stored as [112118 = E “section Solo 1”].
                 *   Lyrics
	             *       Similar to sections, lyrics are simply the same but it has the prefix “lyric” instead of “section”. 
	             *       For example, a lyric would be stored as [112118 = E “lyric OOOoooo”]. 
	             *       Lyrics events according to the Clone Hero spec need to start with a “phrase_start” event. 
	             *       A “phase_end” event isn’t strictly required as a new “phase_start” event will automatically end the 
	             *       previous phase.
                 *
                 */
                else if (_notes.Contains("= N"))
                {
                    int firstValue = 0;
                    int secondToLastValue = 0;
                    int lastValue = 0;

                    string[] parts = _notes.Split(' ');
                    if (parts.Length >= 4)
                    {
                        int.TryParse(parts[2], out firstValue);
                        int.TryParse(parts[parts.Length - 2], out secondToLastValue);
                        int.TryParse(parts[parts.Length - 1], out lastValue);
                    }

                    // Add the values to the lists
                    _note_ticks.Add(firstValue);
                    _note_color.Add(secondToLastValue);
                    _note_length.Add(lastValue);

                    Debug.WriteLine(firstValue);
                    Debug.WriteLine(secondToLastValue);
                    Debug.WriteLine(lastValue);

                    // DEBUGGING
                    /*
                     * Debug.WriteLine(firstValue);
                    Debug.WriteLine(secondToLastValue);
                    Debug.WriteLine(lastValue);
                     * 
                     * foreach (int num in _note_ticks)
                    {
                        Debug.WriteLine(num);
                    }
                    foreach (int num in _note_color)
                    {
                        Debug.WriteLine(num);
                    }
                    foreach (int num in _note_length)
                    {
                        Debug.WriteLine(num);
                    }*/


                } // Note if statement

            } // for each statement


        } // Constructor

    } // public class

} // name space
