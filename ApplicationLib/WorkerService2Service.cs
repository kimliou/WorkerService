using ApplicationLib.SharedService.Interface;
using SharedSettingsLib.Attributes;
using SharedSettingsLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLib;
[InjectForBatchWorkerService2]
public class WorkerService2Service : IBatchMainService
{
  public WorkerService2Service(BatchSettings batchSettings)
  {
    BatchSettings = batchSettings;
    Log = Serilog.Log.Logger;
  }
  BatchSettings BatchSettings { get; set; }
  Serilog.ILogger Log { get; }

  public void Start(bool throwException = false)
  {
    Log.Info($"{BatchSettings.ProcessName}:正常執行以下邏輯");
  }
}
