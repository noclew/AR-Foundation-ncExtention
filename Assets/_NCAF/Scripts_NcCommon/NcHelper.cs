using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NcCommon
{
    public class NcHelper
    {
        //this function translate the global position of a model to a local position in the target space. Technically equivalent to InverseTransfromPoint.
        //reference:: https://answers.unity.com/questions/186252/multiply-quaternion-by-vector.html  
        //reference:: https://answers.unity.com/questions/601062/what-inversetransformpoint-does-need-explanation-p.html >> core material
        static public Vector3 TranslateGlobalPosToLocal(NcTransform model, NcTransform target)
        {
            //this part is inverse of local to global (inverse function of TransformPoint)
            Quaternion target_rot = target.rotation;
            Vector3 target_scale = target.lossyScale;
            Vector3 target_pos = target.position;
            Vector3 model_pos = model.position;

            var difference = (model_pos - target_pos);

            //below is very wrong. It messes up.
            //var FinalPosWrong = Quaternion.Inverse(target_rot) * new Vector3(difference.x / target_scale.x, difference.y / target_scale.y, difference.z / target_scale.z);

            //this is the right wone.
            var finalPos = Vector3.Scale(new Vector3(1 / target_scale.x, 1 / target_scale.y, 1 / target_scale.z), Quaternion.Inverse(target_rot) * difference);
            return finalPos;
        }

        //based on the function above, this function cacluates parameteres of a new global transfrom from a moved target
        static public NcTransform GetNewGlobalTransformData(NcTransform trModel, NcTransform trTarget, NcTransform trMovedTarget)
        {
            ////// position calculation
            //the local positions of the model in both original and moved target transfrom are the same. 
            Vector3 localPos_in_originalTarget = TranslateGlobalPosToLocal(trModel, trTarget);
            //print("translatedLocal : " + localPos_in_originalTarget.ToString("F6"));

            // now we translate  the calculated local position in the moved target space to global
            // This is equivalent to " trMovedTarget.transform.TransformPoint ( localPosInTarget )"
            // ?? operator is needed because local scale of the moved target may not have a local scale if the NcTransform instance was not initiated from Transfrom class. 
            // However, it will have a value because the we will make the instance from a transform of the moved target.
            Vector3 newGlobalPos = trMovedTarget.rotation * Vector3.Scale(localPos_in_originalTarget, trMovedTarget.localScale ?? default) + trMovedTarget.position;
            //print("newGlobalPos : " + newGlobalPos.ToString("F6"));


            ////// rotation calculation
            //this translates the global rotation of the model to the local rotation in the target space.
            Quaternion localRot_in_originalTarget = Quaternion.Inverse(trTarget.rotation) * trModel.rotation;

            //this translates the local rotation of model in the *MOVED* target space to a global rotation.
            Quaternion newGlobalRot = trMovedTarget.rotation * localRot_in_originalTarget;

            ////// scale calculation
            //This calcualtes the local scale of the model in the original target space
            Vector3 newLocalScale = new Vector3(trTarget.lossyScale.x / trModel.lossyScale.x, trTarget.lossyScale.y / trModel.lossyScale.y, trTarget.lossyScale.z / trModel.lossyScale.z);
            //this caclulates the global scale of the model in the moved target space
            Vector3 newGlobalScale = new Vector3(trMovedTarget.lossyScale.x / newLocalScale.x, trMovedTarget.lossyScale.y / newLocalScale.y, trMovedTarget.lossyScale.z / newLocalScale.z);

            return new NcTransform(newGlobalPos, newGlobalRot, newGlobalScale);
        }

        public static void HideObject<T>(T obj)
        {
            GameObject go = obj as GameObject;

            if (typeof(Transform) == obj.GetType()) { go = (obj as Transform).gameObject; }

            if (go == null) return;
            foreach (Renderer r in go.GetComponentsInChildren<Renderer>()) r.enabled = false;
            foreach (Collider c in go.GetComponentsInChildren<Collider>()) c.enabled = false;
        }


        public static void ShowObject<T>(T obj)
        {
            GameObject go = obj as GameObject;

            if (typeof(Transform) == obj.GetType()) { go = (obj as Transform).gameObject; }

            if (go == null) return;
            foreach (Renderer r in go.GetComponentsInChildren<Renderer>()) r.enabled = true;
            foreach (Collider c in go.GetComponentsInChildren<Collider>()) c.enabled = true;
        }

        public static Pose AveragePose(List<Pose> poses)
        {
            Quaternion[] qArray = new Quaternion[poses.Count];
            Vector3[] vArray = new Vector3[poses.Count];

            for (int i = 0; i < poses.Count; i++)
            {
                qArray[i] = poses[i].rotation;
                vArray[i] = poses[i].position;
            }

            return new Pose(AverageVector(vArray), AverageQuaternion(qArray));
        }
        public static Quaternion AverageQuaternion(Quaternion[] qArray)
        {
            Quaternion qAvg = qArray[0];
            float weight;
            for (int i = 1; i < qArray.Length; i++)
            {
                weight = 1.0f / (float)(i + 1);
                qAvg = Quaternion.Slerp(qAvg, qArray[i], weight);
            }
            return qAvg;
        }

        public static Vector3 AverageVector(Vector3[] vArray)
        {
            int addAmount = 0;
            Vector3 addedVector = Vector3.zero;

            foreach (Vector3 singleVector in vArray)
            {
                //Amount of separate rotational values so far
                addAmount++;
                addedVector += singleVector;
            }

            return addedVector / (float)addAmount;
        }

    }
}