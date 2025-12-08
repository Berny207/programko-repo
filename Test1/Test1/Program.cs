namespace Test1
{
    struct Coordinates
    {
        public int x;
        public int y;
        /// <summary>
        /// Are x, y correct coordinates?
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool isInputCorrect(int x, int y)
        {
            return 0 <= x && x < 8 && 0 <= y && y < 8;
        }
        /// <summary>
        /// Constructor fom 2 int variables
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <exception cref="Exception"></exception>
        public Coordinates(int x, int y)
        {
            if (this.isInputCorrect(x, y))
            {
                this.x = x; 
                this.y = y;
            }
            else
            {
                throw new Exception("Invalid coordinates");
            }
        }
        /// <summary>
        /// Another constructor from int[] array
        /// </summary>
        /// <param name="array"></param>
        /// <exception cref="Exception"></exception>
        public Coordinates(int[] array)
        {
            if(array.Length != 2)
            {
                throw new Exception("Invalid coordinates");
            }
            if (this.isInputCorrect(x, y))
            {
                this.x = array[0];
                this.y = array[1];
            }
            else
            {
                throw new Exception("Invalid coordinates");
            }
        }
        /// <summary>
        /// Adds vector to current coordinates<br></br>
        /// Returns true if new coordinates are valid<br></br>
        /// Returns false if new coordinates are invalid
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public bool addVector(Vector vector)
        {
            if(isInputCorrect(this.x+vector.x, this.y + vector.y))
            {
                this.x += vector.x;
                this.y += vector.y;
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// 2D vector - has higher availible value range than standard Coordinates
    /// </summary>
    struct Vector
    {
        public int x;
        public int y;

        public Vector(int x, int y)
        {
            if (-8 < x && x < 8 && -8 < y && y < 8)
            {
                this.x = x; this.y = y;
            }
            else
            {
                throw new Exception("Invalid vector");
            }
        }
    }

    /// <summary>
    /// Just an 8x8 char grid
    /// </summary>
    class Board()
    {
        char[,] grid = new char[8, 8];
        internal char getCell(Coordinates coordinates)
        {
            return grid[coordinates.x, coordinates.y];
        }

        internal void setCell(Coordinates coordinates, char value)
        {
            grid[coordinates.x, coordinates.y] = value;
        }
    }
    internal class Program
    {
        static List<Vector> movementVectors = new List<Vector>
            {   new Vector(-2, -1),
                new Vector(-2, 1),
                new Vector(2, -1),
                new Vector(2, 1),
                new Vector(-1, -2),
                new Vector(-1, 2),
                new Vector(1, -2),
                new Vector(1, 2),
            };
        static Exception invalidInput = new Exception("Invalid input");
        /// <summary>
        /// Parses line from input file as an integer array
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputAmount"></param>
        /// <returns></returns>
        static int[] parseLine(string input, int inputAmount)
        {
            if(input == null)
            {
                throw invalidInput;
            }
            string[] splicedInput = input.Split(" ");
            int[] output = new int[inputAmount];
            if(splicedInput.Length != inputAmount)
            {
                throw invalidInput;
            }
            try
            {
               for(int i = 0; i < inputAmount; i++)
                {
                    output[i] = int.Parse(splicedInput[i]);
                }
            }
            catch
            {
                throw invalidInput;
            }
            return output;
        }
        static StreamReader sr;
        static void Main(string[] args)
        {
            Board board = new Board();
            int obstacleCount = 0;
            Coordinates start;
            Coordinates goal;
            string fileName = "2.txt";

            // Opening file
            try
            {
                sr = new StreamReader(@"..\..\..\vstupni_soubory\" + fileName);
            }
            catch (Exception ex) {Console.WriteLine(ex.Message); return; }

            // Loading obstacle amount
            try
            {
                obstacleCount = parseLine(sr.ReadLine(), 1)[0];
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return; }

            // Loading obstacles
            for(int i = 0;i < obstacleCount; i++)
            {
                Coordinates obstacleCoordinates;
                try
                {
                    obstacleCoordinates = new Coordinates(parseLine(sr.ReadLine(), 2));
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); return; }
                board.setCell(obstacleCoordinates, '#');
            }
            // Loading starting coordinates
            try
            {
               start  = new Coordinates(parseLine(sr.ReadLine(), 2));
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return; }
            // Loading goal coordinates
            try
            {
                goal = new Coordinates(parseLine(sr.ReadLine(), 2));
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return; }
            board.setCell(goal, 'G');
            sr.Close();

            // BFS Algorithm
            int currentDistance = 0;
            PriorityQueue<Coordinates, int> toCheck = new PriorityQueue<Coordinates, int>();
            Coordinates currentCoordinates;
            toCheck.Enqueue(start, 0);

            // Load the element with smallest distance from start
            while (toCheck.Count > 0)
            {
                toCheck.TryDequeue(out currentCoordinates, out currentDistance);
                List<Coordinates> neighnouringCoordinates = new List<Coordinates>();
                foreach(Vector moveVector in movementVectors)
                {
                    Coordinates newCoordinates = currentCoordinates;
                    if (!newCoordinates.addVector(moveVector))
                    {
                        continue;
                    }
                    // Check if new coordinates is goal
                    if(board.getCell(newCoordinates) == 'G')
                    {
                        Console.WriteLine(currentDistance + 1);
                        return;
                    }
                    // Check if new coordinates isn't an obstacle
                    else if(board.getCell(newCoordinates) != '#')
                    {
                        toCheck.Enqueue(newCoordinates, currentDistance + 1);
                    }

                    // Mark current knight coordinate as obstacle to avoid backtracking(we have already checked it with lowest possible distance, no need to check it again)
                    board.setCell(currentCoordinates, '#');
                }
            }
            Console.WriteLine("Do cíle se koněm nejde dostat.");
        }
    }
}
