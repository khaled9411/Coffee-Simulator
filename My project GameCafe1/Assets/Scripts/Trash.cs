using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] private TrashSpawnPoint spawnPoint;
    
    public void Clean()
    {
        spawnPoint.CleanTrash();
    }
}
