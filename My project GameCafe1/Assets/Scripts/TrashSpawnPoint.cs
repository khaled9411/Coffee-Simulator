using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawnPoint : MonoBehaviour
{
    public static int trashCounter { get; private set; } = 0;
    public static int spawnPointsCounter { get; private set; } = 0;
    public static int[] areasSpawnPointsCounter = new int[6];
    public static int[] areasTrashCounter = new int[6];
    public static int IsClean(int departmentNumber)
    {
        float cleanPercentage = areasTrashCounter[departmentNumber]/ areasSpawnPointsCounter[departmentNumber];
        if (cleanPercentage > .5f)
        {
            return -1;
        }else if (cleanPercentage == 0 ) // no trash at all
        {
            return 1;
        }
        return 0;
    }

    [SerializeField] private GameObject[] trashGameObjects;
    [SerializeField] private int myDepartmentNumber;
    [SerializeField] private float maxSpawnTime = 200f;
    [SerializeField] private float minSpawnTime = 60f;
    private GameObject activeTrash;

    [Header("-----Debug-----")]
    [SerializeField] private bool isMyDepartmentOpen;// sould be private

    private void Awake()
    {
        spawnPointsCounter++;
        areasSpawnPointsCounter[myDepartmentNumber]++;
    }

    private void Start()
    {
        Debug.Log("number of Trash Spawn Points: " + spawnPointsCounter);
        CafeManager.instance.OnAreaOppened += CafeManager_OnAreaOppened;
        CafeManager.instance.OnIsOpenChanage += CafeManager_OnIsOpenChange;

        ServicesUI.Instance.OnBuyJanitor += ServicesUI_OnBuyJanitor;
    }

    private void ServicesUI_OnBuyJanitor()
    {
        if(activeTrash != null)
            CleanTrash();
    }

    private void CafeManager_OnAreaOppened()
    {
        if(CafeManager.instance.GetCurrentAreaIndex() >= myDepartmentNumber)
        {
            OnMyDepartmentOpen();
        }
    }
    private void CafeManager_OnIsOpenChange(bool _)
    {
        if (CanSpawnTrash())
        {
            Invoke("SpawnTrash", Random.Range(minSpawnTime, maxSpawnTime));
        }
        else
        {
            CancelInvoke("SpawnTrash");
        }
    }

    private void OnMyDepartmentOpen()
    {
        isMyDepartmentOpen = true;
        if (CanSpawnTrash())
        {
            Invoke("SpawnTrash",Random.Range(minSpawnTime,maxSpawnTime));
        }
        else
        {
            CancelInvoke("SpawnTrash");
        }
    }
    private void SpawnTrash()
    {
        if (CanSpawnTrash())
        {
            trashCounter++;
            areasTrashCounter[myDepartmentNumber]++;
            activeTrash = trashGameObjects[Random.Range(0, trashGameObjects.Length)];
            activeTrash.SetActive(true);
            Debug.Log("trash Number: " + trashCounter);
        }
    }
    private bool CanSpawnTrash()
    {
        // this should also return that the store is oppened
        return IsMyDepartmentOpen() && CafeManager.instance.isOpen && activeTrash == null;
    }
    private bool IsMyDepartmentOpen()
    {
        if (CafeManager.instance.GetCurrentAreaIndex() >= myDepartmentNumber)
        {
            isMyDepartmentOpen = true;
        }
        else
        {
            isMyDepartmentOpen= false;
        }
        return isMyDepartmentOpen;

    }
    public void CleanTrash()
    {
        activeTrash.SetActive(false);
        activeTrash = null;
        trashCounter--;
        areasTrashCounter[myDepartmentNumber]--;
        Debug.Log("trash Number: " + trashCounter);
        Invoke("SpawnTrash", Random.Range(minSpawnTime, maxSpawnTime));
    }
}
