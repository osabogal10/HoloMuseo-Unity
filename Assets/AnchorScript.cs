using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorScript : MonoBehaviour
{

    public WorldAnchorManager worldAnchorManager;
    // Start is called before the first frame update
    void Start()
    {
        AnchorIt();
    }

    public void AnchorIt()
    {
        worldAnchorManager.AttachAnchor(this.gameObject);
        this.gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    public void ReleaseAnchor()
    {
        worldAnchorManager.RemoveAnchor(this.gameObject);
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
