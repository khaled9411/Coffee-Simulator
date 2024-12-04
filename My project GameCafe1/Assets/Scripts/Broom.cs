using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : MonoBehaviour , IInteractable
{
    [field: SerializeField] public string verbName { get; set; } = "Pick";
    BroomAnimation animation;

    private void Start()
    {
        animation = GetComponent<BroomAnimation>();
    }
    public void Interact()
    {
        Player.Instance.interactHandler.PickBroom(transform);
        animation.StopAnimation();
        animation.PlayCleaningAnimation();
    }

}
