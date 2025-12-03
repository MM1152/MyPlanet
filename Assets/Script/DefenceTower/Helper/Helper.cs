using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using TMPro;
public class Helper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userNameText;

    private UserData userData;
    private Tower tower;
    private HelperManager manager;

    public float duration = 5f;
    private float speed;
    private Vector3 dir;
    public Vector3 endPos;
    public Vector3 startPos;
    public void Init(UserData userData, HelperManager manager)
    {
        this.manager = manager;
        var data = manager.TowerManager.TowerFactory.CreateInstance(2003);
        this.userData = userData;
        tower = data;
        tower.Init(this.gameObject, manager.TowerManager, DataTableManager.TowerTable.Get(2003));
        tower.PlaceTower(true);
        tower.BonusAttackRange += 10;
        userNameText.text = userData.nickName;

        gameObject.SetActive(false);
    }

    public void MoveHelper(Vector3 startPos , Vector3 endPos)
    {
        this.endPos = endPos;
        this.startPos = startPos;   
        gameObject.SetActive(true);
        transform.position = startPos;

        speed = Vector3.Distance(startPos, endPos) / duration;
        dir = (endPos - startPos).normalized;
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
                return;
            }

            transform.position += dir * speed * Time.deltaTime;
        }

        tower.Update(Time.deltaTime);
    }
}
