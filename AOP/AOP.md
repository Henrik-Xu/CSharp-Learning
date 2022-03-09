# AOP

## 伪代码演示

Aspect Oriented Programing : AOP addressed the problem of cross-cutting, which wolud be any kind of code that is repeated

in different methods and can't normally be completely refactored into its own module, like with logging or verification.

So, with AOP you can leave the stuff out of the main code and define it vertically like so:

```js
//pseudo code
function mainProgram()
{
  var x = foo();
  doSomethingWith(x);
  return x;
}

aspect logging
{
  before (mainProgram is called)
  {
    log.Write("entering mainPrograming");
  }
  after (mianProgram is called)
  {
    log.Write("existing mainProgram with return value of "+mainProgram.returnValue);
  }
}
```

And the an aspect-weaver is used to compile the code into this:
```js
function mainProgram()
{
  log.Write("entering mainPrograming");

  var x = foo();
  doSomethingWith(x);

  log.Write("existing mainProgram with return value of "+x);
  return x;

}
```

Benefit of AOP :

1. The logic for each concern is now in one place, as opposed to being scattered all overt the code base.

2. Classes are cleaner since the only contain code for their primary concert(or core functionality) and

secondary concerns have been moved to aspects.

## AOP 的理解

1. 概念

`AOP`：`Aspect Oriented Programming` 是一种能在现有面向对象编程基础上，将我们项目中后续需要动态扩展的内容，实现程序修改的最小化。

2. `OOP` 和`AOP` 以及 设计模式

* `OOP`：是我们项目设计的基本标准、方法、技巧、经验的一个综合。不管在哪个环节，都会使用OOP的这些内容，去指导我们设计和写程序。

* `设计模式`：在 `OOP` 基础上对于特殊的问题，但是这种特殊问题，又经常出现，而总结的一套经验，在任何一个设计模式中

我们都会处处体现 `OOP` 的思想

* `AOP`：其实就是在前面 `OOP` +设计模式的情况下，如果某些问题，还是不能很好的解决，我们可以进一步的基于AOP思想来完善。

## AOP 使用场景

在什么情况下，我们需要使用AOP方式呢？

功能性业务：比如你做一个具体的根据相关条件的查询，或者插入、修改某些记录....

  比如银行业务：当你开一张银行卡的时候，银行会把你的个人信息，在银行的数据库中，增加一条记录。    

  再比如，你取款，每次取的时候，在银行的系统中，都会增加一个取款相关的记录。

  以上都是功能性业务。

系统级业务：比如你开卡的时候，同时会给你增加短信提醒。还有你取款的时候，同时会有相关的验证

    还有日志功能。

    比如，我们设计一个功能性业务（区别系统级业务）的时候，对于你后续要集成的系统级业务，我们必须

    要很好的分开，对于系统级业务，我们可以后面在主任务完成后，根据需要增加进去，但是对功能性业务

    没有影响。这种情况下，你就要考虑 `AOP`

## AOP 的几种实现方式

首先，我们做一些准备工作：

```cs
/// <summary>
///课程订单实体类
/// </summary>
public class CourseOrder
{
  public int OrderId { get; set; }
  public int StudentId { get; set; }
  public int CourseId { get; set; }
  public string CourseName { get; set; }
  public int CoursePrice { get; set; }
  public int SchoolId { get; set; }//机构ID

  //。。。
}

/// <summary>
/// 课程订单接口
/// </summary>
public interface IOrderService
{
  //提交订单
  int SubmitOrder(CourseOrder order);

  //查询订单
  List<CourseOrder> QueryOrders();
}

// <summary>
/// 接口的实现（功能性业务）
/// </summary>
public class OrderService : IOrderService
{
  public List<CourseOrder> QueryOrders()
  {
    //实际项目中，在这里编写具体的查询业务...

    return new List<CourseOrder>
    {
      new CourseOrder {  CourseId=1000, CourseName=".NET 学习", CoursePrice=3500, OrderId=2000, SchoolId=502100, StudentId=293400},
      new CourseOrder {  CourseId=1001, CourseName="JAVA 学习", CoursePrice=4500, OrderId=2001, SchoolId=502101, StudentId=293400},
      new CourseOrder {  CourseId=1002, CourseName="前端学习", CoursePrice=5500, OrderId=2002, SchoolId=502102, StudentId=293400}
    };
  }

  public int SubmitOrder(CourseOrder order)
  {
    //在这里编写具体的查询业务...

    Console.WriteLine("--------------------------------《核心业务》课程订单被正确提交...");

    //如果你不用AOP思想的话，你可能想到，在这个地方写调用第三方支付接口完成支付等任务（加入提醒业务）

    return 1000;
  }
}
```

`Main` 方法中的调用：

