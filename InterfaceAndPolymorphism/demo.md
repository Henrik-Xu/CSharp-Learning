### 接口和多态

#### 1. .NET 中接口的定义规范

- a.使用关键字 `interface` 定义，接口类名称通常使用`I`开头。

- b.接口中的属性、方法等，只是做一个声明，而没有任何实现。

- c.接口中的属性、方法等，默认都是 `public`,不要画蛇添足。

#### 2.接口的特点

- a.接口具有强制性，实现接口的类必须实现接口的所有成员。

- b.一个类既可以实现多个接口，也可以同时继承其它类。

#### 3.提高团队成员并行开发项目的效率

- a.接口使用者只关心接口的应用功能，而不关心接口的实现细节。

- b.接口的实现者只关心接口如何实现的内部细节，而不关心谁使用。

#### 4.接口的应用场合

- a.如果某一个功能点需求变化较多，应使用接口保证系统的可扩展性。

- b.如果想实现团队成员的并行开发，可以使用接口来规范对象的使用。

#### 5.接口和抽象类的比较

|        |                 抽象类                 |             接口             |
| ------ | :------------------------------------: | :--------------------------: |
| 不同点 |            用`abstract`定义            |      用`interface`定义       |
|        |             只能继承一个类             |       可以实现多个接口       |
|        |      非抽象派生类必须实现抽象方法      | 实现接口的类必须实现所有成员 |
|        |       需要`override`实现抽象方法       |           直接实现           |
| 相似点 |            都不能直接实例化            |
|        |           都包含未实现的方法           |
|        | 子类或“接口实现类”必须实现未实现的方法 |

#### 6. demo

`IMultiPrint.cs`

```cs
/// <summary>
/// 多功能打印机接口
/// </summary>
public interface IMultiPrinter
{
  // 打印
  void Print(string contents);
  //复印
  void Copy(string contents);
  // 传真
  bool Fax(string contents);
}
```

然后分别定义三个具体的类实现`IMultiPrint`接口：

`CanonPrinter.cs`

```cs
public class CanonPrinter : IMultiPrinter
{
  public void Print(string contents)
  {
    //在这里编写打印程序...
    Console.WriteLine("佳能打印机正在打印：" + contents);
  }

  public void Copy(string contents)
  {
    //在这里编写复印程序...
    Console.WriteLine("佳能打印机正在复印：" + contents);
  }

  public bool Fax(string contents)
  {
    //在这里编写传真程序...
    Console.WriteLine("佳能打印机正在传真：" + contents);
    return true;
  }
}
```

`EpsonMultiPrinter.cs`

```cs
public  class EpsonMultiPrinter:IMultiPrinter
{
  public void Print(string contents)
  {
    //在这里编写打印程序...
    Console.WriteLine("欢迎使用Epson打印机！");
    Console.WriteLine("Epson打印机正在打印：" + contents);
  }

  public void Copy(string contents)
  {
    //在这里编写复印程序...
    Console.WriteLine("欢迎使用Epson打印机！");
    Console.WriteLine("Epson打印机正在复印：" + contents);
  }

  public bool Fax(string contents)
  {
    //在这里编写传真程序...
    Console.WriteLine("欢迎使用Epson打印机！");
    Console.WriteLine("Epson打印机正在传真：" + contents);
    return true;
  }
}
```

`HPMultiPrinter.cs`

```cs
class HPMultiPrinter : IMultiPrinter
{
  public void Print(string contents)
  {
    //在这里编写打印程序...
    Console.WriteLine("欢迎使用HP打印机！");
    Console.WriteLine("HP打印机正在打印：" + contents);
  }

  public void Copy(string contents)
  {
    //在这里编写复印程序...
    Console.WriteLine("欢迎使用HP打印机！");
    Console.WriteLine("HP打印机正在复印：" + contents);
  }

  public bool Fax(string contents)
  {
    //在这里编写传真程序...
    Console.WriteLine("欢迎使用HP打印机！");
    Console.WriteLine("HP打印机正在传真：" + contents);
    return true;
  }
}
```

`Program.cs`中实现多态

```cs
class Program
{
  static void Main(string[] args)
  {
    Print(new HPMultiPrinter());
    Console.WriteLine("-----------------------------------------");
    Print(new CanonPrinter());
    Console.ReadLine();
  }

  //多态特性（接口作为方法的返回值、接口作为方法的参数）
  static void Print(IMultiPrinter printer)
  {
    printer.Print("个人信息表");
    printer.Copy("个人信息");
    printer.Fax("身份证");
  }
}
```
