using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class Note
    {
        public List<string> Tags { get; set; } = new List<string>();
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public Note(string title, string content, DateTime date)
        {
            Title = title;
            Content = content; 
            Date = date;
            Tags = new List<string>();
        }
    }
}
