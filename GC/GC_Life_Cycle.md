### 对象的生命周期

#### 前言

一旦你理解了 `.NET` 的垃圾收集器是如何工作的，那么一些可以引起 `.NET` 应用程序崩溃的神秘问题的原因就会变得更加

清晰。`.NET` 可能已经承诺显式管理内存，但如果你希望避免与内存相关的错误和一些性能问题，则在开发 `.NET` 应用程

序时仍需要分析内存的使用情况。

`CLR` 通过垃圾回收（garbage collection）来管理已分配的类实例（又称为对象）。`C#` 程序员从来不直接从内存中删除

一个对象，在 `C#` 语言中没有 `delete` 关键字。相反， `.NET` 对象被分配到一块叫做托管堆（managed head）的内存

区域上，在将来的某一时刻被垃圾回收器自动销毁。

```cs
MyCustomObject instance = new MyCustomObject();
```

new 关键字返回的是一个指向堆上对象的引用，而不是真正的对象本身。并且，垃圾回收器使用了两个不同的堆，一个专门

用来存储非常大的对象，这个堆在回收周期中较少顾及，因为要重新定位大对象的性能开销很大。

![对象与变量的关系](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/GC/capture.png)

如何定义一个对象不在需要：简短来说只有在一个对象从代码库的任何部分都不可访问时，垃圾回收器就会从堆中删除它。

#### .NET 内存管理的规则

1. 使用 `new` 关键字将一个类实例分配在托管堆上，然后就不用在管。

2. 如果托管堆没有足够的内存类分配所请求的对象，就会进行垃圾回收。

#### 应用程序根

垃圾回收器根据对象的 `root` 是否存在来决定一个对象什么时候不再需要。在一次垃圾回收过程中，运行库将检查托管

对象上的对象，判断应用程序是否仍然可访问它们，即是否还是有根的(root)。`GC Root` 不是对象本身而是指向托管堆

上对象的引用。

`.NET`中主要有六种 `root` 类型：

1.全局对象的引用。

2.静态对象/静态字段的引用。

3.应用程序中代码库中局部对象的引用。

4.传递进一个方法的对象参数的引用。

5.等待被终结 `Finalize`对象的引用。

6. 任何引用对象的 CPU 寄存器。

#### 对象的代

当 `CLR` 试图寻找不可访问的对象时，它不会逐个检查托管堆上的每一个对象。这样做将花费大量时间，尤其是在一些实际

的应用程序中。于是，代(generation)的概念便随着产生。

堆上的每一个对象被指定属于某“代”(generation)。代的设计思路很简单：对象在堆上存在的时间越长，它就更可能应该保

留。例如：定义桌面应用程序主窗口的类将一直停留在内存中直到应用程序结束。相反，最近放在堆上的对象可能很快就不

可访问了（例如在一个方法作用域中创建的对象）。基于这些假设，堆上的每一个对象都属于下列某代。

1. 第 0 代：从没有被标记为回收的新分配的对象。

2. 第 1 代：在上一次垃圾回收中没有被回收的对象（也就是，他被标记为回收，但因为已经获取了足够的堆空间而没有被

删除）。第 0 和 1 代也叫暂时代。

3. 第 2 代：在一次以上的垃圾回收后仍然没有被回收的对象。

垃圾回收器首先要调查所有的第 0 代对象。如果标记和清除这些对象得到了所需数量的空闲内存，任何没有被回收的对象都

被提升到第 1 代。如果算上所有的第 0 代对象后，仍然需要更多的内存，就会检查第 1 代对象的"可访问性"并相应地进行

回收。没有被回收的第一代对象随后被提升到第 2 代。如果垃圾回收器仍然需要更多的内存，它会检查第 2 代对象的可访

问性。这时，如果一个第 2 代对象在垃圾回收后仍然存在，它仍然是第 2 代对象，因为这是预定义的对象代的上限。

#### 并发回收和后台回收

并发回收：`.NET 4.0`之前，运行时使用并发垃圾回收技术来清理不在使用的对象。当对暂时代对象执行回收时，垃圾回收器

会暂时挂起当前进程中的所有活动线程，以确保应用程序在回收过程中不会访问托管堆。并发回收通过专门的线程清理不在暂

时代中的对象。这降低了但并没有消除判断活动线程对 `.NET` 运行时的需求。

后台回收：`.NET 4.0`以后使用后台垃圾回收非暂时代上的对象。

#### System.GC 类型

