using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace NcAF
{
    using UnityEngine.XR.ARSubsystems;
    using TrackingState = UnityEngine.XR.ARSubsystems.TrackingState;

    [RequireComponent(typeof(NcGameObjectInfo))]
    public class NcafARImageInfo : MonoBehaviour
    {
        public bool IsLocalModelActive { get; set; }
        public ARTrackedImage m_arTrackedImage { get; set; }
        public bool m_isFullyTracking { get => IsFullyTracking(); }
        public TrackingState TrackingState { get => m_arTrackedImage ? m_arTrackedImage.trackingState : TrackingState.None; }

        public ARTrackedImage ArTrackedImage { get => m_arTrackedImage; set => m_arTrackedImage = value; }

        
        [Header("AR Target Info")]
        public int m_augmentedImageIndex;
        public string m_augmentedImageName = "";
        public float m_width = 0.5f;
        public float m_height = 0.5f;


        [Header("AR Contents List")]
        public List<Transform> m_localContents  ;

        /// <summary>
        /// Initial transform parameters in global space
        /// </summary>
        public NcTransform m_originalNcTransform;
        private void Awake()
        {
            m_originalNcTransform = new NcTransform(transform);
            //NcafMainController.Instance.AddARImageInfo(this);

        }
        // Start is called before the first frame update
        void Start()
        {
            //NcafMainController.Instance.AddARImageInfo(this);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void ResetARImageInfo()
        {
            m_arTrackedImage = null;
        }

        public bool IsFullyTracking()
        {
            if (m_arTrackedImage == null) return false;

            if (m_arTrackedImage.trackingState == TrackingState.Tracking) return true;
            else return false;
        }

    }


}
