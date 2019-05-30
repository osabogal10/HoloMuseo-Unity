using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialToogle : MonoBehaviour
{
    public GameObject SpatialMappingPrefab;
    public enum ObserverStates
    {
        /// <summary>
        /// The SurfaceObserver is currently running.
        /// </summary>
        Running = 0,

        /// <summary>
        /// The SurfaceObserver is currently idle.
        /// </summary>
        Stopped = 1
    }

    public void toggleSpatialMapping()
    {
        SpatialMappingObserver comp = SpatialMappingPrefab.GetComponent<SpatialMappingObserver>();
        if (comp.ObserverState==0)
        {
            comp.StopObserving();
            SpatialMappingPrefab.SetActive(false);
        }
        else
        {
            SpatialMappingPrefab.SetActive(true);
            comp.StartObserving();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
