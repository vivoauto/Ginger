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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using amdocs.ginger.GingerCoreNET;
using Amdocs.Ginger.Common;
using Amdocs.Ginger.Repository;
using Ginger.UserConfig;
using GingerCore;
using GingerCoreNET.GeneralLib;
using GingerCoreNET.ReporterLib;
using GingerCoreNET.SolutionRepositoryLib.RepositoryObjectsLib.PlatformsLib;
using GingerCoreNET.SourceControl;
using Newtonsoft.Json.Linq;

namespace Ginger
{
    public enum eGingerStatus {
        Closed, Active, AutomaticallyClosed
    }

    public enum eUserType
    {
        Regular,
        Business
    }

    public enum eUserRole
    {
        None,
        [EnumValueDescription("Product Owner")]
        ProductOwner,
        [EnumValueDescription("Scrum Master")]
        ScrumMaster,
        Developer,
        Tester,
        [EnumValueDescription("Technical Writer")]
        TechnicalWriter,
        [EnumValueDescription("User Experience")]
        UserExperience,
        [EnumValueDescription("Release Manager")]
        ReleaseManager,
        [EnumValueDescription("SW Architect")]
        SWArchitect,
        [EnumValueDescription("RM Scoping")]
        RMScoping,
        [EnumValueDescription("Service Partner")]
        ServicePartner,
        PMO,
        [EnumValueDescription("Dev Manager")]
        DevManager,
        [EnumValueDescription("Program Manager")]
        ProgramManager,
        Architect,
        [EnumValueDescription("Project Manager")]
        ProjectManager,
        [EnumValueDescription("Testing Manager")]
        TestingManager,
        [EnumValueDescription("Dev Expert")]
        DevExpert
    }

    public class UserProfile : RepositoryItemBase
    {
        //Move it to UCGridLib
        public class UserProfileGrid
        {
            public string GridId { get; set; }
            public List<UserProfileGridCol> Cols = new List<UserProfileGridCol>();
        }

        public class UserProfileGridCol
        {
            public string Name { get; set; }
            public int Width { get; set; }
        }

        public string LocalWorkingFolder { get; set; }

        public ISolution mSolution { get; set; }

        public ISolution Solution
        {
            get { return mSolution; }
            set
            {
                mSolution = value;
                OnPropertyChanged(nameof(Solution));
            }
        }

        public List<UserProfileGrid> Grids = new List<UserProfileGrid>();

        bool mAutoLoadLastSolution;
        [IsSerializedForLocalRepository]
        public bool AutoLoadLastSolution
        {
            get
            {
                return mAutoLoadLastSolution;
            }
            set
            {
                mAutoLoadLastSolution = value;
                OnPropertyChanged(nameof(AutoLoadLastSolution));
            }
        }

        [IsSerializedForLocalRepository]
        public eGingerStatus GingerStatus { get; set; }

        [IsSerializedForLocalRepository]
        public List<string> RecentAppAgentsMapping = new List<string>();

        [IsSerializedForLocalRepository]
        public Guid RecentBusinessFlow { get; set; }

        [IsSerializedForLocalRepository]
        public Guid RecentEnvironment { get; set; }

        [IsSerializedForLocalRepository]
        public Guid RecentRunset { get; set; }

        [IsSerializedForLocalRepository]
        public SourceControlBase.eSourceControlType SourceControlType { get; set; }

        [IsSerializedForLocalRepository]
        public string SourceControlURL { get; set; }

        [IsSerializedForLocalRepository]
        public string SourceControlUser { get; set; }

        [IsSerializedForLocalRepository]
        public string SolutionSourceControlUser { get; set; }

        [IsSerializedForLocalRepository]
        public bool SolutionSourceControlConfigureProxy { get; set; }

        [IsSerializedForLocalRepository]
        public string SolutionSourceControlProxyAddress { get; set; }

        [IsSerializedForLocalRepository]
        public string SolutionSourceControlAuthorName { get; set; }

        [IsSerializedForLocalRepository]
        public string SolutionSourceControlAuthorEmail { get; set; }

        [IsSerializedForLocalRepository]
        public string SolutionSourceControlProxyPort { get; set; }

        [IsSerializedForLocalRepository]
        public string EncryptedSourceControlPass { get; set; }

        [IsSerializedForLocalRepository]
        public string EncryptedSolutionSourceControlPass { get; set; }

        [IsSerializedForLocalRepository]
        public string SourceControlLocalFolder { get; set; }

