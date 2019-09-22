using System.IO;
using System.Text.RegularExpressions;
using DefaultNamespace;
using NcAF;
using UnityEditor;
using UnityEngine;

public static class MenuItemBatch
{
    // json Asset
    private const string jsonPath = "_ARImages/190921ImageJSon.txt";
    private const string containerName = "--ARImageTargetInfo";

    // default Material to use for the target visuals
    private const string materialPath = "Assets/_NCAF/BatchProcessScript/Imageframe.mat";

    [UnityEditor.MenuItem ("NCAF/Process Batch AR Images")]
    public static void Process ()
    {
        // default Material for image visuals
        Material sourceMaterial = AssetDatabase.LoadAssetAtPath<Material> (materialPath);
        string json = File.ReadAllText (Application.dataPath + "/" + jsonPath);
        Imageframes imagesframes = JsonUtility.FromJson<Imageframes> (json);

        // if no container game object exists, make one
        var container = GameObject.Find (containerName);
        if (container == null)
            container = new GameObject (containerName);

        foreach (var imageframe in imagesframes.imageframe)
        {
            var filename = Path.GetFileName (imageframe.image_filepath);
            var srcPath = "Assets/_ARImages/" + filename;
            var dstPath = "Assets/_ARImages/" + imageframe.name + Path.GetExtension (imageframe.image_filepath);
            var asset = AssetDatabase.LoadAssetAtPath<Texture> (srcPath);
            if (asset == null)
            {
                Debug.LogWarning (srcPath + " doesn't exist");
                continue;
            }

            var existingTexture = AssetDatabase.LoadAssetAtPath<Texture> (dstPath);
            if (existingTexture)
            {
                AssetDatabase.DeleteAsset (dstPath);
                Debug.LogWarning (dstPath + " already exists");
            }

            var error = AssetDatabase.MoveAsset (srcPath, dstPath);
            if (!string.IsNullOrEmpty (error))
                Debug.LogError ($"Error when renaming '{srcPath}' to '{dstPath}: {error}");
        }

        AssetDatabase.SaveAssets ();
        AssetDatabase.Refresh ();

        foreach (var imageframe in imagesframes.imageframe)
        {
            Debug.Log (
                $"imageframe.centerpoint_x {imageframe.centerpoint_x} imageframe.centerpoint_y {imageframe.centerpoint_y}");
            var center = new Vector3 (imageframe.centerpoint_x, imageframe.centerpoint_y, imageframe.centerpoint_z);
            var vectorW = new Vector3 (imageframe.vectorW_x, imageframe.vectorW_y, imageframe.vectorW_z);
            var vectorH = new Vector3 (imageframe.vectorH_x, imageframe.vectorH_y, imageframe.vectorH_z);
//            var up = Vector3.Cross (vectorH, vectorW);

            var quadGameObject = new GameObject {name = imageframe.name};

            quadGameObject
                .transform
                .SetParent (container.transform);

            var mesh = new Mesh ();
/*
            var matrix =
                Matrix4x4.TRS (
                    center,
                    Quaternion.identity,
                    new Vector3 (
                        vectorW.magnitude,
                        1f,
                        vectorH.magnitude));

*/

            mesh.vertices = new[]
            {
                new Vector3 (-0.5f, 0f, -0.5f),
                new Vector3 (-0.5f, 0f, 0.5f),
                new Vector3 (0.5f, 0f, 0.5f),
                new Vector3 (0.5f, 0f, -0.5f)
            };
            mesh.triangles = new[] {0, 1, 3, 1, 2, 3};
            mesh.uv = new[]
            {
                new Vector2 (0, 0),
                new Vector2 (0, 1),
                new Vector2 (1, 1),
                new Vector2 (1, 0)
            };
            mesh.RecalculateNormals ();

            var quadGameObjectTransform = quadGameObject.transform;
            quadGameObjectTransform.localPosition = center;
            quadGameObjectTransform.localRotation = Quaternion.FromToRotation (Vector3.right, vectorW) *
                                                    Quaternion.FromToRotation (Vector3.forward, vectorH);
//            quadGameObjectTransform.localRotation = Quaternion.FromToRotation (Vector3.up, up);
            quadGameObjectTransform.localScale = new Vector3 (vectorW.magnitude, 1f, vectorH.magnitude);

            var meshFilter = quadGameObject.AddComponent<MeshFilter> ();
            var meshRenderer = quadGameObject.AddComponent<MeshRenderer> ();
            var texturePath = "Assets/_ARImages/" + imageframe.name + Path.GetExtension (imageframe.image_filepath);
            var material = meshRenderer.material = new Material (sourceMaterial);
            var texture = AssetDatabase.LoadAssetAtPath<Texture> (texturePath);

            meshFilter.mesh = mesh;
            material.mainTexture = texture;

            quadGameObject.AddComponent<NcGameObjectInfo> ();
            var imageInfo = quadGameObject.AddComponent<NcafARImageInfo> ();

            var regex = new Regex ("^([^_]+)_w([^_]+)_h([^_]+)___(.+)$");
            var imageFrameName = imageframe.name;
            if (!regex.IsMatch (imageFrameName))
            {
                Debug.LogError ($"Unable to parse imageframe name {imageframe.name}");
                continue;
            }

            var match = regex.Match (imageFrameName);
            var indexStr = match.Groups[1].Value;
            var widthStr = match.Groups[2].Value;
            var heightStr = match.Groups[3].Value;
            var name = match.Groups[4].Value;

            if (!int.TryParse (indexStr, out var index))
            {
                Debug.LogError ($"Unable to parse index from {imageFrameName}");
                continue;
            }

            if (!float.TryParse (widthStr, out var width))
            {
                Debug.LogError ($"Unable to parse width from {imageFrameName}");
                continue;
            }

            if (!float.TryParse (heightStr, out var height))
            {
                Debug.LogError ($"Unable to parse height from {imageFrameName}");
                continue;
            }

            imageInfo.m_augmentedImageIndex = index;
            imageInfo.m_width = width;
            imageInfo.m_height = height;
            imageInfo.m_augmentedImageName = name;
        }
    }
}