using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class NoteManager
    {
        public List<Note> Notes { get; private set; }
        public TagManager TagManager { get; private set; }
        public NoteManager()
        {
            Notes = new List<Note>();
            LoadNotes();
            TagManager = new TagManager(this);
        }
        public void AddNote(Note note)
        {
            if (note == null)
            {
                throw new ArgumentNullException(nameof(note));
            }
            else
            {
                if (note.Content.Length >= 1200)
                {
                    MessageBox.Show("Текст слишком длинный!");
                }
                else
                {
                    bool isMatchContent = Regex.IsMatch(note.Content, @"^[^\s]+$");
                    bool isMatchTitle = Regex.IsMatch(note.Title, @"^[^\s]+$");
                    if (isMatchContent == true && isMatchTitle == true)
                    {
                        Notes.Add(note);
                        SaveNotes();
                    }
                    else
                    {
                        MessageBox.Show("Поля не должны быть пустыми!");
                    }
                }
            }
        }
        public void RemoveNote(Note note)

        {
            if (note == null)
            {
                throw new ArgumentNullException(nameof(note));
            }
            Notes.Remove(note);
            SaveNotes();
        }
        public void SaveNotes()
        {
            List<string> lines = new List<string>();

            foreach (var note in Notes)
            {
                string tagsString = note.Tags.Count > 0
                    ? string.Join(";", note.Tags)
                    : "";

                string line = $"{note.Title}|{note.Content}|{note.Date:yyyy-MM-dd HH:mm:ss}|{tagsString}";
                lines.Add(line);
            }

            File.WriteAllLines("notes.txt", lines);
        }
        private void LoadNotes()
        {
            if (!File.Exists("notes.txt"))
                return;

            var lines = File.ReadAllLines("notes.txt");

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('|');

                if (parts.Length >= 3)
                {
                    if (DateTime.TryParse(parts[2], out DateTime date))
                    {
                        var note = new Note(parts[0], parts[1], date);

                        if (parts.Length > 3 && !string.IsNullOrEmpty(parts[3]))
                        {
                            note.Tags = parts[3].Split(';', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();
                        }

                        Notes.Add(note);
                    }
                }
            }
        }
    }  
}
