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
    LayoutContract _backdropLayoutContract;
    BackdropPieceViewModel[,] _backdropPieceViewModels;
    float _pieceWidth;
    float _flightHeight;
    float _circleDistance;
    float _rotationCoefficient;
    float _speed;

    void Awake()
    {
        _modelService = new ModelService(new PieceService(), new SliceStrategyFactory());
        _backdropImage = Resources.Load("Image/LevelBackground/0") as Texture2D;
        _backdropLayoutContract = _modelService.GetProperLayout(GlobalConfiguration.PictureScaleInMeter * Screen.width / Screen.height, GlobalConfiguration.PictureScaleInMeter, 100);
        _backdropPieceViewModels = new BackdropPieceViewModel[_backdropLayoutContract.Rows, _backdropLayoutContract.Columns];
        _pieceWidth = _backdropLayoutContract.Width / _backdropLayoutContract.Columns;
        _flightHeight = _backdropLayoutContract.Width / _backdropLayoutContract.Columns;
        _circleDistance = _flightHeight * Mathf.PI / 2;
        _rotationCoefficient = 45 / _pieceWidth;
        _speed = _backdropLayoutContract.Height;

        var sliceContract = _modelService.GetSlice(_backdropLayoutContract, SliceProgram.Random);
        var pieceContracts = _modelService.GeneratePiece(sliceContract, _backdropImage);

        for (var i = 0; i < _backdropLayoutContract.Rows; i++)
        {
            for (var j = 0; j < _backdropLayoutContract.Columns; j++)
            {
                var pieceContract = pieceContracts[i, j];

                _backdropPieceViewModels[i, j] = new BackdropPieceViewModel
                {
                    Position = pieceContract.Position,
                    MappingMesh = pieceContract.MappingMesh,
                    BackseatMesh = pieceContract.BackseatMesh,
                    Distance = _circleDistance + 4f * (_backdropLayoutContract.Width - 2 * (pieceContract.Position.x + _pieceWidth * (UnityEngine.Random.value - 1)))
                };
            }
        }
    }

    void Update()
    {
        for (var i = 0; i < _backdropLayoutContract.Rows; i++)
        {
            for (var j = 0; j < _backdropLayoutContract.Columns; j++)
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

                if (go == null && 2 * (viewModel.Distance + viewModel.Position.x - _pieceWidth) < _backdropLayoutContract.Width)
                {
                    go = GenerateBackdropPiece(GenerateBackdropPieceName(i, j), new Vector3(_backdropLayoutContract.Width, 0, 0), viewModel.BackseatMesh, viewModel.MappingMesh, _backdropImage);
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

    GameObject GenerateBackdropPiece(string name, Vector3 position, Mesh backseatMesh, Mesh mappingMesh, Texture2D image)
    {
        var go = new GameObject(name);
        go.AddComponent<MeshFilter>().mesh = backseatMesh;
        go.AddComponent<MeshRenderer>().material.color = Color.white;
        go.transform.position = position;

        var mapping = new GameObject("Mapping");
        var mesh = mapping.AddComponent<MeshFilter>().mesh = mappingMesh;
        mapping.AddComponent<MeshRenderer>().material.mainTexture = image;
        mapping.transform.parent = go.transform;
        mapping.transform.localPosition = new Vector3(0, 0, 0);

        return go;
    }

    string GenerateBackdropPieceName(int row, int column)
    {
        return string.Format("Backdrop <{0:000},{1:000}>", row, column);
    }
}
