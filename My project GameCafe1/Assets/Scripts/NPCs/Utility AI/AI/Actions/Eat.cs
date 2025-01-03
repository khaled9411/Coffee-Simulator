using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TL.UtilityAI;
using TL.Core;

namespace TL.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "Eat", menuName = "UtilityAI/Actions/Eat")]
    public class Eat : Action
    {
        public override void Execute(NPCController npc)
        {
            Debug.Log("I ate food!");
            // Logic for updating everything involved with eating
            npc.DoEat(Random.Range(minTimeToExecute, maxTimeToExecute));
            //npc.OnFinishedAction();
        }

        public override void SetRequiredDestination(NPCController npc)
        {
            RequiredDestination = npc.context.restaurant.transform;
            npc.mover.destination = RequiredDestination;
        }
    }
}