using ColossalFramework;
using ICities;
using UnityEngine;

namespace RainFirefighting
{
    public class FireMonitor : ThreadingExtensionBase
    {
        #region Сonstants

        private const int FireFightingRate = 1;
        private const uint FireFightingChance = 23U;

        #endregion

        #region Fields

        private readonly WeatherManager _weatherManager;
        private readonly BuildingManager _buildingManager;
        private readonly GuideManager _guideManager;
        private readonly SimulationManager _simulationManager;

        #endregion

        #region Ctor

        public FireMonitor()
        {
            _weatherManager = Singleton<WeatherManager>.instance;
            _buildingManager = Singleton<BuildingManager>.instance;
            _guideManager = Singleton<GuideManager>.instance;
            _simulationManager = Singleton<SimulationManager>.instance;
        }

        #endregion

        #region Utilities

        private void ExtinguishFire(ushort buildingId, ref Building buildingData)
        {
            var width = buildingData.Width;
            var length = buildingData.Length;
            var force = Mathf.Min(5000, buildingData.m_fireIntensity * (width * length));
            if (_simulationManager.m_randomizer.Int32(FireFightingChance) == 0)
            {
                force = Mathf.Max(force - FireFightingRate, 0) / (width * length);
                buildingData.m_fireIntensity = (byte)force;
                if (force == 0)
                {
                    var flagsOld = buildingData.m_flags;
                    if (buildingData.m_productionRate != 0)
                        buildingData.m_flags |= Building.Flags.Active;
                    var flagsNew = buildingData.m_flags;
                    _buildingManager.UpdateBuildingRenderer(buildingId, buildingData.GetLastFrameData().m_fireDamage == 0);
                    _buildingManager.UpdateBuildingColors(buildingId);
                    if (flagsNew != flagsOld)
                        _buildingManager.UpdateFlags(buildingId, flagsNew ^ flagsOld);
                    if (_guideManager.m_properties != null)
                        _buildingManager.m_buildingOnFire.Deactivate(buildingId, false);
                }
            }
        }

        #endregion

        #region Methods

        public override void OnAfterSimulationTick()
        {
            if (_weatherManager.m_currentRain < 0.1)
                return;

            for (ushort i = 0; i < _buildingManager.m_buildings.m_buffer.Length; i++)
            {
                if (_buildingManager.m_buildings.m_buffer[i].m_fireIntensity == 0)
                    continue;

                ExtinguishFire(i, ref _buildingManager.m_buildings.m_buffer[i]);
            }

            base.OnAfterSimulationTick();
        }

        #endregion
    }
}