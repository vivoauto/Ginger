#region License
/*
Copyright © 2014-2018 European Support Limited

Licensed under the Apache License, Version 2.0 (the "License")
you may not use this file except in compliance with the License.
You may obtain a copy of the License at 

http://www.apache.org/licenses/LICENSE-2.0 

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS, 
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and 
limitations under the License. 
*/
#endregion

using System.Collections.Generic;
using GingerCore.Variables;

namespace Amdocs.Ginger.Repository
{
    public interface ISolution
    {
        string Name { get; set; }
        string Folder { get; set; }
        IEnumerable<VariableBase> Variables { get; }
        string MainApplication { get; }
        object ExecutionLoggerConfigurationSetList { get; }
        object RecentlyUsedBusinessFlows { get; }
        string LastBusinessFlowFileName { get; set; }

        ISolution LoadSolution(string solutionFile, bool v);
        void SetReportsConfigurations();
    }
}
