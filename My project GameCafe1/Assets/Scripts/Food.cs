using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IInteractable , Ibayable
{
    public string verbName { get; set; } = "Eat";
    [SerializeField] private FoodSO foodSO;
    void Start()
    {
        if(TryGetComponent<Targetable>(out Targetable targetable))
        {
            targetable.SetName(foodSO.name);
        }
    }
    public float GetPrice()
    {
        return foodSO.price;
    }

    public void Interact()
    {
        Debug.Log($"you buoght a {foodSO.name} with {foodSO.price} and got {foodSO.hungerRecoveryAmount}");
        Destroy(gameObject);
    }
}
