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
        static List<int> SolveSequence(List<int> sequence)
        {

        }
        static void Main(string[] args)
        {
            List<List<int>> sequences = new List<List<int>>();
            string path = @"..\..\..\..\vstupy.txt";
            using (StreamReader sr = new StreamReader(path))
            {
                sequences = loadInput(sr);
            }
        }
    }
}
