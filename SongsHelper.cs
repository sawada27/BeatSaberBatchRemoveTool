using BeatsaberTools.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tools.Model;

namespace Tools
{
    public class SongsHelper
    {
        private string GetHash(string customerDirectory)
        {
            var infoPath = Path.Combine(customerDirectory, "info.dat");

            var infoText = File.ReadAllText(infoPath);
            var infoByte = File.ReadAllBytes(infoPath);

            var infoData = JsonConvert.DeserializeObject<InfoData>(infoText);
            var difficultyList = new List<string>();

            foreach (var item in infoData._difficultyBeatmapSets)
            {
                if (item != null && item._difficultyBeatmaps != null)
                {
                    foreach (var difficult in item._difficultyBeatmaps)
                    {
                        if (string.IsNullOrWhiteSpace(difficult._beatmapFilename) == false)
                        {
                            difficultyList.Add(difficult._beatmapFilename);
                        }
                    }
                }
            }

            List<byte> byteSource = new List<byte>();
            byteSource.AddRange(infoByte);
            foreach (var difficultyFile in difficultyList)
            {
                var difficultyFilePath = Path.Combine(customerDirectory, difficultyFile);
                if (File.Exists(difficultyFilePath))
                {
                    var bytes = File.ReadAllBytes(difficultyFilePath);
                    byteSource.AddRange(bytes);
                }
            }

            var hash = StringSecurity.SHA1Encrypt(byteSource.ToArray());
            return hash;

        }

        /// <summary>
        /// 去重 Deduplicate
        /// </summary>
        /// <param name="customerLevelDirectory"></param>
        public void DeduplicateSongs(string customerLevelDirectory = "")
        {
            if (string.IsNullOrWhiteSpace(customerLevelDirectory))
            {
                var directoryRoot =  Directory.GetCurrentDirectory(); 
                customerLevelDirectory = Path.Combine(directoryRoot,@"\Beat Saber_Data\CustomLevels");
            }

            if (Directory.Exists(customerLevelDirectory) == false)
            {
                throw new Exception($"{customerLevelDirectory} not exits");
            }

            Dictionary<string, FolderData> uniqueFolders = new Dictionary<string, FolderData>();
            foreach (string folderPath in Directory.GetDirectories(customerLevelDirectory))
            {
                string jsonFilePath = Path.Combine(folderPath, "info.dat");
                if (File.Exists(jsonFilePath))
                {
                    string jsonContent = File.ReadAllText(jsonFilePath);
                    JObject json = JObject.Parse(jsonContent);

                    string name = json["_songName"].ToString();
                    string filename = json["_songFilename"].ToString();
                    long length = Convert.ToInt64(json["_beatsPerMinute"]);
                    string oggFilePath = Path.Combine(folderPath, filename);

                    FileInfo oggFileInfo = new FileInfo(oggFilePath);
                    long oggFileSize = oggFileInfo.Length;

                    string key = $"{name}-{length}-{oggFileSize}";
                    if (uniqueFolders.ContainsKey(key))
                    {
                        Directory.Delete(folderPath, true);
                    }
                    else
                    {
                        uniqueFolders.Add(key, new FolderData { Name = name, Length = length, FolderPath = folderPath });
                    }
                }
            }
        }

        /// <summary>
        /// 移除 RemoveByPlayList
        /// </summary>
        /// <param name="playListPath"></param>
        /// <param name="customerLevelDirectory"></param>
        public void RemovePlayListSongs(string playListPath, string customerLevelDirectory = "")
        {

            if (string.IsNullOrWhiteSpace(customerLevelDirectory))
            {
                customerLevelDirectory = @"J:\BeatSaber1.22.1\Beat Saber_Data\CustomLevels";
            }

            if (string.IsNullOrWhiteSpace(playListPath))
            {
                playListPath = @"J:\BeatSaber1.22.1\Playlists\korea.bplist";
            }

            string jsonText = File.ReadAllText(playListPath);
            var playList = JsonConvert.DeserializeObject<PlayListData>(jsonText);

            var hashList = new List<string>();
            foreach (var play in playList.songs)
            {
                hashList.Add(play.hash);
            }

            foreach (string folderPath in Directory.GetDirectories(customerLevelDirectory))
            {
                string infoDataPath = Path.Combine(folderPath, "info.dat");

                if (File.Exists(infoDataPath))
                {
                    var songHash = GetHash(folderPath);
                    if (hashList.Contains(songHash))
                    {
                        Console.WriteLine($"remove {folderPath}");
                        Directory.Delete(folderPath, true);
                    }
                }
            }
        }


    }
}
