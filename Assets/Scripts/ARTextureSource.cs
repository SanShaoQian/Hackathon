using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;

public class ARTextureSource : MonoBehaviour
{
    [SerializeField] private ARCameraManager cameraManager;

    public UnityEngine.Events.UnityEvent<Texture2D> OnTextureReceived = new UnityEngine.Events.UnityEvent<Texture2D>();

    void OnEnable()
    {
        cameraManager.frameReceived += OnCameraFrameReceived;
    }

    void OnDisable()
    {
        cameraManager.frameReceived -= OnCameraFrameReceived;
    }

    void OnCameraFrameReceived(ARCameraFrameEventArgs args)
    {
        if (cameraManager.TryAcquireLatestCpuImage(out var image))
        {
            // Convert the image to a texture or process as needed
            // Call OnTextureReceived.Invoke(texture) once done
            // For example:
            Texture2D texture = ConvertCameraImageToTexture2D(image);
            OnTextureReceived.Invoke(texture);
            image.Dispose();
        }
    }

    Texture2D ConvertCameraImageToTexture2D(XRCpuImage image)
    {
        // Implement conversion from XRCpuImage to Texture2D
        // Example implementation for demonstration:
        XRCpuImage.ConversionParams conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, image.width, image.height),
            outputDimensions = new Vector2Int(image.width, image.height),
            outputFormat = TextureFormat.RGBA32,
            transformation = XRCpuImage.Transformation.None
        };

        // Create a texture to receive the image data
        Texture2D texture = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);

        // Convert the image to the texture format
        image.Convert(conversionParams, new NativeArray<byte>(texture.GetRawTextureData<byte>(), Allocator.None));

        // Apply the texture data
        texture.Apply();

        return texture;
    }
}
