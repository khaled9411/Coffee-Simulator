using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCaster : MonoBehaviour
{
    [SerializeField] private float rayMaxDistance = 5;
   public Targetable FireRayCast()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayMaxDistance))
        {
            if (hit.collider.TryGetComponent<Targetable>(out Targetable target))
            {
                return target;
            }else return null;
        }
        return null;
    }
}