[GC](https://docs.microsoft.com/zh-CN/dotnet/api/system.gc?view=netframework-4.8)

作用：通过编程使用一些静态成员与垃圾回收器进行交互。（注意：一般我们不需要编程通过 `GC` 类来垃圾回收,可能会造

成性能问题）。

强制垃圾回收：一些场景下，通过编程使用 `GC.Collect()` 强制垃圾回收可能会有好处，即你对你写的代码有把握：

1. 应用程序将要进入一段代码，后者不希望被可能的垃圾回收中断。

2. 应用程序刚刚分配非常多的对象，你想尽可能多地删除已获得的内存。

```cs
// 强制一次垃圾回收，并等待每一个对象都被终结。
GC.Collect();
// 该方法应该总在 GC.Collect() 后调用，它会在回收过程中挂起调用的线程，这样保证代码不调用当前正在被销毁的对象的方法
GC.WaitForPendingFinalizers();
```

```cs
GC.Collect(0,GCCollectionMode.Forced); //只检查第 0 代对象，并且立马执行垃圾回收
GC.WaitForPendingFinalizers();
```

#### Finalizers(终结器)

##### 何时使用？

类使用了非托管资源，例如：原始的操作系统文件句柄，原始的非托管数据库连接，非托管内存等，`Finalize()` 能保证

`.NET` 对象能在垃圾回收时清除非托管资源。

##### `Destructor`析构函数和`Finalize()`的关系?

`C#` 中的析构函数重写了 `System.Object.Finalize` 方法，你必须用析构语法去做，手动重写`Finalize`方法会编译报错。

注意：

1.无法在结构中定义终结器。 它们仅用于类。

2.一个类只能有一个终结器。

3.不能继承或重载终结器。

4.不能手动调用终结器,可以自动调用它们。

5.终结器不使用修饰符或参数

```cs
class MyClass
{
  ~MyClass()
  {
    // do some clean ...
  }
}
```

终结器隐式调用对象基类上的 `Finalize`。 因此，对终结器的调用会隐式转换为以下代码：

```cs
protected override void Finalize()
{
  try
  {
      // do some clean ...
  }
  finally
  {
      base.Finalize();
  }
}
```

##### 方法何时调被用：

三种场景

1. `CLR`进行垃圾回收时

2. 手动调用 `GC.Collect()`方法

3. 应用程序域从内存中卸载时：当应用程序域从内存中卸载时，`CLR`自动调用在它的生命周期中创建的每一个可终结对象

的终结器。

使用终结器释放资源：

一般来说，`C#` 所占用的内存管理空间比使用不面向带有垃圾回收机制的运行时的语言进行开发时所使用的内存管理空间

要少。这是因为 `.NET Framework` 垃圾回收器会隐式管理对象的内存分配和释放。 但是，如果应用程序封装非托管的

资源，例如窗口、文件和网络连接，则应使用终结器释放这些资源。 当对象符合终止条件时，垃圾回收器会运行对象的

`Finalize` 方法

#### 显示释放资源（IDisposable）

针对如上非托管资源的释放问题，我们应该实现 `IDisposable` 接口，手动释放资源，因为非托管资源都是非常宝贵的，如

数据库和文件句柄，所以它们应该尽可能快的被清除，而不能靠垃圾回收的发生。这样可大大提高应用程序的性能。

```cs
public interface IDisposable
{
  void Dispose();
}
```

##### 实现 `Dispose` 方法的标准模式

以下方法如果没有显示调用 `Dispose()`方法，则会在下一次 `GC` 垃圾清理时调用终结器，这样确保了所有的资源正确的释放。

```cs
class MyClass:IDisposable
{
  // 私有标志，用来判断Dispose()是否已经被调用
  private bool disposed = false;

  public void Dispose()
   {
      Dispose(true);
      // 通知 CLR 不需要再调用终结器释放此资源
      GC.SuppressFinalize(this);
   }

   private  void Dispose(bool disposing)
   {
      if (disposed)
         return;

      if (disposing) {
         // 释放托管资源
      }
      //释放非托管的资源

      // 表示已经释放过
      disposed = true;
   }

  ~MyClass()
  {
    Dispose(false);
  }
}
```

##### using 关键字

`using`语句提供了一个更便利的方式释放实现了 `IDisposable` 接口的资源，`using`其实就是 `try...finally`的语法糖。

```cs
using(var myComObject = new MyComObject())
{
  // do your business
  myComObject.Run();
}
```

等价于

```cs
{
  var myComObject = new MyComObject()
  try
  {
    // do your business
    myComObject.Run();
  }
  finally
  {
    if(myComObject != null)
    {
      ((IDisposable)myComObject).Dispose();
    }
  }
}
```

注意：

1.`using`包含的需要释放的资源应该包裹在 `using`语句块中,不应该在 `using`外部初始化示例。否则在调用时可能报错。

2. `using(var r1 = new MyResource1(),var r2 = new MyResource2())`,`using`支持多个对象的同时声明。

```cs
var myComObject = new MyComObject()
using(myComObject)
{
  // do your business
  myComObject.Run();
}
myComObject.SomeProperty = CustomProperty;// 可能会报错
```

#### 参考文章

_精通 C#_

[终结器](https://docs.microsoft.com/zh-CN/dotnet/csharp/programming-guide/classes-and-structs/destructors)
