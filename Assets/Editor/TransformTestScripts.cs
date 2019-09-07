using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


public class testScript : EditorWindow
{
    [SerializeField]
    static Transform modeltr;

    [MenuItem("noclew Test Codes/Test Move On Target (ncTransform)")]
    public static void TestMove()
    {
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        foreach (GameObject go in Selection.gameObjects)
        {
            //Debug.Log(Selection.gameObjects.Length + ":" +  go.name);
        }

        modeltr = GetModelByNameFromSelection("model");
        Transform targettr = GetModelByNameFromSelection("target");
        Transform movedTargettr = GetModelByNameFromSelection("movedTarget");


        NcTransform model = new NcTransform(modeltr);
        NcTransform target = new NcTransform(targettr);
        NcTransform movedTarget = new NcTransform(movedTargettr);
        NcTransform newGlobalTransform = GetNewGlobalTransformData(model, target, movedTarget);

        Undo.RecordObject( modeltr.gameObject.transform , "-moved");
        modeltr.position = newGlobalTransform.position;
        EditorUtility.SetDirty(modeltr.gameObject.transform);

        Undo.RecordObject(modeltr.gameObject.transform, "-rotated");
        modeltr.rotation = newGlobalTransform.rotation;
        EditorUtility.SetDirty(modeltr.gameObject.transform);

        Undo.RecordObject(modeltr.gameObject.transform, "-scaled");
        modeltr.localScale = newGlobalTransform.lossyScale;
        EditorUtility.SetDirty(modeltr.gameObject.transform);

        //Undo.RecordObject(modeltr.gameObject, "4");
        //modeltr.SetParent(movedTargettr);


        //EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        //GetNewGlobalTransformData2(modeltr, targettr, movedTargettr);
    }

    [MenuItem("noclew Test Codes/Test Move On Target (transfrom ver)")]
    public static void TestMovetransform()
    {

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        foreach (GameObject go in Selection.gameObjects)
        {
            //Debug.Log(Selection.gameObjects.Length + ":" +  go.name);
        }

        modeltr = GetModelByNameFromSelection("model");
        Transform targettr = GetModelByNameFromSelection("target");
        Transform movedTargettr = GetModelByNameFromSelection("movedTarget");

        NcTransform newGlobalTransform = GetNewGlobalTransformData2(modeltr, targettr, movedTargettr);

        Undo.RecordObject(modeltr.gameObject.transform, "-moved");
        modeltr.position = newGlobalTransform.position;
        EditorUtility.SetDirty(modeltr.gameObject.transform);

        Undo.RecordObject(modeltr.gameObject.transform, "-rotated");
        modeltr.rotation = newGlobalTransform.rotation;
        EditorUtility.SetDirty(modeltr.gameObject.transform);

        Undo.RecordObject(modeltr.gameObject.transform, "-scaled");
        modeltr.localScale = newGlobalTransform.lossyScale;
        EditorUtility.SetDirty(modeltr.gameObject.transform);

        //Undo.RecordObject(modeltr.gameObject, "4");
        //modeltr.SetParent(movedTargettr);


        //EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        //GetNewGlobalTransformData2(modeltr, targettr, movedTargettr);
    }



    //this function translate the global position of a model to a local position in the target space. Technically equivalent to InverseTransfromPoint.
    static public Vector3 CalcInitialLocalPosOfModelToTarget(Transform model, Transform target)
    {

        //return target.InverseTransformPoint(model.position);
        /////test above
        //this part is inverse of local to global (inverse function of TransformPoint)
        Quaternion target_rot = target.rotation;
        Vector3 target_scale = target.lossyScale;
        Vector3 target_pos = target.position;
        Vector3 model_pos = model.position;

        var difference = (model_pos - target_pos);

        var finalPos = Vector3.Scale(new Vector3(1 / target_scale.x, 1 / target_scale.y, 1 / target_scale.z),  Quaternion.Inverse(target_rot) * difference);

        //var FinalPos = Quaternion.Inverse(target_rot) * new Vector3(difference.x / target_scale.x, difference.y / target_scale.y, difference.z / target_scale.z);

        return finalPos;
    }
    //based on the function above, this function moves the model onto the target setting it as the model's parent
    static public NcTransform GetNewGlobalTransformData2(Transform model, Transform target, Transform movedTarget)
    {
        Debug.Log("DDD");
        //the local positions of the model in both original and moved target transfrom are the same. 
        Vector3 localPosInMovedTarget = CalcInitialLocalPosOfModelToTarget(model, target);
        Debug.Log("translatedLocal : " + localPosInMovedTarget.ToString("F6"));
        Vector3 newGlobalPos = movedTarget.TransformPoint(localPosInMovedTarget);

        //this translates the global rotation of the model to the local rotation in the target space.
        Quaternion modelRotInitial = Quaternion.Inverse(target.rotation) * model.rotation;

        //this translates the local rotation of model in the *MOVED* target space to a global rotation.
        Quaternion newGlobalRot = movedTarget.transform.rotation * modelRotInitial;

        ////// scale calculation
        Vector3 newLocalScale = new Vector3(target.lossyScale.x / model.lossyScale.x, target.lossyScale.y / model.lossyScale.y, target.lossyScale.z / model.lossyScale.z);
        Vector3 newGlobalScale = new Vector3(movedTarget.lossyScale.x / newLocalScale.x, movedTarget.lossyScale.y / newLocalScale.y, movedTarget.lossyScale.z / newLocalScale.z);

        return new NcTransform(newGlobalPos, newGlobalRot, newGlobalScale);
        //model.rotation = (model.rotation * model.transform.rotation) * Quaternion.Inverse(model.rotation);
        //model.position = movedTarget.TransformPoint(localPosInMovedTarget);
        //model.rotation = movedRotation;
        //Debug.Log("newGlobalPos : " + movedTarget.TransformPoint(localPosInMovedTarget).ToString("F6"));
        //Debug.Log("newLocalRot: " + movedRotation.eulerAngles.ToString("F6"));
        
    }


