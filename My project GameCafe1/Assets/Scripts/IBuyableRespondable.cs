using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuyableRespondable : IRespondable
{
   public float price {  get; set; }
}
