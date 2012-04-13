using System;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation.PlatformStrategy
{
	public class PlatformStrategyFactory
	{
        public IPlatformStrategy Create(RuntimePlatform runtimePlatform)
        {
            switch (runtimePlatform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsWebPlayer:
                    return new WindowsPlatformStrategy();
                case RuntimePlatform.OSXDashboardPlayer:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXWebPlayer:
                case RuntimePlatform.IPhonePlayer:
                    return new OSPlatformStrategy();
                case RuntimePlatform.Android:
                    return new AndroidPlatformStrategy();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
