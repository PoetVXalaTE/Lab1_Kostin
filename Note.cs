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
        public Note(string title, string content)
        {
            Title = title;
            Content = content;
            Date = DateTime.Now;
        }
    }
}
