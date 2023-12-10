using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace BeatsaberTools.Model
{
    internal class InfoData
    {
        public string _songName { get; set; }

        public long _beatsPerMinute { get; set; }

        public string _songFilename { get; set; }

        public List<BeatMapSet> _difficultyBeatmapSets { get; set; }
    }

    public class BeatMapSet
    {
        public List<DifficultyBeatMap> _difficultyBeatmaps { get; set; }
    }

    public class DifficultyBeatMap
    {
        public string _beatmapFilename { get; set; }
    }

}
