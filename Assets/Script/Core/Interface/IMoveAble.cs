public interface IMoveAble
{
    public bool IsStun { get; set; }
    public float BaseSpeed { get; }
    public float CurrentSpeed { get; set; }
}