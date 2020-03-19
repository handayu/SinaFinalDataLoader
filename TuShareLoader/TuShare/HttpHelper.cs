using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TuShareLoader
{
    public static class HttpHelper
    {
        //{
        //    "api_name": "daily",
        //    "token":"eea049d7d60973c01823ea6593ced55386e676abcda5b7b44bb19995",
        //    "params":{"ts_code":"000001.SZ","start_date":"20180701", "end_date":"20190718"},
        //    "fields":""
        //}

        //result
        //ts_code trade_date  open high   low close  pre_close change    pct_chg vol        amount

        //ts_code str N 股票代码（支持多个股票同时提取，逗号分隔)
        //trade_date str N 交易日期（YYYYMMDD）
        //start_date str N 开始日期(YYYYMMDD)
        //end_date str N 结束日期(YYYYMMDD)

        public static StockData PostUrl(string stockCode)
        {
            // url:POST请求地址
            // postData:json格式的请求报文,例如：{"key1":"value1","key2":"value2"}
            // 发送的链接包含在代码里，因为不变
            // stockCode = "000001.SZ"
            string year = DateTime.Now.ToString("yyyy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");

            string dateToday = year + month + day;

            //在这里把日期默认到当天end_day
            DataParams DP = new DataParams()
            {
                ts_code = stockCode,
                start_date = "20190101",
                end_date = dateToday
            };

            RequestData dataReq = new RequestData()
            {
                api_name = "daily",
                token = "eea049d7d60973c01823ea6593ced55386e676abcda5b7b44bb19995",
                paramsQ = DP,
                fields = ""
            };

            string json1 = JsonDataContractJsonSerializer.SerializeObject(dataReq);
            json1 = json1.Replace("Q", "");

            //
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(@"http://api.waditu.com");
            req.Method = "POST";
            req.Timeout = 6000;//设置请求超时时间，单位为毫秒
            req.ContentType = "text/plain";
            byte[] data = Encoding.UTF8.GetBytes(json1);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            //反序列化之后返回
            Root rootData = JsonDataContractJsonSerializer.DeserializeJsonToObject<Root>(result);

            StockData sD = new StockData();
            sD.stockName = stockCode;
            sD.items = rootData.data.items;

            return sD;
        }


    }
}
