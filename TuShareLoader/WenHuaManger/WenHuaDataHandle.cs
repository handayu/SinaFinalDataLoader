using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuShareLoader
{
    public static class WenHuaDataHandle
    {
        /// <summary>
        /// contract信息在文件data\cont.dat中
        /// </summary>
        /// <param name="filePathName"></param>
        /// <returns></returns>
        public static List<string> GetConDatData(string filePathName)
        {
            try
            {
                //byte[] bs = FileToByte(@"D:\wh6上海中期\Data\贵金属\cont.dat");
                byte[] bs = FileToByte(filePathName);
                byte[] tt = bs.Skip(8).Take(bs.Length).ToArray();

                List<string> listName = new List<string>();

                for (int i = 0; i < tt.Length; i = i + 141)
                {
                    //byte[] conCode = tt.Skip(i + 4).Take(4).ToArray();
                    //float conCodeFloat = BytesToFloat(conCode);

                    byte[] conIns = tt.Skip(i + 53).Take(28).ToArray();
                    string conInsStr = BytesToString(conIns);

                    listName.Add(conInsStr);
                }
                return listName;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 日线数据信息在文件data\day\0006081.dat中
        /// </summary>
        /// <param name="filePathName"></param>
        /// <returns></returns>
        public static List<MarketData> GetHQDatData(string filePathName)
        {
            //byte[] bs = FileToByte(@"D:\wh6上海中期\Data\贵金属\day\00060881.dat");
            byte[] bs = FileToByte(filePathName);
            List<MarketData> mList = new List<MarketData>();
            //下面测试了是每四个字节一转换，第一个字段按照Int32转换为时间，其余字段，按照float进行转换；
            for (int i = 0; i < bs.Length; i = i + 37)
            {
                byte[] longtimeBS = { bs[i], bs[i + 1], bs[i + 2], bs[i + 3] };
                byte[] floatOpen = { bs[i + 4], bs[i + 5], bs[i + 6], bs[i + 7] };
                byte[] floatHigh = { bs[i + 8], bs[i + 9], bs[i + 10], bs[i + 11] };
                byte[] floatLow = { bs[i + 12], bs[i + 13], bs[i + 14], bs[i + 15] };
                byte[] floatClose = { bs[i + 16], bs[i + 17], bs[i + 18], bs[i + 19] };
                byte[] floatVolume = { bs[i + 20], bs[i + 21], bs[i + 22], bs[i + 23] };
                byte[] floatIntest = { bs[i + 24], bs[i + 25], bs[i + 26], bs[i + 27] };
                byte[] floatSeltle = { bs[i + 28], bs[i + 29], bs[i + 30], bs[i + 31] };
                byte[] floatUnknow = { bs[i + 32], bs[i + 33], bs[i + 34], bs[i + 35] };

                Int32 timeLong = BytesToInt(longtimeBS);
                float openFloat = BytesToFloat(floatOpen);
                float HighFloat = BytesToFloat(floatHigh);
                float LowFloat = BytesToFloat(floatLow);
                float CloseFloat = BytesToFloat(floatClose);
                float Volumnloat = BytesToFloat(floatVolume);
                float IntestFloat = BytesToFloat(floatIntest);
                float SettleFloat = BytesToFloat(floatSeltle);
                float UnknowFloat = BytesToFloat(floatUnknow);

                MarketData mD = new MarketData();
                mList.Add(mD);
            }
            return mList;
        }

        /// <summary>
        /// 将文件转换成byte[]数组
        /// </summary>
        /// <param name="fileUrl">文件路径文件名称</param>
        /// <returns>byte[]数组</returns>
        public static byte[] FileToByte(string fileUrl)
        {
            try
            {
                using (FileStream fs = new FileStream(fileUrl, FileMode.Open, FileAccess.Read))
                {
                    byte[] byteArray = new byte[fs.Length];
                    fs.Read(byteArray, 0, byteArray.Length);
                    return byteArray;
                }
            }
            catch
            {
                return null;
            }
        }

        public static Int32 BytesToInt(byte[] bs)
        {
            return BitConverter.ToInt32(bs, 0);
        }

        public static float BytesToFloat(byte[] bs)
        {
            return BitConverter.ToSingle(bs, 0);
        }

        public static string BytesToString(byte[] bs)
        {
            string str = Encoding.GetEncoding("GBK").GetString(bs);
            return str;
        }

        public static string BytesToString2312(byte[] bs)
        {
            string str = Encoding.GetEncoding("GB2312").GetString(bs);
            return str;
        }

    }
}
