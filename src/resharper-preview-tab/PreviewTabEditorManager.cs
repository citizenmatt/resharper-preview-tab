/*
 * Copyright 2012 - 2013 Matt Ellis
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using JetBrains.DataFlow;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel.Transactions;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using JetBrains.Threading;
using JetBrains.UI.WindowManagement;
using JetBrains.Util;
using JetBrains.VsIntegration.DocumentModel;
using JetBrains.VsIntegration.ProjectDocuments.Projects.Builder;
using JetBrains.VsIntegration.ProjectModel;
using Microsoft.VisualStudio.Shell.Interop;

namespace CitizenMatt.ReSharper.PreviewTab
{
    [SolutionComponent]
    public partial class PreviewTabEditorManager
    {
        // Defined in Microsoft.VisualStudio.Shell.11.0.dll, but this saves us having to reference it
        // (and reference Microsoft.VisualStudio.Shell.12.0.dll for VS2013)
        private static readonly Guid NavigationReason = new Guid("8d57e022-9e44-4efd-8e4e-230284f86376");

        private readonly IVsUIShellOpenDocument3 vsUiShellOpenDocument3;
        private readonly DocumentTransactionManager documentTransactionManager;
        private readonly IThreading threading;
        private IVsNewDocumentStateContext newDocumentStateContext;
        private Action doEnablePreviewTab;

        public PreviewTabEditorManager(Lifetime lifetime, ProjectModelSynchronizer projectModelSynchronizer,
                                       IVsUIShellOpenDocument vsUiShellOpenDocument,
                                       VsDocumentManagerSynchronization vsDocumentManagerSynchronization,
                                       ITextControlManager textControlManager, IFrameFocusHelper frameFocusHelper,
                                       DocumentManager documentManager, DocumentTransactionManager documentTransactionManager,
                                       IThreading threading)
            : base(lifetime, projectModelSynchronizer, vsUiShellOpenDocument, vsDocumentManagerSynchronization,
                   textControlManager, frameFocusHelper, documentManager)
        {
            vsUiShellOpenDocument3 = vsUiShellOpenDocument as IVsUIShellOpenDocument3;
            this.documentTransactionManager = documentTransactionManager;
            this.threading = threading;

            doEnablePreviewTab = DoEnablePreviewTab;
        }

        protected override bool OpenInProvisionTab(TabOptions tabOptions)
        {
            // If we're in a transaction, something's happening (e.g. opening files for a refactoring)
            // so don't force the preview tab. If we're not in a transaction, that just means someone's
            // navigating to the file, so yes, force the preview tab
            return !IsInDocumentTransaction;
        }

        protected override void EnablePreviewTab()
        {
            doEnablePreviewTab();
        }

        private ITextControl OverrideOpenProjectFile(IProjectFile projectFile, bool activate, FileView fileViewPrimary,
            TabOptions tabOptions = TabOptions.Default)
        {
              var textControl = base.OpenProjectFile(projectFile, activate, fileViewPrimary, tabOptions);
              RestoreNewDocumentStateContext();
              return textControl;
        }

        private ITextControl OverrideOpenFile(FileSystemPath fileName, bool activate, TabOptions tabOptions)
        {
            var textControl = base.OpenFile(fileName, activate, tabOptions);
            RestoreNewDocumentStateContext();
            return textControl;
        }

        private bool IsInDocumentTransaction
        {
            get { return documentTransactionManager.CurrentTransaction != null; }
        }

        private void DoEnablePreviewTab()
        {
            if (vsUiShellOpenDocument3 == null)
                return;

            SetNewDocumentState();
            DisablePreviewTabUntilIdle();
        }

        private void SetNewDocumentState()
        {
            newDocumentStateContext = vsUiShellOpenDocument3.SetNewDocumentState((uint)__VSNEWDOCUMENTSTATE.NDS_Provisional, NavigationReason);
        }

        // We can only open one document in the preview tab. Subsequent documents need to use normal tabs.
        // Enqueue a re-enable command with the UI - once the UI is executing again, the preview tab has
        // been shown, and it's fair game again. This fixes issues with multi-file templates
        private void DisablePreviewTabUntilIdle()
        {
            doEnablePreviewTab = () => { };
            threading.ReentrancyGuard.Queue("reset preview tab", () => doEnablePreviewTab = DoEnablePreviewTab);
        }

        // Make sure we restore the document state, or VS will try to open subsequent documents in the preview
        // tab (ideally, this should happen in VsEditorManager, which calls SetNewDocumentState). Incidentally,
        // I don't know why we don't need to reset this when using to open files one at a time, but we do need
        // to use it when we try to open multiple files at once (e.g. multi-file templates). Perhaps the GC
        // kicks in and frees the early object for us? Or there's a timeout?
        private void RestoreNewDocumentStateContext()
        {
            if (newDocumentStateContext != null)
            {
                newDocumentStateContext.Restore();
                newDocumentStateContext = null;
            }
        }
    }
}