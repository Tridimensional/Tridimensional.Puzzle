using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Awake()
    {
 
    }

    void Start()
    {
        gameObject.transform.position = new Vector3(0, 0, -0.38f);
    }
}
