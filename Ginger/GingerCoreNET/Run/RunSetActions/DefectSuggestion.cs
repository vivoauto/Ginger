using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Amdocs.Ginger.CoreNET.Run.RunSetActions
{
    public class DefectSuggestion
    {
        public Guid FailedActionGuid { get; set; }
        public Guid DefectSuggestionGuid { get; set; }
        public bool ToOpenDefectFlag { get; set; }
        public bool IsOpenDefectFlagEnabled { get; set; }
        public bool AutomatedOpeningFlag { get; set; }
        public string ALMDefectID { get; set; }
        public string RunnerName { get; set; }
        public string BusinessFlowName { get; set; }
        public string ActivitiesGroupName { get; set; }
        public int ActivitySequence { get; set; }
        public string ActivityName { get; set; }
        public int ActionSequence { get; set; }
        public string ActionDescription { get; set; }
        public int RetryIteration { get; set; }
        public string ErrorDetails { get; set; }
        public string ExtraDetails { get; set; }
        public bool IsScreenshotButtonEnabled { get; set; }
        public List<string> ScreenshotFileNames { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }

        public DefectSuggestion(Guid failedActionGuid, string runnerName, string businessFlowName, string activitiesGroupName,
                                int activitySequence, string activityName, int actionSequence,
                                string actionDescription, int retryIteration, string errorDetails,
                                string extraDetails, List<string> screenshotFileNames,
                                bool isScreenshotButtonEnabled, bool automatedOpeningFlag, string description)
        {
            FailedActionGuid = failedActionGuid;
            DefectSuggestionGuid = Guid.NewGuid();
            RunnerName = runnerName;
            BusinessFlowName = businessFlowName;
            ActivitiesGroupName = activitiesGroupName;
            ActivitySequence = activitySequence;
            ActivityName = activityName;
            ActionSequence = actionSequence;
            ActionDescription = actionDescription;
            RetryIteration = retryIteration;
            ErrorDetails = errorDetails;
            ExtraDetails = extraDetails;
            ScreenshotFileNames = screenshotFileNames;
            AutomatedOpeningFlag = automatedOpeningFlag;
            IsScreenshotButtonEnabled = isScreenshotButtonEnabled;

            Summary = businessFlowName + "_" + activityName + "_" + actionDescription + "_Failed_" + DateTime.Now.ToString("ddMMyyyy_HHmmss"); // To review DateTime adding!
            Description = description;

            ToOpenDefectFlag = false;
            IsOpenDefectFlagEnabled = true;
            ALMDefectID = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
