using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{
    [SerializeField] private Dictionary<int, GameObject> vfxDic = new Dictionary<int, GameObject>();
    [SerializeField] private List<GameObject> vfxList = new List<GameObject>();
    [SerializeField] private int currentIdVfx = 0;

    protected override void Awake()
    {
        base.Awake();

        for(int i = 0; i < vfxList.Count; i++)
        {
            vfxDic.Add(i, vfxList[i]);
        }
    }

    public GameObject GetVFX(int id)
    {
        if(!vfxDic.ContainsKey(id)) return null;

        GameObject vfx = ObjectPool.Instance.GetVfx(id);
        if (vfx != null)
        {
            return vfx;
        }

        return vfxDic[id];
    }
    public GameObject GetVFX()
    {
        if (currentIdVfx < 0 || currentIdVfx >= vfxDic.Count) return null;

        return vfxDic[currentIdVfx];
    }

    public int GetCurrentIdVfx()
    {
        return currentIdVfx;
    }
}
