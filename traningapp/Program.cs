using System;
using System.Collections.Generic;
using System.IO;

namespace traningapp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Movement> movements = new List<Movement>();

            try
            {
                StreamReader sr = new StreamReader("mozgas.txt");
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split('\t');
                    if (parts.Length == 3)
                    {
                        if (DateTime.TryParse(parts[0], out DateTime date))
                        {
                            Movement movement = new Movement(date, parts[1], int.Parse(parts[2]));
                            movements.Add(movement);
                        }
                    }
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading the file: " + e.Message);
            }

            // Process the data
            Console.WriteLine("Movements data:");
            foreach (Movement movement in movements)
            {
                Console.WriteLine($"Date: {movement.Date}, Type: {movement.Type}, Duration: {movement.Duration} minutes");
            }
        }
    }
}