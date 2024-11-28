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
        [SerializeField] private AnimationCurve responseCurve;
        [SerializeField] private float timeToNewCustomer = 30;

        public override float ScoreConsideration(NPCController npc)
        {
            if (IsCafeAvailable())
            {
                npc.stats.energy = Random.Range(70, 100);
                npc.stats.money = Random.Range(700, 1000);
                npc.stats.hunger = Random.Range(0, 30);
                score = 1;
            }
            else
            {
                score = 0;
            }

            score = responseCurve.Evaluate(Mathf.Clamp01(score));
            return score;
        }

        private bool IsCafeAvailable()
        {
            return CafeManager.instance.isOpen //the cafe Open
                && CafeManager.instance.HasAvailableSeats() //there is a seats
                && Time.time - AIBrain.lastPlayTime >= (timeToNewCustomer + (timeToNewCustomer * (CafeManager.instance.GetOverallPricePercentage() / 100f))); //time to new customer passed
        }
    }
}