```cs
static void Main(string[] args)
{
  Console.WriteLine("-------【AOP】面向切面编程-------\r\n\r\n");

  //创建订单信息对象
  CourseOrder order = new CourseOrder
  {
    CourseId = 1001,
    CourseName = ".NET学习",
    CoursePrice = 5500,
    SchoolId = 502102,
    StudentId = 2200228
  };

  //【1】基于普通的接口适用
  Console.WriteLine("-------------------------------【基于普通接口编程】\r\n");
  IOrderService orderService = new OrderService();
  int result = orderService.SubmitOrder(order);
  Console.WriteLine("订单提交返回结果：" + result);
}
```
### 1. 基于装饰器模式实现 `AOP`

装饰器设计模式充分体现AOP思想

装饰概念：就是对一个东西的“包装”和“点缀”。

```cs
/// <summary>
/// 装饰器类：同样需要使用接口，但是并不是直接把原有替换
/// 
/// 这个类只不过是让用户通过装饰器类，间接的去使用接口实现类
/// 
/// 因为当我们使用的时候，具体的实现类，是通过构造方法注入进来的！
/// 
/// 也就是说，只有运行时，才能得知具体的对象，其实这个就是多态的一种深入体现！
/// </summary>
public class AOPBasedDecorator : IOrderService
{
  //建立依赖关系
  private IOrderService orderService;

  //通过构造方法注入具体的对象
  public AOPBasedDecorator(IOrderService orderService)
  {
    this.orderService = orderService;
  }

  //在接口实现方法中，调用原有接口实现类的“实现”
  public List<CourseOrder> QueryOrders()
  {
    return this.orderService.QueryOrders();
  }
  //下面的方法，请大家体会，装饰器设计模式是如何做到“装饰”功能！
  public int SubmitOrder(CourseOrder order)
  {
    //【1】切入第一个方法
    AddedValidateTime(order);

    int result= this.orderService.SubmitOrder(order); //功能性业务核心调用

    //【2】切入第二个方法
    CallPayInterface(order);

    //【3】切入第三个方法
    AddedReminder(order);

    return result;
  }


  #region 想后续扩展的方法

  //自定义扩展功能1：验证上课时间是否和其他课程冲突，以便提示学员是否决定报名
  public bool AddedValidateTime(CourseOrder order)
  {
    //在这里编写具体的验证过程...

    Console.WriteLine("切入方法1：报名前的上课时间是否冲突检验方法...");
    return false;
  }
   //自定义扩展功能2：通过第三方的接口付款（这个接口通常由专门的人去开发）
  public int CallPayInterface(CourseOrder order)
  {
    //在这里调用编写第三方支付接口的调用过程...

    Console.WriteLine("切入方法2：第三方支付接口的调用过程...");
    return 1;
  }

  //自定义扩展功能3：课程报名成功后，把当前学员的当前课程加入上课提醒日志
  public bool AddedReminder(CourseOrder order)
  {
    //在这里编写具体的验证过程...

    Console.WriteLine("切入方法3：报名后的上课弹窗提醒方法...");
    return true;
  }
  #endregion
```
`Main` 方法中的调用：

```cs
static void Main(string[] args)
{
  Console.WriteLine("-------【AOP】面向切面编程-------\r\n\r\n");

  //创建订单信息对象
  CourseOrder order = new CourseOrder
  {
    CourseId = 1001,
    CourseName = ".NET学习",
    CoursePrice = 5500,
    SchoolId = 502102,
    StudentId = 2200228
  };

  //【2】基于装饰器设计模式为核心业务切入方法
  Console.WriteLine("\r\n-------------------------AOP实现方式1：【基于装饰器设计模式为核心业务切入方法】\r\n");

  IOrderService orderService2 = new AOPBasedDecorator(new OrderService());
  int result2 = orderService2.SubmitOrder(order);
  Console.WriteLine("订单提交返回结果：" + result2);
}
```

缺点：虽然装饰器设计模式，能够很好的体现AOP思想，但是实际的应用价值并不是很高，因为这个装饰器类，是固定某个功能性接口的。

而不具有通用性。对应批量的系统级功能的扩展，显然存在缺陷。

### 2. 基于代理方式实现

解决问题：其实就是针对装饰器设计模式的不足，而且提供的一种工具。

步骤实现：

  【1】同样需要一个接口实现类（和装饰器模式几乎是一样的，但是有一个小区别，那就是多了一个继承）

  【2】添加一个代理实现类，和一个业务代理对象创建类

