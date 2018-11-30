using System;
using System.Collections.Generic;
using System.Text;

namespace Amdocs.Ginger.Common.InterfacesLib
{
    public interface IGingerRunner
    {
        IEnumerable<IBusinessFlow> BusinessFlows { get; set; }
        string Name { get; set; }
        object CurrentSolution { get; set; }
        object SolutionAgents { get; set; }
        object DSList { get; set; }
        object SolutionApplications { get; set; }
        object SolutionFolder { get; set; }
        Ginger.CoreNET.Execution.eRunStatus Status { get; set; }

        void SetExecutionEnvironment(IProjEnvironment runsetExecutionEnvironment, object p);
    }
}
