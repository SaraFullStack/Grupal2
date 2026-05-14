using UnityEngine;
using System.IO;

public class SaveExample : MonoBehaviour
{

    public GameDataSO gameData;
    private string savePath;

    private void Awake()
    {
        
        savePath = Application.persistentDataPath + "/savegame.json";
        
    }

    private void Start()
    {
        gameData.ResetData();
        this.LoadGame();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Guardamos partida");
        this.SaveGame();

    }

    public void SaveGame()
    {
        // Convierte el SO a JSON
        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(savePath, json);
        Debug.Log("Partida Guardada en: " + savePath);
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            // Sobrescribe los datos del SO actual
            JsonUtility.FromJsonOverwrite(json, gameData);
            Debug.Log("Partida Cargada");
        }
        else
        {
            Debug.LogWarning("No se encontró archivo de guardado");
        }

        Debug.Log("TORNILLOS "+ gameData.screws);
    }
}
