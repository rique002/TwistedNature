using System;
using UnityEngine;
public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera fpCamera;




    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (mainCamera.enabled)
            {
                toFP();
            }
            else
            {
                toMain();
            }
        }
    }
    private void Start()
    {
        mainCamera.enabled = true;
        fpCamera.enabled = false;
    }

    [ContextMenu("Switch to First-Person Camera")]
    private void toFP()
    {
        mainCamera.enabled = false;
        fpCamera.enabled = true;
    }

    [ContextMenu("Switch to Main Camera")]
    private void toMain()
    {
        mainCamera.enabled = true;
        fpCamera.enabled = false;
    }

    public bool isMain()
    {
        print("Main Camera Enabled: " + mainCamera.enabled);
        return mainCamera.enabled;
    }


}