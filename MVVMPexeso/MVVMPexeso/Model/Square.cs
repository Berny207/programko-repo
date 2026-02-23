using Othello.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Othello.Model
{
    internal class Square
    {
        private Color Color;
        private Player? Owner;
        private SquareViewModel View;
        private Position position;
        public Square(Position position)
        {
            this.Owner = null;
            this.position = position;
        }
        public void SetColor(Color color)
        {
            this.Color = color;
            this.View.UpdateUI(this.Color);
        }
        public void SetOwner(Player? owner)
        {
            this.Owner = owner;
        }
        public Player? GetOwner()
        {
            return this.Owner;
        }
        public void SetView(SquareViewModel view)
        {
            this.View = view;
        }
        public Position GetPosition()
        {
            return this.position;
        }
    }
}
