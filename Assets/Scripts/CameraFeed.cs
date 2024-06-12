using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CameraFeed : MonoBehaviour
{
    public Texture2D testImage; // Assign your test image in the Inspector
    private ARCameraManager arCameraManager;
    private WebCamTexture webCamTexture;
    private bool useWebCam = false;

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
            Debug.LogWarning("No test image provided and no AR Camera Manager found in the scene. Checking for webcam.");

            // Check if any webcams are available
            if (WebCamTexture.devices.Length > 0)
            {
                Debug.Log("Webcam found. Using webcam feed.");
                webCamTexture = new WebCamTexture();
                webCamTexture.Play();
                useWebCam = true;
            }
            else
            {
                Debug.LogError("No webcams available.");
            }
        }
    }

    void Update()
    {
        if (testImage != null)
        {
            // Displaying the test image, for debugging you might want to show it on a UI component.
        }
        else if (arCameraManager != null)
        {
            // Handling live AR camera feed logic here.
        }
        else if (useWebCam)
        {
            // Handling webcam feed logic here.
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
            return testImage;
        }
        else if (arCameraManager != null)
        {
            return CaptureARCameraImage();
        }
        else if (useWebCam)
        {
            return ConvertWebCamToTexture2D();
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
            cpuImage.Convert(conversionParams, cameraTexture.GetRawTextureData<byte>());

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

    private Texture2D ConvertWebCamToTexture2D()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            // Create a Texture2D with the same dimensions as the webcam feed
            Texture2D webCamTexture2D = new Texture2D(webCamTexture.width, webCamTexture.height);
            webCamTexture2D.SetPixels(webCamTexture.GetPixels());
            webCamTexture2D.Apply();
            return webCamTexture2D;
        }
        else
        {
            Debug.LogError("Webcam is not active.");
            return null;
        }
    }

    void OnDestroy()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            webCamTexture.Stop();
        }
    }
}
