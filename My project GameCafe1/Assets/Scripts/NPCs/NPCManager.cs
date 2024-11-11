using System.Collections.Generic;
using UnityEngine;

// Manager class for handling NPC creation and management
public class NPCManager : MonoBehaviour
{
    public List<NPCBase> allNPCs = new List<NPCBase>(); // List to store all NPC instances

    // Method to add NPCs to the list and initialize their role
    public void AddNPC(NPCBase npc)
    {
        allNPCs.Add(npc);
        npc.PerformRole(); // Trigger the NPC's role behavior
    }

    private void Update()
    {
        // Placeholder for managing NPC behaviors on each frame
        foreach (var npc in allNPCs)
        {
            // You could update NPC interactions, positions, etc. here
        }
    }

    // Method to spawn a worker NPC with a specified role
    public void SpawnWorker(string name, WorkerRole role)
    {
        WorkerNPC worker = new GameObject(name).AddComponent<WorkerNPC>();
        worker.npcName = name;
        worker.workerRole = role;
        AddNPC(worker);
    }

    // Method to spawn a customer NPC with a specified type
    public void SpawnCustomer(string name, CustomerType type)
    {
        CustomerNPC customer = new GameObject(name).AddComponent<CustomerNPC>();
        customer.npcName = name;
        customer.customerType = type;
        AddNPC(customer);
    }

    // Method to spawn a troublemaker NPC
    //public void SpawnTroublemaker(string name)
    //{
    //    TroublemakerNPC troublemaker = new GameObject(name).AddComponent<TroublemakerNPC>();
    //    troublemaker.npcName = name;
    //    AddNPC(troublemaker);
    //}
}
