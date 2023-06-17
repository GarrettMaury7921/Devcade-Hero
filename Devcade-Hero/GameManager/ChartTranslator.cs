using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;

namespace DevcadeHero.GameManager
{
    // .chart file format for Devcade Scraper
    //  N 4 = blue1
    //  N 5 = blue2
    //  N 6 = blue3
    //  N 7 = blue4
    //  N 0 = red
    //  N 1 = blue5
    //  N 2 = green
    //  N 3 = white

    public class ChartTranslator
    {
        // Attributes
        private bool DEBUG = false;

        private List<String> _notes;
        private int current_bpm;
        private int default_bpm = 120; // default BPM value
        private int _resolution = 192; // default resolution value
        private int tsValue;
        private List<int> _bpms;
        private List<int> _bpm_time;
        private List<int> _note_ticks;
        private List<int> _note_color;
        private List<int> _note_length;

        private int count;
        private int bpm_checker;
        private int last_tick;
        private double note_time;
        private int last_bpm;
        private double last_bpm_time;
        private double current_note_time = 0;
        private double _bpm_fractions = 0;
        private int _bpm_fractions_checker = 0;

        private List<double> _time_between_notes;

        public ChartTranslator(List<String> notes)
        {
            // Attributes
            _notes = notes;
            _bpms = new List<int>();
            _bpm_time = new List<int>();
            _note_ticks = new List<int>();
            _note_color = new List<int>();
            _note_length = new List<int>();
            _time_between_notes = new List<double>();
            current_bpm = default_bpm;
            note_time = 0;
            last_tick = 0;
            bpm_checker = 0;

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

                    string[] parts = _notes.Split(new char[] { ' ', '\n', '\r' },
                        StringSplitOptions.RemoveEmptyEntries);

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

                    /* 
                    DEBUGGING
                    
                    Debug.WriteLine(firstValue);
                    Debug.WriteLine(secondToLastValue);
                    Debug.WriteLine(lastValue);
                    
                    foreach (int num in _note_ticks)
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
                    }
                    */

                    // Find the time between each note
                    /* Parse through the Events and get all of the regular note events
                     *
                     *   The calculation of time in seconds from one tick position to the next at a constant BPM is 
                     *   defined as follows-
                     *
                     *   (tickEnd - tickStart) / resolution * 60.0 (seconds per minute) / bpm
                     *
                     *   Therefore to calculate the time any event takes place requires precalculation 
                     *   of the times between all the BPM events that come before it. BPM events are defined in the 
                     *   [SyncTrack] section as defined below.
                     */
                    count++;

                    if (count == 1)
                    {

                        // Check if the bpm has changed
                        // Debug.WriteLine(bpm_checker + " <= " + _bpm_time.Count + " && " + _bpm_time[bpm_checker] + " >= 0");
                        if (bpm_checker <= _bpm_time.Count && _bpm_time[bpm_checker] >= 0)
                        {
                            current_bpm = _bpms[bpm_checker] / 1000;
                            // Debug.WriteLine("REAL BPM " + current_bpm + "\n");
                            
                            // Tick of the last BPM event
                            last_tick = _bpm_time[bpm_checker];
                            // Debug.WriteLine(last_tick);

                            bpm_checker++;
                        }
                    }
                    else
                    {
                        // Check if the bpm has changed
                        //Debug.WriteLine("BPM CHECKER " + bpm_checker);
                        //Debug.WriteLine(count + " bpm time : " + _bpm_time[bpm_checker]);
                        //Debug.WriteLine(bpm_checker + " <= " + _bpm_time.Count + " && " + _note_ticks.ElementAt(count - 2) + " >= " + _bpm_time[bpm_checker]);
                        if (bpm_checker <= _bpm_time.Count - 1 && _note_ticks.ElementAt(count - 1) >= _bpm_time[bpm_checker])
                        {
                            if (DEBUG)
                                Debug.WriteLine(_bpms[bpm_checker] + " " + _bpms[bpm_checker - 1]);
                        }
                        if (bpm_checker <= _bpm_time.Count - 1 && _note_ticks.ElementAt(count - 1) >= _bpm_time[bpm_checker])
                        {
                            if (_bpms[bpm_checker] != _bpms[bpm_checker - 1])
                            {
                                last_tick = _bpm_time[bpm_checker];

                                // Also get time to the current BPM when it's not 120
                                if (_bpm_fractions_checker == 0)
                                {
                                    last_bpm_time = ((((last_tick) - 0) * 60.0) / current_bpm) / _resolution;
                                    _bpm_fractions += last_bpm_time;
                                    if (DEBUG)
                                        Debug.WriteLine("\nLAST BPM TIME: (" + last_tick + " - 0) * 60.0) / " + current_bpm + ") / " + _resolution + " = " + last_bpm_time);
                                }
                                if (_bpm_fractions_checker > 0)
                                {
                                    last_bpm_time = (((((last_tick) - _bpm_time[bpm_checker - 1]) * 60.0) / current_bpm) / _resolution) + _bpm_fractions;
                                    if (DEBUG)
                                        Debug.WriteLine("\nLAST BPM TIME: (" + last_tick + " - " + _bpm_time[bpm_checker - 1] + ") * 60.0) / " + current_bpm + ") / " + _resolution +  " + " + _bpm_fractions + " = " + last_bpm_time);
                                    _bpm_fractions = last_bpm_time;
                                }
                                _bpm_fractions_checker++;

                                current_bpm = _bpms[bpm_checker] / 1000;
                                if (DEBUG)
                                    Debug.WriteLine("REAL BPM " + current_bpm + "\n");

                                current_note_time = _time_between_notes[count - 2];
                            }

                            bpm_checker++;
                        }
                    }

                    last_bpm = current_bpm;


                    // If you want to calculate the time of a note, tickEnd would be the tick of the note, and
                    // tickStart would be the tick of the last bpm event. That would give you the seconds delta between that
                    // bpm event and the note in seconds.

                    // (tickEnd - tickStart) / resolution * 60.0(seconds per minute) / bpm
                    // Then we are adding the last note time to find the time from the start of the song instead of from the last bpm event
                    note_time = ((((_note_ticks.ElementAt(count - 1) - last_tick) * 60.0) / current_bpm) / _resolution) + last_bpm_time;

                    // Add to the list
                    _time_between_notes.Add(note_time);

                } // Note if statement

            } // for each statement

