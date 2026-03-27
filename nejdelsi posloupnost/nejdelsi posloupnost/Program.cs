using System.ComponentModel.DataAnnotations;

namespace nejdelsi_posloupnost
{
    internal class Program
    {
        static List<int> loadSequence(StreamReader sr)
        {
            List<int> result = new List<int>();
            string[] strSequence = sr.ReadLine().Split(" ");
            foreach (string str in strSequence)
            {
                try
                {
                    result.Add(int.Parse(str));
                }
                catch { }
            }
            return result;
        }
        static List<List<int>> loadInput(StreamReader sr)
        {
            List<List<int>> result = new List<List<int>>();
            while (true)
            {
                result.Add(loadSequence(sr));
                sr.ReadLine();
                if (sr.EndOfStream)
                {
                    break;
                }
            }
            return result;
        }
        static (List<int>, List<int>) PrepareSequence(List<int> sequence)
        {
            List<int> sortedSequence = new List<int> ();
            int[] indexes = Enumerable.Range(0, sequence.Count).ToArray();
			Array.Sort(sequence.ToArray(), indexes);
            for(int i = 0; i < sequence.Count; i++)
            {
                sortedSequence.Add(sequence[indexes[i]]);
            }
            return (sortedSequence, indexes.ToList());
		}
        static List<int> SolveSequence(List<int> sequence)
        {
            (List<int> solvedSequence, List<int> indexes) = PrepareSequence(sequence);
			int length = sequence.Count;
            int[] bestLength = new int[length];
            PriorityQueue<int, int> foundLenghts = new PriorityQueue<int, int>();
			int[] origins = new int[length];
            for(int i = length-1; i >= 0; i--)
            {
                if (i == length - 1)
                {
                    foundLenghts.Enqueue(i, -1);
					origins[i] = -1;
                    continue;
				}
                int currentLength = int.MinValue;
                int currentOrigin = -1;
                for(int j = i; j < length; j++)
                {
                    if(sequence[i] < sequence[j])
                    {
                        if (bestLength[j] > currentLength)
                        {
                            currentLength = bestLength[j];
                            currentOrigin = j;
                        }
					}
				}
				if (currentOrigin == -1)
				{
					currentLength = 1;
				}
                bestLength[i] = currentLength + 1;
                origins[i] = currentOrigin;
			}

			// finding the index of the longest sequence
            int longestLength = int.MinValue;
            int longestIndex = -1;
            for(int i = 0; i < length; i++)
            {
                if (bestLength[i] > longestLength)
                {
                    longestLength = bestLength[i];
                    longestIndex = i;
                }
			}
			return ReconstructSequence(sequence, origins, longestIndex);
			// reconstructing the longest sequence
		}
        static List<int> ReconstructSequence(List<int> sequence, int[] origins, int firstIndex)
        {
            List<int> result = new List<int>();
            int currentIndex = firstIndex;
            while (currentIndex != -1)
            {
                result.Add(sequence[currentIndex]);
                currentIndex = origins[currentIndex];
            }
            return result;
		}
        static void Main(string[] args)
        {
            List<List<int>> sequences = new List<List<int>>();
            string path = @"..\..\..\..\vstupy.txt";
            using (StreamReader sr = new StreamReader(path))
            {
                sequences = loadInput(sr);
            }
            foreach (List<int> sequence in sequences)
            {
                List<int> longestSequence = SolveSequence(sequence);
                if (longestSequence.Count == 0)
                {
                    Console.WriteLine("prázdná posloupnost");
                }
                else
                {
                    Console.WriteLine(string.Join(" ", longestSequence));
                }
                Console.WriteLine();
			}
        }
    }
}
