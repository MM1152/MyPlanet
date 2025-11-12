public interface IStatusEffect
{
    public void Apply(IDamageAble target);
    public void Update(float deltaTime);
    public void Remove();
    public void ResetStatusEffect();
    public IStatusEffect DeepCopy();
    public StatusEffectType EffectType { get; }
}