        [IsSerializedForLocalRepository]
        public string ReportTemplateName { get; set; }

        public string SourceControlPass
        {
            get
            {
                bool res = false;
                string pass = string.Empty;
                if (EncryptedSourceControlPass != null)
                    pass = EncryptionHandler.DecryptString(EncryptedSourceControlPass, ref res);
                if (res && String.IsNullOrEmpty(pass) == false)
                    return pass;
                else
                    return string.Empty;
            }
            set
            {
                bool res = false;
                if (value != null)
                    EncryptedSourceControlPass = EncryptionHandler.EncryptString(value, ref res);
            }
        }

        public string SolutionSourceControlPass
        {
            get
            {
                bool res = false;
                string pass = string.Empty;
                if (EncryptedSolutionSourceControlPass != null)
                    pass = EncryptionHandler.DecryptString(EncryptedSolutionSourceControlPass, ref res);
                if (res && String.IsNullOrEmpty(pass) == false)
                    return pass;
                else
                    return string.Empty;
            }
            set
            {
                bool res = false;
                if (value != null)
                    EncryptedSolutionSourceControlPass = EncryptionHandler.EncryptString(value, ref res);
            }
        }

        [IsSerializedForLocalRepository]
        public string ALMUserName { get; set; }

        public string ALMPassword
        {
            get
            {
                bool res = false;
                string pass = EncryptionHandler.DecryptString(EncryptedALMPassword, ref res);
                if (res && String.IsNullOrEmpty(pass) == false)
                    return pass;
                else
                    return string.Empty;
            }
            set
            {
                bool res = false;
                EncryptedALMPassword = EncryptionHandler.EncryptString(value, ref res);
            }
        }
        [IsSerializedForLocalRepository]
        public string EncryptedALMPassword { get; set; }

        eTerminologyType mTerminologyType;
        [IsSerializedForLocalRepository]
        public eTerminologyType TerminologyDictionaryType
        {
            get { return mTerminologyType; }
            set { mTerminologyType = value; OnPropertyChanged(nameof(TerminologyDictionaryType)); }
        }

        eAppReporterLoggingLevel mAppLogLevel;
        [IsSerializedForLocalRepository]
        public eAppReporterLoggingLevel AppLogLevel
        {
            get { return mAppLogLevel; }
            set { mAppLogLevel = value; Reporter.CurrentAppLogLevel = mAppLogLevel; OnPropertyChanged(nameof(AppLogLevel)); }
        }

        GingerCore.eUserType mUserType;
        [IsSerializedForLocalRepository]
        public GingerCore.eUserType UserType
        { 
            get
            {
                return mUserType;
            }
            set
            {
                mUserType = value;
                OnPropertyChanged(nameof(UserType));
            }
        }

        public UserTypeHelper UserTypeHelper { get; set; }

        public static string UserProfileFilePath
        {
            get
            {
                return Path.Combine(WorkSpace.Instance.LocalUserApplicationDataFolderPath, "Ginger.UserProfile.xml");
            }
        }

        public void SaveUserProfile()
        {
            try
            {
                SaveRecentAppAgentsMapping();
            }
            catch (Exception ex) { AppReporter.ToLog(eAppReporterLogLevel.ERROR, $"Method - {MethodBase.GetCurrentMethod().Name}, Error - {ex.Message}", ex); }
            
            RepositorySerializer.SaveToFile(this, UserProfileFilePath);
        }

        public void SaveRecentAppAgentsMapping()
        {
            if (mSolution != null)
            {
                //remove last saved mapping for this solution
                string existingSolMapping = RecentAppAgentsMapping.Where(x => x.Contains(mSolution.Name + "***") == true).FirstOrDefault();
                if (string.IsNullOrEmpty(existingSolMapping) == false)
                {
                    RecentAppAgentsMapping.Remove(existingSolMapping);
                }                    

                //create new save to this solution
                existingSolMapping = mSolution.Name + "***";
                foreach (ApplicationPlatform ap in mSolution.ApplicationPlatforms)
                {
                    if (string.IsNullOrEmpty(ap.LastMappedAgentName) == false)
                    {
                        existingSolMapping = string.Format("{0}{1},{2}#", existingSolMapping, ap.AppName, ap.LastMappedAgentName);
                    }
                }
                RecentAppAgentsMapping.Add(existingSolMapping);
            }
        }

