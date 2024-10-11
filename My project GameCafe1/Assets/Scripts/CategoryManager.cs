using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryManager : MonoBehaviour
{
    public List<ItemCategory> categories; // List of all available categories

    [Header("UI Elements")]
    public Dropdown categoryDropdown; // Dropdown UI element for selecting a category
    public Transform itemListContainer; // Container to hold and display item UI elements
    public GameObject itemUIPrefab; // Prefab for displaying individual item details in the UI

    private void Start()
    {
        PopulateCategoryDropdown(); // Fill the dropdown with category names
        categoryDropdown.onValueChanged.AddListener(OnCategorySelected); // Add listener for dropdown selection changes
    }

    // Populate the dropdown with available categories
    private void PopulateCategoryDropdown()
    {
        categoryDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (var category in categories)
        {
            options.Add(category.categoryName);
        }
        categoryDropdown.AddOptions(options);
    }

    // When a category is selected, display its items
    private void OnCategorySelected(int index)
    {
        DisplayItems(categories[index]);
    }

    // Display items of the selected category
    private void DisplayItems(ItemCategory category)
    {
        // Clear old items from the list
        foreach (Transform child in itemListContainer)
        {
            Destroy(child.gameObject);
        }

        // Instantiate and display new items
        foreach (var item in category.items)
        {
            GameObject itemUI = Instantiate(itemUIPrefab, itemListContainer);
            // Update item UI with item details
            itemUI.GetComponentInChildren<Text>().text = $"{item.itemName} - ${item.price}\n{item.description}";
        }
    }
}
