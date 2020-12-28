using System;
using System.Collections.Generic;
using JourneyStories.stories;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace JourneyStories
{
    public class Main : MBSubModuleBase
    {
        private StorySelector _selector = new StorySelector();
        private Ticker _ticker;
        private XmlParser _xmlParser = new XmlParser();
        private Position2D _position = new Position2D(0, 0);

        public Main()
        {
            _ticker = new Ticker();
        }

        private string getTerrainType(TerrainType type)
        {
            switch (type)
            {
                case TerrainType.Water:
                {
                    return "Water";
                }
                case TerrainType.Mountain:
                {
                    return "Mountain";
                }
                case TerrainType.Snow:
                {
                    return "Snow";
                }
                case TerrainType.Steppe:
                {
                    return "Steppe";
                }
                case TerrainType.Plain:
                {
                    return "Plain";
                }
                case TerrainType.Desert:
                {
                    return "Desert";
                }
                case TerrainType.Swamp:
                {
                    return "Swamp";
                }
                case TerrainType.Dune:
                {
                    return "Dune";
                }
                case TerrainType.Bridge:
                {
                    return "Bridge";
                }
                case TerrainType.River:
                {
                    return "River";
                }
                case TerrainType.Forest:
                {
                    return "Forest";
                }
                case TerrainType.ShallowRiver:
                {
                    return "ShallowRiver";
                }
                case TerrainType.Lake:
                {
                    return "Lake";
                }
                case TerrainType.Canyon:
                {
                    return "Canyon";
                }
                case TerrainType.RuralArea:
                {
                    return "RuralArea";
                }
                default:
                {
                    return "unKnow";
                }
            }
        }

        private void LoadStoriesData()
        {
            try
            {
                ModConfigs _modConfigs = _xmlParser.ParseModConfigs();
                ModConfigs.GlobalModConfigs.Copy(_modConfigs);
                _ticker = new Ticker();
                XmlParser parser = new XmlParser();
                List<Stories> list = parser.ParseStoriesInPath();
                InformationManager.DisplayMessage(new InformationMessage("Load Stories Count：" + list.Count));
                _selector = new StorySelector();
                _selector.Insert(list);
            }
            catch (Exception e)
            {
                InformationManager.DisplayMessage(new InformationMessage("Load Stories Error," + e));
            }
        }

        protected override void OnSubModuleLoad()
        {
            LoadStoriesData();
            if (ModConfigs.GlobalModConfigs.DebugMode)
            {
                Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Message",
                    new TextObject("测试按钮 TestMsg"),
                    9990,
                    () => { LoadStoriesData(); },
                    false));
            }
        }

        protected override void OnApplicationTick(float dt)
        {
            try
            {
                if (!_ticker.Hit())
                {
                    return;
                }

                if (!_ticker.DoubleCheckHit(GetPosition2D()))
                {
                    return;
                }

                if (ModConfigs.GlobalModConfigs.DebugMode)
                {
                    if (MobileParty.MainParty != null && Campaign.Current != null &&
                        Campaign.Current.MapSceneWrapper != null)
                    {
                        string s = "[season:" + GetSeason() + " time:" + CampaignTime.Now.GetHourOfDay.ToString() +
                                   ":00 " + GetTimeStage() +
                                   " pos:(" + MobileParty.MainParty.Position2D.x
                                   + "," + MobileParty.MainParty.Position2D.y + ") Terrain:" + getTerrainType(
                                       Campaign.Current.MapSceneWrapper.GetTerrainTypeAtPosition(MobileParty.MainParty
                                           .Position2D)) + "]";
                        InformationManager.DisplayMessage(new InformationMessage(s));
                    }
                }

                if (Campaign.Current != null)
                {
                    if (Campaign.Current.MapSceneWrapper != null && Campaign.Current != null &&
                        Campaign.Current.MapSceneWrapper != null)
                    {
                        SimpleCondition simpleCondition = GetSimpleCondition();
                        Story story = _selector.SelectOne(_selector.SelectFromCondition(simpleCondition));
                        if (story != null)
                        {
                            Color color = new Color(story.Config.StoryColor.Red, story.Config.StoryColor.Green,
                                story.Config.StoryColor.Blue, story.Config.StoryColor.Alpha);
                            InformationManager.DisplayMessage(new InformationMessage(story.Content, color));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (ModConfigs.GlobalModConfigs.DebugMode)
                {
                    InformationManager.DisplayMessage(new InformationMessage("Error" + e));
                }
            }
        }

        private SimpleCondition GetSimpleCondition()
        {
            return new SimpleCondition(GetSeason(), GetTimeStage(), GetTerrain());
        }

        private Season GetSeason()
        {
            int s = CampaignTime.Now.GetSeasonOfYear;
            switch (s)
            {
                case 0:
                {
                    return Season.Spring;
                }
                case 1:
                {
                    return Season.Summer;
                }
                case 2:
                {
                    return Season.Autumn;
                }
                case 3:
                {
                    return Season.Winter;
                }
                default:
                {
                    return Season.All;
                }
            }
        }

        private TimeStage GetTimeStage()
        {
            int v = CampaignTime.Now.GetHourOfDay;
            if (4 < v && v <= 8)
            {
                return TimeStage.Dawn;
            }
            else if (8 < v && v <= 12)
            {
                return TimeStage.Morning;
            }
            else if (12 < v && v <= 16)
            {
                return TimeStage.Noon;
            }
            else if (16 < v && v <= 20)
            {
                return TimeStage.Evening;
            }
            else if (20 < v && v <= 24)
            {
                return TimeStage.Night;
            }
            else if (0 < v && v <= 4)
            {
                return TimeStage.Midnight;
            }

            return TimeStage.All;
        }

        private Terrain GetTerrain()
        {
            TerrainType type = Campaign.Current.MapSceneWrapper.GetTerrainTypeAtPosition(MobileParty.MainParty
                .Position2D);
            switch (type)
            {
                case TerrainType.Water:
                {
                    return Terrain.Water;
                }
                case TerrainType.Mountain:
                {
                    return Terrain.Mountain;
                }
                case TerrainType.Snow:
                {
                    return Terrain.Snow;
                }
                case TerrainType.Steppe:
                {
                    return Terrain.Steppe;
                }
                case TerrainType.Plain:
                {
                    return Terrain.Plain;
                }
                case TerrainType.Desert:
                {
                    return Terrain.Desert;
                }
                case TerrainType.Swamp:
                {
                    return Terrain.Swamp;
                }
                case TerrainType.Dune:
                {
                    return Terrain.Dune;
                }
                case TerrainType.Bridge:
                {
                    return Terrain.Bridge;
                }
                case TerrainType.River:
                {
                    return Terrain.River;
                }
                case TerrainType.Forest:
                {
                    return Terrain.Forest;
                }
                case TerrainType.ShallowRiver:
                {
                    return Terrain.ShallowRiver;
                }
                case TerrainType.Lake:
                {
                    return Terrain.Lake;
                }
                case TerrainType.Canyon:
                {
                    return Terrain.Canyon;
                }
                case TerrainType.RuralArea:
                {
                    return Terrain.RuralArea;
                }
                default:
                {
                    return Terrain.All;
                }
            }
        }

        private Position2D GetPosition2D()
        {
            if (MobileParty.MainParty != null)
            {
                _position.X = MobileParty.MainParty.Position2D.x;
                _position.Y = MobileParty.MainParty.Position2D.y;
            }

            return _position;
        }
    }
}