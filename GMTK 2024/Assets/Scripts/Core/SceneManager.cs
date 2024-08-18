using System;
using System.Collections;
using Game.Core.Scenes;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Manages Scene Transitions
    /// </summary>
    public class SceneManager : GenericSingleton<SceneManager>
    {
        /// <summary>
        /// Enum used to identify scenes in the game.
        /// </summary>
        public enum Scenes
        {
            TitleScreen,
            Game
        }
        
        // Internal
        private Coroutine sceneChangeCoroutine;
        
        // References
        internal SceneTransitioner sceneTransitioner;
        internal bool allowTransition;
        
        
        protected override void Awake()
        {
            // Singleton
            base.Awake();
            if (Instance != this) return;
            
            // References
            sceneTransitioner = GetComponentInChildren<SceneTransitioner>();
        }

        /// <summary>
        /// Get the current scene.
        /// </summary>
        /// <returns>The name of the current string.</returns>
        public string GetCurrentScene()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        /// <summary>
        /// Is the given scene playing
        /// </summary>
        /// <param name="scene">The scene being checked</param>
        /// <returns>Whether or not the scene is playing</returns>
        public bool IsScene(Scenes scene)
        {
            return GetCurrentScene() == scene.ToString();
        }

        /// <summary>
        /// Reloads the current scene.
        /// </summary>
        public void ReloadScene()
        {
            ChangeScene(GetCurrentScene());
        }

        /// <summary>
        /// Begins a scene change.
        /// </summary>
        /// <param name="scene">Scene to change to.</param>
        /// <param name="transitionType">Transition type to use.</param>
        /// <param name="callback">Callback to invoke between scene change.</param>
        public void ChangeScene
        (
            Scenes scene,
            SceneTransitioner.TransitionType transitionType = SceneTransitioner.TransitionType.Fade,
            Action callback = null
        )
        {
            ChangeScene(scene.ToString(), transitionType, callback);
        }
        
        /// <summary>
        /// Begins a scene change.
        /// </summary>
        /// <param name="sceneName">Name of scene to change to.</param>
        /// <param name="transitionType">Transition type to use.</param>
        /// <param name="callback">Callback to invoke between scene change.</param>
        public void ChangeScene(
            string sceneName, 
            SceneTransitioner.TransitionType transitionType = SceneTransitioner.TransitionType.Fade,
            Action callback = null
        )
        {
            if (sceneChangeCoroutine != null) StopCoroutine(sceneChangeCoroutine);
            
            // Scene Transition
            sceneChangeCoroutine = StartCoroutine(TransitionScene(sceneName, transitionType, callback));
            
        }
        
        /// <summary>
        /// Transitions the scene through a coroutine.
        /// </summary>
        /// <param name="sceneName">Scene to change to.</param>
        /// <param name="transitionType">Transition type to use.</param>
        /// <param name="callback">Callback to invoke between scene change.</param>
        /// <returns>Coroutine transition for the scene.</returns>
        private IEnumerator TransitionScene(
            string sceneName,
            SceneTransitioner.TransitionType transitionType,
            Action callback
        )
        {
            // Fade Out
            yield return StartCoroutine(sceneTransitioner.TransitionOut(transitionType));
         
            // Callback
            callback?.Invoke();
            
            // Begin Scene Load
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;
            
            // Allow Scene Change
            while (!asyncLoad.isDone) {
                if (asyncLoad.progress >= 0.9f) {
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }

            // Buffer Frame
            yield return null;
            yield return new WaitUntil(() => allowTransition);
            
            // Fade In
            // NOTE: Used to ensure the scene transition fader is fully black
            yield return StartCoroutine(sceneTransitioner.TransitionIn(transitionType));

            // Buffer Frame
            yield return null;
            
            // Reset
            sceneChangeCoroutine = null;
        }
    }
}
