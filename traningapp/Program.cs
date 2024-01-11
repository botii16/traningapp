using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static string filePath = "C:\\Users\\HP\\Desktop\\mozgas.txt";
    static List<Edzes> edzesek = new List<Edzes>();

    static void Main(string[] args)
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("1. Új adat rögzítése");
            Console.WriteLine("2. Meglévő adatok kiíratása táblázatszerűen");
            Console.WriteLine("3. Meglévő adatok kiíratása adott edzéstípusra szűrve");
            Console.WriteLine("4. Statisztika kiíratása");
            Console.WriteLine("5. Kilépés");
            Console.Write("Válasszon egy menüpontot: ");
            string menuChoice = Console.ReadLine();

            switch (menuChoice)
            {
                case "1":
                    UjAdatRogzitese();
                    break;
                case "2":
                    AdatokKiolvasasa();
                    break;
                case "3":
                    AdatokSzures();
                    break;
                case "4":
                    StatisztikaKiiratasa();
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Érvénytelen választás. Kérem, válasszon újra.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void UjAdatRogzitese()
    {
        Console.Write("Adja meg a dátumot (éééé.hh.nn formátumban): ");
        string datum = Console.ReadLine();

        Console.Write("Adja meg az edzés típusát: ");
        string edzesTipus = Console.ReadLine();

        Console.Write("Adja meg az időtartamot (percben): ");
        int idotartam = Convert.ToInt32(Console.ReadLine());

        Edzes ujEdzes = new Edzes(datum, edzesTipus, idotartam);
        edzesek.Add(ujEdzes);

        using (StreamWriter sw = File.AppendText(filePath))
        {
            sw.WriteLine($"{datum}\t{edzesTipus}\t{idotartam}");
        }

        Console.WriteLine("Az adat sikeresen rögzítve.");
    }

    static void AdatokKiolvasasa()
    {
        Console.WriteLine("Az edzésadatok:");
        Console.WriteLine("Dátum\t\tEdzés típusa\tIdőtartam");

        using (StreamReader sr = new StreamReader(filePath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split('\t');
                Console.WriteLine($"{parts[0]}\t{parts[1]}\t\t{parts[2]}");
            }
        }
    }

    static void AdatokSzures()
    {
        Console.Write("Adja meg az edzéstípust: ");
        string edzesTipus = Console.ReadLine();

        Console.WriteLine($"Az {edzesTipus} edzések:");
        Console.WriteLine("Dátum\t\tEdzés típusa\tIdőtartam");

        using (StreamReader sr = new StreamReader(filePath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split('\t');
                if (parts[1] == edzesTipus)
                {
                    Console.WriteLine($"{parts[0]}\t{parts[1]}\t\t{parts[2]}");
                }
            }
        }
    }

    static void StatisztikaKiiratasa()
    {
        Dictionary<string, int> edzesTipusok = new Dictionary<string, int>();
        Dictionary<string, int> edzesIdok = new Dictionary<string, int>();

        using (StreamReader sr = new StreamReader(filePath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split('\t');
                string edzesTipus = parts[1];
                int idotartam = Convert.ToInt32(parts[2]);

                if (edzesTipusok.ContainsKey(edzesTipus))
                {
                    edzesTipusok[edzesTipus]++;
                    edzesIdok[edzesTipus] += idotartam;
                }
                else
                {
                    edzesTipusok.Add(edzesTipus, 1);
                    edzesIdok.Add(edzesTipus, idotartam);
                }
            }
        }

        Console.WriteLine("Statisztika:");
        Console.WriteLine("Edzéstípus\tEdzések száma\tÖsszidő\t\tÁtlagos idő\tLeghosszabb idő");

        foreach (var kvp in edzesTipusok)
        {
            string edzesTipus = kvp.Key;
            int edzesekSzama = kvp.Value;
            int osszIdo = edzesIdok[edzesTipus];
            int atlagosIdo = osszIdo / edzesekSzama;

            // Check if there are elements in the list before calling Max()
            int legHosszabbIdo = edzesek.Any(e => e.EdzesTipus == edzesTipus) ? edzesek.FindAll(e => e.EdzesTipus == edzesTipus).Max(e => e.Idotartam) : 0;

            Console.WriteLine($"{edzesTipus}\t\t{edzesekSzama}\t\t{osszIdo} perc\t\t{atlagosIdo} perc\t\t{legHosszabbIdo} perc");
        }
    }

}

class Edzes
{
    public string Datum { get; set; }
    public string EdzesTipus { get; set; }
    public int Idotartam { get; set; }

    public Edzes(string datum, string edzesTipus, int idotartam)
    {
        Datum = datum;
        EdzesTipus = edzesTipus;
        Idotartam = idotartam;
    }
}
