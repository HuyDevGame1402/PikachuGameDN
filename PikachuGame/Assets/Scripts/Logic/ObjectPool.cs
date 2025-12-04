using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [SerializeField] private Dictionary<int, List<GameObject>> vfxDic = new Dictionary<int, List<GameObject>>();

    protected override void Awake()
    {
        base.Awake();
    }


    public void AddVfxDic(int id, GameObject gameObject)
    {
        if(vfxDic.ContainsKey(id))
        {
            List<GameObject> listOb = vfxDic[id];
            if (!listOb.Contains(gameObject))
            {
                listOb.Add(gameObject);
            }
            vfxDic[id] = listOb;
        }
        else
        {
            List<GameObject> listOb = new List<GameObject>();
            listOb.Add(gameObject);
            vfxDic.Add(id, listOb);
        }
    }

    public GameObject GetVfx(int id)
    {
        if (vfxDic.ContainsKey(id))
        {
            List<GameObject> listOb = vfxDic[id];

            for(int i = 0; i < listOb.Count; i++)
            {
                if (listOb[i].activeSelf)
                {
                    continue;
                }
                else
                {
                    return listOb[i];
                }
            } 
            return null;

        }
        else
        {
            return null;
        }
    }

}
