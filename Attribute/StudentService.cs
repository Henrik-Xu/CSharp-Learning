using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ORM.Models;
using ORM.MainService;
using System.Data;
using System.Data.SqlClient;

namespace ORM.DAL
{
    public class StudentService
    {
        #region  添加学员对象

        /// <summary>
        /// 添加学员对象
        /// </summary>
        /// <param name="objStudent"></param>
        /// <returns></returns>
        public int AddStudent(Students student)
        {
            #region 以前的写法

            ////【1】编写SQL语句         
            //StringBuilder sqlBuilder = new StringBuilder();
            //sqlBuilder.Append("insert into Students(studentName,Gender,Birthday,");
            //sqlBuilder.Append("StudentIdNo,Age,PhoneNumber,StudentAddress,CardNo,ClassId,StuImage)");
            //sqlBuilder.Append(" values('{0}','{1}','{2}',{3},{4},'{5}','{6}','{7}',{8},'{9}');select @@Identity");
            ////【2】解析对象
            //string sql = string.Format(sqlBuilder.ToString(), student.StudentName,
            //         student.Gender, student.Birthday.ToString("yyyy-MM-dd"),
            //        student.StudentIdNo, null,
            //        student.PhoneNumber, student.StudentAddress, student.CardNo,
            //        student.ClassId, student.StuImage);
            //try
            //{           
            //   return Convert.ToInt32(SQLHelper.GetSingleResult(sql)); //【3】执行SQL语句，返回结果
            //}
            //catch (SqlException ex)
            //{
            //    throw new Exception("数据库操作出现异常！具体信息：\r\n" + ex.Message);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            #endregion

            //基于ORM框架的调用

            //调用格式化的SQL语句测试
            //  return DBService.SaveByCommonSql(student, true);
            //自动生成带参数的SQL语句测试
            return DBService.SaveByParamSql(student, true);

        }

        #endregion

        #region 修改学员信息

        /// <summary>
        /// 修改学员对象
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public int ModifyStudent(Students student)
        {
            ////【1】编写SQL语句
            //StringBuilder sqlBuilder = new StringBuilder();
            //sqlBuilder.Append("update Students set studentName='{0}',Gender='{1}',Birthday='{2}',");
            //sqlBuilder.Append(
            //    "StudentIdNo={3},Age={4},PhoneNumber='{5}',StudentAddress='{6}',CardNo='{7}',ClassId={8},StuImage='{9}'");
            //sqlBuilder.Append(" where StudentId={10}");
            ////【2】解析对象
            //string sql = string.Format(sqlBuilder.ToString(), student.StudentName,
            //         student.Gender, student.Birthday.ToString("yyyy-MM-dd"),
            //        student.StudentIdNo, student.Age,
            //        student.PhoneNumber, student.StudentAddress, student.CardNo,
            //        student.ClassId, student.StuImage, student.StudentId);
            try
            {
                //  return Convert.ToInt32(SQLHelper.Update(sql));    //【3】执行SQL语句，返回结果 
                return DBService.UpdateByParamSql(student);
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库操作出现异常！具体信息：\r\n" + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 删除学员对象

        /// <summary>
        /// 删除学员对象
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public int DeleteStudent(Students student)
        {
            try
            {
                return DBService.DeleteByParamSql(student);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    throw new Exception("该学号被其他实体引用，不能直接删除该学员对象！");
                else
                    throw new Exception("数据库操作出现异常！具体信息：\r\n" + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 查询学员信息

        /// <summary>
        /// 根据班级查询学员信息
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public List<Students> GetStudentByClass(string className)
        {
            #region 以前的写法

            //string sql = "select StudentId,StudentName,Gender,PhoneNumber,StudentIdNo,Birthday,ClassName from Students";
            //sql += " inner join StudentClass on Students.ClassId=StudentClass.ClassId";
            //sql += " where ClassName='{0}'";
            //sql = string.Format(sql, className);
            //SqlDataReader reader = SQLHelper.GetReader(sql);
            //List<StudentExt> list = new List<StudentExt>();
            //while (reader.Read())
            //{
            //    list.Add(new StudentExt()
            //    {
            //        StudentId = Convert.ToInt32(objReader["StudentId"]),
            //        StudentName = objReader["StudentName"].ToString(),
            //        Gender = objReader["Gender"].ToString(),
            //        PhoneNumber = objReader["PhoneNumber"].ToString(),
            //        Birthday = Convert.ToDateTime(objReader["Birthday"]),
            //        StudentIdNo = objReader["StudentIdNo"].ToString(),
            //        ClassName = objReader["ClassName"].ToString()
            //    });
            //}
            //objReader.Close();
            //return list;

            #endregion

            //组合SQL语句
            string sql = "select StudentId,StudentName,Gender,PhoneNumber,StudentIdNo,Birthday,ClassName from Students";
            sql += " inner join StudentClass on Students.ClassId=StudentClass.ClassId";
            sql += $" where ClassName='{className}'";
            //执行查询
            SqlDataReader reader = HelperFactory.SQLHelper.GetReader(sql);
            return DBService.GetEntitiesFromReader<Students>(reader);
        }

        #endregion
    }
}
