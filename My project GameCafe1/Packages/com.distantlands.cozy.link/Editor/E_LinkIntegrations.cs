using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DistantLands.Cozy.EditorScripts
{
    public class E_LinkIntegrations : EditorWindow, ICozyEditorMenuEntry
    {


        // [MenuItem("Tools/Cozy: Stylized Weather 3/Check Link Integrations", false, 1500)]
        [MenuItem("Window/Cozy: Stylized Weather 3/Check Link Integrations", false, 1021)]
        static void Init()
        {

            E_LinkIntegrations window = (E_LinkIntegrations)EditorWindow.GetWindow(typeof(E_LinkIntegrations), false, "Setup LINK");
            window.minSize = new Vector2(400, 500);
            window.Show();

        }

        [InitializeOnLoadMethod()]
        static void CheckOpen()
        {

            if (EditorPrefs.GetInt("LINK_Integration") == 0)
                Debug.LogWarning("COZY: Weather 3 -- NO MULTIPLAYER SOLUTION IS SET FOR LINK. Please set one in the COZY: Weather Hub!");

        }

        private void OnGUI()
        {

            EditorGUILayout.LabelField("LINK Integrations", E_CozyEditor.TitleStyle);
            EditorGUILayout.HelpBox("Thank you for your purchase of LINK: COZY Multiplayer Module! Select your multiplayer solution from the dropdown and allow your scripts to reload to get started.", MessageType.Info);

            string[] integrations = new string[5] { "None", "PUN", "FishNetworking", "Netcode for Gameobjects", "Mirror" };

            int i = EditorGUILayout.Popup(new GUIContent("Multiplayer Type"), EditorPrefs.GetInt("LINK_Integration"), integrations);
            EditorGUILayout.Space();

            if (i != EditorPrefs.GetInt("LINK_Integration"))
            {

                SetupIntegration(i);
                EditorPrefs.SetInt("LINK_Integration", i);

            }

            EditorGUILayout.HelpBox("Then in a scene setup for multiplayer, setup your scene for COZY and add the LINK module to the COZY weather sphere in the settings/modules tab. If you are using Mirror, be sure to click the Ensure Setup button.", MessageType.Info);



        }

        void SetupIntegration(int integration)
        {

            switch (integration)
            {
                case (0):
                    RemoveDefine("LINK_PUN");
                    RemoveDefine("LINK_FISHNET");
                    RemoveDefine("LINK_NETCODE");
                    RemoveDefine("LINK_MIRROR");
                    break;
                case (1):
                    AddDefine("LINK_PUN");
                    RemoveDefine("LINK_FISHNET");
                    RemoveDefine("LINK_NETCODE");
                    RemoveDefine("LINK_MIRROR");
                    break;
                case (2):
                    RemoveDefine("LINK_PUN");
                    AddDefine("LINK_FISHNET");
                    RemoveDefine("LINK_NETCODE");
                    RemoveDefine("LINK_MIRROR");
                    break;
                case (3):
                    RemoveDefine("LINK_PUN");
                    RemoveDefine("LINK_FISHNET");
                    AddDefine("LINK_NETCODE");
                    RemoveDefine("LINK_MIRROR");
                    break;
                case (4):
                    RemoveDefine("LINK_PUN");
                    RemoveDefine("LINK_FISHNET");
                    RemoveDefine("LINK_NETCODE");
                    AddDefine("LINK_MIRROR");
                    break;

            }

        }

        static void AddDefine(string define)
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            allDefines.Add(define);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", allDefines.ToArray()));


        }

        static void RemoveDefine(string define)
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            List<string> allDefines = definesString.Split(';').ToList();

            if (allDefines.Contains(define))
                allDefines.Remove(define);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));


        }

        public E_CozyEditor.MenuSection GetMenuSection()
        {
            return new E_CozyEditor.MenuSection(new GUIContent("LINK Integrations"), OnGUI);
        }
    }
}