using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.Platform.VisualStudio.SinceVs11.Shell.Zones;

namespace CitizenMatt.ReSharper.PreviewTab
{
    [ZoneMarker]
    public class ZoneMarker : IRequire<ISinceVs11Zone>
    {
    }
}