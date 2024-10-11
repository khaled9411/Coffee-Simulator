#if LINK_MIRROR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DistantLands.Cozy.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DistantLands.Cozy
{
    // [RequireComponent(typeof(MirrorView))]
    // [RequireComponent(typeof(NetworkIdentity))]
    public class LinkMirrorModule : CozyModule
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

        public bool isMaster { get { return mirrorView.isMaster; } }

        //VARIABLES______________________________________________________________________________________________________
        private MirrorView nvCache;


        //FUNCTIONS_______________________________________________________________________________________________________
        public void Update()
        {


            if (currentDelay <= 0)
            {
                try
                {

                    if (isMaster)
                        MasterCommunication();
                    else
                        ClientCommunication();
                }
                catch
                {

                }
                currentDelay = updateDelay;
            }
            else
                currentDelay -= Time.deltaTime;

        }

        public MirrorView mirrorView
        {
            get
            {
                if (nvCache)
                {

                    return nvCache;

                }
                else
                {
                    nvCache = GetComponentInParent<MirrorView>();
                    if (nvCache == null)
                    {
                        Debug.LogError("Make sure to add the Mirror View component to your weather sphere!");
                        return null;
                    }
                    return nvCache;
                }

            }
        }




        public void SyncCozyTime()
        {

            if (Mathf.Abs(mirrorView.time - weatherSphere.timeModule.currentTime) > timeSettingSensitivity)
                weatherSphere.timeModule.currentTime = mirrorView.time;
            weatherSphere.timeModule.currentDay = mirrorView.day;
            weatherSphere.timeModule.currentYear = mirrorView.year;

        }


        public void SyncCozyAmbience()
        {

            ambienceManager.currentAmbienceProfile = ambienceManager.ambienceProfiles[mirrorView.ambience];

        }


        public void SyncCozyWeather()
        {

            weatherSphere.weatherModule.ecosystem.weightedWeatherProfiles.Clear();
            int l = 0;
            int[] weatherCache = PollValues(mirrorView.weatherCacheString);

            foreach (int j in weatherCache)
            {

                WeatherRelation k = new WeatherRelation();
                k.profile = weatherSphere.weatherModule.ecosystem.forecastProfile.profilesToForecast[j];
                k.weight = PollWeightValues(mirrorView.weatherValuesString)[l];
                weatherSphere.weatherModule.ecosystem.weightedWeatherProfiles.Add(k);
                l++;

            }
        }

        public void SyncCozyTimeMaster()
        {

            mirrorView.time = weatherSphere.timeModule.currentTime;
            mirrorView.day = weatherSphere.timeModule.currentDay;
            mirrorView.year = weatherSphere.timeModule.currentYear;

        }

        public void SyncCozyAmbienceMaster()
        {

            List<AmbienceProfile> ambiences = ambienceManager.ambienceProfiles.ToList();
            mirrorView.ambience = ambiences.IndexOf(ambienceManager.currentAmbienceProfile);

        }

        public void SyncCozyWeatherMaster()
        {

            string weatherIDString = "";

            foreach (int i in GetWeatherIDs())
                weatherIDString = $"{weatherIDString}{i},";

            mirrorView.weatherCacheString = weatherIDString;

            weatherIDString = "";

            foreach (float i in GetWeatherIntensities())
                weatherIDString = $"{weatherIDString}{i},";

            mirrorView.weatherValuesString = weatherIDString;

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
    [CustomEditor(typeof(LinkMirrorModule))]
    [CanEditMultipleObjects]
    public class E_LinkMirrorModule : E_CozyModule
    {

        LinkMirrorModule module;

        public override GUIContent GetGUIContent()
        {
            //Place your module's GUI content here.
            return new GUIContent("    Mirror Link", (Texture)Resources.Load("Link"), "Controls COZY for use in a multiplayer environment using Mirror.");
        }

        void OnEnable()
        {
            module = (LinkMirrorModule)target;
        }

        public override void DisplayInCozyWindow()
        {
            serializedObject.Update();

            if (GUILayout.Button("Ensure Setup"))
                EnsureSetup();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("updateDelay"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("timeSettingSensitivity"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("linkTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("linkWeather"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("linkAmbience"));

            serializedObject.ApplyModifiedProperties();

        }

        void EnsureSetup()
        {

            if (!module.GetComponentInParent<MirrorView>())
                module.transform.parent.gameObject.AddComponent<MirrorView>();

        }

    }
#endif
}

#endif