using System.Collections.Generic;
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

        _backdropNormalMap = _modelService.GenerateNormalMap(sliceContract, _backdropImage);

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

    Texture2D GetNormalMap(SliceContract sc, int rows, int columns)
    {
        float widthInSpace = sc.Vertexes[0, columns].x;
        float heightInSpace = sc.Vertexes[rows, 0].y;

        Texture2D linePic = new Texture2D(1024, 640);
        for (int x = 0; x < linePic.width; x++)
        {
            for (int y = 0; y < linePic.height; y++)
            {
                linePic.SetPixel(x, y, Color.white);
            }
        }
        linePic.Apply();


        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                //Debug.Log(sc.Lines.Get(i,j,i+1,j));
                //Debug.Log(sc.Lines.Get(i,j,i,j+1));

                Vector2[] v1 = sc.Lines.Get(i, j, i + 1, j);
                Vector2[] v2 = sc.Lines.Get(i, j, i, j + 1);

                int startX = 0;
                int startY = 0;
                //int endX = 0;
                //int endY = 0;

                if (v1 != null)
                {
                    for (int z = 0; z < v1.Length; z++)
                    {
                        if (z == 0)
                        {
                            startX = Mathf.FloorToInt(linePic.width * (v1[z].x / widthInSpace));
                            startY = Mathf.FloorToInt(linePic.height * (v1[z].y / heightInSpace));
                        }
                        else
                        {
                            int currentVX = Mathf.FloorToInt(linePic.width * (v1[z].x / widthInSpace));
                            int currentVY = Mathf.FloorToInt(linePic.height * (v1[z].y / heightInSpace));
                            DrawLine(ref linePic, new IntVector2(startX, startY), new IntVector2(currentVX, currentVY));
                            startX = currentVX;
                            startY = currentVY;
                        }
                    }
                }

                if (v2 != null)
                {
                    for (int z = 0; z < v2.Length; z++)
                    {
                        if (z == 0)
                        {
                            startX = Mathf.FloorToInt(linePic.width * (v2[z].x / widthInSpace));
                            startY = Mathf.FloorToInt(linePic.height * (v2[z].y / heightInSpace));
                        }
                        else
                        {
                            int currentVX = Mathf.FloorToInt(linePic.width * (v2[z].x / widthInSpace));
                            int currentVY = Mathf.FloorToInt(linePic.height * (v2[z].y / heightInSpace));
                            DrawLine(ref linePic, new IntVector2(startX, startY), new IntVector2(currentVX, currentVY));
                            startX = currentVX;
                            startY = currentVY;
                        }
                    }
                }

            }
        }

        return GenerateNormalMapFromHeightMap(Texture2DGrayBloom(ref linePic, 4), 10.0f);

        /*Debug.Log(rows + "-----" + columns);
        Debug.Log(bigx + "-----" + bigy);
        //Debug.Log(sc.Vertexes.Length);
        //Debug.Log(sc.Lines.Get(1,1,1,2));
		
        Texture2D t2d = new Texture2D(100,100);
        return t2d;*/
    }

    private void DrawLine(ref Texture2D t2d, IntVector2 start, IntVector2 end)
    {
        if (Mathf.Abs(start.x - end.x) >= Mathf.Abs(start.y - end.y))
        {
            for (int i = start.x; i < end.x + 1; i++)
            {
                float ypoint = start.y + (i * 1.0f - start.x) / (end.x - start.x) * (end.y - start.y);

                int lowY = Mathf.FloorToInt(ypoint);
                float lowColor = (ypoint - lowY) * 0.9f;
                float hiColor = 1f - lowColor;
                t2d.SetPixel(i, lowY, new Color(lowColor, lowColor, lowColor, 1f));
                t2d.SetPixel(i, lowY + 1, new Color(hiColor, hiColor, hiColor, 1f));
            }
        }
        else
        {
            for (int i = start.y; i < end.y + 1; i++)
            {
                float xpoint = start.x + (i * 1.0f - start.y) / (end.y - start.y) * (end.x - start.x);

                int lowX = Mathf.FloorToInt(xpoint);
                float lowColor = (xpoint - lowX) * 0.9f;
                float hiColor = 1f - lowColor;

                t2d.SetPixel(lowX, i, new Color(lowColor, lowColor, lowColor, 1f));
                t2d.SetPixel(lowX + 1, i, new Color(hiColor, hiColor, hiColor, 1f));
            }
        }
        t2d.Apply();
    }

    /// <summary>
    /// Texture2s the D gray bloom.
    /// </summary>
    /// <returns>
    /// The D gray bloom.
    /// </returns>
    /// <param name='t2d'>
    /// T2d.
    /// </param>
    private Texture2D Texture2DGrayBloom(ref Texture2D t2d, int radius)
    {
        int countnum = 0;
        int countnum2 = 0;

        Texture2D newT2d = new Texture2D(t2d.width, t2d.height);
        for (int x = 0; x < newT2d.width; x++)
        {
            for (int y = 0; y < newT2d.height; y++)
            {
                newT2d.SetPixel(x, y, Color.white);
            }
        }
        newT2d.Apply();


        Dictionary<string, float> colordict = new Dictionary<string, float>();

        for (int x = 0; x < t2d.width; x++)
        {
            for (int y = 0; y < t2d.height; y++)
            {
                float minDist = radius + 1f;
                float gsaa = t2d.GetPixel(x, y).grayscale;
                if (gsaa < 1)
                {
                    for (int xa = (-radius + 1); xa < radius; xa++)
                    {
                        for (int ya = (-radius + 1); ya < radius; ya++)
                        {
                            if (x + xa < 0 || y + ya < 0 || x + xa > t2d.width || y + ya > t2d.height)
                            {
                                continue;
                            }

                            //caculate the color
                            float ndist = Mathf.Sqrt((xa * xa + ya * ya) * 1.0f);
                            if (ndist > radius) continue;
                            float flc = ndist / radius;

                            if (colordict.ContainsKey((x + xa) + "_" + (y + ya)))
                            {
                                float oldColor = colordict[(x + xa) + "_" + (y + ya)];
                                if (oldColor > flc)
                                {
                                    newT2d.SetPixel(x + xa, y + ya, new Color(flc, flc, flc, 1f));
                                    colordict[(x + xa) + "_" + (y + ya)] = flc;
                                }
                            }
                            else
                            {
                                newT2d.SetPixel(x + xa, y + ya, new Color(flc, flc, flc, 1f));
                                colordict.Add((x + xa) + "_" + (y + ya), flc);
                            }
                        }
                    }
                }

                /*if(minDist < radius)
                {
                    countnum ++;
                    float colorel = minDist / radius;
                    newT2d.SetPixel(x,y,new Color(colorel,colorel,colorel,1f));
                }
                else
                {
                    newT2d.SetPixel(x,y,new Color(1f,1f,1f,1f));
                }*/
            }
        }
        newT2d.Apply();
        //Debug.Log(countnum);
        //Debug.Log(countnum2);
        return newT2d;
    }

    /// <summary>
    /// Generates the normal map from height map.
    /// </summary>
    /// <returns>
    /// The normal map from height map.
    /// </returns>
    /// <param name='source'>
    /// Source.
    /// </param>
    /// <param name='strength'>
    /// Strength.
    /// </param>
    private Texture2D GenerateNormalMapFromHeightMap(Texture2D source, float strength)
    {
        strength = Mathf.Clamp(strength, 0.0F, 10.0F);
        Texture2D result;
        float xLeft;
        float xRight;
        float yUp;
        float yDown;
        float yDelta;
        float xDelta;
        result = new Texture2D(source.width, source.height, TextureFormat.ARGB32, true);
        for (int by = 0; by < result.height; by++)
        {
            for (int bx = 0; bx < result.width; bx++)
            {
                xLeft = source.GetPixel(bx - 1, by).grayscale * strength;
                xRight = source.GetPixel(bx + 1, by).grayscale * strength;
                yUp = source.GetPixel(bx, by - 1).grayscale * strength;
                yDown = source.GetPixel(bx, by + 1).grayscale * strength;
                xDelta = ((xLeft - xRight) + 1) * 0.5f;
                yDelta = ((yUp - yDown) + 1) * 0.5f;
                result.SetPixel(bx, by, new Color(xDelta, yDelta, 1.0f, yDelta));
            }
        }
        result.Apply();
        return result;
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

public struct IntVector2
{
    public int x;
    public int y;

    public IntVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    int sqrMagnitude
    {
        get { return x * x + y * y; }
    }
}
