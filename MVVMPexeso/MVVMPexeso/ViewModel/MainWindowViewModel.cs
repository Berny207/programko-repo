using Othello.Model;
using MVVMProject.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace Othello.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public RelayCommand StartCommand => new RelayCommand(execute => StartGame(), canExecute => _isGameRunning == false);
        public RelayCommand SquareClickCommand => new RelayCommand(execute => SquareClicked(execute as SquareViewModel), canExecute => _isGameRunning == true);

        public MainWindowViewModel() 
        {
            Squares = new ObservableCollection<SquareViewModel>();
        }

        private Color defaultColor = Color.FromRgb(0, 128, 0);
        private Color _higlightColor  = Colors.Green;

        private bool _isGameRunning = false;
        private bool _isBusy = false;

        private List<Player> Players = new List<Player>();
        private Player activePlayer;

        public GameBoard gameBoard;

        #region Data Binding

        // Vlastnosti, na nichž máme data binding: karty pexesa, velikost gridu (neměnné), skóre
        public ObservableCollection<SquareViewModel> Squares { get; set; }

        public int GridSize = 8;

        private string _label;
        public string Label
        { 
            get
            {
                return _label;
            }
            set
            {
                _label = value;
                OnPropertyChanged(nameof(Label));
            }
        }
        #endregion

        #region Herní logika
        // Samotná herní logika
        public void StartGame()
        {   
            CreateGameCards();
            CreatePlayers();
            SetOwner(gameBoard.GetSquare(new Position(3, 4)), Players[0]);
            SetOwner(gameBoard.GetSquare(new Position(4, 3)), Players[0]);
            SetOwner(gameBoard.GetSquare(new Position(4, 4)), Players[1]);
            SetOwner(gameBoard.GetSquare(new Position(3, 3)), Players[1]);
            OnPropertyChanged(nameof(GridSize)); // máme nachystané karty, vyvoláme funkci, že se grid změnil
            _isGameRunning = true;
        }
        private void CreateGameCards()
        {
            gameBoard = new GameBoard();
            // přidáme dvojice karet
            for(int i = 0; i< GridSize; i++)
            {
                for(int j = 0; j< GridSize; j++)
                {
                    Position pos = new Position(i, j);
                    SquareViewModel newViewModel = new SquareViewModel(gameBoard.GetSquare(pos));
                    Squares.Add(newViewModel);
                    gameBoard.GetSquare(pos).SetColor(defaultColor);
                }
            }
        }
        private void CreatePlayers()
        {
            Players.Add(new Player(Color.FromRgb(0, 0, 0), "černý"));
            Players.Add(new Player(Color.FromRgb(255, 255, 255), "bílý"));
            activePlayer = Players[0];
        }
        private void SquareClicked(SquareViewModel squareViewModel)
        {
            Square square = squareViewModel.Model;
            // check if move is valid
        }

        private bool FindEncirclements(Square square, Player player)
        {
            Position squarePos = square.GetPosition();
            // gaming
            // check horizontals
            CheckLeft(squarePos, player);
            return true;
        }

        private bool CheckLeft(Position squarePos, Player player)
        {
            Position testPos;
            bool atLeastOneEnemy = false;
            // left
            for (int i = squarePos.x; i >= 0; i--)
            {
                testPos = new Position(i, squarePos.y);
                bool? result = TestPosition(testPos, player, atLeastOneEnemy);
                if(result is null)
                {
                    atLeastOneEnemy = true;
                    continue;
                }
                bool output = (bool)result;
                return output;
            }
            return false;
        }
        private bool CheckRight(Position squarePos, Player player)
        {
            Position testPos;
            bool atLeastOneEnemy = false;
            // left
            for (int i = squarePos.x; i < 8; i++)
            {
                testPos = new Position(i, squarePos.y);
                bool? result = TestPosition(testPos, player, atLeastOneEnemy);
                if (result is null)
                {
                    atLeastOneEnemy = true;
                    continue;
                }
                bool output = (bool)result;
                return output;
            }
            return false;
        }
        private bool CheckUp(Position squarePos, Player player)
        {
            Position testPos;
            bool atLeastOneEnemy = false;
            // left
            for (int i = squarePos.y; i >= 0; i--)
            {
                testPos = new Position(squarePos.x, i);
                bool? result = TestPosition(testPos, player, atLeastOneEnemy);
                if (result is null)
                {
                    atLeastOneEnemy = true;
                    continue;
                }
                bool output = (bool)result;
                return output;
            }
            return false;
        }
        private bool CheckDown(Position squarePos, Player player)
        {
            Position testPos;
            bool atLeastOneEnemy = false;
            // left
            for (int i = squarePos.y; i < 8; i++)
            {
                testPos = new Position(squarePos.x, i);
                bool? result = TestPosition(testPos, player, atLeastOneEnemy);
                if (result is null)
                {
                    atLeastOneEnemy = true;
                    continue;
                }
                bool output = (bool)result;
                return output;
            }
            return false;
        }
        private bool? TestPosition(Position testPos, Player player, bool hasOneEnemy)
        {
            Square testSquare = gameBoard.GetSquare(testPos);
            if (testSquare.GetOwner() == null)
            {
                return false;
            }
            if(testSquare.GetOwner() == player)
            {
                return hasOneEnemy;
            }
            return null;
        }
        private void SetOwner(Square square, Player player)
        {
            square.SetColor(player.Color);
            square.SetOwner(player);
        }
        #endregion

    }

}

    

