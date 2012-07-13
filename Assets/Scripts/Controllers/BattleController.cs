using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    bool _moving = false;
    float _angle;
    ISceneService _sceneService;
    Transform _pieceTransform;
    Vector3 _hitPoint;
    Vector3 _lastHitPoint;
    Vector3 _mousePosition;

    void Awake()
    {
        _sceneService = SceneService.Instance;

        _sceneService.Initialize(camera);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePosition = Input.mousePosition;

            var ray = camera.ScreenPointToRay(_mousePosition);
            var raycastHit = new RaycastHit();

            if (Physics.Raycast(ray, out raycastHit, 20f))
            {
                _moving = true;
                _hitPoint = raycastHit.point;
                _pieceTransform = raycastHit.transform;

                var vector = _pieceTransform.position - _hitPoint;
                _angle = Mathf.Atan2(vector.y, vector.x);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _moving = false;
            _hitPoint = Vector3.zero;
            _pieceTransform.position = Vector3.zero;
            _lastHitPoint = Vector3.zero;
        }

        if (_moving)
        {
            SetPiecePosition();
        }
    }

    void SetPiecePosition()
    {
        _mousePosition = Input.mousePosition;

        var ray = camera.ScreenPointToRay(_mousePosition);
        var raycastHit = new RaycastHit();

        if (!Physics.Raycast(ray, out raycastHit, 20f)) { return; }

        if (_lastHitPoint == Vector3.zero)
        {
            _lastHitPoint = raycastHit.point;
            return;
        }

        if (Vector3.Equals(_lastHitPoint, raycastHit.point))
        {
            return;
        }

        var thisMoveVector = raycastHit.point - _lastHitPoint;
        var objectMouseToCenterVector = _pieceTransform.position - _hitPoint;
        var thisMoveAngle = (Mathf.Atan2(thisMoveVector.y, thisMoveVector.x));
        var angelDistanceNum = Mathf.DeltaAngle(180 * thisMoveAngle / Mathf.PI, 180 * _angle / Mathf.PI);
        var rotAngle = 0f;

        if (objectMouseToCenterVector.magnitude > _pieceTransform.lossyScale.x / 5)
        {
            rotAngle = (Mathf.Sin(angelDistanceNum / 180 * Mathf.PI)) * thisMoveVector.magnitude * 1000 * Time.deltaTime;
        }

        _pieceTransform.Rotate(Vector3.forward * rotAngle);
        _angle += rotAngle / 180 * Mathf.PI;

        var x = raycastHit.point.x + objectMouseToCenterVector.magnitude * Mathf.Cos(_angle);
        var y = raycastHit.point.y + objectMouseToCenterVector.magnitude * Mathf.Sin(_angle);
        _pieceTransform.position = new Vector3(x, y, _pieceTransform.position.z);

        _lastHitPoint = raycastHit.point;
    }
}
