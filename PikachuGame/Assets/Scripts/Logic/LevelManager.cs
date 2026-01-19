using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private DataLocal dataLocal;
    [SerializeField] private List<Sprite> spriteLevelStar = new List<Sprite>();
    private void Start()
    {
        dataLocal = GameObject.Find("DataLoad").GetComponent<DataLocal>();
        SetUpLevelMap();
    }

    private void SetUpLevelMap()
    {
        if (dataLocal != null)
        {
            Dictionary<string, LevelScoreData> levelScores = new Dictionary<string, LevelScoreData>();
            levelScores = dataLocal.levelScores;

            List<Transform> listTransform = GetTransfromChild();
            bool isActiveNextLevel = false;
            foreach (var level in levelScores)
            {
                Transform levelTransfrom = GetLevelTransfrom(int.Parse(level.Key) - 1);
                levelTransfrom.GetChild(0).GetComponent<SpriteRenderer>().sprite = GetLevelStar(level.Value.stars);
                levelTransfrom.GetComponent<OnClickLevel>().isActive = true;
                listTransform.Remove(levelTransfrom);
            }

            for(int i = 0; i < listTransform.Count; i++)
            {
                listTransform[i].GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteLevelStar[0];
                if(isActiveNextLevel == false)
                {
                    listTransform[i].GetComponent<OnClickLevel>().isActive = true;
                    isActiveNextLevel = true;
                }
                else
                {
                    listTransform[i].GetComponent<OnClickLevel>().isActive = false;
                }
            }
        }
    }

    private List<Transform> GetTransfromChild()
    {
        List<Transform> trans = new List<Transform>();
        for(int i = 0;i < transform.childCount; i++)
        {
            trans.Add(transform.GetChild(i));
        }
        return trans;
    }

    private Sprite GetLevelStar(int star)
    {
        return spriteLevelStar[star];
    }

    private Transform GetLevelTransfrom(int level)
    {
        return transform.GetChild(level);
    }
}
