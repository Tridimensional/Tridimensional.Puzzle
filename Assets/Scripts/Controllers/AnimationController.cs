using Tridimensional.Puzzle.Foundation;
using Tridimensional.Puzzle.Foundation.Enumeration;
using Tridimensional.Puzzle.IOC;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.UI.ViewModel;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    IModelService _modelService;
    Texture2D _backdropImage;
    Texture2D _backdropNormalMap;
    BackdropPieceViewModel[,] _backdropPieceViewModels;
    float _visionWidth;
    float _pieceWidth;
    float _flightHeight;
    float _circleDistance;
    int _rows;
    int _columns;
    bool _finished;

    void Awake()
    {
        InitializationLight();
        InitializationCamera();
        InitializationEnvironment();
    }

    void InitializationLight()
    {
        var go = new GameObject("Light");
        var light = go.AddComponent<Light>();

        light.intensity = 0.5f;
        light.type = LightType.Directional;
        light.transform.position = new Vector3(0, 0, -1);
        light.transform.rotation = Quaternion.Euler(30, 30, 0);
    }

    void InitializationCamera()
    {
        camera.backgroundColor = GlobalConfiguration.BackgroundColor;
        camera.transform.position = new Vector3(0, 0, -GlobalConfiguration.CameraToSubjectInMeter);
        camera.fieldOfView = 2 * Mathf.Atan(GlobalConfiguration.PictureHeightInMeter / 2 / GlobalConfiguration.CameraToSubjectInMeter) * 180 / Mathf.PI;
    }

    void InitializationEnvironment()
    {
        _modelService = InjectionRepository.Instance.Get<IModelService>();
        _backdropImage = Resources.Load("Image/LevelBackground/0") as Texture2D;

        var layoutContract = _modelService.GetProperLayout(Screen.width, Screen.height, 100);
        var sliceContract = _modelService.GetSlice(_backdropImage, layoutContract, SliceProgram.Default);
        var pieceContracts = _modelService.GeneratePieceContracts(sliceContract);

        _rows = layoutContract.Rows;
        _columns = layoutContract.Columns;
        _visionWidth = GlobalConfiguration.PictureHeightInMeter * Screen.width / Screen.height;
        _pieceWidth = _visionWidth / _columns;
        _flightHeight = _pieceWidth;
        _circleDistance = Mathf.PI * _flightHeight / 2;
        _backdropPieceViewModels = new BackdropPieceViewModel[layoutContract.Rows, layoutContract.Columns];
        _backdropNormalMap = _modelService.GenerateNormalMap(sliceContract);

        for (var i = 0; i < _rows; i++)
        {
            for (var j = 0; j < _columns; j++)
            {
                var pieceContract = pieceContracts[i, j];

                _backdropPieceViewModels[i, j] = new BackdropPieceViewModel
                {
                    Position = pieceContract.Position,
                    MappingMesh = pieceContract.MappingMesh,
                    BackseatMesh = pieceContract.BackseatMesh,
                    Distance = _circleDistance + 4f * (_visionWidth - 2 * (pieceContract.Position.x + _pieceWidth * (UnityEngine.Random.value - 1)))
                };
            }
        }
    }

    void Update()
    {
        _finished = true;

        for (var i = 0; i < _rows; i++)
        {
            for (var j = 0; j < _columns; j++)
            {
                var viewModel = _backdropPieceViewModels[i, j];
                if (viewModel.Distance <= 0) { continue; }
                else { _finished = false; }

                Update(viewModel, _modelService.GeneratePieceName(i, j));
            }
        }

        if (_finished)
        {
            //Application.LoadLevel(LevelName.Crossing.ToString());
        }
    }

    private void Update(BackdropPieceViewModel viewModel, string objectName)
    {
        var straightlineSpeed = GlobalConfiguration.PictureHeightInMeter;
        var corneringSpeeds = straightlineSpeed * 0.2f;
        var rotationSpeed = 45 / _pieceWidth;

        if (viewModel.Distance > _circleDistance)
        {
            var step = straightlineSpeed * Time.deltaTime;
            var distance = viewModel.Distance - _circleDistance;
            viewModel.Distance = distance >= step ? (viewModel.Distance - step) : (_circleDistance - (Time.deltaTime - (distance / straightlineSpeed)) * corneringSpeeds);
        }
        else
        {
            viewModel.Distance -= corneringSpeeds * Time.deltaTime;
        }

        if (viewModel.Distance < 0) { viewModel.Distance = 0; }

        var go = GameObject.Find(objectName);

        if (go == null && 2 * (viewModel.Distance + viewModel.Position.x - _pieceWidth) < _visionWidth)
        {
            go = _modelService.GeneratePiece(objectName, viewModel.Position, viewModel.MappingMesh, viewModel.BackseatMesh, new Color32(0xcc, 0xcc, 0xcc, 0xff), _backdropImage, _backdropNormalMap);
        }

        if (go != null)
        {
            var criticalAngle = 180;

            if (viewModel.Distance >= _circleDistance)
            {
                go.transform.position = viewModel.Position + new Vector3(viewModel.Distance - _circleDistance, 0, -_flightHeight);
            }
            else
            {
                var radius = _flightHeight / 2;
                var angle = viewModel.Distance / _circleDistance * Mathf.PI;
                go.transform.position = viewModel.Position + new Vector3(-radius * Mathf.Sin(angle), 0, -radius * (1 - Mathf.Cos(angle)));
            }

            if (viewModel.Distance >= _circleDistance)
            {
                go.transform.rotation = Quaternion.Euler(0, criticalAngle - (viewModel.Distance - _circleDistance) * rotationSpeed, 0);
            }
            else
            {
                go.transform.rotation = Quaternion.Euler(0, criticalAngle + (360 - criticalAngle) * (_circleDistance - viewModel.Distance) / _circleDistance, 0);
            }
        }
    }
}
