using DG.Tweening;
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
        [SerializeField] private float handMovementAmount = 0.1f;
        [SerializeField] private float handMovementDuration = 0.5f;

        private NPCAnimationEvents animationEvents;
        private Vector3[] initialPositions;
        private Tween[] handTweens;

        void Start()
        {
            animationEvents = GetComponent<NPCAnimationEvents>();
            initialPositions = new Vector3[2];
            handTweens = new Tween[2];

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

        public void StartHandMovement()
        {
            StopHandMovement();
            initialPositions[0] = rightHandIKTarget.localPosition;
            initialPositions[1] = leftHandIKTarget.localPosition;


            handTweens[0] = rightHandIKTarget.DOLocalMoveX(initialPositions[0].x + handMovementAmount, handMovementDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            handTweens[1] = leftHandIKTarget.DOLocalMoveX(initialPositions[1].x + handMovementAmount, handMovementDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void StopHandMovement()
        {

            for (int i = 0; i < handTweens.Length; i++)
            {
                if (handTweens[i] != null && handTweens[i].IsActive())
                {
                    handTweens[i].Kill();
                    handTweens[i] = null;
                }
            }
        }
    }
}