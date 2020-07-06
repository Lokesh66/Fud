using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SafeArea : MonoBehaviour
{
    #region Simulations
    /// <summary>
    /// Simulation device that uses safe area due to a physical notch or software home bar. For use in Editor only.
    /// </summary>
    public enum SimDevice
    {
        /// <summary>
        /// Don't use a simulated safe area - GUI will be full screen as normal.
        /// </summary>
        None,
        /// <summary>
        /// Simulate the iPhone X and Xs (identical safe areas).
        /// </summary>
        iPhoneX,
        /// <summary>
        /// Simulate the iPhone Xs Max and XR (identical safe areas).
        /// </summary>
        iPhoneXsMax
    }

    /// <summary>
    /// Simulation mode for use in editor only. This can be edited at runtime to toggle between different safe areas.
    /// </summary>
    public static SimDevice Sim = SimDevice.None;

    /// <summary>
    /// Normalised safe areas for iPhone X with Home indicator (ratios are identical to iPhone Xs). Absolute values:
    ///  PortraitU x=0, y=102, w=1125, h=2202 on full extents w=1125, h=2436;
    ///  PortraitD x=0, y=102, w=1125, h=2202 on full extents w=1125, h=2436 (not supported, remains in Portrait Up);
    ///  LandscapeL x=132, y=63, w=2172, h=1062 on full extents w=2436, h=1125;
    ///  LandscapeR x=132, y=63, w=2172, h=1062 on full extents w=2436, h=1125.
    ///  Aspect Ratio: ~19.5:9.
    /// </summary>
    Rect[] NSA_iPhoneX = new Rect[]
    {
        new Rect (0f, 102f / 2436f, 1f, 2202f / 2436f),  // Portrait
        new Rect (132f / 2436f, 63f / 1125f, 2172f / 2436f, 1062f / 1125f)  // Landscape
    };

    /// <summary>
    /// Normalised safe areas for iPhone Xs Max with Home indicator (ratios are identical to iPhone XR). Absolute values:
    ///  PortraitU x=0, y=102, w=1242, h=2454 on full extents w=1242, h=2688;
    ///  PortraitD x=0, y=102, w=1242, h=2454 on full extents w=1242, h=2688 (not supported, remains in Portrait Up);
    ///  LandscapeL x=132, y=63, w=2424, h=1179 on full extents w=2688, h=1242;
    ///  LandscapeR x=132, y=63, w=2424, h=1179 on full extents w=2688, h=1242.
    ///  Aspect Ratio: ~19.5:9.
    /// </summary>
    Rect[] NSA_iPhoneXsMax = new Rect[]
    {
            new Rect (0f, 102f / 2688f, 1f, 2454f / 2688f),  // Portrait
            new Rect (132f / 2688f, 63f / 1242f, 2424f / 2688f, 1179f / 1242f)  // Landscape
    };
    #endregion

    RectTransform Panel;
    Rect LastSafeArea = new Rect(0, 0, 0, 0);
    [SerializeField] bool ConformX = true;  // Conform to screen safe area on X-axis (default true, disable to ignore)
    [SerializeField] bool ConformY = true;  // Conform to screen safe area on Y-axis (default true, disable to ignore)

    void Awake()
    {
        Panel = GetComponent<RectTransform>();

        if (Panel == null)
        {
            Debug.LogError("Cannot apply safe area - no RectTransform found on " + name);
            Destroy(gameObject);
        }

        //Refresh();
    }

    public static bool IsIPhoneXAspect()
    {
        bool isIphoneXAspect = false;

        float w = Mathf.Max(Screen.width, Screen.height);
        float h = Mathf.Min(Screen.width, Screen.height);
        float aspect = w / h;

        if (aspect > 1.78f)
        {
            isIphoneXAspect = true;
        }

        return isIphoneXAspect;
    }

    void Update ()
    {
        if(IsIPhoneXAspect())
            Refresh ();
    }

    public void Refresh()
    {
        Rect safeArea = GetSafeArea();

        if (safeArea != LastSafeArea)
            ApplySafeArea(safeArea);
    }

    Rect GetSafeArea()
    {
        Rect safeArea = Screen.safeArea;

#if UNITY_EDITOR

        if (IsIPhoneXAspect())
        {
            Sim = SimDevice.iPhoneX;
        }

#endif

        if (Application.isEditor && Sim != SimDevice.None)
        {
            Rect nsa = new Rect(0, 0, Screen.width, Screen.height);

            switch (Sim)
            {
                case SimDevice.iPhoneX:
                    if (Screen.height > Screen.width)  // Portrait
                        nsa = NSA_iPhoneX[0];
                    else  // Landscape
                        nsa = NSA_iPhoneX[1];
                    break;
                case SimDevice.iPhoneXsMax:
                    if (Screen.height > Screen.width)  // Portrait
                        nsa = NSA_iPhoneXsMax[0];
                    else  // Landscape
                        nsa = NSA_iPhoneXsMax[1];
                    break;
                default:
                    break;
            }

            safeArea = new Rect(Screen.width * nsa.x, Screen.height * nsa.y, Screen.width * nsa.width, Screen.height * nsa.height);
        }

        return safeArea;
    }

    void ApplySafeArea(Rect r)
    {
        LastSafeArea = r;

        // Ignore x-axis?
        //if (Screen.width < Screen.height)// !ConformX)
        //{
        //    r.x = 0;
        //    r.width = Screen.width;
        //}

        //// Ignore y-axis?
        //if (Screen.width > Screen.height)// !ConformY)
        //{
        //    r.y = 0;
        //    r.height = Screen.height;
        //}

        // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
        Vector2 anchorMin = r.position;
        Vector2 anchorMax = r.position + r.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        Panel.anchorMin = anchorMin;
        Panel.anchorMax = anchorMax;

        Debug.LogFormat("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
            name, r.x, r.y, r.width, r.height, Screen.width, Screen.height);
    }
}
