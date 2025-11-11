using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class PlaceTower : MonoBehaviour
{
    [SerializeField] private TowerPlaceHold placeHold;
    // FIX : 추후 행성에 맞춰서 동적으로 변경
    [SerializeField] private int placeHoldCount;
    [SerializeField] private Button rotateLeft;
    [SerializeField] private Button rotateRight;
    [SerializeField] private int currentIndex;
    [SerializeField] private TowerManager towerManager;

    private RectTransform rect;
    private TowerPlaceHold[] placeHoldList;
    private float angle;
    private bool init = false;
    //FIX : 테스트용 프리셋 데이터
    private PresetData presetData = new PresetData(new List<int>());

    public async UniTask Init()
    {
        await DataTableManager.WaitForInitalizeAsync();

        if (init) return;

        placeHoldCount = presetData.towerIds.Count; 
        rect = GetComponent<RectTransform>();

        float radius = rect.rect.width / 2f;
        placeHoldList = new TowerPlaceHold[placeHoldCount];

        angle = 360f / placeHoldCount;

        //각도 나눠서 타워 설치 위치 조정 및
        //설치된 타워 구분용으로 사용됨
        for(int i = 0; i < placeHoldCount; i++)
        {
            var data = DataTableManager.Get<TowerTable>(DataTableIds.TowerTable).Get(presetData.towerIds[i]);

            // 타워 플레이스 홀더와 더불어서 TowerManager에 Preset 타워들 세팅
            TowerPlaceHold towerPlaceHolder = Instantiate(placeHold, transform);
            towerPlaceHolder.Init(i + 1, data.ID);

            towerManager.AddTower(data);

            placeHoldList[i] = towerPlaceHolder;

            RectTransform objRect = towerPlaceHolder.GetComponent<RectTransform>();

            float x = radius * Mathf.Cos(Mathf.Deg2Rad * angle * i);
            float y = radius * Mathf.Sin(Mathf.Deg2Rad * angle * i);
            objRect.anchoredPosition = new Vector2(y, x);
            objRect.transform.rotation = Quaternion.Euler(0f , 0f , -angle * i);
        }

        rotateLeft.onClick.AddListener(() => Rotate(-1));
        rotateRight.onClick.AddListener(() => Rotate(1));
        init = true;
    }
    

    private void Rotate(float dir)
    {
        gameObject.transform.eulerAngles += new Vector3(0f, 0f, dir * angle);
        currentIndex += (int)dir;
        if (currentIndex == -1)
            currentIndex = placeHoldCount - 1;
        else if(currentIndex >= placeHoldCount)
            currentIndex = currentIndex % placeHoldCount;   
    }

    public void Place(TowerTable.Data towerData)
    {
        for(int i = 0; i < placeHoldList.Length; i++)
        {
            if(placeHoldList[i].TowerId == towerData.ID)
            {
                placeHoldList[i].SetPlace();
                break;
            }
        }
    }

    public bool IsPlaced(int index)
    {
        return placeHoldList[index].GetPlaced();
    }
}
