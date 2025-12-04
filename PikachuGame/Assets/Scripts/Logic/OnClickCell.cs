using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickCell : MonoBehaviour
{
    void OnMouseDown()
    {
        if (PikachuGameLogic.Instance.GetProcessLogic()) return;
        Cell cellLocal = GetComponent<Cell>();
        cellLocal.ShowBackgroundTouched();
        PikachuGameLogic.Instance.SetSelectedCell(cellLocal);
    }
}
