### Task 的基本使用 5 ：Task 中的取消功能：使用的是 CacellationTokenSoure 解决多任务中协作取消和超时取消方法

1.简单的线程的取消（存在 bug）

```
static void Main(string[] args)
{
  var isStop = false;//标志变量
  var thread = new Thread(() =>
  {
    while (!isStop)
    {
        Thread.Sleep(200);
        Console.WriteLine("threadId=" + Thread.CurrentThread.ManagedThreadId);
    }
  });
  thread.Start();

  //主线程的任务
  Thread.Sleep(1000);
  isStop = true;//不能让多个线程操作一个共享变量，否则在release版本中有潜在的bug

  Console.Read();
}
```

2.`Task`任务的取消与判断：`CancellationTokenSource` 实现和前面`isStop`判断相同的功能，去处理"取消任务” 但是比前面优化很多....

```
static void Main(string[] args)
{
  //前面用过的类：
  CancellationTokenSource cts = new CancellationTokenSource();

  Task task = Task.Factory.StartNew(() =>
  {
    while (!cts.IsCancellationRequested)//判断任务是否被取消
    {
      Thread.Sleep(200);
      Console.WriteLine("threadId=" + Thread.CurrentThread.ManagedThreadId);
    }
  }, cts.Token);

  Thread.Sleep(1000);
  cts.Cancel();

  Console.Read();
}
```

3.`Task`任务取消的时候，我们希望能够有一些其他的清理工作要执行，也就是这个取消的动作会触发一个任务，比如更新订单队列，或数据库等

```
static void Main(string[] args)
{
  CancellationTokenSource cts = new CancellationTokenSource();

  //注册一个委托：这个委托将在任务取消的时候调用
  cts.Token.Register(() =>
  {
    //在这个地方可以编写自己要处理的逻辑....

    Console.WriteLine("当前task被取消，我们现在可以做相关的清理工作...");
  });

  Task task = Task.Factory.StartNew(() =>
  {
    while (!cts.IsCancellationRequested)
    {
      Thread.Sleep(200);
      Console.WriteLine("threadId=" + Thread.CurrentThread.ManagedThreadId);
    }
  }, cts.Token);

  Thread.Sleep(1000);
  cts.Cancel();

  Console.Read();
}
```

4.`Task`任务延时取消：比如我们请求远程的接口，如果在指定时间没有返回数据，我们可以做一个时间限制，超时可以取消任务

```
static void Main(string[] args)
{
  //CancellationTokenSource cts = new CancellationTokenSource();
  CancellationTokenSource cts = new CancellationTokenSource(2000);

  //注册一个委托：这个委托将在任务取消的时候调用
  cts.Token.Register(() =>
  {
    //在这个地方可以编写自己要处理的逻辑....

    Console.WriteLine("当前task被取消，我们现在可以做相关的清理工作...");
  });

  Task task = Task.Factory.StartNew(() =>
  {
    while (!cts.IsCancellationRequested)
    {
      Thread.Sleep(200);
      Console.WriteLine("threadId=" + Thread.CurrentThread.ManagedThreadId);
    }
  }, cts.Token);

  ////取消方法1：public void CancelAfter(TimeSpan delay);
  //cts.CancelAfter(new TimeSpan(0, 0, 0, 2));

  //取消方法2：public void CancelAfter(int millisecondsDelay);
  //在构造方法中设置取消的时间

  Console.Read();
}
```
