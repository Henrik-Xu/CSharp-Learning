## Get 路由详解

首先，创建一个用于程序的实体类`Student.cs`,拥有如下属性:

```cs
public class Student
{
  public int StudentId { get; set; }
  public string StudentName { get; set; }
  public int Age { get; set; }
  public string PhoneNumber { get; set; }

}
```

新建一个视图,取名 `GetRequest.cshtml`,提供界面访问`api`数据的按钮，在`body`中写下如下布局：

```html
<div>
  <input type="button" id="btn1" value="Get：无参数的请求" /><br /><br />
  <input type="button" id="btn2" value="Get：2个参数的请求-->基础数据类型" /><br /><br />
  <input type="button" id="btn3" value="Get：实体作为参数" /><br /><br />
  <input type="button" id="btn4" value="Get：传递json字符串（包含多个参数）" /><br /><br />
  <input type="button" id="btn5" value="Get：没有写HttpGet特性-1" /><br /><br />
  <input type="button" id="btn6" value="Get：没有写HttpGet特性-2" /><br /><br />
  <input type="button" id="btn7" value="Get：返回一个对象集合" /><br /><br />
</div>
```

然后我们依次来实现`Get`的不同参数请求，默认引入`jQuery`库，新建`StudentController.cs`控制器文件，用来提供数据,并且在`StudentController`类中加上路由前置：

```cs
  [RoutePrefix("api/Student")]
  public class StudentController : ApiController
  {

  }
```

###### Get：无参数的请求

`StudentController`代码：

```cs
  /// <summary>
  /// Get请求：无参数
  /// </summary>
  /// <returns></returns>
  [Route("GetStudent")]
  [HttpGet]
  public int GetStudent()
  {
    return 1500;
  }
```

`GetRequest.cshtml`代码：

```cs
  //【1】Get：无参数的请求
  $("#btn1").click(function () {
    $.get("/api/Student/GetStudent", null, function (data, status) {
      alert(data);
    });
  });
```

###### Get：2 个参数的请求-->基础数据类型

`StudentController`代码：

```cs
  /// <summary>
  /// Get请求：两个参数-->基础数据类型
  /// </summary>
  /// <param name="stuId"></param>
  /// <param name="name"></param>
  /// <returns></returns>
  [Route("GetStudentInfo")]
  [HttpGet]
  public string GetStudentInfo(int stuId, string name)
  {
    return $"学员基本信息：{stuId}   {name}";
  }
```

`GetRequest.cshtml`代码：

```cs
 //【2】Get：2个参数的请求-->基础数据类型
  $("#btn2").click(function () {
    $.get("/api/Student/GetStudentInfo", { stuId: 1000, name: "vip高级进阶学员" },
      function (data, status) {
        alert(data);
      });
  });
```

###### Get：实体作为参数

`StudentController`代码：

```cs
  /// <summary>
  /// 【3】Get请求：实体作为参数
  /// </summary>
  /// <param name="student"></param>
  /// <returns></returns>
  //[Route("QueryStudent")]
  //[HttpGet]
  //public string QueryStudent(Student student)  //这种方法是无法接收实体参数的（无法映射）
  //{
  //  return $"查询到100个学员对象，其中一个学员信息：{student.StudentId}   {student.StudentName}";
  //}

  [Route("QueryStudent")]
  [HttpGet]
  public string QueryStudent([FromUri]Student student)
  {
    string name1 = System.Web.HttpContext.Current.Request.QueryString["StudentName"];
    string name2= System.Web.HttpContext.Current.Request.Params["StudentName"];  //只要通过 URL 的方式都可以取到数据

    return $"查询到100个学员对象，其中一个学员信息：{student.StudentId}   {student.StudentName}";
  }
```

`GetRequest.cshtml`代码：

```cs
//【3】Get：实体作为参数
  $("#btn3").click(function () {
    $.get("/api/Student/QueryStudent", { StudentId: 1000, StudentName: "vip高级进阶学员", Age: 20, PhoneNumber: "13002221000" },
      function (data, status) {
        alert(data);
      });
  });
```

###### Get：传递 json 字符串（包含多个参数）

`StudentController`代码：

```cs
/// <summary>
/// 【4】接收json字符串，并转化成实体对象
/// </summary>
/// <param name="jsonStudent"></param>
/// <returns></returns>
[Route("GetStudentByJson")]
[HttpGet]
public string GetStudentByJson(string jsonStudent)
{
  //将json字符串反序列化
  Student model = Newtonsoft.Json.JsonConvert.DeserializeObject<Student>(jsonStudent);

  return $"{model.StudentId}   {model.StudentName}  {model.PhoneNumber}";
}
```

`GetRequest.cshtml`代码：

```cs
  $("#btn4").click(function () {
  //包装数据
  var jsonStudent = { StudentId: 1000, StudentName: "vip高级进阶学员", Age: 20, PhoneNumber: "13002221000" }
  // var info = JSON.stringify(jsonStudent);
  // alert(info);

  $.get("/api/Student/GetStudentByJson", { jsonStudent: JSON.stringify(jsonStudent) },
    function (data, status) {
      alert(data);
    });
});
```

###### Get：没有写 HttpGet 特性-1

`StudentController`代码：

```cs
  /// <summary>
  /// 【5】Get请求中，如果没有添加 [HttpGet]特性，并且方法中没有Get关键字出现，请求是不能提交的
  /// </summary>
  /// <param name="studentId"></param>
  /// <returns></returns>
  [Route("QueryStudentScore")]
  public string QueryStudentScore(string studentId)
  {
    return $"学员：{studentId} 成绩是90";
  }
```

`GetRequest.cshtml`代码：

```cs
  $("#btn5").click(function () {
    $.get("/api/Student/QueryStudentScore", { studentId:"1000" },
      function (data, status) {
        alert(data);
      });
  });
```

###### Get：没有写 HttpGet 特性-2

`StudentController`代码：

```cs
  /// <summary>
  /// 【5】Get请求中，在省略 [HttpGet]特性的情况下，可以根据方法名称中的关键字识别Get请求
  ///
  /// 建议：还是认真的把Get请求类型写上
  /// </summary>
  /// <param name="studentId"></param>
  /// <returns></returns>
  [Route("GetStudentScore")]
  public string GetStudentScore(string studentId)
  {
    return $"学员：{studentId} 成绩是90";
  }
```

`GetRequest.cshtml`代码：

```cs
 $("#btn6").click(function () {
  $.get("/api/Student/GetStudentScore", { studentId: "1000" },
    function (data, status) {
      alert(data);
    });
});
```

###### 返回一个对象集合

`StudentController`代码：

```cs
  /// 【6】Geg请求：返回一个对象集合
  /// </summary>
  /// <param name="className"></param>
  /// <returns></returns>
  [Route("GetStudentList")]
  [HttpGet]
  public List<Student> GetStudentList(string className)
  {
    //实际开发中，可以在这里查询数据库...

    return new List<Student>
    {
      new Student { StudentName="andy",Age=20,PhoneNumber="13000000000" ,StudentId=1001},
      new Student { StudentName="tom",Age=21,PhoneNumber="13000000001" ,StudentId=1002},
      new Student { StudentName="kid",Age=22,PhoneNumber="13000000002" ,StudentId=1003},
    };
  }
```

`GetRequest.cshtml`代码：

```cs
//【6】Get：返回对象集合
  $("#btn7").click(function () {
    $.get("/api/Student/GetStudentList", { className: "软件1班" },
      function (data, status) {
        alert(data);
        //遍历对象集合
        jQuery.each(data, function (i, item) {
            alert(item.StudentName + "   " + item.Age);
        });
      });
  });
```
