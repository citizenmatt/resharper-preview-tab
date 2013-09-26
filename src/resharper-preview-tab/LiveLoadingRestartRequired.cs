using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Extensions;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.Util;

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
            return solution != null && solution.GetComponents<EditorManager>().Count() > 1;
        }
    }
}