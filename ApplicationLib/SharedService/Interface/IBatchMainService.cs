using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLib.SharedService.Interface;

public interface IBatchMainService
{
  void Start(bool throwException = false);
}
