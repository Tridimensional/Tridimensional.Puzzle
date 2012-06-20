using System.Collections;
using UnityEngine;

public class PieceBehaviour : MonoBehaviour
{
    IEnumerator OnMouseDown()
    {
        var camera = Camera.mainCamera;

        if (camera)
        {
            var screenPosition = camera.WorldToScreenPoint(transform.position);
            var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
            var offset = transform.position - camera.ScreenToWorldPoint(mousePosition);

            while (Input.GetMouseButton(0))
            {
                mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
                transform.position = offset + camera.ScreenToWorldPoint(mousePosition);

                yield return new WaitForFixedUpdate();
            }
        }
    }
}
