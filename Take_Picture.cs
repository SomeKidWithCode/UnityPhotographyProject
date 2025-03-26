using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

// I would like to rename this
public class Take_Picture : MonoBehaviour
{
    public Camera playerOriginCam;
    Camera cameraCam;
    public int resWidth = 2550;
    public int resHeight = 3300;
    private bool takeHiResShot = false;
    public bool isPictureTaker = false;

    void Start()
    {
        cameraCam = GetComponent<Camera>();
        if (playerOriginCam == null)
            throw new System.Exception("playerOriginCam MUST be set");
        
        Debug.Log("Main cam: " + Camera.main);
    }

    void SetAsPictureTaker()
        => isPictureTaker = true;
    void UnsetPictureTaker()
        => isPictureTaker = false;

    public static string ScreenShotName(int width, int height)
        => Path.Combine(Application.dataPath, "../Pictures", $"screen_{width}x{height}_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png");

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    private void LateUpdate()
    {
        if (!isPictureTaker)
            return;
        takeHiResShot |= Input.GetKeyDown(KeyCode.K);
        if (takeHiResShot)
            TakeAPicture();
    }

    void TakeAPicture()
    {
        // Create a new render texture
        RenderTexture rt = new(resWidth, resHeight, 24);

        // Set the camera's camera's target texture to the new render texture (bind)
        cameraCam.targetTexture = rt;

        // Create a blank screenshot
        Texture2D screenShot = new(resWidth, resHeight, TextureFormat.RGB24, false);

        // Manually render the camera, thereby drawing on the texture2d
        cameraCam.Render();

        // Save original active render texture
        RenderTexture ogTex = RenderTexture.active;

        // Set the active render texture
        RenderTexture.active = rt;

        // Read the pixels from the texture2d
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

        // Unset camera's target texture
        cameraCam.targetTexture = null;

        // Change active render texture back to the original
        RenderTexture.active = ogTex;

        // Destroy temporary render texture
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        string filename = ScreenShotName(resWidth, resHeight);
        File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
        takeHiResShot = false;
    }
}
