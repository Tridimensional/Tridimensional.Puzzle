using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Core.Enumeration;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using Tridimensional.Puzzle.UI.ViewModel;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    BackdropPieceViewModel[,] _backdropPieceViewModels;
    bool _finished;
    float _circleDistance;
    float _corneringSpeed;
    float _flightHeight;
    float _pieceWidth;
    float _rotationSpeed;
    float _straightSpeed;
    float _visionWidth;
    IModelService _modelService;
    ISceneService _sceneService;
    int _rows;
    int _columns;
    Texture2D _backdropImage;
    Texture2D _backdropNormalMap;

    void Awake()
    {
        _modelService = ModelService.Instance;
        _sceneService = SceneService.Instance;

        _sceneService.Initialize(gameObject.camera);

        InitializeEnvironment();
    }

    void InitializeEnvironment()
    {
        _backdropImage = Resources.Load("Image/LevelBackground/0") as Texture2D;

        var layoutContract = _modelService.GetProperLayout(Screen.width, Screen.height, 100);
        var sliceContract = _modelService.GetSlice(_backdropImage, layoutContract, SlicePattern.Default);
        var pieceContracts = _modelService.GeneratePieceContracts(sliceContract);

        _backdropNormalMap = _modelService.GenerateNormalMap(sliceContract);
        _rows = layoutContract.Rows;
        _columns = layoutContract.Columns;
        _visionWidth = GlobalConfiguration.PictureHeightInMeter * Screen.width / Screen.height;
        _pieceWidth = _visionWidth / _columns;
        _flightHeight = _pieceWidth;
        _circleDistance = Mathf.PI * _flightHeight / 2;
        _backdropPieceViewModels = new BackdropPieceViewModel[layoutContract.Rows, layoutContract.Columns];
        _straightSpeed = GlobalConfiguration.PictureHeightInMeter;
        _corneringSpeed = _straightSpeed * 0.2f;
        _rotationSpeed = 45 / _pieceWidth;

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
            Application.LoadLevel(LevelName.Crossing.ToString());
        }
    }

    private void Update(BackdropPieceViewModel viewModel, string pieceName)
    {
        if (viewModel.Distance > _circleDistance)
        {
            var straightStep = _straightSpeed * Time.deltaTime;
            var distance = viewModel.Distance - _circleDistance;
            viewModel.Distance = distance >= straightStep ? (viewModel.Distance - straightStep) : (_circleDistance - (Time.deltaTime - (distance / _straightSpeed)) * _corneringSpeed);
        }
        else
        {
            viewModel.Distance -= _corneringSpeed * Time.deltaTime;
        }

        if (viewModel.Distance < 0) { viewModel.Distance = 0; }

        var go = GameObject.Find(pieceName);

        if (go == null && 2 * (viewModel.Distance + viewModel.Position.x - _pieceWidth) < _visionWidth)
        {
            go = _modelService.GeneratePiece(pieceName, viewModel.Position, viewModel.MappingMesh, viewModel.BackseatMesh, new Color32(0xcc, 0xcc, 0xcc, 0xff), _backdropImage, _backdropNormalMap);
            GameObject.DontDestroyOnLoad(go);
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
                var radius = _flightHeight * 0.5f;
                var angle = viewModel.Distance / _circleDistance * Mathf.PI;
                go.transform.position = viewModel.Position + new Vector3(-radius * Mathf.Sin(angle), 0, -radius * (1 - Mathf.Cos(angle)));
            }

            if (viewModel.Distance >= _circleDistance)
            {
                go.transform.rotation = Quaternion.Euler(0, criticalAngle - (viewModel.Distance - _circleDistance) * _rotationSpeed, 0);
            }
            else
            {
                go.transform.rotation = Quaternion.Euler(0, criticalAngle + (360 - criticalAngle) * (_circleDistance - viewModel.Distance) / _circleDistance, 0);
            }
        }
    }
}
