## 进程和线程

- 进程：一个正在运行的程序就是一个进程,操作系统根据进程分配各种资源（内存...）。

- 线程：操作系统为了提高效率会将一个进程分成多个线程，并按照线程来分配 CPU 执行的时间。

- 时间分配：比如 A 进程 10 个线程，B 进程 2 个线程，操作系统会按照 12 个线程来分配 CPU 时间，这样 A 进程有机会获得 CPU 的几率就大。

- 线程特点：在具有多个 CPU 的计算机中，可以并行执行。如果是单 CPU，则会出现假象。

- 单线程：只有一个线程的进程称为“单线程”进程。拥有多个线程的的进程称为多线程进程。

- Thread 类：表示托管线程，每个 Thread 对象都代表着一个托管线程，每个托管线程都对应这一个函数。

- Thread 类：与操作系统真实的本地线程不是一一对应关系，它只是一个“逻辑线程”。

- ProcessThread 类：用于表示操作系统中真实的本地线程。

新建一个 WinForm 桌面应用程序，桌面布局如下所示,两个按钮分别取名`btnExecute1`和`btnExecute2`

![](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/AsynProgramming/imgs/1.png)

在`btnExecute1`的点击事件中添加如下代码

```
private void btnExecute1_Click(object sender, EventArgs e)
{
  int a = 0;
  Thread thread = new Thread( ()=>
  {
    for (int i = 1; i < 20; i++)
    {
      Console.WriteLine((a + i) + "  ");
      Thread.Sleep(500);
    }
  });

  thread.IsBackground = true;//设置为后台线程（通常要这样设置）
  thread.Start();
}
```

在`btnExecute2`的点击事件中添加如下代码

```
private void btnExecute2_Click(object sender, EventArgs e)
{
  Thread thread2 = new Thread(delegate ()
  {
    for (int i = 1; i < 60; i++)
    {
        Console.WriteLine("--------------a"+i+"--------------");
        Thread.Sleep(100);
    }
  });
  thread2.IsBackground = true;
  thread2.Start();
}
```
