## 异步委托回调函数

新建一个 WinForm 桌面应用程序，桌面布局如下所示：

![](https://github.com/Damon-Salvatore/CSharp-Learning/tree/master/AsynProgramming/imgs)

#### 【1】声明委托

```
public delegate int MyCalculator(int num, int ms);
```

#### 【2】根据委托定义方法

```
private int ExecuteTask(int num, int ms)
{
  System.Threading.Thread.Sleep(ms);
  return num * num;
}
```

#### 【3】创建委托变量（后面异步方法回调方法都要用，所以，定义成成员变量）

```
private MyCalculator myCal = null;
```

#### 【4】定义回调方法

```
private void MyCallBack(IAsyncResult result)
{
  int res = myCal.EndInvoke(result);

  //异步显示结果：result.AsyncState字段用来封装回调函数时自定义的参数，object类型
  string info = $"第{result.AsyncState.ToString()} 计算结果是{res}";
  Console.WriteLine(info);
}
```

#### 【5】在按钮`btn_Exec`中同时执行多个任务

```
private void btnExec_Click(object sender, EventArgs e)
{
  myCal += ExecuteTask;
  for (int i = 1; i < 11; i++)//预订10个任务
  {
    //开始异步执行，同时封装回调方法
    myCal.BeginInvoke(10 * i, 1000 * i, MyCallBack, i);
    //最后一个参数i 给回调函数的字段AsyncState赋值，如果数据很多可以定义成类或结构都可以的
  }
}
```

#### 得出结论

异步编程需要知道的一些内容：

- 1. 异步编程是建立在委托基础上的。
- 2. 异步调用的每个方法都是在独立的线程中执行。所以，异步本质就是多线程。
- 3. 比较适合在后台运行一些较为耗时的《简单任务》，并且任务要求相互独立。
- 4. 后台任务必须按照一定顺序执行的时候，异步就不太合适。应该采用多线程技术。
