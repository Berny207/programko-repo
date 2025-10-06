using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace UspornaNavigace
{
    public class Town
    {
        public int ID {get; set; }
        public List<Road> roads = new List<Road>();
        /// <summary>
        /// distancesFromStart[0] is best unpaid path <br/>
        /// distancesFromStart[1] is best paid path
        /// </summary>
        public List<int> distancesFromStart = new List<int>();
        public Town()
        {
            distancesFromStart.Add(-1);
            distancesFromStart.Add(-1);
        }
        public bool update(int bestUnpaidPath, int bestPaidPath, bool isRoadPaid)
        {
            bool result = false;
            if (this.distancesFromStart[0] > bestUnpaidPath && isRoadPaid == false)
            {
                this.distancesFromStart[0] = bestUnpaidPath;
                result = true;
            }
            if (this.distancesFromStart[1] > bestPaidPath)
            {
                this.distancesFromStart[1] = bestPaidPath;
                if (this.distancesFromStart[0] <= this.distancesFromStart[1])
                {
                    result = true;
                }
            }
        }
    }

    public class Road
    {
        public int townFromID {get; set; }
        public int townToID {get; set; }
        public int length {get; set; }
        public bool paid { get; set; }

        public Road(int townFromID, int townToID, int length, int paid)
        {
            this.townFromID = townFromID;
            this.townToID = townToID;
            this.length = length;
            if(paid == 0)
            {
                this.paid = false;
            }
            else
            {
                this.paid = true;
            }
        }
    }
    internal class Program
    {
        static Exception invalidInput = new();
        static List<int> parseInput(string input, int numberAmount)
        {
            List<int> result = new List<int>();
            string[] splicedStrings = input.Split(" ");
            if (splicedStrings.Length != numberAmount)
            {
                throw (invalidInput);
            }
            foreach (string s in splicedStrings)
            {
                try
                {
                    int i = int.Parse(s);
                    if(i < 0)
                    {
                        throw (invalidInput);
                    }
                    result.Add(i);
                }
                catch
                {
                    throw(invalidInput);
                }
            }
            return result;
        }
        static List<Town> loadTowns(int M, int S)
        {

            List<Town> towns = new List<Town>();
            for(int i = 0;i<M;i++)
            {
                towns.Add(new Town());
            }
            for(int i = 0; i < S; i++)
            {
                List<int> parsedInput = new List<int>();
                try
                {
                    parsedInput = parseInput(Console.ReadLine(), 4);
                }
                catch
                {
                    throw (invalidInput);
                }
                if (M <= parsedInput[0]|| M <= parsedInput[1] || parsedInput[2] == 0 || (parsedInput[3] != 0 && parsedInput[3] != 1))
                {
                    throw (invalidInput);
                }
                Road road = new Road(parsedInput[0], parsedInput[1], parsedInput[2], parsedInput[3]);
                towns[road.townFromID].roads.Add(road);
            }
            return towns;
        }

        static string inputFail = "Neplatný vstup.";
        static void Main(string[] args)
        {
            List<int> parsedInput = new List<int>();
            int M;
            int S;
            //nacteni vstupu M, S
            try
            {
                parsedInput = parseInput(Console.ReadLine(), 2);
                M = parsedInput[0];
                S = parsedInput[1];
            }
            catch
            {
                Console.WriteLine(inputFail);
                return;
            }
            if(M == 0)
            {
                Console.WriteLine(inputFail);
                return;
            }

            List<Town> towns = loadTowns(M, S);

            parsedInput = new List<int>();
            int startTownID;
            int goalTownID;
            try
            {
                parsedInput = parseInput(Console.ReadLine(), 2);
                startTownID = parsedInput[0];
                goalTownID = parsedInput[1];
            }
            catch
            {
                Console.WriteLine(inputFail);
                return;
            }
            if(startTownID >= M || goalTownID >= M)
            {
                Console.WriteLine(inputFail);
                return;
            }

            List<Town> queue = new List<Town>();
            towns[startTownID].distancesFromStart[0] = 0;
            towns[startTownID].distancesFromStart[1] = 0;
            queue.Add(towns[startTownID]);
            List<Town> bestPath = new List<Town>();

            while (queue.Count > 0)
            {
                //take first town from queue
                //look at it's neighbours
                //update them - update values, decide whether to add them to the queue or not
                foreach (Road r in queue[0].roads)
                {
                    if (queue[0].distancesFromStart[0] == -1 && r.paid == true)
                    {
                        continue;
                    }
                    bool addToQueue = towns[r.townToID].update(queue[0].distancesFromStart[0] + r.length, queue[0].distancesFromStart[1] + r.length, r.paid);
                    if (addToQueue)
                    {
                        queue.Add(towns[r.townToID]);
                    }
                }
            }
        }
    }
}
