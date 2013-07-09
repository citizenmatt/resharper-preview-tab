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

using JetBrains.DataFlow;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel.Transactions;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using JetBrains.UI.WindowManagement;
using JetBrains.VsIntegration.DocumentModel;
using JetBrains.VsIntegration.ProjectModel;
using Microsoft.VisualStudio.Shell.Interop;

namespace CitizenMatt.ReSharper.PreviewTab
{
    [SolutionComponent]
    public partial class PreviewTabEditorManager
    {
        private readonly DocumentTransactionManager documentTransactionManager;

        public PreviewTabEditorManager(Lifetime lifetime, ProjectModelSynchronizer projectModelSynchronizer,
                                       IVsUIShellOpenDocument vsUiShellOpenDocument,
                                       VsDocumentManagerSynchronization vsDocumentManagerSynchronization,
                                       ITextControlManager textControlManager, IFrameFocusHelper frameFocusHelper,
                                       DocumentManager documentManager, DocumentTransactionManager documentTransactionManager)
            : base(lifetime, projectModelSynchronizer, vsUiShellOpenDocument, vsDocumentManagerSynchronization,
                   textControlManager, frameFocusHelper, documentManager)
        {
            this.documentTransactionManager = documentTransactionManager;
        }

        protected override bool OpenInProvisionTab(TabOptions tabOptions)
        {
            // If we're in a transaction, something's happening (e.g. opening files for a refactoring)
            // so don't force the preview tab. If we're not in a transaction, that just means someone's
            // navigating to the file, so yes, force the preview tab
            return IsInDocumentTransaction ? base.OpenInProvisionTab(tabOptions) : ShouldUsePreviewTab(tabOptions);
        }

        private bool IsInDocumentTransaction
        {
            get { return documentTransactionManager.CurrentTransaction != null; }
        }

        private bool ShouldUsePreviewTab(TabOptions tabOptions)
        {
            return true;
        }
    }
}