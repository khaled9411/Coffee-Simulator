using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TL.UtilityAI;
using TL.Core;

namespace TL.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "Playe", menuName = "UtilityAI/Actions/Playe")]
    public class Playe : Action
    {
        public override void Execute(NPCController npc)
        {
            Debug.Log("I am Playe in the cafe!");
            // Logic for updating everything involved with Playing
            npc.DoPlay(3);
            //npc.OnFinishedAction();
        }

        public override void SetRequiredDestination(NPCController npc)
        {
            RequiredDestination = npc.availableDevice;
            npc.mover.destination = RequiredDestination;
            //Debug.LogWarning($"I am {npc} and my Destination is {RequiredDestination} in vector3 {RequiredDestination?.position}");
        }
    }
}