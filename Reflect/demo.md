## 反射

### 在.NET中，反射（reflection）是一个运行库类型发现的过程。使用反射服务，可以通过编程使用一个友好的对象模型得到与通过`ildasm.exe`显示的相同的元数据信息。

新建一个控制台应用程序，取名`Demo`,在这里告诉读者项目的名字的原因是后面反射需要用到的命名空间和项目名称有关。

先别着急，我们先来做一些准备工作。

新建一个接口类`IQueryService.cs`

```
public interface IQueryService
{
    string GetEntityTypeById();
}
```

新建一个文件夹，名字叫做`School`，包含四个类，用于后面示例的操作类
`Student.cs`:

```
/// <summary>
/// 学生类
/// </summary>
public class Student : IQueryService
{
  public Student() { Console.WriteLine("Student()：无参数构造方法被调用！"); }
  public Student(int stuId)
  {
    this.StudentId = stuId;
    Console.WriteLine("Student(int stuId)：1个参数构造方法被调用！");
  }
  public Student(int stuId,string stuName)
  {
    this.StudentId = stuId;
    this.StudentName = stuName;
    Console.WriteLine("Student(int stuId,string stuName)：2个参数构造方法被调用！");
  }
  public int StudentId { get; set; }
  public string StudentName { get; set; }

  public string GetEntityTypeById()
  {
    //根据ID从数据库中查询学员的类型...

    return "我正在学习反射！！！";
  }
}
```

`President.cs`:

```
 /// <summary>
/// 校长类：一个学校只有一个校长【单例模式】
/// </summary>
public class President
{
  private static President instance = null;
  private President()
  {
    Console.WriteLine("单例模式中，President私有构造方法被调用！");
  }
  public static President GetPresident()
  {
    if (instance == null)
    {
        instance = new President();
    }
    return instance;
  }
  public void SayHello()
  {
    Console.WriteLine("各位老师同学们，大家好！我是新任校长，工作中请大家多支持！");
  }
}
```

`Instructor.cs`:

```
/// <summary>
/// 辅导员泛型类：1个泛型参数
/// </summary>
public class Instructor1<T1>
{
  public void SayHello(T1 t1)
  {
    Console.WriteLine($"GenericInstructor1<T1 >： public void SayHello(T1 t1) 方法被调用！");
    Console.WriteLine($"T1的类型={t1.GetType().Name}");
  }
}
/// <summary>
/// 辅导员泛型类：2个泛型参数
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
public class Instructor2<T1, T2>
{
  public void SayHello(T1 t1, T2 t2)
  {
    Console.WriteLine($"GenericInstructor2<T1, T2>： public void SayHello(T1 t1,T2 t2) 方法被调用！");
    Console.WriteLine($"T1的类型={t1.GetType().Name}  T2的类型={t2.GetType().Name} ");
    Console.WriteLine($"大家好！我是{t1}{t2}");
  }
}
/// <summary>
/// 辅导员泛型类：3个泛型参数
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
public class Instructor3<T1, T2, T3>
{
  public void SayHello(T1 t1, T2 t2, T3 t3)
  {
    Console.WriteLine($"GenericInstructor3<T1, T2, T3>： public void SayHello(T1 t1,T2 t2,T3 t3) 方法被调用！");
    Console.WriteLine($"T1的类型={t1.GetType().Name}  T2的类型={t2.GetType().Name}  T3的类型={t3.GetType().Name}");
  }
}
```

`Teacher.cs`:

```
/// <summary>
/// 教师类
/// </summary>
public class Teacher
{
  public int TeacherId { get; set; }
  public string TeacherName { get; set; }
  public string Department { get; set; }//所在系

  public static readonly string Company = "CSharp_Learning"; //所在机构

  //所讲课程
  public Dictionary<int, string> Courses { get; } = new Dictionary<int, string>()
  {
    [1] = ".Net高级编程",
    [2] = "ASP.NET网站开发",
    [3] = "Web前端技术"
  };

  public Teacher()
  {
    TeacherId = 1001;
    TeacherName = "Mr.Chang";
    Department = "计算机系";
  }
  public void SayHello()
  {
    Console.WriteLine($"SayHello()   大家好！我是{Department}的课程讲师：{TeacherName}！");
  }
  /// <summary>
  /// 授课方法：2次重载
  /// </summary>
  /// <param name="courseId"></param>
  public void Teach(int courseId)
  {
    Console.WriteLine($"Teach(int courseId)   今天我们讲授：{Courses[courseId]}");
  }
  public void Teach(int courseId, string chapter)
  {
    Console.WriteLine($"Teach(int courseId, string chapter)   今天我们讲授：{Courses[courseId]}中的{chapter}！");
  }
  public void Teach(int courseId, string chapter, string content)
  {
    Console.WriteLine($"Teach(int courseId, string chapter, string content)   今天我们讲授：{Courses[courseId]}中{chapter}关于{content}的内容！");
  }

  /// <summary>
  /// 领工资（私有方法）
  /// </summary>
  private void PrivateGetSalary()
  {
    Console.WriteLine(" private  void GetSalary()：今天我自己去领工资！");
  }
  /// <summary>
  /// 做演讲（静态方法）
  /// </summary>
  /// <param name="title"></param>
  public static void Lecture(string title)
  {
    Console.WriteLine($"public static void Lecture(string title) ：今天我演讲的题目是：{title}");
  }
  /// <summary>
  /// 泛型方法
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <typeparam name="C"></typeparam>
  /// <param name="time">授课时间</param>
  /// <param name="count">上课次数</param>
  public void TeachAdvancedCourse<T, C>(T time, C count)
  {
    Console.WriteLine($"public void TeachAdvancedCourse<T, C>(T time, C count)：我们每周{count}次课 晚上{time}");
  }

  /// <summary>
  /// 反射和普通方法调用测试
  /// </summary>
  public void Test()
  {

  }
}
```

