using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Model
{
    internal class PlayListData
    {
        public string playlistTitle { get; set; }

        public string playlistAuthor { get; set; }

        public string PlaylistDescription { get; set; }

        public List<Songs> songs { get; set; }
    }

    public class Songs
    {
        public string songName { get; set; }

        public string hash { get; set; }

        public string levelid { get; set; }

    }
}
