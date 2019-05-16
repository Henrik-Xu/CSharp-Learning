using System;
using System.Collections.Generic;

namespace myApp
{
    public static class CustomExtendMethod
    {
        /// <summary>
        /// 这个方法是对int类型增加的扩展方法
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        public static string ShowScore (this int sum)
        {
            return "本次考试总成绩：" + sum;
        }
        public static void ShowList (this string[] nameList)
        {
            foreach (var item in nameList)
            {
                Console.WriteLine (item);
            }
        }

        public static string SayHello (this Student student)
        {
            return "欢迎您：" + student.StudentName;
        }
        public static string GetAvg (this Student student, int csharp, int database)
        {
            int avg = (csharp + database) / 2;
            return string.Format ($"欢迎您：{student.StudentName} !  您两门的平均成绩为：{avg}");
        }
        public static IEnumerable<TSource> MyCustomWhere<TSource> (this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            //。。。
            List<TSource> list = new List<TSource> ();
            //遍历查找
            foreach (var item in source)
            {
                if (predicate (item))
                {
                    list.Add (item);
                }
            }
            return list;
        }
    }
}
