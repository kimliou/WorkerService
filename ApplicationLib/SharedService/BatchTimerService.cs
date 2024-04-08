using ApplicationLib.SharedService.Interface;
using Microsoft.Extensions.Hosting;
using SharedSettingsLib.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLib.SharedService;

public class BatchTimerService : IHostedService, IDisposable
{
  public BatchTimerService(
    IBatchMainService batchMainService,
    ISettingService settingService,
    BatchSettings batchSettings
  )
  {
    MainService = batchMainService;
    BatchSettings = batchSettings;
    SettingService = settingService;
    Log = Serilog.Log.Logger;
  }
  IBatchMainService MainService { get; }
  ISettingService SettingService { get; }
  BatchSettings BatchSettings { get; }
  public Serilog.ILogger Log { get; }

  private Timer? Timer = null;
  private int executionCount = 0;

  public Task StartAsync(CancellationToken cancellationToken)
  {
    SettingService.SetSettingsFromDB();
    if (Debugger.IsAttached)
    {
      DoWork(state: null);
    }
    else
    {
      Timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(BatchSettings.IntrvSec));
    }
    return Task.CompletedTask;
  }

  private void DoWork(object? state)
  {
    Interlocked.Increment(ref executionCount);
    if (executionCount == 1)
    {
      MainService.Start();
      Log.Info($"記億體使用: {Process.GetCurrentProcess().WorkingSet64:0,0} bytes");

    }
    Interlocked.Decrement(ref executionCount);
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    Log.Info($"{nameof(StopAsync)}: Batch Stoping ...");
    Timer?.Change(Timeout.Infinite, 0);
    return Task.CompletedTask;
  }

  public void Dispose()
  {
    Log.Info($"{nameof(StopAsync)}: Batch Stoped ...");
    Timer?.Dispose();
  }
}
