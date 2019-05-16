namespace myApp
{
    public class Person
    {
        public string Address { get; set; }

        public string IdentityCardNo { get; set; }
    }

    public interface IPerson<in T>
    {
        string SayHello (T content);
    }

    public class Teacher<T> : IPerson<T>
    {
        public string SayHello (T content)
        {
            return $"I am Learning {content}";
        }
    }

}
