### 委托、匿名方法和 Lambda 表达式

#### 1. 使用 var 推断类型

```cs
var a = 20;

var bookName = ".NET开发";
var student1 = new Student() { StudentName = "Damon Salvatore", Age = 22 };
Console.WriteLine($"【1】var关键字的使用：共有 {a} 个人在学习{bookName} 系列课程，其中{student1.StudentName} ,{student1.Age}岁,已经是高手了。");

//Var在匿名类中的使用（后面学习EF的时候，会大量使用）
var person = new
{
  Name = "Damon",
  Age = 25,
  ClassName = "计算机一班"
};
Console.WriteLine("【4】var在匿名类中的使用-->姓名：{0} 年龄：{1} 班级：{2}", person.Name, person.Age, person.ClassName);
```

#### 2. 委托基础

委托的基本概念：委托其实就是一个方法的原型：返回值 + 参数类型和个数

委托：方法的指针（方法的代表）

委托创建的基本步骤 :

1. 声明委托

2. 根据委托定义具体的方法

3. 创建委托对象（关联具体的方法）

4. 通过委托调用方法，而不是直接使用方法

```cs
// 1. 声明委托
public delegate void CalculatorDelegate(int a, int b);

// 2. 根据委托定义具体的方法
static void Add(int a, int b)
{
  Console.WriteLine("通过委托调用的方法：a+b={0}", a + b);
}

//根据委托定义一个“具体方法”实现减法功能
static void Sub(int a, int b)
{
  Console.WriteLine("通过委托调用的方法：a-b={0}", a - b);
}
static void Mul(int a, int b)
{
  Console.WriteLine("通过委托调用的方法：a*b={0}", a * b);
}
static void Div(int a, int b)
{
  Console.WriteLine("通过委托调用的方法：a/b={0}", a / b);
}

// 3. 创建委托对象（关联具体的方法）
CalculatorDelegate delCal = new CalculatorDelegate(Add);

// 4. 通过委托调用方法，而不是直接使用方法
delCal(50, 20);

// 5. 多路委托（多播委托）
Console.WriteLine("------------------------------------------");
delCal += Sub; //
delCal(50, 20);

Console.WriteLine("------------------------------------------");
delCal += Mul;
delCal(50, 20);

Console.WriteLine("------------------------------------------");
delCal -= Add;
delCal -= Sub;
delCal(50, 20);

```

#### 3. 匿名方法

定义 ： 方法没有具体的名称，只有委托关键字，方法的参数，方法体，

使用 ： 匿名方法允许委托变量作为参数传递，以代替单独方法的定义

```cs
CalculatorDelegate delCal2 = delegate (int num1, int num2)
{
  Console.WriteLine("通过匿名方法计算：a+b={0}", num1 + num2);
};
delCal2(100, 50);
```

#### 4. Lambda 表达式

- C#3.0 时代，引入了 Lambda 表达式，利用他可以更加简练的编写代码块

- Lambda 表达式中的参数类型可以是“明确类型”，也可以是“推断类型”

- 如果是推断类型，则参数的数据类型是编译器根据上下文自动推断出来

Lambda 表达式与匿名方法比较：

- lambda 表达式本身就是匿名方法

- Lambda 表达式参数允许不明确声明，而匿名方法参数类型必须明确指出

- Lambda 表达式中方法体，允许单一表达式或者多条语句组成，而匿名方法不允许单一表达式

```cs
CalculatorDelegate delCal3 = (int num1, int num2) => { Console.WriteLine("通过Lambda表达计算：a+b={0}", num1 + num2); };
delCal3(100, 50);

/* Lambda表达定义防方式：（参数列表）=>{方法体}  =>读作goes to */
MathDelegate math1 = (int n) => { return n * n; };
MathDelegate math2 = n => n * n; ; //如果参数只有一个可以省略（） 如果方法体只有一行代码可以省略{}

CalculatorDelegate delCal4 = (p, q) =>
{
  var r1 = p + q;
  int r2 = p - q;
  Console.WriteLine("p+q={0}", r1);
  Console.WriteLine("p-q={0}", r2);
};
Console.WriteLine(math1(10));
Console.WriteLine(math2(20));
delCal4(200, 100);
}
```

#### 5. 自定义泛型委托和系统委托 Action/Func

```cs
public delegate void MyGenericDelegate<T1, T2>(T1 a, T2 b);

public delegate T1 MyGenericDelegate2<T1, T2>(T1 a, T2 b);

static void GenericMethod(int a, int b)
{
  int result = a + b;
  Console.WriteLine("泛型委托调用的 GenericMethod 方法：" + result);
}

Console.WriteLine("\r\n--------------自定义泛型委托和系统委托------------------");

MyGenericDelegate<int, int> genericDelegate = GenericMethod;

MyGenericDelegate<int, int> genericDelegate2 = delegate (int r1, int r2)
{
  int result = r1 + r2;
  Console.WriteLine("泛型委托genericDelegate2调用的方法：" + result);
};

MyGenericDelegate2<int, int> genericDelegate3 = (r1, r2) =>
{
  Console.WriteLine("genericDelegate3是带有返回值的泛型委托变量");
  return r1 + r2;
};

//系统也给我们提供了两个泛型委托，分别对应有返回值和无返回值
Action<int, int, int> myAction = (a1, a2, a3) =>
{
  int result = (a1 + a2) * a3;
  Console.WriteLine($"Action 调用结果 ${result}");
};
//Action泛型委托供给16个，也就是说参数最多可以是16个。

//Func泛型委托
Func<int, int, string> myFunc = (a1, a2) =>
{
  int result = a1 + a2;
  return "Func委托调用返回结果：" + result;
};
genericDelegate(10, 20);
genericDelegate2(10, 20);
Console.WriteLine(genericDelegate3(10, 20));

myAction(1, 2, 3);
Console.WriteLine(myFunc(1, 2));
```

