using Serilog;
using SharedSettingsLib.Attributes;

var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
Environment.SetEnvironmentVariable("APP_BASE_DIRECTORY", System.AppDomain.CurrentDomain.BaseDirectory);

var appsettings = new ConfigurationBuilder()
  .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
  .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
  .AddEnvironmentVariables()
  .Build();

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Debug()
  .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
  .Enrich.FromLogContext()
  //.Enrich.With(new SWIP_SharedSettingsLib.LogForce.SerilogThreadIDEnricher())
  .ReadFrom.Configuration(appsettings)
  .CreateLogger();
var log = Log.Logger;
var FileName = appsettings.GetValue<string>("FileName") ?? "N/A";
log.Information($"{nameof(FileName)}: {FileName}");
log.Information($"System.AppDomain.CurrentDomain.BaseDirectory: {System.AppDomain.CurrentDomain.BaseDirectory}");
var namespaces = $"{nameof(SharedSettingsLib)},{nameof(ApplicationLib)}";
System.Attribute[] attributes = { new InjectForBatchWorkerService1() };

IHost host =
Host.CreateDefaultBuilder(args)
.UseWindowsService(options =>
{
  options.ServiceName = "WorkerService1";
})
.ConfigureServices(services =>
{
  // initial entity framework database connection string //
  //{
  //  var connectionString = SharedSettingsLib.AppSettings.DefaultConnectionString(appsettings);
  //  //SECRET//log.Information($"{nameof(connectionString)}: {connectionString}");
  //  services.AddDbContext<DatabaseLib.EFModels.EFDBContext>(dbContextOptionsBuilder =>
  //  {
  //    dbContextOptionsBuilder.UseSqlServer(connectionString);
  //    dbContextOptionsBuilder.ConfigureWarnings(warnings =>
  //      warnings.Ignore(SqlServerEventId.DecimalTypeKeyWarning)
  //    // ^^ 20230309, Kim: 忽略程式啟動時的 Entity Framework 警告 warn: Microsoft.EntityFrameworkCore.Model.Validation[30003] The decimal property 'SEQ_NO' is part of a key on entity type 'SYS_CD' ...
  //    );
  //  },
  //ServiceLifetime.Transient, ServiceLifetime.Singleton);
  //}
  // register service //
  {
    //var connectionString = SharedSettingsLib.AppSettings.DefaultConnectionString(appsettings);
    services.AddHttpClient(Microsoft.Extensions.Options.Options.DefaultName)
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
      ClientCertificateOptions = ClientCertificateOption.Manual,
      ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
      {
        return true;
      }
    }); // ^ HttpClient Ignore SSL Certificate
    services.AddSingleton<Serilog.ILogger>(Log.Logger);
    //services.AddTransient<SqlConnection>(_ => { return new SqlConnection(connectionString); });
    services.AddHttpClient();
    //services.AddTransient<IDapperContext, DapperContext>();
    ApplicationLib.SharedService.ProgramService.RegisterTypesAsSingleton(services, namespaces, attributes);
  }
  services.AddHostedService<ApplicationLib.SharedService.BatchTimerService>();
})
.Build();

await host.RunAsync();
