using System.Drawing;

namespace DLCollection.Demo
{
    public class SettingsControlProperties
    {
        public enum Cascade { None, Default, Alt1, Alt2 }
        public Cascade CascadeType { get; set; } = Cascade.None;

        public enum Source { None, Capture, Browse }
        public Source SourceType { get; set; } = Source.None;
        public string BrowsePath { get; set; }

        public double ScaleFactor { get; set; }
        public int MinNeighbors { get; set; }

        public Size MinSize { get; set; }
        public Size MaxSize { get; set; }

        public SettingsControlProperties() { }
        public SettingsControlProperties(SettingsControlProperties settings)
        {
            CascadeType = settings.CascadeType;

            SourceType = settings.SourceType;
            BrowsePath = settings.BrowsePath;

            ScaleFactor = settings.ScaleFactor;
            MinNeighbors = settings.MinNeighbors;

            MinSize = settings.MinSize;
            MaxSize = settings.MaxSize;
        }

        public SettingsControlProperties Copy()
        {
            return new SettingsControlProperties(this);
        }
    }
}
