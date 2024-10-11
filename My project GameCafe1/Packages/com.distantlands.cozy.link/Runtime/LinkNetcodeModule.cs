#if LINK_NETCODE
using System.Collections.Generic;
using System;
using UnityEngine;
using DistantLands.Cozy.Data;
using Unity.Netcode;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DistantLands.Cozy
{
    [RequireComponent(typeof(NetcodeView))]
    [RequireComponent(typeof(NetworkObject))]
    public class LinkNetcodeModule : CozyModule
    {

        public bool linkTime = true;
        public bool linkWeather = true;
        public bool linkAmbience = true;
        [Tooltip("Controls the amount of time (in seconds) before an RPC is sent to the server to sync the COZY systems.")]
        [Range(0, 6)]
        public float updateDelay = 0.5f;
        [Tooltip("Controls the amount of ticks away from the main server a client has to be before resyncing with the server. (Default: 2)")]
        [Range(0, 15)]
        public float timeSettingSensitivity = 2;
        float currentDelay;

        public Dictionary<WeatherProfile, int> weatherHashes = new Dictionary<WeatherProfile, int>();
        public Dictionary<AmbienceProfile, int> ambienceHashes = new Dictionary<AmbienceProfile, int>();
        public CozyAmbienceModule ambienceManager;

        public bool isMaster { get { return netcodeView.NetworkManager.IsServer || netcodeView.NetworkManager.IsHost; } }

        //VARIABLES______________________________________________________________________________________________________
        private NetcodeView nvCache;


        //FUNCTIONS_______________________________________________________________________________________________________
        public void Update()
        {

            if (currentDelay <= 0)
            {
                if (isMaster)
                    MasterCommunication();
                else
                    ClientCommunication();
                currentDelay = updateDelay;
            }
            else
                currentDelay -= Time.deltaTime;

        }

        public NetcodeView netcodeView
        {
            get
            {
                if (nvCache)
                {

                    return nvCache;

                }
                else
                {
                    nvCache = GetComponent<NetcodeView>();
                    return nvCache;
                }

            }
        }


        public void SyncCozyTime()
        {

            if (Mathf.Abs(netcodeView.ticks.Value - weatherSphere.timeModule.currentTime) > timeSettingSensitivity)
                weatherSphere.timeModule.currentTime = netcodeView.ticks.Value;
            weatherSphere.timeModule.currentDay = netcodeView.day.Value;
            weatherSphere.timeModule.currentYear = netcodeView.year.Value;

        }


        public void SyncCozyAmbience()
        {

            ambienceManager.currentAmbienceProfile = ambienceManager.ambienceProfiles[netcodeView.ambience.Value];

        }


        public void SyncCozyWeather()
        {

            List<WeatherRelation> i = new List<WeatherRelation>();
            int l = 0;

            foreach (int j in netcodeView.weatherCache)
            {

                WeatherRelation k = new WeatherRelation();
                k.profile = weatherSphere.weatherModule.ecosystem.forecastProfile.profilesToForecast[j];
                k.weight = netcodeView.weatherValues[l];
                i.Add(k);
                l++;

            }
            weatherSphere.weatherModule.ecosystem.weightedWeatherProfiles = i;

        }

        public void SyncCozyTimeMaster()
        {

            netcodeView.ticks.Value = weatherSphere.timeModule.currentTime;
            netcodeView.day.Value = weatherSphere.timeModule.currentDay;
            netcodeView.year.Value = weatherSphere.timeModule.currentYear;

        }

        public void SyncCozyAmbienceMaster()
        {
            
            netcodeView.ambience.Value = ambienceHashes[ambienceManager.currentAmbienceProfile];

        }

        public void SyncCozyWeatherMaster()
        {

            netcodeView.weatherCache.Clear();

            foreach (int i in GetWeatherIDs())
                netcodeView.weatherCache.Add(i);

            netcodeView.weatherValues.Clear();

            foreach (float i in GetWeatherIntensities())
                netcodeView.weatherValues.Add(i);

        }

        private void MasterCommunication()
        {


            if (linkTime) SyncCozyTimeMaster();
            if (linkAmbience) SyncCozyAmbienceMaster();
            if (linkWeather) SyncCozyWeatherMaster();

        }

        private void ClientCommunication()
        {


            if (linkTime) SyncCozyTime();
            if (linkAmbience) SyncCozyAmbience();
            if (linkWeather) SyncCozyWeather();

        }

        public override void InitializeModule()
        {

            base.InitializeModule();

            for (int i = 0; i < weatherSphere.weatherModule.ecosystem.forecastProfile.profilesToForecast.Count; i++)
            {
                weatherHashes.Add(weatherSphere.weatherModule.ecosystem.forecastProfile.profilesToForecast[i], i);
            }


            if (linkAmbience)
            {

                if (weatherSphere.GetModule<CozyAmbienceModule>() == null)
                {
                    linkAmbience = false;
                }
                else
                {
                    ambienceManager = weatherSphere.GetModule<CozyAmbienceModule>();

                    for (int i = 0; i < ambienceManager.ambienceProfiles.Length; i++)
                    {
                        ambienceHashes.Add(ambienceManager.ambienceProfiles[i], i);
                    }

                }


            }

        }

        public int[] GetWeatherIDs()
        {

            List<int> i = new List<int>();

            foreach (WeatherRelation j in weatherSphere.weatherModule.ecosystem.weightedWeatherProfiles)
            {
                i.Add(weatherHashes[j.profile]);
            }

            return i.ToArray();

        }


        public float[] GetWeatherIntensities()
        {

            List<float> i = new List<float>();

            foreach (WeatherRelation j in weatherSphere.weatherModule.ecosystem.weightedWeatherProfiles)
            {
                i.Add(j.weight);
            }

            return i.ToArray();

        }


    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LinkNetcodeModule))]
    [CanEditMultipleObjects]
    public class E_LinkNetcodeModule : E_CozyModule
    {


        public override GUIContent GetGUIContent()
        {

            //Place your module's GUI content here.
            return new GUIContent("    Netcode Link", (Texture)Resources.Load("Link"), "Controls COZY for use in a multiplayer environment using Netcode.");

        }

        void OnEnable()
        {

        }

        public override void DisplayInCozyWindow()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("updateDelay"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("timeSettingSensitivity"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("linkTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("linkWeather"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("linkAmbience"));

            serializedObject.ApplyModifiedProperties();

        }

    }
#endif
}

#endif