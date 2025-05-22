using UnityEngine;
using System.IO;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CameraController : MonoBehaviour
{
    public Camera playerOriginCam;
    public bool canTakePicture = false;
    public RenderTexture renderTexToCopy;
    public Material materialToCopy;
    public GameObject screenTarget;

    public Transform leftAttachmentPointTransform;
    public Transform rightAttachmentPointTransform;

    public int resWidth = 3000;
    public int resHeight = 3000;

    Camera cameraCam;
    RenderTexture copiedRenderTex;
    Material copiedMaterial;

    bool toggleCam = false;
    bool takePic = false;

    public SteamVR_Action_Boolean takePictureAction;
    public SteamVR_Action_Boolean switchCameraViewAction;
    public SteamVR_Input_Sources leftHand;
    public SteamVR_Input_Sources rightHand;

    SteamVR_Input_Sources currentHoldingHand = SteamVR_Input_Sources.Any;
    SteamVR_Input_Sources inverseHoldingHand = SteamVR_Input_Sources.Any;

    void Start()
    {
        cameraCam = GetComponent<Camera>();
        if (playerOriginCam == null)
            throw new System.Exception("playerOriginCam MUST be set");

        cameraCam.enabled = false;


        // Step 1, duplicate needed objects
        copiedRenderTex = Instantiate(renderTexToCopy);
        copiedMaterial = Instantiate(materialToCopy);

        // Step 2, set targetTexture to copied texture
        cameraCam.targetTexture = copiedRenderTex;

        // Step 3, link copied rt to copied material
        copiedMaterial.mainTexture = copiedRenderTex;
        copiedMaterial.SetTexture("_EmissionMap", copiedRenderTex);

        // Step 4, set material of the screen to the copied material
        screenTarget.GetComponent<Renderer>().material = copiedMaterial;
    }

    public void SetPictureTaker(bool canTakePic)
    {
        // Enable this camera for taking pictures
        canTakePicture = canTakePic;

        // Enable camera component
        cameraCam.enabled = canTakePic;

        // Render texture reset on dropping
        if (!canTakePic)
            cameraCam.targetTexture = copiedRenderTex;
    }

    public static string ScreenShotName(int width, int height) =>
        Path.Combine(Application.dataPath, "../Pictures", $"screen_{width}x{height}_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png");

    private void LateUpdate()
    {
        if (!canTakePicture)
            return;

        takePic |= Input.GetKeyDown(KeyCode.K) || (currentHoldingHand != SteamVR_Input_Sources.Any && takePictureAction.GetStateDown(currentHoldingHand));
        if (takePic)
            TakeAPicture();

        toggleCam |= Input.GetKeyDown(KeyCode.I) || (currentHoldingHand != SteamVR_Input_Sources.Any && switchCameraViewAction.GetStateDown(currentHoldingHand));
        if (toggleCam)
            ToggleCameraView();
    }

    void TakeAPicture()
    {
        // Save original camera render texture
        RenderTexture ogCamTex = cameraCam.targetTexture;

        // This code slice creates a new texture, then sets the camera's targetTexture
        // Under normal circumstances, the camera's render to the screen
        // When setting it's renderTexture, they instead render to the set render texture
        // Render in this case means (i think), draw what the camera object 'sees' onto the render texture

        // Create a new render texture
        RenderTexture rt = new(resWidth, resHeight, 24);

        // Bind the Camera's camera's target texture to the new render texture
        cameraCam.targetTexture = rt;

        // Create a blank screenshot
        Texture2D screenShot = new(resWidth, resHeight, TextureFormat.RGB24, false);

        // Manually render the camera, thereby drawing on the texture2d
        cameraCam.Render();

        // Save original render texture in case something was using it
        RenderTexture origRenderTex = RenderTexture.active;

        // Set the active render texture
        RenderTexture.active = rt;

        // Read the pixels from the active render texture to the texture2d
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

        // Reset camera's target texture to the original
        cameraCam.targetTexture = ogCamTex;

        // Change active render texture back to the original
        RenderTexture.active = origRenderTex;

        // Destroy temporary render texture
        Destroy(rt);

        // Save picture
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = ScreenShotName(resWidth, resHeight);
        File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Saved screenshot under: {0}", filename));

        takePic = false;
    }

    void ToggleCameraView()
    {
        if (cameraCam.targetTexture == null)
            cameraCam.targetTexture = copiedRenderTex;
        else
            cameraCam.targetTexture = null;

        toggleCam = false;
    }

    public void CameraHeldUpdate(Hand h)
    {
        if (h.handType == SteamVR_Input_Sources.LeftHand)
        {
            currentHoldingHand = SteamVR_Input_Sources.LeftHand;
            inverseHoldingHand = SteamVR_Input_Sources.RightHand;
        }
        else
        {
            currentHoldingHand = SteamVR_Input_Sources.RightHand;
            inverseHoldingHand = SteamVR_Input_Sources.LeftHand;
        }
    }
}
