using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskUserHalloweenController : MonoBehaviour
{

    public RawImage BackgroundRawImage;

    private void Awake()
    {
        if (BackgroundRawImage == null)
            throw new System.Exception("A BackgroundRawImage must be defined in MaskUserHalloweenController");
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

    public void OnCaptureScreenshot()
    {
        CaptureImageFromKinect();
    }
}