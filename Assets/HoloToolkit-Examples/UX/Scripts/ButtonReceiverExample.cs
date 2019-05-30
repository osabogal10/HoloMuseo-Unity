// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using HoloToolkit.Unity.Receivers;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;
using HoloToolkit.Unity.Buttons;

public class ButtonReceiverExample : InteractionReceiver
    {
        public GameObject textObjectState;
        private TextMesh txt;
        private bool active;
        GameObject canvasGO;
        GameObject titleGO;
        VideoPlayer video;
        AudioSource audio;
        GameObject descriptionGO;
        Canvas canvas;

        private TargetCollection targets;
        string filePath = Application.streamingAssetsPath + "/targets.json";

    void Start() {
        StartCoroutine(GetData());
                active = false;
                txt = textObjectState.GetComponentInChildren<TextMesh>();
                foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Opciones"))
                {
                    obj.SetActive(false);
                }
        inicializarTargets();
        }

        void inicializarTargets()
    {
        string dataJson = File.ReadAllText(filePath);
        Debug.Log("Cargado JSON: " + dataJson);
        txt.text = "Cargado JSON";
        targets = TargetCollection.FromJSON(dataJson);
        Debug.Log(targets.SaveToString());
        VideoPlayer[] videos = FindObjectsOfType<VideoPlayer>();
        foreach (VideoPlayer v in videos)
        {
            Debug.Log(v.gameObject.name);
            Debug.Log("Root de video: " + v.transform.root.name);
            v.url = targets.buscarTarget(v.gameObject.transform.root.name).urlVideo;
            StartCoroutine(prepareVideo(v));
        }
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in audios)
        {
            StartCoroutine(GetAudioClip(a));
        }
    }

        protected override void InputDown(GameObject obj, InputEventData eventData) {

        string rootName = obj.transform.root.name;
        Debug.Log("Root Object: "+obj.transform.root.name);

        string TargetName = obj.transform.root.gameObject.name;
        Text[] TextArrays = obj.GetParentRoot().GetComponentsInChildren<Text>();

        DataTarget data = targets.buscarTarget(rootName);

        //Text objDesc = descripcion.GetComponent<Text>();
        switch (obj.name)
        {
            case "MainButton":
                // Do something when circle is pressed
                if (active)
                {
                    active = !active;
                    obj.transform.parent.Find("ButtonGroup").gameObject.SetActive(active);
                }
                else
                {
                    active = !active;
                    obj.transform.parent.Find("ButtonGroup").gameObject.SetActive(active);
                }
                break;

            case "Historia":


                foreach (Text t in TextArrays)
                {
                    string textName = t.transform.gameObject.name;
                    if (data != null)
                    {
                        if (textName == "Description")
                        {

                            t.text = data.descripcionHistoria;
                        }
                        if (textName == "Title")
                        {
                            t.text = data.tituloHistoria;
                        }
                    }
                    else {
                        if (textName == "Description")
                        {

                            t.text = "No encontrado";
                        }
                        if (textName == "Title")
                        {
                            t.text = "Objeto";
                        }
                    }


                }
                break;

            case "Tesis":
                foreach (Text t in TextArrays)
                {
                    string textName = t.transform.gameObject.name;
                    if (data != null)
                    {


                        if (textName == "Description")
                        {

                            t.text = data.descripcionTesis;
                        }
                        if (textName == "Title")
                        {
                            t.text = data.tituloTesis;
                        }
                    }

                    else
                    {
                        if (textName == "Description")
                        {

                            t.text = "No encontrado";
                        }
                        if (textName == "Title")
                        {
                            t.text = "Objeto";
                        }
                    }

                }
                break;

            case "Demos":
                foreach (Text t in TextArrays)
                {
                    string textName = t.transform.gameObject.name;
                    if (data != null)
                    {
                        if (textName == "Description")
                        {

                            t.text = data.descripcionDemos;
                        }
                        if (textName == "Title")
                        {
                            t.text = data.tituloDemos;
                        }
                    }
                    else
                    {
                        if (textName == "Description")
                        {

                            t.text = "No encontrado";
                        }
                        if (textName == "Title")
                        {
                            t.text = "Objeto";
                        }
                    }


                }
                break;

            case "PlayButton":
                video = obj.GetParentRoot().GetComponentInChildren<VideoPlayer>();
                if (video.isPrepared)
                {
                    if (video.isPlaying)
                    {
                        video.Pause();
                    }
                    else
                    {
                        video.Play();
                    }
                    detenerVideos(rootName);
                }
                else
                {
                    //video.Prepare();
                    Debug.Log("Video " + obj.name + " no preparado.");
                }

                break;

            case "TTSButton":
                
                audio = obj.GetParentRoot().GetComponentInChildren<AudioSource>();
                if (audio.isPlaying)
                {
                    audio.Pause();
                    obj.GetComponent<CompoundButtonText>().Text = "Reproducir";
                    Debug.Log("Pausando clip");
                }
                else
                {
                    audio.Play();
                    obj.GetComponent<CompoundButtonText>().Text = "Pausar";
                    Debug.Log("Reproduciendo clip");
                }
                silenciarAudios(rootName);
                break;

            case "ResetButton":
                audio = obj.GetParentRoot().GetComponentInChildren<AudioSource>();
                audio.Stop();
                Debug.Log("Audio Detenido");
                GameObject tts = obj.transform.parent.Find("TTSButton").gameObject;
                tts.GetComponent<CompoundButtonText>().Text = "Reproducir";
                silenciarAudios("ninguno");
                break;

            default:
                foreach (Text t in TextArrays)
                {
                    string textName = t.transform.gameObject.name;
                    if (textName == "Description")
                    {

                        t.text = "No encontrado";
                    }
                    if (textName == "Title")
                    {
                        t.text = obj.name + "de : " + rootName;
                    }
                }
                break;

                //}
                /*
                if (obj.name == "Historia" || obj.name == "Demos" || obj.name == "Tesis")
                {
                    Debug.Log(obj.name + "Pressed");
                    GameObject titulo = obj.transform.root.Find("Contenido/Panel/TextContent/Title").gameObject;
                    GameObject descripcion = obj.transform.root.Find("Contenido/Panel/TextContent/Description").gameObject;
                    titulo.GetComponent<Text>().text = obj.name + "de : " + obj.transform.root.name;
                    descripcion.GetComponent<Text>().text = "Descripción de: " + obj.transform.root.name;
                } */
                break;
            }

        }

    public void silenciarAudios(string name)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("TTSAudio"))
        {
            string rootName = obj.transform.root.name;
            if (!rootName.Equals(name))
            {
                obj.GetComponent<AudioSource>().Stop();
            }
        }
        
    }

    public void detenerVideos(string name)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Movie"))
        {
            string rootName = obj.transform.root.name;
            if (!rootName.Equals(name))
            {
                obj.GetComponent<VideoPlayer>().Pause();
            }
        }

    }

    IEnumerator<int> prepareVideo(VideoPlayer player)
    {
        player.Prepare();

        //Wait until video is prepared
        while (!player.isPrepared)
        {
            Debug.Log("Preparing Video");
            yield return 1;
        }
    }

    IEnumerator GetAudioClip(AudioSource audioSource)
    {
        string url = targets.buscarTarget(audioSource.gameObject.transform.root.name).urlAudio;
        Debug.Log("La url es: "+url);
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.LogError(uwr.error);
                yield break;
            }

            AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
            audioSource.clip = clip;
            //audioSource.loop = true;
            //audioSource.Play();
                
        }
    }

    IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://172.24.42.35:8080/targets.json");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error: " + www.error);
            txt.text = "Error: " + www.error;
        }
        else
        {
            // Show results as text
            Debug.Log("Resultado: " + www.downloadHandler.text);
            txt.text = "Resultado: " + www.downloadHandler.isDone;
            
            File.WriteAllText(filePath, www.downloadHandler.text);
            txt.text = "Archivo Listo";

            //GameObject.Find("Loading").SetActive(false);
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }


    protected override void InputUp(GameObject obj, InputEventData eventData) {
            Debug.Log(obj.name + " : InputUp");
            txt.text = obj.name + " : InputUp";
        }
    }
