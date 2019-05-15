using System;
using System.Data.SqlClient;
using System.Reflection;
namespace myApp
{
    public class CommonSQLHelper
    {
        [Obsolete ("该属性已过时，请使用新的属性：SqlConnString")]
        public string ConnString { get; set; }

        public string SqlConnString { get; set; }

        [Obsolete ("该方法已过时，请使用新的方法：public int Update(string sql,SqlParameter param)", true)]
        public int Update (string sql)
        {
            return -1;
        }

        public int Update (string sql, SqlParameter param)
        {
            return -1;
        }
    }

    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class MyCustomAttribute : Attribute
    {
        public int Id { get; set; }

        public bool IsNotNull { get; set; }

        public string Comment { get; set; }
        public MyCustomAttribute () { }

        public MyCustomAttribute (int id)
        {
            this.Id = id;
        }

        public MyCustomAttribute (int id, bool isNotNull) : this (id)
        {
            this.IsNotNull = isNotNull;
        }

        public MyCustomAttribute (int id, bool isNotNull, string comment) : this (id, isNotNull)
        {
            this.Comment = comment;
        }
    }

    /// <summary>
    /// 特性可以继承
    /// /// </summary>
    public class YourCustomAttribute : Attribute
    {
        public string CourseName { get; set; }
    }

    /// <summary>
    /// 别名特性：可以用来修饰类或者属性
    /// /// </summary>
    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Property)]
    public class TableByNameAttribute : Attribute
    {
        /// <summary>
        /// 提供属性访问
        /// /// </summary>
        /// <value></value>
        public string TableName { get; }
        public TableByNameAttribute (string tableName)
        {
            this.TableName = tableName;
        }
    }

    /// <summary>
    /// 为枚举增加特性
    /// </summary>
    [AttributeUsage (AttributeTargets.Enum | AttributeTargets.Field)]
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; }

        public DescriptionAttribute (string description)
        {
            this.Description = description;
        }
    }

    /// <summary>
    /// 订单状态枚举
    /// 实际开发中，可以把描述内容，放到配置文件中
    /// /// </summary>
    public enum OrderStatus
    {
        [Description ("未付款")]
        UnPaid = 0,

        [Description ("已付款")]
        AlreadyPaid = 1,

        [Description ("延迟发货")]
        Delivery = 2,

        [Description ("已收货")]
        Confirm = 3
    }

    /// <summary>
    /// 通过扩展方法给枚举增加扩展特性的获取方法
    /// /// </summary>
    public static class EnumExtend
    {
        public static string GetDescription (this Enum e)
        {
            FieldInfo field = e.GetType ().GetField (e.ToString ());
            DescriptionAttribute att = (DescriptionAttribute) field.GetCustomAttribute (typeof (DescriptionAttribute));;
            return att.Description;
        }
    }
}
