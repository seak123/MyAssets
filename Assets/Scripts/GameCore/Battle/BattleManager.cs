using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

public enum BattleState
{
    PreBattle = 0,
    Begin = 1,
    EmbattleHero = 2,
    Schedule = 3,
    PlayCard = 4,
    Action = 5,
    Final = 6,
}

public enum BattleOperationMode
{
    Normal,
    HoldCard,
    SelectTarget,
    SelectPath,
}

[LuaCallCSharp]
public class BattleInputParam
{
    //地图输入 参数类
    public BattleInputParam(Vector2Int pos, ref Vector2Int[] array)
    {
        selectPos = pos;
        selectPath = array;
    }

    //选中的地图坐标
    public Vector2Int selectPos;
    //选中的路径
    public Vector2Int[] selectPath;
}

[LuaCallCSharp]
public class BattleManager : Singleton<BattleManager>
{
    private BattleState curState;
    private BattleOperationMode curMode;

    private readonly float cardExcuteBound = 300f; //卡片触发执行的边界

    private CardItem selectCard; //选中的卡片实体
    private CardExcuteType selectCardType; //选中的卡片执行类型

    private MapCoordinates selectPos; //选中的坐标位
    private List<MapCoordinates> selectPath; //当前选中的路径


    private Unit selectUnit; //当前选中的 单位

    public Action<GestureData> onReleaseCard;
    public Action<Vector3> onSelectMap;
    public Action<BattleOperationMode> onOperationModeChange;
    public Action<MapCoordinates> onTouchMap;

    public BattleOperationMode CurMode
    {
        get
        {
            return curMode;
        }
    }

    public MapCoordinates SelectPos
    {
        get
        {
            return selectPos;
        }
    }

    public List<MapCoordinates> SelectPath
    {
        get
        {
            return selectPath;
        }
    }

    public BattleInputParam GetSelectParam()
    {
        Vector2Int pos = new Vector2Int(selectPos.X, selectPos.Z);
        Vector2Int[] path = new Vector2Int[selectPath.Count];
        for (int i = 0; i < selectPath.Count; ++i)
        {
            path[i] = new Vector2Int(selectPath[i].X, selectPath[i].Z);
        }
        return new BattleInputParam(pos, ref path);
    }

    public void StartBattle()
    {
        curState = BattleState.PreBattle;
        curMode = BattleOperationMode.Normal;
        selectPath = new List<MapCoordinates>();

        GameOperation.Instance.onSingleDown += OnSingleDown;
        GameOperation.Instance.onLongTapDown += OnLongTapDown;
        GameOperation.Instance.onLongTap += OnLongTap;
        GameOperation.Instance.onLongTapUp += OnLoneTapUp;

        Init();
    }
    public void ChangeState(int newState)
    {
        curState = (BattleState)newState;
    }

    public BattleState GetState()
    {
        return curState;
    }

    private void Init()
    {
        MapManager.Instance.Init();
        Performer.Instance.Init();
        CameraManager.Instance.Init();
    }

    private void Update()
    {
        if (IsSelectCardInExecuteArea())
        {
            switch (selectCard.CardExecuteType)
            {
                case CardExcuteType.NoTarget:
                    break;
                case CardExcuteType.SelectPos:
                    SwitchOperationMode(BattleOperationMode.SelectTarget);
                    selectCard.SwitchViewMode(CardViewMode.SelectTarget);
                    break;
                case CardExcuteType.SelectUnit:
                    break;
            }

        }
        else
        {
            if (selectCard != null)
            {
                SwitchOperationMode(BattleOperationMode.HoldCard);
                selectCard.SwitchViewMode(CardViewMode.HoldCard);
            }
        }
        OperationSelecter();
    }

    private void SwitchOperationMode(BattleOperationMode newMode)
    {
        if (curMode == newMode) return;
        curMode = newMode;
        if (onOperationModeChange != null)
        {
            onOperationModeChange.Invoke(newMode);
        }
    }

