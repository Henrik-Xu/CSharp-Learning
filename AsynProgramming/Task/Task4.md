## Task 的基本使用【4】：Task 常见枚举 （延续、完成、取消、异常）TaskContiuationOptions

1.`ContinueWith`的使用

```
static void Main(string[] args)
{
  // 任务1
  Task task1 = new Task(() =>
  {
    Console.WriteLine("Child（1）Time={0}  Id={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  });

  // 任务2
  Task task2 = task1.ContinueWith((task) =>
  {
    Console.WriteLine("Child（2）Time={0}  Id={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  });
  // 任务3
  Task task3 = task2.ContinueWith((task) =>
  {
    Console.WriteLine("Child（3）Time={0}  Id={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  });
  task1.Start();
  Console.Read();
}
```

2.任务取消信号源对象的使用：`CancellationTokenSource`

```
static void Main(string[] args)
{
  //【1】任务取消信号源对象
  CancellationTokenSource cts = new CancellationTokenSource();

  cts.Cancel();//《3》传达任务取消的请求

  // 任务1
  Task task1 = new Task(() =>
  {
      Console.WriteLine("Child（1）Time={0}  Id={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  });

  // 任务2
  Task task2 = task1.ContinueWith((task) =>
  {
      Console.WriteLine("Child（2）Time={0}  Id={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  }, cts.Token);//《2》cts.Token：获取任务取消信号源

  // 任务3
  Task task3 = task2.ContinueWith((task) =>
  {
      Console.WriteLine("Child（3）Time={0}  Id={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  });
  task1.Start();
  Console.Read();

  // 结果分析：task2任务取消，task3先执行了！why?
  // task1-->ContinueWith  task2-->ContinueWith -->task3
  // 当ContinueWith时候，会预先判断cts.Token的值，当task2取消后，task2和task3就没有了延续关系。task3和task1也就没有了关系。
  // 所以：ContinueWith这个关系不会正常延续...
}
```

3.任务延续的保证：`TaskContinuationOptions.LazyCancellation`前面`task2`被取消后，但是任务`task1`如果还希望和`task3`有顺序关系，该如何做？....

```
static void Main(string[] args)
{
  // 【1】任务取消信号源对象
  CancellationTokenSource cts = new CancellationTokenSource();
  cts.Cancel();//《3》传达任务取消的请求

  // 任务1
  Task task1 = new Task(() =>
  {
    Console.WriteLine("Child（1）Time={0}  Id={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  });

  // 任务2
  Task task2 = task1.ContinueWith((task) =>
  {
    Console.WriteLine("Child（2）Time={0}  Id={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  }, cts.Token, TaskContinuationOptions.LazyCancellation, TaskScheduler.Current);
  //【2】cts.Token：获取任务取消信号源
  // TaskContinuationOptions.LazyCancellation  :在延续被取消的情况下，也会等待前面的task完成后再做判断

  // 任务3
  Task task3 = task2.ContinueWith((task) =>
  {
    Console.WriteLine("Child（3）Time={0}  Id={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  });
  task1.Start();
  Console.Read();
}
```

4.防止多任务时候线程的切换：`TaskContinuationOptions.ExecuteSynchronously`

```
static void Main(string[] args)
{
  // 任务1
  Task task1 = new Task(() =>
  {
    Console.WriteLine("Child（1）Time={0}  ThreadId={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  });

  // 任务2
  Task task2 = task1.ContinueWith((task) =>
  {
    Console.WriteLine("Child（2）Time={0}  ThreadId={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  }, TaskContinuationOptions.ExecuteSynchronously);
  //TaskContinuationOptions.ExecuteSynchronously 这个枚举就是希望前面那个task执行的Thread也在本任务中延续执行。
  //好处：这种方式，对于多而小的任务，可以在一定程度上防止线程切换，如果没有这个枚举，task1和task2会用不同的线程（当然不是完全绝对）

  // 任务3
  Task task3 = task2.ContinueWith((task) =>
  {
    Console.WriteLine("Child（3）Time={0}  ThreadId={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  }, TaskContinuationOptions.ExecuteSynchronously);//继续使用第一个任务的线程执行
  task1.Start();
  Console.Read();
}
```

5.延续任务必须在前面`task`完成状态下才能执行：`TaskContinuationOptions.OnlyOnRanToCompletion`

```
static void Main(string[] args)
{
  // 任务1
  Task task1 = new Task(() =>
  {
    Console.WriteLine("Child（1）Time={0}  ThreadId={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  });

  // 任务2
  Task task2 = task1.ContinueWith((task) =>
  {
    Console.WriteLine("Child（2）Time={0}  ThreadId={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  }, TaskContinuationOptions.OnlyOnRanToCompletion);

  // 好处：不需要判断前面的线程IsCompleted等这种操作...
  task1.Start();
  Console.Read();
}
```

6.延续任务必须在前面`task`非完成状态下，或出现异常的时候才能执行：`TaskContinuationOptions.OnlyOnFaulted/NotOnRanToCompletion`

```
static void Main(string[] args)
{
  // 任务1
  Task task1 = new Task(() =>
  {
    Console.WriteLine("Child task1（1）Time={0}  ThreadId={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);

    throw new Exception("task1 出现异常了！");
  });

  // 任务2
  //Task task2 = task1.ContinueWith((task) =>
  //{
  //    Console.WriteLine("Child task2（2）Time={0}  ThreadId={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  //}, TaskContinuationOptions.OnlyOnRanToCompletion);

  // 实现方法1：OnlyOnFaulted
  // 任务2
  //Task task2 = task1.ContinueWith((task) =>
  //{
  //    Console.WriteLine("Child task2（2）Time={0}  ThreadId={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  //}, TaskContinuationOptions.OnlyOnFaulted);
  ////TaskContinuationOptions.OnlyOnFaulted  :  当前前面任务未处理异常的时候，执行的任务。


  // 实现方法2：NotOnRanToCompletion
  // 任务2
  Task task2 = task1.ContinueWith((task) =>
  {
    Console.WriteLine("Child task2（2）Time={0}  ThreadId={1}", DateTime.Now.ToLongTimeString(), Thread.CurrentThread.ManagedThreadId);
  }, TaskContinuationOptions.NotOnRanToCompletion);
  //TaskContinuationOptions.NotOnRanToCompletion  : 也就是前面任务没有完成的时候执行

  task1.Start();
  Console.Read();
}
```
