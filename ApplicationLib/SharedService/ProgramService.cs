using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLib.SharedService;

public class ProgramService
{
  public static IConfigurationRoot NewConfigurationBuilder()
  {
    return new ConfigurationBuilder()
      .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true)
      .AddEnvironmentVariables()
      .Build();
  }

  /// <summary>
  /// 自動註冊命名空間裡, 有指定 Attribute 的服務類別, 
  /// 指定多個命名空間以","逗點分隔 
  /// </summary>
  /// <param name="services"></param>
  /// <param name="namespaces"> 多個命名空間字串, 以逗點","分隔 </param>
  /// <param name="attributes"> 需要註冊的類別 Attribute </param>
  public static void RegisterTypesAsSingleton(
    IServiceCollection services, string namespaces, params Attribute[] attributes
  )
  {
    RegisterTypes(services, namespaces, asTransient: false, attributes);
  }
  public static void RegisterTypesAsTransient(
    IServiceCollection services, string namespaces, params Attribute[] attributes
  )
  {
    RegisterTypes(services, namespaces, asTransient: true, attributes);
  }
  public static void RegisterTypes(
    IServiceCollection services, string namespaces, bool asTransient = false, params Attribute[] attributes
  )
  {
    foreach (var attribute in attributes)
    {
      try
      {
        namespaces.Trim().Split(',').ToList().Where(get => !string.IsNullOrEmpty(get)).ToList().ForEach(_namespace =>
        {
          Assembly.Load(_namespace).GetTypes()
            .Where(type1 => !type1.IsInterface)
            .Where(type2 => type2.GetCustomAttributes().Contains(attribute))
            .ToList().ForEach(implementationType =>
            {
              var serviceType =
              Assembly.Load(_namespace).GetTypes().FirstOrDefault(
                _interfaceType => _interfaceType.Name.Equals(implementationType.GetInterfaces().FirstOrDefault()?.Name, StringComparison.Ordinal))
              ?? implementationType;
              if (asTransient)
              {
                services.AddTransient(serviceType, implementationType);
              }
              else
              {
                services.AddSingleton(serviceType, implementationType);
              }
            });
        });
      }
      catch (Exception ex)
      {
        Log.Fatal($"{nameof(Exception)}: {ex.Message}");
      }
    }
  }

}
