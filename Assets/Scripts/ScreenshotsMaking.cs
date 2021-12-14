using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotsMaking : MonoBehaviour
{
    private const string FilePath = "C:/Unity/Money Takeover/Screenshots/";
    private int _screenshotNumber = 1;

    public void MakeScreenshot()
    {
        var fileName = "Level_" + FindObjectOfType<LastPlayedLevelSaving>().GetSceneNumber() + "_" 
                       + _screenshotNumber + ".png";
        ScreenCapture.CaptureScreenshot(FilePath + fileName);
        _screenshotNumber++;
    }
}
