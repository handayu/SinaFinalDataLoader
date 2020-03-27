using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuShareLoader
{
    public static class Common
    {
        public static Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            //  对于C#的随机数
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);

            //  为了在白色背景上显示，尽量生成深色
            int int_Red = RandomNum_First.Next(256);
            int int_Green = RandomNum_Sencond.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;

            Color color = Color.FromArgb(int_Red, int_Green, int_Blue);
            return color;
        }


    }
}
