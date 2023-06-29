using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NezihBankMAUI.Models
{
    internal class City
    {
        public int Id { get; set; }

        public string Ad { get; set; }

        public override string ToString()
        {
            return Ad;
        }
    }

    internal class District
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public int IlId { get; set; }
        //public Il Il { get; set; }

        public override string ToString()
        {
            return Ad;
        }
    }
}
