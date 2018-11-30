using Amdocs.Ginger.Common;
using Amdocs.Ginger.Common.InterfacesLib;
using GingerCore;
using GingerCore.Environments;

namespace Ginger.Repository
{
    public class RepositoryItemFactory : IRepositoryItemFactory
    {
        public IBusinessFlow CreateBusinessFlow()
        {
            return new BusinessFlow();
        }

        public IValueExpression CreateValueExpression(IProjEnvironment mProjEnvironment, IBusinessFlow mBusinessFlow, ObservableList<IDataSourceBase> observableList = null, bool v = false)
        {
            return new ValueExpression(mProjEnvironment, mBusinessFlow, observableList, v);
        }

        public ObservableList<IDatabase> GetDatabaseList()
        {
            return new ObservableList<IDatabase>();
        }

        public ObservableList<IDataSourceBase> GetDatasourceList()
        {
            return new ObservableList<IDataSourceBase>();
        }
    }
}
