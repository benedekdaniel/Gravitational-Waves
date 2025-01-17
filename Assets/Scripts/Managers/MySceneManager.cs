﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using static Game.Managers.UIManager;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace Game.Managers {
    public static class MySceneManager {

        public static Scene Current { get => SceneManager.GetActiveScene(); }
        public static Stack<string> Previous { get; private set; }

        public static List<string> AdditonalScenes { get; private set; }

        private static List<string> OverMenuScenes;

        public static Dictionary<string, List<KeyValuePair<string, dynamic>>> SceneArguments = new Dictionary<string, List<KeyValuePair<string, dynamic>>>
        {
            { 
                "GameView", 
                new List<KeyValuePair<string, dynamic>> {
                    new KeyValuePair<string, dynamic>("IsHost", false),
                } 
            }
        };

        /// <summary>
        /// Returns the value of the specified argument by name for the specified scene, if any.
        /// </summary>
        /// <param name="sceneName">The specified scene.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>The value found for the specified scene by the specified argument name, if any. Null otherwise</returns>
        public static T GetSceneArgument<T>(string sceneName, string argumentName)
        {
            if (SceneArguments.TryGetValue(sceneName, out List<KeyValuePair<string, dynamic>> argumentsListForScene))
            {
                return (T) argumentsListForScene.Find(arg => arg.Key == argumentName).Value; 
            }

            return default;
        }

        public static void SetSceneArgument(string sceneName, string argumentName, dynamic value)
        {
            List<KeyValuePair<string, dynamic>> argumentsListForScene = new List<KeyValuePair<string, dynamic>>();

            if (!SceneArguments.ContainsKey(sceneName)) // If no arguments list for the scene, create it.
                SceneArguments.Add(sceneName, argumentsListForScene);

            argumentsListForScene = SceneArguments[sceneName];

            argumentsListForScene.RemoveAll(arg => arg.Key == argumentName);
            argumentsListForScene.Add(new KeyValuePair<string, dynamic>(argumentName, value));
        }

        public static string overlay = "OverlayMenu";
        public static string mainMenu = "MainMenu";
        private static event UnityAction OnExit;

        public static bool Ready { get; private set; }


        static MySceneManager() {
            Debug.Log("Loading MySceneManager");
            AdditonalScenes = new List<string>();
            OverMenuScenes = new List<string>() { overlay, mainMenu };
            SceneManager.activeSceneChanged += OnSceneLoaded;
            Previous = new Stack<string>();
            Ready = true;
        }

        public static void Load() {
            
        }

        public static void LoadScene(string name) {
            OnExit?.Invoke();
            if (name != null && name != "") {
                Debug.LogFormat("Changing Scene from {0} to {1}", Current.name, name);
                Previous.Push(Current.name);
                AdditonalScenes.Clear();
                SceneManager.LoadScene(name, LoadSceneMode.Single);
                AdditonalScenes = new List<string>();
            } else {
                string error = name == null ? "Null String" : "Empty String";
                Debug.LogWarningFormat("{0} Scene Called", error);
            }

        }

        public static void OnSceneLoaded(Scene previousScene, Scene newScene) {
            InstanceManager.Init();
        }

        public static void LoadSceneAdditive(string name) {
            Debug.LogFormat("Loading Additive Scene {0}", name);
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
            AdditonalScenes.Add(name);
        }

        public static void UnloadScene(string name) {
            if (AdditonalScenes.Contains(name)) {
                Debug.LogFormat("Removing Scene {0}", name);
                AdditonalScenes.Remove(name);
                SceneManager.UnloadSceneAsync(name);
            }

        }

        public static void AddOnGameExitCallback(UnityAction callback) {
            OnExit += callback;
        }


        internal static void OverlayMenuToggle() {
            if (AdditonalScenes.Contains(overlay)) {
                UnloadOverlayMenu();
            } else {
                LoadOverlayMenu();
            }

        }
        internal static void LoadOverlayMenu() {
            if (!AdditonalScenes.Contains(overlay) && !OverMenuScenes.Contains(Current.name)) {
                LoadSceneAdditive(overlay);
            }
        }

        internal static void UnloadOverlayMenu() {
            if (AdditonalScenes.Contains(overlay)) {
                UnloadScene(overlay);
            }
        }


        public static void LoadPreviousScene() {
            Debug.Log(Previous.Peek());
            LoadScene(Previous.Pop());
            Previous.Pop();
        }

        public static void MenuExitGame(){
            OnExit?.Invoke();
            Debug.Log("Exiting Game");
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            Debug.Log("UnityEditor Stop");
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Debug.Log("Application Quit");
            Application.Quit();
#endif
        }
    }
}