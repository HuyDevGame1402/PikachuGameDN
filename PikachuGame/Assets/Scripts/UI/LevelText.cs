using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    

    public void SetLevelText(int level)
    {
        levelText.text = "Level " + level.ToString();
    }
}
