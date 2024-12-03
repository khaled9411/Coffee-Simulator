using System.Collections.Generic;
using UnityEngine;

public class FoodOrderSystem : MonoBehaviour
{
    public enum FoodType { None, Meat, Fries, Pizza, Burger }

    public class CustomerOrder
    {
        public FoodType Order { get; set; }
        public bool IsOrderServed { get; set; }
    }

    private CustomerOrder order;

    public CustomerOrder CreateOrder()
    {

        if (Random.Range(0f, 1f) < 0.5f)
        {
            FoodType randomOrder = (FoodType)Random.Range(1, 5); //NOT Includes "None"
            order = new CustomerOrder
            {
                Order = randomOrder,
                IsOrderServed = false
            };

            CafeteriaSystem.instance.RegisterCustomerOrder(transform, randomOrder);
        }
        else
        {
            order = null;
        }
        return order;
    }

    public bool CheckIfOrderServed()
    {
        if (order != null)
        {
            return order.IsOrderServed;
        }
        return false;
    }

    public void ServeOrder()
    {
        if (order != null)
        {
            order.IsOrderServed = true;
            CafeteriaSystem.instance.RemoveCustomerOrder(transform);
        }
    }

    public void ResetOrders()
    {
        order = null;
        Debug.Log("All orders have been reset.");
    }
}
