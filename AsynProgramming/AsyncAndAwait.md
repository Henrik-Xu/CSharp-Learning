## async/await

- 1.C#5.0 中引入了 async 和 await。这两个关键字可以让我们更加轻松的完成异步代码的编写。

- 2.方法（包括 Lambada 表达式和匿名方法）可以用 async 关键字标记，允许该方法以非阻塞的形式进行工作。

- 3.用 async 关键字的方法（包括 Lambada 表达式和匿名方法）在遇到 await 关键字之前将以阻塞的形式运行。

- 4.单个 async 可以拥有多个 await 上下文

- 5.当遇到 await 表达式时，调用线程将挂起，直到 await 的任务完成。同时，控制将返回返回给方法的调用者。

- 6.await 关键字将从视图中隐藏返回的 Task 对象，直接返回实际的返回值。没有返回值的方法可以简单的返回 void。

- 7.根据命名约定，要被异步调用的方法应该以"Async"作为后缀。

```
static void Main(string[] args)
{
  string txt = DoWorkAsync().Result;
  Console.WriteLine("main threadId=" + Thread.CurrentThread.ManagedThreadId);

  Console.Read();
}
public static async Task<string> DoWorkAsync()
{
  return await Task.Run(()=>
  {
    Console.WriteLine("child threadId=" + Thread.CurrentThread.ManagedThreadId);
    Thread.Sleep(1000);
    return "Done with work!";
  });
}
```
