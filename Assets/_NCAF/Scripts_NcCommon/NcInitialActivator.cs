using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NcInitialActivator : MonoBehaviour
{
    public bool m_DeactivateOnStart = true;

    // Update is called once per frame
    void Update()
    {
        gameObject.SetActive(!m_DeactivateOnStart);
        Destroy(this);
    }
}
