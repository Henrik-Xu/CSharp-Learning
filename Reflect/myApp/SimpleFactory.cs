using System;
using System.Reflection;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace myApp
{
  public class SimpleFactory
  {
    private static string typeName = ConfigurationManager.AppSettings["TypeName"].ToString();
    public static IQueryService GetEntity()
    {
      return (IQueryService)Assembly.Load("ReflectionDemo").CreateInstance(typeName);
    }
  }
}
