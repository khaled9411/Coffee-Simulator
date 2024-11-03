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
            npc.stats.hunger += 10;
            npc.stats.money -= 20;
            npc.stats.energy -= 20;
            npc.stats.cafe -= 10;
            npc.aiBrain.finishedExecutingBestAction = true;
            //npc.OnFinishedAction();
        }

        public override void SetRequiredDestination(NPCController npc)
        {
            RequiredDestination = npc.context.cafe.transform;
            npc.mover.destination = RequiredDestination;
        }
    }
}