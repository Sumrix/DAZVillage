using UnityEngine;
using System.Collections;

public class GraphicSettings :
    MonoBehaviour
{
    private bool _fps30;
    public bool FPS30
    {
        get { return _fps30; }
        set
        {
            _fps30 = value;
            Application.targetFrameRate = value ? 30 : 60;
        }
    }
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}