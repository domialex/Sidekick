using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sidekick.Core.Extensions
{
  public static class TypeExtensions
  {
    public static TAttribute GetAttribute<TAttribute>(this Type type)
    {
      var attributeType = typeof(TAttribute);
      return (TAttribute)type.GetCustomAttributes(false).Where(x => attributeType.IsAssignableFrom(x.GetType())).FirstOrDefault();
    }

    public static IList<TAttribute> GetAttributes<TAttribute>(this Type type)
    {
      var attributeType = typeof(TAttribute);
      return type.GetCustomAttributes(false)
        .Where(x => attributeType.IsAssignableFrom(x.GetType()))
        .Select(x => (TAttribute)x)
        .ToList();
    }

    public static List<Type> GetImplementedInterface(this Type @interface)
    {
      return FindTypes(x => x.GetInterfaces().Contains(@interface));
    }

    public static List<Type> GetImplementedAttribute(this Type attribute)
    {
      return FindTypes(x => x.GetCustomAttributes(false).Any(y => attribute.IsAssignableFrom(y.GetType())));
    }

    private static List<Type> FindTypes(Func<Type, bool> func)
    {
      var results = new List<Type>();
      var executedAssemblies = new List<string>();
      FindTypes(ref results, ref executedAssemblies, func, AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetName()).ToArray());
      return results;
    }

    private static void FindTypes(ref List<Type> results, ref List<string> executedAssemblies, Func<Type, bool> func, AssemblyName[] assemblies)
    {
      foreach (var assemblyName in assemblies.Where(x => !x.FullName.StartsWith("Microsoft") && !x.FullName.StartsWith("System")))
      {
        if (executedAssemblies.Contains(assemblyName.FullName))
        {
          continue;
        }

        executedAssemblies.Add(assemblyName.FullName);

        try
        {
          var assembly = Assembly.Load(assemblyName);

          foreach (var type in assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract && !x.IsInterface).Where(func))
          {
            if (!results.Any(x => x.FullName == type.FullName))
            {
              results.Add(type);
            }
          }

          FindTypes(ref results, ref executedAssemblies, func, assembly.GetReferencedAssemblies());
        }
        catch (Exception) { }
      }
    }
  }
}
