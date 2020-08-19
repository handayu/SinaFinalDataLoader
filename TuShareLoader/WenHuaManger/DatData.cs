using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuShareLoader
{
    /// <summary>
    /// Info个股信息
    /// </summary>
    public class Level2Info
    {
        public float WenHuaCode
        {
            get;
            set;
        }

        public UInt16 DatCode
        {
            get;
            set;
        }

        public string Instrument
        {
            get;
            set;
        }

        public string DataCodeStr
        {
            get
            {
                //这里要参考文华反解码出来的行情代码，如果是5位，前面补3个000，如果是4位，前面补4个0.保证一共有8位
                if(DatCode.ToString().Length == 5)
                {
                    return "000" + DatCode.ToString();
                }

                if (DatCode.ToString().Length == 4)
                {
                    return "0000" + DatCode.ToString();
                }

                return "";
            }
        }

        public override string ToString()
        {
            return Instrument.Replace("\0","").Trim() + " " + DataCodeStr;
        }
    }

    /// <summary>
    /// 行情数据
    /// </summary>
    public class MarketData
    {
        public DateTime DateTimeNum
        {
            get
            {
                //转datetime - 转double
                DateTime tM = ConvertToDateTime(HQTime);
                return tM;
            }
        }

        public Int32 HQTime
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

        public float MADiff
        {
            get;
            set;
        }

        public float Avg
        {
            get;
            set;
        }

        /// <summary>
        /// 计算Ma
        /// </summary>
        /// <param name="mDList"></param>
        public static void CalMaDiff(List<MarketData> mDList)
        {
            for(int i = mDList.Count - 1; i >= mDList.Count - 500; i--)
            {
                float sum = 0;
                float avg = 0;
                for (int j = i; j >= i - 39; j--)
                {
                    sum = sum + mDList[j].Close;
                }

                avg = sum / 40;

                mDList[i].Avg = avg;
                mDList[i].MADiff = (mDList[i].Close - avg) / avg;
            }
        }

        private DateTime ConvertToDateTime(Int32 d)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0);
            startTime = startTime.AddSeconds(d).ToLocalTime();
            return startTime;
        }
    }

    public class DatDataManager
    {
        public Dictionary<string, Dictionary<string, string>> BankuaiGeguPathDic = new Dictionary<string, Dictionary<string, string>>();


        private static DatDataManager instance = null;
        private static readonly object padlock = new object();

        private DatDataManager()
        {
        }

        public static DatDataManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DatDataManager();
                    }
                    return instance;
                }
            }
        }

    }
}
