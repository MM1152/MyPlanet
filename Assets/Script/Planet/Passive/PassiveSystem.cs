public class PassiveSystem
{
    private IPassive passive;
    
    public void Init(IPassive passive)
    {
        this.passive = passive;
        this.passive.ApplyPassive();
    }

}