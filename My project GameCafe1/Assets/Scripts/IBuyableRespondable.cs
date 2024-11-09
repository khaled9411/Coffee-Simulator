using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuyableRespondable : IRespondable
{
   public bool isAvailable {  get; set; }
   public float price {  get; set; }
    public bool IsPurchased();
}
