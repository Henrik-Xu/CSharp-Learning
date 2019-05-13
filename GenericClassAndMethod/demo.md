### 泛型

#### 泛型的好处

增加类型安全，编码灵活性提高。

常见泛型：泛型类、泛型方法;后续深入：泛型委托（自定义委托、常见系统泛型委托 Func、Action）。

泛型类的规范： public class 类名<T>{类的成员...};T: 仅仅是一个占位符，只要符合 C# 命名规范即可。

但是一般使用 T,表示一个通用的数据类型，在使用的时候用实际的类型代替。

如果包含任意多个类型的参数，参数之间用逗号分隔。GenericStack<T1，T2，T3>{...}

所定义的各种类型参数，可以用做成员变量的类型、属性、方法等返回值类型及方法参数...

#### 基于泛型类的出栈入栈

[基于泛型类的出栈入栈](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/GenericClassAndMethod/myApp/GenericStack.cs)

#### 泛型类中使用的几个关键点

- `default`关键字的使用

[default 关键字](https://docs.microsoft.com/zh-CN/dotnet/csharp/programming-guide/statements-expressions-operators/default-value-expressions)

泛型使用的两种错误:

```cs
public class GenericClass<T1,T2>
{
  private T1 obj1;

  public GenericClass()
  {
    // obj1 = null; // 1.不能赋值为 null
    // obj1= new T1(); // 2.不能人为假定某种类型，因为这种类型也许没有构造方法，也许是私有的

    // WorkAround :default 关键字。
    // 如果 T1 是引用类型，就赋值为 null,如果值类型就为默认值，如果是结构体，则成员的具体取决于成员的类型
    obj1 = default(T1);
  }
}
```

#### 添加约束类型的泛型类

[类型参数的约束](https://docs.microsoft.com/zh-CN/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters)

```cs
public class GenericClass<T1,T2,T3>
      where T1 : struct // 类型必须是结构类型
      where T2 : class  // 类型必须是引用类型
      where T3 : new () // 在这个类中，类型必须有一个无参数的构造方法，且必须把这个约束放到最后。
                        // 其他类型--> 基类类型 where T2:T1{} 表示 T2 必须与 T1 类型相同或者继承自 T1
                        // 接口类型： 类型必须是接口或者实现了接口
{
  // 产品列表
  public List<T2> ProductList {get; set; }

  // 产品发行者
  public List<T3> Publisher {get; set; }

  public GenericClass()
  {
    ProductList = new List<T2>();
    Publisher = new T3();
  }

  public T2 BuyProduct(T1 num)
  {
    // return ProductList[num]; 直接写是错误的
    dynamic index = num;
    return ProductList[index];
  }
}
```

#### 根据泛型类要求设计参数

```cs
class Course
{
  public string CourseName { get; set; }//课程名称
  public int Period { get; set; } //课程学习周期
}

class Teacher
{
  public Teacher() { }
  public string Name { get; set; }
  public int Count { get; set; }//授课数量
}
```

`Program.cs`

```cs
static void Main(string[] args)
{
  //【1】实例化泛型类型对象
  GenericClass<int, Course, Teacher> myClass = new GenericClass2<int, Course, Teacher>();

  //【2】给对象属性赋值
  myClass.Publisher = new Teacher { Name = "马云", Count = 20 };
  myClass.ProductList = new List<Course>()
  {
      new Course (){ CourseName="课程1", Period=1},
      new Course (){ CourseName="课程2", Period=2},
      new Course (){ CourseName="课程3", Period=3},
  };

  //【3】调用对象的方法
  Course myCourse = myClass.BuyProduct(0);
  string info = $"我购买的课程名称是：{myCourse.CourseName}  学期：{myCourse.Period} 个月  课程主讲：{myClass.Publisher.Name}";
  Console.WriteLine(info);
  Console.Read();
}
```

#### 泛型方法实现四则混合运算

[dynamic 关键字](https://docs.microsoft.com/zh-CN/dotnet/csharp/language-reference/keywords/dynamic)

```cs
static T Add1<T>(T a, T b)
{
  //  return a + b; //这种写法是错误的
  dynamic a1 = a;//动态类型仅在编译期间存在，运行期间会被object类型替代（编译的时候不考虑具体类型）
  dynamic b1 = b;
  return a1 + b1;
}

static T Add2<T>(T a, T b) where T : struct
{
  dynamic a1 = a;
  dynamic b1 = b;
  return a1 + b1;
}

static T Sub<T>(T a, T b) where T : struct
{
  dynamic a1 = a;
  dynamic b1 = b;
  return a1 - b1;
}

static T Multiply<T>(T a, T b) where T : struct
{
  dynamic a1 = a;
  dynamic b1 = b;
  return a1 * b1;
}
static T Div<T>(T a, T b) where T : struct
{
  dynamic a1 = a;
  dynamic b1 = b;
  return a1 / b1;
}
```

#### 泛型委托

首先，定义一个泛型委托

```cs
public delegate T MyGenericDelegate<T>(T a, T b);
```

然后，定义和委托签名相同的方法

```cs
static int Add(int a, int b)
{
    return a + b;
}
static double Sub(double a, double b)
{
    return a - b;
}
```

接着，泛型委托就跟普通委托的使用方法没有任何区别了

```cs
static void Main(string[] args)
{
  //使用委托
  MyGenericDelegate<int> mydelegate1 = Add;
  MyGenericDelegate<double> mydelegate2 = Sub;

  Console.WriteLine(mydelegate1(10, 20));
  Console.WriteLine(mydelegate2(10.5, 20.6));

  Console.Read();
}
```

有时候，委托的名字对我们没有实际意义，所以我们可以直接使用系统委托,帮助我们简化代码

- Func : 接收多个参数且最后一个参数为函数返回值的委托

[Func](https://msdn.microsoft.com/zh-cn/library/bb534960.aspx)

- Action : 接收多个参数而没有返回值的委托

[Action](https://msdn.microsoft.com/zh-cn/library/system.action.aspx)

```cs
Func<int,int,int> func1=Add;
Fun<double,double,double> func2=Sub;
```

如果方法名字对我们不是很重要，我们可以把方法名也去掉，使用匿名函数

[匿名函数](<https://msdn.microsoft.com/zh-cn/library/0yw3tz5k(v=vs.110).aspx>)

```cs
Func<int, int, int> func1 = delegate (int a, int b)
{
    return a + b;
};
Func<double, double, double> func2 = delegate (double a, double b)
{
  return a - b;
};
```

甚至，你可以使用 lambada 表达式来简化方法

[lambda](<https://msdn.microsoft.com/zh-cn/library/bb397687(v=vs.110).aspx>)

```cs
Func<int, int, int> func1 = (a, b) => a + b;
Func<double, double, double> func2 =(a,b)=> a - b;
```
