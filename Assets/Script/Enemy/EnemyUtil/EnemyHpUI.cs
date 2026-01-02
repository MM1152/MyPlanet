using UnityEngine;

public class EnemyHpUI : MonoBehaviour
{
    [SerializeField] private SliderValue hpSlider;
    [SerializeField] private Vector3 baseLocalPos;
    private Enemy enemy;

    public void Init(Enemy enemy)
    {
        this.enemy = enemy;
        this.transform.SetParent(enemy.transform); 
        this.transform.localPosition = baseLocalPos;
        this.transform.localRotation = Quaternion.identity;
        this.transform.localScale = Vector3.one*2f;
    }
    private void Update()
    {
        hpSlider.UpdateSlider(enemy.currentHP, enemy.MaxHp);
    }
    public void Release()
    {
        this.transform.SetParent(null);  
        Managers.ObjectPoolManager.Despawn(PoolsId.EnemyHpUI, this.gameObject);
    }
}
