using Othello.Model;
using MVVMProject.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Othello.ViewModel
{
    internal class SquareViewModel : ViewModelBase
    {
        public Square Model { get; }
        public SquareViewModel(Square square)
        {
            Model = square;
            square.SetView(this);
        }
        public void UpdateUI(Color newColor)
        {
            BackgroundColor = new SolidColorBrush(newColor);
        }
        private SolidColorBrush _backgroundColor;
        public SolidColorBrush BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }
    }
}
