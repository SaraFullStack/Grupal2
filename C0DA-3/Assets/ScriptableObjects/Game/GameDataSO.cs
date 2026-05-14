using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/Game/Game Data")]
public class GameDataSO : ScriptableObject
{
    [Header("Coleccionables")]
    public int screws;
    public int cores;

    [Header("Desbloqueados")]
    public List<int> obteinedCores = new List<int>();
    public List<int> watchedTutoriasl = new List<int>();

    // Método para limpiar datos si es necesario
    public void ResetData()
    {
        screws = 0;
        cores = 0;
        obteinedCores.Clear();
        watchedTutoriasl.Clear();
    }
}