using ColossalFramework;
using ICities;

namespace RainFirefighting
{
    public class WeatherMonitor : ThreadingExtensionBase
    {
        private float _tickGrabber;
        private readonly BuildingManager _buildingManager;
        private readonly WeatherManager _weatherManager;

        public WeatherMonitor()
        {
            this._buildingManager = Singleton<BuildingManager>.instance;
            this._weatherManager = Singleton<WeatherManager>.instance;
        }

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            _tickGrabber += simulationTimeDelta;
            if (_tickGrabber > 30)
            {
                //DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, _tickGrabber.ToString());
                _tickGrabber = 0;
                if (_weatherManager.m_currentRain > 0)
                {
                    for (var i = 0; i < this._buildingManager.m_buildings.m_size; i++)
                    {
                        _buildingManager.m_buildings.m_buffer[i].m_fireIntensity = 0;
                    }
                }

            }
        }
    }

}