using Amdocs.Ginger.Common;
using Amdocs.Ginger.Common.Enums;
using Amdocs.Ginger.Common.GeneralLib;
using Amdocs.Ginger.Common.InterfacesLib;
using Amdocs.Ginger.CoreNET.Run.RunsetActions;
using Amdocs.Ginger.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ginger.Run
{
    public class RunSetConfig : RepositoryItemBase
    {
        private string mName;
        [IsSerializedForLocalRepository]
        public string Name
        {
            get { return mName; }
            set
            {
                if (mName != value)
                {
                    mName = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }


        private string mDescription;
        [IsSerializedForLocalRepository]
        public string Description
        {
            get { return mDescription; }
            set
            {
                if (mDescription != value)
                {
                    mDescription = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        [IsSerializedForLocalRepository]
        public ObservableList<IGingerRunner> GingerRunners = new ObservableList<IGingerRunner>();

        [IsSerializedForLocalRepository]
        public ObservableList<RunSetActionBase> RunSetActions = new ObservableList<RunSetActionBase>();

        [IsSerializedForLocalRepository]
        public ObservableList<Guid> Tags = new ObservableList<Guid>();

        public override string GetNameForFileName() { return Name; }

        public string LastRunsetLoggerFolder { get; set; }
        public bool RunsetExecLoggerPopulated
        {
            get
            {
                if (System.IO.Directory.Exists(LastRunsetLoggerFolder))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //TODO: remove make obsolete and use Run Set Actions only 
        /// <summary>
        /// DO_NOT_USE
        /// </summary>
        public bool SendEmail { get; set; }
        /// <summary>
        /// DO_NOT_USE
        /// </summary>
        public Email Email { get; set; }

        public bool mRunModeParallel = true;
        [IsSerializedForLocalRepository(true)]
        public bool RunModeParallel
        {
            get
            {
                return mRunModeParallel;
            }
            set
            {
                mRunModeParallel = value;
                OnPropertyChanged(nameof(this.RunModeParallel));
            }
        }

        public bool mRunWithAnalyzer = true;
        [IsSerializedForLocalRepository(true)]
        public bool RunWithAnalyzer
        {
            get
            {
                return mRunWithAnalyzer;
            }
            set
            {
                mRunWithAnalyzer = value;
                OnPropertyChanged(nameof(this.RunWithAnalyzer));
            }
        }

        public override string ItemName
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        public override eImageType ItemImageType
        {
            get
            {
                return eImageType.RunSet;
            }
        }

        public override string ItemNameField
        {
            get
            {
                return nameof(this.Name);
            }
        }
    }
}
