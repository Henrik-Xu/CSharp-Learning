using System;
namespace myApp.School
{
  public class Student : IQueryService
  {
    public int StudentId { get; set; }

    public string StudentName { get; set; }

    public Student()
    {
      Console.WriteLine("Student(): 无参数构造方法被调用");
    }

    public Student(int studentId)
    {
      this.StudentId = studentId;
      Console.WriteLine("Student(studentId): 1个参数构造方法被调用");
    }

    public Student(int studentId, string studentName) : this(studentId)
    {
      this.StudentName = studentName;
      System.Console.WriteLine("Student(studentId,studentName): 2个参数构造方法被调用");
    }
    public string GetEntityTypeById(int id)
    {
      //根据ID从数据库中查询学员的类型...

      return ".NET高级学员";
    }
  }
}
