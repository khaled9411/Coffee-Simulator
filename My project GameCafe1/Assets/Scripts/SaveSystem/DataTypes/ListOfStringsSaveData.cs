using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfStringsSaveData : SaveData
{
   public List<string> value;

    public ListOfStringsSaveData(List<string> value)
    {
        this.value = value;
    }
}
