using System;
using System.Reflection;
using myApp.School;

namespace myApp
{
    class Program
    {
        static void Main (string[] args)
        {
            //  程序依据需求的不同，耦合度不断的变化，开发者应该对这一些列变化深入掌握，才能更好的把握.Net原理

            #region 【1】普通对象的创建：性能最好，最容易理解，耦合度最高

            // Console.WriteLine ("\r\n【1】普通对象的创建----------------------------------");
            // Student student1 = new Student ();
            // Console.WriteLine (student1.GetEntityTypeById ());

            #endregion

            #region 【2】基于接口的对象创建：接口更好的体现面向抽象编程，一定程度解耦

            // Console.WriteLine ("\r\n【2】基于接口的对象创建-----------------------------");
            // IQueryService student2 = new Student ();
            // Console.WriteLine (student2.GetEntityTypeById ());

            #endregion

            #region 【3】反射的基本使用

            Console.WriteLine ("\r\n【3】反射的基本使用----------------------------------");
            Assembly ass3 = Assembly.Load ("myApp"); //在当前运行目录下根据程序集名称加载

            string path = System.IO.Directory.GetCurrentDirectory () + "\\myApp.dll";
            Console.WriteLine (path);
            //使用完整的路径加载程序集文件,。net core中需要在 根目录执行 dotnet myApp.dll
            Assembly ass3_2 = Assembly.LoadFile (path);
            //根据程序集文件名称，加载当前运行目录下的程序集,net core中需要在 根目录执行 dotnet myApp.dll
            Assembly ass3_3 = Assembly.LoadFrom ("myApp.dll");

            //观察程序集对象给我们提供的信息
            foreach (var item in ass3.GetTypes ()) //Type表示当前程序集中所能找到的可用类型
            {
                Console.WriteLine (item.Name + " \t" + item);
            }

            Console.WriteLine ("----------------------------------------");

            foreach (var item in ass3_2.GetTypes ()) //Type表示当前程序集中所能找到的可用类型
            {
                Console.WriteLine (item.Name + " \t" + item);
            }

            Console.WriteLine ("----------------------------------------");

            foreach (var item in ass3_3.GetTypes ()) //Type表示当前程序集中所能找到的可用类型
            {
                Console.WriteLine (item.Name + " \t" + item);
            }

            #endregion

            Console.Read ();
        }
    }
}
