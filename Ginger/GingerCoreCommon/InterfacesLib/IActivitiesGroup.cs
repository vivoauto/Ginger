﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Amdocs.Ginger.Common.InterfacesLib
{
    public enum eActivitiesGroupRunStatus
    {
        Pending,
        Running,
        Passed,
        Failed,
        Stopped,
        Blocked,
        Skipped
    }
    public enum executionLoggerStatus
    {
        NotStartedYet,
        StartedNotFinishedYet,
        Finished
    }
    public interface IActivitiesGroup
    {

        Dictionary<Guid, DateTime> ExecutedActivities { get; set; }
        Guid Guid { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string AutomationPrecentage { get; }
        DateTime StartTimeStamp { get; set; }
        DateTime EndTimeStamp { get; set; }
        Single? ElapsedSecs { get; set; }
        eActivitiesGroupRunStatus RunStatus { get; set; }
        string ExternalID { get; set; }
        string ExternalID2 { get; set; }

        string TempReportFolder { get; set; }
        ObservableList<IActivityIdentifiers> ActivitiesIdentifiers { get; set; }
        string TestSuiteId { get; set; }
        executionLoggerStatus ExecutionLoggerStatus { get; set; }
        string ExecutionLogFolder { get; set; }
        double? Elapsed { get; set; }
    }
}