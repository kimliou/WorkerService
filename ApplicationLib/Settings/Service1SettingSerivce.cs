using ApplicationLib.SharedService.Interface;
using SharedSettingsLib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLib.Settings;
[InjectForBatchWorkerService1]
public class Service1SettingSerivce: IBatchSettings
{
    public Service1SettingSerivce(BatchSettings batchSettings)
  {
    BatchSettings = batchSettings;
  }
  BatchSettings BatchSettings { get; set; }
  public void GetSettingsFromCodeTable()
  {
    BatchSettings.SMTPEmailFrom = "SMTPEmailFrom1";//從DB或設定撈
    BatchSettings.SMTPEmailTo = "SMTPEmailTo1";//從DB或設定撈
    BatchSettings.SMTPSubject = "SMTPSubject1";//從DB或設定撈
    BatchSettings.SMTPBody = "SMTPBody1";//從DB或設定撈
    BatchSettings.IntrvSec = 10;
  }
}
