### Thread 总结

#### 手工创建次线程

1.使用`ThreadStart`委托：`ThreadStart`委托指向一个没有参数，无返回值的方法。这个委托在一个方法被设计用来仅仅

在后台运行，而没有更多的交互时非常有用。

2.使用`ParameterizedThreadStart`委托：`ParameterizedThreadStart`指向一个`System.Object`类型的参数，无返回

值的方法，可以通过一个自定义的类或结构来传递任意数量的参数。

#### 子线程中访问主线程

1.`WinForm`应用程序：`this.invoke()`方法,或者使用控件的`invoke`方法。

```cs
var context="some context";

Thread thread1 = new Thread(() =>
{
  if (this.someControl.InvokeRequired)
  {
    this.someControl.Invoke(
      new Action<string>(s => { this.someControl.Text = s; }), context);
  }
});
```

2.`WPF`应用程序:`this.Dispatcher.Invoke()`方法。

#### 前台线程和后台线程

1.前台线程：前台线程能阻止应用程序的终结，一直到所有的前台线程终止后，CLR 才能关闭应用程序（即卸载承载的应用程序域）。

2.后台线程：当所有的前台线程终止，应用程序域卸载时，所有的后台线程也会被自动终止。

#### 并发问题

在构建多线程应用程序时，需要保证任何共享数据都处于被保护状态，以防止多个线程修改它的值。

1.`lock`关键字

- lock (this) is a problem if the instance can be accessed publicly.

- lock (typeof (MyType)) is a problem if MyType is publicly accessible.

- lock(“myLock”) is a problem since any other code in the process using the same string, will share the same lock.

best practice:

```cs
private readonly object threadLock = new object();
```

2.`Monitor`

`lock`的本质是`Monitor`,但是`Monitor`比`lock`的优势在于更好的控制能力,`lock`的代码实际被编译如下:

```cs
private readonly object threadLock = new object();

public void SomeMethod()
{
  Monitor.Enter(threadLock);
  try
  {
    // your code...
  }
  finally
  {
    Monitor.Exit(threadLock);
  }
}
```

#### CLR 线程池

```cs
public static class ThreadPool
{
  public static bool QueueUserWorkItem(waitCallback callBack);

  public static bool QueueUserWorkItem(waitCallback callBack,object state);
}
```

相比显示创建`Thread`对象,使用被 CLR 所维护的线程池的好处如下

- 1.线程池减少了线程创建、开始和停止的次数，而这提高了效率。

- 2.使用线程池，能够使我们将注意力放到业务逻辑上而不是多线程架构上。
