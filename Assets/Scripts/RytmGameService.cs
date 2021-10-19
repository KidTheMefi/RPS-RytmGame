using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RytmGameService : MonoBehaviour
{
    [SerializeField] private GameObject railPrefab;
    public List<SpawnProperty> spawnProperties = new List<SpawnProperty>();

    [SerializeField, Range(0, 13)] private float bottomBorder;
    [SerializeField, Range(1, 13)] private float areaRange;

    Rail railScript;
    //IconFabric iconFabric;
    private int wave = 0;

    void Start()
    {
        railPrefab = Instantiate(railPrefab, Vector3.up * -3, Quaternion.identity);
        railScript = railPrefab.GetComponent<Rail>();
        railScript.ClickAreaSetup(bottomBorder, areaRange);
        Rail.RailClear += NextWave;

        if (spawnProperties.Count > 0)
        {
            railScript.SpawnStart(spawnProperties[wave]);
        }
        else Debug.LogWarning("No spawnProperties in the List");

    }

    public void NextWave()
    {
        wave++;
        if (wave >= spawnProperties.Count)
        {
            wave = 0;
        }
        Debug.Log("Next wave " + wave);
        railScript.SpawnStart(spawnProperties[wave]);
    }

    private void OnDestroy()
    {
        Rail.RailClear -= NextWave;
    }

    public void ButtonPressed(int playerButtonEnum)
    {
        switch (railScript.AreaCheck())
        {
            case 0:
                Debug.Log("Sword");
                break;
            case 1:
                Debug.Log("Schield");
                break;
            case 2:
                Debug.Log("Arrow");
                break;
            default:
                Debug.Log("No icons in area");
                break;
        }
    }
}
