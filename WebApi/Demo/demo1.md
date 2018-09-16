### WebApi 的基本使用

1.新建一个`ASP.NET Web`应用程序，取名`WebApiDemo1`,选择如下：

![](https://github.com/Damon-Salvatore/CSharp-Learning/blob/master/WebApi/Demo/imgs/1.png)

2.新建一个`MVC`的控制器， 取名`HomeController`,在`Index.cshtml`中编写我们的测试用例，注意，需要自己添加`JQuery`

```
<!DOCTYPE html>
<html>
<head>
  <meta name="viewport" content="width=device-width" />
  <title>WebApi2的学习</title>
  <script src="~/Scripts/jquery-1.10.2.min.js"></script>
  <script type="text/javascript">
    $(function () {
      $("#btn2").click(function () {
        $.post("/Course/UpdateCourse", null, function (data, status) {
          alert(data);
        });
      });
      $("#btn3").click(function () {
        $.post("/Course/DeleteCourse", { "": 7 }, function (data, status) {
          alert(data);
        });
      });
      $("#btn4").click(function () {
        $.post("/Course/AddCourse", { Id: 8, Name: "WebAPI2", Category: 2, Price: 80 },
          function (data, status) {
            alert(data);
          });
      });
      $("#btn5").click(function () {
        $.get("/api/teacher", null,
          function (data, status) {
            alert(data);
          });
      });
      $("#btn6").click(function () {
        $.post("/api/teacher/QueryTeacherById", { "": 1000 },
          function (data, status) {
            alert(data);
          });
      });
      $("#btn7").click(function () {
        $.get("/api/teacher/GetCount", { age: "50" },
          function (data, status) {
            alert(data);
          });
      });
      $("#btn8").click(function () {
        $.post("/api/teacher/GetTeacherName", { "": 2000 },
          function (data, status) {
            alert(data);
          });
      });
    });
    </script>
</head>
<body>
  <div>
    <a href="/api/Course">【1】简单测试WebAPI2</a><br /><br />
    <a href="/api/Course/100">【2】简单测试WebAPI2</a><br /><br />
    <a href="/api/Course?courseId=3000">【3】简单测试WebAPI2</a><br /><br />
    <a href="/customApi/Course/GetCourseByName">【4】自定义路由（和MVC类似，增加action）</a><br /><br />
    <a href="/Course/QueryCourse?courseId=5">【5】测试特性路由-Get</a><br /><br />

    <input type="button" id="btn2" value="【6】测试特性路由-Post-无参数" /><br /><br />
    <input type="button" id="btn3" value="【7】测试特性路由-Post-一个参数" /><br /><br />
    <input type="button" id="btn4" value="【8】测试特性路由-Post-多个参数（实体对象）" /><br /><br />

    <input type="button" id="btn5" value="【9】路由前缀-1" /><br /><br />
    <input type="button" id="btn6" value="【10】路由前缀-2" /><br /><br />

    <input type="button" id="btn7" value="【11】路由约束-1" /><br /><br />
    <input type="button" id="btn8" value="【12】路由约束-2" /><br /><br />
  </div>
</body>
</html>
```

3.新建一个`WebApi`的控制器，取名`CourseController`,主要是基本的`Get`请求和`Post`请求。同时，也引出了几个问题：

a.为什么要使用 `WebAPI` 的特性路由？

实际开发中`http` 请求的方法可能是相同的（比如都是`Get` 或 `post` 请求），而且请求的参数也相同。这个问题就麻烦。因为遵照 `Restful` 风格无法解决。

大家可能想到自定义一个路由，比如增加 `action`，可以，但是在 `webAPI` 里面是不提倡的,这就是特性路由的由来。

b.`Post` 请求的规范

- 无参数

  无参数的 `Post` 请求，和 `Get` 方式请求相同。只不过在客户端`$.get` 和`$.post` 区别。同时只需要添加`[httpPost]`标记特性即可。

- 有参数
  一个参数的 `POST` 请求,和 `Get` 方式不一样，动作方法参数上面必须添加`[FromBody]`标记，否则访问不到！同时 `WebAPI` 请求传递的参数，也有特定的格式。而这种格式并不是我们常见的 `key-value` 格式。`WebAPI`模型绑定器寻找的时候，并不是按照 `key`去查找。而是空字符串。

```
/// <summary>
/// 基础APIA练习类
/// </summary>
public class CourseController : ApiController
{
  [HttpGet]
  public string GetCourse()
  {
    //在这个地方可以请求数据库或其他资源...

    return "GetCourse()方法返回的结果：WebAPI2开发技术";
  }

  [HttpGet]
  public string GetCourseById(int courseId)
  {
    return $"GetCourseById(int courseId)方法返回结果：WebAPI2学习--对应的课程编号：{courseId}";
  }

  public string GetCourseByName()
  {
    return $"GetCourseByName()方法返回结果!";
  }

  //特性路由1
  [Route("Course/QueryCourse")]
  [HttpGet]
  public string QueryCourse(int courseId)
  {
    //在这里可以添加其他的操作...

    return "Get请求到的课程ID=" + courseId;
  }

  //特性路由2
  [Route("Course/UpdateCourse")]
  [HttpPost]
  public string UpdateCourse()
  {
    //在这里可以添加其他的操作...

    return "您正在修改课程！";
  }
  //特性路由3
  [Route("Course/DeleteCourse")]
  [HttpPost]
  public string DeleteCourse([FromBody]int courseId)
  {
    //在这里可以添加其他的操作...

    return "Post请求到的课程ID=" + courseId;
  }

  //特性路由
  [Route("Course/AddCourse")]
  [HttpPost]
  public string AddCourse([FromBody]Models.Course course)
  {
    //在这里可以添加其他的操作...

    return $"Post请求到的课程ID={course.Id} Name={course.Name} Category={course.Category} Price={course.Price}";
  }
}
```

4.特性路由在动作方法中统一使用时，我们可以给整个控制器加路由前缀，通过这样的方式减少代码量,并且可以添加路由约束来规范参数的传递。

新建一个`WebApi`的控制器，取名`TeacherController`，代码如下：

```
/// <summary>
/// 路由前缀
/// </summary>
[RoutePrefix("api/Teacher")]
public class TeacherController : ApiController
{
  [Route("")]
  [HttpGet]
  public int GetAllTeacherCount()
  {
    return 9;
  }

  [Route("QueryTeacherById")]
  [HttpPost]
  public string QueryTeaherById([FromBody] int teacherId)
  {
    return "马老师的讲师编号：" + teacherId;
  }
  //路由约束1
  [Route("GetCount/{age:int=0}")]
  [HttpGet]
  public string GetCount(int age)
  {
    return $"查询讲师年龄等于{age}共计10人";
  }
  //路由约束1
  [Route("GetTeacherName/{id:int=0}")]
  [HttpPost]
  public string GetTeacherName([FromBody]int id)
  {
    return "当前授课老师的讲师编号是：" + id;
  }
}
```
