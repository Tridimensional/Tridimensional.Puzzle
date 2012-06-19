using System.Collections;
using UnityEngine;

public class PieceBehaviour : MonoBehaviour
{
    IEnumerator OnMouseDown()
    {
        var camera = Camera.mainCamera;

        if (camera)
        {
            //转换对象到当前屏幕位置
            var screenPosition = camera.WorldToScreenPoint(transform.position);

            //鼠标屏幕坐标
            var mScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);

            //获得鼠标和对象之间的偏移量,拖拽时相机应该保持不动
            var offset = transform.position - camera.ScreenToWorldPoint(mScreenPosition);

            //若鼠标左键一直按着则循环继续
            while (Input.GetMouseButton(0))
            {
                //鼠标屏幕上新位置
                mScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);

                // 对象新坐标 
                transform.position = offset + camera.ScreenToWorldPoint(mScreenPosition);

                //协同，等待下一帧继续
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
