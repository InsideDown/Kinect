using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LightController : MonoBehaviour
{

    public string LightURL = "http://10.0.0.4/api/";
    public string UserID = "EEPOy42DbV82fdvDeoIAIn12yNRqWyfZPQZlcuqe";
    public string LightID = "3";
    public float LightOffTime = 5.0f;

    private string _LightURL = "";
    private string _LightStateURL = "";
    private List<string> _LightColors = new List<string> { "red", "green", "blue" };
    private Coroutine AnimCoroutine;
    private Coroutine LightPutCoroutine;
    private Coroutine LightOffCoroutine; //constantly runs to turn light off
    private bool _IsLightOn = false;
    private int _CurColor = 0;

    private void Awake()
    {
        _LightURL = LightURL + UserID;
        _LightStateURL = _LightURL + "/lights/" + LightID + "/state";
    }

    private void OnEnable()
    {
        EventManager.OnLightTriggerEvent += EventManager_OnLightTriggerEvent;
    }

    private void OnDisable()
    {
        EventManager.OnLightTriggerEvent -= EventManager_OnLightTriggerEvent;
    }

    public void TurnLightOff()
    {
        _IsLightOn = false;
        StopAnim();
        string lightPayload = "{\"on\":false}";
        LightPutCoroutine = StartCoroutine(LightPutRequest(_LightStateURL, lightPayload));
    }

    public void TurnLightOn()
    {
        _IsLightOn = true;
        StopAnim();
        string lightPayload = "{\"on\":true}";
        LightPutCoroutine = StartCoroutine(LightPutRequest(_LightStateURL, lightPayload));
        LightOffCoroutine = StartCoroutine(LightOffTimer());
    }

    private void LightOn()
    {
        string colorStr = _LightColors[_CurColor];
        EventManager.Instance.LightColorEvent(colorStr);
        StartCoroutine(LightDelayOn(colorStr));
    }

    IEnumerator LightDelayOn(string colorStr)
    {
        yield return new WaitForSeconds(0.5f);
        switch (colorStr)
        {
            case "red":
                SetLightRed();
                break;
            case "green":
                SetLightGreen();
                break;
            case "blue":
                SetLightBlue();
                break;
        }
        _CurColor++;
        if (_CurColor >= _LightColors.Count)
            _CurColor = 0;
    }

    private IEnumerator LightOffTimer()
    {
        yield return new WaitForSeconds(LightOffTime);
        TurnLightOff();
    }

    private IEnumerator LightPutRequest(string url, string payload)
    {
        if (!string.IsNullOrEmpty(payload))
        {
            byte[] payloadData = System.Text.Encoding.UTF8.GetBytes(payload);
            UnityWebRequest www = UnityWebRequest.Put(url, payloadData);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log("Upload complete!");
            }
        }
    }

    public void SetLightRed()
    {
        StopAnim();
        Vector2 redColor = RGBToXY(255, 0, 0);
        string xyVal = "[" + redColor.x + "," + redColor.y + "]";
        string lightPayload = "{\"on\":true, \"bri\": 254, \"sat\":0, \"xy\":" + xyVal + "}";
        LightPutCoroutine = StartCoroutine(LightPutRequest(_LightStateURL, lightPayload));
        LightOffCoroutine = StartCoroutine(LightOffTimer());
    }

    public void SetLightGreen()
    {
        StopAnim();
        Vector2 greenColor = RGBToXY(0, 255, 0);
        string xyVal = "[" + greenColor.x + "," + greenColor.y + "]";
        string lightPayload = "{\"on\":true, \"bri\": 254, \"sat\":0, \"xy\":" + xyVal + "}";
        LightPutCoroutine = StartCoroutine(LightPutRequest(_LightStateURL, lightPayload));
        LightOffCoroutine = StartCoroutine(LightOffTimer());
    }

    public void SetLightBlue()
    {
        StopAnim();
        Vector2 blueColor = RGBToXY(0, 0, 255);
        string xyVal = "[" + blueColor.x + "," + blueColor.y + "]";
        string lightPayload = "{\"on\":true, \"bri\": 254, \"sat\":0, \"xy\":" + xyVal + "}";
        LightPutCoroutine = StartCoroutine(LightPutRequest(_LightStateURL, lightPayload));
        LightOffCoroutine = StartCoroutine(LightOffTimer());
    }


    /// <summary>
    /// cancel our light animation
    /// </summary>
    private void StopAnim()
    {
        if (AnimCoroutine != null)
            StopCoroutine(AnimCoroutine);

        if (LightPutCoroutine != null)
            StopCoroutine(LightPutCoroutine);

        if(LightOffCoroutine != null)
            StopCoroutine(LightOffCoroutine);

        AnimCoroutine = null;
        LightPutCoroutine = null;
        LightOffCoroutine = null;
    }

    private Vector2 RGBToXY(int r, int g, int b)
    {
        float cR = r / 255;
        float cG = g / 255;
        float cB = b / 255;

        float red = (cR > 0.04045) ? Mathf.Pow((cR + 0.055f) / (1.0f + 0.055f), 2.4f) : (cR / 12.92f);
        float green = (cG > 0.04045) ? Mathf.Pow((cG + 0.055f) / (1.0f + 0.055f), 2.4f) : (cG / 12.92f);
        float blue = (cB > 0.04045) ? Mathf.Pow((cB + 0.055f) / (1.0f + 0.055f), 2.4f) : (cB / 12.92f);

        float X = red * 0.664511f + green * 0.154324f + blue * 0.162028f;
        float Y = red * 0.283881f + green * 0.668433f + blue * 0.047685f;
        float Z = red * 0.000088f + green * 0.072310f + blue * 0.986039f;

        float finalX = X / (X + Y + Z);
        float finalY = Y / (X + Y + Z);

        return new Vector2(finalX, finalY);
    }

    private void EventManager_OnLightTriggerEvent()
    {
        Debug.Log("trigger the light");
        LightOn();
    }
}
