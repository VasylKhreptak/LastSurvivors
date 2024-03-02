﻿namespace Infrastructure.Data.Persistent
{
    public class PersistentData
    {
        public readonly PlayerData PlayerData = new PlayerData();
        public readonly AnalyticsData AnalyticsData = new AnalyticsData();
        public readonly Settings Settings = new Settings();
    }
}