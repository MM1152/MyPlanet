using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
public abstract class Consumable
{
    protected TowerManager towerManager;
    protected BasePlanet planet;
    protected ConsumalbeTable.Data consumData;
    public ConsumalbeTable.Data ConsumData => consumData;


    private CancellationTokenSource ctr;
    protected float duration;
    protected UnityEngine.GameObject uiTab;
    public void Init(TowerManager towerManager , BasePlanet planet , ConsumalbeTable.Data data)
    {
        this.towerManager = towerManager;
        this.planet = planet;
        consumData = data;

        if(ctr != null && !ctr.IsCancellationRequested)
        {
            Release();
        }

        ctr = new CancellationTokenSource();
        this.duration = consumData.duration;
        UseItemAsync(consumData.duration, ctr).Forget();
    }

    public void SetUI(UnityEngine.GameObject uiTab)
    {
        this.uiTab = uiTab;
    }

    public void Release()
    {
        if(ctr != null && !ctr.IsCancellationRequested)
        {
            ctr.Cancel();
            ctr.Dispose();
            ctr = null;
        }
    }

    public void Update(float deltaTIme)
    {
        duration -= deltaTIme;
    }

    public float GetDuration()
    {
        return duration;
    }

    protected abstract UniTaskVoid UseItemAsync(float duration , CancellationTokenSource ctr);
    protected abstract void ResetItem();
}