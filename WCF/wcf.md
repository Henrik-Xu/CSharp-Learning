### WCF 总结

#### 概念

`Windows Communication Foundation` 通信框架，是基于微软 `.NET` 平台编写分布式应用的统一编程模型。

`.NET` 平台下，常见的有三种分布式技术：

1. `webservice` 基于 `http` 协议的 `soap` 模式

2. `remoting` 也是一种分布式架构技术，常常用于 `tcp` 模式的二进制传输

3. `MSMQ` 这是一种分布式的离线式技术，用于业务解耦

分布式技术太多，使用不方便，需要整合，`WCF` 应运而生！因此，`WCF` 本质就是对上面技术的再次封装。

#### 第一个 WCF 程序

新建一个 `WCF` 服务，服务端程序代码如下：

```cs
//表示这个接口类遵循WCF协定
[ServiceContract]
public interface ITeachService
{
  //表示这个方法是服务协定的一部分，只有添加了以上特性，才能保证接口被WCF运行时获取到
  [OperationContract]
  string GetCourseName(int courseId);

  //根据需要添加其他接口...
}
```

接口的实现类

```cs
// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“TeachService”。
public class TeachService : ITeachService
{
  public string GetCourseName(int courseId)
  {
    //实际开发中，可以根据需要从数据库中获取数据...

    return "WCF通信技术";
  }
}
```

#### 承载 WCF 服务

谁能够承载？也就是说宿主是谁？宿主：`IIS`、`Console`、`Winform`、`Windows 服务` 等都是宿主。

下面的列子就是用控制台程序作为宿主。注意，服务必须以管理员身份运行。

```cs
static void Main(string[] args)
{
  //使用ServiceHost启动
  ServiceHost host = new ServiceHost(typeof(TeachService));
  host.Open();
  Console.WriteLine("WCF Service is Started!");
  Console.Read();
}
```

注意：新建一个 `WCF` 服务，`app.config`文件会默认添加`WCF`的配置，具体配置说明如下：

```xml
<system.serviceModel>
  <!--这个bindings是对下面的binding配置更加详细的说明，也就是会影响到下面的配置-->
  <bindings>
    <basicHttpBinding>
      <!--增加一个binding，打开的超时时间，发送信息的超时时间-->
      <binding name="myBasicHttpBinding" openTimeout="00:30:00" sendTimeout="00:30:00">
        <!--设置安全性-->
        <security mode="None"></security>
      </binding>
    </basicHttpBinding>
  </bindings>
  <behaviors>
    <!--对影响WCF运行时的一些方法进行配置-->
    <serviceBehaviors>
      <behavior name="">
        <!--为避免泄露元数据，在部署前需要将下面的值设置为false-->
        <serviceMetadata httpGetEnabled="true" httpsGetEnabled="true" />
        <!--是否给Client端抛出service的CLR异常-->
        <serviceDebug includeExceptionDetailInFaults="false" />
      </behavior>
    </serviceBehaviors>
  </behaviors>
  <services>
    <service name="MyFirstWCF.TeachService">
      <!--address：WCF服务的地址。如果address为空，则使用下面的baseAddress地址-->
      <!--binding：客户端和服务器通信的通道类型-->
      <!--contact：契约（所用的服务接口，或者说是服务的具体内容）-->
      <endpoint address="" binding="basicHttpBinding" contract="MyFirstWCF.ITeachService"
                  bindingConfiguration="myBasicHttpBinding">
        <identity>
          <dns value="localhost" />
        </identity>
      </endpoint>
      <!--元数据交换的终结点，供相应的服务向客户端做自我介绍-->
      <endpoint address="mex" binding="mexHttpBinding" contract="IMetadataExchange" />
      <host>
        <!--服务的基地址-->
        <baseAddresses>
          <add baseAddress="http://localhost:8733/MyFirstWCF/TeachService/" />
        </baseAddresses>
      </host>
    </service>
  </services>
  </system.serviceModel>
```

