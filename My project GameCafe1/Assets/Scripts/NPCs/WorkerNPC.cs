using UnityEngine;

// Enum representing specific worker roles
public enum WorkerRole
{
    Cleaner,
    Cook,
    Cashier
}

// Worker NPC class that inherits from NPCBase, representing workers with different roles
public class WorkerNPC : NPCBase
{
    public WorkerRole workerRole; // Role of the worker (Cleaner, Cook, Cashier)

    private void Start()
    {
        npcType = NPCType.Worker; // Set the NPC type to Worker
    }

    // Define the specific actions based on the worker's role
    public override void PerformRole()
    {
        switch (workerRole)
        {
            case WorkerRole.Cleaner:
                PerformCleaning();
                break;
            case WorkerRole.Cook:
                PrepareFood();
                break;
            case WorkerRole.Cashier:
                HandleCashRegister();
                break;
        }
    }

    // Method for cleaning action
    private void PerformCleaning()
    {
        Debug.Log(npcName + " is cleaning the area.");
    }

    // Method for food preparation action
    private void PrepareFood()
    {
        Debug.Log(npcName + " is preparing food.");
    }

    // Method for cash handling action
    private void HandleCashRegister()
    {
        Debug.Log(npcName + " is handling the cash register.");
    }
}
