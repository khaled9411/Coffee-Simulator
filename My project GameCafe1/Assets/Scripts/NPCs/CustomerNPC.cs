using UnityEngine;

// Enum representing customer types
public enum CustomerType
{
    Rich,
    Poor
}

// Customer NPC class that inherits from NPCBase, representing customers with different behaviors
public class CustomerNPC : NPCBase
{
    public CustomerType customerType; // Type of customer (Rich or Poor)

    private void Start()
    {
        npcType = NPCType.Customer; // Set the NPC type to Customer
    }

    // Define customer actions based on the type of customer
    public override void PerformRole()
    {
        PlayGames(); // Basic action of playing games for all customers
        if (customerType == CustomerType.Rich)
        {
            SpendMoreMoney(); // Additional action for rich customers
        }
        else if (customerType == CustomerType.Poor)
        {
            SpendLessMoney(); // Additional action for poor customers
        }
    }

    // Method for playing games action
    private void PlayGames()
    {
        Debug.Log(npcName + " is playing games.");
    }

    // Method for spending more money action (specific to rich customers)
    private void SpendMoreMoney()
    {
        Debug.Log(npcName + " is spending a lot of money.");
    }

    // Method for spending less money action (specific to poor customers)
    private void SpendLessMoney()
    {
        Debug.Log(npcName + " is spending less money.");
    }
}
