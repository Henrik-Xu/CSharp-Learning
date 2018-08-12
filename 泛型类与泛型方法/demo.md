## 热身（基于泛型类的出栈入栈）

### Step 1.新建一个`GenericStack.cs`类，写入以下代码

```
 /*
  泛型的好处：增加类型安全，编码灵活性提高
  常见泛型：泛型类、泛型方法
  后续深入：泛型委托（自定义委托、常见系统泛型委托Func、Action）
  泛型类的规范： pulic class 类名<T>{类的成员...}
  T: 仅仅是一个占位符，只要符合C#命名规范即可  但是一般使用T
  表示一个通用的数据类型，在使用的时候用实际的类型代替
  如果包含任意多个类型的参数，参数之间用逗号分隔。
  GenericStack<T1，T2，T3>{...}
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
