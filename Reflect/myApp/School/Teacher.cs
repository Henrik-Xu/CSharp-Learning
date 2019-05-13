using System;
using System.Collections.Generic;
namespace myApp.School
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        public string TeacherName { get; set; }

        public string Department { get; set; }

        public static readonly string Company = "https://github.com/Damon-Salvatore";

        public Dictionary<int, string> Courses { get; } = new Dictionary<int, string> ()
        {
            [1] = ".Net 高级编程", [2] = "Asp.NET 网站技术", [3] = "Web 前端技术"
        };

        public Teacher ()
        {
            TeacherId = 1001;
            TeacherName = "Damon Salvatore";
            Department = "计算机系";
        }

        public void SayHello ()
        {
            Console.WriteLine ($"SayHello() 大家好，我是{Department}的课程讲师：{TeacherName}");
        }

        public void Teach (int couseId)
        {
            Console.WriteLine ($"Teach(int couseId)   今天我们讲授：{Courses[couseId]}");
        }
        public void Teach (int couseId, string chapter)
        {
            Console.WriteLine ($"Teach(int couseId, string chapter)   今天我们讲授：{Courses[couseId]}中的{chapter}！");
        }
        public void Teach (int couseId, string chapter, string content)
        {
            Console.WriteLine ($"Teach(int couseId, string chapter, string content)   今天我们讲授：{Courses[couseId]}中{chapter}关于{content}的内容！");
        }

        /// <summary>
        /// 领工资（私有方法）
        /// </summary>
        private void GetSalary ()
        {
            Console.WriteLine (" private  void GetSalary()：今天我自己去领工资！");
        }

        /// <summary>
        /// 做演讲（静态方法）
        /// </summary>
        /// <param name="title"></param>
        public static void Lecture (string title)
        {
            Console.WriteLine ($"public static void Lecture(string title) ：今天我演讲的题目是：{title}");
        }

        /// <summary>
        /// 泛型方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="C"></typeparam>
        /// <param name="time">授课时间</param>
        /// <param name="count">上课次数</param>
        public void TeachAdvancedCourse<T, C> (T time, C count)
        {
            Console.WriteLine ($"public void TeachAdvancedCourse<T, C>(T time, C count)：我们每周{count}次课 晚上{time}");
        }

    }
}
