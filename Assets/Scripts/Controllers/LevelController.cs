using Tridimensional.Puzzle.Foundation;
using Tridimensional.Puzzle.Foundation.Enumeration;
using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy;
using Tridimensional.Puzzle.UI.ViewModel;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    IModelService _modelService;
    Texture2D _backdropImage;
    Texture2D _backdropNormalMap;
    BackdropPieceViewModel[,] _backdropPieceViewModels;
    float _visionWidth;
    float _pieceWidth;
    float _flightHeight;
    float _circleDistance;
    float _rotationCoefficient;
    float _speed;
    int _rows;
    int _columns;

    void Awake()
    {
        _modelService = new ModelService(new PieceService(), new SliceStrategyFactory());
        _backdropImage = Resources.Load("Image/LevelBackground/0") as Texture2D;

        var backdropLayoutContract = _modelService.GetProperLayout(Screen.width, Screen.height, 100);
        var sliceContract = _modelService.GetSlice(backdropLayoutContract, SliceProgram.Default);
        var pieceContracts = _modelService.GeneratePiece(sliceContract, _backdropImage);

        _rows = backdropLayoutContract.Rows;
        _columns = backdropLayoutContract.Columns;
        _visionWidth = GlobalConfiguration.PictureScaleInMeter * Screen.width / Screen.height;
        _pieceWidth = GlobalConfiguration.PictureScaleInMeter / _rows;
        _flightHeight = _pieceWidth;
        _circleDistance = Mathf.PI * _flightHeight / 2;
        _rotationCoefficient = 45 / _pieceWidth;
        _speed = GlobalConfiguration.PictureScaleInMeter;
        _backdropPieceViewModels = new BackdropPieceViewModel[backdropLayoutContract.Rows, backdropLayoutContract.Columns];
        _backdropNormalMap = _modelService.GenerateNormalMap(sliceContract, _backdropImage);

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
        for (var i = 0; i < _rows; i++)
        {
            for (var j = 0; j < _columns; j++)
            {
                var viewModel = _backdropPieceViewModels[i, j];
                if (viewModel.Distance <= 0) { continue; }

                var fallingSpeed = _speed * 0.2f;

                if (viewModel.Distance > _circleDistance)
                {
                    var step = _speed * Time.deltaTime;
                    var distance = viewModel.Distance - _circleDistance;
                    viewModel.Distance = distance >= step ? (viewModel.Distance - step) : (_circleDistance - (Time.deltaTime - (distance / _speed)) * fallingSpeed);
                }
                else
                {
                    viewModel.Distance -= fallingSpeed * Time.deltaTime;
                }

                if (viewModel.Distance < 0) { viewModel.Distance = 0; }

                var go = GameObject.Find(GenerateBackdropPieceName(i, j));

                if (go == null && 2 * (viewModel.Distance + viewModel.Position.x - _pieceWidth) < _visionWidth)
                {
                    go = GenerateBackdropPiece(GenerateBackdropPieceName(i, j), new Vector3(_visionWidth, 0, 0), viewModel.BackseatMesh, viewModel.MappingMesh);
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
                        go.transform.rotation = Quaternion.Euler(0, criticalAngle - (viewModel.Distance - _circleDistance) * _rotationCoefficient, 0);
                    }
                    else
                    {
                        go.transform.rotation = Quaternion.Euler(0, criticalAngle + (360 - criticalAngle) * (_circleDistance - viewModel.Distance) / _circleDistance, 0);
                    }
                }
            }
        }
    }

    GameObject GenerateBackdropPiece(string name, Vector3 position, Mesh backseatMesh, Mesh mappingMesh)
    {
        var go = new GameObject(name);
        go.AddComponent<MeshFilter>().mesh = backseatMesh;
        go.AddComponent<MeshRenderer>().material.color = new Color32(0xcc, 0xcc, 0xcc, 0xff);
        go.transform.position = position;

        var mapping = new GameObject("Mapping");
        mapping.AddComponent<MeshFilter>().mesh = mappingMesh;
        mapping.AddComponent<MeshRenderer>().material = Resources.Load("Material/BumpedDiffuse") as Material;
        mapping.transform.renderer.material.SetTexture("_MainTex", _backdropImage);
        mapping.transform.renderer.material.SetTexture("_BumpMap", _backdropNormalMap);

        mapping.transform.parent = go.transform;
        mapping.transform.localPosition = new Vector3(0, 0, 0);

        return go;
    }

    string GenerateBackdropPieceName(int row, int column)
    {
        return string.Format("Piece <{0:000},{1:000}>", row, column);
    }
}
