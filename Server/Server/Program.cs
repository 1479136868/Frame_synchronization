using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            NetManager.GetInstance().Start();
            Console.WriteLine("网络管理启动");

            SyncPostitionManager.GetInstance().Init();
            Console.WriteLine("帧同步的逻辑启动...");


            while (true)
            {

            }
        }
    }
}
