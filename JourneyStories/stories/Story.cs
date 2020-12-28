﻿using System.Collections.Generic;

namespace JourneyStories.stories
{
    public class StoryColor
    {
        public float Red;
        public float Green;
        public float Blue;
        public float Alpha;

        public StoryColor(float read, float green, float blue)
        {
            Red = read;
            Green = green;
            Blue = blue;
        }

        public StoryColor(float read, float green, float blue, float alpha)
        {
            Red = read;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }
    }

    // Time stage , each time stage is 4 hours of a day
    public enum TimeStage
    {
        // 00:00 ~ 00:00
        All,

        // 04:00 ~ 08:00
        Dawn,

        // 08:00 ~ 12:00
        Morning,

        // 12:00 ~ 16:00
        Noon,

        // 16:00 ~ 20:00
        Evening,

        // 20:00 ~ 24:00
        Night,

        // 00:00 ~ 04:00
        Midnight
    }

    public enum Season
    {
        All,
        Spring,
        Summer,
        Autumn,
        Winter
    }

    public enum Terrain
    {
        All,
        Water,
        Mountain,
        Snow,
        Steppe,
        Plain,
        Desert,
        Swamp,
        Dune,
        Bridge,
        River,
        Forest,
        ShallowRiver,
        Lake,
        Canyon,
        RuralArea,
    }

    // Condition of when and where the story happen
    public class Condition
    {
        // Season show which season the story can happen.
        public HashSet<Season> Seasons;

        // TimeStage show when the story can happen in a day.
        public HashSet<TimeStage> TimeStages;

        // Terrain show where the story can happen.
        public HashSet<Terrain> Terrains;

        public Condition()
        {
            Seasons = new HashSet<Season>();
            TimeStages = new HashSet<TimeStage>();
            Terrains = new HashSet<Terrain>();
        }

        // If some conditions are not set, will set default
        public void Check()
        {
            if (Seasons.Count == 0)
            {
                Seasons.Add(Season.All);
            }

            if (TimeStages.Count == 0)
            {
                TimeStages.Add(TimeStage.All);
            }

            if (Terrains.Count == 0)
            {
                Terrains.Add(Terrain.All);
            }
        }
    }

    public class Config
    {
        // Priority to show, value is from 0~10, 0 lowest, 10 highest.
        public int Priority = 5;

        // Whether this story can repeat showed.
        public bool Repeat = true;

        // Color
        public StoryColor StoryColor;

        // Whether play a music( todo )
        public string Music;

        public Config()
        {
            StoryColor = new StoryColor(DefaultModConfigs.DefaultRed, DefaultModConfigs.DefaultGreen,
                DefaultModConfigs.DefaultBlue, DefaultModConfigs.DefaultAlpha);
        }
    }

    public class Story
    {
        public int Id;

        // The content to show in message
        public string Content;

        // Last hit
        private bool _hit = false;

        // Config for each story
        public Config Config;

        // 私有优先级随着每次命中不断减少，可以降低故事重复出现的概率
        private int _selfPriority = 5;

        public Story(string content)
        {
            Content = content;
        }

        public void SetConfig(Config config)
        {
            if (config == null)
            {
                return;
            }

            Config = config;
            _selfPriority = config.Priority;
        }

        private void InitSelfPriority()
        {
            _selfPriority = Config == null ? 5 : Config.Priority == 0 ? 1 : Config.Priority;
        }

        public int GetSelfPriority()
        {
            if (Config != null && !Config.Repeat && _hit)
            {
                return 0;
            }

            return _selfPriority;
        }

        public void Hit()
        {
            _hit = true;
            if (_selfPriority > 1)
            {
                _selfPriority--;
            }
            else
            {
                InitSelfPriority();
            }
        }
    }

    public class Stories
    {
        public Condition Condition;
        public Config Config;
        public List<Story> StoryList;

        public Stories()
        {
            Condition = new Condition();
            Config = new Config();
            StoryList = new List<Story>();
        }

        public void Check()
        {
            Condition.Check();
            foreach (Story story in StoryList)
            {
                story.SetConfig(Config);
            }
        }
    }
}