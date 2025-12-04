using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : Singleton<SpriteManager>
{

    [SerializeField] private Dictionary<int, Sprite> spriteDic = new Dictionary<int, Sprite>();
    [SerializeField] private List<Sprite> spriteCells = new List<Sprite>();    
 
    protected override void Awake()
    {
        base.Awake();
        SetUpSpriteDic();
    }

    public Sprite GetSprite(int id)
    {
        if(spriteDic.ContainsKey(id)) return spriteDic[id];

        return null;
    }

    private void SetUpSpriteDic()
    {
        for(int i = 0; i < spriteCells.Count; i++)
        {
            spriteDic.Add(i, spriteCells[i]);
        }
    }
}
