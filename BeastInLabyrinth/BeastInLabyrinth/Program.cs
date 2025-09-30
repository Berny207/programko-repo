using BeastInLabyrinth;
using System;
using System.Collections.Generic;
using static Util.Util;

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
		/// <summary>
		/// monster's movement options
		/// </summary>
		private int[,] pohybovaMatice = { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
		private char[] znaceni = { '^', '>', 'v', '<' };
		private bool pohybVpred = false;
		public void Krok(Bludiste bludiste)
		{
			bludiste.radky[y][x] = '.';
			if (pohybVpred == true)
			{
				pohybVpred = false;
				x = x + pohybovaMatice[direction, 0];
				y = y + pohybovaMatice[direction, 1];

			}
			else
			{
				bool jeZedPred = JeZed(bludiste, 0);
				bool jeZedVpravo = JeZed(bludiste, 1);
				if (jeZedVpravo == false)
				{
					pohybVpred = true;
					direction = Modulo((direction + 1), 4);
				}
				if (jeZedPred == true && jeZedVpravo == true)
				{
					direction = Modulo((direction - 1), 4);
				}
				if (jeZedPred == false && jeZedVpravo == true)
				{
					x = x + pohybovaMatice[direction, 0];
					y = y + pohybovaMatice[direction, 1];
				}
			}
			bludiste.radky[y][x] = znaceni[direction];
		}

		/// <summary>
		/// returns if there is a wall around the monster in target direction
		/// <para>Direction is the same as monster's direction</para>
		/// </summary>
		/// <param name="bludiste"></param>
		/// <param name="smer"></param>
		/// <returns></returns>
		public bool JeZed(Bludiste bludiste, int smer)
		{
			int pravaRukaX = x + pohybovaMatice[(direction + smer) % (pohybovaMatice.Length / 2), 0];
			int pravaRukaY = y + pohybovaMatice[(direction + smer) % (pohybovaMatice.Length / 2), 1];
			if (bludiste.radky[pravaRukaY][pravaRukaX] == 'X')
			{
				return true;
			}
			else
			{
				return false;
			}
		}
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
		for (int y = 0; y < HEIGHT; y++)
		{
			for (int x = 0; x < WIDTH; x++)
			{
				if (radky[y][x] != '.' && radky[y][x] != 'X')
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
		Console.WriteLine();
	}
}



internal class Program
{
	static void Main(string[] args)
	{
		Bludiste bludiste = new Bludiste();
		bludiste.WIDTH = int.Parse(Console.ReadLine());
		bludiste.HEIGHT = int.Parse(Console.ReadLine());
		string loadedLine;
		for (int i = 0; i < bludiste.HEIGHT; i++)
		{
			bludiste.radky.Add(Console.ReadLine().ToList());
		}

		Prisera prisera = bludiste.najdiPriseru();

		int N = 20;
		for (int i = 0; i < N; i++)
		{
			prisera.Krok(bludiste);
			bludiste.vytiskniBludiste();
		}
	}
}


namespace Util
{
	public static class Util
	{
		public static int Modulo(int num1, int num2)
		{
			//we add n times num2 to num1, where n is |num1|.num2+1. +1 because we are rounding down, new num1 > 0
			num1 = num1 + num2 * (Math.Abs(num1) / num2 + 1);
			return num1 % num2;
		}
	}
}