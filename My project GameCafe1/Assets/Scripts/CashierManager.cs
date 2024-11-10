using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierManager : MonoBehaviour
{
    public Transform[] queuePositions;
    public List<GameObject> customersQueue = new List<GameObject>();

    public void AddCustomer(GameObject customer)
    {
        customersQueue.Add(customer);
        UpdateQueuePositions();
    }

    public void RemoveCustomer()
    {
        if (customersQueue.Count > 0)
        {
            GameObject firstCustomer = customersQueue[0];
            customersQueue.RemoveAt(0);
            // make the customer go to another action
            UpdateQueuePositions();
        }
    }

    private void UpdateQueuePositions()
    {
        for (int i = 0; i < customersQueue.Count; i++)
        {
            customersQueue[i].transform.position = queuePositions[i].position;
        }
    }
}
