using System;
using System.Collections.Generic;
using System.Text;
using GingerCore.DataSource;

namespace Amdocs.Ginger.Common.InterfacesLib
{
    public interface IDataSourceBase
    {
        ObservableList<DataSourceBase> GetVariables();
    }
}
