using UnityEngine;
public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera fpCamera;

    private void Start()
    {
        mainCamera.enabled = true;
        fpCamera.enabled = false;
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.C))
        // {
        //     if (mainCamera.enabled)
        //     {
        //         toFP();
        //     }
        //     else
        //     {
        //         toMain();
        //     }
        // }
    }

    public void toFP()
    {
        // mainCamera.enabled = false;
        // fpCamera.enabled = true;
    }

    public void toMain()
    {
        // mainCamera.enabled = true;
        // fpCamera.enabled = false;
    }

    public bool isMain()
    {
        return true;
        // return mainCamera.enabled;
    }
}