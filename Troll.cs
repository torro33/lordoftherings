using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZH2B
{
    class Troll
    {
        bool lepettMar;

        public bool LepettMar
        {
            get { return lepettMar; }
            set { lepettMar = value; }
        }

        public void GimlitLegyoz(int x, int y, int maxFutasiIdo)
        {
            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; j < 1; j++)
                {
                    if ((i + x) >= 0 && (i + x) < Szimulacio.Palya.GetLength(0) && (j + y) >= 0 && (j + y) < Szimulacio.Palya.GetLength(1))
                    {
                        if (Szimulacio.Palya[x + i, y + j].MezonTorpe != null)
                        { 
                            Console.WriteLine("Gimlit elkapta egy troll!");
                            Szimulacio.SzimulacioVege(Szimulacio.Palya[x + i, y + j].MezonTorpe);
                        }
                    }
                }
            }
        }
    }
}
