using ApplicationLib.SharedService.Interface;
using Serilog;
using SharedSettingsLib.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLib.SharedService;
[InjectForBatchWorkerService1]
[InjectForBatchWorkerService2]
public class SettingService : ISettingService
{
  public SettingService(BatchSettings batchSettings)
  {
    BatchSettings = batchSettings;
    Log = Serilog.Log.Logger;
  }
   BatchSettings BatchSettings { get; set; }
  Serilog.ILogger Log { get; }
  public void SetSettingsFromDB()
  {
    try
    {
      BatchSettings.ServiceName = Assembly.GetEntryAssembly()!.GetName().Name!;
      BatchSettings.ProcessName = Process.GetCurrentProcess().ProcessName;
      BatchSettings.ProcessName = Process.GetCurrentProcess().ProcessName;
      {
        //共用參數寫入        
        BatchSettings.SMTPServer = "SMTPServer";//從DB或設定撈
        BatchSettings.SMTP_Pass = "SMTP_Pass";//從DB或設定撈
        BatchSettings.SMTPPort = "SMTPPort";//從DB或設定撈
        BatchSettings.SMTP_ID = "SMTP_ID";//從DB或設定撈
        BatchSettings.EnableSsl = true;//從DB或設定撈
      }
    }
    catch (Exception ex)
    {
      Log.Error($"{nameof(SettingService)}:{ex.Message}{ex.InnerException}");
      throw;
    }
  }
}
