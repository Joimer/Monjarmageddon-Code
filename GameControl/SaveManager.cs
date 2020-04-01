using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour {

    public static SaveManager instance;
    private static string savePath = Application.persistentDataPath + "/save.dat";
    private static string configPath = Application.persistentDataPath + "/config.dat";

    public static SaveManager GetInstance() {
        if (instance == null) {
            instance = new GameObject().AddComponent<SaveManager>();
            instance.name = "Save Manager";
        }
        return instance;
    }

    private void Awake() {
        DontDestroyOnLoad(instance);
        if (instance == null) {
            instance = this;
        }
    }

    public static void SaveGameData(SaveState saveState) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        bf.Serialize(file, saveState);
        file.Close();
    }

    public static SaveState LoadGameData() {
        SaveState saveState = new SaveState();
        if (File.Exists(savePath)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            try {
                saveState = (SaveState)bf.Deserialize(file);
            } catch { }
            file.Close();
        }

        return saveState;
    }

    public static void SaveGameConfig(GameConfig gameConfig) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(configPath);
        bf.Serialize(file, gameConfig);
        file.Close();
    }

    public static GameConfig LoadGameConfig() {
        GameConfig config = new GameConfig();
        if (File.Exists(configPath)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(configPath, FileMode.Open);
            try {
                config = (GameConfig) bf.Deserialize(file);
            } catch { }
            file.Close();
        }

        return config;
    }
}
