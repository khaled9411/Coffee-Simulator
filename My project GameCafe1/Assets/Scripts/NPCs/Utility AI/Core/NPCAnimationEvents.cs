using UnityEngine;
using UnityEngine.Events;

namespace TL.Core
{
    public class NPCAnimationEvents : MonoBehaviour
    {
        [System.Serializable]
        public class NPCAnimationEventSet
        {
            public UnityEvent onStartWalking;
            public UnityEvent onStopWalking;
            public UnityEvent onStartSitting;
            public UnityEvent onStopSitting;
            public UnityEvent onFinishAction;
        }

        public NPCAnimationEventSet animationEvents;

        public void TriggerStartWalking()
        {
            animationEvents.onStartWalking?.Invoke();
        }

        public void TriggerStopWalking()
        {
            animationEvents.onStopWalking?.Invoke();
        }

        public void TriggerStartSitting()
        {
            animationEvents.onStartSitting?.Invoke();
        }

        public void TriggerStopSitting()
        {
            animationEvents.onStopSitting?.Invoke();
        }

        public void TriggerFinishAction()
        {
            animationEvents.onFinishAction?.Invoke();
        }
    }
}