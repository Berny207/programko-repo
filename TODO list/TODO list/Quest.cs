using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_list
{
    internal class Quest
    {
        public string Text { get; set; }
        public DateTime Deadline { get; set; }
        public int Importance { get; set; }

        public Quest(string text, DateTime deadline, int importance)
        {
            Text = text;
            Deadline = deadline;
            Importance = importance;
        }

    }
}
