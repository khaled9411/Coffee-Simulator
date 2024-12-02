using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TL.UtilityAI.Actions;

public class Door : MonoBehaviour, IInteractable
{
    [field: SerializeField] public string verbName { get; set; }

    private bool isOpen = false;
    private Collider doorCollider;

    public float openAngle = 160f;
    public float closeAngle = 0f;
    public float animationDuration = 1f;

    private void Start()
    {
        Faint.Instance.onFaint += CloseDoor;
        Bed.Instance.onSleep += CloseDoor;

        doorCollider = GetComponent<Collider>();

        verbName = "Open";
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            if (isOpen)
            {
                CloseDoor();
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                OpenDoor();
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void Interact()
    {
        if (isOpen)
        {
            CloseDoor();
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            OpenDoor();
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void OpenDoor()
    {
        doorCollider.isTrigger = true;

        transform.DOLocalRotate(new Vector3(0, openAngle, 0), animationDuration)
                 .SetEase(Ease.OutCubic);


        isOpen = true;
        verbName = "Close";
    }

    public void CloseDoor()
    {
        CafeManager.instance.isOpen = false;
        doorCollider.isTrigger = false;

        transform.DOLocalRotate(new Vector3(0, closeAngle, 0), animationDuration)
                 .SetEase(Ease.OutCubic);


        isOpen = false;
        verbName = "Open";
    }
}
