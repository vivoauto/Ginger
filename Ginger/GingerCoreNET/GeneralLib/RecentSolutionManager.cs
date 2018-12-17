using Amdocs.Ginger.Common;
using Amdocs.Ginger.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GingerCoreNET.GeneralLib
{
    public class RecentSolutionManager
    {
        const int MAX_RECENTSOL = 10;

        // Keep the folder names of last solutions opened
        [IsSerializedForLocalRepository]
        public List<string> RecentSolutions = new List<string>();

        private ObservableList<ISolution> mRecentSolutionsAsObjects = null;


        private void CleanRecentSolutionsList()
        {
            try
            {
                //Clean not exist Solutions
                for (int i = 0; i < RecentSolutions.Count; i++)
                {
                    if (Directory.Exists(RecentSolutions[i]) == false)
                    {
                        RecentSolutions.RemoveAt(i);
                        i--;
                    }
                }

                //clean resent solutions list from duplications caused due to bug
                for (int i = 0; i < RecentSolutions.Count; i++)
                {
                    for (int j = i + 1; j < RecentSolutions.Count; j++)
                    {
                        if (SolutionRepository.NormalizePath(RecentSolutions[i]) == SolutionRepository.NormalizePath(RecentSolutions[j]))
                        {
                            RecentSolutions.RemoveAt(j);
                            j--;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppReporter.ToLog(eAppReporterLogLevel.ERROR, "Failed to do Recent Solutions list clean up", ex);
            }
        }

        public void AddSolutionToRecent(ISolution loadedSolution)
        {
            //remove existing similar folder path
            string solPath = RecentSolutions.Where(x => SolutionRepository.NormalizePath(x) == SolutionRepository.NormalizePath(loadedSolution.Folder)).FirstOrDefault();
            if (solPath != null)
            {
                RecentSolutions.Remove(solPath);
                ISolution sol = mRecentSolutionsAsObjects.Where(x => SolutionRepository.NormalizePath(x.Folder) == SolutionRepository.NormalizePath(loadedSolution.Folder)).FirstOrDefault();
                if (sol != null)
                {
                    mRecentSolutionsAsObjects.Remove(sol);
                }
            }

            // Add it in first place 
            RecentSolutions.Insert(0, loadedSolution.Folder);
            mRecentSolutionsAsObjects.AddToFirstIndex(loadedSolution);

            while (RecentSolutions.Count > MAX_RECENTSOL)//to keep list of MAX_RECENTSOL
            {
                RecentSolutions.RemoveAt(MAX_RECENTSOL);
            }
        }


        public ObservableList<ISolution> RecentSolutionsAsObjects
        {
            get
            {
                if (mRecentSolutionsAsObjects == null)
                {
                    LoadRecentSolutionsAsObjects();
                }
                return mRecentSolutionsAsObjects;
            }
            set
            {
                mRecentSolutionsAsObjects = value;
            }
        }

        private void LoadRecentSolutionsAsObjects()
        {

            CleanRecentSolutionsList();

            mRecentSolutionsAsObjects = new ObservableList<ISolution>();
            int counter = 0;
            foreach (string s in RecentSolutions)
            {
                string SolutionFile = Path.Combine(s, @"Ginger.Solution.xml");
                if (File.Exists(SolutionFile))
                {
                    try
                    {
                        ISolution sol = (ISolution) Solution.LoadSolution(SolutionFile, false);
                        mRecentSolutionsAsObjects.Add(sol);
                    }
                    catch (Exception ex)
                    {
                        AppReporter.ToLog(eAppReporterLogLevel.ERROR, string.Format("Failed to load the recent solution which in path '{0}'", s), ex);
                    }

                    counter++;
                    if (counter >= 10)
                    {
                        break; // only first latest 10 solutions
                    }
                }
            }

            return;
        }

    }
}
