using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class Note
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public Note(string title, string content, DateTime date)
        {
            if (title == " ")
            {
                MessageBox.Show("Введите название!");
            }
            else
            {
                Title = title;
            }

            if (content.Length >= 1000)
            {
                MessageBox.Show("Заметка слишком длинная!");
            }
            else
            {
                if (content == " ")
                {
                    MessageBox.Show("Введите текст!");
                }
                else
                {
                    Content = content; 
                }
            }
            Date = date;
            Date = DateTime.Now;
        }
    }
}
