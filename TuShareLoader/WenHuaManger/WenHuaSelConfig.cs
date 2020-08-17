using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuShareLoader
{
    public class WenHuaSelConfig
    {
        /// <summary>
        /// 板块-个股列表
        /// </summary>
        public Dictionary<string, List<string>> BanKuaiAndGegu
        {
            get;
            set;
        }

        /// <summary>
        /// 个股-数据列表
        /// </summary>
        public Dictionary<string, List<MarketData>> GeguData
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }


        private static WenHuaSelConfig instance = null;
        private static readonly object padlock = new object();

        private WenHuaSelConfig()
        {
        }

        public static WenHuaSelConfig Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new WenHuaSelConfig();
                    }
                    return instance;
                }
            }
        }



    }
}
