## Task 的基本使用 3

`Task`常见枚举`TaskCreationOptions`（父子任务运行和拒绝附加，长时间运行的任务...）

```
static void Main(string[] args)
{
  // 摘要:
  //     指定将任务附加到任务层次结构中的某个父级。默认情况下，子任务（即由外部任务创建的内部任务）将独立于其父任务执行。可以使用
  //     System.Threading.Tasks.TaskContinuationOptions.AttachedToParent
  //     选项以便将父任务和子任务同步。请注意，如果使用 System.Threading.Tasks.TaskCreationOptions.DenyChildAttach
  //     选项配置父任务，则子任务中的 System.Threading.Tasks.TaskCreationOptions.AttachedToParent 选项不起作用，并且子任务将作为分离的子任务执行。有关详细信息，请参阅附加和分离的子任务。

  Task parentTask = new Task(() =>
  {
    //任务1
    Task task1 = new Task(() =>
    {
      Thread.Sleep(1000);
      Console.WriteLine("Child（1）Time={0}", DateTime.Now.ToLongTimeString());
    }, TaskCreationOptions.AttachedToParent);
    task1.Start();

    //任务2
    Task task2 = new Task(() =>
    {
      Thread.Sleep(3000);
      Console.WriteLine("Child（2）Time={0}", DateTime.Now.ToLongTimeString());
    }, TaskCreationOptions.AttachedToParent);
    task2.Start();
  });

  parentTask.Start();
  parentTask.Wait();//等待附加的子任务全部完成，相当于：Task.WaitAll(task1,task2);
                    //如果子线程不使用TaskCreationOptions.AttachedToParent，则主线程直接运行不等待

  Console.WriteLine("This is Main Thread!");

  Console.Read();
}
```

```
static void Main(string[] args)
{
  Task parentTask = new Task(() =>
  {
    //任务1
    Task task1 = new Task(() =>
    {
      Thread.Sleep(1000);
      Console.WriteLine("Child（1）Time={0}", DateTime.Now.ToLongTimeString());
    }, TaskCreationOptions.AttachedToParent);
    task1.Start();
    //任务2
    Task task2 = new Task(() =>
    {
      Thread.Sleep(3000);
      Console.WriteLine("Child（2）Time={0}", DateTime.Now.ToLongTimeString());
    }, TaskCreationOptions.AttachedToParent);
    task2.Start();
  }, TaskCreationOptions.DenyChildAttach);//禁止子任务附加（如果前面附加了，在这里等于没有附加）

  parentTask.Start();
  parentTask.Wait();//等待附加的子任务全部完成，相当于：Task.WaitAll(task1,task2);
                    //如果子线程不使用TaskCreationOptions.AttachedToParent，则主线程直接运行不等待

  Console.WriteLine("This is Main Thread!");

  Console.Read();
}
```

```
static void Main(string[] args)
{
  Task task1 = new Task(() =>
  {
    Thread.Sleep(1000);
    Console.WriteLine("Child（1）Time={0}", DateTime.Now.ToLongTimeString());
  }, TaskCreationOptions.LongRunning);

  //LongRunning：如果你明确的知道这个任务是运行时间长，建议你选择此项。同时我们也建议使用“Thread”，而不是ThreadPool。
  //如果我们采用了ThreadPool,我们长时间的使用线程，而不归还，强制开启新的线程，会影响性能。

  task1.Start();
  task1.Wait();

  //以上代码看不到效果...可以通过windbg去看（略）

  Console.WriteLine("This is Main Thread!");

  Console.Read();
}
```
