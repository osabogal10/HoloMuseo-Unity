using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.Linq;
using UnityEngine.Networking;
using System.IO;

public class DataSetLoader : MonoBehaviour
{
    // Model is the GameObject to be augmented
    public GameObject Model;
    public GameObject ReceiverObject;
    private string dataDir;
    private string fileNameXML;
    private string fileNameDAT;
    public GameObject textObjectState;
    private TextMesh txt;

    // Use this for initialization
    void Start()
    {
        txt = textObjectState.GetComponentInChildren<TextMesh>();
        Debug.Log("Persistent dataPath: "+Application.persistentDataPath);
        dataDir = Application.persistentDataPath + "/data";
        fileNameXML = "HoloDB.xml";
        fileNameDAT = "HoloDB.dat";
        if (!Directory.Exists(dataDir))
        {
            Directory.CreateDirectory(dataDir);
        }
        StartCoroutine(GetData());
        
    }

    IEnumerator GetData()
    {
        UnityWebRequest wwwXML = UnityWebRequest.Get("http://holomuseo-server.herokuapp.com/" + fileNameXML);
        yield return wwwXML.SendWebRequest();

        if (wwwXML.isNetworkError || wwwXML.isHttpError)
        {
            Debug.Log("Error: " + wwwXML.error);
        }
        else
        {
            
            File.WriteAllBytes(dataDir + "/" + fileNameXML, wwwXML.downloadHandler.data);
        }
        UnityWebRequest wwwDAT = UnityWebRequest.Get("http://holomuseo-server.herokuapp.com/" + fileNameDAT);
        yield return wwwDAT.SendWebRequest();

        if (wwwDAT.isNetworkError || wwwDAT.isHttpError)
        {
            Debug.Log("Error: " + wwwDAT.error);
        }
        else
        {

            File.WriteAllBytes(dataDir + "/" + fileNameDAT, wwwDAT.downloadHandler.data);
        }
        // Registering call back to know when Vuforia is ready
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // This function is called when vuforia gives the started callback
    void OnVuforiaStarted()
    {

        // The 'path' string determines the location of xml file
        // For convinence the RealTime.xml is placed in the StreamingAssets folder
        // This file can be downloaded and the relative path will be used accordingly

        string path = "";
#if UNITY_IPHONE
		path = Application.dataPath + "/Raw/RealTime.xml";
#elif UNITY_ANDROID
		path = "jar:file://" + Application.dataPath + "!/assets/RealTime.xml";
#else
        //path = Application.dataPath + "/StreamingAssets/HoloDB.xml";
        path = dataDir+"/"+fileNameXML;
#endif

        bool status = LoadDataSet(path, VuforiaUnity.StorageType.STORAGE_ABSOLUTE);

        if (status)
        {
            Debug.Log("Dataset Loaded");
        }
        else
        {
            Debug.Log("Dataset Load Failed");
        }
    }

    // Load and activate a data set at the given path.
    private bool LoadDataSet(string dataSetPath, VuforiaUnity.StorageType storageType)
    {
        // Request an ImageTracker instance from the TrackerManager.
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        objectTracker.Stop();
        IEnumerable<DataSet> dataSetList = objectTracker.GetActiveDataSets();
        foreach (DataSet set in dataSetList.ToList())
        {
            objectTracker.DeactivateDataSet(set);
        }

        // Check if the data set exists at the given path.
        if (!DataSet.Exists(dataSetPath, storageType))
        {
            Debug.LogError("Data set " + dataSetPath + " does not exist.");
            return false;
        }

        // Create a new empty data set.
        DataSet dataSet = objectTracker.CreateDataSet();

        // Load the data set from the given path.
        if (!dataSet.Load(dataSetPath, storageType))
        {
            Debug.LogError("Failed to load data set " + dataSetPath + ".");
            return false;
        }

        // (Optional) Activate the data set.
        objectTracker.ActivateDataSet(dataSet);
        objectTracker.Start();

        AttachContentToTrackables(dataSet);

        return true;
    }

    // Add Trackable event handler and content (cubes) to the Targets.
    private void AttachContentToTrackables(DataSet dataSet)
    {
        // get all current TrackableBehaviours
        IEnumerable<TrackableBehaviour> trackableBehaviours =
        TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();

        // Loop over all TrackableBehaviours.
        foreach (TrackableBehaviour trackableBehaviour in trackableBehaviours)
        {
            // check if the Trackable of the current Behaviour is part of this dataset
            if (dataSet.Contains(trackableBehaviour.Trackable))
            {
                Debug.Log("Trackable name: " + trackableBehaviour.TrackableName);
                GameObject go = trackableBehaviour.gameObject;
                go.name = trackableBehaviour.TrackableName;

                // Add a Trackable event handler to the Trackable.
                // This Behaviour handles Trackable lost/found callbacks.
                go.AddComponent<DefaultTrackableEventHandler>();

                // Instantiate the model.
                // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                GameObject contenido = Instantiate(Model) as GameObject;

                // Attach the cube to the Trackable and make sure it has a proper size.
                contenido.transform.parent = trackableBehaviour.transform;
                contenido.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
                contenido.transform.localPosition = new Vector3(0.0f, 0.35f, 0.0f);
                contenido.transform.localRotation = Quaternion.identity;
                contenido.active = true;
                
                trackableBehaviour.gameObject.active = true;
            }
        }

        register reg = ReceiverObject.GetComponent<register>();
        Receiver rec = ReceiverObject.GetComponent<Receiver>();
        txt.text = "Va a registrar";
        reg.RegisterButtons();
        rec.inicializarTargets();
        txt.text = "Targets iniciados";
    }
}

