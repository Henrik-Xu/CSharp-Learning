### Attribute(特性)

#### 定义

.NET 编译器的任务之一是为所有定义和引用的类型生成元数据描述。除了程序集中标准的元数据外。.NET 平台允许程序员

使用特性(Attribute)把更多的元数据嵌入到程序集中。一句话来说，特性就是用于类型（比如类、接口、结构等）、

成员（比如属性、方法等）、程序集或模块的代码注释。

#### 使用

当在代码中应用特性时，如果它们没有被另一个软件显示地反射，那么嵌入的元数据基本没什么作用。反之，嵌入程序集中

的元数据介绍将被忽略不计，而并无害处。

#### 编写自定义 Attribute

- 自定义 Attribute : 本身是一个类，必须继承自 Attribute

- 特性放到哪里？放到元素据中，不在 IL 里面（元数据读取使用反射）

- AttributeUsage : 用来修饰特性的特性，用来指示特性的功能

- AttributeTargets : 指定可以应用 Attribute 特性修饰的元素，是一个枚举类型。

注意事项：

- 必须以 Attribute 结尾

- 使用中可以省略。

- Attribute 使用的范围。

示例 1 : [自定义 Attribute 的使用](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/Attribute/myApp/Program.cs)，部分代码如下:

```cs
static void Main (string[] args)
{
    CommonSQLHelper sqlHelper = new CommonSQLHelper ();
    // sqlHelper.ConnString = ".."; // //使用这个有警告
    // sqlHelper.Update (""); // 添加true以后就不让使用了

    // 类特性的查找
    Teacher teacher = new Teacher ();
    object[] array = teacher.GetType ().GetCustomAttributes (typeof (MyCustomAttribute), true);
    foreach (var item in array)
    {
        MyCustomAttribute att = item as MyCustomAttribute;
        Console.WriteLine ("Id={0}  IsNotNull={1}  Comment={2}", att.Id, att.IsNotNull, att.Comment);
    }
    Console.WriteLine ("\r\n------------------------------------");

    // 获取属性的特性
    PropertyInfo property = teacher.GetType ().GetProperty ("TeacherName");
    object[] propertyAttArray = property.GetCustomAttributes (typeof (MyCustomAttribute), true);
    foreach (var item in propertyAttArray)
    {
        MyCustomAttribute attr = item as MyCustomAttribute;
        Console.WriteLine (attr.Comment);
    }

    Console.WriteLine ("\r\n------------------------------------");

    //获取数据表的别名
    Console.WriteLine ("数据表的别名：" + GetDBTableName (teacher));

    Console.WriteLine ("\r\n------------------------------------");
    //枚举特性
    Order order = new Order { Status = OrderStatus.AlreadyPaid };
    Console.WriteLine (order.Status.GetDescription ());

    TestDebug ("Only Run in Debug Environment");

    Console.Read ();
}
```

示例 2 : [自定义 ORM 框架](https://github.com/Damon-Salvatore/CustomORM)
