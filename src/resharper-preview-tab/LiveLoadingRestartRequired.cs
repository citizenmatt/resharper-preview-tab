using System;
using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.Application.Components;
using JetBrains.Application.Extensions;
using JetBrains.DataFlow;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.Util;
using Lifetimes = JetBrains.DataFlow.Lifetimes;

namespace CitizenMatt.ReSharper.PreviewTab
{
    [ShellComponent]
    public class LiveLoadingRestartRequired : IExtensionRepository
    {
        private readonly SolutionsManager solutionsManager;

        public LiveLoadingRestartRequired(SolutionsManager solutionsManager)
        {
            this.solutionsManager = solutionsManager;
        }

        public bool CanUninstall(string id)
        {
            return false;
        }

        public void Uninstall(string id, bool removeDependencies, IEnumerable<string> dependencies, Action<LoggingLevel, string> logger)
        {
        }

        public bool HasMissingExtensions()
        {
            return false;
        }

        public void RestoreMissingExtensions()
        {
        }

        public IEnumerable<string> GetExtensionsRequiringRestart()
        {
            if (NeedsRestart())
                yield return "PreviewTab";
        }

        // If the plugin is loaded at startup, the component model successfully resolves EditorManager
        // to our derived type. If we're live loaded (i.e. just installed), then it's already got the
        // default implementation of EditorManager, as well as our version. If we find more than one
        // instance, report that we need a restart. On the plus side, EditorManager is a solution
        // component, so we can be live loaded when a solution is closed and everything is good
        private bool NeedsRestart()
        {
            var solution = solutionsManager.Solution;
            if (solution != null)
            {
                // Annoyingly, the simple solution.GetComponents<EditorManager>().Count() > 1
                // causes issues when using a checked build (e.g. EAP builds). Retrieving a
                // component asserts the same cardinality as the first time it was fetched.
                // In other words, the first time you call GetComponent or GetComponents you
                // set the cardinality of that type to be single or multiple, respectively.
                // Checked builds assert that you call it the same way on subsequent calls,
                // so you don't mistakenly try to get many when there's only supposed to be
                // one, and more importantly, vice versa. The assert (VerifyCardinality) is
                // disabled in production builds (thanks to [Conditional("JET_MODE_ASSERT")])
                // but it would be nice to have a safe GetAllComponents that wouldn't assert
                // and just return 1 or more components.
                try
                {
                    solution.GetComponent<EditorManager>();
                }
                catch (Exception e)
                {
                    return true;
                }
            }
            return false;
        }
    }
}