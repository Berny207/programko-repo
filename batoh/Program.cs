using System.Net.NetworkInformation;
using System.Text;
using System.Linq;

namespace batoh
{
    internal class Item
    {
        public int value { get; }
        public int weight { get; }
        public int id { get; }
        public Item(int value, int weight, int id)
		{
			this.value = value;
			this.weight = weight;
            this.id = id;
		}
	}
    internal class Program
    {
        static double Evaluate(List<Item> assignment, List<Item> items, int capacity)
        {
            List<Item> remainingItems = new List<Item>();
            double aproximatedBestSolution = CalculateValue(assignment);
			int currentWeight = CalculateWeight(assignment);
			foreach (Item item in items)
            {
                if (!assignment.Contains(item))
                {
                    remainingItems.Add(item);
                }
            }
            // order remaining items by their value/weight ratio
            List<Item> sortedItems = items.OrderByDescending(x => (double)x.value/x.weight).ToList();
            foreach(Item sortedItem in sortedItems)
            {
                if(sortedItem.weight + currentWeight > capacity)
                {
                    aproximatedBestSolution += (capacity - currentWeight) * sortedItem.value / sortedItem.weight;
                    return aproximatedBestSolution;
                }
                currentWeight += sortedItem.weight;
                aproximatedBestSolution += sortedItem.value;
            }
            return aproximatedBestSolution;
        }
		static Item? SelectItemToAssign(Item? lastItem, List<Item> items, List<Item> bannedItems)
		{
            bool targetReached = false;
            if(lastItem == null)
            {
                targetReached = true;
            }
            foreach(Item item in items)
            {
                if(targetReached & !bannedItems.Contains(item))
                {
                    return item;
                }
                if(item == lastItem)
                {
                    targetReached = true;
                }
            }
			return null;
		}
        static int CalculateWeight(List<Item> items)
        {
            int weight = 0;
            foreach(Item item in items)
            {
                weight += item.weight;
            }
            return weight;
        }
        static int CalculateValue(List<Item> items)
        {
			int value = 0;
			foreach (Item item in items)
			{
				value += item.value;
			}
			return value;
		}
        static List<Item> FilterItems(List<Item> items, List<Item> selectedItems, int capacity)
        {
            List<Item> blacklistedItems = new List<Item>();
            int bagWeight = CalculateWeight(selectedItems);
            foreach(Item item in items)
            {
                if(capacity < item.weight + bagWeight)
                {
                    blacklistedItems.Add(item);
                }
            }
            return blacklistedItems;
        }
		static List<Item> LoadItems(StreamReader sr)
        {
            string[] valueLine = sr.ReadLine().Split(' ');
            string[] weightLine = sr.ReadLine().Split(' ');
            List<Item> items = new List<Item>();
            for(int i = 0; i < valueLine.Length; i++)
            {
                try
                {
                    int value = int.Parse(valueLine[i]);
                    int weight = int.Parse(weightLine[i]);
					items.Add(new Item(value, weight, i+1));
				}
                catch
                {
                    throw new Exception("Invalid input");
                }
			}
            return items;
		}
        static int LoadCapacity(StreamReader sr)
        {
            int capacity = 0;
            try
            {
                capacity = int.Parse(sr.ReadLine());
            }
            catch
            {
                throw new Exception("Invalid input");
            }
            if(capacity < 0)
            {
                throw new Exception("Capacity cannot be negative");
			}
            return capacity;
		}
        static List<string> LoadSolution(StreamReader sr)
        {
            return sr.ReadLine().Substring(3).Split(" ").ToList();
        }
        static int LoadSolutionCapacity(StreamReader sr)
        {
			int solutionCapacity = 0;
			try
			{
                string line = sr.ReadLine();
                line = line.Substring(3);
				solutionCapacity = int.Parse(line);
			}
			catch
			{
				throw new Exception("Invalid input");
			}
			if (solutionCapacity < 0)
			{
				throw new Exception("Solution capacity cannot be negative");
			}
			return solutionCapacity;
		}
        static string PrintResult(List<int> ids)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = ids.Count - 1; i >= 0;i--)
            {
                sb.Append(ids[i].ToString() + " ");
            }
            return sb.ToString();
        }

        static List<int> ConvertToIDList(List<Item> items)
        {
            List<int> IDs = new List<int>();
            foreach (Item item in items)
            {
                IDs.Add(item.id);
            }
            return IDs;
        }
        static (List<int>, int) Solve(List<Item> items, int capacity)
        {
			List<Item> bestSolution = new List<Item>();
			int bestSolutionValue = int.MinValue;
			Stack<Item> assignment = new Stack<Item>();
			Item? lastItem = null;

			while (true)
			{
				List<Item> blacklistedItems = FilterItems(items, assignment.ToList(), capacity);
				// Try to assign a value
				Item? toAssign = SelectItemToAssign(lastItem, items, blacklistedItems);
				if (toAssign is null)
				{
					// Cannot find a valid value to assign -> we are in a leaf, 
					if (CalculateValue(assignment.ToList()) > bestSolutionValue)
					{
						bestSolution = assignment.ToList();
						bestSolutionValue = CalculateValue(assignment.ToList());
					}
					try
					{
						Item removedItem = assignment.Pop();
						lastItem = removedItem;
					}
					catch
					{
						break;
					}
					continue;
				}

                double aproximatedSolution = Evaluate(assignment.ToList(), items, capacity);
                if( aproximatedSolution <= bestSolutionValue)
                {
					// backtrack again
					try
					{
						Item removedItem = assignment.Pop();
						lastItem = removedItem;
					}
					catch
					{
						break;
					}
					continue;
				}
				assignment.Push(toAssign);
				lastItem = toAssign;
			}
            return (ConvertToIDList(bestSolution), bestSolutionValue);
		}
		static void Main(string[] args)
        {
            int capacity = 0;
            int solutionCapacity = 0;
            List<string> solution = new List<string>();
            List<Item> items = new List<Item>();
			string filePath = "..//..//..//inputs.txt";
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (true)
                {
                    try
                    {
                        items = LoadItems(sr);
                        capacity = LoadCapacity(sr);
                        solutionCapacity = LoadSolutionCapacity(sr);
                        solution = LoadSolution(sr);
                        (List<int> bestSolution, int bestSolutionValue) = Solve(items, capacity);
                        Console.WriteLine(bestSolutionValue);
                        Console.WriteLine(PrintResult(bestSolution));
						Console.WriteLine("Solution is: ");
						Console.WriteLine(solutionCapacity);
                        foreach (string sol in solution) Console.Write(sol + " ");
                        Console.WriteLine("\n");
                    }
                    catch
                    {
                        Console.WriteLine("Error loading input");
                        return;
                    }
                    if (sr.ReadLine() == null)
                    {
                        break;
                    }
                }
            }
		}
	}
}
