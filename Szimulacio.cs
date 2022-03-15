using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZH2B
{
    static class Szimulacio
    {
        static Torpe gimli;
        static Random rnd = new Random();

        static Mezo[,] palya;

        static public Mezo[,] Palya
        {
            get { return palya; }
            set { palya = value; }
        }
        static int maxFutasiIdo = 50;
        static int MLetrehozasiIdo = Program.M;
        static int LegolasPontjai = 0;

        public static void Futtatas()
        {
            Console.WriteLine("Szeretnél betölteni mentést? y/n");
            if(Console.ReadLine() == "y" )
            { FajlbolBetolt(); }
            else
            { Feltolt(); }

            Console.Clear();
            Megjelenit();
            while (maxFutasiIdo > 0)
            {
                if (MLetrehozasiIdo == 0)
                {
                    OrkokMegjelennek();
                    MLetrehozasiIdo = Program.M;
                }
                TrollokLepnek();
                TrollTamad();
                GimliLep();
                GimliHarcol();
                Console.Clear();
                Megjelenit();
                FajlbaKiir();
                MLetrehozasiIdo--;
                maxFutasiIdo--;

            }
            SzimulacioVege(gimli);
        }

        public static void Feltolt()
        {
            palya = new Mezo[Program.N, Program.N];

            for (int i = 0; i < palya.GetLength(0); i++)
            {
                for (int j = 0; j < palya.GetLength(1); j++)
                {
                    palya[i, j] = new Mezo();
                }
            }

            int db = (Program.N * Program.N) / 10;
            while (db > 0)
            {
                int i = rnd.Next(0, Program.N);
                int j = rnd.Next(0, Program.N);
                if (palya[i, j].MezonOrk == null)
                {
                    if (rnd.Next(0, 2) % 2 == 1)
                    { palya[i, j].MezonOrk = new Ork(Tipus.Mordori); }
                    else
                    { palya[i, j].MezonOrk = new Ork(Tipus.Urukhai); }
                    db--;
                }
            }

            palya[Program.N / 2, 0].MezonTroll = new Troll();
            palya[Program.N / 2, Program.N - 1].MezonTroll = new Troll();

            palya[Program.N / 2, Program.N / 2].MezonTorpe = new Torpe();
            gimli = palya[Program.N / 2, Program.N / 2].MezonTorpe;
        }

        public static void Megjelenit()
        {
            for (int i = 0; i < palya.GetLength(0); i++)
            {
                for (int j = 0; j < palya.GetLength(1); j++)
                {
                    if (palya[i, j].MezonTorpe != null)
                    {
                        Console.Write("G");
                    }
                    else if (palya[i, j].MezonTroll != null)
                    {
                        Console.Write("T");
                    }
                    else if (palya[i, j].MezonOrk != null)
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("\n");
            }
        }



        public static void GimliHarcol()
        {
            for (int i = 0; i < palya.GetLength(0); i++)
            {
                for (int j = 0; j < palya.GetLength(1); j++)
                {
                    if (palya[i, j].MezonTorpe != null)
                    {
                        GimliHarcolEllenoriz(i,j);
                    }
                }
            }
        }

        public static void GimliHarcolEllenoriz(int x, int y)
        {
            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; j < 1; j++)
                {
                    if ((i + x) >= 0 && (i + x) < palya.GetLength(0) && (j + y) >= 0 && (j + y) < palya.GetLength(1))
                    {
                        if(palya[x+i,y+j].MezonOrk != null)
                        {

                            if (rnd.Next(0, 2) % 2 == 1)
                            {
                                if (palya[x + i, y + j].MezonOrk.Faj == Tipus.Urukhai)
                                {
                                    palya[x, y].MezonTorpe.LegyozottOrkok += 2;
                                }
                                else
                                {
                                    palya[x, y].MezonTorpe.LegyozottOrkok++;
                                }
                            }
                            else
                            {
                                LegolasPontjai++;
                            }
                            palya[x + i, y + j].MezonOrk = null;
                        }
                    }
                }
            }
        }

        public static void GimliLep()
        {

            bool valid = false;
            while (!valid)
            {
                ConsoleKeyInfo k = Console.ReadKey();
                if (k.Key == ConsoleKey.UpArrow)
                {
                    if (GimliLepEllenoriz(-1, 0))
                    { valid = true; }
                }
                else if (k.Key == ConsoleKey.DownArrow)
                {
                    if (GimliLepEllenoriz(1, 0))
                    { valid = true; }
                }
                else if (k.Key == ConsoleKey.RightArrow)
                {
                    if (GimliLepEllenoriz(0, 1))
                    { valid = true; }
                }
                else if (k.Key == ConsoleKey.LeftArrow)
                {
                    if (GimliLepEllenoriz(0, -1))
                    { valid = true; }
                }
            }


        }

        public static bool GimliLepEllenoriz(int x, int y)
        {
            for (int i = 0; i < palya.GetLength(0); i++)
            {
                for (int j = 0; j < palya.GetLength(1); j++)
                {
                    if (palya[i, j].MezonTorpe != null)
                    {
                        if ((i + x) >= 0 && (i + x) < palya.GetLength(0) && (j + y) >= 0 && (j + y) < palya.GetLength(1))
                        {
                            palya[i + x, j + y].MezonTorpe = palya[i, j].MezonTorpe;
                            palya[i, j].MezonTorpe = null;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static void TrollokLepnek()
        {
            int x = 0;
            int y = 0;

            bool valid = false;
            for (int i = 0; i < palya.GetLength(0); i++)
            {
                for (int j = 0; j < palya.GetLength(1); j++)
                {
                    valid = false;
                    if (palya[i, j].MezonTroll != null && !palya[i, j].MezonTroll.LepettMar)
                    {
                        while (!valid)
                        {
                            switch (rnd.Next(0, 4))
                            {
                                case 0: x = 0; y = -1;
                                    break;
                                case 1: x = 0; y = 1;
                                    break;
                                case 2: x = -1; y = 0;
                                    break;
                                case 3: x = 1; y = 0;
                                    break;
                            }
                            if ((i + x) >= 0 && (i + x) < palya.GetLength(0) && (j + y) >= 0 && (j + y) < palya.GetLength(1))
                            {

                                palya[i, j].MezonTroll.LepettMar = true;
                                palya[i + x, j + y].MezonTroll = palya[i, j].MezonTroll;
                                palya[i, j].MezonTroll = null;
                                valid = true;
                            }
                        }
                    }
                }
            }

            foreach (Mezo m in palya)
            {
                if (m.MezonTroll != null)
                {
                    m.MezonTroll.LepettMar = false;
                }
            }
        }

        public static void TrollTamad()
        {
            for (int i = 0; i < palya.GetLength(0); i++)
            {
                for (int j = 0; j < palya.GetLength(1); j++)
                {
                    if (palya[i, j].MezonTroll != null)
                    {
                        palya[i, j].MezonTroll.GimlitLegyoz(i, j, maxFutasiIdo);
                    }
                }
            }
        }



        public static void OrkokMegjelennek()
        {
            int db = (Program.N * Program.N) / 10;
            while (db > 0)
            {
                int i = rnd.Next(0, Program.N);
                int j = rnd.Next(0, Program.N);
                if (palya[i, j].MezonOrk == null)
                {
                    if (rnd.Next(0, 2) % 2 == 1)
                    { palya[i, j].MezonOrk = new Ork(Tipus.Mordori); }
                    else
                    { palya[i, j].MezonOrk = new Ork(Tipus.Urukhai); }
                    db--;
                }
            }
        }

        public static void SzimulacioVege(Torpe g)
        {
            Console.WriteLine("Gimli pontszáma: " + g.LegyozottOrkok);
            Console.WriteLine("Legolas pontszáma: " + LegolasPontjai);
            maxFutasiIdo = 0;
        }

        public static void FajlbaKiir()
        {
            StreamWriter sw = new StreamWriter("mentes.txt");
            sw.WriteLine(gimli.LegyozottOrkok);
            for (int i = 0; i < palya.GetLength(0); i++)
            {
                string sor = "";

                for (int j = 0; j < palya.GetLength(1); j++)
                {
                    string objektumTipus = "";
                    Mezo temp = palya[i, j];
                    if(temp.MezonOrk != null)
                    { objektumTipus += "1;"; }
                    if(temp.MezonTorpe != null)
                    { objektumTipus += "2;"; }
                    if(temp.MezonTroll != null)
                    { objektumTipus += "3;"; }
                    if(objektumTipus == "")
                    { objektumTipus += "0"; }
                    else
                    { objektumTipus = objektumTipus.Remove(objektumTipus.Length - 1, 1); }

                    sor += objektumTipus + ",";

                }
                sor = sor.Remove(sor.Length - 1, 1);
                sw.WriteLine(sor);
            }
            sw.Close();
        }

        public static void FajlbolBetolt()
        {
            palya = new Mezo[Program.N, Program.N];

            for (int i = 0; i < palya.GetLength(0); i++)
            {
                for (int j = 0; j < palya.GetLength(1); j++)
                {
                    palya[i, j] = new Mezo();
                }
            }

            StreamReader sr = new StreamReader("mentes.txt");
            int legyozottOrkok = int.Parse(sr.ReadLine());
            int db = 0;
            while (!sr.EndOfStream)
            {
                string sor = sr.ReadLine();
                string[] adatok = sor.Split(',');
                for (int i = 0; i < adatok.Length; i++)
                {
                    if(adatok[i].Length > 1)
                    {
                        string[] tipusok = adatok[i].Split(';');
                        for (int x = 0; x < tipusok.Length; x++)
                        {
                            switch(tipusok[x])
                            {
                                case "1": palya[db, i].MezonOrk = new Ork(Tipus.Mordori);
                                    break;
                                case "2": palya[db, i].MezonTorpe = new Torpe(); gimli = palya[db, i].MezonTorpe;
                                    break;
                                case "3": palya[db, i].MezonTroll = new Troll();
                                    break;
                            }
                        }
                    }
                    else
                    {
                        switch (adatok[i])
                        {
                            case "1":
                                palya[db, i].MezonOrk = new Ork(Tipus.Mordori);
                                break;
                            case "2":
                                palya[db, i].MezonTorpe = new Torpe(); gimli = palya[db, i].MezonTorpe;
                                break;
                            case "3":
                                palya[db, i].MezonTroll = new Troll();
                                break;
                        }
                    }
                }
                db++;
            }
            gimli.LegyozottOrkok = legyozottOrkok;
            sr.Close();
        }
    }
}
