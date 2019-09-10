using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NcAF
{
    public class NcafRefPointVizActivator : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(NcafMainController.Instance.m_isRefPointVisEnabled);
        }

        // Update is called once per frame
        void Update()
        {
            gameObject.SetActive(NcafMainController.Instance.m_isRefPointVisEnabled);
        }

        public void SetActive( bool flag)
        {
            gameObject.SetActive(flag);
        }
    }
}
