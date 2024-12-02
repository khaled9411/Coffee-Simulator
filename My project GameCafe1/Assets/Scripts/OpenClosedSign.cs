using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClosedSign : MonoBehaviour , IInteractable
{
    [field: SerializeField] public string verbName { get; set; }

    public float openAngle = 160f;
    public float closeAngle = 0f;
    public float animationDuration = 1f;


    // Start is called before the first frame update
    void Start()
    {
        CafeManager.instance.OnIsOpenChanage += ChangeState;

        verbName = "Open";
    }

    public void Interact()
    {
        CafeManager.instance.isOpen = !CafeManager.instance.isOpen;
    }

    private void ChangeState(bool isOpen)
    {
        if (isOpen)
        {
            OpenSign();
        }
        else
        {
            CloseSign();
        }
    }

    private void CloseSign()
    {
        transform.DOLocalRotate(new Vector3(0, closeAngle, 0), animationDuration)
         .SetEase(Ease.OutCubic);

        verbName = "Open";
    }

    private void OpenSign()
    {
        transform.DOLocalRotate(new Vector3(0, openAngle, 0), animationDuration)
         .SetEase(Ease.OutCubic);

        verbName = "Close";
    }
}
