namespace HonKaiImpact.Structures
{
	public struct HerrscherCoreStatus()
	{
		public void Reset(bool InbResetValue = false)
		{
			bOrigin = InbResetValue;
			bReason = InbResetValue;
			bVoid = InbResetValue;
			bThunder = InbResetValue;
			bWind = InbResetValue;
			bIce = InbResetValue;
			bDeath = InbResetValue;
			bFire = InbResetValue;
			bSentience = InbResetValue;
			bEarth = InbResetValue;
			bLegion = InbResetValue;
			bBinding = InbResetValue;
			bCorruption = InbResetValue;
			bFinality = InbResetValue;
		}

		public bool bOrigin = false;
		public bool bReason = false;
		public bool bVoid = false;
		public bool bThunder = false;
		public bool bWind = false;
		public bool bIce = false;
		public bool bDeath = false;
		public bool bFire = false;
		public bool bSentience = false;
		public bool bEarth = false;
		public bool bLegion = false;
		public bool bBinding = false;
		public bool bCorruption = false;
		public bool bFinality = false;
	}
}
