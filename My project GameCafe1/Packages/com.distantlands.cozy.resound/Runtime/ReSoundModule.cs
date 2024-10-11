using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using DistantLands.Cozy.Data;
using UnityEngine.Audio;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DistantLands.Cozy
{
    [ExecuteAlways]
    public class ReSoundModule : CozyModule, ICozyBiomeModule
    {

        public Transform resoundParent;
        [CozyProfile]
        public ReSoundDJ DJ;
        [CozyProfile]
        public ReSoundSetlist setlist;
        public Dictionary<ReSoundTrack, MixerChannel> localChannelMixerState = new Dictionary<ReSoundTrack, MixerChannel>();
        public Dictionary<ReSoundTrack, AudioSource> channelMixerOutput = new Dictionary<ReSoundTrack, AudioSource>();

        [System.Serializable]
        public class MixerChannel
        {
            public float volume;
            public bool transitioning;
            public IEnumerator TransitionVolume(float target, float time)
            {
                float timer = time;
                transitioning = true;
                float startingVolume = volume;
                while (timer > 0)
                {
                    timer -= Time.deltaTime;
                    volume = Mathf.Lerp(startingVolume, target, 1 - (timer / time));
                    yield return new WaitForEndOfFrame();
                }
                transitioning = false;
                volume = target;
            }

            public MixerChannel()
            {
                volume = 0;
                transitioning = false;
            }
            public MixerChannel(float _volume, bool _tranisitoning)
            {
                volume = _volume;
                transitioning = _tranisitoning;
            }
        }

        //PLAYBACK
        public float songTimer;
        public bool paused;
        public AudioMixerGroup mixerGroup;
        public ReSoundTrack currentTrack;
        public float masterVolume = 1;

        //BIOMES
        public float weight;
        public List<ReSoundModule> biomes = new List<ReSoundModule>();
        public ReSoundModule parentModule;
        public bool isBiomeModule { get; set; }


        //FX
        public Dictionary<ReSoundFX, float> fXes = new Dictionary<ReSoundFX, float>();
        internal float fxWeight;


        public override void InitializeModule()
        {
            base.InitializeModule();

            if (!Application.isPlaying)
                return;

            isBiomeModule = GetComponent<CozyBiome>();

            SetupMixerChannels();

            if (isBiomeModule)
            {
                AddBiome();
                return;
            }

            fXes = new Dictionary<ReSoundFX, float>();
            parentModule = this;
            biomes = FindObjectsOfType<ReSoundModule>().Where(x => x != this).ToList();
            masterVolume = 1;

            PlayFromBeginning();
        }

        /// <summary>
        /// Sets up the global mixer state with a channel for every song and adds instances for the audio sources in the scene.
        /// </summary>
        public void SetupMixerChannels()
        {

            if (isBiomeModule)
            {
                if (parentModule == null)
                    parentModule = weatherSphere.GetModule<ReSoundModule>();

                foreach (ReSoundTrack track in parentModule.DJ.availableTracks)
                {
                    if (!localChannelMixerState.Keys.Contains(track))
                        localChannelMixerState.Add(track, new MixerChannel());
                }
                return;
            }

            SetupParent();

            foreach (ReSoundTrack track in DJ.availableTracks)
            {

                GameObject songObject = new GameObject();
                songObject.transform.parent = resoundParent;
                songObject.name = track.name;

                AudioSource source = songObject.AddComponent<AudioSource>();
                source.volume = 0;
                source.pitch = 1f;
                source.clip = track.clip;
                source.outputAudioMixerGroup = mixerGroup;
                source.playOnAwake = true;
                source.time = 0;
                source.Play();

                if (!localChannelMixerState.Keys.Contains(track))
                    localChannelMixerState.Add(track, new MixerChannel());
                if (!channelMixerOutput.Keys.Contains(track))
                    channelMixerOutput.Add(track, source);

            }
        }

        void SetupParent()
        {
            if (resoundParent == null)
            {
                foreach (Transform t in FindObjectsOfType<Transform>())
                {
                    if (t.name == "ReSound Parent")
                    {
                        resoundParent = t;
                        return;
                    }
                }

                resoundParent = new GameObject().transform;
                resoundParent.name = "ReSound Parent";
            }
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {

            if (!Application.isPlaying)
                return;

            if (!paused)
                UpdateLocalMixer();

            if (isBiomeModule)
                return;

            ComputeBiomeWeights();
            UpdateGlobalMixer();

            if (!channelMixerOutput[currentTrack].isPlaying && DJ.noSilenceMode)
                channelMixerOutput[currentTrack].Play();

        }


        public void UpdateLocalMixer()
        {

            if (weight == 0)
            {
                return;
            }

            if (songTimer <= (parentModule.DJ.transitionType != ReSoundDJ.TransitionType.noFade ? parentModule.DJ.transitionTime : 0))
                StopTrack(currentTrack);

            if (songTimer <= (parentModule.DJ.transitionType == ReSoundDJ.TransitionType.crossfade ? parentModule.DJ.transitionTime : 0))
                PlayTrack(RandomTrack());

            // songTimer = currentTrack.clip.length - parentModule.channelMixerOutput[currentTrack].time;
            songTimer -= Time.deltaTime;

            foreach (var channel in channelMixerOutput)
            {
                channel.Value.volume = localChannelMixerState[channel.Key].volume;
            }
        }

        public void UpdateGlobalMixer()
        {

            fxWeight = Mathf.Clamp01(fxWeight);

            foreach (var channel in localChannelMixerState)
            {
                channelMixerOutput[channel.Key].volume = localChannelMixerState[channel.Key].volume * weight * masterVolume;
            }

            // foreach (var fx in fXes)
            // {
            //     channelMixerOutput[fx.Key.track].volume += fx.Value * weight;
            // }

            foreach (var biome in biomes)
            {
                foreach (var pair in biome.localChannelMixerState)
                {
                    var track = pair.Key;
                    var channel = pair.Value;
                    var output = channelMixerOutput[track];

                    output.volume += biome.weight * channel.volume * masterVolume;

                }
            }

            if (DJ.resetOnEntry)
                foreach (var channel in channelMixerOutput)
                {
                    if (channel.Value.volume == 0)
                    {
                        AudioClip clip = channel.Key.clipType == ReSoundTrack.ClipType.singleClip ? channel.Key.clip : channel.Key.playlist[Random.Range(0, channel.Key.playlist.Length - 1)];
                        channel.Value.clip = clip;
                        channel.Value.Play();
                        channel.Value.time = 0;
                    }
                }
        }

        ReSoundTrack RandomTrack()
        {
            ReSoundTrack randomTrack = null;
            List<float> chancePerTrack = new List<float>();
            float totalChance = 0;

            foreach (ReSoundTrack k in setlist.availableTracks)
            {
                if (k == currentTrack)
                {
                    chancePerTrack.Add(0);
                    totalChance += 0;
                    continue;
                }

                float chance = k.GetChance(weatherSphere, 0);
                chancePerTrack.Add(chance);
                totalChance += chance;
            }

            float selection = Random.Range(0, totalChance);

            int iterator = 0;
            float iteratedChance = 0;

            while (iteratedChance <= selection)
            {
                if (iterator >= chancePerTrack.Count)
                {
                    randomTrack = setlist.availableTracks[setlist.availableTracks.Count - 1];
                    break;
                }

                if (selection >= iteratedChance && selection < iteratedChance + chancePerTrack[iterator])
                {
                    randomTrack = setlist.availableTracks[iterator];
                    break;
                }
                iteratedChance += chancePerTrack[iterator];
                iterator++;

            }

            if (!randomTrack)
            {
                randomTrack = setlist.availableTracks[0];
            }

            return randomTrack;
        }

        #region Disc Controls

        public void PlayTrack(ReSoundTrack track)
        {
            Play();
            if (!localChannelMixerState.ContainsKey(track))
                localChannelMixerState.Add(track, new MixerChannel(0, true));

            currentTrack = track;
            // AudioClip clip = track.clipType == ReSoundTrack.ClipType.singleClip ? track.clip : track.playlist[Random.Range(0, track.playlist.Length - 1)];
            // if (!isBiomeModule)
            // {
            //     channelMixerOutput[track].clip = clip;
            //     channelMixerOutput[track].Play();
            // }
            songTimer = parentModule.channelMixerOutput[track].clip.length + (DJ.noSilenceMode ? 0 : Random.Range(setlist.minSilenceTime, setlist.maxSilenceTime));
            if (parentModule.DJ.transitionType == ReSoundDJ.TransitionType.noFade)
            {
                if (!isBiomeModule)
                {
                    localChannelMixerState[track].volume = 1;
                }
            }
            else
            {
                StartCoroutine(localChannelMixerState[track].TransitionVolume(1, parentModule.DJ.transitionTime));
            }
        }

        public void StopTrack(ReSoundTrack track)
        {
            if (track == null)
                return;

            if (parentModule.DJ.transitionType == ReSoundDJ.TransitionType.noFade)
            {
                localChannelMixerState[track].volume = 0;
            }
            else
                StartCoroutine(localChannelMixerState[track].TransitionVolume(0, parentModule.DJ.transitionTime));
        }

        public void Skip()
        {
            songTimer = 0;
        }

        public void Pause()
        {
            paused = true;
            foreach (var output in channelMixerOutput)
            {
                output.Value.Pause();
            }
        }

        public void Play()
        {
            paused = false;
            foreach (var output in channelMixerOutput)
            {
                output.Value.UnPause();
            }
        }

        public void Shuffle()
        {
            Play();
            PlayTrack(RandomTrack());
        }

        public void PlayFromBeginning()
        {
            if (setlist.initialSong && setlist.startingStyle == ReSoundSetlist.StartingStyle.startWithInitialSong)
            {
                PlayTrack(setlist.initialSong);
            }
            else
            {
                PlayTrack(RandomTrack());
            }
        }

        public IEnumerator FreezeForTime(float freezeTime)
        {

            Pause();

            yield return new WaitForSeconds(freezeTime);

            Play();
        }

        public IEnumerator FadeToVolume(float fadeTime, float targetVolume)
        {

            float currentVolume = masterVolume;

            for (float i = 0; i < fadeTime; i += Time.deltaTime)
            {

                masterVolume = Mathf.Lerp(currentVolume, targetVolume, i);
                yield return new WaitForEndOfFrame();

            }

            masterVolume = targetVolume;

        }

        public IEnumerator FadeOutFadeIn(float fadeTime, float waitTime)
        {

            float currentVolume = masterVolume;

            for (float i = 0; i < fadeTime; i += Time.deltaTime)
            {

                masterVolume = Mathf.Lerp(currentVolume, 0, i);
                yield return new WaitForEndOfFrame();

            }
            masterVolume = 0;

            yield return new WaitForSeconds(waitTime);

            for (float i = 0; i < fadeTime; i += Time.deltaTime)
            {

                masterVolume = Mathf.Lerp(0, currentVolume, i);
                yield return new WaitForEndOfFrame();

            }

            masterVolume = currentVolume;

        }

        public void RunFreezeForTime(float freezeTime)
        {
            StartCoroutine(FreezeForTime(freezeTime));
        }
        public void RunFadeToVolume(float targetVolume)
        {
            StartCoroutine(FadeToVolume(1, targetVolume));
        }
        public void RunFadeOutFadeIn(float waitTime)
        {
            StartCoroutine(FadeOutFadeIn(1f, waitTime));
        }

        #endregion

        #region Biome Controls
        public void AddBiome()
        {
            if (parentModule == null)
                parentModule = weatherSphere.GetModule<ReSoundModule>();

            weatherSphere.GetModule<ReSoundModule>().biomes = FindObjectsOfType<ReSoundModule>().Where(x => x != weatherSphere.GetModule<ReSoundModule>()).ToList();

        }

        public void RemoveBiome()
        {
            parentModule?.biomes.Remove(this);
        }

        public void UpdateBiomeModule()
        {

        }

        public bool CheckBiome()
        {
            if (!CozyWeather.instance.GetModule<ReSoundModule>())
            {
                Debug.LogError("The ReSound biome module requires the ReSound module to be enabled on your weather sphere. Please add the ReSound module before setting up your biome.");
                return false;
            }
            return true;
        }

        public void ComputeBiomeWeights()
        {
            float totalSystemWeight = 0;
            biomes.RemoveAll(x => x == null);

            foreach (ReSoundModule biome in biomes)
            {
                if (biome != this)
                {
                    totalSystemWeight += biome.system.targetWeight;
                }
            }

            weight = Mathf.Clamp01(1 - totalSystemWeight);
            totalSystemWeight += weight;

            foreach (ReSoundModule biome in biomes)
            {
                if (biome.system != this)
                    biome.weight = biome.system.targetWeight / (totalSystemWeight == 0 ? 1 : totalSystemWeight);
            }
        }

        #endregion

        #region FX Controls


        public override void FrameReset()
        {

            List<KeyValuePair<ReSoundFX, float>> list = fXes.ToList();

            for (int i = 0; i < fXes.Count; i++)
            {
                var pair = list[i];
                fXes[pair.Key] = 0;
            }
        }


        #endregion

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ReSoundModule))]
    [CanEditMultipleObjects]
    public class E_ReSoundModule : E_CozyModule, E_BiomeModule
    {

        public static bool selectionWindowIsOpen;
        public static bool djWindowIsOpen;
        public static bool setlistWindowIsOpen;
        private ReSoundModule reSoundModule;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            reSoundModule = (ReSoundModule)target;
        }

        public override GUIContent GetGUIContent()
        {

            //Place your module's GUI content here.
            return new GUIContent("    ReSound", (Texture)Resources.Load("ReSound Icon"), "Integrate soundtrack and ambient sounds into your COZY system.");

        }
        public override void OpenDocumentationURL()
        {
            Application.OpenURL("https://distant-lands.gitbook.io/cozy-stylized-weather-documentation/how-it-works/modules/resound-module");
        }

        public override void GetReportsInformation()
        {

            EditorGUILayout.PropertyField(serializedObject.FindProperty("currentTrack"));

        }

        public override void DisplayInCozyWindow()
        {
            serializedObject.Update();



            EditorGUILayout.BeginHorizontal();


            if (reSoundModule.paused)
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("PlayButton")))
                {
                    reSoundModule.Play();
                }
            }
            else
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("PauseButton")))
                {
                    reSoundModule.Pause();
                }
            }

            if (GUILayout.Button(EditorGUIUtility.IconContent("StepButton")))
            {
                reSoundModule.Skip();
            }

            GUILayout.Label(new GUIContent(reSoundModule.currentTrack ? reSoundModule.currentTrack.name : "No track playing currently - "), EditorStyles.boldLabel);


            EditorGUILayout.EndHorizontal();

            selectionWindowIsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(selectionWindowIsOpen, new GUIContent("    Selection Settings"), EditorUtilities.FoldoutStyle);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (selectionWindowIsOpen)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DJ"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("setlist"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("mixerGroup"));
                EditorGUI.indentLevel--;

            }

            djWindowIsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(djWindowIsOpen, new GUIContent("    DJ Settings"), EditorUtilities.FoldoutStyle);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (djWindowIsOpen)
            {
                EditorGUI.indentLevel++;
                if (reSoundModule.DJ)
                    CreateEditor(reSoundModule.DJ).OnInspectorGUI();
                else
                    EditorGUILayout.HelpBox("Assign a DJ & setlist to begin using ReSound.", MessageType.Warning);
                EditorGUI.indentLevel--;

            }


            setlistWindowIsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(setlistWindowIsOpen, new GUIContent("    Setlist Settings"), EditorUtilities.FoldoutStyle);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (setlistWindowIsOpen)
            {
                EditorGUI.indentLevel++;
                if (reSoundModule.setlist)
                    CreateEditor(reSoundModule.setlist).OnInspectorGUI();
                else
                    EditorGUILayout.HelpBox("Assign a DJ & setlist to begin using ReSound.", MessageType.Warning);
                EditorGUI.indentLevel--;

            }

            serializedObject.ApplyModifiedProperties();

        }

        public void DrawBiomeReports()
        {

        }

        public void DrawInlineBiomeUI()
        {
            EditorGUILayout.BeginHorizontal();


            if (reSoundModule.paused)
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("PlayButton")))
                {
                    reSoundModule.Play();
                }
            }
            else
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("PauseButton")))
                {
                    reSoundModule.Pause();
                }
            }

            if (GUILayout.Button(EditorGUIUtility.IconContent("StepButton")))
            {
                reSoundModule.Skip();
            }

            GUILayout.Label(new GUIContent(reSoundModule.currentTrack ? reSoundModule.currentTrack.name : "No track playing currently - "), EditorStyles.boldLabel);


            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("setlist"));
            serializedObject.ApplyModifiedProperties();
        }

    }
#endif
}