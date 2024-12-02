using System.Collections;
using System.Collections.Generic;
using TL.Core;
using TL.UtilityAI.Actions;
using UnityEngine;

public class CustomerArea : MonoBehaviour
{
    [SerializeField] private Door door;
    private int npcCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NPCController>(out NPCController npc) && !(npc.aiBrain.bestAction is Playe))
        {
            npcCount++;
            if (npcCount >= 1)
            {
                door.OpenDoor();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<NPCController>(out _))
        {
            npcCount--;
            if (npcCount <= 0)
            {
                door.CloseDoor();
            }
        }
    }
}
