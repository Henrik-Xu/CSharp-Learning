using System;

namespace myApp
{
    [MyCustom]
    [MyCustom (Id = 1000)]
    [MyCustom (Id = 1001, Comment = "实体对象的描述信息")]
    [MyCustom (Id = 1002, IsNotNull = false, Comment = "实体对象的描述信息")]
    [TableByName ("Model_Teacher")]
    public class Teacher
    {
        public int TeacherId { get; set; }

        [MyCustom (Id = 1000, IsNotNull = false, Comment = "Name:Damon Salvatore")]
        public string TeacherName { get; set; }

        public int Salary { get; set; }
    }
}
