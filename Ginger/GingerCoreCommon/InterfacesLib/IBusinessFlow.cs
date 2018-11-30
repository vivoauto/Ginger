using GingerCore.Variables;

namespace Amdocs.Ginger.Common
{
    public interface IBusinessFlow
    {
        string RunStatus { get; set; }

        bool Active { get; set; }
        string Name { get; set; }
        object Guid { get; set; }

        ObservableList<VariableBase> GetVariables();

        VariableBase GetHierarchyVariableByName(string varName, bool considerLinkedVar = true);
    }
}
