using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.position = new Vector3(0, 0, -1);
        gameObject.transform.rotation = Quaternion.Euler(30, 30, 0);
    }
}
