using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "Levels")]
public class LevelCreators : ScriptableObject
{
    public List<GameObject> Levels;
}
