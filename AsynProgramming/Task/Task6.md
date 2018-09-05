## Task 中的返回值

1.常规的获取返回值

```
static void Main(string[] args)
{
  Task<int> task1 = Task.Factory.StartNew(() =>
  {
      //在这个地方可以编写自己需要的逻辑部分...
      return 100;
  });

  //  task1.Wait();
  Console.WriteLine(task1.Result);

  Console.Read();
}
```

2.`ContinueWith<TResult>`也可以具有返回值

```
static void Main(string[] args)
{
  Task<int> task1 = Task.Factory.StartNew(() =>
  {
    //在这个地方可以编写自己需要的逻辑部分...

    return 100;
  });
  var task2 = task1.ContinueWith(task =>
  {
    var num = task.Result;
    var square = num * num;
    return square;
  });

  Console.WriteLine(task2.Result);

  Console.Read();
}
```

3.Task.WhenAll<TResult>/WhenAny

```
static void Main(string[] args)
{
  Task<int> task1 = Task.Factory.StartNew(() =>
  {
    //在这个地方可以编写自己需要的逻辑部分...

    return 100;
  });
  Task<int> task2 = Task.Factory.StartNew(() =>
  {
    //在这个地方可以编写自己需要的逻辑部分...

    return 200;
  });

  var task3 = Task.WhenAll(new Task<int>[2] { task1, task2 });
  var result = task3.Result;

  foreach (var item in result)
  {
    Console.WriteLine(item);
  }

  Console.Read();
}
```
