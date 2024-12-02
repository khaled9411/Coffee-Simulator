using System.Collections;
using TL.UtilityAI;
using UnityEngine;


public class TroublemakerNPC : MonoBehaviour
{
    private bool _canMakeTrouble;
    public bool canMakeTrouble { 
        get { return _canMakeTrouble; } 
        private set { _canMakeTrouble = value; } }

    [SerializeField] private float timeToReset = 5f;

    private void Start()
    {
        canMakeTrouble = true;
        SetCanMakeTroubleFALSE();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SetCanMakeTroubleFALSE();
            GetComponent<AIBrain>().finishedExecutingBestAction = true;
        }
    }

    public void SetCanMakeTroubleFALSE()
    {
        if (canMakeTrouble)
        {
            canMakeTrouble = false;
            StartCoroutine(ResetBoolAfterDelay());
        }
    }

    private IEnumerator ResetBoolAfterDelay()
    {
        yield return new WaitForSeconds(timeToReset);
        canMakeTrouble = true;
    }
}
