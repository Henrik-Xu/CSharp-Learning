using System;

namespace myApp
{
  class Program
  {
    static void Main(string[] args)
    {
      //【1】创建泛型类对象
      GenericStack<int> stack1 = new GenericStack<int>(5);
      try
      {
        //【2】入栈
        stack1.Push(1);
        stack1.Push(2);
        stack1.Push(3);
        stack1.Push(4);
        stack1.Push(5);
        stack1.Push(5);
        //【3】出栈
        Console.WriteLine(stack1.Pop());
        Console.WriteLine(stack1.Pop());
        Console.WriteLine(stack1.Pop());
        Console.WriteLine(stack1.Pop());
        Console.WriteLine(stack1.Pop());
      }
      catch (IndexOutOfRangeException ex)
      {
        Console.WriteLine(ex.Message);
      }


      GenericStack<string> stack2 = new GenericStack<string>(5);

      try
      {
        stack2.Push("课程1");
        stack2.Push("课程2");
        stack2.Push("课程3");
        stack2.Push("课程4");
        stack2.Push("课程5");

        Console.WriteLine(stack2.Pop());
        Console.WriteLine(stack2.Pop());
        Console.WriteLine(stack2.Pop());
        Console.WriteLine(stack2.Pop());
        Console.WriteLine(stack2.Pop());
        Console.WriteLine(stack2.Pop());
      }
      catch (IndexOutOfRangeException ex)
      {
        Console.WriteLine(ex.Message);
      }


      Console.Read();
    }
  }
}
