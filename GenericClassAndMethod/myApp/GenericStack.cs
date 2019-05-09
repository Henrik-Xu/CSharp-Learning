using System;

namespace myApp
{
  /// <summary>
  /// 泛型堆栈：入栈和出栈操作类（任意类型）
  /// </summary>
  /// <typeparam name="T">可以是任意类型</typeparam>
  public class GenericStack<T>
  {
    private T[] stackArray; //泛型数组
    private int currentPosition; //当前位置
    private int count; //栈的容量

    public GenericStack(int count)
    {
      this.count = count;
      this.stackArray = new T[count]; //初始化数组大小
      this.currentPosition = 0; //当前默认位置，索引从0开始
    }

    /// <summary>
    /// 出栈
    /// </summary>
    /// <param name="item"></param>
    public void Push(T item)
    {
      if (currentPosition >= count)
      {
        throw new IndexOutOfRangeException();
      }
      else
      {
        this.stackArray[currentPosition] = item;
        currentPosition++;
      }
    }

    public T Pop()
    {
      if (currentPosition < 0)
      {
        throw new IndexOutOfRangeException();
      }
      T data = this.stackArray[currentPosition - 1];
      currentPosition--;
      return data;
    }
  }
}
