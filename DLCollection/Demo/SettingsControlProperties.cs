using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
