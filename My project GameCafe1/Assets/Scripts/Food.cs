using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IInteractable , Ibuyable
{
    public string verbName { get; set; } = "Eat";
    [SerializeField] private FoodSO foodSO;

    private bool canBuy = false;
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
        Debug.Log($"you buoght a {foodSO.name} with {GetPrice()} and got {foodSO.hungerRecoveryAmount}");
        gameObject.SetActive(false);
    }

}
