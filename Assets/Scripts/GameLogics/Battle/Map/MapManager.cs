using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

enum MapState
{
    Normal,
    SelectPoint,
    SelectPath,
}

public enum MapCellState
{
    Empty,
    Obstruct,
    Occupy,
}

//地图单位格子信息存储类
public class MapCellData
{
    public MapCellState state = MapCellState.Empty;
    public Unit unit;
}

class AStarNodeInfo
{
    public MapCoordinates pos = new MapCoordinates(0, 0);
    public int gWeight = 0;
    public int hWeight = 0;
    public AStarNodeInfo parent;
};

class AccessNodeInfo
{
    public MapCoordinates pos = new MapCoordinates(0, 0);
    public int cost = 0;
}

[LuaCallCSharp]
public class MapManager : Singleton<MapManager>, IBattleManager
{
    MapGrid mapGrid;
    MapState state;

    MapCoordinates selectPoint;

    Dictionary<int, Unit> unitMap;
    MapCellData[] mapDatas;  //地图单位单元数据

    bool isInited = false;
    public Vector2 MapCenter
    {
        get
        {
            if (mapGrid != null)
            {

                float xPos = ((MapConstant.sideLength) * mapGrid.width) / 2;
                float yPos = ((MapConstant.sideLength) * mapGrid.height) / 2;
                return new Vector2(xPos, yPos);
            }
            return Vector2.zero;
        }
    }

    private void Update()
    {
        if (isInited)
        {
            RefreshMapView();
        }
    }
    public void Init()
    {
        BattleManager.Instance.onOperationModeChange += OnOperationStateChange;
        BattleManager.Instance.onTouchMap += OnTouchMap;


        state = MapState.Normal;
        unitMap = new Dictionary<int, Unit>();

        isInited = true;
    }
    public void LoadMap(int col, int row)
    {
        ResourceManager.Instance.LoadAssetAsync("Battle/Map/MapGrid", (asset) =>
         {
             GameObject obj = Instantiate(asset) as GameObject;
             obj.transform.position = Vector3.zero;
             var mapGrid = obj.GetComponent<MapGrid>();
             mapGrid.width = col;
             mapGrid.height = row;
             mapGrid.CreateMapCells();

             this.mapGrid = mapGrid;
             mapDatas = new MapCellData[col * row];
         });
    }

    private void RefreshMapView()
    {
        switch (state)
        {
            case MapState.Normal:
                break;
            case MapState.SelectPoint:
                if (BattleManager.Instance.SelectPos.Equal(selectPoint))
                {
                    break;
                }
                selectPoint = BattleManager.Instance.SelectPos;
                mapGrid.ResetCells();

                mapGrid.ColorCell(selectPoint, Color.green);
                break;
            case MapState.SelectPath:
                if (BattleManager.Instance.SelectPath.Count > 0)
                {
                    mapGrid.ResetCells();
                    for(int i = 0; i < BattleManager.Instance.SelectPath.Count; ++i)
                    {
                        mapGrid.ColorCell(BattleManager.Instance.SelectPath[i], Color.yellow);
                    }
                }
                break;
        }
    }
    //=============================  事件 ======================================

    private void OnOperationStateChange(BattleOperationMode current)
    {
        switch (current)
        {
            case BattleOperationMode.Normal:
            case BattleOperationMode.HoldCard:
                SwitchMapState(MapState.Normal);
                break;
            case BattleOperationMode.SelectTarget:
                SwitchMapState(MapState.SelectPoint);
                break;
            case BattleOperationMode.SelectPath:
                SwitchMapState(MapState.SelectPath);
                break;
        }
    }

    private void OnTouchMap(MapCoordinates pos)
    {
        if (GetMapCellData(pos).state == MapCellState.Occupy)
        {
            var unit = GetMapCellData(pos).unit;
            DisplayUnitArea(unit);
        }
        else
        {
            mapGrid.ResetCells();
        }
    }


    private void SwitchMapState(MapState next)
    {
        if (state == next) return;
        state = next;
        switch (state)
        {
            case MapState.Normal:
                mapGrid.ResetCells();
                break;
        }
    }
    //===================== map function =================================
    public MapCellData GetMapCellData(MapCoordinates pos)
    {
        if (!IsMapCoordValid(pos))
        {
            DebugUtil.Error("Get Wrong Cell Data, pos is ", pos);
        }
        int key = pos.Z * this.mapGrid.width + pos.X;
        if (mapDatas[key] == null)
        {
            mapDatas[key] = new MapCellData();
        }
        return mapDatas[key];
    }

    private void SetMapCellState(MapCoordinates pos, MapCellState newState)
    {
        var cell = GetMapCellData(pos);
        cell.state = newState;
    }

    private void DisplayUnitArea(Unit unit)
    {
        var accessInfo = FindAccessNode(unit.pos, unit.vo.mobility + unit.vo.attackRange);
        for (int i = 0; i < accessInfo.Count; ++i)
        {
            mapGrid.ColorCell(accessInfo[i].pos, accessInfo[i].cost <= unit.vo.mobility ? Color.blue : Color.red);
        }
    }

    //===================== map base function ================================

