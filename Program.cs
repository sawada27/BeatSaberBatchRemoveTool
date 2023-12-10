using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tools.Model;

namespace Tools
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bsHelper = new SongsHelper();

            //bsHelper.DeduplicateSongs();
            bsHelper.RemovePlayListSongs(@"\Playlists\korean.bplist", @"\Beat Saber_Data\CustomLevels");
            


        }
    }




}