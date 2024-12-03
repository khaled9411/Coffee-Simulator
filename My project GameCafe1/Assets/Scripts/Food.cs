using HUDIndicator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IInteractable , Ibuyable
{
    public static bool IsFoodTutorialDone = false;

    public string verbName { get; set; } = "Eat";
    [SerializeField] private FoodSO foodSO;
    [SerializeField] private float timeToRespawn = 30;
    [SerializeField] private GameObject visual;
    private float currentTime;
    private Collider myCollider;
    
    //private bool canBuy = false;
    void Start()
    {
        IsFoodTutorialDone = PlayerPrefs.GetInt("IsFoodTutorialDone", 0) == 1;

        myCollider = GetComponent<Collider>();
        if (TryGetComponent<Targetable>(out Targetable targetable))
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

        if (!IsFoodTutorialDone)
        {
            IsFoodTutorialDone = true;
            PlayerPrefs.SetInt("IsFoodTutorialDone", 1);
            TutorialSystem.instance.parts[0].indicatorOnScreen.visible = false;
            TutorialSystem.instance.parts[0].indicatorOffScreen.visible = false;
        }
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
