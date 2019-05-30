using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class register : MonoBehaviour
{
    public GameObject textObjectState;
    private TextMesh txt;
    // Start is called before the first frame update
    void Start()
    {
        txt = textObjectState.GetComponentInChildren<TextMesh>();

    }

    private void Awake()
    {
        txt = textObjectState.GetComponentInChildren<TextMesh>();
        RegisterButtons();
    }

    public void RegisterButtons()
    {
        var receiver = GameObject.Find("Receiver").GetComponent<Receiver>();
        if (receiver != null)
        {
            GameObject[] objetos = GameObject.FindGameObjectsWithTag("Interactable");

            foreach (GameObject t in objetos)
            {
                receiver.Registerinteractable(t);
                Debug.Log("Registrado:" + t.gameObject.name);
                txt.text = "Registrado:" + t.gameObject.name;
            }
        }
        else
        {
            Debug.Log("Receiver null");
            txt.text = "Receiver null";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
