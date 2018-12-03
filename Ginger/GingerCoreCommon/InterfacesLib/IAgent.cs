using GingerCoreNET.SolutionRepositoryLib.RepositoryObjectsLib.PlatformsLib;

namespace Amdocs.Ginger.Common
{
    public interface IAgent
    {
        string Name { get; }
        ePlatformType Platform { get; }
    }
}
