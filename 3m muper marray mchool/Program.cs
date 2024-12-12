using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3m_muper_marray_mchool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Random rnd = new Random();
            int why = 20;
            int ecks = 20;
            char[,] posits = new char[ecks, why];
            bool ended = false;
                string startcoords = rnd.Next(0, ecks) + "," + rnd.Next(0, why);
            string endcoords = startcoords;
            while (endcoords == startcoords)
            {
                endcoords = rnd.Next(0, ecks) + "," + rnd.Next(0, why);
            }
            
            string[] strings = startcoords.Split(',');
            int xcoord = Convert.ToInt32(strings[0]);
            int ycoord = Convert.ToInt32(strings[1]);
            posits[xcoord, ycoord] = '#';
            strings = endcoords.Split(',');
            xcoord = Convert.ToInt32(strings[0]);
            ycoord = Convert.ToInt32(strings[1]);
            posits[xcoord, ycoord] = 'X';
            List<string> blockades = new List<string>();
            for (int i = 0; i < 80; i++)
            {
                blockades.Add(startcoords);
                int randx = 0;
                int randy = 0;
                while (blockades.Last() == startcoords || blockades.Last() == endcoords)
                {
                    randx = rnd.Next(0, ecks);
                    randy = rnd.Next(0, why);
                    blockades.RemoveAt(blockades.Count -1);
                    blockades.Add(randx + "," + randy);
                }
                posits[randx, randy] = '█';
            }
            gridmake(ended);
            Stack<string> pathway = new Stack<string>();
            List<string> potential = new List<string>();
            pathway.Push(startcoords);
            
                while (ended == false)
                {

                    Console.Clear();
                    potential.Clear();
                    int[,] gunit = new int[ecks, why]; //distance from start
                    int[,] hunit = new int[ecks, why]; //displacement to end
                    int[,] tunit = new int[ecks, why]; //total
                    int[] currentcoords = new int[2];

                    while (potential.Count == 0)
                    {
                        try
                        {
                            string[] currentspace = pathway.Peek().Split(',');
                            //Console.WriteLine(pathway.Peek());
                            currentcoords[0] = Convert.ToInt32(currentspace[0]);
                            currentcoords[1] = Convert.ToInt32(currentspace[1]);

                            if (currentcoords[0] + 1 < ecks && !(pathway.Contains(((currentcoords[0] + 1) + "," + (currentcoords[1])))))
                                potential.Add((currentcoords[0] + 1) + "," + (currentcoords[1]));

                            if (currentcoords[0] - 1 > -1 && !(pathway.Contains(((currentcoords[0] - 1) + "," + (currentcoords[1])))))
                                potential.Add((currentcoords[0] - 1) + "," + (currentcoords[1]));

                            if (currentcoords[1] + 1 < why && !(pathway.Contains(((currentcoords[0]) + "," + (currentcoords[1] + 1)))))
                                potential.Add((currentcoords[0]) + "," + (currentcoords[1] + 1));

                            if (currentcoords[1] - 1 > -1 && !(pathway.Contains(((currentcoords[0]) + "," + (currentcoords[1] - 1)))))
                                potential.Add((currentcoords[0]) + "," + (currentcoords[1] - 1));

                            foreach (string block in blockades)
                            {
                                for (int i = 0; i < potential.Count; i++)
                                {
                                    if (potential[i] == block)
                                    {
                                        potential.Remove(block);
                                    }
                                }
                            }

                            if (potential.Count == 0)
                            {
                                posits[currentcoords[0], currentcoords[1]] = '!';
                                blockades.Add(pathway.Pop());
                            }
                        }
                        catch (Exception xe)
                        {

                            Console.WriteLine("Unsolvable maze");
                            ended = true;
                            potential.Add("99,99");
                        }
                    }
                    if (ended == false)
                    {
                        for (int i = 0; i < potential.Count; i++)
                        {
                            //Console.WriteLine(potential[i]);
                            string[] named = potential[i].Split(',');
                            posits[Convert.ToInt32(named[0]), Convert.ToInt32(named[1])] = '?';
                            if (potential[i] == endcoords)
                            {
                                pathway.Push(potential[i]);
                                posits[Convert.ToInt32(named[0]), Convert.ToInt32(named[1])] = 'X';
                                ended = true;
                                i = potential.Count;
                            }
                            else
                            {
                                gunit[Convert.ToInt32(named[0]), Convert.ToInt32(named[1])] = pathway.Count * 100;
                                double temp;
                                if (Convert.ToInt32(named[0]) > xcoord)
                                {
                                    temp = Math.Pow(Convert.ToInt32(named[0]) - xcoord, 2);
                                }
                                else
                                {
                                    temp = Math.Pow(xcoord - Convert.ToInt32(named[0]), 2);
                                }
                                if (Convert.ToInt32(named[1]) > ycoord)
                                {
                                    temp += Math.Pow(Convert.ToInt32(named[1]) - ycoord, 2);
                                }
                                else
                                {
                                    temp += Math.Pow(ycoord - Convert.ToInt32(named[1]), 2);
                                }
                                temp = Math.Sqrt(temp) * 100;
                                hunit[Convert.ToInt32(named[0]), Convert.ToInt32(named[1])] = Convert.ToInt32(Math.Round(temp));
                                tunit[Convert.ToInt32(named[0]), Convert.ToInt32(named[1])] = gunit[Convert.ToInt32(named[0]), Convert.ToInt32(named[1])] + hunit[Convert.ToInt32(named[0]), Convert.ToInt32(named[1])];
                            }

                        }
                        //gridmake(ended);
                        if (ended == false)
                        {
                            for (int i = 0; i < potential.Count; i++)
                            {
                                string[] named = potential[i].Split(',');
                                int[] shamed = new int[named.Length];
                                shamed[0] = Convert.ToInt32(named[0]);
                                shamed[1] = Convert.ToInt32(named[1]);
                                //Console.WriteLine(potential[i] + " ; " + tunit[shamed[0], shamed[1]] + "(" + gunit[shamed[0], shamed[1]] + "/" + hunit[shamed[0], shamed[1]] + ")");
                            }
                            int topofpop = int.MaxValue;
                            string chosenone = "if this gets outputted thats not good";
                            for (int y = 0; y < why; y++)
                            {
                                for (int x = 0; x < ecks; x++)
                                {
                                    //Console.WriteLine(tunit[x, y]);
                                    if ((tunit[x, y] <= topofpop) && (tunit[x, y] != 0))
                                    {
                                        topofpop = tunit[x, y];
                                        chosenone = x + "," + y;
                                        //Console.WriteLine(chosenone);
                                    }
                                }
                            }
                            pathway.Push(chosenone);
                            string[] strung = chosenone.Split(',');
                            //Console.WriteLine(strung[0]);
                            //Console.WriteLine(strung[1]);
                            int xc = Convert.ToInt32(strung[0]);
                            int yc = Convert.ToInt32(strung[1]);
                            posits[xc, yc] = 'O';
                            gridmake(ended);
                        }
                    
                }
                gridmake(ended);
                Console.WriteLine(pathway.Count);
                //Console.ReadLine();
            }
            void gridmake(bool good)
            {
                Console.Write("┌");
                for (int i = 0; i < ecks; i++)
                {
                    Console.Write("─");
                }
                Console.Write("┐");
                Console.WriteLine();
                for (int y = 0; y < why; y++)
                {
                    for (int x = 0; x < ecks; x++)
                    {
                        if (x == 0)
                        {
                            Console.Write("│");
                        }
                        if (good == false)
                        {    
                            if (posits[x, y] == 'O' || posits[x, y] == '#')
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            }
                            if (posits[x, y] == 'X')
                            {
                                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                            }
                            if (posits[x, y] == '!')
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed ;
                            }
                        }
                        else
                        {
                            if (posits[x, y] == 'O' || posits[x, y] == '#' || posits[x, y] == 'X')
                            {
                                Console.BackgroundColor = ConsoleColor.Green;
                            }
                            if (posits[x, y] == '!')
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                            }
                        }
                        Console.Write(posits[x, y]);
                        Console.BackgroundColor = ConsoleColor.Black;
                        if (x == ecks-1)
                        {
                            Console.Write("│");
                        }
                        
                    }
                    Console.WriteLine();
                }
                Console.Write("└");
                for (int i = 1; i <= ecks; i++)
                {
                    Console.Write("─");
                }
                Console.Write("┘");
                Console.WriteLine();
            }
        }
    }
}