    //Astar
    public List<MapCoordinates> FindPath(MapCoordinates start, MapCoordinates end)
    {
        List<AStarNodeInfo> openList = new List<AStarNodeInfo>();
        bool[,] closeArray = new bool[mapGrid.width, mapGrid.height];


        var keyNode = new AStarNodeInfo();
        keyNode.pos = start;
        keyNode.gWeight = 0;
        keyNode.hWeight = MapCoordinates.Distance(start, end);
        openList.Add(keyNode);

        Vector2Int[] neighbourOffsets = new Vector2Int[4]
       {
            new Vector2Int(-1,0),
            new Vector2Int(1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1)
       };
        while (openList.Count > 0)
        {
            AStarNodeInfo curNode = openList[0];
            openList.RemoveAt(0);
            //check if curNode is end
            if (curNode.pos.Equal(end))
            {
                keyNode = curNode;
                break;
            }

            //insert neighbours
            for (int i = 0; i < neighbourOffsets.Length; ++i)
            {
                MapCoordinates neighbourPos = new MapCoordinates(curNode.pos.X + neighbourOffsets[i].x, curNode.pos.Z + neighbourOffsets[i].y);
                if (!IsMapCoordCanMove(neighbourPos) || closeArray[neighbourPos.X, neighbourPos.Z]) continue;

                //check if openList has contained same node
                AStarNodeInfo sameNode = openList.Find((AStarNodeInfo node) => { return node.pos.Equal(neighbourPos); });
                if (sameNode != null)
                {
                    if (sameNode.gWeight > curNode.gWeight + 1)
                    {
                        sameNode.parent = curNode;
                        sameNode.gWeight = curNode.gWeight + 1;
                    }
                    continue;
                }

                AStarNodeInfo newNode = new AStarNodeInfo();
                newNode.pos = neighbourPos;
                newNode.gWeight = curNode.gWeight + 1;
                newNode.hWeight = MapCoordinates.Distance(neighbourPos, end);
                newNode.parent = curNode;
                openList.Add(newNode);
            }
            closeArray[curNode.pos.X, curNode.pos.Z] = true;

            openList.Sort((AStarNodeInfo a, AStarNodeInfo b) =>
            {
                return (a.gWeight + a.hWeight) - (b.gWeight + b.hWeight);
            });
        }

        //return astar result-path
        List<MapCoordinates> path = new List<MapCoordinates>();
        while (keyNode != null)
        {
            path.Insert(0, keyNode.pos);
            keyNode = keyNode.parent;
        }

        return path;
    }

    //BFS
    private List<AccessNodeInfo> FindAccessNode(MapCoordinates start, int range)
    {
        List<AccessNodeInfo> accessNods = new List<AccessNodeInfo>();
        List<AccessNodeInfo> openList = new List<AccessNodeInfo>();
        bool[,] visitArray = new bool[mapGrid.width, mapGrid.height];
        AccessNodeInfo startInfo = new AccessNodeInfo();
        startInfo.pos = start;
        startInfo.cost = 0;
        openList.Add(startInfo);
        visitArray[start.X, start.Z] = true;
        Vector2Int[] neighbourOffsets = new Vector2Int[4]
       {
            new Vector2Int(-1,0),
            new Vector2Int(1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1)
       };
        while (openList.Count > 0)
        {
            AccessNodeInfo curNode = openList[0];
            openList.RemoveAt(0);
            if (curNode.cost > range)
            {
                continue;
            }
            

            var sameNode = accessNods.Find((AccessNodeInfo node) => { return node.pos.Equal(curNode.pos); });
            if (sameNode != null)
            {
                if (sameNode.cost <= curNode.cost)
                {
                    break;
                }
                else
                {
                    sameNode.cost = curNode.cost;
                }
            }
            else
            {
                accessNods.Add(curNode);
            }

            for (int i = 0; i < neighbourOffsets.Length; ++i)
            {
                MapCoordinates neighbourPos = new MapCoordinates(curNode.pos.X + neighbourOffsets[i].x, curNode.pos.Z + neighbourOffsets[i].y);
                var v = IsMapCoordValid(neighbourPos);
                var b = visitArray[curNode.pos.X, curNode.pos.Z];
                if (!IsMapCoordValid(neighbourPos)||visitArray[neighbourPos.X, neighbourPos.Z]) continue;
                AccessNodeInfo newNode = new AccessNodeInfo();
                newNode.pos = neighbourPos;
                newNode.cost = curNode.cost + 1;
                openList.Add(newNode);
                visitArray[neighbourPos.X, neighbourPos.Z] = true;
            }
        }

        return accessNods;
    }

    private bool IsMapCoordValid(MapCoordinates coord)
    {
        return coord.X >= 0 && coord.Z >= 0 && coord.X < mapGrid.width && coord.Z < mapGrid.height;
    }

    private bool IsMapCoordCanMove(MapCoordinates coord)
    {
        if (!IsMapCoordValid(coord)) return false;
        return GetMapCellData(coord).state == MapCellState.Empty;
    }

    //===================== unit function ================================

    public void CreateUnit(int uid, UnitVO unitVO, MapCoordinates createPos)
    {
        ResourceManager.Instance.LoadAssetAsync(unitVO.prefabPath, (asset) =>
        {
            GameObject obj = Instantiate(asset) as GameObject;
            UnitAvatar avatar = obj.AddComponent<UnitAvatar>();
            avatar.UnitPos = createPos;
            Unit unit = new Unit();
            unit.vo = unitVO;
            unit.avatar = avatar;
            unit.pos = createPos;
            unitMap.Add(uid, unit);
            obj.transform.SetParent(mapGrid.MapUnitRoot.transform);
            obj.transform.position = createPos.WorldPosition;
            SetMapCellState(createPos, MapCellState.Occupy);
            GetMapCellData(createPos).unit = unit;

        });
    }

    public bool FindUnit(MapCoordinates pos, out Unit unit)
    {
        var data = GetMapCellData(pos);
        if (data.state == MapCellState.Occupy)
        {
            if (data.unit != null)
            {
                unit = data.unit;
                return true;
            }

        }
        unit = null;
        return false;
    }
}
