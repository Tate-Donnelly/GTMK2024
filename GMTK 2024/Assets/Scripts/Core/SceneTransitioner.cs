using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game.Core.Scenes
{
    /// <summary>
    /// Helper for Unity's SceneManager. Handles scene transition effects.
    /// NOTE: We split scene management and scene transition logic into two separate classes in
    /// order to easily modify scenes transition effects in the future.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class SceneTransitioner : MonoBehaviour
    {
        // Constants
        private const float TRANSITION_DURATION = 0.5f;
        
        /// <summary>
        /// Enum used to identify transition types.
        /// </summary>
        public enum TransitionType
        {
            None,
            Fade
        }
        
        // References
        private CanvasGroup canvasGroup;
        
        
        private void Awake()
        {
            // References
            canvasGroup = GetComponent<CanvasGroup>();
        }
        
        /// <summary>
        /// Plays the transition in via animation, awaiting the animation event to
        /// indicate that the transition is complete.
        /// </summary>
        /// <param name="type">The transition type.</param>
        /// <returns>Coroutine.</returns>
        internal IEnumerator TransitionIn(TransitionType type = TransitionType.Fade)
        {
            // Animation
            Tween tween = MakeTransitionTween(type, true);
            
            // Playback
            if (tween != null)
            {
                tween.Play();
                yield return tween.WaitForCompletion();
            }
            
            // State
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
        
        /// <summary>
        /// Plays the transition out via animation, awaiting the animation event to
        /// indicate that the transition is complete.
        /// </summary>
        /// <param name="type">The transition type.</param>
        /// <returns>Coroutine.</returns>
        internal IEnumerator TransitionOut(TransitionType type = TransitionType.Fade)
        {
            // State
            canvasGroup.blocksRaycasts = true;
            
            // Animation
            Tween tween = MakeTransitionTween(type, false);
            
            // Playback
            if (tween != null)
            {
                tween.Play();
                yield return tween.WaitForCompletion();
            }
            
            // State
            canvasGroup.alpha = 1;
        }
        
        /// <summary>
        /// Makes a transition tween based on the transition type.
        /// </summary>
        /// <param name="type">The type of transition to make.</param>
        /// <param name="isTransitionIn">Whether the transition is inward (black to transparent)</param>
        /// <returns>The transition tween.</returns>
        private Tween MakeTransitionTween(TransitionType type, bool isTransitionIn)
        {
            return MakeTransitionTween(type, isTransitionIn ? 0 : 1, TRANSITION_DURATION, Ease.OutCubic);
        }
        
        /// <summary>
        /// Makes a transition tween based on the transition type.
        /// </summary>
        /// <param name="type">The type of transition to make.</param>
        /// <param name="targetAlpha">The alpha to transition to.</param>
        /// <param name="duration">The duration of the teen.</param>
        /// <param name="ease">The ease of the tween.</param>
        /// <returns>The transition tween.</returns>
        public Tween MakeTransitionTween(TransitionType type, float targetAlpha, float duration, Ease ease)
        {
            switch (type)
            {
                case TransitionType.None:
                    return DOTween.To(() => 0, (_) => { },  0, duration)
                        .OnComplete(() => canvasGroup.alpha = targetAlpha);
                case TransitionType.Fade:
                    return canvasGroup
                        .DOFade(targetAlpha, duration)
                        .SetEase(ease);
                default:
                    return null;
            }
        }
    }
}
