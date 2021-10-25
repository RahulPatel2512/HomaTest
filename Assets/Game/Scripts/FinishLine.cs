using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] List<WinCube> winCubes;

    //SetPlayer in all Win Clubes
    public void SetPlayer(Transform C_Player){
        for (int i = 0; i < winCubes.Count; i++)
        {
            winCubes[i].Player=C_Player;
        }
    }
}
