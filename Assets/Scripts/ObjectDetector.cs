using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using UI = UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ObjectDetector : MonoBehaviour
{
    public NNModel _model;
    public Texture2D _image;
    //public UI.RawImage _imageView;
    public CameraFeed _cameraFeed;

    private string detectedAnimal;
    private IWorker _worker;
    private Model _runtimeModel;

    private int _resizeLength = 640; // Length of one side of the square after resizing

    // Class labels
    private readonly string[] _labels = {
        "Spider","Parrot","Scorpion","Sea turtle","Cattle","Fox","Hedgehog","Turtle","Cheetah","Snake","Shark",
        "Horse","Magpie","Hamster","Woodpecker","Eagle","Penguin","Butterfly","Lion","Otter","Raccoon",
        "Hippopotamus","Bear","Chicken","Pig","Owl","Caterpillar","Koala","Polar bear","Squid","Whale",
        "Harbor seal","Raven","Mouse","Tiger","Lizard","Ladybug","Red panda","Kangaroo","Starfish","Worm",
        "Tortoise","Ostrich","Goldfish","Frog","Swan","Elephant","Sheep","Snail","Zebra","Moths and butterflies",
        "Shrimp","Fish","Panda","Lynx","Duck","Jaguar","Goose","Goat","Rabbit","Giraffe","Crab","Tick",
        "Monkey","Bull","Seahorse","Centipede","Mule","Rhinoceros","Canary","Camel","Brown bear","Sparrow",
        "Squirrel","Leopard","Jellyfish","Crocodile","Deer","Turkey","Sea lion"
    };

    void Start()
    {
        Debug.Log("Starting object detection...");

        // Load the model and create a worker
        _runtimeModel = ModelLoader.Load(_model);
        _worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, _runtimeModel);
    }

    public string getDetectionResults()
    {
        return detectedAnimal;
    }
    void Update()
    {
        if (_worker == null)
        {
            Debug.LogWarning("Worker is not initialized.");
            return;
        }
        if (_cameraFeed == null)
        {
            Debug.LogWarning("Camera Manager is not initialized.");
            return;
        }

        // Get the latest frame from the CameraFeed script
        Texture2D cameraTexture = _cameraFeed.GetCameraTexture();

        if (cameraTexture != null)
        {
            // Resize the image to the model's input size and create a Tensor
            var resizedTexture = ResizedTexture(cameraTexture, _resizeLength, _resizeLength);
            Tensor inputTensor = new Tensor(resizedTexture, channels: 3);

            Debug.Log($"Input Tensor Shape: {inputTensor.shape}");

            // Execute inference
            _worker.Execute(inputTensor);

            // Retrieve and process the model output
            Tensor output0 = _worker.PeekOutput("Identity");
            Debug.Log($"Output Tensor Shape: {output0.shape}");

            List<DetectionResult> detections = ParseOutputs(output0, 0.5f, 0.75f);

            // Cleanup resources
            inputTensor.Dispose();
            output0.Dispose();

            // Scale factors to map the results back to the original image size
            float scaleX = cameraTexture.width / (float)_resizeLength;
            float scaleY = cameraTexture.height / (float)_resizeLength;

            // Create a clone of the original image for visualization
            var displayTexture = ResizedTexture(cameraTexture, cameraTexture.width, cameraTexture.height);

            // Mapping colors to classes
            Dictionary<int, Color> colorMap = new Dictionary<int, Color>();
            if (detections.Count == 0)
            {
                detectedAnimal = "";
            }
            foreach (DetectionResult detection in detections)
            {
                Debug.Log($"Detected {_labels[detection.classId - 4]} with confidence {detection.score:0.00}");
                detectedAnimal = _labels[detection.classId - 4];
                // Assign or retrieve color for this class
                Color color;
                if (colorMap.ContainsKey(detection.classId))
                {
                    color = colorMap[detection.classId];
                }
                else
                {
                    color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                    colorMap[detection.classId] = color;
                }

                // Draw detected rectangles on the image
                for (int x = (int)(detection.x1 * scaleX); x < (int)(detection.x2 * scaleX); x++)
                {
                    for (int y = (int)(detection.y1 * scaleY); y < (int)(detection.y2 * scaleY); y++)
                    {
                        // Note: Texture2D's origin is bottom-left, so flip the y-coordinate
                        displayTexture.SetPixel(x, cameraTexture.height - y, color);
                    }
                }
            }
            displayTexture.Apply();

            // Display the result
            //_imageView.texture = displayTexture;

            Debug.Log("Object detection completed.");
        }
        else
        {
            Debug.LogWarning("No valid camera texture available.");
        }
    }

    private void OnDestroy()
    {
        // Dispose of the worker to release resources
        if (_worker != null)
        {
            _worker.Dispose();
        }
    }

    private List<DetectionResult> ParseOutputs(Tensor output0, float threshold, float iouThres)
    {
        int outputWidth = output0.shape.width;
        List<DetectionResult> candidateDetections = new List<DetectionResult>();
        List<DetectionResult> detections = new List<DetectionResult>();

        for (int i = 4; i < outputWidth; i++)
        {
            // Parse detection result
            var result = new DetectionResult(output0, i);

            // Check if the detection meets the score threshold
            if (result.score < threshold)
            {
                //Debug.Log("Result"+ result.score);
                continue;
            }

            candidateDetections.Add(result);
        }

        // Non-Maximum Suppression (NMS)
        while (candidateDetections.Count > 0)
        {
            int idx = 0;
            float maxScore = 0.0f;
            for (int i = 0; i < candidateDetections.Count; i++)
            {
                if (candidateDetections[i].score > maxScore)
                {
                    idx = i;
                    maxScore = candidateDetections[i].score;
                }
            }

            var best = candidateDetections[idx];
            candidateDetections.RemoveAt(idx);

            detections.Add(best);

            List<int> toRemove = new List<int>();
            for (int i = 0; i < candidateDetections.Count; i++)
            {
                float iou = Iou(best, candidateDetections[i]);
                if (iou >= iouThres)
                {
                    toRemove.Add(i);
                }
            }

            for (int i = toRemove.Count - 1; i >= 0; i--)
            {
                candidateDetections.RemoveAt(toRemove[i]);
            }
        }

        return detections;
    }

    private Texture2D ConvertToTexture2D(XRCpuImage cpuImage)
    {
        // Create a new Texture2D with the same dimensions as the camera image
        var texture = new Texture2D(cpuImage.width, cpuImage.height, TextureFormat.RGBA32, false);

        // Get the raw texture data from the XRCpuImage
        var conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, cpuImage.width, cpuImage.height),
            outputDimensions = new Vector2Int(cpuImage.width, cpuImage.height),
            outputFormat = TextureFormat.RGBA32, // Choose a suitable format for your model
            transformation = XRCpuImage.Transformation.MirrorY
        };

        var rawTextureData = texture.GetRawTextureData<byte>();
        cpuImage.Convert(conversionParams, rawTextureData);
        texture.Apply();

        return texture;
    }
    private float Iou(DetectionResult boxA, DetectionResult boxB)
    {
        if ((boxA.x1 == boxB.x1) && (boxA.x2 == boxB.x2) && (boxA.y1 == boxB.y1) && (boxA.y2 == boxB.y2))
        {
            return 1.0f;
        }
        else if (((boxA.x1 <= boxB.x1 && boxA.x2 > boxB.x1) || (boxA.x1 >= boxB.x1 && boxB.x2 > boxA.x1))
            && ((boxA.y1 <= boxB.y1 && boxA.y2 > boxB.y1) || (boxA.y1 >= boxB.y1 && boxB.y2 > boxA.y1)))
        {
            float intersection = (Mathf.Min(boxA.x2, boxB.x2) - Mathf.Max(boxA.x1, boxB.x1))
                * (Mathf.Min(boxA.y2, boxB.y2) - Mathf.Max(boxA.y1, boxB.y1));
            float union = (boxA.x2 - boxA.x1) * (boxA.y2 - boxA.y1) + (boxB.x2 - boxB.x1) * (boxB.y2 - boxB.y1) - intersection;
            return (intersection / union);
        }

        return 0.0f;
    }

    // Image resizing
    private static Texture2D ResizedTexture(Texture2D texture, int width, int height)
    {
        var rt = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(texture, rt);

        var previousRt = RenderTexture.active;
        RenderTexture.active = rt;

        var resizedTexture = new Texture2D(width, height);
        resizedTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        resizedTexture.Apply();

        RenderTexture.active = previousRt;
        RenderTexture.ReleaseTemporary(rt);

        Debug.Log($"Resized texture to {width}x{height}");
        return resizedTexture;
    }
}

// Class to hold detection results
class DetectionResult
{
    public float x1 { get; }
    public float y1 { get; }
    public float x2 { get; }
    public float y2 { get; }
    public int classId { get; }
    public float score { get; }

    public DetectionResult(Tensor t, int idx)
    {
        // Parse bounding box coordinates and convert from center to corner format
        float halfWidth = t[0, 0, idx, 2] / 2;
        float halfHeight = t[0, 0, idx, 3] / 2;
        x1 = t[0, 0, idx, 0] - halfWidth;
        y1 = t[0, 0, idx, 1] - halfHeight;
        x2 = t[0, 0, idx, 0] + halfWidth;
        y2 = t[0, 0, idx, 1] + halfHeight;

        // Determine the class with the highest score
        int classes = t.shape.channels - 4;
        score = 0f;
        for (int i = 0; i < classes; i++)
        {
            float classScore = t[0, 0, idx, i + 4];
            if (classScore > score)
            {
                classId = idx;  // Note: Changed from idx to i to get the class ID correctly
                score = classScore;
            }
        }
    }
}
