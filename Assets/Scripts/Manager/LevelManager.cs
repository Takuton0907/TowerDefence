using Cinemachine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    private enum LEVEL_STATE
    {
        Init,
        Play,
        CharaSelect,
        Clear,
        GameOver,
        Result,
        Fin,
    }

    public enum POSE
    {
        Pose,
        InPlay,
    }

    //ゲームのState
    LEVEL_STATE levelState = LEVEL_STATE.Init;

    //現在pose中かのState
    POSE _poseState = POSE.InPlay;

    //マップのデータ
    public MapDate _mapDate;

    //そのステージのEnemyManager
    public EnemyManager _enemyManager;

    //そのステージのTowerManager
    public TowerManager _towerManager;

    //守るべきライフ
    public int m_life { private set; get; } = 5;
    int _maxLife;

    [SerializeField]
    Text _lifeText;

    //一番初めの敵が出るまでの時間
    [SerializeField]
    float _starInterval = 1;

    [Header("CostParam")]

    //持てるコストの最大値
    [SerializeField]
    uint _maxCost = 100;

    //現在使用できるコスト
    [SerializeField]
    public uint _cost { set; get; } = 50; 

    //costの回復するまでの時間
    [SerializeField]
    float _interval = 5;

    //スピードを変化させる
    public float _totalRate = 1;

    //キャラを選択しているときのスピード
    [SerializeField]
    private float _selectRate = 0.1f;
    private float _time;

    //一定時間で回復するCostの量
    [SerializeField]
    uint _heelCost = 5;

    //コストを表示するスライダー
    [SerializeField]
    Slider _costSlider;
    private Text _costText;

    //コストが回復することを示すスライダー
    [SerializeField]
    Slider _heelCostSlider;

    //タワーを置ける最大数
    [SerializeField] 
    int _maxTowerCount = 5;

    //現在のタワー数を表示するテキスト
    [SerializeField] 
    Text _towerCostText;

    [Header("Button")]

    //キャラを退却させるボタン
    [SerializeField]
    Button _removeButton;

    //背面に見えないように設置するボタン
    [SerializeField]
    Button _backgroundButton;

    //スピードを変えるボタン群
    [SerializeField]
    Button _playButton;
    [SerializeField]
    Button _upSpeedButton;

    //Playボタンの変更する画像
    [SerializeField]
    Sprite _playSprite;
    [SerializeField]
    Sprite _stopSprite;

    //ボタンの変更する画像
    [SerializeField]
    Sprite _upSprite;
    [SerializeField]
    Sprite _nomalSprite;

    [Header("Camera")]
    //ゲームシーン全体を見ているカメラ
    [SerializeField]
    CinemachineVirtualCamera _maneVcam;
    //各キャラにフォーカスするためのカメラ
    [SerializeField]
    CinemachineVirtualCamera _charaVcam;

    [Header("Clear")]
    [SerializeField]
    CanvasGroup _clearCanvasGroup;
    [SerializeField]
    GameObject _clearTextObj;

    [Header("GameOver")]
    [SerializeField]
    CanvasGroup _gameOverCanvasGroup;
    [SerializeField]
    GameObject _gameOverTextObj;

    [Header("ResultsUI")]

    [SerializeField]
    GameObject[] _results;
    [SerializeField]
    Animation _ClearTextAnimation;

    [SerializeField]
    Animator _gameOverAnimator;


    private new void Awake()
    {
        if (GameManager.Instance.nextGameStagePath != string.Empty) GameSetting(GameManager.Instance.nextGameStagePath);

        _mapDate.MapDateReset();

        _costText = _costSlider.GetComponentInChildren<Text>();

        UseCost(0);

        _heelCostSlider.maxValue = _interval;

        TowerTextUpdate();

        _clearCanvasGroup.blocksRaycasts = true;

        _maxLife = m_life;
    }

    private void Update()
    {
        switch (levelState)
        {
            case LEVEL_STATE.Init:
                if (_time >= _starInterval)
                {
                    _clearCanvasGroup.blocksRaycasts = false;
                    _enemyManager.EnemyManagerInit();
                    _time = 0;
                    levelState = LEVEL_STATE.Play;
                }
                _time += Time.deltaTime;
                break;
            case LEVEL_STATE.Play:
                switch (_poseState)
                {
                    case POSE.Pose:
                        break;
                    case POSE.InPlay:
                        //コストの回復
                        if (_time > _interval)
                        {
                            UseCost(_heelCost);
                            _time = 0;
                        }
                        _time += Time.deltaTime * _totalRate;
                        _heelCostSlider.value = _time;
                        if (m_life <= 0)
                        {
                            GameOver();
                        }
                        foreach (var item in _enemyManager.instanceEnemys)
                        {
                            item.EnemyUpdate();
                        }

                        _enemyManager.EnemySpawnUpdate(_totalRate);

                        _towerManager.TowerUpdate(_totalRate);
                        break;
                }
                break;
            case LEVEL_STATE.CharaSelect:
                switch (_poseState)
                {
                    case POSE.Pose:
                        break;
                    case POSE.InPlay:
                        //コストの回復
                        if (_time > _interval)
                        {
                            UseCost(_heelCost);
                            _time = 0;
                        }
                        _time += Time.deltaTime * _selectRate;
                        _heelCostSlider.value = _time;
                        if (m_life <= 0)
                        {
                            GameOver();
                        }
                        foreach (var item in _enemyManager.instanceEnemys)
                        {
                            item.EnemyUpdate();
                        }

                        _enemyManager.EnemySpawnUpdate(_selectRate);

                        _towerManager.TowerUpdate(_selectRate);
                        break;
                }
                break;
            case LEVEL_STATE.Clear:
                if (!_ClearTextAnimation.isPlaying)
                {
                    if (m_life == _maxLife)
                    {
                        GameManager.Instance.SetClearValue(GameManager.Instance.nextGameStagePath, 3);
                        _results[0].SetActive(true);
                    }
                    else if (m_life >= _maxLife / 2)
                    {
                        GameManager.Instance.SetClearValue(GameManager.Instance.nextGameStagePath, 2);
                        _results[1].SetActive(true);
                    }
                    else
                    {
                        GameManager.Instance.SetClearValue(GameManager.Instance.nextGameStagePath, 1);
                        _results[2].SetActive(true);
                    }

                    levelState = LEVEL_STATE.Result;
                }
                break;
            case LEVEL_STATE.GameOver:
                if (_gameOverAnimator.IsInTransition(0))
                {
                    FadeManager.Instance.LoadScene("SELECT", 1);
                    Debug.Log("animFin");
                    levelState = LEVEL_STATE.Fin;
                }
                    break;
            case LEVEL_STATE.Result:

                if (Input.GetMouseButtonDown(0))
                {
                    FadeManager.Instance.LoadScene("SELECT", 1);
                    Debug.Log("ステージクリア");
                    levelState = LEVEL_STATE.Fin;
                }
                break;
            default:
                break;
        }
    }

    //Pose中にするための関数
    public void OnClickStateChange()
    {
        switch (_poseState)
        {
            case POSE.Pose:
                _playButton.image.sprite = _stopSprite;
                _poseState = POSE.InPlay;
                break;
            case POSE.InPlay:
                _playButton.image.sprite = _playSprite;
                _poseState = POSE.Pose;
                break;
        }
    }
    //全体のスピード変更
    public void OnclickSpeedChange()
    {
        if (_enemyManager.instanceEnemys.Count > 0)
        {
            _enemyManager.EnemySpeedChange(false);
            if (_upSpeedButton.image.sprite == _upSprite)
            {
                _upSpeedButton.image.sprite = _nomalSprite;
            }
            else
            {
                _upSpeedButton.image.sprite = _upSprite;
            }
            _totalRate = _enemyManager.instanceEnemys[0]._speedRate;
        }
        else
        {
            if (_totalRate == 1)
            {
                _totalRate = _enemyManager._speedRateUP;
            }
            else
            {
                _totalRate = 1;
            }
        }
    }
    //タワーを持った時にスピ―ドを変化させる
    public void DragSpeedChange()
    {
        if (_enemyManager.instanceEnemys.Count > 0)
        {
            _enemyManager.EnemySpeedChange(true);
            _totalRate = _enemyManager.instanceEnemys[0]._speedRate;
        }
        else
        {
            if (_totalRate == 1)
            {
                _totalRate = _enemyManager._speedRateDown;
            }
            else
            {
                _totalRate = 1;
            }
        }
    }
    public void DragSpeedChange(float value)
    {
        _enemyManager.EnemySpeedChange(value);
        _totalRate = _enemyManager.instanceEnemys[0]._speedRate;
    }
    //コストの使用
    public void UseCost()
    {
        _cost += _heelCost;
        _costText.text = $"{_cost} / {_maxCost}";
        _costSlider.value = (float)_cost / _maxCost;
    }
    void UseCost(uint value)
    {
        _cost += value;
        //if (_cost > _maxCost)
        //{
        //    _cost = _maxCost;
        //}
        _costText.text = $"{_cost} / {_maxCost}";
        _costSlider.value = (float)_cost / _maxCost;
    }
    public void SetTower(uint value, TowerMonoBehaviur tower)
    {
        _towerManager.SetTower(tower);
        UseCost(value);
    }
    //リトライボタンを押したと時の処理
    public void ClickToRePlay()
    {
        levelState = LEVEL_STATE.Init;
    }
    //ゲームオーバー後の処理
    private void GameOver()
    {
        Time.timeScale = 1;

        levelState = LEVEL_STATE.GameOver;
        
        _gameOverTextObj.SetActive(true);

        _gameOverAnimator = _gameOverTextObj.GetComponent<Animator>();

        _gameOverCanvasGroup.alpha = 1;

        _gameOverCanvasGroup.blocksRaycasts = true;

        Debug.Log("GameOver");
    }
    //ステージクリア後の処理
    public void StageClear()
    {
        Time.timeScale = 1;

        levelState = LEVEL_STATE.Clear;

        _clearTextObj.SetActive(true);

        _ClearTextAnimation = _clearTextObj.GetComponent<Animation>();

        _ClearTextAnimation.Play();

        _clearCanvasGroup.alpha = 1;

        _clearCanvasGroup.blocksRaycasts = true;

        Debug.Log("GameClear");
    }
    //敵がタワーにたどり着いたときに呼び出す
    public void Damage()
    {
        m_life--;
        LifeUpdate();
    }
    // 指定したTiteStatasのオブジェクト取得
    public List<int> GetIndexs(TILE tileStatas)
    {
        List<int> vs = new List<int>();
        for (int i = 0; i < _mapDate.mapDates.Count; i++)
        {
            if (_mapDate.mapDates[i].tileBaseNum == (int)tileStatas)
            {
                vs.Add(i);
            }
        }
        return vs;
    }
    public List<int> GetIndexs(TILE tileStatas, TILE tileStatas2)
    {
        List<int> vs = new List<int>();
        for (int i = 0; i < _mapDate.mapDates.Count; i++)
        {
            int tileBasenum = _mapDate.mapDates[i].tileBaseNum;
            if (tileBasenum == (int)tileStatas || tileBasenum == (int)tileStatas2)
            {
                vs.Add(i);
            }
        }
        return vs;
    }
    public List<int> GetIndexs(TILE tileStatas, TILE tileStatas2, TILE tileStatas3)
    {
        List<int> vs = new List<int>();
        for (int i = 0; i < _mapDate.mapDates.Count; i++)
        {
            int tileBasenum = _mapDate.mapDates[i].tileBaseNum;
            if (tileBasenum == (int)tileStatas || tileBasenum == (int)tileStatas2 || tileBasenum == (int)tileStatas3)
            {
                vs.Add(i);
            }
        }
        return vs;
    }
    //敵が進む道を検索
    public List<MAP_DATE> Sarch()
    {
        TileOpen();

        int dateIndex = _mapDate.GetStartIndex();
        var openList = new List<int>();

        return Aster(dateIndex, openList);
    }
    //A*の実装
    private List<MAP_DATE> Aster(int index, List<int> list)
    {
        for (int i = 0; i < 1000; i++)
        {
            if (_mapDate.mapDates[index].tileBaseNum == (int)TILE.GOAL)
            {
                Debug.Log("探索終了");
                break;
            }

            //次に動けるTileの取得
            var mapIndexs = _mapDate.GetNextTilesIndex(index);

            //通れる道以外の削除
            foreach (var item in mapIndexs)
            {
                if (_mapDate.mapDates[item].states == TILE_STATAS.Opne)
                {
                    if (_mapDate.mapDates[item].tileBaseNum == (int)TILE.LOAD || _mapDate.mapDates[item].tileBaseNum == (int)TILE.GOAL || _mapDate.mapDates[item].tileBaseNum == (int)TILE.START)
                    {
                        _mapDate.mapDates[item].parentDate = _mapDate.mapDates[index];
                        list.Add(item);
                    }
                }
            }
            //距離の計算
            foreach (var item in mapIndexs)
            {
                _mapDate.mapDates[item].C = _mapDate.GetCost(index, _mapDate.GetGoalIndex());
                _mapDate.mapDates[item].H = _mapDate.GetCost(_mapDate.GetStartIndex(), index);
                _mapDate.mapDates[item].S = _mapDate.mapDates[item].C + _mapDate.mapDates[item].H;
                //Debug.Log("C = " + _mapDate.mapDates[item].C + " H = " + _mapDate.mapDates[item].H);
            }
            //スタートした場所は検索済みなのでCloaseに
            _mapDate.mapDates[index].states = TILE_STATAS.Close;

            int minValue = int.MaxValue;

            foreach (var item in list)
            {
                if (_mapDate.mapDates[item].states == TILE_STATAS.Close) continue;
                if (_mapDate.mapDates[item].S < minValue)
                {
                    minValue = _mapDate.mapDates[item].S;
                    index = item;
                }
            }
        }

        var vs = new List<MAP_DATE>();
        //_mapDate.mapDates[index].GetParh(vs);
        MAP_DATE date = _mapDate.mapDates[index];
        while (date.tileBaseNum != (int)TILE.START)
        {
            vs.Add(date);
            date = date.parentDate;
        }

        return vs;
    }
    //すべてのタイルのSATASをOpenにする
    private void TileOpen()
    {
        foreach (var item in _mapDate.mapDates)
        {
            item.states = TILE_STATAS.Opne;
        }
    }
    //タワーの削除
    public void OnClickRemoveChara(TowerMonoBehaviur tower)
    {
        List<int> indexs = GetIndexs(TILE.SET_TOWER);
        foreach (var item in indexs)
        {
            Vector3 posi = tower.transform.position;
            posi -= new Vector3(0.5f, 0.5f, 0);
            //Debug.Log($"_mapDate.mapDates[item].posi = {_mapDate.mapDates[item].posi} towerPosi = {posi}");
            if (_mapDate.mapDates[item].posi == posi)
            {
                _mapDate.mapDates[item].tower = false;
                break;
            }
        }
        TowerTextUpdate();
        _towerManager.instanceTowers.Remove(tower);
        Destroy(tower.gameObject);
    }
    //タワーが置けるかの判定
    public bool HasTowerCount()
    {
        return _towerManager.instanceTowers.Count <= _maxTowerCount ? true: false ;
    }
    //タワーのテキスト更新
    public void TowerTextUpdate()
    {
        _towerCostText.text = $"残り配置可能数 : {_maxTowerCount - _towerManager.instanceTowers.Count}";
    }
    //ライフの更新
    public void LifeUpdate()
    {
        _lifeText.text = $"{m_life} / {_maxLife}";
    }
    //キャラクターをクリックしたときの動作
    public void CahraClick(GameObject target)
    {
        levelState = LEVEL_STATE.CharaSelect;

        _enemyManager.EnemySpeedChange(_selectRate);

        _charaVcam.Priority = _maneVcam.Priority + 1;
        _charaVcam.Follow = target.transform;
        _charaVcam.LookAt = target.transform;

        _backgroundButton.gameObject.SetActive(true);
    }
    //BackGroundボタンを押したときの動作
    public void OnClickBackGroundButton()
    {
        levelState = LEVEL_STATE.Play;
        _enemyManager.EnemySpeedChange(_totalRate);

        _charaVcam.Priority = _maneVcam.Priority - 1;
        _backgroundButton.gameObject.SetActive(false);
        TowerMonoBehaviur tower = _charaVcam.m_Follow.GetComponent<TowerMonoBehaviur>();
        tower.CloseCanvas();
    }
    //元のカメラへ戻す
    public void CameraReset()
    {
        levelState = LEVEL_STATE.Play;
        _enemyManager.EnemySpeedChange(_totalRate);
        _charaVcam.Priority = _maneVcam.Priority - 1;
        _backgroundButton.gameObject.SetActive(false);
    }
    //ゲームスタート時に選択されたステージのアセットを取ってくる
    private void GameSetting(string datePath)
    {
        Debug.Log(datePath);

        Object[] assets = Resources.LoadAll(datePath);

        foreach (var item in assets)
        {
            if (item as TextAsset != null)
            {
                _enemyManager._stageText = item as TextAsset;
            }
            else if (item as MapDate != null)
            {
                _mapDate = item as MapDate;
            }
            else if (item as GameObject != null)
            {
                GameObject map = Instantiate(item as GameObject);
                map.transform.position = new Vector3();
            }
        }
    }
}