    private void OperationSelecter()
    {
        switch (curMode)
        {
            case BattleOperationMode.SelectTarget:
                Vector3 targetPos;
                if (GameOperation.Instance.MapPosSelector(out targetPos))
                {
                    selectPos = MapCoordinates.FromPosition(targetPos);
                };
                break;
        }
    }

    //=========================== event ================================

    private void OnLongTapDown(GestureData data)
    {

        Vector3 tapMapPos;
        MapCoordinates tapMapCoord;
        Unit unit;
        //检测 长按的地图位是否包含有效单位
        if (GameOperation.Instance.MapPosSelector(data.pos, out tapMapPos))
        {
            tapMapCoord = MapCoordinates.FromPosition(tapMapPos);
            if (MapManager.Instance.FindUnit(tapMapCoord, out unit))
            {
                selectUnit = unit;
                SwitchOperationMode(BattleOperationMode.SelectPath);
            }
            else
            {
                return;
            }
        };

    }
    private void OnLongTap(GestureData data)
    {
        if (selectUnit != null)
        {
            Vector3 tapMapPos;
            MapCoordinates tapMapCoord;

            //检测 长按的地图位是否包含有效单位
            if (GameOperation.Instance.MapPosSelector(data.pos, out tapMapPos))
            {
                tapMapCoord = MapCoordinates.FromPosition(tapMapPos);
                if (selectPath.Count > 0)
                {
                    MapCoordinates pathEnd = selectPath[selectPath.Count - 1];
                    var addPath = MapManager.Instance.FindPath(pathEnd, tapMapCoord);
                    var astarPath = MapManager.Instance.FindPath(selectUnit.pos, tapMapCoord);


                    if ((selectPath.Count + addPath.Count - 1) > astarPath.Count)
                    {
                        selectPath = astarPath;
                    }
                    else
                    {
                        for (int i = 1; i < addPath.Count; ++i)
                        {
                            selectPath.Add(addPath[i]);
                        }
                    }
                }
                else
                {
                    var astarPath = MapManager.Instance.FindPath(selectUnit.pos, tapMapCoord);

                    selectPath = astarPath;

                }
            }

        }

    }

    private void OnLoneTapUp(GestureData data)
    {
        selectUnit = null;
        selectPath.Clear();
        SwitchOperationMode(BattleOperationMode.Normal);
    }

    private void OnSingleDown(GestureData data)
    {
        if (curMode == BattleOperationMode.Normal)
        {
            Vector3 targetPos;
            if (GameOperation.Instance.MapPosSelector(out targetPos))
            {
                if (onTouchMap != null)
                {
                    onTouchMap.Invoke(MapCoordinates.FromPosition(targetPos));
                }
            };
        }
    }

    //=========================== battle func ===========================

    private bool IsSelectCardInExecuteArea()
    {
        if (selectCard == null)
        {
            return false;
        }

        Vector2 focusPos;
#if UNITY_IOS || UNITY_ANDROID
           focusPos = Input.touches[0].position;
#endif

#if UNITY_STANDALONE_WIN
        focusPos = Input.mousePosition;
#endif


        return focusPos.y >= cardExcuteBound;
    }

    //是否在 卡牌可使用的状态
    public bool IsInCardActiveState()
    {
        return curState == BattleState.EmbattleHero || curState == BattleState.PlayCard;
    }

    //=========================== card func =============================
    //选中卡片
    public bool SelectCard(CardItem card)
    {
        if (!IsInCardActiveState())
        {
            return false;
        }
        if (curMode == BattleOperationMode.Normal)
        {
            selectCard = card;
            selectCardType = card.CardExecuteType;
            SwitchOperationMode(BattleOperationMode.HoldCard);
            return true;
        }
        return false;
    }

    public bool ReleaseCard(CardItem card)
    {
        bool result = true;
        if (card.Uid != selectCard.Uid || !IsSelectCardInExecuteArea())
        {
            result = false;
        }
        selectCard = null;
        SwitchOperationMode(BattleOperationMode.Normal);
        return result;
    }
}
