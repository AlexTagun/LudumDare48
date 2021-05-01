using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform _followCamera;
    [SerializeField] private TextMeshProUGUI _levelText;
    
    [SerializeField] private DropGenerator _dropGenerator;
    [SerializeField] private ItemSpawner _itemSpawner;

    [SerializeField] private Transform _center;
    
    [SerializeField] private float _spawnOffset = 20f;

    [SerializeField] private ItemSpawnPositionController _itemSpawnPositionController;

    public static int CurHeroCount = 0;
    public static int CollectedCoinsCount = 0;

    private void Awake()
    {
        SpeedUpHeroManager.Init();
    }

    private void Update()
    {
        
        TrySpawn();
    }

    private void TrySpawn()
    {
        if (!CanSpawnObject())
        {
            return;
        }
        
        RollDrop(true);
        RollDrop(false);
        
        LadderLevelManager.LevelUp();
        _levelText.text = LadderLevelManager.GetDisplayCurrentLevel().ToString();
    }

    private bool CanSpawnObject()
    {
        return  _itemSpawnPositionController.GetNextSpawnPosition() + _spawnOffset >= _followCamera.position.y;
    }

    private void RollDrop(bool isFirstPoint)
    {
        var drop = _dropGenerator.GetDropInfo(LadderLevelManager.CurrentLevel);
        
        var spawnPosition = _itemSpawnPositionController.GetSpawnPosition(drop?.SpawnOffset, isFirstPoint);
        

        if (drop == null)
        {
            return;
        }

        var spawnGo =  _itemSpawner.SpawnObject(drop, spawnPosition);

        var spawnRotator = spawnGo.GetComponent<SpawnRotator>();
        if (spawnRotator != null)
        {
            spawnRotator.RotateForward(_center.position);
        }
    }

    private void OnDestroy()
    {
        SpeedUpHeroManager.Clear();
    }
}
