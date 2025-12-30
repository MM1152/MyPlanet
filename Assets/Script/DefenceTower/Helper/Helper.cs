using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class Helper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userNameText;

    private AsyncPlayerData.Data userData;
    private List<Tower> towers = new List<Tower>();
    private HelperManager manager;
    private HelperViewer helperViewer;

    public float duration = 5f;
    private float speed;
    private Vector3 dir;
    public Vector3 endPos;
    public Vector3 startPos;
    public void Init(AsyncPlayerData.Data userData, HelperManager manager , HelperViewer helperViewer)
    {
        this.manager = manager;
        this.helperViewer = helperViewer;

        for(int i = 0; i< userData.playerTowerIds.Count; i++)
        {
            var towerId = userData.playerTowerIds[i];
            var towerFullDamage = userData.playerTowerFullDamages[i];

            if (towerId == -1) continue;
            var tower = manager.TowerManager.TowerFactory.CreateInstance(towerId);
            tower.Init(towerFullDamage, this.gameObject ,manager.TowerManager, DataTableManager.TowerTable.Get(towerId));
            tower.LevelUp(DataTableManager.LevelUpTable.Get(towerId, 1));
            tower.BonusAttackRange += 10;
           

            towers.Add(tower);
        }

        userNameText.text = userData.playerNickName;
        gameObject.SetActive(false);

        Instantiate(DataTableManager.PlanetTable.Model, transform);

        this.userData = userData;
        
        //tower.BonusAttackRange += 10;
    }

    public void MoveHelper(Vector3 startPos , Vector3 endPos)
    {
        for(int i = 0; i < towers.Count; i++)
        {
            towers[i].PlaceTower(true);
        }

        this.endPos = endPos;
        this.startPos = startPos;   
        gameObject.SetActive(true);
        transform.position = startPos;

        speed = Vector3.Distance(startPos, endPos) / duration;
        dir = (endPos - startPos).normalized;

        helperViewer.RotationActive(true);
    }

    private void Update()
    {
        if(speed != 0 && dir != Vector3.zero)
        {
            if (Vector3.Distance(transform.position , endPos) <= 0.1f)
            {
                speed = 0;
                dir = Vector3.zero;
                gameObject.SetActive(false);
                helperViewer.RotationActive(false);
                return;
            }

            transform.position += dir * speed * Time.deltaTime;
        }


        foreach(var tower in towers)
        {
            tower.Update(Time.deltaTime);
        }
    }
}
