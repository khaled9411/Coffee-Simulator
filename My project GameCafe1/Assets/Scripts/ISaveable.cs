using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    SaveData SaveData();
    void LoadData(SaveData data);
}

