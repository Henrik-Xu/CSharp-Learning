using System;
namespace myApp.School
{
    /// <summary>
    /// 校长类：一个学校只有一个校长【单利模式】
    /// /// </summary>
    public class President
    {
        private static President instance = null;

        private President ()
        {
            Console.WriteLine ("单利模式中，President私有构造方法被调用！");
        }

        public static President GetPresident ()
        {
            if (instance == null)
            {
                instance = new President ();
            }
            return instance;
        }

        public void SayHello ()
        {
            Console.WriteLine ("各位老师同学们，大家好！我是新任校长，工作中请大家多支持！");
        }
    }
}
