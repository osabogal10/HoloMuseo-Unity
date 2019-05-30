using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataTarget
{

    public string targetName;
    public string urlAudio;
    public string urlVideo;
    public string tituloHistoria;
    public string descripcionHistoria;
    public string tituloTesis;
    public string descripcionTesis;
    public string tituloDemos;
    public string descripcionDemos;

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
