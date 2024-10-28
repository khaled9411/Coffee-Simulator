using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    string UniqueID { get; }
    SaveData SaveData();
    void LoadData(SaveData data);
}

