using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace TL.Core
{
    public class NPCVisualController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform rightHandIKTarget;
        [SerializeField] private Transform leftHandIKTarget;
        [SerializeField] private Rig rig;

        private NPCAnimationEvents animationEvents;

        void Start()
        {
            animationEvents = GetComponent<NPCAnimationEvents>();

            if (animator == null)
                animator = GetComponentInChildren<Animator>();
        }

        public void SetWalkingState(bool isWalking)
        {
            if (animator != null)
                animator.SetBool("IsWalking", isWalking);
        }

        public void SetSittingState(bool isSitting)
        {
            if (animator != null)
                animator.SetBool("Sitting", isSitting);
        }

        public void SetHandIKTargets(Transform leftHandPos, Transform rightHandPos)
        {
            if (leftHandPos != null && rightHandPos != null)
            {
                leftHandIKTarget.localPosition = leftHandPos.localPosition;
                leftHandIKTarget.localRotation = leftHandPos.localRotation;

                rightHandIKTarget.localPosition = rightHandPos.localPosition;
                rightHandIKTarget.localRotation = rightHandPos.localRotation;
            }
        }

        public void AdjustIKWeight(float targetWeight)
        {
            if (rig != null)
            {
                rig.weight = Mathf.Clamp01(targetWeight);
            }
        }

        public bool IsIdleAnimationActive()
        {
            return animator != null &&
                   animator.GetCurrentAnimatorStateInfo(0).IsName("HumanM@Idle01");
        }
    }
}