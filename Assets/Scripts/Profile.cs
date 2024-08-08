using UnityEngine;

public class Profile : MonoBehaviour
{
    [System.Serializable]
    private class PlayerData
    {
        public int bestResult = 0;
    }

    private static PlayerData playerData;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CheckData()
    {
        SetPlayerData();
    }

    private static void SetPlayerData()
    {
        if (playerData != null) return;

        playerData = GetData<PlayerData>("PlayerData");
    }

    private static T GetData<T>(string key) where T: new()
    {
        if(PlayerPrefs.HasKey(key))
        {
            return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
        }

        var data = new T();
        PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
        return data;
    }

    public static void Save(bool player = true)
    {
        if (player)
        {
            PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(playerData));
        }
    }

    public static int BestResult
    {
        get => playerData.bestResult;
        set
        {
            if(value > playerData.bestResult)
            {
                playerData.bestResult = value;
            }

            Save(player: true);
        }
    }


}
