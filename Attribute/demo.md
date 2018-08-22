## 特性

新建一个控制台项目，名称叫做`AttributeDemo`

新建一个`UseCustomAttribute.cs`类文件，建这个类的目的是为了演示各种`Attribute`的效果，删除默认的`UseCustomAttribute`构造方法。

在`UseCustomAttribute.cs`文件中，添加一个`CommonSQLHelper`类,引入命名空间`using System.Data.SqlClient;`,下面建的几个特性类都是在`UseCustomAttribute.cs`文件中

```
//序列化 [Serializable]
public class CommonSQLHelper
{
  [Obsolete("该属性已过时，请使用新的属性：SqlConnString")]
  public string ConnString { get; set; }

  public string SqlConnString { get; set; }

  //添加ture以后不让使用了。
  [Obsolete("该方法已过时，请使用新的方法：public int Update(string sql,SqlParameter param)", true)]
  public int Update(string sql)
  {
    return -1;
  }

  public int Update(string sql, SqlParameter param)
  {
    return -1;
  }
}
```

继续在`UseCustomAttribute`文件中添加类，类名称叫`MyCustomAttribute.cs`，并且自定义类继承系统的`Attribute`。

```
/// <summary>
/// 特性：本身是一个类，必须继承自Attribute
/// 特性放到哪里？放到元数据中，不在IL里面（元数据读取使用反射）
/// 自定义特性的基本使用
/// AttributeUsageAttribute：用来修饰特性的特性
/// AttributeTargets
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property,
  AllowMultiple = true, Inherited = true)]
//  [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class MyCustomAttribute : Attribute
{
  public MyCustomAttribute() { }
  public MyCustomAttribute(int id)
  {
    this.Id = id;
  }
  public MyCustomAttribute(int id, bool isNotNull) : this(id)
  {
    this.IsNotNull = isNotNull;
  }
  public MyCustomAttribute(int id, bool isNotNull, string comment)
      : this(id, isNotNull)
  {
    this.Comment = comment;
  }

  public int Id { get; set; }
  public bool IsNotNull { get; set; }//是否为空
  public string Comment { get; set; }//备注

  //特性也可以添加方法
  public string GetInfo()
  {
    return $"Id={Id}  IsNotNull={IsNotNull } Comment={Comment}";
  }
}
```

继续添加类`YourCustomAttribute`,继承自我们自定义的特性类`MyCustomAttribute`,表明 **特性可以继承**

```
public class YourCustomAttribute : MyCustomAttribute
{
  public string CourseName { get; set; }
}
```

新建一个类`TableByNameAttribute.cs`表示别名特性,可以用来修饰类和属性

```
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class TableByNameAttribute : Attribute
{
  public TableByNameAttribute(string tableByName)
  {
    this.TableByName = tableByName;
  }

  /// <summary>
  /// 提供属性访问
  /// </summary>
  public string TableByName { get; }
}
```

新建一个类`EnumExtend.cs`,通过扩展方法给枚举增加扩展特性的获取方法,添加命名空间`using System.Reflection;`以及`using System.ComponentModel;`

```
public static class EnumExtend
{
  public static string GetDescription(this Enum e)
  {
    FieldInfo field = e.GetType().GetField(e.ToString());
    DescriptionAttribute att = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute));
    return att.Description;
  }
}
```

好了，自定义特性类完毕之后，我们新建一个单独的文件`Teacher.cs`,用来验证我们自定义的特性。

```
[MyCustom]
[MyCustom(Id = 1000)]
[MyCustom(Id = 1000, Comment = "实体对象的描述信息")]
[MyCustom(Id = 1000, IsNotNull = false, Comment = "实体对象的描述信息")]
[TableByName("Model_Teacher")]
public class Teacher
{
    public int TeacherId { get; set; }
    public string TeacherName { get; set; }
    public int Salary { get; set; }
}
```
