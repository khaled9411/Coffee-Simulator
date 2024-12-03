using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IInteractable , Ibuyable
{
    public string verbName { get; set; } = "Eat";
    [SerializeField] private FoodSO foodSO;
    [SerializeField] private float timeToRespawn = 30;
    [SerializeField] private GameObject visual;
    private float currentTime;
    private Collider myCollider;
    
    //private bool canBuy = false;
    void Start()
    {
        if(TryGetComponent<Targetable>(out Targetable targetable))
        {
            targetable.SetName(foodSO.name);
        }
        
    }
    private void Update()
    {
        if(currentTime> 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime< 0 )
            {
                ShowVisuals();
                EnableCollider();
            }
        }
    }
    public float GetPrice()
    {
        return foodSO.price;
    }

    public void Interact()
    {
        Debug.Log($"you buoght a {foodSO.name} with {GetPrice()} and got {foodSO.hungerRecoveryAmount}");
        HungerSystem.Instance.OnEatFood?.Invoke(foodSO.hungerRecoveryAmount);
        HideVisuals();
        DisableCollider();
    }
    private void ShowVisuals()
    {
        visual.SetActive(true);
    }
    private void HideVisuals()
    {
        visual.SetActive(false);
        Invoke("ShowVisuals", timeToRespawn);
        Invoke("EnableCollider", timeToRespawn);
    }
    private void EnableCollider()
    {
        myCollider.enabled = true;
    }
    private void DisableCollider()
    {
        myCollider.enabled = false;
    }
}
