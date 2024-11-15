using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawnPoint : MonoBehaviour
{
    public static int trashCounter { get; private set; } = 0;
    public static int spawnPointsCounter { get; private set; } = 0;

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
    }

    private void Start()
    {
        Debug.Log("number of Trash Spawn Points: " + spawnPointsCounter);
        CafeManager.instance.OnAreaOppened += CafeManager_OnAreaOppened;
        CafeManager.instance.OnIsOpenChanage += CafeManager_OnIsOpenChange;
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
            activeTrash = trashGameObjects[Random.Range(0, trashGameObjects.Length)];
            activeTrash.SetActive(true);
            Debug.Log("trash Number: " + trashCounter);
        }
    }
    private bool CanSpawnTrash()
    {
        // this should also return that the store is oppened
        return isMyDepartmentOpen && CafeManager.instance.isOpen && activeTrash == null;
    }
    public void CleanTrash()
    {
        activeTrash.SetActive(false);
        activeTrash = null;
        trashCounter--;
        Debug.Log("trash Number: " + trashCounter);
        Invoke("SpawnTrash", Random.Range(minSpawnTime, maxSpawnTime));
    }
}
