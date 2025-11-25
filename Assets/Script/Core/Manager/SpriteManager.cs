using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class SpriteManager
{
    private bool init;
    private Dictionary<SpriteType , Dictionary<int , Sprite>> spriteTable = new Dictionary<SpriteType , Dictionary<int , Sprite>>();

    public async UniTask InitalizeAsync()
    {

        init = true;
    }
    


}

public enum SpriteType
{
    None, 
    Type,
    ElementType,
    AttackType,
}
