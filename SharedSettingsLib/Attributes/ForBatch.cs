using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Attributes;

/// <summary>
/// WorkerService1 專用的 Singleton Injection _ 
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)] public class InjectForBatchWorkerService1 : Attribute { }
/// <summary>
/// WorkerService2 專用的 Singleton Injection _ 
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)] public class InjectForBatchWorkerService2 : Attribute { }
