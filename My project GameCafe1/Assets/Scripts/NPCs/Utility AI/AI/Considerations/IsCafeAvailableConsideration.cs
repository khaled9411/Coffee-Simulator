using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TL.Core;
using TL.UtilityAI;

namespace TL.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "IsCafeAvailableConsideration", menuName = "UtilityAI/Considerations/Is Cafe Available Consideration")]
    public class IsCafeAvailableConsideration : Consideration
    {
        private AnimationCurve responseCurve;
        public override float ScoreConsideration(NPCController npc)
        {
            if (CafeManager.instance.isOpen && CafeManager.instance.HasAvailableSeats())
            {
                score = 1;
            }
            else
            {
                score = 0;
            }

            score = Mathf.Clamp01(score);
            return score;
        }
    }
}