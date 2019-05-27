## Post 路由详解

首先，创建一个用于程序的实体类`Score.cs`,拥有如下属性:

```cs
public class Score
{
  public int ScoreId { get; set; }
  public string StudentId { get; set; }
  public int CSharp { get; set; }
  public int DB { get; set; }
}
```

新建一个视图,取名 `PostRequest.cshtml`,提供界面访问`api`数据的按钮，在`body`中写下如下布局：

```html
<div>
  <input type="button" id="btn1" value="Post：1个参数的请求（请不要写key的值）" /><br /><br />
  <input type="button" id="btn2" value="Post：2个参数的请求-->基础数据类型" /><br /><br />
  <input type="button" id="btn3" value="Post：基于dynamic实现多个参数的请求" /><br /><br />
  <input type="button" id="btn4" value="Post：基于JSON传递实体对象-1" /><br /><br />

  <input type="button" id="btn6" value="Post：传递基础类型参数+实体参数" /><br /><br />
  <input type="button" id="btn7" value="Post：简单数组作为参数" /><br /><br />

  <input type="button" id="btn8" value="Post：对象集合作为参数" /><br /><br />
</div>
```

然后我们依次来实现`Post`的不同参数请求，默认引入`jQuery`库，新建`ScoreController.cs`控制器文件，用来提供数据,并且在`ScoreController`类中加上路由前置：

```cs
  [RoutePrefix("api/Score")]
  public class ScoreController : ApiController
  {

  }
```

###### Post：1 个参数的请求（请不要写 key 的值）

`ScoreController`代码：

```cs
  /// <summary>
  /// 【1】POST：一个参数的请求
  /// </summary>
  /// <param name="scoreId"></param>
  /// <returns></returns>
  [Route("GetScoreById")]
  [HttpPost]
  public string GetScoreById([FromBody]int scoreId)
  {
    return $"POST一个参数请求返回成绩信息：DB--90  C#--98";
  }
```

`PostRequest.cshtml`代码：

```cs
 //【1】Post：1个参数的请求，请不要给key赋值
  $("#btn1").click(function () {
    $.post("/api/Score/GetScoreById", { "": 3000 }, function (data, status) {
      alert(data);
    });
  });
```

###### Post：2 个参数的请求-->基础数据类型

`ScoreController`代码：

```cs
  /// <summary>
  /// 【2】POST：多个基础类型的参数传递（这种方式是无法实现的）
  ///
  /// 无法将多个参数(“scoreId”和“studentId”)绑定到请求的内容
  /// </summary>
  /// <param name="scoreId"></param>
  /// <param name="studentId"></param>
  /// <returns></returns>
  [Route("GetScoreByIdAndName")]
  [HttpPost]
  public string GetScoreByIdAndName([FromBody]int scoreId,[FromBody] int studentId)
  {
    return $"POST多个个参数请求返回成绩信息：DB--90  C#--98";
  }
```

`PostRequest.cshtml`代码：

```cs
  //【2】Post：多个参数的请求-->基础数据类型
  $("#btn2").click(function () {
    $.post("/api/Score/GetScoreByIdAndName", { scoreId: 1000, studentId: 2000 },
      function (data, status) {
        alert(data);
      });
  });
```

###### Post：基于 dynamic 实现多个参数的请求

`ScoreController`代码：

```cs
  /// <summary>
  /// 【3】POST：基于dynamic实现多个参数的请求
  /// </summary>
  /// <param name="param"></param>
  /// <returns></returns>
  [Route("GetScoreByDynamic")]
  [HttpPost]
  public string GetScoreByDynamic(dynamic param)
  {
    return $"基于dynamic实现多个参数的请求返回成绩信息：{param.StudentName}  {param.StudentId}";
  }
```

`PostRequest.cshtml`代码：

```cs
  //【3】Post：基于dynamic实现多个参数的请求
  $("#btn3").click(function () {
    $.ajax({
      type: "post",
      url: "/api/Score/GetScoreByDynamic",
      contentType: 'application/json',//注意json里面的key要和后面获取属性的大小写一致，否则获取不到
      data: JSON.stringify({ StudentId: 1000, StudentName: "vip高级学员", Age: 20 }),
      success: function (data, status) {
        alert(data);
      }
    });
  });
```

###### Post：基于 JSON 传递实体对象-1

`ScoreController`代码：

