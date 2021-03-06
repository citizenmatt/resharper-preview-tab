﻿/*
 * Copyright 2012 - 2016 Matt Ellis
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
using System.Threading;
using JetBrains.DataFlow;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel.Transactions;
using JetBrains.IDE;
using JetBrains.Platform.VisualStudio.SinceVs11.IDE;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using JetBrains.Threading;
using JetBrains.UI.WindowManagement;
using JetBrains.VsIntegration.DocumentModel;
using JetBrains.VsIntegration.ProjectDocuments.Projects.Builder;
using Microsoft.VisualStudio.Shell.Interop;

namespace CitizenMatt.ReSharper.PreviewTab
{
  [SolutionComponent]
  public class PreviewTabEditorManager : EditorManagerSinceVs11
  {
    private readonly DocumentTransactionManager documentTransactionManager;
    private readonly IThreading threading;
    private long previewTabRequests;

    public PreviewTabEditorManager(Lifetime lifetime, ProjectModelSynchronizer projectModelSynchronizer,
                                   IVsUIShellOpenDocument vsUiShellOpenDocument,
                                   VsDocumentManagerSynchronization vsDocumentManagerSynchronization,
                                   ITextControlManager textControlManager, IFrameFocusHelper frameFocusHelper,
                                   DocumentManager documentManager,
                                   DocumentTransactionManager documentTransactionManager,
                                   IThreading threading)
      : base(lifetime, projectModelSynchronizer, vsUiShellOpenDocument, vsDocumentManagerSynchronization,
             textControlManager, frameFocusHelper, documentManager)
    {
      this.documentTransactionManager = documentTransactionManager;
      this.threading = threading;

      previewTabRequests = 0;
    }

    protected override bool OpenInProvisionTab(TabOptions tabOptions)
    {
      // If we're in a transaction, something's happening (e.g. opening files for a refactoring)
      // so don't force the preview tab. If we're not in a transaction, that just means someone's
      // navigating to the file, so yes, force the preview tab
      return !HasCurrentPreviewTabRequest && !IsInDocumentTransaction;
    }

    protected override IDisposable WithEnabledPreviewTab(bool openInPreviewTab)
    {
      var disposable = base.WithEnabledPreviewTab(openInPreviewTab);
      AddPreviewTabRequest();
      return disposable;
    }

    private bool HasCurrentPreviewTabRequest => Interlocked.Read(ref previewTabRequests) != 0;
    private bool IsInDocumentTransaction => documentTransactionManager.CurrentTransaction != null;

    // Prevent opening multiple documents in the preview tab at the same time. Wait until the UI has
    // a chance to catch up. This fixes an issue with multi-file templates that want to open multiple
    // documents at the same time - and we get an exception.
    private void AddPreviewTabRequest()
    {
      Interlocked.Increment(ref previewTabRequests);
      threading.ReentrancyGuard.Queue("reset preview tab", () => Interlocked.Decrement(ref previewTabRequests));
    }
  }
}