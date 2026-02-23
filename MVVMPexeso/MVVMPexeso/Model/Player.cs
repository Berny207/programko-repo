using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Model
{
    internal class Player
    {
        public Color Color;
        public string Name;

        public Player(Color color, string name)
        {
            this.Color = color;
            this.Name = name;
        }
    }
}
