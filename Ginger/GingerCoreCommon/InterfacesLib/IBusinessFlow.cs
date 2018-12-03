using System;
using System.Collections.Generic;
using GingerCore.Variables;

namespace Amdocs.Ginger.Common
{
    public interface IBusinessFlow
    {
        string RunStatus { get; set; }

        bool Active { get; set; }
        string Name { get; set; }
        object Guid { get; set; }
        object Mandatory { get; set; }
        Guid InstanceGuid { get; set; }
        object RunDescription { get; set; }
        object BFFlowControls { get; set; }

        ObservableList<VariableBase> GetVariables();

        VariableBase GetHierarchyVariableByName(string varName, bool considerLinkedVar = true);
        IBusinessFlow CreateCopy(bool v);
        void Reset();
        void AttachActivitiesGroupsAndActivities();
        IEnumerable<VariableBase> GetBFandActivitiesVariabeles(bool v);
    }
}
