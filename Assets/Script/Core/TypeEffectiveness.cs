// �󼺰���

public class TypeEffectiveness
{
    private ElementType type;
    public ElementType Type => type;
    public int TypeToInt => (int)type;

    // ������ �Ӽ� ���̺����� ��������� �ҵ�
    public float[,] typeToDamageTable = new float[,]
    {
        //              Normal, Fire ,Ice, Steel,Light , Dark 
            /*Normal*/ { 1f ,  1f ,  1f ,   1f ,   1f  ,  1f  },
            /* Fire */ { 1f ,  1f , 0.5f,  1.5f ,  1f  ,  1f  },
            /* Ice  */ { 1f , 1.5f,  1f ,  0.5f ,  1f  ,  1f  },
            /* Steel*/ { 1f , 0.5f, 1.5f,   1f  ,  1f  ,  1f  },
            /* Light*/ { 1f ,  1f ,  1f ,   1f  ,  1f  ,  1.5f},
            /* Dark */ { 1f ,  1f ,  1f ,   1f  , 1.5f ,  1f  },
    };

    public void Init(ElementType type)
    {
        this.type = type;
    }

    public float GetDamagePercent(ElementType targetType)
    {
        return typeToDamageTable[TypeToInt, (int)targetType];
    }
    
    public float GetDefenderDamagePercent(ElementType attackerType)
    {
        return typeToDamageTable[(int)attackerType, TypeToInt];
    }

    public float GetReverseDamagePercent(ElementType attackerType, ElementType targetType)
    {
        float percent = typeToDamageTable[(int)attackerType, (int)targetType];

        return percent == 0f ? 0f : 1f / percent;
    }
}