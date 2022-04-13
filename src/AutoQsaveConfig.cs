using System;
using MoonSharp.Interpreter;
using UnityEngine;

namespace CastleStory_AutoQsave
{
    struct AutoQsaveConfig
    {
        private TimeSpan _interval;
        public TimeSpan interval
        {
            get
            {
                return _interval;
            }
        }

        private AutoQsaveType _type;
        public AutoQsaveType type
        {
            get
            {
                return _type;
            }
        }

        public AutoQsaveConfig(TimeSpan interval, AutoQsaveType type)
        {
            _interval = interval;
            _type = type;
        }

        static public AutoQsaveConfig Parse(Table table)
        {
            TimeSpan interval;
            AutoQsaveType type;

            try
            {
                var intervalData = table.Get("interval").Table;
                var intervalValue = ((int)intervalData.Get(1).Number);
                var intervalType = intervalData.Get(2).String;
                switch (intervalType)
                {
                    case "s":
                        interval = TimeSpan.FromSeconds(intervalValue);
                        break;
                    case "m":
                        interval = TimeSpan.FromMinutes(intervalValue);
                        break;
                    default:
                        Debug.Log("Incorrect interval type.");
                        interval = TimeSpan.Zero;
                        break;
                }

                var autoQsaveType = table.Get("type").String;
                type = (AutoQsaveType)Enum.Parse(typeof(AutoQsaveType), autoQsaveType);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                interval = TimeSpan.Zero;
                type = AutoQsaveType.Auto;
            }

            return new AutoQsaveConfig(interval, type);
        }
    }
}
