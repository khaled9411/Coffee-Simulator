using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IHungerSystem
{
    void ReduceHungerOverTime();
    void IncreaseHunger(float amount);
}
