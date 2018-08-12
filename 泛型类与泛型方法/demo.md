## Demo 1.热身（基于泛型类的出栈入栈）

### Step 1.新建一个`GenericStack.cs`类，写入以下代码

```
 /*
  泛型的好处：增加类型安全，编码灵活性提高;
  常见泛型：泛型类、泛型方法;后续深入：泛型委托（自定义委托、常见系统泛型委托Func、Action）
  泛型类的规范： public class 类名<T>{类的成员...};T: 仅仅是一个占位符，只要符合C#命名规范即可。
  但是一般使用T,表示一个通用的数据类型，在使用的时候用实际的类型代替。
  如果包含任意多个类型的参数，参数之间用逗号分隔。GenericStack<T1，T2，T3>{...}
  所定义的各种类型参数，可以用做成员变量的类型、属性、方法等返回值类型及方法参数...
*/
 /// <summary>
 /// 泛型堆栈：入栈和出栈操作类（任意类型）
 /// </summary>
 /// <typeParam name="T">可以是任意类型</typeParam>
public class GenericStack<T>
{
  private T[] stackArray;//泛型数组
  private int currentPosition;//当前位置
  private int count;//栈的数据容量

  public GenericStack(int count)
  {
    this.count = count;
    this.stackArray = new T[count];//初始化数组大小
    this.currentPosition = 0;//当前位置默认值，索引从0开始
  }

  /// <summary>
  /// 入栈方法
  /// </summary>
  /// <param name="item"></param>
  public void Push(T item)
  {
    if (currentPosition >= count)
    {
        Console.WriteLine("栈空间已经满！");
    }
    else
    {
        this.stackArray[currentPosition] = item;//将当前元素压入栈
        currentPosition++;//调整位置索引值
    }
  }
  /// <summary>
  /// 出栈方法
  /// </summary>
  /// <returns></returns>
  public T Pop()
  {
    T data = this.stackArray[currentPosition - 1];
    currentPosition--;
    return data;
  }
}
```

### Step 2.在`Program.cs`文件中调用方法

```
static void Main(string[] args)
{
  //【1】创建泛型类对象
  GenericStack<int> stack1 = new GenericStack<int>(5);
  //【2】入栈
  stack1.Push(1);
  stack1.Push(2);
  stack1.Push(3);
  stack1.Push(4);
  stack1.Push(5);
  //【3】出栈
  Console.WriteLine(stack1.Pop());
  Console.WriteLine(stack1.Pop());
  Console.WriteLine(stack1.Pop());
  Console.WriteLine(stack1.Pop());
  Console.WriteLine(stack1.Pop());

  GenericStack<string> stack2 = new GenericStack<string>(5);

  stack2.Push("课程1");
  stack2.Push("课程2");
  stack2.Push("课程3");
  stack2.Push("课程4");
  stack2.Push("课程5");

  Console.WriteLine(stack2.Pop());
  Console.WriteLine(stack2.Pop());
  Console.WriteLine(stack2.Pop());
  Console.WriteLine(stack2.Pop());
  Console.WriteLine(stack2.Pop());

  Console.Read();
}
```

## Demo 2.(泛型类中使用的几个关键点)

### No 1. `default`关键字的使用([default 关键字](https://docs.microsoft.com/zh-CN/dotnet/csharp/programming-guide/statements-expressions-operators/default-value-expressions))

新建一个类`GenericClass1.cs`

```
public class GenericClass1<T1, T2>
{
  private T1 obj1;

  public GenericClass1()
  {
    //泛型使用的两种错误
    //obj1 = null;
    //obj1 = new T1(); //不能人为假定某种类型，因为这种类型也许没有构造方法，也许是私有的

    //解决方法：default关键字
    obj1 = default(T1); //如果T1是引用类型，就赋值null，如果是值类型就给默认值，
                        //如果是结构体，则成员的具体默认值取决于成员的类型
  }
}
```

### No 2. 添加约束类型的泛型类([类型参数的约束](https://docs.microsoft.com/zh-CN/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters))

新建一个类`GenericClass2.cs`

```
public class GenericClass2<T1, T2, T3>
    where T1 : struct //类型必须是结构类型
    where T2 : class  //类型必须是引用类型
    where T3 : new()  //在这个类中，类型必须有一个无参数的构造方法，且必须把这个约束放到最后。
                      // 其他类型-->基类类型  where T2：T1{}  表示T2必须与T1类型相同或者继承自T1
                      // 接口类型（类型必须是接口或者实现了接口）
{
  //产品列表
  public List<T2> ProductList { get; set; }

  //产品发行者
  public T3 Publisher { get; set; }


  public GenericClass2()
  {
      ProductList = new List<T2>();
      Publisher = new T3();
  }

  /// <summary>
  /// 购买第几个产品
  /// </summary>
  /// <param name="num"></param>
  /// <returns></returns>
  public T2 BuyProduct(T1 num)
  {
      //  return ProductList[num]; //直接写是错误的
      dynamic index = num;
      return ProductList[index];
  }
}
```

### No 3. 根据泛型类要求设计参数

```
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

然后在`Program.cs`中调用方法

```
static void Main(string[] args)
{
  //【1】实例化泛型类型对象
  GenericClass2<int, Course, Teacher> myClass2 = new GenericClass2<int, Course, Teacher>();

  //【2】给对象属性赋值
  myClass2.Publisher = new Teacher { Name = "马云", Count = 20 };
  myClass2.ProductList = new List<Course>()
  {
      new Course (){ CourseName="课程1", Period=1},
      new Course (){ CourseName="课程2", Period=2},
      new Course (){ CourseName="课程3", Period=3},
  };

  //【3】调用对象的方法
  Course myCourse = myClass2.BuyProduct(0);
  string info = $"我购买的课程名称是：{myCourse.CourseName}  学期：{myCourse.Period} 个月  课程主讲：{myClass2.Publisher.Name}";
  Console.WriteLine(info);
  Console.Read();
}
```

## Demo 3.泛型方法实现四则混合运算 ([dynamic 关键字](https://docs.microsoft.com/zh-CN/dotnet/csharp/language-reference/keywords/dynamic))

```
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

## Demo 4.泛型委托

首先，你需要定义一个泛型委托

```
public delegate T MyGenericDelegate<T>(T a, T b);
```

然后，定义和委托签名相同的方法

```
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

```
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

有时候，委托的名字对我们没有实际意义，所以我们可以直接使用系统委托,帮助我们简化代码，
比如上例可以直接使用系统的(Func)[https://msdn.microsoft.com/zh-cn/library/bb534960.aspx],接收多个参数且最后一个参数为函数返回值的委托。

```
Func<int,int,int> func1=Add;
Fun<double,double,double> func2=Sub;
```

如果方法名字对我们不是很重要，我们可以把方法名也去掉，使用(匿名函数)[https://msdn.microsoft.com/zh-cn/library/0yw3tz5k(v=vs.110).aspx]

```
Func<int, int, int> func1 = delegate (int a, int b)
{
    return a + b;
};
Func<double, double, double> func2 = delegate (double a, double b)
{
  return a - b;
};
```

甚至，你可以使用(lambda)[https://msdn.microsoft.com/zh-cn/library/bb397687(v=vs.110).aspx]表达式来简化方法

```
Func<int, int, int> func1 = (a, b) => a + b;
Func<double, double, double> func2 =(a,b)=> a - b;
```

最后，你还需要了解一下另一个系统委托(Action)[https://msdn.microsoft.com/zh-cn/library/system.action.aspx],接收多个参数而没有返回值的委托。
