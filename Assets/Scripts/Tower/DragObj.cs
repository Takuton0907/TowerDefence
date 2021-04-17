using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

/// <summary> タワーの持ち上げた時の処理 </summary>
public class DragObj : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform parentTransform;

    [SerializeField] GameObject TowerPrefab;

    GameObject copyObj;
    Tilemap tilemap;
    List<int> towePosiIndexs;

    [SerializeField] int _cost = 10;

    float _lastSpeedRate;

    private void Start()
    {
        parentTransform = GameObject.FindGameObjectWithTag("Map").transform;
    }

    //オブジェクトを持ち始める
    public void OnBeginDrag(PointerEventData data)
    {
        if (!LevelManager.Instance.HasTowerCount()) return;

        if (data == null) return;
        if (LevelManager.Instance._cost < this._cost)
        {
            Debug.Log("コストが足りません");
            return;
        }

        Debug.Log("OnBeginDrag");
        copyObj = Instantiate(gameObject, gameObject.transform.position, Quaternion.identity, transform.parent);
        copyObj.GetComponent<CanvasGroup>().blocksRaycasts = false;

        towePosiIndexs = LevelManager.Instance.GetIndexs(TILE.SET_TOWER);
        GameObject overTileObj = GameObject.FindGameObjectWithTag("OverTile");
        tilemap = overTileObj.GetComponent<Tilemap>();
        TileMapController.SetToerMap(ref tilemap, LevelManager.Instance._mapData, towePosiIndexs);

        if (LevelManager.Instance._enemyManager.instanceEnemys.Count != 0)
        {
            _lastSpeedRate = LevelManager.Instance._enemyManager.instanceEnemys[0]._speedRate;
        }

        LevelManager.Instance.DragSpeedChange();

        copyObj.transform.GetChild(0).gameObject.SetActive(true);

        float rect = TowerPrefab.GetComponent<TowerBase>()._area * 0.7f;

        copyObj.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(rect, rect);
    }

    public void OnDrag(PointerEventData data)
    {
        if (!LevelManager.Instance.HasTowerCount()) return;
        if (data == null) return;
        if (copyObj == null) return;
        if (LevelManager.Instance._cost < this._cost) return;

        Vector3 TargetPos = Camera.main.ScreenToWorldPoint(data.position);
        TargetPos.z = -1;
        copyObj.transform.position = TargetPos;
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (!LevelManager.Instance.HasTowerCount()) return;

        if (data == null)
        {
            Destroy(copyObj);
            return;
        }
        if (copyObj == null)
        {
            Destroy(copyObj);
            return;
        }
        if (LevelManager.Instance._cost < this._cost)
        {
            Destroy(copyObj);
            return;
        }

        for (int i = 0; i < copyObj.transform.childCount; i++)
        {
            copyObj.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_lastSpeedRate == 0)
        {
            LevelManager.Instance.DragSpeedChange();
        }
        else
        {
            LevelManager.Instance.DragSpeedChange(_lastSpeedRate);
        }

        Vector3 posi = Vector3Int.FloorToInt(copyObj.transform.position);
        posi = new Vector3(posi.x + 0.5f, posi.y + 0.5f, 0);

        foreach (var item in LevelManager.Instance._mapData.mapDatas)
        {
            //Debug.Log($"item.posi = {item.posi} posi = {posi}");
            if (item.posi == posi - new Vector3(0.5f, 0.5f, 0))
            {
                if (item.tower == true)
                {

                    Destroy(copyObj);

                    tilemap.ClearAllTiles();
                    return;
                }
                break;
            }
        }


        tilemap.ClearAllTiles();

        Destroy(copyObj);
        copyObj = Instantiate(TowerPrefab, gameObject.transform.position, Quaternion.identity, transform.parent);
        DropArea dropArea = parentTransform.GetComponent<DropArea>();
        Debug.Log(parentTransform);

        if (dropArea == null)
        {
            Debug.Log("DropAreaが見つかりません");
            Destroy(copyObj);
            return;
        }

        int setPosiIndex = int.MaxValue;
        foreach (var item in towePosiIndexs)
        {
            if (LevelManager.Instance._mapData.mapDatas[item].posi + new Vector3(0.5f, 0.5f, 0) == posi)
            {
                Debug.Log("一致しました");
                setPosiIndex = item;
                break;
            }
        }
        if (setPosiIndex == int.MaxValue)
        {
            Debug.Log("一致しませんでした");
            Destroy(copyObj);
            return;
        }

        copyObj.transform.SetParent(parentTransform);
        copyObj.GetComponent<CanvasGroup>().blocksRaycasts = true;
        LevelManager.Instance._mapData.mapDatas[setPosiIndex].tower = true;
        copyObj.transform.position = LevelManager.Instance._mapData.mapDatas[setPosiIndex].posi + new Vector3(0.5f, 0.5f, 0);

        TowerBase towerMonoBehaviur = copyObj.GetComponent<TowerBase>();
        towerMonoBehaviur.Init();

        LevelManager.Instance.SetTower((uint)-_cost, towerMonoBehaviur);

        LevelManager.Instance.TowerTextUpdate();

        copyObj = null;
    }
}