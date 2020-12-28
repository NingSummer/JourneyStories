using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace JourneyStories.stories
{
    class XmlParser
    {
        private const string StoriesPack = "StoriesPack";
        private const string StoriesStr = "Stories";
        private const string ConditionStr = "Condition";
        private const string ConfigStr = "Config";
        private const string SeasonsStr = "Seasons";
        private const string SeasonStr = "Season";
        private const string TimeStagesStr = "TimeStages";
        private const string TimeStageStr = "TimeStage";
        private const string TerrainsStr = "Terrains";
        private const string TerrainStr = "Terrain";
        private const string StoryListStr = "StoryList";
        private const string StoryStr = "Story";
        private const string ContentStr = "Content";


        // 00:00 ~ 00:00
        private const string All = "All";

        // 04:00 ~ 08:00
        private const string Dawn = "Dawn";

        // 08:00 ~ 12:00
        private const string Morning = "Morning";

        // 12:00 ~ 16:00
        private const string Noon = "Noon";

        // 16:00 ~ 20:00
        private const string Evening = "Evening";

        // 20:00 ~ 00:00
        private const string Night = "Night";

        // 00:00 ~ 04:00
        private const string Midnight = "Midnight";

        private const string Spring = "Spring";
        private const string Summer = "Summer";
        private const string Autumn = "Autumn";
        private const string Winter = "Winter";

        private const string Water = "Water";
        private const string Mountain = "Mountain";
        private const string Snow = "Snow";
        private const string Steppe = "Steppe";
        private const string Plain = "Plain";
        private const string Desert = "Desert";
        private const string Swamp = "Swamp";
        private const string Dune = "Dune";
        private const string Bridge = "Bridge";
        private const string River = "River";
        private const string Forest = "Forest";
        private const string ShallowRiver = "ShallowRiver";
        private const string Lake = "Lake";
        private const string Canyon = "Canyon";
        private const string RuralArea = "RuralArea";

        private const string Priority = "Priority";
        private const string Repeat = "Repeat";
        private const string Color = "Color";
        private const string Music = "Music";

        private const string Red = "Red";
        private const string Green = "Green";
        private const string Blue = "Blue";
        private const string Alpha = "Alpha";

        public List<Stories> ParseStoriesInPath()
        {
            string current = System.IO.Directory.GetCurrentDirectory();
            String prefix = current.Substring(0, current.LastIndexOf("bin"));
            string path = prefix + "Modules/JourneyStories/ModuleData/stories/" + ModConfigs.GlobalModConfigs.Language;
            List<Stories> list = new List<Stories>();
            DirectoryInfo root = new DirectoryInfo(@path);
            FileInfo[] files = root.GetFiles();
            foreach (FileInfo fileInfo in files)
            {
                List<Stories> l = Parse(path + "/" + fileInfo.Name);
                if (l != null)
                {
                    foreach (Stories stories in l)
                    {
                        list.Add(stories);
                    }
                }
            }

            return list;
        }

        private List<Stories> Parse(string file)
        {
            XmlReader reader = null;
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                reader = XmlReader.Create(@file, settings);
                doc.Load(reader);
                List<Stories> list = new List<Stories>();
                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.Name.Equals(StoriesPack))
                    {
                        foreach (XmlNode n in node.ChildNodes)
                        {
                            if (n.Name.Equals(StoriesStr))
                            {
                                Stories stories = ParseStories(n);
                                if (stories != null)
                                {
                                    list.Add(stories);
                                }
                            }
                        }
                    }
                }

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private Stories ParseStories(XmlNode node)
        {
            try
            {
                Stories stories = new Stories();
                foreach (XmlNode n in node.ChildNodes)
                {
                    switch (n.Name)
                    {
                        case ConditionStr:
                        {
                            ParseCondition(n, stories.Condition);
                            break;
                        }
                        case ConfigStr:
                        {
                            ParseConfig(n, stories.Config);
                            break;
                        }
                        case StoryListStr:
                        {
                            ParseStoryList(n, stories.StoryList);
                            break;
                        }
                    }
                }

                stories.Check();

                return stories;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private void ParseConfig(XmlNode node, Config config)
        {
            try
            {
                foreach (XmlNode n in node.ChildNodes)
                {
                    XmlElement xe = (XmlElement) n;
                    String value = xe.GetAttribute("Value");

                    switch (xe.Name)
                    {
                        case Priority:
                        {
                            config.Priority = int.Parse(value);
                            break;
                        }
                        case Repeat:
                        {
                            config.Repeat = "true".Equals(value);
                            break;
                        }
                        case Music:
                        {
                            config.Music = value;
                            break;
                        }
                        case Color:
                        {
                            ParseColor(n, config.StoryColor);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string ParseNodeString(XmlElement xe, string key, string defaultValue)
        {
            string v = xe.GetAttribute(key);
            if (v.Length == 0)
            {
                return defaultValue;
            }

            return v;
        }

        private float ParseNodeFloat(XmlElement xe, string key, float defaultValue)
        {
            string v = xe.GetAttribute(key);
            if (v.Length == 0)
            {
                return defaultValue;
            }

            return float.Parse(v);
        }

        private bool ParseNodeBool(XmlElement xe, string key, bool defaultValue)
        {
            string v = xe.GetAttribute(key);
            if (v.Length == 0)
            {
                return defaultValue;
            }

            return bool.Parse(v);
        }

        private void ParseColor(XmlNode node, StoryColor color)
        {
            XmlElement xe = (XmlElement) node;
            color.Red = ParseNodeFloat(xe, "Red", DefaultModConfigs.DefaultRed);
            color.Green = ParseNodeFloat(xe, "Green", DefaultModConfigs.DefaultGreen);
            color.Blue = ParseNodeFloat(xe, "Blue", DefaultModConfigs.DefaultBlue);
            color.Alpha = ParseNodeFloat(xe, "Alpha", DefaultModConfigs.DefaultAlpha);
        }

        private void ParseStoryList(XmlNode node, List<Story> list)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name.Equals(StoryStr))
                {
                    XmlElement xe = (XmlElement) n;
                    String value = xe.GetAttribute("Value");
                    if (value.Length != 0)
                    {
                        list.Add(new Story(value));
                    }
                }
            }
        }

        private void ParseCondition(XmlNode node, Condition condition)
        {
            try
            {
                foreach (XmlNode n in node.ChildNodes)
                {
                    switch (n.Name)
                    {
                        case SeasonsStr:
                        {
                            ParseSeasons(n, condition.Seasons);
                            break;
                        }
                        case TimeStagesStr:
                        {
                            ParseTimeStages(n, condition.TimeStages);
                            break;
                        }
                        case TerrainsStr:
                        {
                            ParseTerrains(n, condition.Terrains);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void ParseSeasons(XmlNode node, HashSet<Season> seasons)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                XmlElement xe = (XmlElement) n;
                String value = xe.GetAttribute("Value");
                if (xe.Name.Equals(SeasonStr))
                {
                    switch (value)
                    {
                        case All:
                        {
                            seasons.Add(Season.All);
                            break;
                        }
                        case Spring:
                        {
                            seasons.Add(Season.Spring);
                            break;
                        }
                        case Summer:
                        {
                            seasons.Add(Season.Summer);
                            break;
                        }
                        case Autumn:
                        {
                            seasons.Add(Season.Autumn);
                            break;
                        }
                        case Winter:
                        {
                            seasons.Add(Season.Winter);
                            break;
                        }
                    }
                }
            }
        }

        private void ParseTimeStages(XmlNode node, HashSet<TimeStage> timeStages)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                XmlElement xe = (XmlElement) n;
                String value = xe.GetAttribute("Value");
                if (xe.Name.Equals(TimeStageStr))
                {
                    switch (value)
                    {
                        case All:
                        {
                            timeStages.Add(TimeStage.All);
                            break;
                        }
                        case Dawn:
                        {
                            timeStages.Add(TimeStage.Dawn);
                            break;
                        }
                        case Morning:
                        {
                            timeStages.Add(TimeStage.Morning);
                            break;
                        }
                        case Noon:
                        {
                            timeStages.Add(TimeStage.Noon);
                            break;
                        }
                        case Evening:
                        {
                            timeStages.Add(TimeStage.Evening);
                            break;
                        }
                        case Night:
                        {
                            timeStages.Add(TimeStage.Night);
                            break;
                        }
                        case Midnight:
                        {
                            timeStages.Add(TimeStage.Midnight);
                            break;
                        }
                    }
                }
            }
        }

        private void ParseTerrains(XmlNode node, HashSet<Terrain> terrains)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                XmlElement xe = (XmlElement) n;
                String value = xe.GetAttribute("Value");
                if (xe.Name.Equals(TerrainStr))
                {
                    switch (value)
                    {
                        case All:
                        {
                            terrains.Add(Terrain.All);
                            break;
                        }
                        case Water:
                        {
                            terrains.Add(Terrain.Water);
                            break;
                        }
                        case Mountain:
                        {
                            terrains.Add(Terrain.Mountain);
                            break;
                        }
                        case Snow:
                        {
                            terrains.Add(Terrain.Snow);
                            break;
                        }
                        case Steppe:
                        {
                            terrains.Add(Terrain.Steppe);
                            break;
                        }
                        case Plain:
                        {
                            terrains.Add(Terrain.Plain);
                            break;
                        }
                        case Desert:
                        {
                            terrains.Add(Terrain.Desert);
                            break;
                        }
                        case Swamp:
                        {
                            terrains.Add(Terrain.Swamp);
                            break;
                        }
                        case Dune:
                        {
                            terrains.Add(Terrain.Dune);
                            break;
                        }
                        case Bridge:
                        {
                            terrains.Add(Terrain.Bridge);
                            break;
                        }
                        case River:
                        {
                            terrains.Add(Terrain.River);
                            break;
                        }
                        case Forest:
                        {
                            terrains.Add(Terrain.Forest);
                            break;
                        }
                        case ShallowRiver:
                        {
                            terrains.Add(Terrain.ShallowRiver);
                            break;
                        }
                        case Lake:
                        {
                            terrains.Add(Terrain.Lake);
                            break;
                        }
                        case Canyon:
                        {
                            terrains.Add(Terrain.Canyon);
                            break;
                        }
                        case RuralArea:
                        {
                            terrains.Add(Terrain.RuralArea);
                            break;
                        }
                    }
                }
            }
        }

        public ModConfigs ParseModConfigs()
        {
            ModConfigs modConfigs = new ModConfigs();
            string current = System.IO.Directory.GetCurrentDirectory();
            String prefix = current.Substring(0, current.LastIndexOf("bin"));
            string path = prefix + "Modules/JourneyStories/ModuleData/config";
            DirectoryInfo root = new DirectoryInfo(@path);
            FileInfo[] files = root.GetFiles();
            foreach (FileInfo fileInfo in files)
            {
                if (fileInfo.Name.Equals("mod_config.xml"))
                {
                    XmlReader reader = null;
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.IgnoreComments = true;
                        reader = XmlReader.Create(@path + "/mod_config.xml", settings);
                        doc.Load(reader);
                        foreach (XmlNode node in doc.ChildNodes)
                        {
                            if (node.Name.Equals("ModConfig"))
                            {
                                foreach (XmlNode n in node.ChildNodes)
                                {
                                    if (n.Name.Equals("Config"))
                                    {
                                        XmlElement xe = (XmlElement) n;
                                        modConfigs.DebugMode =
                                            ParseNodeBool(xe, "DebugMode", DefaultModConfigs.DebugMode);
                                        modConfigs.BaseTimeSec =
                                            ParseNodeFloat(xe, "BaseTimeSec", DefaultModConfigs.BaseTimeSec);
                                        modConfigs.TimeFactor =
                                            ParseNodeFloat(xe, "TimeFactor", DefaultModConfigs.TimeFactor);
                                        modConfigs.CheckIntervalSecond =
                                            ParseNodeFloat(xe, "CheckIntervalSecond",
                                                DefaultModConfigs.CheckIntervalSecond);
                                        modConfigs.Language =
                                            ParseNodeString(xe, "Language", DefaultModConfigs.Language);
                                        return modConfigs;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }
                }
            }

            return modConfigs;
        }
    }
}