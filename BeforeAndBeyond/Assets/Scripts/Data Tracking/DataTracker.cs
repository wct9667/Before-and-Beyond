using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTracker : MonoBehaviour
{
    public static DataTracker Instance;

    private CharacterType currentCharacter;
    private float hackerTime;
    private float knightTime;
    private int hackerSwapCount = 0;
    private int knightSwapCount = 0;

    public float HackerTime => hackerTime;
    public float KnightTime => knightTime;
    public int HackerSwapCount => hackerSwapCount;
    public int KnightSwapCount => knightSwapCount;

    private Dictionary<CharacterType, int[]> abilityUsageCounts = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeUsageCounts();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentCharacter)
        {
            case CharacterType.Hacker:
                hackerTime += Time.deltaTime;
                break;
            case CharacterType.Knight:
                knightTime += Time.deltaTime;
                break;
        }
    }

    [System.Serializable]
    public class TrackingSaveData
    {
        public string hackerTime;
        public string knightTime;
        public int hackerSwapCount;
        public int knightSwapCount;
        public string hackerAverageTime;
        public string knightAverageTime;
    }

    public void SaveToFile()
    {
        float hackerAverage = hackerSwapCount > 0 ? hackerTime / hackerSwapCount : 0f;
        float knightAverage = knightSwapCount > 0 ? knightTime / knightSwapCount : 0f;

        string hackerTimeF1 = hackerTime.ToString("F1") + "s";
        string knightTimeF1 = knightTime.ToString("F1") + "s";
        string hackerAverageF1 = hackerAverage.ToString("F1") + "s";
        string knightAverageF1 = knightAverage.ToString("F1") + "s";

        TrackingSaveData data = new TrackingSaveData
        {
            hackerTime = hackerTimeF1,
            knightTime = knightTimeF1,
            hackerSwapCount = hackerSwapCount,
            knightSwapCount = knightSwapCount,
            hackerAverageTime = hackerAverageF1,
            knightAverageTime = knightAverageF1,
        };

        string mainJson = JsonUtility.ToJson(data, true);

        string hackerAbilitiesJson = "\"hackerAbilityUsage\": {\n";
        string knightAbilitiesJson = "\"knightAbilityUsage\": {\n";

        for (int i = 0; i < 3; i++)
        {
            string abilityKey = $"\"Ability {i + 1}\"";
            hackerAbilitiesJson += $" {abilityKey}: {abilityUsageCounts[CharacterType.Hacker][i]}";
            knightAbilitiesJson += $" {abilityKey}: {abilityUsageCounts[CharacterType.Knight][i]}";

            if (i < 2)
            {
                hackerAbilitiesJson += ",\n";
                knightAbilitiesJson += ",\n";
            }
        }

        hackerAbilitiesJson += "\n}";
        knightAbilitiesJson += "\n}";

        string fullJson = mainJson.TrimEnd('}') + ",\n" + hackerAbilitiesJson + ",\n" + knightAbilitiesJson + "\n}";
        string path = Application.persistentDataPath + "/trackingData.json";
        System.IO.File.WriteAllText(path, fullJson);
        Debug.Log($"Tracking data saved to {path}");
    }


    public void SetActiveCharacter(CharacterType newCharacter)
    {
        if (newCharacter != currentCharacter) 
        {
            if (newCharacter == CharacterType.Hacker)
                hackerSwapCount++;
            else if (newCharacter == CharacterType.Knight)
                knightSwapCount++;
        }

        currentCharacter = newCharacter;
    }

    private void InitializeUsageCounts()
    {
        foreach (CharacterType type in System.Enum.GetValues(typeof(CharacterType)))
        {
            abilityUsageCounts[type] = new int[3];
        }
    }

    public void RegisterAbilityUse(CharacterType type, int abilityIndex)
    {
        if (abilityIndex < 0 || abilityIndex >= 3) return;

        if (!abilityUsageCounts.ContainsKey(type))
            abilityUsageCounts[type] = new int[3];

        abilityUsageCounts[type][abilityIndex]++;
    }


    public void PrintData()
    {
        Debug.Log($"Hacker Time: {hackerTime:F1} seconds | Swaps to Hacker: {hackerSwapCount}");
        Debug.Log($"Knight Time: {knightTime:F1} seconds | Swaps to Knight: {knightSwapCount}");
        for (int i = 0; i < 3; i++)
        {
            Debug.Log($"Hacker ability {i + 1} used: {abilityUsageCounts[CharacterType.Hacker][i]} times");
            Debug.Log($"Knight ability {i + 1} used: {abilityUsageCounts[CharacterType.Knight][i]} times");
        }
    }
}
