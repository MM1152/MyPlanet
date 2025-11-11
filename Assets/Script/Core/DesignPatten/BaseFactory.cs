public abstract class BaseFactory<T>
{
    public abstract T CreateInstance(int id);
}