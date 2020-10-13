using UnityEngine;
using System.Collections;

public class ScreenShot : MonoBehaviour {

int c = 0;
	// Use this for initialization
	void Start () 
	{
		ScreenCapture.CaptureScreenshot(Application.dataPath + "/" + c + ".png");
	}
	
	// Update is called once per frame
	void Update () {
		if (c % 300 == 1)
	ScreenCapture.CaptureScreenshot(Application.dataPath + "/" + c + ".png");
	c++;
	}
}
