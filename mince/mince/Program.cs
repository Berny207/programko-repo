using System.Text;

namespace mince
{
    internal class Sequence
    {
        Stack<int> stack;
        public Sequence()
        {
            stack = new Stack<int>();
        }
        public void Add(int value)
        {
            stack.Push(value);
        }
        public int Remove()
        {
            return stack.Pop();
        }
        public int Peek()
        {
            return stack.Peek();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            List<int> list = stack.ToList();
            for(int i = stack.Count()-1;i >= 0; i--)
            {
                int value = list[i];
                if(value == 0)
                {
                    continue;
                }
                sb.Append(" " + value.ToString());
            }
            return sb.ToString();
        }
    }

    internal class Program
    {
        static (List<int>, int) LoadInputs(string inputFileName)
        {
            List<int> output = new List<int>();
            int sum = 0;
            using (StreamReader streamReader = new StreamReader($"..\\..\\..\\{inputFileName}"))
            {
                string line = streamReader.ReadLine();
                string[] listString = line.Split(" ");
                foreach (string str in listString)
                {
                    int coin = 0;
                    try
                    {
                        coin = int.Parse(str);
                    }
                    catch
                    {
                        throw new Exception("Invalid input");
                    }
                    if(coin <= 0)
                    {
                        throw new Exception("Incorrect input");
                    }
                    output.Add(coin);
                }
                line = streamReader.ReadLine();
                try
                {
                    sum = int.Parse(line);
                }
                catch
                {
                    throw new Exception("Invalid input");
                }
            }
            return (output, sum);
        }
        static int? SelectCoinToAdd(int currentSum, int sum, List<int> PossibleCoins, int lastSelectedCoin)
        {
            foreach (int coin in PossibleCoins) 
            {
                if (coin > lastSelectedCoin && lastSelectedCoin != -1)
                {
                    continue;
                }
                if (currentSum + coin <= sum)
                {
                    return coin;
                }
            }
            return null;
        }
        static void Main(string[] args)
        {
            int sum = 0;
            List<int> coins = new List<int>();
            try
            {
                (coins, sum) = LoadInputs("inputs.txt");
            }
            catch
            {
                Console.WriteLine("Invalid input");
                return;
            }
            Sequence sequence = new Sequence();
            int currentSum = 0;
            int lastCoin = -1;
            while (true)
            {
                int? coinToAdd = SelectCoinToAdd(currentSum, sum, coins, lastCoin);
                if (coinToAdd == null)
                {
                    // Cannot add another coin
                    if (currentSum == sum)
                    {
                        Console.WriteLine(sequence);
                    }
                    // Backtrack
                    try
                    {
						lastCoin = sequence.Remove() - 1;
                        currentSum -= (lastCoin+1);
					}
                    catch
                    {
                        break;
                    }
                    continue;
                }
                sequence.Add((int)coinToAdd);
                currentSum += (int)coinToAdd;
                lastCoin = (int)coinToAdd;
            }
        }
    }
}
