## TPL(Task Parallel Library)

### 一:并行计算：Parallel 各种使用方法

1.并行计算的基础使用

```
static void Main(string[] args)
{
  Console.WriteLine("-------------串行执行------------");
  for (int i = 0; i < 10; i++)
  {
    Console.WriteLine(i);
  }
  Console.WriteLine("-------------并行执行------------");
  Parallel.For(0, 10, (num) =>
  {
    Console.WriteLine(num);
  });

  Console.Read();
}
```

2.重载方法 1：中断的出现的 bug

```
static void Main(string[] args)
{
  Console.WriteLine("-------------并行执行-----------");
  //public static ParallelLoopResult For(int fromInclusive, int toExclusive, Action<int, ParallelLoopState> body);
  Parallel.For(0, 10, (num, loop) =>
  {
    if (num == 5)
    {
      loop.Break();
      //建议不要在Parallel.For中使用Break，因为很多时候不会在你希望的地方停止，因为并发的其他线程也在执行...这种随意中断会有bug
    }
    Console.WriteLine(num);
  });

  Console.Read();
}
```

3.重载方法 2：可以指定参与线程的个数：

```
//public static ParallelLoopResult For(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Action<int> body);
static void Main(string[] args)
{
  Console.WriteLine("-------------并行执行-----------");
  Parallel.For(0, 10, new ParallelOptions()
  {
    MaxDegreeOfParallelism = Environment.ProcessorCount - 2
    //Environment.ProcessorCount 表示当前计算机上处理器的个数，设置这个值后，可以让固定个数的线程不参与
  }, (num) =>
  {
    //在这里编写业务逻辑...

    Console.WriteLine(num);
  });

  Console.Read();
}
```

4.Parallel.For：使用数组可以做累加运算

```
static void Main(string[] args)
{
  int[] nums = { 10, 90, 20, 45, 89, 35 };
  int sum = 0;
  Parallel.For(0, nums.Length, (item) =>
  {
    //可以在这里编写相关的业务逻辑...

    sum += nums[item];
    Console.WriteLine(sum);
  });
  Console.Read();
}
```

5.Parallel.ForEach：可以遍历字典

```
static void Main(string[] args)
{
  Dictionary<int, string> dic = new Dictionary<int, string>()
  {
    {1,".NET基础"},
    {2,".NET高级" },
    {3,"Java高级"}
  };

  Parallel.ForEach(dic, (item) =>
  {
    Console.WriteLine(item.Key);
  });
  Parallel.ForEach(dic, (item) =>
  {
    Console.WriteLine(item.Value);
  });
  Console.Read();
}
```

6.Parallel.Invoke 并行计算方法

```
static void Main(string[] args)
{
  Parallel.Invoke(() =>
  {
    //在这里写我们需要的业务逻辑...
    Console.WriteLine("Paralle Calculator 1 id=" + Thread.CurrentThread.ManagedThreadId);
  },
  () =>
  {
    //在这里写我们需要的业务逻辑...
    Console.WriteLine("Paralle Calculator 2 id=" + Thread.CurrentThread.ManagedThreadId);
  });
  Console.Read();
}
```

### 二、PLinq

为什么使用 Plinq？也就是在 LINQ 查询中，为了提高效率，可以使用并行版本

1.普通 LINQ 查询

```
static void Main(string[] args)
{
  var nums = Enumerable.Range(0, 10);//生成一组整数
  var query = from n in nums
              select new
              {
                tid = Thread.CurrentThread.ManagedThreadId,
                num = n
              };
  foreach (var item in query)
  {
    Console.WriteLine(item);
  }
  Console.Read();
}
```

2.PLinq 查询操作（并行）

```
static void Main(string[] args)
{
  var nums = Enumerable.Range(0, 10000);//生成一组整数
  var query = from n in nums.AsParallel()  //AsParallel（）这个是扩展方法，可以将串行的代码转为并行
              select new
              {
                tid = Thread.CurrentThread.ManagedThreadId,
                num = n
              };
  foreach (var item in query)
  {
    Console.WriteLine(item);
  }
  Console.Read();
}
```

3.扩展方法:AsOrdered() 就是按照原始顺序排序。AsOrdered() 不等于我们串行编程中的 OrderBy,AsSequential()就是把 PLINQ 转成 LINQ

```
static void Main(string[] args)
{
  var nums = new int[] { 10, 8, 7, 3, 4 };

  var query = from n in nums.AsParallel().AsOrdered().AsSequential()//.AsUnordered()//.AsOrdered()
              select new
              {
                tid = Thread.CurrentThread.ManagedThreadId,
                num = n
              };
  foreach (var item in query)
  {
    Console.WriteLine(item);
  }
  Console.Read();
}
```
