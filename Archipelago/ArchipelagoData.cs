using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace GnosiaArchipelagoRandomizer.Archipelago
{
    public class ArchipelagoData
    {
        public string Uri;
        public string SlotName;
        public string Password;
        public int Index;

        public HashSet<long> CheckedLocations;

        /// <summary>
        /// seed for this archipelago data. Can be used when loading a file to verify the session the player is trying to
        /// load is valid to the room it's connecting to.
        /// </summary>
        private string seed;

        private Dictionary<string, object> slotData;

        public SlotDataContents SlotData
        {
            get
            {
                slotData.TryGetValue("version", out object v);
                slotData.TryGetValue("options", out object o);
                string version = Convert.ToString(v);
                OptionsContents options = null;
                if (o is JObject jo)
                {
                    options = jo.ToObject<OptionsContents>(JsonSerializer
                        .Create(new JsonSerializerSettings
                        {
                            ContractResolver = new DefaultContractResolver
                            {
                                NamingStrategy = new SnakeCaseNamingStrategy()
                            }
                        }));
                }
                return new SlotDataContents { Version = version, Options = options };
            }
        }
        public bool NeedSlotData => slotData == null;

        public record SlotDataContents
        {
            public string Version { get; set; }
            public OptionsContents Options { get; set; }
        }

        public record OptionsContents
        {
            public bool? DeathLink { get; set; }
            public List<string> ExcludeLocations { get; set; }
            public Goal? Goal { get; set; }
            public int? RequiredNotePercent { get; set; }
            public List<string> ExcludedAchievements { get; set; }
            public bool? RandomizeCharacterUnlocks { get; set; }
            public int? StartingCrewCount { get; set; }
            public bool? RandomizeRoleUnlocks { get; set; }
            public bool? RandomizeNotes { get; set; }
            public bool? RandomizeSkills { get; set; }
            public bool? AddRoleAchievementLocations { get; set; }
            public bool? AddWinWithCharacterLocations { get; set; }
            public bool? AddWinAgainstCharacterLocations { get; set; }
            public bool? AddWinAsRoleLocations { get; set; }
            public bool? AddWinAgainstRoleLocations { get; set; }
            public TutorialHandling? TutorialHandling { get; set; }
            public int? ExpMultiplier { get; set; }
            public bool? AllowGenderSpecificLogic { get; set; }
        }

        public enum Goal
        {
            NormalEnding = 0,
            RoleAchievements = 2,
        }

        public enum TutorialHandling
        {
            Vanilla = 0,
            Skip = 1,
            SkipAndRemoveLocations = 2,
        }

        public ArchipelagoData()
        {
            Uri = "localhost";
            SlotName = "Player1";
            CheckedLocations = new();
        }

        public ArchipelagoData(string uri, string slotName, string password)
        {
            Uri = uri;
            SlotName = slotName;
            Password = password;
            CheckedLocations = new();
        }

        /// <summary>
        /// assigns the slot data and seed to our data handler. any necessary setup using this data can be done here.
        /// </summary>
        /// <param name="roomSlotData">slot data of your slot from the room</param>
        /// <param name="roomSeed">seed name of this session</param>
        public void SetupSession(Dictionary<string, object> roomSlotData, string roomSeed)
        {
            if (roomSlotData != null)
                slotData = roomSlotData;
            seed = roomSeed;
        }

        public string GetSeed()
        {
            return seed;
        }

        /// <summary>
        /// returns the object as a json string to be written to a file which you can then load
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}