    //this function translate the global position of a model to a local position in the target space. Technically equivalent to InverseTransfromPoint.
    //reference:: https://answers.unity.com/questions/186252/multiply-quaternion-by-vector.html
    //reference:: https://answers.unity.com/questions/601062/what-inversetransformpoint-does-need-explanation-p.html
    static public Vector3 TranslateGlobalPosToLocal(NcTransform model, NcTransform target)
    {
        //this part is inverse of local to global (inverse function of TransformPoint)
        Quaternion target_rot = target.rotation;
        Vector3 target_scale = target.lossyScale;
        Vector3 target_pos = target.position;
        Vector3 model_pos = model.position;

        var diference = (model_pos - target_pos);
        //var finalPos = Quaternion.Inverse(target_rot) * new Vector3(diference.x / target_scale.x, diference.y / target_scale.y, diference.z / target_scale.z); // This is wrong
        var finalPos = Vector3.Scale(new Vector3(1 / target_scale.x, 1 / target_scale.y, 1 / target_scale.z), Quaternion.Inverse(target_rot) * diference);
        return finalPos;
    }

    //based on the function above, this function cacluates parameteres of a new global transfrom from a moved target
    static public NcTransform GetNewGlobalTransformData(NcTransform trModel, NcTransform trTarget, NcTransform trMovedTarget)
    {
        ////// position calculation
        //the local positions of the model in both original and moved target transfrom are the same. 
        Vector3 localPos_in_originalTarget = TranslateGlobalPosToLocal(trModel, trTarget);
        Debug.Log("translatedLocal : " + localPos_in_originalTarget.ToString("F6"));

        // now we translate  the calculated local position in the moved target space to global
        // This is equivalent to " trMovedTarget.transform.TransformPoint ( localPosInTarget )"
        // ?? operator is needed because local scale of the moved target may not have a local scale if the NcTransform instance was not initiated from Transfrom class. 
        // However, it will have a value because the we will make the instance from a transform of the moved target.
        Vector3 newGlobalPos = trMovedTarget.rotation * Vector3.Scale(localPos_in_originalTarget, trMovedTarget.localScale ?? default) + trMovedTarget.position;
        Debug.Log("newGlobalPos : " + newGlobalPos.ToString("F6"));


        ////// rotation calculation
        //this translates the global rotation of the model to the local rotation in the target space.
        Quaternion localRot_in_originalTarget = Quaternion.Inverse(trTarget.rotation) * trModel.rotation;

        //this translates the local rotation of model in the *MOVED* target space to a global rotation.
        Quaternion newGlobalRot = trMovedTarget.rotation * localRot_in_originalTarget;

        ////// scale calculation
        Vector3 newLocalScale = new Vector3(trTarget.lossyScale.x / trModel.lossyScale.x, trTarget.lossyScale.y / trModel.lossyScale.y, trTarget.lossyScale.z / trModel.lossyScale.z);
        Vector3 newGlobalScale = new Vector3(trMovedTarget.lossyScale.x / newLocalScale.x, trMovedTarget.lossyScale.y / newLocalScale.y, trMovedTarget.lossyScale.z / newLocalScale.z);

        return new NcTransform(newGlobalPos, newGlobalRot, newGlobalScale);
    }

    static public Transform GetModelByNameFromSelection(string name)
    {
        foreach(GameObject go in Selection.gameObjects)
        {
            if (go.name == name) return go.transform;
        }
        return null;
    }
}
