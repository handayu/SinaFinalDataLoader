using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuShareLoader
{
    public class RequestData
    {
        public string api_name;

        public string token;

        public DataParams paramsQ;

        public string fields;
    }

    public class DataParams
    {
        public string ts_code;

        public string start_date;

        public string end_date;
    }

    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> fields { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<List<string>> items { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string has_more { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string request_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
    }


    public class StockData
    {
        //证券名称
        public string stockName { get; set; }

        //证券数据
        public List<List<string>> items { get; set; }

        //均线指标
        public List<List<string>> HoladMaList()
        {
            if (items != null)
            {
                List<List<string>> paramsMaList = new List<List<string>>();

                for (int i = 0; i < items.Count - 80; i++)
                {
                    List<string> maParams = new List<string>();
                    maParams.Add(stockName);
                    maParams.Add(items[i][1]);
                    maParams.Add(items[i][5]);
                    double sum = 0.00;
                    double avg = 0.00;
                    for (int j = i; j < i + 60; j++)
                    {
                        double maTemp = 0.00;
                        double.TryParse(items[j][5], out maTemp);
                        sum = sum + maTemp;
                    }
                    avg = sum / 60;
                    maParams.Add(avg.ToString());

                    double price = 0.00;
                    double.TryParse(items[i][5], out price);
                    double radio = (price - avg) / avg;
                    maParams.Add(radio.ToString());

                    paramsMaList.Add(maParams);
                }

                return paramsMaList;
            }
            return null;
        }
    }

}
