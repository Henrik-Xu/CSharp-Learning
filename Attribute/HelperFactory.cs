using System.Data.SqlClient;
using System.Data.OleDb;

namespace ORM.MainService
{
   public class HelperFactory
    {
        //Access数据库
        public static IBaseDALHelper<OleDbDataReader, OleDbParameter> OldDbHelper = new ImplBaseHelper<OleDbConnection, OleDbCommand, OleDbDataReader, OleDbParameter>();

        //SqlServer数据库
        public static IBaseDALHelper<SqlDataReader, SqlParameter> SQLHelper = new ImplBaseHelper<SqlConnection, SqlCommand, SqlDataReader, SqlParameter>();
    }
}
