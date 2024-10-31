using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HungerSystem.Instance.OnHungerAmountChange += EvaluateHunger;
    }

    private void EvaluateHunger(float hungerAmount)
    {
        if (hungerAmount <= 0) 
        {
            Faint.Instance.StartFaint();
            HungerSystem.Instance.IncreaseHunger(10);
        }
    }
}
