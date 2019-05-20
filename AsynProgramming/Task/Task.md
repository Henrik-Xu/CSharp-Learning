### Task

#### Task 的基本使用 1

1. new 一个新的 Task 来启动（包含很多的重载）

```cs
 Task task = new Task(() =>
  {
    //在这个地方写我们需要的逻辑...
    Console.WriteLine("子线程Id={0}", Thread.CurrentThread.ManagedThreadId);
  });
  task.Start();
```

2.使用 Task 的 Run() 方法

```cs
Task task2 = Task.Run(() =>
  {
    //在这个地方写我们需要的逻辑...
    Console.WriteLine("子线程Id={0}", Thread.CurrentThread.ManagedThreadId);
  });
```

3.使用 TaskFactory 启动（类似于 ThreadPool）

```cs
Task task3 = Task.Factory.StartNew(() =>
  {
    //在这个地方写我们需要的逻辑...
    Console.WriteLine("子线程Id={0}", Thread.CurrentThread.ManagedThreadId);
  });
```

4.以上是三种异步方式。对于第一种情况，我们也可以使用同步执行（阻塞）

```cs
Task task4 = new Task(() =>
  {
    Console.WriteLine("子线程开始执行......");
    Thread.Sleep(1000);
    Console.WriteLine("子线程Id={0}", Thread.CurrentThread.ManagedThreadId);
  });
  task4.RunSynchronously();//这个是同步方法
```

5.Task 还可以有返回值 Task<TResult> 它的父类也是 Task

```cs
var task5 = new Task<string>(() =>
  {
    //在这个地方可以编写自己的业务逻辑...
    return "We are Studying Task！";
  });
  task5.Start();

  var result = task5.Result;
```
