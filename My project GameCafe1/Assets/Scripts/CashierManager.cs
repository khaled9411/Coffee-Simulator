using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TL.Core;
using TL.UtilityAI;
using UnityEngine;

public class CashierManager : MonoBehaviour
{
    public static CashierManager instance { get; private set; }

    public Transform[] queuePositions;
    public List<Tuple<Transform, Transform>> customerPos = new List<Tuple<Transform, Transform>>();
    public Queue<Transform> customersQueue = new Queue<Transform>();
    [SerializeField] private bool IsPlayerOnCashier;
    [SerializeField] private float customerCashierTime = 1;
    private float currentCustomerCashierTime;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("HI I have a brother");
        }
    }


    private void Start()
    {
        currentCustomerCashierTime = customerCashierTime;
        foreach (Transform pos in queuePositions)
        {
            customerPos.Add(new Tuple<Transform, Transform>(pos, null));
        }
    }
    private void Update()
    {
        if (IsPlayerOnCashier && IsFirstNPCArrived()) // and the npc arrived to the cashier
        {
            currentCustomerCashierTime -= Time.deltaTime;
            if (currentCustomerCashierTime <= 0)
            {
                currentCustomerCashierTime = customerCashierTime;
                RemoveCustomer();
            }
        }
    }
    private bool IsFirstNPCArrived()
    {
        Debug.Log($"first: {HasCustomer()} , second: {Vector3.Distance(customerPos[0].Item1.transform.position, customerPos[0].Item2.transform.position) < .2f}");
        return HasCustomer() && Vector3.Distance(customerPos[0].Item1.transform.position, customerPos[0].Item2.transform.position) < 1.1f;
    }
    private bool HasCustomer()
    {
        return customerPos.Count > 0;
    }
    public void AddCustomer(Transform customer)
    {
        MoveController moveController = customer.GetComponent<MoveController>();

        for (int i = 0; i < customerPos.Count; i++)
        {
            if (customerPos[i].Item2 == null)
            {
                customerPos[i] = new Tuple<Transform, Transform>(customerPos[i].Item1, customer);

                //*
                if (moveController != null)
                {
                    moveController.destination = customerPos[i].Item1;
                }

                return;
            }
        }
        customersQueue.Enqueue(customer);
    }

    public void RemoveCustomer()
    {
        //*
        AIBrain aIBrain = customerPos[0].Item2.GetComponent<AIBrain>();
        if (aIBrain != null)
        {
            aIBrain.finishedExecutingBestAction = true;
        }

        for (int i = 0; i < customerPos.Count; i++)
        {
            if (i + 1 < customerPos.Count)
            {
                customerPos[i] = new Tuple<Transform, Transform>(customerPos[i].Item1, customerPos[i + 1].Item2);

                if (customerPos[i].Item2 == null)
                {
                    //*
                   
                    return;
                }
                else
                {
                    MoveController moveController = customerPos[i].Item2.GetComponent<MoveController>();
                    if (moveController != null)
                    {
                        moveController.destination = customerPos[i].Item1;
                    }
                }
            }
            else if (customersQueue.Count > 0)
            {
                customerPos[i] = new Tuple<Transform, Transform>(customerPos[i].Item1, customersQueue.Dequeue());

                //*
                MoveController moveController = customerPos[i].Item2.GetComponent<MoveController>();
                if (moveController != null)
                {
                    moveController.destination = customerPos[i].Item1;
                }

            }
            else
            {
                customerPos[i] = new Tuple<Transform, Transform>(customerPos[i].Item1, null);
            }
        }
    }

}