            // DEBUGGING     
            /*foreach (double num in _bpm_time)
            {
                Debug.WriteLine(num);
            }*/

        } // Constructor

        // ***** METHODS TO OBTAIN VALUES *****
        public List<int> GetBPMTickTime()
        {
            return _bpm_time;
        }
        public List<int> GetBPM()
        {
            return _bpms;
        }
        public List<int> GetNoteTickTime()
        {
            return _note_ticks;
        }
        public List<int> GetNoteColor()
        {
            return _note_color;
        }
        public List<int> GetNoteLength()
        {
            return _note_length;
        }
        public List<double> TimeBetweenNotes()
        {
            return _time_between_notes;
        }
        public int GetNoteCount()
        {
            return GetNoteColor().Count;
        }

        public List<int> FindMultiNotes()
        {
            List<int> note_ticks = GetNoteTickTime();

            // Multi note variable: 0 is a non multi note / 1 is a multi note
            List<int> multi_note = new();

            for (int i = 0; i < note_ticks.Count; i++)
            {
                int currentNote = note_ticks[i];
                bool isMultiNote = false;

                // Check if the current note is the same as the next note(s)
                for (int j = i + 1; j < note_ticks.Count; j++)
                {
                    if (currentNote == note_ticks[j])
                    {
                        isMultiNote = true;
                        break;
                    }
                }

                if (isMultiNote)
                {
                    multi_note.Add(1);
                }
                else
                {
                    multi_note.Add(0);
                }
            }

            // Print out 1s and 0s
            if (DEBUG)
            {
                foreach (int i in multi_note)
                {
                    Debug.WriteLine(i);
                }
            }
            

            return multi_note;
        }


    } // public class

} // name space