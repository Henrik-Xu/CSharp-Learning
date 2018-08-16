## New 关键字的使用

### `Animal.cs`的代码如下

```
abstract class Animal
{
  #region
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
  #endregion
  //自我介绍
  public void Introduce()
  {
    string info = string.Format("我是漂亮的{0}，我的名字叫{1}，身穿{2}的衣服，我爱吃{3}！",
        Kind, Name, Color, Favorite);
    Console.WriteLine(info);
  }
  //虚方法
  public virtual void Have()
  {
    Console.WriteLine("我们要吃饭啦！");
  }
}
```

### `Cat.cs`的代码如下

```
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
  public override bool Equals(object obj)
  {
    Cat objCat = obj as Cat;
    if (objCat.Name == this.Name &&
        objCat.Kind == this.Kind &&
        objCat.Color == this.Color &&
        objCat.Favorite == this.Favorite)
        return true;
    else return false;
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
  //吃饭
  public override void Have()
  {
    base.Have();
    Console.WriteLine("我们要吃香喷喷的排骨啦！");
  }
  #endregion
  public new void Introduce()
  {
    string info = string.Format("Hi! I am {0}，My Name is {1}！",
        Kind, Name);
    Console.WriteLine(info);
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
    objCat.Introduce();
    Dog objDog = new Dog("棒棒", "黑色", "小黑狗", "排骨");
    objDog.Introduce();
    Console.ReadLine();
  }
}
```
