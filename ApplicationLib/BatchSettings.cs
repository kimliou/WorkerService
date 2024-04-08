using SharedSettingsLib.Attributes;
using SharedSettingsLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLib;
[InjectForBatchWorkerService1]
[InjectForBatchWorkerService2]
public class BatchSettings: SMTPSetting
{
  public string ServiceName { get; set; } = "";
  public string ProcessName { get; set; } = "";
  public int IntrvSec { get; set; }
}
