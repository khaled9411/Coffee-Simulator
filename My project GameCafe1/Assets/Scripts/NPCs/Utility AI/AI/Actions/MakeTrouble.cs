using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TL.UtilityAI;
using TL.Core;

namespace TL.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "Make Trouble", menuName = "UtilityAI/Actions/Make Trouble")]
    public class MakeTrouble : Action
    {
        public override void Execute(NPCController npc)
        {
            CafeManager.instance.overallSatisfactionRate -= 0.25f;
        }

        public override void SetRequiredDestination(NPCController npc)
        {
            RequiredDestination = npc.context.cafe.transform;
            npc.mover.destination = RequiredDestination;
        }
    }
}
