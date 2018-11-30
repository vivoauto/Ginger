namespace Amdocs.Ginger.Common
{
    public interface IRepositoryItemFactory
    {
        IBusinessFlow CreateBusinessFlow();
        IValueExpression CreateValueExpression(IProjEnvironment mProjEnvironment, IBusinessFlow mBusinessFlow, ObservableList<InterfacesLib.IDataSourceBase> observableList= null, bool v=false);
        ObservableList<IDatabase> GetDatabaseList();
    }
}
