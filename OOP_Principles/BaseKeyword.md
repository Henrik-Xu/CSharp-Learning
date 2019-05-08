### base 关键字的使用（base 关键字用来调用父类的构造方法）

#### `Animal.cs`

```cs
class Animal
{
  //父类构造函数
  public Animal() { }

  public Animal(string name, string color, string kind)
  {
    this.Color = color;
    this.Name = name;
    this.Kind = kind;
  }

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
}
```

#### `Cat.cs`

```cs
class Cat : Animal
{
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
}
```

#### `Dog.cs`

```cs
class Dog : Animal
{
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
}
```

#### `Program.cs`

```cs
class Program
{
  static void Main(string[] args)
  {
    //创建一只狗和一只猫
    Cat objCat = new Cat("球球儿", "黄色", "小花猫", "小鱼");
    Dog objDog = new Dog("棒棒", "黑色", "小黑狗", "排骨");
    Console.WriteLine("主人要求她们自我介绍后表演节目......");
    Console.WriteLine();
    objCat.Dancing();
    Console.WriteLine();
    objDog.Race();
    Console.ReadLine();
  }
}
```
