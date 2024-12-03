using System.Collections.Generic;
using UnityEngine;

public class CafeteriaSystem : MonoBehaviour
{
    public static CafeteriaSystem instance;

    public class CafeteriaResources
    {
        public int Meat { get; set; }
        public int Fries { get; set; }
        public int Pizza { get; set; }
        public int Burger { get; set; }
    }

    public CafeteriaWorker worker;

    private Dictionary<Transform, FoodOrderSystem.FoodType> customerOrders = new Dictionary<Transform, FoodOrderSystem.FoodType>();
    private CafeteriaResources resources = new CafeteriaResources { Meat = 10, Fries = 10, Pizza = 10, Burger = 10 };

    public CafeteriaResources Getesources()
    {
        return resources;
    }

    private void Awake()
    {
        instance = this;
    }

    public void RegisterCustomerOrder(Transform customer, FoodOrderSystem.FoodType order)
    {
        Debug.Log($"RegisterCustomerOrder {customer}");
        if (!customerOrders.ContainsKey(customer) && HasResources(order))
        {
            customerOrders[customer] = order;
        }
        else
        {
            Debug.Log("Cannot register order: Either already exists or insufficient resources.");
        }
    }

    public void RemoveCustomerOrder(Transform customer)
    {
        Debug.Log($"RemoveCustomerOrder {customer}");
        if (customerOrders.ContainsKey(customer))
        {
            customerOrders.Remove(customer);
        }
    }

    public Transform GetOrderToServe()
    {
        foreach (var customerOrder in customerOrders)
        {
            DeductResource(customerOrders[customerOrder.Key]);
            return customerOrder.Key;
        }

        Debug.Log("No orders to serve.");
        return null;
    }



    public bool HasResources(FoodOrderSystem.FoodType order)
    {
        switch (order)
        {
            case FoodOrderSystem.FoodType.Meat: return resources.Meat > 0;
            case FoodOrderSystem.FoodType.Fries: return resources.Fries > 0;
            case FoodOrderSystem.FoodType.Pizza: return resources.Pizza > 0;
            case FoodOrderSystem.FoodType.Burger: return resources.Burger > 0;
            default: return false;
        }
    }

    private void DeductResource(FoodOrderSystem.FoodType order)
    {
        switch (order)
        {
            case FoodOrderSystem.FoodType.Meat: resources.Meat--; break;
            case FoodOrderSystem.FoodType.Fries: resources.Fries--; break;
            case FoodOrderSystem.FoodType.Pizza: resources.Pizza--; break;
            case FoodOrderSystem.FoodType.Burger: resources.Burger--; break;
        }
    }

    public void ResetCafeteria()
    {
        customerOrders.Clear();
        worker.ResetWorker();
        Debug.Log("Cafeteria has been reset.");
    }
}
