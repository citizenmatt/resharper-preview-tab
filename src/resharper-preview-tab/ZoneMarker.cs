﻿/*
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

using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.Platform.VisualStudio.SinceVs11.Shell.Zones;

namespace CitizenMatt.ReSharper.PreviewTab
{
    [ZoneMarker]
    public class ZoneMarker : IRequire<ISinceVs11Zone>
    {
    }
}