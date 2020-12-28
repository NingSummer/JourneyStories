using System;
using TaleWorlds.Core;

namespace JourneyStories.stories
{
    public class Position2D
    {
        public float X;
        public float Y;

        public Position2D(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    public class Ticker
    {
        // 触发一次story的概率p计算：

        /*
         * p = min(1,(now-lastTick)/5*BaseTimeSec) * TimeFactor + diff(condition)* (1-TimeFactor)
         * 
         */
        private DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
        private long _lastTickInSecond;
        private long _lastCheck;
        private Position2D _lastCheckSimplePosition2D = new Position2D(0, 0);
        private Position2D _lastHitSimplePosition2D = new Position2D(0, 0);
        private float _baseConditionFactor = 1 / 3;
        private float MaxPositionX = 1;
        private float MaxPositionY = 1;

        public Ticker()
        {
            _lastTickInSecond = GetSecondNow();
            _lastCheck = _lastTickInSecond;
        }

        private long GetSecondNow()
        {
            DateTime dateCur = DateTime.Now;
            return Convert.ToInt64((dateCur - dateStart).TotalSeconds);
        }

        public bool Hit()
        {
            long now = GetSecondNow();
            if (now - _lastCheck > ModConfigs.GlobalModConfigs.CheckIntervalSecond)
            {
                _lastCheck = now;
                return true;
            }

            return false;
        }

        public bool DoubleCheckHit(Position2D position2D)
        {
            long now = GetSecondNow();
            float t1 = (now - _lastTickInSecond) / (5.0f * ModConfigs.GlobalModConfigs.BaseTimeSec);
            float p0 = Math.Min(1, t1) * ModConfigs.GlobalModConfigs.TimeFactor;
            float p1 = 0.7f * DiffCondition(position2D, _lastCheckSimplePosition2D);
            float p2 = 0.3f * DiffCondition(position2D, _lastHitSimplePosition2D);
            float p3 = (p1 + p2) * 0.5f;
            float p = p0 + p3 * (1 - ModConfigs.GlobalModConfigs.TimeFactor);
            Random rd = new Random();
            int r = rd.Next(Int32.MaxValue);
            int bound = (int) (p * Int32.MaxValue);
            bool h = r <= bound;
            if (ModConfigs.GlobalModConfigs.DebugMode)
            {
                string ss = "p0:" + p0 + " p1:" + p1 + " p2:" + p2 + " p:" + p + " maxX:" + MaxPositionX + " maxY:" +
                            MaxPositionY;
                InformationManager.DisplayMessage(new InformationMessage(ss));
            }

            if (h)
            {
                _lastHitSimplePosition2D.X = position2D.X;
                _lastHitSimplePosition2D.Y = position2D.Y;
                _lastTickInSecond = now;
            }
            else
            {
                _lastCheckSimplePosition2D.X = position2D.X;
                _lastCheckSimplePosition2D.Y = position2D.Y;
            }

            return h;
        }


        private float DiffCondition(Position2D now, Position2D last)
        {
            MaxPositionX = Math.Max(MaxPositionX, now.X);
            MaxPositionY = Math.Max(MaxPositionY, now.Y);
            if (last == null)
            {
                return 1;
            }

            float v = 0;
            float delta = Math.Abs(now.X - last.X);
            MaxPositionX = Math.Max(MaxPositionX, delta);
            v += delta / MaxPositionX;
            delta = Math.Abs(now.Y - last.Y);
            MaxPositionY = Math.Max(MaxPositionY, delta);
            v += delta / MaxPositionY;
            v *= 0.5f;
            v *= ModConfigs.GlobalModConfigs.PositionFactor;
            return Math.Min(1, v);
        }
    }
}