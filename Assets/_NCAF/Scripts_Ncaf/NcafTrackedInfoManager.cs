using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

namespace NcAF
{
    // This is a simple midification fo the base class <TrackedImageInfoManager>
    // In order to make this work, chage the modifiers of the base class functions like this:
    // ... protected void UpdateInfo...
    // ... virtual void OnTrackedImagesChanged...
    public class NcafTrackedInfoManager : TrackedImageInfoManager
    {

    //    public override void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    //    {
    //        ///////////////////// nc added start
    //        Dictionary<string, ARTrackedImage> detectedImages = new Dictionary<string, ARTrackedImage>();
    //        ///////////////////// nc added ended

    //        foreach (var trackedImage in eventArgs.added)
    //        {
    //            // Give the initial image a reasonable default scale
    //            trackedImage.transform.localScale = new Vector3(0.01f, 1f, 0.01f);
    //            UpdateInfo(trackedImage);

    //            ///////////////////// nc added start
    //            detectedImages.Add(trackedImage.referenceImage.name, trackedImage);
    //            ///////////////////// nc added ended
    //        }

    //        foreach (var trackedImage in eventArgs.updated)
    //        {
    //            UpdateInfo(trackedImage);
    //            ///////////////////// nc added start
    //            detectedImages.Add(trackedImage.referenceImage.name, trackedImage);
    //            ///////////////////// nc added ended
    //        }

    //        ///////////////////// nc added start
    //        foreach (var trackedImage in eventArgs.updated)
    //        {
    //            detectedImages.Remove(trackedImage.referenceImage.name);
    //        }

    //        if(NcafMainController.Instance == null)
    //        {
    //            Debug.LogError("ERR>> Main Controller is not assigned (Singleton error)");
    //            return;
    //        }
    //        NcafMainController.Instance.ProcessDetectedImages(detectedImages);
    //        ///////////////////// nc added ended

    //}
    }
}