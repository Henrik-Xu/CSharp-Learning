using System;
using System.Diagnostics;
using System.Reflection;

namespace myApp
{
    class Program
    {
        static void Main (string[] args)
        {
            CommonSQLHelper sqlHelper = new CommonSQLHelper ();
            // sqlHelper.ConnString = ".."; // //使用这个有警告
            // sqlHelper.Update (""); // 添加true以后就不让使用了

            // 类特性的查找
            Teacher teacher = new Teacher ();
            object[] array = teacher.GetType ().GetCustomAttributes (typeof (MyCustomAttribute), true);
            foreach (var item in array)
            {
                MyCustomAttribute att = item as MyCustomAttribute;
                Console.WriteLine ("Id={0}  IsNotNull={1}  Comment={2}", att.Id, att.IsNotNull, att.Comment);
            }
            Console.WriteLine ("\r\n------------------------------------");

            // 获取属性的特性
            PropertyInfo property = teacher.GetType ().GetProperty ("TeacherName");
            object[] propertyAttArray = property.GetCustomAttributes (typeof (MyCustomAttribute), true);
            foreach (var item in propertyAttArray)
            {
                MyCustomAttribute attr = item as MyCustomAttribute;
                Console.WriteLine (attr.Comment);
            }

            Console.WriteLine ("\r\n------------------------------------");

            //获取数据表的别名
            Console.WriteLine ("数据表的别名：" + GetDBTableName (teacher));

            Console.WriteLine ("\r\n------------------------------------");
            //枚举特性
            Order order = new Order { Status = OrderStatus.AlreadyPaid };
            Console.WriteLine (order.Status.GetDescription ());

            TestDebug ("Only Run in Debug Environment");

            Console.Read ();
        }

        static string GetDBTableName<T> (T table)
        {
            var attribute = table.GetType ().GetCustomAttribute (typeof (TableByNameAttribute), true);
            if (attribute == null)
            {
                return table.GetType ().Name;
            }
            else
            {
                return ((TableByNameAttribute) attribute).TableName;
            }
        }

        [Conditional ("DEBUG")]
        static void TestDebug (string info)
        {
            Console.WriteLine (info);
        }
    }
}
