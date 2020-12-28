using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace JourneyStories.stories
{
    public class SimpleCondition
    {
        public Season Season;
        public TimeStage TimeStage;
        public Terrain Terrain;

        public SimpleCondition(Season season, TimeStage timeStage, Terrain terrain)
        {
            Season = season;
            TimeStage = timeStage;
            Terrain = terrain;
        }
    }

    public class StorySelector
    {
        private SelectEngine<Story> _selectEngine;
        private MapTable<Season> _seasonMap;
        private MapTable<TimeStage> _timeStageMap;
        private MapTable<Terrain> _terrainMap;

        public StorySelector()
        {
            _selectEngine = new SelectEngine<Story>();
            initSeasons();
            initTimeStage();
            initTerrain();
        }

        private void initSeasons()
        {
            List<Season> seasons = new List<Season>();
            seasons.Add(Season.All);
            seasons.Add(Season.Spring);
            seasons.Add(Season.Summer);
            seasons.Add(Season.Autumn);
            seasons.Add(Season.Winter);
            _seasonMap = _selectEngine.RegisterCondition(seasons);
        }

        private void initTimeStage()
        {
            List<TimeStage> timeStages = new List<TimeStage>();
            timeStages.Add(TimeStage.All);
            timeStages.Add(TimeStage.Dawn);
            timeStages.Add(TimeStage.Morning);
            timeStages.Add(TimeStage.Noon);
            timeStages.Add(TimeStage.Evening);
            timeStages.Add(TimeStage.Night);
            timeStages.Add(TimeStage.Midnight);
            _timeStageMap = _selectEngine.RegisterCondition(timeStages);
        }

        private void initTerrain()
        {
            List<Terrain> terrains = new List<Terrain>();
            terrains.Add(Terrain.All);
            terrains.Add(Terrain.Water);
            terrains.Add(Terrain.Mountain);
            terrains.Add(Terrain.Snow);
            terrains.Add(Terrain.Steppe);
            terrains.Add(Terrain.Plain);
            terrains.Add(Terrain.Desert);
            terrains.Add(Terrain.Swamp);
            terrains.Add(Terrain.Dune);
            terrains.Add(Terrain.Bridge);
            terrains.Add(Terrain.River);
            terrains.Add(Terrain.Forest);
            terrains.Add(Terrain.ShallowRiver);
            terrains.Add(Terrain.Lake);
            terrains.Add(Terrain.Canyon);
            terrains.Add(Terrain.RuralArea);
            _terrainMap = _selectEngine.RegisterCondition(terrains);
        }

        public List<Story> SelectFromCondition(SimpleCondition condition)
        {
            List<Story> result = _selectEngine.StartSelect().FindInNextCondition(_seasonMap.GetID(condition.Season))
                .FindInCondition(_seasonMap.GetID(Season.All))
                .FindInNextCondition(_timeStageMap.GetID(condition.TimeStage))
                .FindInCondition(_timeStageMap.GetID(TimeStage.All))
                .FindInNextCondition(_terrainMap.GetID(condition.Terrain))
                .FindInCondition(_terrainMap.GetID(Terrain.All))
                .GetSelectedData();
            return result;
        }

        // Select one story from list by priority
        public Story SelectOne(List<Story> list)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            if (list.Count == 1)
            {
                return list[0];
            }

            int all = 0;
            foreach (Story story in list)
            {
                all += story.GetSelfPriority() * 10;
            }

            Random rd = new Random(DateTime.Now.Millisecond);
            int r = rd.Next(all);

            if (ModConfigs.GlobalModConfigs.DebugMode)
            {
                string s = "[selector list:" + list.Count + " all:" + all + " r:" + r + "]";
                InformationManager.DisplayMessage(new InformationMessage(s));
            }

            all = 0;
            foreach (Story story in list)
            {
                all += story.GetSelfPriority() * 10;
                if (r < all)
                {
                    story.Hit();
                    return story;
                }
            }

            return null;
        }

        public void Insert(List<Stories> list)
        {
            foreach (Stories stories in list)
            {
                foreach (Season season in stories.Condition.Seasons)
                {
                    Node<Story> root = _selectEngine.AddCondition(_seasonMap.GetID(season));
                    foreach (TimeStage timeStage in stories.Condition.TimeStages)
                    {
                        Node<Story> n = _selectEngine.AddCondition(root, _timeStageMap.GetID(timeStage));
                        foreach (Terrain terrain in stories.Condition.Terrains)
                        {
                            Node<Story> bottom = _selectEngine.AddCondition(n, _terrainMap.GetID(terrain));
                            bottom.AddData(stories.StoryList);
                        }
                    }
                }
            }
        }
    }
}