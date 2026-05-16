using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class NoteForm : Form
    {
        private NoteManager noteManager;
        private TagManager tagManager;

        private TextBox titleTextBox;
        private TextBox contentTextBox;
        private Button addNoteButton;
        private ListBox notesListBox;
        private Button removeNoteButton;

       
        private TextBox tagTextBox;
        private Button addTagButton;
        private ListBox currentTagsListBox;
        private Button removeTagButton;
        private ComboBox filterComboBox;
        private Button filterButton;

        public NoteForm()
        {
            this.Text = "Управление заметками";

            this.Width = 517;
            this.Height = 580;   

            titleTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 10),
                Width = 200,
                PlaceholderText = "Заголовок"
            };
            contentTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 40),
                Width = 200,
                Height = 100,
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                PlaceholderText = "Содержание"
            };
            addNoteButton = new Button
            {
                Location = new System.Drawing.Point(10, 150),
                Text = "Добавить",
                Width = 100
            };
            addNoteButton.Click += AddNoteButton_Click;

            notesListBox = new ListBox
            {
                Location = new System.Drawing.Point(220, 10),
                Width = 270,
                Height = 200
            };
            notesListBox.SelectedIndexChanged += NotesListBox_SelectedIndexChanged;

            removeNoteButton = new Button
            {
                Location = new System.Drawing.Point(220, 220),
                Text = "Удалить",
                Width = 100
            };
            removeNoteButton.Click += RemoveNoteButton_Click;

            this.Controls.Add(titleTextBox);
            this.Controls.Add(contentTextBox);
            this.Controls.Add(addNoteButton);
            this.Controls.Add(notesListBox);
            this.Controls.Add(removeNoteButton);

            noteManager = new NoteManager();
            tagManager = noteManager.TagManager;

            InitializeTagControls();
            UpdateNotesList();
            UpdateFilterComboBox();
        }

        private void InitializeTagControls()
        {
            tagTextBox = new TextBox
            {
                Location = new Point(10, 268),
                Width = 180,
                PlaceholderText = "Новый тег"
            };

            addTagButton = new Button
            {
                Location = new Point(200, 268),
                Text = "Добавить тег",
                Width = 100
            };
            addTagButton.Click += AddTagButton_Click;

            currentTagsListBox = new ListBox
            {
                Location = new Point(10, 330),
                Width = 240,
                Height = 130
            };

            removeTagButton = new Button
            {
                Location = new Point(310, 300),
                Text = "Удалить тег",
                Width = 120
            };
            removeTagButton.Click += RemoveTagButton_Click;

            filterComboBox = new ComboBox
            {
                Location = new Point(10, 300),
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            filterButton = new Button
            {
                Location = new Point(200, 300),
                Text = "Фильтр",
                Width = 80
            };
            filterButton.Click += FilterButton_Click;

            this.Controls.Add(tagTextBox);
            this.Controls.Add(addTagButton);
            this.Controls.Add(currentTagsListBox);
            this.Controls.Add(removeTagButton);
            this.Controls.Add(filterComboBox);
            this.Controls.Add(filterButton);
        }

        private void UpdateNotesList()
        {
            notesListBox.Items.Clear();
            foreach (var note in noteManager.Notes)
            {
                string tags = note.Tags.Count > 0 ? " [" + string.Join(", ", note.Tags) + "]" : "";
                notesListBox.Items.Add($"{note.Title} ({note.Date.ToString("yyyy-MM-dd")}){tags}");
            }
        }

        private void UpdateCurrentTags()
        {
            currentTagsListBox.Items.Clear();
            Note selected = GetSelectedNote();
            if (selected == null) return;

            foreach (var tag in selected.Tags)
            {
                currentTagsListBox.Items.Add(tag);
            }
        }

        private void UpdateFilterComboBox()
        {
            filterComboBox.Items.Clear();
            filterComboBox.Items.Add("Все заметки");

            if (tagManager != null)
            {
                foreach (var tag in tagManager.GetAllTags())
                {
                    filterComboBox.Items.Add(tag);
                }
            }
            filterComboBox.SelectedIndex = 0; ;
        }

        private Note GetSelectedNote()
        {
            if (notesListBox.SelectedIndex == -1) return null;

            string selectedItem = notesListBox.SelectedItem.ToString();
            string title = selectedItem.Split('(')[0].Trim();
            return noteManager.Notes.Find(n => n.Title.Trim() == title);
        }

        private void NotesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCurrentTags();
        }

        private void AddTagButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tagTextBox.Text)) return;

            Note selected = GetSelectedNote();
            if (selected == null)
            {
                MessageBox.Show("Сначала выберите заметку!");
                return;
            }

            if (tagManager == null) return;

            tagManager.AddTag(selected, tagTextBox.Text);
            tagTextBox.Clear();
            UpdateCurrentTags();
            UpdateNotesList();
            UpdateFilterComboBox();
        }

        private void RemoveTagButton_Click(object sender, EventArgs e)
        {
            if (currentTagsListBox.SelectedIndex == -1) return;

            Note selected = GetSelectedNote();
            if (selected == null) return;

            string tag = currentTagsListBox.SelectedItem.ToString();
            tagManager.RemoveTag(selected, tag);

            if (tagManager == null) return;

            UpdateCurrentTags();
            UpdateNotesList();
            UpdateFilterComboBox();
        }

        private void FilterButton_Click(object sender, EventArgs e)
        {
            if (filterComboBox.SelectedIndex == 0)
            {
                UpdateNotesList();
                return;
            }

            string selectedTag = filterComboBox.SelectedItem.ToString();
            List<Note> filtered = tagManager.FilterByTag(selectedTag);

            notesListBox.Items.Clear();
            foreach (var note in filtered)
            {
                string tags = note.Tags.Count > 0 ? " [" + string.Join(", ", note.Tags) + "]" : "";
                notesListBox.Items.Add($"{note.Title} ({note.Date.ToString("yyyy-MM-dd")}){tags}");
            }
        }

        private void AddNoteButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(titleTextBox.Text) || string.IsNullOrEmpty(contentTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            Note newNote = new Note(titleTextBox.Text, contentTextBox.Text, DateTime.Now);
            try
            {
                noteManager.AddNote(newNote);
                titleTextBox.Clear();
                contentTextBox.Clear();
                UpdateNotesList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RemoveNoteButton_Click(object sender, EventArgs e)
        {
            if (notesListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите заметку для удаления!");
                return;
            }
            string selectedItem = notesListBox.SelectedItem.ToString();


            string[] parts = selectedItem.Split(new[] { '(' }, StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                string title = parts[0].Trim();
                string datePart = parts[1].Split(')')[0].Trim();

                if (DateTime.TryParse(datePart, out DateTime date))
                {
                    var noteToRemove = noteManager.Notes.Find(n => n.Title.Trim() == title && n.Date.Date
                    == date.Date);
                    if (noteToRemove != null)
                    {
                        try
                        {
                            noteManager.RemoveNote(noteToRemove);
                            MessageBox.Show("Заметка удалена!");
                            UpdateNotesList();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
