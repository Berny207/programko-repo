using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniMax_Nim_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var initialPiles = new List<int> { 2, 2 };
            bool botStarts = true;

            var game = new NimGame(initialPiles, botStarts);
            GameState state;

            do
            {
                state = game.PlayTurn();
            } while (state == GameState.Ongoing);


            if (state == GameState.BotWon)
                Console.WriteLine("Vyhrál počítač!");
            else
                Console.WriteLine("Gratulujeme! Vyhráli jste!");
        }
    }

    public enum GameState
    {
        Ongoing,
        BotWon,
        HumanWon
    }

    public class NimGameState
    {
        public List<int> Piles { get; private set; }
        public int MatchesInGame { get; private set; }

        public NimGameState(IEnumerable<int> initialPiles)
        {
            Piles = new List<int>(initialPiles);
            MatchesInGame = Piles.Sum();
        }

        public void MakeMove(int pileIndex, byte matchesToRemove)
        {
            if (IsValidMove(pileIndex, matchesToRemove))
            {
                Piles[pileIndex] -= matchesToRemove;
                MatchesInGame -= matchesToRemove;
            }
            else
            {
                throw new ArgumentException("Neplatný tah!");
            }
        }

        private bool IsValidMove(int pileIndex, byte matchesToRemove)
        {
            return pileIndex >= 0 &&
                   pileIndex < Piles.Count &&
                   Piles[pileIndex] >= matchesToRemove &&
                   matchesToRemove > 0;
        }
    }

    public class NimGame
    {
        private NimGameState _state; // pro privátní datové položky používáme podtržítko na začátku jména
        private bool _botStarts;
        private bool _isBotTurn;

        public NimGame(List<int> initialPiles, bool botStarts)
        {
            _state = new NimGameState(initialPiles);
            _botStarts = botStarts;
            _isBotTurn = botStarts;
        }

        public GameState PlayTurn()
        {           
            PrintGameState();

            if (_isBotTurn)
            {
                var botMove = GetBestBotMove();
                MakeAndPrintBotMove(botMove);
            }
            else
            {
                var humanMove = GetHumanInput();
                _state.MakeMove(humanMove.Item1, humanMove.Item2);
            }

            _isBotTurn = !_isBotTurn;

            if (_state.MatchesInGame == 0)
                if (_isBotTurn)
                    return GameState.BotWon;
                else
                    return GameState.HumanWon;
            else
                return GameState.Ongoing;
        }

        private Tuple<int, byte> GetBestBotMove()
        {
            Tuple<int, byte> bestTurn = new Tuple<int, byte>(0, 1);

			int score = minimax(_state.Piles.ToList(), 10, true);

            int minimax(List<int> piles, int depth, bool maximizingPlayer)
            {
                //zkopíruj int list do jiného
                List<int> copyLists(List<int> toCopy)
                {
                    List<int> output = new List<int>();
                    foreach (int element in toCopy)
                    {
                        output.Add(element);
                    }
                    return output;
                }
                //projdi a vrať všechny možné pozice po daném tahu
                List<List<int>> getPossiblePiles(List<int> piles)
                {
                    List<List<int>> possiblePiles = new List<List<int>>();
                    for (int i = 0; i < piles.Count; i++)
                    {
                        List<int> newPiles = copyLists(piles);
                        if (piles[i] >= 2)
                        {
                            newPiles[i] = newPiles[i] - 2;
                            possiblePiles.Add(newPiles);
                        }
						newPiles = copyLists(piles);
						if (piles[i] >= 1)
                        {
                            newPiles[i] = newPiles[i] - 1;
                            possiblePiles.Add(newPiles);
                        }
                    }
                    return possiblePiles;
                }

                Tuple<int, byte> getTurnTupleFromPositions(List<int> pilesBefore, List<int> pilesAfter)
                {
                    for(int i = 0;i < pilesBefore.Count; i++)
                    {
                        if (pilesBefore[i] != pilesAfter[i])
                        {
                            return new Tuple<int, byte>(i, Convert.ToByte(pilesBefore[i] - pilesAfter[i]));
                        }
                    }
                    return null;
                }

                List<List<int>> possiblePiles = getPossiblePiles(piles);
                //je konečná pozice? vrať hráče, který vyhrál
                if (possiblePiles.Count == 0)
                {
                    return Convert.ToInt32(maximizingPlayer);
                }
                int evalFunction(List<int> piles)
                {
                    //pokud je počet na hromádce % 3 roven 1, je to prohrávající hromádka pro hráče na tahu
                    //pokud je počet prohrávajících hromádek lichý, je daná pozice prohrávajíci
                    int losingPiles = 0;
                    foreach (int pile in piles)
                    {
                        if(pile%3 == 1)
                        {
                            losingPiles++;
                        }
                    }
                    return (losingPiles % 2)*2-1;
                }
                //pokud jsme vyčerpali naši hloubku, přestaneme se zajímat o minimax a jednoduše si spočítáme evaluační funkci
                if(depth == 0)
                {
                    return evalFunction(piles);
                }
                int bestTurnValue = 0;
                //pro každý možný tah
                foreach (List<int> possiblePile in possiblePiles)
                {
                    int result = minimax(possiblePile, depth - 1, !maximizingPlayer);
                    if (maximizingPlayer == true)
                    {
                        if (result > bestTurnValue || bestTurnValue == 0)
                        {
                            bestTurnValue = result;
                        }
                    }
                    else
                    {
                        if (result < bestTurnValue || bestTurnValue == 0)
                        {
                            bestTurnValue = result;
                        }
                    }
                    //jestliže se jedná o poslední minimax z našeho tahu, pak potřebujeme z toho dostat nejlepší tah
                    if(bestTurnValue == result && depth == 10)
                    {
                        bestTurn = getTurnTupleFromPositions(piles, possiblePile);
                    }
                }
                return bestTurnValue;     
            }
            Console.WriteLine(bestTurn);
            return bestTurn;
        }

        private void PrintGameState()
        {
            Console.WriteLine("Aktuální stav hry:");
            foreach (var pile in _state.Piles)
                Console.Write(pile + " ");
            Console.WriteLine();
        }

        private void MakeAndPrintBotMove(Tuple<int, byte> move)
        {
            _state.MakeMove(move.Item1, move.Item2);
            Console.WriteLine($"Počítač bere {move.Item2} sirky z hromádky {move.Item1}");
        }

        private Tuple<int, byte> GetHumanInput()
        {

            Console.Write("Z které hromádky chcete brát? (");
            for (int i = 0; i < _state.Piles.Count; i++)
            {
                if (_state.Piles[i] > 0)
                    Console.Write($"{i} ");
            }   
            Console.Write(")");

            int pileIndex = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"Kolik sirek chcete vzít? (1-{_state.Piles[pileIndex]})");
            byte matches = Convert.ToByte(Console.ReadLine());

            return new Tuple<int, byte>(pileIndex, matches);
        }
    }

    
}