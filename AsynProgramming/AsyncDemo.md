## 异步委托

新建一个 WinForm 桌面应用程序，桌面布局如下所示,两个按钮分别取名`btnTest1`和`btnTest2`

![](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/AsynProgramming/imgs/1.png)

添加两个方法，用来和委托事件关联：

```
private int ExecuteTask1(int num)
{
  System.Threading.Thread.Sleep(5000);
  return num * num;
}
private int ExecuteTask2(int num)
{
  return num * num;
}
```

在第一个`btnTest1`的点击事件中实现同步编程，可以发现整个`lblInfo2`需要等待`lblInfo1`执行完成之后再显示，而且整个应用程序处于阻塞状态，十分影响体验。

```
private void btnTest1_Click(object sender, EventArgs e)
{
  this.lblInfo1.Text = ExecuteTask1(10).ToString();
  this.lblInfo2.Text = ExecuteTask2(20).ToString();
}
```

在第二个`btnTest2`的点击事件中，我们使用异步委托来实现异步编程

```
//BeginInvoke（<输入和输出变量>,AysncCallBak,object asyncState)方法：异步调用的核心
//第一个参数10，表示委托对应的方法实参。
//第二个参数AysncCallBak，回调函数，表示异步调用结束的时候自动调用的函数
//第三个参数，asyncState，用来传递异步相关结果，也就是向回调函数提供相关的数据
//返回值： IAsyncResult-->异步操作的状态接口。封装了异步中执行中的参数。
private void btnTest2_Click(object sender, EventArgs e)
{
  Func<int, int> myCal2 = ExecuteTask1;

  //1. 异步调用任务
  IAsyncResult result = myCal2.BeginInvoke(10, null, null);
  this.lblInfo1.Text = "正在计算...";

  //2. 可以并行计算其他任务...
  this.lblInfo2.Text = ExecuteTask2(20).ToString();

  //3. 获取异步的执行结果
  int res = myCal2.EndInvoke(result);
  //委托类型的EndInvoke（） : 借助于IAsyncResult接口对象，任务进行中，不断地的观察异步调用是否结束
  //该方法知道异步调用的方法全部参数，所以异步调用完毕后，取出异步调用结果作为返回值
  this.lblInfo1.Text = res.ToString();
}
```