```cs
  /// <summary>
  /// 【4】基于JSON传递实体对象，前端可以直接传递普通json，后台也不用非得写FromBody
  /// </summary>
  /// <param name="student"></param>
  /// <returns></returns>
  [Route("QueryStudent")]
  [HttpPost]
  public string QueryStudent(Student student)
  {
    return $"基于JSON传递实体对象请求返回信息：{student.StudentName}";
  }
```

`PostRequest.cshtml`代码：

```cs
  //【4】Post：基于JSON传递实体对象-1
  $("#btn4").click(function () {
    $.post("/api/Score/QueryStudent", { StudentId: 1000, StudentName: "vip高级学员", Age: 20, PhoneNumber: "1370000000" },
      function (data, status) {
        alert(data);
      });
  });
```

###### Post：传递基础类型参数+实体参数

`ScoreController`代码：

```cs
  /// <summary>
  /// 【5】POST：基础数据类参数+实体参数一起传递
  ///  通用dynamic类型，接收复杂的JSON字符
  /// </summary>
  /// <returns></returns>
  [Route("MultiParam")]
  [HttpPost]
  public string MultiParam(dynamic param)
  {
    //方式1：通过key动态的获取对应的数据，并根据需要转换
    var teacher = param.Teacher.ToString();
    var course = Newtonsoft.Json.JsonConvert.DeserializeObject<Course>(param.Course.ToString());

    //方式2：对应动态类型中包括的子对象也可以通过jObject类型转换（下面的效果和上面是一样）
    Newtonsoft.Json.Linq.JObject jCourse = param.Course;
    var courseModel = jCourse.ToObject<Course>();//讲jsonObject转换成具体的实体对象

    return $"Teacher={teacher}  Course>Id={course.Id}  CourseName={course.Name}";
  }
```

`PostRequest.cshtml`代码：

```cs
  //【5】POST：基础数据类型参数+一个实体参数传递
  $("#btn6").click(function () {
    var data = {
      "Teacher": "常老师",
      "Course": {
        "Id": 248962,
        "Name": ".NET高级进阶VIP",
        "Category": "编程语言"
      }
    }
    $.ajax({
      type: "post",
      url: "/api/Score/MultiParam",
      contentType: 'application/json',//如果使用内容类型为json的时候，传递的数据要先序列化
      data: JSON.stringify(data),
      success: function (data, status) {
        alert(data);
      }
    });
  });
```

###### Post：简单数组作为参数

`ScoreController`代码：

```cs
  /// <summary>
  /// 【6】简单数据作为参数
  /// </summary>
  /// <param name="param"></param>
  /// <returns></returns>
  [Route("ArrayParam")]
  [HttpPost]
  public string ArrayParam(string[] param)
  {
    return $"{param[0]}  {param[1]}  {param[2]}";
  }
```

`PostRequest.cshtml`代码：

```cs
//【6】数组作为参数
  $("#btn7").click(function () {
    var score = ["90", "89", "60"];
    $.ajax({
      type: "post",
      url: "/api/Score/ArrayParam",
      contentType: 'application/json',//如果使用内容类型为json的时候，传递的数据要先序列化
      data: JSON.stringify(score),
      success: function (data, status) {
        alert(data);
      }
    });
  });
```

###### Post：对象集合作为参数

`ScoreController`代码：

```cs
  /// <summary>
  /// 【7】实体集合作为参数
  /// </summary>
  /// <param name="stuList"></param>
  /// <returns></returns>
  [Route("ListParam")]
  [HttpPost]
  public string ListParam(List<Student> stuList)
  {
    return stuList.Count.ToString();
  }
```

`PostRequest.cshtml`代码：

```cs
  //【7】对象集合作为参数
  $("#btn8").click(function () {
    var stuList = [
      { StudentId: "100", StudentName: "tom", Age: "20", PhoneNumber: "13600000000" },
      { StudentId: "101", StudentName: "kid", Age: "21", PhoneNumber: "13600000001" },
      { StudentId: "102", StudentName: "bub", Age: "22", PhoneNumber: "13600000002" },
    ];
    $.ajax({
      type: "post",
      url: "/api/Score/ListParam",
      contentType: 'application/json',//如果使用内容类型为json的时候，传递的数据要先序列化
      data: JSON.stringify(stuList),
      success: function (data, status) {
        alert(data);
      }
    });
  });
```
