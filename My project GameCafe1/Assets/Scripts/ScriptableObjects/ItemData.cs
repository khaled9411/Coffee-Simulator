using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Shop/Item")]
public class ItemData : ScriptableObject
{
    public string itemName; // The name of the item
    public float price;     // The price of the item
    public string description; // A description of the item
}
