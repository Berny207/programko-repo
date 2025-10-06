using System.Security.Cryptography.X509Certificates;

namespace stabilni_manzelstvi
{

    public class Clovek
    {
        public int jmeno { get; set; }
        public bool zadany { get; set; } = false;
        public int bestMatch {  get; set; }
        public int IcurrentBestPreference { get; set; } = 0;
        public List<int> preferences = new List<int>();
        public List<int> requests = new List<int>();
        public Clovek(int jmeno, List<int> preferences)
        {
            this.jmeno = jmeno;
            this.preferences = preferences;
        }

        public int selectBestMatch()
        {
            if(requests.Count == 0)
            {
                return 0;
            }

            int bestRequest = -1;
            int bestRequestRating = -1;
            foreach(int request in requests)
            {
                int requestRating = this.preferences.IndexOf(request);
                if(requestRating < bestRequestRating||bestRequestRating == -1)
                {
                    bestRequest = request;
                    bestRequestRating = requestRating;
                }
            }
            if (bestRequestRating > this.preferences.IndexOf(bestMatch) && this.zadany == true)
            {
				return 0;
				
            }
            else
            {
				return bestRequest;
			}
        }
    }
    internal class Program
    {
        public static List<int> parseInput(String[] input)
        {
            List<int> result = new List<int>();
            foreach (String s in input)
            {
                result.AddRange(parseIntList(s.ToList()));
            }
            return result;
        }
        public static List<int> parseIntList(List<char> input)
        {
            List<int> result = new List<int>();
            foreach (char c in input)
            {
                result.Add(c-'0');
            }
            return result;
        }
        static void Main(string[] args)
        {
            bool finished = false;
            int N = int.Parse(Console.ReadLine());
            
            List<Clovek> zeny = new List<Clovek>();
            List<Clovek> muzi = new List<Clovek>();


            for(int i = 0; i < N * 2; i++)
            {
                Clovek novyClovek = new Clovek(i%N+1, parseInput(Console.ReadLine().Split(' ')));
                if(i >= N)
                {
                    muzi.Add(novyClovek);
                }
                else
                {
                    zeny.Add(novyClovek);
                }
            }
            while (finished==false)
            {
                //nezadane zeny poradaji muze na topu jejich seznamu
                foreach (Clovek zena in zeny)
                {
                    if (zena.zadany == false)
                    {
                        muzi[zena.preferences[zena.IcurrentBestPreference] - 1].requests.Add(zena.jmeno);
                        zena.IcurrentBestPreference++;
                    }
                }
                //muzi odmitnou vsechny horsi volby(vcetne vybrane zeny)
                foreach (Clovek muz in muzi)
                {
                    int proposition = muz.selectBestMatch();
                    muz.requests.Clear();
                    if (proposition <= 0)
                    {
                        continue;
                    }
                    if (muz.zadany == true)
                    {
                        zeny[muz.bestMatch - 1].zadany = false;
                        zeny[muz.bestMatch - 1].bestMatch = 0;
                    }
                    muz.bestMatch = proposition;
                    muz.zadany = true;
					zeny[proposition - 1].bestMatch = muz.jmeno;
                    zeny[proposition - 1].zadany = true;
                }
                //kontrola, jestli vsechny zeny jsou zadane, pokud anom tak ukonci program
                finished = true;
                foreach (Clovek zena in zeny)
                {
                    if (zena.zadany == false)
                    {
                        finished = false;
                    }
                }
            }//vrat se zpatky na krok jedna

            //vytiskni vystup

            foreach(Clovek zena in zeny)
            {
                Console.WriteLine(zena.bestMatch);
            }
        }
    }
}
