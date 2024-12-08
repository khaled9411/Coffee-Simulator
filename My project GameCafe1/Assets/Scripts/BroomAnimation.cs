using UnityEngine;
using DG.Tweening;
using System.Collections;

public class BroomAnimation : MonoBehaviour
{
    [SerializeField] private float cleaningDuration = 0.5f;
    [SerializeField] private float attackingDuration = 0.3f;
    [SerializeField] private Collider hitCollider;

    private Vector3 initialPosition;
    private Vector3 initialRotation;

    private void Start()
    {
        InputManager.Instance.OnInteractWithPicked1 += Instance_OnInteractWithPicked1;
    }

    private void Instance_OnInteractWithPicked1()
    {
        if (!Player.Instance.interactHandler.hasBroom) return;
        StopAnimation();
        PlayAttackingAnimation();

    }

    public void StartPos()
    {
        initialPosition = transform.localPosition;
        initialRotation = new Vector3(-60, 0, 0);

        StopAnimation();
        PlayCleaningAnimation();

    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        StopAnimation();
    //        PlayCleaningAnimation();
    //    }
    //    else if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        StopAnimation();
    //        PlayAttackingAnimation();
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<TroublemakerNPC>(out TroublemakerNPC npc))
        {
            npc.SetCanMakeTroubleFALSE();
        }
    }

    public void PlayCleaningAnimation()
    {
        transform.localPosition = initialPosition;
        transform.localEulerAngles = initialRotation;

        transform.DOLocalRotate(new Vector3(-60, initialRotation.y, initialRotation.z), cleaningDuration / 2)
             .SetEase(Ease.InOutSine)
             .SetLoops(2, LoopType.Yoyo);

        transform.DOLocalMoveZ(initialPosition.z + 0.2f, cleaningDuration / 2)
             .SetEase(Ease.InOutSine)
             .SetLoops(-1, LoopType.Yoyo);
    }

    public void StopAnimation()
    {
        transform.DOKill();
    }

    public void PlayAttackingAnimation()
    {
        StartCoroutine(StartHitCollider());

        transform.localPosition = initialPosition;
        transform.localEulerAngles = initialRotation;

        transform.DOLocalRotate(new Vector3(-90, initialRotation.y, initialRotation.z), attackingDuration / 2)
             .SetEase(Ease.InOutSine)
             .SetLoops(2, LoopType.Yoyo);
    }

    private IEnumerator StartHitCollider()
    {
        hitCollider.enabled = true;

        yield return new WaitForSeconds(attackingDuration);

        hitCollider.enabled = false;
        StopAnimation();
        PlayCleaningAnimation();
    }

    private void OnDestroy()
    {
        StopAnimation();
    }

}
