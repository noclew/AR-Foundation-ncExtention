using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NcAF
{
    public class NcafUIController : MonoBehaviour
    {
        static NcafUIController _instance;

        public static NcafUIController Instance { get { return _instance; } }

        [Header("GUI Components")]
        public Canvas m_debugHierarchyCanvas;
        public Dropdown m_AlignModeDropdown;
        public Dropdown m_ImageAlignModeDropdown;

        // Start is called before the first frame update
        void Start()
        {
            if (_instance != null && _instance != this) Destroy(this);
            else NcafUIController._instance = this;

            if (m_AlignModeDropdown != null)
            {
                m_AlignModeDropdown.onValueChanged.AddListener(DropDownChangeAlignModeHandler);
            }

            if (m_ImageAlignModeDropdown != null)
            {
                m_ImageAlignModeDropdown.onValueChanged.AddListener(DropDownChangeImageAlignnModeHandler);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ToggleHierarchyDebugCanvas(bool val)
        {
            if (m_debugHierarchyCanvas == null)
            {
                Debug.LogError("ERR>> Hierarchy Debug Canvas is not set");
                return;
            }
            m_debugHierarchyCanvas.gameObject.SetActive(!m_debugHierarchyCanvas.gameObject.activeSelf);
        }

        public void DropDownChangeAlignModeHandler(int index)
        {
            if (index == 0)
            {
                if (NcafMainController.Instance.m_alignMode != AlIGNMODE.MANUAL)
                    NcafMainController.Instance.SetAlignMode(AlIGNMODE.MANUAL);
            }
            if (index == 1)
            {
                if (NcafMainController.Instance.m_alignMode != AlIGNMODE.IMAGEBASED)
                    NcafMainController.Instance.SetAlignMode(AlIGNMODE.IMAGEBASED);
            }
            if (index == 2)
            {
                if (NcafMainController.Instance.m_alignMode != AlIGNMODE.TOUCH)
                    NcafMainController.Instance.SetAlignMode(AlIGNMODE.TOUCH);
            }
        }

        public void DropDownChangeImageAlignnModeHandler(int index)
        {
            if (index == 0)
            {
                if (NcafMainController.Instance.m_imageAlignMode != IMAGEALIGNMODE.SINGLE)
                    NcafMainController.Instance.SetImageAlignMode(IMAGEALIGNMODE.SINGLE);
            }
            if (index == 1)
            {
                if (NcafMainController.Instance.m_imageAlignMode != IMAGEALIGNMODE.INTEPOLATION)
                    NcafMainController.Instance.SetImageAlignMode(IMAGEALIGNMODE.INTEPOLATION);
            }
        }

        public void AlignmentModeChangeListener()
        {
            switch (NcafMainController.Instance.m_alignMode)
            {
                case AlIGNMODE.MANUAL:
                    m_AlignModeDropdown.value = 0;
                    break;
                case AlIGNMODE.IMAGEBASED:
                    m_AlignModeDropdown.value = 1;
                    break;
                case AlIGNMODE.TOUCH:
                    m_AlignModeDropdown.value = 2;
                    break;
                default:
                    Debug.LogError("ERR>> Alignment Dropdown Listener Error");
                    break;
            }

        }

        public void ImageAlignmentModeChangeListner()
        {
            switch (NcafMainController.Instance.m_imageAlignMode)
            {
                case IMAGEALIGNMODE.SINGLE:
                    m_ImageAlignModeDropdown.value = 0;
                    break;
                case IMAGEALIGNMODE.INTEPOLATION:
                    m_ImageAlignModeDropdown.value = 1;
                    break;
                default:
                    Debug.LogError("ERR>> Image Alignment Dropdown Listener Error");
                    break;
            }
        }
        
    }
}