using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuShareLoader
{
    public class MarketData
    {
        public string Instrument
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public string HQTime
        {
            get;
            set;
        }
        public float Open
        {
            get;
            set;
        }
        public float High
        {
            get;
            set;
        }
        public float Low
        {
            get;
            set;
        }
        public float Close
        {
            get;
            set;
        }

        public float Volumn
        {
            get;
            set;
        }
        public float Intest
        {
            get;
            set;
        }
        public float SettlePrice
        {
            get;
            set;
        }
        public float Unknow
        {
            get;
            set;
        }
    }
}
