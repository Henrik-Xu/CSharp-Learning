### 正确的处理 C# 程序中的异常

前言：虽然 `C#`中异常处理很简单，但是对程序员来说，仅仅`try...catch(){}`是不够的，更应该知道什么时机抛出异常，如何正确的处理异常，

以及何种种类的异常应该被抛出。

### C# 中的异常分类

1. 系统级异常

`.NET`平台引发的异常称为系统异常，这些异常被认为是无法修复的致命错误，主要有`ArgumentOutOfRangeException`,

`IndexOutOfRangeException`,`StackOverflowException`等，它们主要派生自 `SystemException`

```cs
public class SystemException:Exception
{

}
```

2. 应用程序级异常

应用程序级异常的目的是标识出错误的来源，告诉程序员异常是由正在执行的应用程序代码库引发的，而不是`.NET`基础类库或`.NET`运行时引擎引发的。

```cs
public class ApplicationException:Exception
{

}
```

3. 自定义异常

最佳实践：自定义异常应该继承自`ApplicationException`

```cs
public class MyCustomException:ApplicationException
{

}
```

什么时候需要自定义异常？

一般在出现的类与该错误关系紧密时才需要创建自定义异常。例如：一个自定义文件类引发许多文件相关的错误，一个数据库访问对象引发关于特定数据库表的错误等。

自定义异常遵守的规范？

1. 继承自 `Exception/ApplicationException`类,虽然自定义异常应当派生自`ApplicationException`类，但是很少有程序员这样做。更常见的情况是将其

简单的归入`System.Exception`类。这在技术上是合法的。

2. 有 `[System.Serializable]`特性标记。

3. 定义一个默认的构造函数。

4. 定义一个设定继承的 `Message` 属性的构造函数。

5. 定义一个处理“内部异常”的构造函数。

6. 定义一个处理类型序列化的构造函数。

```cs
[Serializable]
public class MyException : Exception
{
    public MyException() { }
    public MyException(string message) : base(message) { }
    public MyException(string message, Exception inner) : base(message, inner) { }
    protected MyException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

      // 其他自定义属性、构造函数、数据成员......
}
```

并且，我们宇宙最强的 `IDE VS Studio` 也为我们封装好了 `Exception`的代码片段，只要打出关键字并且按两次 `Tab`键即可生成如上的代码片段。

#### 处理多个异常

基本原则： 多个异常处理的 `catch`块应该从最具体的到最抽象的，注意始终要在最后一层加上对基类 `Exception`的捕获，这样做的目的是为了防止

未知的错误导致程序崩溃。

```cs
try
{
  // do stuff that might throw an exception
}
Catch(CustomException a1)
{
  // handle a1 exception
}
Catch(CustomException2 a2)
{
  // handle a2 exception
}
Catch(Exception ex)
{
  // handle ex
}
```

#### finally 块

`finally`块不是必须要有的，它是为了保证不管是否有（任何类型）异常，`finally 中的代码语句始终都能被执行。**即使是 return 关键字的使用**

`finally`块主要用来销毁对象、关闭文件、断开数据库连接等操作时，将资源清理加入到 `finally`块来确保操作正确执行。

```cs
try
{

}
finally
{

}
```

如果类实现了`IDisposable`接口，则上述语法等效于 `using`

#### 正确的重新抛出异常（rethrow exception）

有的程序员经常在开发中编写这样的代码:

```cs
try
{
  // do stuff that might throw an exception
}
catch(Exception e)
{
  throw e;
}
```

上面的代码有两个问题:

1. `throw ex` 将异常中的调用堆栈重置到抛出语句所在的位置;丢失有关异常实际创建位置的信息。

2. 只是捕捉和重新抛出没有任何的意义，进一步说，就跟没有使用异常处理的性质一样。

为了避免以上的问题，我们应该采用如下的方式

```cs
try
{
  // do stuff that might throw an exception
}
catch(Exception e)
{
  throw;
}
```

或者

```cs
try
{
  // do stuff that might throw an exception
}
catch(Exception ex)
{
  throw new Exception("my custom exception",ex);
}
```
