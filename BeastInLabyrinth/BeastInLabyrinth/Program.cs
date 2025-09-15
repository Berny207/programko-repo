using System.IO;
using static System.Net.Mime.MediaTypeNames;


namespace BeastInLabyrinth
{
    public class Prisera
    {
        public int x { get; set; }
        public int y { get; set; }

        /// <summary>
        /// 0 = up
        /// <para>1 = right</para>
        /// 2 = down
        /// <para>3 = left</para>
        /// </summary>
        public int direction { get; set; }

        public void krok(Bludiste bludiste)
        {
            //pokud mas zed po prave strane a misto pred sebou, jdi dopredu
            //pokud mas zed po prave strane a zed pred sebou, otoc se doleva
            //pokud nemas zed po prave strane, otoc se doprava a udelej krok dopredu(zde muzu vzdy udelat krok dopredu protoze vim, ze tam neni zed)

            //zadny jiny pripad nemuze nastat
        }


        public bool jeZedPoPraveRuce(Bludiste bludiste)
        {
            return false;
        }
    }
    public class Bludiste
    {
        public int WIDTH { get; set; }
        public int HEIGHT { get; set; }

        public List<List<char>> radky = new List<List<char>>();

        public Prisera najdiPriseru()
        {
            Prisera prisera = new Prisera();
            for(int y = 0; y < HEIGHT; y++)
            {
                for(int x = 0;x < WIDTH; x++)
                {
                    if(radky[y][x] != '.' && radky[y][x] != 'X')
                    {
                        prisera.x = x;
                        prisera.y = y;
                        switch (radky[y][x])
                        {
                            case '^':
                                prisera.direction = 0;
                                break;
                            case '>':
                                prisera.direction = 1;
                                break;
                            case 'v':
                                prisera.direction = 2;
                                break;
                            case '<':
                                prisera.direction = 3;
                                break;
                        }
                    }
                }
            }
            return prisera;
        }

        public void vytiskniBludiste()
        {
            foreach (List<char> radek in radky)
            {
                Console.WriteLine(radek.ToArray());
            }
        }
    }



    internal class Program
    {
        static public string MoveUpDirectory(string path)
        {
            if(path == null)
            {
                return path;
            }
            while (path[path.Length - 1] != '\\')
            {
                path = path.Remove(path.Length - 1);
            }
            return path;
        }

        static void Main(string[] args)
        {

            // get StreamReader for loading from file
            string inputPath = MoveUpDirectory(System.Environment.ProcessPath)+"bludiste.txt";
            StreamReader loader = new StreamReader(inputPath);

            Bludiste bludiste = new Bludiste();
            bludiste.WIDTH = int.Parse(loader.ReadLine());
            bludiste.HEIGHT = int.Parse(loader.ReadLine());
            string loadedLine;
            while((loadedLine = loader.ReadLine()) != null)
            {
                bludiste.radky.Add(loadedLine.ToList());
            }
            bludiste.vytiskniBludiste();
            Prisera prisera = bludiste.najdiPriseru();
        }
    }
}
