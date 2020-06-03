using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;

    public MapCell cellPrefab;
    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    MapCell[] cells;

    public Text cellLabelPrefab;
    Canvas gridCanvas;
    MapMesh mapMesh;
    GameObject mapCellRoot;
    GameObject mapUnitRoot;

    public GameObject MapUnitRoot
    {
        get
        {
            return mapUnitRoot;
        }
    }

    public void CreateMapCells()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        mapMesh = GetComponentInChildren<MapMesh>();

        cells = new MapCell[height * width];

        for (int z = 0, i = 0; z < height; ++z)
        {
            for (int x = 0; x < width; ++x)
            {
                CreateCell(x, z, i++);
            }
        }
        mapMesh.Triangulate(cells);
    }
    private void Awake()
    {
        mapCellRoot = transform.Find("MapCellRoot").gameObject;

        mapUnitRoot = transform.Find("MapUnitRoot").gameObject;
    }

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    public Vector3 World2MapPostion(Vector3 pos)
    {
        return transform.InverseTransformPoint(pos);

    }

    public void ResetCells()
    {
        for (int z = 0, i = 0; z < height; ++z)
        {
            for (int x = 0; x < width; ++x)
            {
                MapCell cell = cells[i++];
                cell.color = defaultColor;
            }
        }
        mapMesh.Triangulate(cells);
    }


    public void ColorCell(MapCoordinates coordinates, Color color)
    {

        int index = coordinates.X + coordinates.Z * width;
        MapCell cell = cells[index];
        cell.color = color;
        mapMesh.Triangulate(cells);
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + 0.5f) * MapConstant.sideLength;
        position.y = 0;
        position.z = (z + 0.5f) * MapConstant.sideLength;

        MapCell cell = cells[i] = Instantiate<MapCell>(cellPrefab);
        cell.transform.SetParent(mapCellRoot.transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = MapCoordinates.FromPosition(position);
        cell.color = defaultColor;

        Text label = Instantiate<Text>(cellLabelPrefab, gridCanvas.transform, false);
        label.rectTransform.SetParent(gridCanvas.transform);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }
}