使用 `Windows 服务`承载 `WCF`：

[创建 Windows 服务](https://docs.microsoft.com/zh-CN/dotnet/framework/windows-services/walkthrough-creating-a-windows-service-application-in-the-component-designer)

[使用 Windows 服务承载 WCF 服务](https://docs.microsoft.com/zh-CN/dotnet/framework/wcf/feature-details/how-to-host-a-wcf-service-in-a-managed-windows-service)

使用 `IIS` 承载 `WCF`服务

[如何：在 IIS 中承载 WCF 服务](https://docs.microsoft.com/zh-CN/dotnet/framework/wcf/feature-details/how-to-host-a-wcf-service-in-iis)

#### ABC 详解

基于 `WCF` 实现客户端和服务端通信，必须了解 `ABC` 的概念

A： `Address 地址`：表示在哪里（`where`）？服务的地址就像我们访问 `web` 的时候，有一个地址。

对于 `Http` 来说有`http://myserver:8080/myservice`；对于`Tcp`，地址：`net.tcp://myserver:8080/myservice`

B： `Binding 绑定`： 表示怎么做（`How`）？就是我们 `Client` 和 `Service` 的通道，比如说我们访问 `web`，

我们使用 `http` 通道。那么 `wcf` 支持哪些通道？ `BasicHttpBinding` `WSHttpBinding`, `NetTcpBinding`,`netMSMQBinding`.

C：`Contract 契约`：表示内容是什么（`Content`）？也就是我们定义的接口是什么样的参数、返回值、接口名称等

#### 客户端使用服务

创建 `Client` 客户端程序，使用 `WCF` 服务，在右键`引用`选项中，`添加服务引用`，输入

上面`http://localhost:8733/MyFirstWCF/TeachService/`的 `Address`,将服务引向客户端

```cs
static void Main(string[] args)
{

  MyTeachService.TeachServiceClient client = new MyTeachService.TeachServiceClient();
  string courseName = client.GetCourseName(1001);

  Console.WriteLine("我们正在学习：" + courseName);
  Console.Read();
}
```

#### WCF 常用 Binding

1.`BasicHttpBinding`：符合 `WS` 的 `HTTP` 协议绑定，适用于连接到 `Web` 服务的基本配置文件规范 (例如，

基于`ASP.NET Web` 服务的服务）。

2.`WSHttpBinding`：可互操作绑定，适用于连接到终结点符合 `WS-*` 协议。

3.`NetNamedPipeBinding`：使用 `.NET Framework` 连接到同一台计算机上其他 `WCF` 终结点。

4. `NetTcpBinding`: 此绑定提供了比 `HTTP` 绑定更高的性能。使用场合： 两个`.NET` 程序搭建的一个跨机器访问情景。

`tcp` 远比 `http` [也就是 `BasicHttpBinding`]快的多。`http` 通常是 `.NET` 程序和非 `.NET` 程序通信，不能使用 `tcp` 的情

况下，使用 `SOAP`。

5. `NetMSMQBinding` : 使用场合：封装以前的 `MSMQ` 的一个专用类，用于构建离线访问，`MSMQ` 就像一个蓄水池（参考

图片），上游是 `Client`，一直往 `MSMQ` 添加。 客户端和服务器端有一个消息队列。`MSMQ`它是一个基于硬盘的形式（文本

文件）。

![MSMQ](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/WCF/msmq.png)

##### 3.1 `NetMSMQBinding` 的使用：

检查计算机是否安装消息队列 `MSMQ`,如果没有安装，请按照如下方法:

在计算机管理面板->程序->启动和关闭 `Windows` 功能->找到 `MSMQ`服务器，选中，并确定。

![OpenMsMq](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/WCF/openMSMQ.png)

在桌面->右键点击计算机（或此电脑）->管理->服务和应用程序下面的子节点->消息队列->专用消息对队列

![newMSMQ](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/WCF/newMSMQ.png)

点击空白处->新建一个消息队列（和 `ClientApp` 配置文件中 `address` 最后的消息队列名称一样的消息队列 `myteachservice`，

注意：勾选“事务性”

![newQueue](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/WCF/newQueue.png)

最终效果：

![queue](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/WCF/queue.png)

客户端不断的把消息放进消息队列，服务器端，再定时的去读取。

##### 3.2 `NetMSMQBinding` 的代码修改：

修改方法：将 `address` 修改为：`address="net.msmq://localhost/private/myteachservice"`

将 `binding` 修改为：`binding="netMsmqBinding"`

![codeerror](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/WCF/codemodify.png)

运行调试：编译并运行 `ServiceApp`，启动 `WCF`。

结果出错：协议必须使用双工。如下图：

![onewayerror](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/WCF/oneway.png)

解决：一定要单向！接口中的方法不能有返回值！客户端把消息丢进消息队列不需要返回。去掉接口方法的返回值，并修改属性 `IsOneWay =true`：

```cs
[ServiceContract]
public interface ITeachService
{
    [OperationContract(IsOneWay =true)]
    void GetCourseName(int courseId);
}
```

同时需要修改接口的实现方法。

再次运行，还是报错：绑定验证失败！

![securityerror](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/WCF/security.png)

解决：在 ServiceApp 的配置文件中添加节点：

```xml
<bindings>
  <netMsmqBinding>
    <binding name="mybinding">
      <security mode="None" />
    </binding>
  </netMsmqBinding>
</bindings>
```

同时，在 endpoint 节点中添加 bindingConfiguration="mybinding"，最后效果如下：

```xml
<endpoint address="net.msmq://localhost/private/myteachservice" binding="netMsmqBinding"
        contract="NetMSMQBinding_ServiceApp.ITeachService" bindingConfiguration="mybinding">
  <identity>
      <dns value="localhost" />
  </identity>
</endpoint>
```

再次启动，依然有错，提示：方案不支持端口号

![porterror](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/WCF/port.png)

解决：消息队列本身不支持端口号，把配置文件中的端口号 1234 去掉。再次启动，运行成功！这时候把服务器端关掉。

重新配置`ClientApp`端的服务引用，出现错误

![updateserviceerror](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/WCF/updateservice.png)

![configerror](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/WCF/configerror.png)

把客户端配置文件全部删掉，只保留一个 `configuration` 节点，再更新。

最终服务器端代码

```cs
public class TeachService : ITeachService
{
  public void GetCourseName(int courseId)
  {
      Console.WriteLine($"接收到TeachService消息！课程编号：{courseId} 时间：{DateTime.Now.ToLongTimeString()}");
  }
}
```

客户端代码：

```cs
static void Main(string[] args)
{
    MyWCFService.TeachServiceClient client = new MyWCFService.TeachServiceClient();
    Console.WriteLine("ClientApp启动");
    System.Threading.Thread.Sleep(3000);
    client.GetCourseName(1001);
    Console.WriteLine("ClientApp发送第1次消息！");
    System.Threading.Thread.Sleep(2000);
    client.GetCourseName(1002);
    Console.WriteLine("ClientApp发送第2次消息！");
    System.Threading.Thread.Sleep(1000);
    client.GetCourseName(1003);
    Console.WriteLine("ClientApp发送第3次消息！");

    Console.WriteLine("消息发送完毕！");
    Console.Read();
}
```

最后的运行结果

消息队列

![queueresult](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/WCF/messagequeue.png)

服务端客户端

![serverresult](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/WCF/wcfserver.png)

#### 总结

本文讲述了`WCF`的基础知识，这将在我们使用 `WCF`创建服务中十分有用，对于`WCF`来说，还有很多很深入的地方，

我们可以在工作中边使用边学习。
