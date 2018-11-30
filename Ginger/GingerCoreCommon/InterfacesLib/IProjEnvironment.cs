using GingerCore.Environments;

namespace Amdocs.Ginger.Common
{
    public interface IProjEnvironment
    {
        object Guid { get; }

        EnvApplication GetApplication(string appName);
    }
}
