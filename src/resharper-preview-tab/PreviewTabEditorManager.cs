using JetBrains.DataFlow;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel.Transactions;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using JetBrains.UI.WindowManagement;
using JetBrains.VsIntegration.DevEleven;
using JetBrains.VsIntegration.DocumentModel;
using JetBrains.VsIntegration.ProjectModel;
using Microsoft.VisualStudio.Shell.Interop;

namespace resharper_preview_tab
{
    [SolutionComponent]
    public class PreviewTabEditorManager : VSEditorManager11 
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