好了，准备工作做好了，让我们来一步步编写反射的例子，相信你肯定能获益匪浅的。

打开`Program.cs`文件,注意，我们首先在 Main 函数结尾加一行`Console.Read()`代码，为了让我们更清楚看控制台的结果。

### 程序依据需求的不同，耦合度不断的变化，开发者应该对这一些列变化深入掌握，才能更好的把握.Net 原理

### 【1】普通对象的创建：性能最好，最容易理解，耦合度最高

```
static void Main(string[] args)
{
  Console.WriteLine("\r\n【1】普通对象的创建----------------------------------");
  Student student1 = new Student();
  Console.WriteLine(student1.GetEntityTypeById());
}
```

### 【2】基于接口的对象创建：接口更好的体现面向抽象编程，一定程度解耦

```
static void Main(string[] args)
{
  Console.WriteLine("\r\n【2】基于接口的对象创建-----------------------------");
  IQueryService student2 = new Student();
  Console.WriteLine(student2.GetEntityTypeById(1));
}
```

### 【3】反射的基本使用,引入命名空间`using System.Reflection;`
[Load](https://docs.microsoft.com/zh-cn/dotnet/api/system.reflection.assembly.load?view=netframework-4.7.2) 
[LoadFrom](https://docs.microsoft.com/zh-cn/dotnet/api/system.reflection.assembly.loadfrom?view=netframework-4.7.2) 
[LoadFile](https://docs.microsoft.com/zh-cn/dotnet/api/system.reflection.assembly.loadfile?view=netframework-4.7.2) 

```
static void Main(string[] args)
{
  Console.WriteLine("\r\n【3】反射的基本使用----------------------------------");
  Assembly ass1 = Assembly.Load("Demo");//在当前运行目录下根据程序集名称加载

  string path = System.IO.Directory.GetCurrentDirectory() + "\\Demo.exe";
  Assembly ass2 = Assembly.LoadFile(path);//使用完整的路径加载程序集文件
  Assembly ass3 = Assembly.LoadFrom("Demo.exe");//根据程序集文件名称，加载当前运行目录下的程序集

  //观察程序集对象给我们提供的信息
  foreach (var item in ass1.GetTypes())//Type表示当前程序集中所能找到的可用类型
  {
    Console.WriteLine(item.Name + " \t" + item);
  }
}
```

### 【4】基于反射的对象创建

晚期绑定（late binding)是一种创建一个给定类型的实例并在运行时调用其成员，而不需要在编译时知道它存在的一种技术。[System.Activator](https://docs.microsoft.com/en-us/dotnet/api/system.activator.createinstance?view=netframework-4.7.2#System_Activator_CreateInstance_System_Type_)类是.NET晚期绑定过程中的关键所在。

```
static void Main(string[] args)
{
  Console.WriteLine("\r\n【4】基于反射的对象创建-----------------------------");

  Assembly ass1 = Assembly.Load("Demo");
  //根据一个类或接口的完全限定名，得到这个类型，
  //这个类型可以是我目前使用的各种类型：类类型、接口类型、数组类型、值类型、枚举类型、类型参数、泛型类型定义，以及开放或封闭构造的泛型类型。
  Type studentType = ass1.GetType("Demo.Student");

  Student student3 = (Student)Activator.CreateInstance(studentType);//Activator并非反射中的类
  Student student4 = (Student)Assembly.Load("Demo").CreateInstance("Demo.Student");
  IQueryService student5 = (Student)Assembly.Load("Demo").CreateInstance("Demo.Student");
  Console.WriteLine(student3.GetEntityTypeById(1));
  Console.WriteLine(student4.GetEntityTypeById(2));
  Console.WriteLine(student5.GetEntityTypeById(3));
}
```

### 【5】反射的基本使用（对象的延迟创建：简单工厂，抽象工厂...）

新建一个`SimpleFactory.cs`文件,需要引用`System.Configuration`程序集,并且在项目中添加(`using System.Configuration;`)进来，同时还要添加`System.Reflection;`命名空间

```
public class SimpleFactory
{
  private static string typeName = ConfigurationManager.AppSettings["TypeName"].ToString();

  public static IQueryService GetEntity()
  {
      return (IQueryService)Assembly.Load("Demo").CreateInstance(typeName);
  }
}
```

同时，上面的代码读取了`App.config`中的配置，所以要在`configuration`节点下添加如下配置

```
<appSettings>
  <add key="TypeName" value="Demo.Student"/>
</appSettings>
```

接着，`Program.cs`中

```
static void Main(string[] args)
{
  Console.WriteLine("\r\n【5】反射在简单工厂中的使用-------------------------");
  IQueryService student6 = SimpleFactory.GetEntity();
  Console.WriteLine(student6.GetEntityTypeById(1));
}
```

### 【6】在反射中使用构造方法

```
static void Main(string[] args)
{
  Console.WriteLine("\r\n【6】在反射中使用构造方法-----------------------");
  Type studentType = Assembly.Load("Demo").GetType("Demo.Student");

  //获取程序集中指定类型的构造方法
  ConstructorInfo[] ctors = studentType.GetConstructors();
  Console.WriteLine("当前类型中构造函数的总数：" + ctors.Length);
  //显示所有的构造方法在IL中的名称（在IL中都是.ctor,可以通过ILDASM工具查看）
  foreach (ConstructorInfo item in ctors)
  {
      Console.WriteLine(item.Name);
  }

  //通过构造方法创建对象（请用断点调试下面的三条语句）
  object stuObj1 = Activator.CreateInstance(studentType);//调用无参数的构造方法
  object stuObj2 = Activator.CreateInstance(studentType, new object[] { 1001 });//调用一个参数的构造方法
  object stuObj3 = Activator.CreateInstance(studentType, new object[] { 1001, "小李子" });//调用两个参数的构造方法
}
```

### 【7】单例模式中私有构造方法调用

```
static void Main(string[] args)
{
  Console.WriteLine("\r\n【7】单利模式中私有构造方法调用--------------");
  President newPresident = President.GetPresident();
  newPresident.SayHello();

  Type presidentType = Assembly.Load("Demo").GetType("Demo.President");
  object president = Activator.CreateInstance(presidentType, true);//**true表示可以匹配私有构造方法（请断点调试观察）**
  Console.Read();
}
```

### 【8】泛型类中使用反射创建对象
如果我们调用Type.GetType()来获取泛型类型的元数据描述，就必须使用包含`“反引号”（`）`加上数字值得语法来表示类型支持的类型参数个数。
```
System.Collections.Generic.List<T> -->System.Collections.Generic.List`1
System.Collections.Generic.Dictionary<TKey,TValue> -->System.Collections.Generic.Dictionary`2
```
```
static void Main(string[] args)
{
  Console.WriteLine("\r\n【8】泛型类中使用反射创建对象--------------");

  //泛型类在IL中和普通类名称是不一样的，请大家通过ILDASM查看，比如Instructor2`2 后面的“ `2 ” 如果是三个泛型参数，则后面是3
  //也可以通过控制台，直接查看输出，比如【3】中的输出
  Type genericInstructorType = Assembly.Load("Demo").GetType("Demo.Instructor2`2");
  Type commonType = genericInstructorType.MakeGenericType(new Type[] { typeof(string), typeof(string) });
  object objInstructor = Activator.CreateInstance(commonType);
  ((Instructor2<string, string>)objInstructor).SayHello("计算机系", "辅导员");
}
```

### 【9】基于反射调用实例公有方法、私有方法、静态方法

```
static void Main(string[] args)
{
  Console.WriteLine("\r\n【9】基于反射调用实例公有方法、私有方法、静态方法--------------");

  Type teacherType = Assembly.Load("Demo").GetType("Demo.Teacher");
  object teacher = Activator.CreateInstance(teacherType);

  //以下输出内容为：属性对应的方法、自定义的各种方法、通过继承获得的方法（特别注意，这里全部是公有方法！）
  foreach (MethodInfo item in teacherType.GetMethods())
  {
      Console.WriteLine(item.Name);
  }

  Console.WriteLine("\r\n-----------------------------------------------------");
  //通过方法名获取方法
  MethodInfo method = teacherType.GetMethod("SayHello");
  method.Invoke(teacher, null);//**调用无参数方法使用null**

  MethodInfo method1 = teacherType.GetMethod("Teach", new Type[] { typeof(int) });
  method1.Invoke(teacher, new object[] { 1 });//**调用1个参数的方法**

  MethodInfo method2 = teacherType.GetMethod("Teach", new Type[] { typeof(int), typeof(string) });
  method2.Invoke(teacher, new object[] { 1, "第3章" });//**调用2个参数的方法**

  MethodInfo method3 = teacherType.GetMethod("Teach", new Type[] { typeof(int), typeof(string), typeof(string) });
  method3.Invoke(teacher, new object[] { 1, "第3章", ".Net反射技术" });//**调用3个参数的方法**

  Console.WriteLine("\r\n-----------------------------------------------------");
  MethodInfo method4 = teacherType.GetMethod("PrivateGetSalary", BindingFlags.Instance | BindingFlags.NonPublic);
  method4.Invoke(teacher, null);//**调用私有方法**

  Console.WriteLine("\r\n-----------------------------------------------------");
  MethodInfo method5 = teacherType.GetMethod("Lecture", new Type[] { typeof(string) });
  method5.Invoke(teacher, new object[] { ".Net反射的原理和应用" });//**调用静态方法**
  method5.Invoke(null, new object[] { ".Net反射的原理和应用" });//**调用静态方法第一个实例可以为null,实例方法调用则不能省略**

  Console.WriteLine("\r\n-----------------------------------------------------");
  MethodInfo method6 = teacherType.GetMethod("TeachAdvancedCourse");
  MethodInfo genericMethod6 = method6.MakeGenericMethod(typeof(string), typeof(int));
  genericMethod6.Invoke(teacher, new object[] { "8:00-10:00", 2 });//**调用泛型方法**
}
```

### 【10】基于反射调用字段和属性

```
static void Main(string[] args)
{
  Console.WriteLine("\r\n【10】基于反射调用公有属性和字段-------------------");

  Type teacherType = Assembly.Load("Demo").GetType("Demo.Teacher");
  object teacher = Activator.CreateInstance(teacherType);

  //显示全部属性
  foreach (PropertyInfo item in teacherType.GetProperties())
  {
    Console.WriteLine(item.Name);
  }
  Console.WriteLine("\r\n--------------------------------------------------");

  //显示全部字段
  foreach (FieldInfo item in teacherType.GetFields())
  {
    Console.WriteLine(item.Name);
  }
  Console.WriteLine("\r\n--------------------------------------------------");

  //**给属性赋值**
  PropertyInfo property = teacherType.GetProperty("Department");
  property.SetValue(teacher, ".Net教学组");
  Console.WriteLine(property.GetValue(teacher));

  //**给字段赋值**
  FieldInfo field = teacherType.GetField("Company");
  field.SetValue(teacher, "腾讯课堂");
  Console.WriteLine(field.GetValue(teacher));

  //典型应用：查看自定有ORM中Select方法的封装...
}
```

### 【11】关于反射的性能测试和优化,引入命名空间`using System.Diagnostics;`

```
static void Main(string[] args)
{
  Console.WriteLine("\r\n【11】关于反射的性能测试和优化-------------------");

  long time1 = 0;
  long time2 = 0;
  long time3 = 0;

  //普通方法
  Stopwatch sw1 = new Stopwatch();
  sw1.Start();
  for (int i = 0; i < 1000000; i++)
  {
      Teacher myTeacher = new Teacher();
      myTeacher.Test();
  }
  sw1.Stop();
  time1 = sw1.ElapsedMilliseconds;

  //反射方法（大概是普通方法的500多倍）
  Stopwatch sw2 = new Stopwatch();
  sw2.Start();
  for (int i = 0; i < 1000000; i++)
  {
      Teacher myTeacher = (Teacher)Assembly.Load("Demo").CreateInstance("Demo.Teacher");
      myTeacher.Test();
  }
  sw2.Stop();
  time2 = sw2.ElapsedMilliseconds;

  //优化反射方法（和普通方法相差无几）
  Stopwatch sw3 = new Stopwatch();
  sw3.Start();
  Type tType = Assembly.Load("Demo").GetType("Demo.Teacher");
  for (int i = 0; i < 1000000; i++)
  {
      Teacher myTeacher = (Teacher)Activator.CreateInstance(tType);
      myTeacher.Test();
  }
  sw3.Stop();
  time3 = sw3.ElapsedMilliseconds;

  Console.WriteLine($"普通方法：{time1}\t反射方法：{time2}  优化反射方法：{time3}");
}
```
