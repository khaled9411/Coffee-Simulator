#if LINK_PUN
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using DistantLands.Cozy.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Photon.Pun;
using Photon.Realtime;

namespace DistantLands.Cozy
{
    [RequireComponent(typeof(PhotonView))]
    public class LinkPhotonModule : CozyModule, IPhotonViewCallback
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

        public bool isMaster => PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient;
        public Dictionary<WeatherProfile, int> weatherHashes = new Dictionary<WeatherProfile, int>();
        public Dictionary<AmbienceProfile, int> ambienceHashes = new Dictionary<AmbienceProfile, int>();
        public CozyAmbienceModule ambienceManager;

        public enum LinkType { server, client }
        public LinkType linkType;

        private float m_Ticks;

        //VARIABLES______________________________________________________________________________________________________
        private PhotonView pvCache;


        //FUNCTIONS_______________________________________________________________________________________________________
        public void Update()
        {

            if (isMaster)
            {

                if (currentDelay <= 0)
                {
                    MasterSendRPC();
                    currentDelay = updateDelay;
                }
                else
                    currentDelay -= Time.deltaTime;

            }

        }

        public PhotonView photonView
        {
            get
            {
                if (pvCache)
                {

                    return pvCache;

                }
                else
                {
                    pvCache = GetComponent<PhotonView>();
                    return pvCache;
                }

            }
        }

        [PunRPC]
        public void SyncCozyTime(float currentTicks, int currentDay, int currentYear)
        {

            if (Mathf.Abs(currentTicks - weatherSphere.timeModule.currentTime) > timeSettingSensitivity)
                weatherSphere.timeModule.currentTime = currentTicks;
            weatherSphere.timeModule.currentYear = currentYear;
            weatherSphere.timeModule.currentDay = currentDay;

        }
        [PunRPC]
        public void SyncCozyAmbience(int ambience)
        {

            ambienceManager.currentAmbienceProfile = ambienceManager.ambienceProfiles[ambience];

        }

        [PunRPC]
        public void SyncCozyWeather(int[] cache, float[] values)
        {

            List<WeatherRelation> i = new List<WeatherRelation>();
            int l = 0;

            foreach (int j in cache)
            {

                WeatherRelation k = new WeatherRelation();
                k.profile = weatherSphere.weatherModule.ecosystem.forecastProfile.profilesToForecast[j];
                k.weight = values[l];
                i.Add(k);
                l++;

            }
            weatherSphere.weatherModule.ecosystem.weightedWeatherProfiles = i;

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



        private void MasterSendRPC()
        {

            if (linkTime) photonView.RPC(nameof(SyncCozyTime), RpcTarget.Others, (float)weatherSphere.timeModule.currentTime, weatherSphere.timeModule.currentDay, weatherSphere.timeModule.currentYear);
            if (linkAmbience) photonView.RPC(nameof(SyncCozyAmbience), RpcTarget.Others, ambienceHashes[ambienceManager.currentAmbienceProfile]);
            if (linkWeather) photonView.RPC(nameof(SyncCozyWeather), RpcTarget.Others, GetWeatherIDs(), GetWeatherIntensities());

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

            PhotonNetwork.AddCallbackTarget(this);
            photonView.AddCallbackTarget(this);

        }

        public void OnDisable()
        {


            PhotonNetwork.RemoveCallbackTarget(this);
            photonView.RemoveCallback<LinkPhotonModule>(this);


        }


    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LinkPhotonModule))]
    [CanEditMultipleObjects]
    public class E_LinkPhotonModule : E_CozyModule
    {


        public override GUIContent GetGUIContent()
        {

            //Place your module's GUI content here.
            return new GUIContent("    Photon Link", (Texture)Resources.Load("Link"), "Controls COZY for use in a multiplayer environment using Photon.");

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