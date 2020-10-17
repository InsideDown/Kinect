﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskUserHalloweenController : MonoBehaviour
{

    public RawImage BackgroundRawImage;
    public List<GameObject> ObjectsToHide = new List<GameObject>();
    public GameObject Character;

    private Vector3 CharacterScale;
    private Vector3 HideCharacterScale = Vector3.zero;

    private void Awake()
    {
        if (BackgroundRawImage == null)
            throw new System.Exception("A BackgroundRawImage must be defined in MaskUserHalloweenController");

        if (Character == null)
            throw new System.Exception("A Character must be defined in MaskUserHalloweenController");

        CharacterScale = Character.transform.localScale;
        ShowCharacter(false);
    }

    private IEnumerator TakeSnapshot()
    {
        yield return new WaitForEndOfFrame();

        KinectManager kinectManager = KinectManager.Instance;

        if(kinectManager)
        {
            int imageWidth = kinectManager.GetColorImageWidth();
            int imageHeight = kinectManager.GetColorImageHeight();
            Texture2D texture = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, true);
            texture.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
            texture.LoadRawTextureData(texture.GetRawTextureData());
            texture.Apply();
            BackgroundRawImage.texture = texture;
            BackgroundRawImage.color = Color.white;
        }
        // gameObject.renderer.material.mainTexture = TakeSnapshot;
    }

    private void CaptureImageFromKinect()
    {
        BackgroundRemovalManager backManager = BackgroundRemovalManager.Instance;
        KinectManager kinectManager = KinectManager.Instance;

        if (kinectManager && backManager && backManager.enabled /**&& backManager.IsBackgroundRemovalInitialized()*/)
        {
            StartCoroutine(TakeSnapshot());

        }
        else if (kinectManager /**&& kinectManager.IsInitialized()*/)
        {
            Debug.Log("videorawImage2");
            SimpleBackgroundRemoval simpleBR = GameObject.FindObjectOfType<SimpleBackgroundRemoval>();
            bool isSimpleBR = simpleBR && simpleBR.enabled;

            Debug.Log("manager exists, doing something here");

            BackgroundRawImage.texture = kinectManager.GetUsersClrTex();  // color camera texture
            //BackgroundRawImage.rectTransform.localScale = kinectManager.GetColorImageScale();
            BackgroundRawImage.color = !isSimpleBR ? Color.white : Color.clear;
        }
    }

    private void HideObjects()
    {
        foreach(GameObject obj in ObjectsToHide)
        {
            obj.SetActive(false);
        }
    }

    private void ShowCharacter(bool showCharBool = true)
    {
       if(showCharBool)
        {
            Character.transform.localScale = CharacterScale;
        }
        else
        {
            Character.transform.localScale = HideCharacterScale;
        }
    }

    private void OnEnable()
    {
        EventManager.OnUserFoundEvent += EventManager_OnUserFoundEvent;
        EventManager.OnAllUsersLostEvent += EventManager_OnAllUsersLostEvent;
    }
    private void OnDisable()
    {
        EventManager.OnUserFoundEvent -= EventManager_OnUserFoundEvent;
        EventManager.OnAllUsersLostEvent -= EventManager_OnAllUsersLostEvent;
    }

    private void EventManager_OnAllUsersLostEvent()
    {
        ShowCharacter(false);
    }

    private void EventManager_OnUserFoundEvent()
    {
        ShowCharacter();
    }

    public void OnCaptureScreenshot()
    {
        HideObjects();
        CaptureImageFromKinect();
    }
}