using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Category", menuName = "Shop/Category")]
public class ItemCategory : ScriptableObject
{
    public string categoryName; // Name of the category (e.g., Food, Devices)
    public List<ItemData> items; // List of items in this category
}