#### 6. 协变和逆变

.Net 4 之前，泛型接口是不能变化，.Net 4 通过协变和逆变（抗变）为泛型接口、泛型委托添加一个重要的扩展。

协变接口允许其方法的返回值相对于指定的参数为更具体的派生类型。

逆变接口允许其方法参数相对于指定的参数为更抽象的类型。

- 协变和逆变是针对《参数》和《返回值》的类型进行自动转换

- out（协变）：放到方法的《返回值》前面，记忆：out 表示出

- in（逆变）：放到方法《参数》前面 in 表示入

注意事项 ：

- 只有引用类型才支持使用泛型接口中的变体。 值类型不支持变体。

- 实现变体接口的类仍是固定类。

```cs
// The following line generates a compiler error
// because classes are invariant.
// List<Object> list = new List<String>();

// You can use the interface object instead.
IEnumerable<Object> listObjects = new List<String>();
```

```cs
Person person1 = new Student(); //依据里氏替换原则
// Student stu =new Person();//不允许直接赋值的

List<Person> pList1 = new List<Person>();
// List<Person> pList2 = new List<Student>();

//如果某个返回值的类型可以由其派生类型替换，那么这个类型就是支持协变的。
//List实现了 IEnumerable接口，这个接口是支持协变的（可以通过F12查看元数据）
IEnumerable<Person> pList3 = new List<Student>();

//逆变：如果某个参数的类型可以由其派生类型替换，那么这个类型就是支持逆变的
IPerson<Student> pList4 = new Teacher<Student>();
IPerson<Student> pList5 = new Teacher<Person>(); // 将子类 Student 用父类 Person 替换
```

#### 7. 扩展方法

扩展方法简单总结：

- 不带参数的扩展方法定义: static 方法名 (this 目标类型 目标类型参数)

- 带参数的扩展方法定义: static 方法名 (this 目标类型 目标类型参数，参数类型 1，参数名 2，… )

- this 关键字 : 表示可以向 this 关键字后面的类型添加扩展方法。

扩展方法使用中应该注意的问题：

- 扩展方法必须定义在静态类中，扩展方法本身也是静态方法，扩展方法也可以重载。

- 如果扩展方法和对应的类位于不同的命名空间，使用时应引入扩展方法所在静态类的命名空间。

- 当类本身的方法与扩展方法重名时，类本身的方法被优先调用。（建议通过命名空间的方式来实现扩展方法的使用）

- 扩展方法不要过多使用。尤其是系统定义的类，不要随意增加扩展方法。

```cs
public static class CustomExtendMethod
{
    /// <summary>
    /// 这个方法是对int类型增加的扩展方法
    /// </summary>
    /// <param name="sum"></param>
    /// <returns></returns>
    public static string ShowScore (this int sum)
    {
        return "本次考试总成绩：" + sum;
    }
    public static void ShowList (this string[] nameList)
    {
        foreach (var item in nameList)
        {
            Console.WriteLine (item);
        }
    }

    public static string SayHello (this Student student)
    {
        return "欢迎您：" + student.StudentName;
    }
    public static string GetAvg (this Student student, int csharp, int database)
    {
        int avg = (csharp + database) / 2;
        return string.Format ($"欢迎您：{student.StudentName} !  您两门的平均成绩为：{avg}");
    }
    public static IEnumerable<TSource> MyCustomWhere<TSource> (this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        //。。。
        List<TSource> list = new List<TSource> ();
        //遍历查找
        foreach (var item in source)
        {
            if (predicate (item))
            {
                list.Add (item);
            }
        }
        return list;
    }
}

 static void Main(string[] args)
 {
    Console.WriteLine("------------------扩展方法的更多应用---------------------");
      List<Student> stuList = new List<Student>
      {
          new Student { StudentId = 1001, Age = 20, StudentName = "VIP学员001" },
          new Student { StudentId = 1005, Age = 22, StudentName = "VIP学员002" },
          new Student { StudentId = 1007, Age = 28, StudentName = "VIP学员003" },
          new Student { StudentId = 1004, Age = 25, StudentName = "VIP学员004" },
          new Student { StudentId = 1003, Age = 29, StudentName = "VIP学员005" }
      };

    //使用扩展方法，轻松完成对象查找
    var list = stuList.Where(s => s.Age > 25);

    Func<Student, bool> func = s => s.Age > 25;
    var list2 = stuList.Where(func);

    var list3 = stuList.Where(s => s.Age > 25).Select(s => s);
    var list4 = stuList.Where(s => s.Age > 25).Select(s => s.StudentName);

    foreach (var item in list)
    {
      Console.WriteLine($"{item.StudentId}\t{item.StudentName}\t{item.Age}");
    }

    Console.WriteLine("---------------------------------------------------------------");

    foreach (var item in list2)
    {
      Console.WriteLine($"{item.StudentId}\t{item.StudentName}\t{item.Age}");
    }

    Console.WriteLine("---------------------------------------------------------------");

    foreach (var item in list3)
    {
      Console.WriteLine($"{item.StudentId}\t{item.StudentName}\t{item.Age}");
    }

    Console.WriteLine("---------------------------------------------------------------");

    foreach (var item in list4)
    {
      Console.WriteLine(item);
    }

    Console.WriteLine("---------------------------------------------------------------");

    var list5 = stuList.MyCustomWhere(s => s.Age < 25);
    foreach (var item in list5)
    {
      Console.WriteLine($"{item.StudentId}\t{item.StudentName}\t{item.Age}");
    }
 }
```
