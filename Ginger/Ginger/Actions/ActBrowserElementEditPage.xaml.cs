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

using System.Windows.Controls;
using GingerCore.Actions;
using GingerCore.Platforms;
using System.Linq;
using GingerCoreNET.SolutionRepositoryLib.RepositoryObjectsLib.PlatformsLib;
using Amdocs.Ginger.Common.UIElement;
using Ginger.Actions._Common.ActUIElementLib;
using GingerCore.Platforms.PlatformsInfo;
using System.Collections.Generic;

namespace Ginger.Actions
{
    /// <summary>
    /// Interaction logic for ActJavaBrowserElementEditPage.xaml
    /// </summary>
    public partial class ActBrowserElementEditPage : Page
    {
        private ActBrowserElement mAct;
        PlatformInfoBase mPlatform;
        public ActBrowserElementEditPage(ActBrowserElement act)
        {
            InitializeComponent();
            mAct = act;

            if (act.Platform == ePlatformType.NA)
            {
                act.Platform = GetActivityPlatform();
            }
            mPlatform = PlatformInfoBase.GetPlatformImpl(act.Platform);

            List<ActBrowserElement.eControlAction> supportedControlActions = mPlatform.GetPlatformBrowserControlOperations();

            //bind controls
            App.FillComboFromEnumVal(xControlActionComboBox, mAct.ControlAction, supportedControlActions.Cast<object>().ToList());
            App.ObjFieldBinding(xControlActionComboBox, ComboBox.SelectedValueProperty, mAct, ActBrowserElement.Fields.ControlAction);

            ValueUC.Init(mAct.GetOrCreateInputParam("Value"));
            xLocateValueVE.BindControl(mAct, Act.Fields.LocateValue);
            xGotoURLTypeRadioButton.Init(typeof(ActBrowserElement.eGotoURLType), xGotoURLTypeRadioButtonPnl, mAct.GetOrCreateInputParam(ActBrowserElement.Fields.GotoURLType, ActBrowserElement.eGotoURLType.Current.ToString()));
            xElementLocateByComboBox.BindControl(mAct, Act.Fields.LocateBy);
            xImplicitWaitVE.BindControl(mAct, ActBrowserElement.Fields.ImplicitWait);

            SetVisibleControlsForAction();
        }

        private void ResetView()
        {
            xLocateByAndValuePanel.Visibility = System.Windows.Visibility.Collapsed;
            xOpenURLInPnl.Visibility = System.Windows.Visibility.Collapsed;
            xValueGrid.Visibility = System.Windows.Visibility.Collapsed;
            xImplicitWaitPnl.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ControlActionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetVisibleControlsForAction();
        }

        private ePlatformType GetActivityPlatform()
        {
            string targetapp = App.BusinessFlow.CurrentActivity.TargetApplication;
            ePlatformType platform = (from x in App.UserProfile.Solution.ApplicationPlatforms where x.AppName == targetapp select x.Platform).FirstOrDefault();
            return platform;
        }

        private void SetVisibleControlsForAction()
        {
            ResetView();
            ePlatformType ActivityPlatform = mAct.Platform;

            if (mAct.ControlAction == ActBrowserElement.eControlAction.SwitchFrame || mAct.ControlAction == ActBrowserElement.eControlAction.SwitchWindow || mAct.ControlAction == ActBrowserElement.eControlAction.CloseTabExcept)
            {
                xLocateByAndValuePanel.Visibility = System.Windows.Visibility.Visible;
                SetLocateValueControls();
            }
            else if (mAct.ControlAction == ActBrowserElement.eControlAction.GotoURL || mAct.ControlAction == ActBrowserElement.eControlAction.OpenURLNewTab ||
                     mAct.ControlAction == ActBrowserElement.eControlAction.InjectJS || mAct.ControlAction == ActBrowserElement.eControlAction.RunJavaScript)
            {
                if (mAct.ControlAction == ActBrowserElement.eControlAction.GotoURL || mAct.ControlAction == ActBrowserElement.eControlAction.OpenURLNewTab)
                {
                    if (mAct.ControlAction == ActBrowserElement.eControlAction.GotoURL)
                    {
                        xOpenURLInPnl.Visibility = System.Windows.Visibility.Visible;
                    }
                    xValueGrid.Visibility = System.Windows.Visibility.Visible;
                    xValueLabel.Content = "URL:";
                }
                else if (mAct.ControlAction == ActBrowserElement.eControlAction.InjectJS || mAct.ControlAction == ActBrowserElement.eControlAction.RunJavaScript)
                {
                    xValueGrid.Visibility = System.Windows.Visibility.Visible;
                    xValueLabel.Content = "Script:";
                }
            }
            else
            {
                if (mAct.ControlAction == ActBrowserElement.eControlAction.InitializeBrowser)
                {
                    if (!(ActivityPlatform == ePlatformType.Web))
                    {
                        xLocateByAndValuePanel.Visibility = System.Windows.Visibility.Visible;
                        SetLocateValueControls();

                        xImplicitWaitPnl.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
        }

        private void ElementLocateByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetLocateValueControls();
        }

        private void SetLocateValueControls()
        {
            if (xElementLocateByComboBox.SelectedItem == null)
            {
                xLocateValueVE.Visibility = System.Windows.Visibility.Visible;
                xLocateValueEditFrame.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            eLocateBy SelectedLocType = (eLocateBy)((GingerCore.General.ComboEnumItem)xElementLocateByComboBox.SelectedItem).Value;
            switch (SelectedLocType)
            {
                case eLocateBy.POMElement:
                    xLocateValueVE.Visibility = System.Windows.Visibility.Collapsed;
                    xLocateValueEditFrame.Visibility = System.Windows.Visibility.Visible;
                    Page p = new LocateByPOMElementPage(mAct);
                    xLocateValueEditFrame.Content = p;
                    break;
                default:
                    xLocateValueVE.Visibility = System.Windows.Visibility.Visible;
                    xLocateValueEditFrame.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }
    }
}
