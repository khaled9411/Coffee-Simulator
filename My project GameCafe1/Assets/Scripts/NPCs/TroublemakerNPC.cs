using UnityEngine;

// Troublemaker NPC class that inherits from NPCBase, representing troublemakers with disruptive behaviors
public class TroublemakerNPC : NPCBase
{
    private void Start()
    {
        npcType = NPCType.Troublemaker; // Set the NPC type to Troublemaker
    }

    // Define random disruptive actions for troublemakers (vandalizing or stealing)
    public override void PerformRole()
    {
        if (Random.value > 0.5f)
        {
            Vandalize(); // Action for vandalizing
        }
        else
        {
            Steal(); // Action for stealing
        }
    }

    // Method for vandalizing action
    private void Vandalize()
    {
        Debug.Log(npcName + " is vandalizing the equipment.");
    }

    // Method for stealing action
    private void Steal()
    {
        Debug.Log(npcName + " is stealing items.");
    }
}
