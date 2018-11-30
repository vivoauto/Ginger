using Amdocs.Ginger.Common.InterfacesLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amdocs.Ginger.CoreNET.Reports
{
    public abstract class XMLReportBase
    {
        public IReportInfo RI;
        public abstract string CreateReport(IReportInfo RI, bool statusByGroupActivity);
        //public int Passcount { get { return RI.TotalBusinessFlowsPassed; } }
        //public int Failcount { get { return RI.TotalBusinessFlowsFailed; } }
        //public int ActivityCount { get { return RI.TotalActivitiesCount; } }

        //public int ActivityPass { get { return RI.TotalActivitiesByRunStatus(Amdocs.Ginger.CoreNET.Execution.eRunStatus.Passed); } }
        //public int ActivityFail { get { return RI.TotalActivitiesByRunStatus(Amdocs.Ginger.CoreNET.Execution.eRunStatus.Failed); } }
        //public int ActivityOther { get { return ActivityCount - ActivityPass - ActivityFail; } } // ?? TODO: give details of all options

        //public int ActionCount { get { return RI.TotalActionsCount(); } }
        //public int ActionPass { get { return RI.TotalActionsCountByStatus(Amdocs.Ginger.CoreNET.Execution.eRunStatus.Passed); } }
        //public int ActionFail { get { return RI.TotalActionsCountByStatus(Amdocs.Ginger.CoreNET.Execution.eRunStatus.Failed); } }
        //public int ActionOther { get { return ActionCount - ActionPass - ActionFail; } }

        //public int ValidationCount { get { return RI.TotalValidationsCount(); } }
        //public int ValidationPass { get { return RI.TotalValidationsCountByStatus(ActReturnValue.eStatus.Passed); } }
        //public int validationFail { get { return RI.TotalValidationsCountByStatus(ActReturnValue.eStatus.Failed); } }

        protected string BizFlowHTMLColumns(BusinessFlowReport BFR)
        {
            BusinessFlow BF = BFR.GetBusinessFlow();
            XElement xe = new XElement("div", BF.Name);
            xe.Add(new XElement("td", BF.ElapsedSecs));

            XElement xstatus = new XElement("td", BF.RunStatus);
            if (BF.RunStatus == Amdocs.Ginger.CoreNET.Execution.eRunStatus.Passed)
            {
                xstatus.SetAttributeValue("bgColor", "green");
            }
            else
                if (BF.RunStatus == Amdocs.Ginger.CoreNET.Execution.eRunStatus.Failed)
            {
                xstatus.SetAttributeValue("bgColor", "red");
            }

            xe.Add(xstatus);
            return xe.ToString();
        }

        protected XNode CreateHTMLTable<T>(IEnumerable<T> items, IEnumerable<string> header, params Func<T, string>[] columns)
        {
            if (!items.Any())
                return null;

            var html = items.Aggregate(new XElement("table", new XAttribute("border", 1)),
                (table, item) =>
                {
                    table.Add(columns.Aggregate(new XElement("tr"),
                        (row, cell) =>
                        {
                            row.Add(new XElement("td", EvalColumn(cell, item)));
                            return row;
                        }));
                    return table;
                });

            html.AddFirst(header.Aggregate(new XElement("tr"),
                (row, caption) => { row.Add(new XElement("th", caption)); return row; }));

            return html;
        }

        XNode EvalColumn<T>(Func<T, string> cell, T item)
        {
            var raw = cell(item);
            try
            {
                var xml = XElement.Parse(raw);
                return xml;
            }
            catch (XmlException)
            {
                return new XText(raw);
            }
        }
    }
}
