using System.Threading.Tasks;

namespace Budget_MaSo
{
    internal class Program
    {
        public static Random random = new Random();
        public static Action[] EasyTasks = new Action[] {AdditionTask, SubtractionTask, MultiplicationTask };
		public static Action[] MediumTasks = new Action[] { DivisionTask, SquareTask, TriangleTask};
        public static Action[] HardTasks = new Action[] { ModuloTask, PowerTask, PythagoryanTheorem};
		public static int points = 0;
        public const int EASY_POINTS = 10;
        public const int MEDIUM_POINTS = 20;
        public const int HARD_POINTS = 40;
        public const int TICKET_COST = 80;
		static void ProcessInput(string args)
        {
            switch (args)
            {
                case "0":
                    EndProgram();
                    break;
                case "1":
                    EasyTask();
                    break;
                case "2":
                    MediumTask();
                    break;
                case "3":
                    HardTask();
                    break;
                case "lístek":
                    if (points >= TICKET_COST)
                    {
                        points -= TICKET_COST;
                        Console.WriteLine($"Lístek zakoupen! Máte {points} bodů.");
                    }
                    else
                    {
                        Console.WriteLine($"Nemáte dostatek bodů pro nákup lístku. Potřebujete ještě {TICKET_COST - points} bodů.");
                    }
                    break;
				default:
                    Console.WriteLine("Neplatný vstup. Zadejte 0 pro ukončení programu, 1 pro snadný úkol, 2 pro střední úkol nebo 3 pro těžký úkol. Zadejte lístek pokud si chcete koupit lístek za 50 bodů");
                    break;
			}
        }
        static void EndProgram()
        {
            Environment.Exit(67);
		}
        static void EasyTask()
        {
			int randomIndex = random.Next(EasyTasks.Length);
			EasyTasks[randomIndex]();
		}
        static void AdditionTask()
        {
			int num1 = random.Next(100, 100000);
			int num2 = random.Next(100, 100000);
            Console.WriteLine($"{num1} + {num2}");
            int solution = num1 + num2;
            EvaluateTask(solution, EASY_POINTS);
		}
        static void SubtractionTask()
        {
			int num1 = random.Next(100, 100000);
			int num2 = random.Next(100, 100000);
            if(num2 > num1)
            {
                int temp = num1;
                num1 = num2;
                num2 = temp;
			}
			Console.WriteLine($"{num1} - {num2}");
			int solution = num1 - num2;
			EvaluateTask(solution, EASY_POINTS);
		}
        static void MultiplicationTask()
        {
			int num1 = random.Next(10, 100);
			int num2 = random.Next(10, 30);
			Console.WriteLine($"{num1} * {num2}");
			int solution = num1 * num2;
			EvaluateTask(solution, EASY_POINTS);
		}
        static void DivisionTask()
        {
			int num1 = random.Next(1, 10);
			int solution = random.Next(0, 10);
			Console.WriteLine($"{num1*solution} / {num1}");
			EvaluateTask(solution, MEDIUM_POINTS);
		}
        static void SquareTask()
        {
            int num = random.Next(1, 20);
            Console.WriteLine($"Vypočítej obvod a obsah čtverce, jehož délka strany je {num} cm. Výsledek zapiš jako obvodovsah");
            Console.WriteLine($"Například pro stranu 3 cm by správná odpověď byla 129, protože obvod je 4*3=12 a obsah je 3*3=9.");
            int O = 4 * num;
            int S = num * num;
			string combined = O.ToString() + S.ToString();
			int solution = int.Parse(combined);
			EvaluateTask(solution, MEDIUM_POINTS);
		}
        static void TriangleTask()
        {
            int a = random.Next(1, 20);
            int b = random.Next(1, 20);
			int c = random.Next(1, 20);
            Console.WriteLine($"Vypočítej obvod trojúhelníku s délkami stran {a} cm, {b} cm a {c} cm. Pokud délky stran nedávají validní trojúhelník, napiš obvod 0.");
            int solution = 0;
            if (a + b > c && a + c > b && b + c > a)
            {
                solution = a + b + c;
            }
            EvaluateTask(solution, MEDIUM_POINTS);
		}
        static void ModuloTask()
        {
			int num1 = random.Next(10, 100);
			int num2 = random.Next(0, 10);
            Console.WriteLine($"{num1} % {num2}");
            int solution = num1 % num2;
            EvaluateTask(solution, HARD_POINTS);
		}
        static void PowerTask()
        {
            int solution = random.Next(1, 10);
            Console.WriteLine($"Jaké číslo musíme vynásobit samo sebou, abychom dostali {solution*solution}?");
            EvaluateTask(solution, HARD_POINTS);
		}
        static void PythagoryanTheorem()
        {
            int m = random.Next(1, 7);
            int n = random.Next(1, 7);
			if (n > m)
			{
				int temp = m;
				m = n;
				n = temp;
			}
			int a = m * m - n * n;
            int b = 2 * m * n;
			Console.WriteLine($"Vypočítej obvod trojúhelníku, jehož dvě kratší strany mají délky {a} cm a {b} cm a jehož jeden úhel má velikost 90°.");
            EvaluateTask(m*m+n*n+a+b, HARD_POINTS);
		}
		static void MediumTask()
        {
			int randomIndex = random.Next(MediumTasks.Length);
			MediumTasks[randomIndex]();
		}
        static void HardTask()
        {
			int randomIndex = random.Next(HardTasks.Length);
			HardTasks[randomIndex]();
		}
        static void EvaluateTask(int solution, int pointAward)
        {
            while (true)
            {
                string? answer = Console.ReadLine();
                try
                {
                    if (answer == null)
                    {
                        Console.WriteLine("Neplatný vstup. Zadejte číslo.");
                        continue;
					}
					int userAnswer = int.Parse(answer);
                    if (userAnswer == solution)
                    {
                        points += pointAward;
                        Console.WriteLine($"Správně! Máte {points} bodů.");
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"Špatně. Správná odpověď byla {solution}.");
                        return;
					}
                }
                catch
                {
                    Console.WriteLine("Neplatný vstup. Zadejte číslo.");
                }
            }
		}
        static void Main(string[] args)
        {
            while (true) 
            {
                Console.WriteLine($"Zadejte 0 pro ukončení programu, 1 pro snadný úkol, 2 pro střední úkol nebo 3 pro těžký úkol. Zadejte lístek pokud si chcete koupit lístek za {TICKET_COST} bodů");
				string input = Console.ReadLine();
                ProcessInput(input);
            }
        }
    }
}
