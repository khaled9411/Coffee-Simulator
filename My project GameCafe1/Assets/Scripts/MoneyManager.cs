using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    // Singleton instance for easy access to the MoneyManager
    public static MoneyManager Instance;

    // Key to store the money value in PlayerPrefs
    private const string MoneyKey = "PlayerMoney";

    // Property to get and set the money value
    private int _money;
    public int Money
    {
        get { return _money; }
        private set
        {
            _money = value;
            SaveMoney(); // Save the money value whenever it changes
        }
    }

    // Awake is called before the first frame update
    private void Awake()
    {
        // Implement Singleton pattern to ensure only one instance of MoneyManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the MoneyManager alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

        LoadMoney(); // Load saved money value from PlayerPrefs
    }

    // Method to add money to the current balance
    public void AddMoney(int amount)
    {
        Money += amount;
    }

    // Method to subtract money from the current balance
    public void SubtractMoney(int amount)
    {
        if (Money >= amount) // Ensure that there is enough money to subtract
        {
            Money -= amount;
        }
        else
        {
            Debug.LogWarning("Not enough money to subtract");
        }
    }

    // Method to save the money value in PlayerPrefs
    private void SaveMoney()
    {
        PlayerPrefs.SetInt(MoneyKey, Money);
        PlayerPrefs.Save();
    }

    // Method to load the money value from PlayerPrefs
    private void LoadMoney()
    {
        Money = PlayerPrefs.GetInt(MoneyKey, 0); // Default to 0 if no value is found
    }

    // Method to reset money value (optional)
    public void ResetMoney()
    {
        Money = 0;
    }
}
