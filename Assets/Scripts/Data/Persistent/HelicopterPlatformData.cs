﻿using Plugins.Banks;

namespace Data.Persistent
{
    public class HelicopterPlatformData
    {
        public readonly HelicopterData HelicopterData = new HelicopterData();
        public readonly ClampedIntegerBank UpgradeContainer = new ClampedIntegerBank(0, 100);
    }
}