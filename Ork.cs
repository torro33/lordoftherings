using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZH2B
{
    enum Tipus
    { 
        Mordori,
        Urukhai
    }

    class Ork
    {
        Tipus faj;

        internal Tipus Faj
        {
            get { return faj; }
            set { faj = value; }
        }

        public Ork(Tipus faj)
        {
            this.faj = faj;
        }

    }
}