        public void LoadRecentAppAgentMapping()
        {
            if (mSolution != null)
            {
                //unserialize the current solution mapping saving
                string existingSolMapping = RecentAppAgentsMapping.Where(x => x.Contains(mSolution.Name + "***") == true).FirstOrDefault();
                if (string.IsNullOrEmpty(existingSolMapping))
                {
                    return;//no saved mapping
                }
                else
                {
                    string solName = mSolution.Name + "***";
                    existingSolMapping = existingSolMapping.Replace(solName, string.Empty);
                    List<string> appAgentMapping = existingSolMapping.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    Dictionary<string, string> mappingDic = new Dictionary<string, string>();
                    foreach (string mapping in appAgentMapping)
                    {
                        string[] appAgent = mapping.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (appAgent.Length == 2)
                        {
                            mappingDic.Add(appAgent[0], appAgent[1]);
                        }
                    }

                    //update the solution apps with the saved mapping
                    if (mappingDic.Count > 0)
                    {
                        foreach (ApplicationPlatform ap in mSolution.ApplicationPlatforms)
                        {
                            if (mappingDic.Keys.Contains(ap.AppName))
                            {
                                if (ap != null && WorkSpace.Instance.SolutionRepository.GetAllRepositoryItems<IAgent>().Count > 0)
                                {
                                    List<IAgent> platformAgents = (from p in WorkSpace.Instance.SolutionRepository.GetAllRepositoryItems<IAgent>() where p.Platform == ap.Platform select p).ToList();
                                    IAgent matchingAgent = platformAgents.Where(x => x.Name == mappingDic[ap.AppName]).FirstOrDefault();
                                    if (matchingAgent != null)
                                        ap.LastMappedAgentName = matchingAgent.Name;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static UserProfile LoadUserProfile()
        {
            if (General.isDesignMode()) return null;
            
            string InstallationConfigurationPath = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("Ginger.exe", "Ginger.InstallationConfiguration.Json");
            DateTime InstallationDT = File.GetLastWriteTime(InstallationConfigurationPath);
            
            string UserConfigJsonString = string.Empty;
            JObject UserConfigJsonObj = null;
            Dictionary<string, string> UserConfigdictObj = null;
            if (System.IO.File.Exists(InstallationConfigurationPath))
            {
                UserConfigJsonString = System.IO.File.ReadAllText(InstallationConfigurationPath);
                UserConfigJsonObj = JObject.Parse(UserConfigJsonString);
                UserConfigdictObj = UserConfigJsonObj.ToObject<Dictionary<string, string>>();
            }

            if (File.Exists(UserProfileFilePath))
            {
                try
                {
                    DateTime UserProfileDT = File.GetLastWriteTime(UserProfileFilePath);
                    AppReporter.ToLog(eAppReporterLogLevel.INFO, string.Format("Loading existing User Profile at '{0}'", UserProfileFilePath));
                    string userProfileTxt = File.ReadAllText(UserProfileFilePath);
                    UserProfile up = (UserProfile)NewRepositorySerializer.DeserializeFromText(userProfileTxt);
                    up.FilePath = UserProfileFilePath;                 
                    if (DateTime.Compare(UserProfileDT, InstallationDT) < 0)
                    {
                        if (UserConfigdictObj != null)
                        {
                            up.AddUserConfigProperties(UserConfigdictObj);
                        }
                    }
                    return up;
                }
                catch (Exception ex)
                {
                    AppReporter.ToLog(eAppReporterLogLevel.ERROR, string.Format("Failed to load the existing User Profile at '{0}'", UserProfileFilePath), ex);
                }
            }

            AppReporter.ToLog(eAppReporterLogLevel.INFO, "Creating new User Profile");

            UserProfile up2 = new UserProfile();
            up2.LoadDefaults();
            if (UserConfigdictObj != null)
            {
                up2.AddUserConfigProperties(UserConfigdictObj);
            }
            
            return up2;
        }

        public void LoadUserTypeHelper()
        {
            UserTypeHelper = new UserTypeHelper();
            UserTypeHelper.Init(UserType);
        }

        private void AddUserConfigProperties(Dictionary<string, string> dictObj)
        {
            switch (dictObj["UserType"])
            {
                case "Regular":
                    UserType = GingerCore.eUserType.Regular;
                    break;

                case "Business":
                    UserType = GingerCore.eUserType.Business;
                    break;
            }

            switch (dictObj["TerminologyDictionaryType"])
            {
                case "Default":
                    TerminologyDictionaryType = GingerCore.eTerminologyType.Default;
                    break;

                case "Testing":
                    TerminologyDictionaryType = GingerCore.eTerminologyType.Testing;
                    break;
                case "Gherkin":
                    TerminologyDictionaryType = GingerCore.eTerminologyType.Gherkin;
                    break;
            }
        }

        public void LoadDefaults()
        {
            AutoLoadLastSolution = true; //#Task 160            
            string defaultFolder = WorkSpace.Instance.DefaultUserLocalWorkingFolder;//calling it so it will be created
        }
               
        internal string GetDefaultReport()
        {
            if (!string.IsNullOrEmpty(ReportTemplateName))
            {
                return ReportTemplateName;
            }
            else
            {
                // for new user who didn't defined yet report we give the default Full rep
                return "Full Detailed Report";
            }
        }

        public override string ItemName
        {
            get
            {
                return string.Empty;
            }
            set
            {
                return;
            }
        }

        bool mDoNotAskToUpgradeSolutions = false;
        [IsSerializedForLocalRepository]
        public bool DoNotAskToUpgradeSolutions
        {
            get
            {
                return mDoNotAskToUpgradeSolutions;
            }
            set
            {
                mDoNotAskToUpgradeSolutions = value;
                OnPropertyChanged(nameof(DoNotAskToUpgradeSolutions));
            }
        }

        [IsSerializedForLocalRepository]
        public bool NewHelpLibraryMessgeShown { get; set; }


        bool mAskToSaveBusinessFlow = true;
        [IsSerializedForLocalRepository(true)]
        public bool AskToSaveBusinessFlow
        {
            get
            {
                return mAskToSaveBusinessFlow;
            }
            set
            {
                mAskToSaveBusinessFlow = value;
                OnPropertyChanged(nameof(AskToSaveBusinessFlow));
            }
        }

        [IsSerializedForLocalRepository]
        public bool DoNotAskToRecoverSolutions { get; set; }

        string mProfileImage;
        [IsSerializedForLocalRepository]
        public string ProfileImage
        {
            get
            {
                return mProfileImage;
            }
            set
            {
                if (mProfileImage != value)
                {
                    mProfileImage = value;
                    OnPropertyChanged(nameof(ProfileImage));
                }
            }
        }

        public string UserName
        {
            get { return Environment.UserName; }
        }

        string mUserFirstName;
        [IsSerializedForLocalRepository]
        public string UserFirstName
        {
            get
            {
                return mUserFirstName;
            }
            set
            {
                mUserFirstName = value;
                OnPropertyChanged(nameof(UserFirstName));
            }
        }

        string mUserMiddleName;
        [IsSerializedForLocalRepository]
        public string UserMiddleName
        {
            get
            {
                return mUserMiddleName;
            }
            set
            {
                mUserMiddleName = value;
                OnPropertyChanged(nameof(UserMiddleName));
            }
        }

        string mUserLastName;
        [IsSerializedForLocalRepository]
        public string UserLastName
        {
            get
            {
                return mUserLastName;
            }
            set
            {
                mUserLastName = value;
                OnPropertyChanged(nameof(UserLastName));
            }
        }

        string mUserEmail;
        [IsSerializedForLocalRepository]
        public string UserEmail
        {
            get
            {
                return mUserEmail;
            }
            set
            {
                mUserEmail = value;
                OnPropertyChanged(nameof(UserEmail));
            }
        }

        string mUserPhone;
        [IsSerializedForLocalRepository]
        public string UserPhone
        {
            get
            {
                return mUserPhone;
            }
            set
            {
                mUserPhone = value;
                OnPropertyChanged(nameof(UserPhone));
            }
        }

        eUserRole mUserRole;
        [IsSerializedForLocalRepository]
        public eUserRole UserRole
        {
            get
            {
                return mUserRole;
            }
            set
            {
                mUserRole = value;
                OnPropertyChanged(nameof(UserRole));
            }
        }

        string mUserDepartment;
        [IsSerializedForLocalRepository]
        public string UserDepartment
        {
            get
            {
                return mUserDepartment;
            }
            set
            {
                mUserDepartment = value;
                OnPropertyChanged(nameof(UserDepartment));
            }
        }

        public int SolutionSourceControlTimeout { get; set; }

        public void AddSolutionToRecent(Ginger.SolutionGeneral.Solution sol)
        {
            throw new NotImplementedException();
        }
    }
}
