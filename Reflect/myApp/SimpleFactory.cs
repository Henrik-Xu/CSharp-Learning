using System;
using System.Configuration;
using System.Reflection;

namespace myApp
{
  public class SimpleFactory
  {
    private static string typeName = ConfigurationManager.AppSettings["TypeName"].ToString ();
    public static IQueryService GetEntity ()
    {
      return (IQueryService) Assembly.Load ("myApp").CreateInstance (typeName);
    }
  }
}
