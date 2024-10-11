using UnityEngine;

// Enum representing the types of NPCs
public enum NPCType
{
    Worker,
    Customer,
    Troublemaker
}

// Base class for all NPCs with shared properties and behaviors
public abstract class NPCBase : MonoBehaviour
{
    public string npcName; // Name of the NPC
    public NPCType npcType; // Type of the NPC (Worker, Customer, or Troublemaker)

    // Abstract method for each NPC to implement specific role behavior
    public abstract void PerformRole();
}
