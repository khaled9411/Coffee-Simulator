using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TL.UtilityAI;
using TL.Core;

namespace TL.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "Walk", menuName = "UtilityAI/Actions/Walk")]
    public class Walk : Action
    {
        public override void Execute(NPCController npc)
        {
            npc.aiBrain.finishedExecutingBestAction = true;
        }

        public override void SetRequiredDestination(NPCController npc)
        {
            RequiredDestination = npc.context.walkPoints[Random.Range(0, npc.context.walkPoints.Length)];
            npc.mover.destination = RequiredDestination;
        }
    }
}