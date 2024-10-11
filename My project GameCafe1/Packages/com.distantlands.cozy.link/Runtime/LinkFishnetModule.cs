#if LINK_FISHNET
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DistantLands.Cozy.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif
using FishNet.Object;

namespace DistantLands.Cozy
{
    [RequireComponent(typeof(FishnetView))]
    [RequireComponent(typeof(NetworkObject))]
    public class LinkFishnetModule : CozyModule
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

        public CozyAmbienceModule ambienceManager;

        public bool isMaster { get { return fishnetView.isMaster; } }

        //VARIABLES______________________________________________________________________________________________________
        private FishnetView nvCache;


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

        public FishnetView fishnetView
        {
            get
            {
                if (nvCache != null)
                {

                    return nvCache;

                }
                else
                {
                    nvCache = GetComponent<FishnetView>();
                    return nvCache;
                }

            }
        }




        public void SyncCozyTime()
        {

            if (Mathf.Abs(fishnetView.time.Value - weatherSphere.dayPercentage) > timeSettingSensitivity)
                weatherSphere.timeModule.currentTime = fishnetView.time.Value;
            weatherSphere.timeModule.currentDay = fishnetView.day.Value;
            weatherSphere.timeModule.currentYear = fishnetView.year.Value;

        }


        public void SyncCozyAmbience()
        {

            ambienceManager.currentAmbienceProfile = ambienceManager.ambienceProfiles[fishnetView.ambience.Value];

        }


        public void SyncCozyWeather()
        {

            weatherSphere.weatherModule.ecosystem.weightedWeatherProfiles.Clear();
            int l = 0;
            int[] weatherCache = PollValues(fishnetView.weatherCacheString.Value);

            foreach (int j in weatherCache)
            {

                WeatherRelation k = new WeatherRelation();
                k.profile = weatherSphere.weatherModule.ecosystem.forecastProfile.profilesToForecast[j];
                k.weight = PollWeightValues(fishnetView.weatherValuesString.Value)[l];
                weatherSphere.weatherModule.ecosystem.weightedWeatherProfiles.Add(k);
                l++;

            }
        }

        public void SyncCozyTimeMaster()
        {

            fishnetView.time.Value = weatherSphere.timeModule.currentTime;
            fishnetView.day.Value = weatherSphere.timeModule.currentDay;
            fishnetView.year.Value = weatherSphere.timeModule.currentYear;

        }

        public void SyncCozyAmbienceMaster()
        {

            List<AmbienceProfile> ambiences = ambienceManager.ambienceProfiles.ToList();
            fishnetView.ambience.Value = ambiences.IndexOf(ambienceManager.currentAmbienceProfile);

        }

        public void SyncCozyWeatherMaster()
        {

            string weatherIDString = "";

            foreach (int i in GetWeatherIDs())
                weatherIDString = $"{weatherIDString}{i},";

            fishnetView.weatherCacheString.Value = weatherIDString;

            weatherIDString = "";

            foreach (float i in GetWeatherIntensities())
                weatherIDString = $"{weatherIDString}{i},";

            fishnetView.weatherValuesString.Value = weatherIDString;

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

            if (linkAmbience)
            {
                if (weatherSphere.GetModule<CozyAmbienceModule>() == null)
                {
                    linkAmbience = false;
                }
                else
                {
                    ambienceManager = weatherSphere.GetModule<CozyAmbienceModule>();
                }
            }
        }

        public int[] GetWeatherIDs()
        {

            List<int> i = new List<int>();

            foreach (WeatherRelation j in weatherSphere.weatherModule.ecosystem.weightedWeatherProfiles)
            {
                i.Add(weatherSphere.weatherModule.ecosystem.forecastProfile.profilesToForecast.IndexOf(j.profile));
            }

            return i.ToArray();

        }

        public int[] PollValues(string key)
        {

            char comma = ",".ToCharArray()[0];
            string[] valuesInStringFormat = key.Split(comma);
            List<int> i = new List<int>();

            foreach (string k in valuesInStringFormat)
            {

                if (k == "")
                    continue;

                string trimmed = k.TrimEnd(comma);
                int parsed = int.Parse(trimmed);
                i.Add(parsed);
            }

            return i.ToArray();

        }

        public float[] PollWeightValues(string key)
        {

            char comma = ",".ToCharArray()[0];
            string[] valuesInStringFormat = key.Split(comma);
            List<float> i = new List<float>();

            foreach (string k in valuesInStringFormat)
            {
                if (k == "")
                    continue;
                i.Add(float.Parse(k.TrimEnd(comma)));
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
    [CustomEditor(typeof(LinkFishnetModule))]
    [CanEditMultipleObjects]
    public class E_LinkFishnetModule : E_CozyModule
    {

        public override GUIContent GetGUIContent()
        {
            //Place your module's GUI content here.
            return new GUIContent("    Fishnet Link", (Texture)Resources.Load("Link"), "Controls COZY for use in a multiplayer environment using Fishnet.");
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