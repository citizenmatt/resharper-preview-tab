/*
 * Copyright 2012 - 2015 Matt Ellis
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

using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.VsIntegration.SinceVs11.IDE;

namespace CitizenMatt.ReSharper.PreviewTab
{
  public partial class PreviewTabEditorManager : EditorManagerSinceVs11
  {
    public override ITextControl OpenProjectFile(IProjectFile projectFile, bool activate, FileView fileViewPrimary,
                                                 TabOptions tabOptions = TabOptions.Default)
    {
      return OverrideOpenProjectFile(projectFile, activate, fileViewPrimary, tabOptions);
    }

    public override ITextControl OpenFile(FileSystemPath fileName, bool activate, TabOptions tabOptions)
    {
      return OverrideOpenFile(fileName, activate, tabOptions);
    }
  }
}

namespace JetBrains.VsIntegration.ProjectDocuments.Projects.Builder
{
}
