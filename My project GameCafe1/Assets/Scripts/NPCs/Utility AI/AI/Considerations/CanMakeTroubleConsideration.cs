using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TL.Core;
using TL.UtilityAI;

namespace TL.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "CanMakeTroubleConsideration", menuName = "UtilityAI/Considerations/Can Make Trouble")]
    public class CanMakeTroubleConsideration : Consideration
    {
        [SerializeField] private AnimationCurve responseCurve;
        public override float ScoreConsideration(NPCController npc)
        {
            if(npc.troublemaker != null && npc.troublemaker.canMakeTrouble && CafeManager.instance.isOpen)
            {
                score = 1;
            }
            else
            {
                score = 0;
            }

            score = responseCurve.Evaluate(Mathf.Clamp01(score));
            return score;
        }
    }
}