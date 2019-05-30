using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetCollection 
{
    [SerializeField]
    List<DataTarget> targetList;

    public TargetCollection()
    {
        targetList = new List<DataTarget>();
    }

    public void addTarget(DataTarget target)
    {
        targetList.Add(target);
    }

    public DataTarget buscarTarget(string pTargetName)
    {
        DataTarget r = null;
        foreach(DataTarget dt in targetList)
        {
            Debug.Log("Target: "+dt.targetName);
            if (dt.targetName.Equals(pTargetName))
            {
                r = dt;
            }
        }
        return r;
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(targetList);
    }

    public static TargetCollection FromJSON(string data)
    {
        return JsonUtility.FromJson<TargetCollection>(data);
    }
}
