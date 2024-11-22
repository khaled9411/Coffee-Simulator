using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTo : MonoBehaviour, IRespondable
{
    [field: SerializeField] public string respondableName { get; set; }
    [field: SerializeField] public string verbName { get; set; }

    [field: SerializeField] public Transform NewPos;

    public void Respond()
    {
        FadeEvent.OnFadeInEnd += FadeEvent_OnFadeInEnd;
        FadeEvent.TriggerFadeInStart();
    }

    private void FadeEvent_OnFadeInEnd()
    {
        Player.Instance.SetPos(NewPos.position);
        FadeEvent.OnFadeInEnd -= FadeEvent_OnFadeInEnd;
    }
}
