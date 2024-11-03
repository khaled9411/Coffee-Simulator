using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TL.UtilityAI;
using TL.Core;

namespace TL.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "Work", menuName = "UtilityAI/Actions/Work")]
    public class Work : Action
    {
        public override void Execute(NPCController npc)
        {
            npc.DoWork(3);
        }

        public override void SetRequiredDestination(NPCController npc)
        {
            RequiredDestination = npc.context.storage.transform;
            npc.mover.destination = RequiredDestination;
        }
    }
}