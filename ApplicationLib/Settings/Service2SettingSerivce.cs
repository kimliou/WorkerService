using ApplicationLib.SharedService.Interface;
using SharedSettingsLib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLib.Settings;
[InjectForBatchWorkerService2]
public class Service2SettingSerivce: IBatchSettings
{
    public Service2SettingSerivce(BatchSettings batchSettings)
  {
    BatchSettings = batchSettings;
  }
  BatchSettings BatchSettings { get; set; }
  public void GetSettingsFromCodeTable()
  {
    BatchSettings.SMTPEmailFrom = "SMTPEmailFrom2";//從DB或設定撈
    BatchSettings.SMTPEmailTo = "SMTPEmailTo2";//從DB或設定撈
    BatchSettings.SMTPSubject = "SMTPSubject2";//從DB或設定撈
    BatchSettings.SMTPBody = "SMTPBody2";//從DB或設定撈
    BatchSettings.IntrvSec = 20;
  }
}
