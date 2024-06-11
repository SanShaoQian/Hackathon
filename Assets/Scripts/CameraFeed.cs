using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CameraFeed : MonoBehaviour
{
    public Texture2D testImage; // Assign your test image in the Inspector
    private ARCameraManager arCameraManager;

    void Start()
    {
        // Find the AR Camera Manager component in the scene
        arCameraManager = FindObjectOfType<ARCameraManager>();

        if (testImage != null)
        {
            Debug.Log("Using the test image provided.");
        }
        else if (arCameraManager != null)
        {
            Debug.Log("AR Camera Manager found, using AR camera feed.");
        }
        else
        {
            Debug.LogError("No test image provided and no AR Camera Manager found in the scene.");
        }
    }

    void Update()
    {
        if (testImage != null)
        {
            Debug.Log("Displaying the test image.");
        }
        else if (arCameraManager != null)
        {
            Debug.Log("Displaying the live AR camera feed.");
        }
        else
        {
            Debug.LogWarning("No valid camera texture available.");
        }
    }

    public Texture2D GetCameraTexture()
    {
        if (testImage != null)
        {
            Debug.Log("Returning the test image texture.");
            return testImage;
        }
        else if (arCameraManager != null)
        {
            Debug.Log("Capturing the AR camera feed.");
            return CaptureARCameraImage();
        }

        Debug.LogError("No camera texture available to return.");
        return null;
    }

    private Texture2D CaptureARCameraImage()
    {
        // Create a new texture to store the AR camera image
        Texture2D cameraTexture = null;

        if (arCameraManager.TryAcquireLatestCpuImage(out XRCpuImage cpuImage))
        {
            // Create a Texture2D with the same dimensions as the AR camera image
            cameraTexture = new Texture2D(cpuImage.width, cpuImage.height, TextureFormat.RGBA32, false);

            // Convert the AR camera image to a Texture2D
            var conversionParams = new XRCpuImage.ConversionParams(cpuImage, TextureFormat.RGBA32);
            var rawTextureData = cameraTexture.GetRawTextureData<byte>();
            cpuImage.Convert(conversionParams, new NativeArray<byte>(rawTextureData, Allocator.Temp));

            // Apply the updated texture data
            cameraTexture.Apply();
            cpuImage.Dispose();
        }
        else
        {
            Debug.LogError("Failed to acquire the AR camera image.");
        }

        return cameraTexture;
    }
}
