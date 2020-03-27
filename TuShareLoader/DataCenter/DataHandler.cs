using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace TuShareLoader
{
    public static class DataHandler
    {
        /// <summary>
        /// 板块个个股代码的集合
        /// </summary>
        private static List<BanKuaiDta> m_HoldBanKuaiAndStocks = new List<BanKuaiDta>();

        /// <summary>
        /// 板块 - 个股 - 数据的集合
        /// </summary>
        private static Dictionary<string, Dictionary<string, List<List<string>>>> m_banKuaiStockDataDic = new Dictionary<string, Dictionary<string, List<List<string>>>>();

        public static List<BanKuaiDta> HoldBanKuaiAndStocks
        {
            get
            {
                return m_HoldBanKuaiAndStocks;
            }
        }

        public static Dictionary<string, Dictionary<string, List<List<string>>>> BanKuaiStockDataDic
        {
            get
            {
                return m_banKuaiStockDataDic;
            }
        }

        /// <summary>
        /// 获取所有文件夹-及个股的数据
        /// </summary>
        /// <returns></returns>
        public static List<BanKuaiDta> HandleBanKuaiAndStocks()
        {
            List<BanKuaiDta> banKuaiAndStocksDic = new List<BanKuaiDta>();

            //1.获取所有的板块和股票数据
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "A股板块";
            string[] files = Directory.GetFiles(filePath, "*.txt");

            foreach (string file in files)
            {

                string fileName = Path.GetFileNameWithoutExtension(file);

                List<string> stockList = new List<string>();

                //设置文件共享方式为读写，FileShare.ReadWrite，这样的话，就可以打开了
                FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "")
                        stockList.Add(line.ToString());
                }

                BanKuaiDta dat = new BanKuaiDta()
                {
                    BanKuaiName = fileName,
                    BanKuaiStockList = stockList
                };

                banKuaiAndStocksDic.Add(dat);
            }

            return m_HoldBanKuaiAndStocks = banKuaiAndStocksDic;
        }

        /// <summary>
        /// 根据板块和个股数据源获取各个股票的数据以及处理的数据
        /// </summary>
        public static void BKStockDatList(BanKuaiDta data)
        {
            Dictionary<string, List<List<string>>> stockDataDic = new Dictionary<string, List<List<string>>>();

            for (int j = 0; j < data.BanKuaiStockList.Count; j++)
            {
                //内部把token和Url，日期，数据周期，等参数封包了，只暴露了股票代码参数，默认从20190601开始的数据
                StockData sData1 = HttpHelper.PostUrl(data.BanKuaiStockList[j]);
                List<List<string>> maList = sData1.HoladMaList();

                if (maList.Count <= 0) continue;
                maList.Reverse();

                stockDataDic.Add(data.BanKuaiStockList[j], maList);
            }

            m_banKuaiStockDataDic.Add(data.BanKuaiName, stockDataDic);
        }
    }
}
