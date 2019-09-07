using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NcAF
{
    [RequireComponent(typeof(NcGameObjectInfo))]
    public class NcafARImageInfo : MonoBehaviour
    {
        [Header("AR Target Info")]
        public int m_augmentedImageIndex;
        public string m_augmentedImageName = "";
        public float m_width = 0.5f;
        public float m_height = 0.5f;
        public bool m_isFullyTracking { get; set; }

        [Header("AR Contents List")]
        public List<Transform> m_localContents  ;

        /// <summary>
        /// Initial transform parameters in global space
        /// </summary>
        public NcTransform m_originalNcTransform;
        private void Awake()
        {
            m_originalNcTransform = new NcTransform(transform);
            m_isFullyTracking = false;
        }
        // Start is called before the first frame update
        void Start()
        {
            NcafMainController.Instance.AddAugmentedImageInfo(this);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }


}
