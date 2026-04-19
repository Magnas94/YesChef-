using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera m_Cam;

    // Start is called before the first frame update
    void Start()
    {
        m_Cam = Camera.main;    
    }

    // Update is called once per frame
    void LateUpdate ()
    {

        transform.LookAt(transform.position + m_Cam.transform.rotation * Vector3.forward, m_Cam.transform.rotation * Vector3.up);
    }
}