```cs
/// <summary>
/// 为代理使用的业务接口实现类：需要继承一个父类MarshalByRefObject，继承这个父类才允许远程代理创建对象
/// 
/// 使用代理的目的：就是帮我们创建具体的业务实现对象
/// 
/// </summary>
public class OrderServiceProxy : MarshalByRefObject, IOrderService
{
  public List<CourseOrder> QueryOrders()
  {
    //实际项目中，在这里编写具体的查询业务...

    return new List<CourseOrder>
    {
      new CourseOrder {  CourseId=1000, CourseName=".NET 学习", CoursePrice=3500, OrderId=2000, SchoolId=502100, StudentId=293400},
      new CourseOrder {  CourseId=1001, CourseName="JAVA 学习", CoursePrice=4500, OrderId=2001, SchoolId=502101, StudentId=293400},
      new CourseOrder {  CourseId=1002, CourseName="前端学习", CoursePrice=5500, OrderId=2002, SchoolId=502102, StudentId=293400}
    };
  }

  public int SubmitOrder(CourseOrder order)
  {
    //在这里编写具体的查询业务...

    Console.WriteLine("--------------------------------《核心业务》课程订单被正确提交...");

    return 1000;
  }
}
```

基于远程代理实现业务扩展

```cs
/// <summary>
/// 基于远程代理实现业务扩展
/// 
/// T  定义的是泛型类，目的是实现可以对任意对象的扩展，从而解决装饰器设计模式的单一问题
/// </summary>
/// <typeparam name="T"></typeparam>
public class AOPRealProxy<T> : RealProxy
{
  private T targetObject;//使用依赖，保存要装饰的对象

  //初始化远程代理父类，需要把当前代理的类型传递过来
  public AOPRealProxy(T tObject) : base(typeof(T))
  {
    this.targetObject = tObject;
  }

  /// <summary>
  /// 重写了一个抽象方法：实际上这个方法非常的重要，它就是我们要调用的核心业务
  /// </summary>
  /// <param name="msg"></param>
  /// <returns></returns>
  public override IMessage Invoke(IMessage msg)
  {
    AddedValidateTime(msg); //【1】切入第1个方法  

    //核心业务调用
    IMethodCallMessage callMsg = (IMethodCallMessage)msg;
    var result = callMsg.MethodBase.Invoke(targetObject, callMsg.Args);//参数1：核心业务对象 参数2：方法参数

    CallPayInterface(msg);      //【2】切入第2个方法
    AddedReminder(msg);      //【3】切入第3个方法

    return new ReturnMessage(result, new object[0], 0, null, callMsg);
  }

  #region 【3】编写切入的扩展方法（这些是我们开发中自己要写的）

  //实际开发中，这个地方，完全可以是一个模块

  //自定义扩展功能1：验证上课时间是否和其他课程冲突，以便提示学员是否决定报名
  public bool AddedValidateTime(IMessage msg)
  {
    //在这里编写具体的验证过程...

    //order参数大家可以参考前面的转换方法获得..
    object[] args = ((IMethodCallMessage)msg).Args;

    Console.WriteLine("切入方法1：报名前的上课时间是否冲突检验方法...");
    return false;
  }
  //自定义扩展功能2：通过第三方的接口付款（这个接口通常由专门的人去开发）
  public int CallPayInterface(IMessage msg)
  {
    //在这里调用编写第三方支付接口的调用过程...

    Console.WriteLine("切入方法2：第三方支付接口的调用过程...");
    return 1;
  }

  //自定义扩展功能3：课程报名成功后，把当前学员的当前课程加入上课提醒日志
  public bool AddedReminder(IMessage msg)
  {
    //在这里编写具体的验证过程...

    Console.WriteLine("切入方法3：报名后的上课弹窗提醒方法...");
    return true;
  }

  #endregion
}

/// <summary>
/// 代理业务类：完成被代理对象的创建
/// </summary>
public class ProxBusiness
{
  public static T CreateObject<T>()
  {
    AOPRealProxy<T> realProxy = new AOPRealProxy<T>(Activator.CreateInstance<T>());
    return (T)realProxy.GetTransparentProxy();//返回当前我们注入的实例对象
  }
}
```
`Main` 方法中的调用：

```cs
static void Main(string[] args)
{
  Console.WriteLine("-------【AOP】面向切面编程-------\r\n\r\n");

  //创建订单信息对象
  CourseOrder order = new CourseOrder
  {
    CourseId = 1001,
    CourseName = ".NET学习",
    CoursePrice = 5500,
    SchoolId = 502102,
    StudentId = 2200228
  };

  //【3】基于动态代理方式为核心业务切入方法
  Console.WriteLine("\r\n------------------AOP实现方2：【基于远程动态代理方式为核心业务切入方法】\r\n");
  IOrderService orderService3 = ProxBusiness.CreateObject<OrderServiceProxy>();
  int result3 = orderService3.SubmitOrder(order);
  Console.WriteLine("订单提交返回结果：" + result3);
}
```

### 3、使用第三方框架实现

对比发现：使用第三方框架，比前面的系统代理类更简单。我们可以根据自己的需要，寻找更多的，更好用的第三方框架。

功能性业务

