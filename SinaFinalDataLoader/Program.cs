using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SinaFinalDataLoader
{
    class Program
    {
        //历史数据
        //新浪期货数据各品种代码（商品连续）如下
        //RB0 螺纹钢
        //AG0 白银
        //AU0 黄金
        //CU0 沪铜
        //AL0 沪铝
        //ZN0 沪锌
        //PB0 沪铅
        //RU0 橡胶
        //FU0 燃油
        //WR0 线材
        //A0 大豆
        //M0 豆粕
        //Y0 豆油
        //J0 焦炭
        //C0 玉米
        //L0 乙烯
        //P0 棕油
        //V0 PVC
        //RS0 菜籽
        //RM0 菜粕
        //FG0 玻璃
        //CF0 棉花
        //WS0 强麦
        //ER0 籼稻
        //ME0 甲醇
        //RO0 菜油
        //TA0 甲酸

        //商品期货
        //http://stock2.finance.sina.com.cn/futures/api/json.php/IndexService.getInnerFuturesMiniKLineXm?symbol=CODE
        //例子：
        //http://stock2.finance.sina.com.cn/futures/api/json.php/IndexService.getInnerFuturesMiniKLine5m?symbol=M0
        //5分钟http://stock2.finance.sina.com.cn/futures/api/json.php/IndexService.getInnerFuturesMiniKLine5m?symbol=M0
        //15分钟
        //http://stock2.finance.sina.com.cn/futures/api/json.php/IndexService.getInnerFuturesMiniKLine15m?symbol=M0
        //30分钟
        //http://stock2.finance.sina.com.cn/futures/api/json.php/IndexService.getInnerFuturesMiniKLine30m?symbol=M0
        //60分钟
        //http://stock2.finance.sina.com.cn/futures/api/json.php/IndexService.getInnerFuturesMiniKLine60m?symbol=M0
        //日K线
        //http://stock2.finance.sina.com.cn/futures/api/json.php/IndexService.getInnerFuturesDailyKLine?symbol=M0
        //http://stock2.finance.sina.com.cn/futures/api/json.php/IndexService.getInnerFuturesDailyKLine?symbol=M1401
 
        //股指期货 5分钟
        //http://stock2.finance.sina.com.cn/futures/api/json.php/CffexFuturesService.getCffexFuturesMiniKLine5m?symbol=IF1306
        //15
        //http://stock2.finance.sina.com.cn/futures/api/json.php/CffexFuturesService.getCffexFuturesMiniKLine15m?symbol=IF1306
        //30分钟
        //http://stock2.finance.sina.com.cn/futures/api/json.php/CffexFuturesService.getCffexFuturesMiniKLine30m?symbol=IF1306
        //60分钟
        //http://stock2.finance.sina.com.cn/futures/api/json.php/CffexFuturesService.getCffexFuturesMiniKLine60m?symbol=IF1306
        //日线
        //http://stock2.finance.sina.com.cn/futures/api/json.php/CffexFuturesService.getCffexFuturesDailyKLine?symbol=IF1306

        static void Main(string[] args)
        {
           //string str =  GetUrltoHtml("http://stock2.finance.sina.com.cn/futures/api/json.php/CffexFuturesService.getCffexFuturesDailyKLine?symbol=IF1306","GB2312");
           //将json字符串转换为对象


        }

        /// <summary>
        /// 获取期货日线数据
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetUrltoHtml(string Url, string type)
        {
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(type)))
                {
                    return reader.ReadToEnd();
                }

            }
            catch (System.Exception ex)
            {
                //
            }
            return "";
        }
    }
}
