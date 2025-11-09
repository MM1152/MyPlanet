// 상성관련

public class TypeEffectiveness
{
    private ElementType type;
    public ElementType Type => type;
    public int TypeToInt => (int)type;

    // 원래는 속성 테이블에서 관리해줘야 할듯
    public float[,] typeToDamageTable = new float[,]
    {
        //         Fire , Steel , Water , Light , Dark
        /* Fire */ { 1f ,  1.5f ,  0.5f ,   1f  ,  1f},
        /* Steel*/ { 0.5f , 1f ,  1.5f  ,   1f  ,  1f},
        /* Water*/ { 1.5f , 0.5f ,  1f  ,   1f  ,  1f},
        /* Light*/ { 1f ,  1f ,  1f  ,   1f  ,  1.5f},
        /* Dark */ { 1f ,  1f ,  1f  ,   0.5f ,  1f},
    };

    public void Init(ElementType type)
    {
        this.type = type;
    }
    
    public float GetDamagePercent(ElementType targetType)
    {
        return typeToDamageTable[TypeToInt, (int)targetType];
    }
}