```cs
/// <summary>
/// 接口的实现（功能性业务）
/// 
/// 这个接口实现类，实现方法必须是virtual
/// </summary>
public class OrderServiceCastle : IOrderService
{
  public virtual List<CourseOrder> QueryOrders()
  {
    //实际项目中，在这里编写具体的查询业务...

    return new List<CourseOrder>
    {
      new CourseOrder {  CourseId=1000, CourseName=".NET 学习", CoursePrice=3500, OrderId=2000, SchoolId=502100, StudentId=293400},
      new CourseOrder {  CourseId=1001, CourseName="JAVA 学习", CoursePrice=4500, OrderId=2001, SchoolId=502101, StudentId=293400},
      new CourseOrder {  CourseId=1002, CourseName="前端学习", CoursePrice=5500, OrderId=2002, SchoolId=502102, StudentId=293400}
    };
  }

  public virtual int SubmitOrder(CourseOrder order)
  {
    //在这里编写具体的查询业务...

    Console.WriteLine("--------------------------------《核心业务》课程订单被正确提交...");

    //如果你不用AOP思想的话，你可能想到，在这个地方写调用第三方支付接口完成支付等任务（加入提醒业务）

    return 1000;
  }
}
```

例如 `Castle Core`

```cs
//引入命名空间
using Castle.Core.Interceptor;  //nuget这个Castle.Core

/// <summary>
/// 基于Castle动态代理完成对象扩展
/// 
/// 需要实现一个拦截器接口
/// </summary>
public class AOPBasedCastle : IInterceptor
{
  /// <summary>
  /// 接口实现方法：主要是完成核心业务调用
  /// </summary>
  /// <param name="invocation"></param>
  public void Intercept(IInvocation invocation)
  {
    AddedValidateTime(invocation); //【1】切入第1个方法  

    invocation.Proceed();//核心业务调用

    CallPayInterface(invocation);      //【2】切入第2个方法
    AddedReminder(invocation);      //【3】切入第3个方法           
  }

  #region 【3】编写切入的扩展方法（这些是我们开发中自己要写的）

  //实际开发中，这个地方，完全可以是一个模块

  //自定义扩展功能1：验证上课时间是否和其他课程冲突，以便提示学员是否决定报名
  public bool AddedValidateTime(IInvocation invocation)
  {
    //在这里编写具体的验证过程...

    object[] args = invocation.Arguments; //这个地方我仅仅是告诉你，怎么得到这个参数，然根据你的业务你去使用

    Console.WriteLine("切入方法1：报名前的上课时间是否冲突检验方法...");
    return false;
  }
  //自定义扩展功能2：通过第三方的接口付款（这个接口通常由专门的人去开发）
  public int CallPayInterface(IInvocation invocation)
  {
    //在这里调用编写第三方支付接口的调用过程...

    Console.WriteLine("切入方法2：第三方支付接口的调用过程...");
    return 1;
  }

  //自定义扩展功能3：课程报名成功后，把当前学员的当前课程加入上课提醒日志
  public bool AddedReminder(IInvocation invocation)
  {
    //在这里编写具体的验证过程...

    Console.WriteLine("切入方法3：报名后的上课弹窗提醒方法...");
    return true;
  }
  #endregion
}
```

 `Main` 方法中的调用：

```cs
using Castle.DynamicProxy;

static void Main(string[] args)
{
  Console.WriteLine("-------【AOP】面向切面编程-------\r\n\r\n");

  //创建订单信息对象
  CourseOrder order = new CourseOrder
  {
    CourseId = 1001,
    CourseName = ".NET学习",
    CoursePrice = 5500,
    SchoolId = 502102,
    StudentId = 2200228
  };

 //【4】第三方的代理框架
  Console.WriteLine("--------------\r\nAOP实现方3：【基于第三方Castle动态代理方式为核心业务切入方法】\r\n");
  ProxyGenerator generator = new ProxyGenerator();//为类或接口提供代理对象
  AOPBasedCastle intercetpor = new AOPBasedCastle();

  IOrderService orderService4 = generator.CreateClassProxy<OrderServiceCastle>(intercetpor);
  int result4 = orderService4.SubmitOrder(order);

  Console.WriteLine("订单提交返回结果：" + result4);
}
```

Castle使用的问题：

【1】 本框架不针对具体的业务做扩展，而是一种通用扩展。也就说通过代理类创建的对象，在调用的时候，所有的行为都会被自动的切入系统级业务。

【2】解决方法：只有需要扩展这些业务的地方，我们才使用代理框架产生对象，如果不需要，请使用原有的对象。

【3】事务一分为二：这种通用扩展也有好处，比如我对功能性的某些业务，做批量的切入，（日志、缓存等）

 这时候我们写一个拦截器，你会发现所有的功能性业务都会自定切入。

 ### 其他第三方 `AOP` 工具

 ...






