using UnityEngine;
using System.Collections;

public class CoinGenerator : MonoBehaviour
{
    public int coinsToAdd = 50;
    public float interval = 30f;

    private void Start()
    {
        StartCoroutine(AddCoinsEveryInterval());
    }

    private IEnumerator AddCoinsEveryInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            MoneyManager.Instance.AddMoney(coinsToAdd);
            Debug.Log($"{coinsToAdd} coins added. Current money: {MoneyManager.Instance.Money}");
        }
    }
}
