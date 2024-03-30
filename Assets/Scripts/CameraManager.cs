using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private string _screenShotFileName = "test";

    public void StartCaptureScreen()
    {
        StartCoroutine(CaptureScreen());
    }

    IEnumerator CaptureScreen()
    {
        Canvas[] canvasArray = Resources.FindObjectsOfTypeAll<Canvas>();
        foreach (Canvas canvas in canvasArray)
        {
            canvas.enabled = false;
        }

        yield return new WaitForEndOfFrame();
        string _filePath = Application.persistentDataPath + "/";// + "/Screenshots/";
        bool _captured = false;
        int _fileNumber = 0;
        while (!_captured)
        {
            if (System.IO.File.Exists(_filePath + _fileNumber + ".png"))
                _fileNumber++;
            else
            {
                ScreenCapture.CaptureScreenshot(_filePath + _fileNumber + ".png");
                _captured = true;
            }

            yield return null;
        }

        foreach (Canvas canvas in canvasArray)
        {
            canvas.enabled = true;
        }

        Debug.Log(Application.persistentDataPath);
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
