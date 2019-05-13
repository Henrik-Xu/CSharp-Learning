using System;
namespace myApp.School
{
    public class Instructor<T1>
    {
        public void SayHello (T1 t1)
        {
            Console.WriteLine ($"GenericInstructor1<T1 >： public void SayHello(T1 t1) 方法被调用！");
            Console.WriteLine ($"T1的类型={t1.GetType().Name}");
        }
    }

    public class Instructor2<T1, T2>
    {
        public void SayHello (T1 t1, T2 t2)
        {
            Console.WriteLine ($"GenericInstructor2<T1, T2>： public void SayHello(T1 t1,T2 t2) 方法被调用！");
            Console.WriteLine ($"T1的类型={t1.GetType().Name}  T2的类型={t2.GetType().Name} ");
        }
    }

    public class Instructor3<T1, T2, T3>
    {
        public void SayHello (T1 t1, T2 t2, T3 t3)
        {
            Console.WriteLine ($"GenericInstructor2<T1, T2, T3>： public void SayHello(T1 t1,T2 t2, T3 t3) 方法被调用！");
            Console.WriteLine ($"T1的类型={t1.GetType().Name}  T2的类型={t2.GetType().Name}  T3的类型={t3.GetType().Name}");
        }
    }
}
