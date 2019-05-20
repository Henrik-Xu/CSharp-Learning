## Task 的基本使用 2

1.使用 `Task` 各种阻塞方法

```cs
  Task task1 = new Task(() =>
  {
    Thread.Sleep(1000);
    Console.WriteLine("子线程Id={0}", Thread.CurrentThread.ManagedThreadId);
  });
  task1.Start();

  Task task2 = new Task(() =>
  {
    Thread.Sleep(3000);
    Console.WriteLine("子线程Id={0}", Thread.CurrentThread.ManagedThreadId);
  });
  task2.Start();

  // 第一种：等待所有的任务都完成
  Task.WaitAll(task1, task2);

  var taskArray = new Task[2] { task1, task2 };
  Task.WaitAll(taskArray);

  // 第二种：等待任一任务完成
  var taskArray2 = new Task[2] { task1, task2 };
  Task.WaitAny(taskArray2);
```

2.`Task`的延续

- 1: `WhenAll`

```cs
  Task.WhenAll(task1, task2).ContinueWith(task3 =>
  {
    Console.WriteLine("Child（3）Time={0}", DateTime.Now.ToLongTimeString());
  });
}
```

2：`WhenAny`

```cs
 Task.WhenAny(task1, task2).ContinueWith(task3 =>
  {
    Console.WriteLine("Child（3）Time={0}", DateTime.Now.ToLongTimeString());
  });
```

3:使用工厂完成：`ContinueWhenAll`

```cs
  // Factory里面的：public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction);
  // 摘要:
  //     创建一个延续任务，该任务在一组指定的任务完成后开始。
  //
  // 参数:
  //   tasks:
  //     继续执行的任务所在的数组。
  //
  //   continuationAction:
  //     在 tasks 数组中的所有任务完成时要执行的操作委托。
  //
  // 返回结果:
  //     新的延续任务。
  //
  Task.Factory.ContinueWhenAll(new Task[] { task1, task2 }, (task) =>
  {
    Console.WriteLine("Child（3）Time={0}", DateTime.Now.ToLongTimeString());
  });

```
