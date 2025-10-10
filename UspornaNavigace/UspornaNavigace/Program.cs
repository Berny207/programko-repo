using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace UspornaNavigace
{
    public class Town
    {
        public int ID {get; set; }
        public List<Road> roads = new List<Road>();
        /// <summary>
        /// bestDistances[0] is best unpaid path <br/>
        /// bestDistances[1] is best paid path <br/>
        /// if bestDistances[1] is -1, the town hasn't been discovered.
        /// </summary>
        public List<int> bestDistances = new List<int>();

        public int bestFreeDistanceFromTownID = -1;
        public int bestPaidDistanceFromTownID = -1;
        public Town(int ID)
        {
            bestDistances.Add(-1);
            bestDistances.Add(-1);
            this.ID = ID;
        }
        public bool update(Road road, Town townFrom)
        {
            bool result = false;
            if (road.paid == true)
            {
                //update paid path using free path
                if (this.bestDistances[1] > road.length + townFrom.bestDistances[0] || this.bestDistances[1] == -1)
                {
                    result = true;
                    this.bestDistances[1] = road.length + townFrom.bestDistances[0];
					this.bestPaidDistanceFromTownID = road.townFromID;
				}
                return result;
            }
            //check if free path has been found into the town road is coming from
            if (townFrom.bestDistances[0] != -1) {
                if (this.bestDistances[0] > road.length + townFrom.bestDistances[0] || this.bestDistances[0] == -1)
                {
                    result = true;
                    this.bestDistances[0] = road.length + townFrom.bestDistances[0];
					this.bestFreeDistanceFromTownID = road.townFromID;

				}
            }
            //update the paid path at all times
            if (this.bestDistances[1] > road.length + townFrom.bestDistances[1] || this.bestDistances[1] == -1)
            {
                result = true;
                this.bestDistances[1] = road.length + townFrom.bestDistances[1];
				this.bestPaidDistanceFromTownID = road.townFromID;
			}
            return result;
        }
        public void queueRoads(PriorityQueue<Road, int> queue)
        {
			foreach (Road r in this.roads)
			{
				if (r.paid == true & this.bestDistances[0] == -1)
				{
					continue;
				}
				if (r.paid == true)
				{
					queue.Enqueue(r, r.length + this.bestDistances[0]);
				}
				else
				{
					queue.Enqueue(r, r.length + this.bestDistances[1]);
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
                towns.Add(new Town(i));
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
                Road roadFrom = new Road(parsedInput[0], parsedInput[1], parsedInput[2], parsedInput[3]);
                towns[roadFrom.townFromID].roads.Add(roadFrom);
                Road roadTo = new Road(parsedInput[1], parsedInput[0], parsedInput[2], parsedInput[3]);
                towns[roadTo.townFromID].roads.Add(roadTo);
            }
            return towns;
        }

        static string inputFail = "Neplatný vstup.";
        static string noPathFound = "Cesta neexistuje.";
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

            towns[startTownID].bestDistances[0] = 0;
			towns[startTownID].bestDistances[1] = 0;
            //solve part
            PriorityQueue<Road, int> roadsToVisit = new PriorityQueue<Road, int>();
			towns[startTownID].queueRoads(roadsToVisit);
			while (roadsToVisit.Count > 0)
            {
                roadsToVisit.TryDequeue(out Road inspectedRoad, out int distance);
                bool updateSuccess = towns[inspectedRoad.townToID].update(inspectedRoad, towns[inspectedRoad.townFromID]);
                if(updateSuccess == true)
                {
                    towns[inspectedRoad.townToID].queueRoads(roadsToVisit);
                    if(inspectedRoad.townToID == goalTownID)
                    {
                        //printing out the result
                        List<int> bestPathByID = new List<int>();
                        int currentTownID = goalTownID;
                        bool havePaid = false;
                        while(currentTownID != startTownID)
                        {
                            bestPathByID.Add(currentTownID);
                            if(havePaid == false)
                            {
								currentTownID = towns[currentTownID].bestPaidDistanceFromTownID;
                                
							}
                        }
                        bestPathByID.Add(startTownID);
                        bestPathByID.Reverse();
                        foreach(int townID in bestPathByID)
                        {
                            if(townID == goalTownID)
                            {
                                Console.WriteLine(goalTownID.ToString());
                                break;
                            }
                            Console.Write(townID.ToString() + " -> ");
                        }
						Console.WriteLine("vzdálenost: " + towns[inspectedRoad.townToID].bestDistances[1].ToString());
                        return;
                    }
				}
            }
			Console.WriteLine(noPathFound);
		}
    }
}
