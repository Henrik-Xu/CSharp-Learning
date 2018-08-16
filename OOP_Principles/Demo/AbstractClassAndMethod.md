## 抽象类、抽象方法的定义与使用

### `Animal.cs`的代码如下

```
abstract class Animal
{
  //父类构造函数
  public Animal() { }
  public Animal(string name, string color, string kind)
  {
    this.Color = color;
    this.Name = name;
    this.Kind = kind;
  }
  #region
  public string Name { get; set; }//名字
  public string Color { get; set; }//颜色
  public string Kind { get; set; }//种类
  public string Favorite { get; set; }//喜好
  //自我介绍
  public void Introduce()
  {
    string info = string.Format("我是漂亮的{0}，我的名字叫{1}，身穿{2}的衣服，我爱吃{3}！",
        Kind, Name, Color, Favorite);
    Console.WriteLine(info);
  }
  #endregion
  //抽象方法
  public abstract void Have();
}
```

### `Cat.cs`的代码如下

```
class Cat : Animal
{
  #region
  public Cat(string name, string color, string kind, string favorite)
      : base(name, color, kind)
  {
    this.Favorite = favorite;
  }
  //跳舞
  public void Dancing()
  {
    base.Introduce();
    Console.WriteLine("下面我给大家表演《小猫迪斯科》，请大家鼓掌啊：>");
  }
  #endregion
  //吃饭
  public override void Have()
  {
    Console.WriteLine("我们要吃香喷喷的烤鱼啦！");
  }
}
```

### `Dog.cs`的代码如下

```
class Dog : Animal
{
  #region
  public Dog(string name, string color, string kind, string favorite)
      : base(name, color, kind)
  {
    this.Favorite = favorite;
  }
  //赛跑
  public void Race()
  {
    base.Introduce();
    Console.WriteLine("下面我给大家表演《狗狗精彩百米跨栏》，请大家鼓掌啊：>");
  }
  #endregion
  //吃饭
  public override void Have()
  {
    Console.WriteLine("我们要吃香喷喷的排骨啦！");
  }
}
```

### `Program.cs`的代码如下

```
class Program
{
  static void Main(string[] args)
  {
    //创建一只狗和一只猫
    Cat objCat = new Cat("球球儿", "黄色", "小花猫", "小鱼");
    Dog objDog = new Dog("棒棒", "黑色", "小黑狗", "排骨");
    //将子类对象添加的父类集合
    List<Animal> list = new List<Animal>();
    list.Add(objCat);
    list.Add(objDog);
    //取出子类对象
    foreach (Animal obj in list)
    {
        obj.Have();
    }
    Console.ReadLine();
  }
}
```
