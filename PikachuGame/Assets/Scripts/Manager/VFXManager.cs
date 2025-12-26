using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{
    // vfx eat cell
    [SerializeField] private Dictionary<int, GameObject> vfxDic = new Dictionary<int, GameObject>();
    [SerializeField] private List<GameObject> vfxList = new List<GameObject>();
    [SerializeField] private int currentIdVfx = 0;
    // vfx logic text
    [SerializeField] private Dictionary<int, GameObject> vfxDicText = new Dictionary<int, GameObject>();
    [SerializeField] private List<GameObject> vfxListText = new List<GameObject>();
    [SerializeField] private int currentIdVfxText = 0;
    [SerializeField] private Transform transformSpawnVfxText;

    protected override void Awake()
    {
        base.Awake();

        SetUpVfxInDic(vfxList, vfxDic);
        SetUpVfxInDic(vfxListText, vfxDicText); 
    }

    private void SetUpVfxInDic(List<GameObject> vfxList, Dictionary<int, GameObject> vfxDic)
    {
        for (int i = 0; i < vfxList.Count; i++)
        {
            vfxDic.Add(i, vfxList[i]);
        }
    }

    public GameObject GetVFX(int id, VfxEnum vfxEnum)
    {
        Dictionary<int, GameObject> vfxDic = GetDicVfx(vfxEnum);

        if(!vfxDic.ContainsKey(id)) return null;

        GameObject vfx = ObjectPool.Instance.GetVfx(id, ObjectPool.Instance.GetDic(vfxEnum));
        if (vfx != null)
        {
            return vfx;
        }

        return vfxDic[id];
    }
    public GameObject GetVFX(VfxEnum vfxEnum)
    {
        if(vfxEnum == VfxEnum.vfxCell)
        {
            if (currentIdVfx < 0 || currentIdVfx >= vfxDic.Count) return null;

            return vfxDic[currentIdVfx];
        }
        else
        {
            if (currentIdVfx < 0 || currentIdVfx >= vfxDicText.Count) return null;

            return vfxDicText[currentIdVfx];
        }
    }

    public int GetCurrentIdVfx()
    {
        return currentIdVfx;
    }
    public Transform GetTransformSpawnVfxText()
    {
        return transformSpawnVfxText;
    }
    private Dictionary<int, GameObject> GetDicVfx(VfxEnum vfxEnum)
    {
        if(vfxEnum == VfxEnum.vfxCell)
        {
            return vfxDic;
        }
        else
        {
            return vfxDicText;
        }
    }
}
