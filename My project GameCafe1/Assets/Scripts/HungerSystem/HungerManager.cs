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

    bool once = true;
    private void EvaluateHunger(float hungerAmount)
    {

        if (!Food.IsFoodTutorialDone && hungerAmount < 10 && once) 
        {
            TutorialSystem.instance.OpenTutorialPopupByIndex(0);
            once = false;
        }



        if (hungerAmount <= 0) 
        {
            Debug.Log($"hungerAmount is {hungerAmount} and StartFaint");
            Faint.Instance.StartFaint();
            HungerSystem.Instance.IncreaseHunger(10);
        }
    }
}
