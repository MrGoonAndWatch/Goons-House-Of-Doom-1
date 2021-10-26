using System.Linq;
using UnityEngine;

public class ProcessGameState : MonoBehaviour
{
    private void Start()
    {
        LoadGameStateToScene();
    }

    private static void LoadGameStateToScene()
    {
        var dataSaver = FindObjectOfType<DataSaver>();
        var gameState = dataSaver.GetGameState();

        var enemies = FindObjectsOfType<DamageHandler>();
        var doors = FindObjectsOfType<Door>();
        var items = FindObjectsOfType<Item>();

        DestroyPreviouslyKilledEnemies(gameState, enemies);
        UnlockPreviouslyUnlockedDoors(gameState, doors);
        ProcessPreviouslyTriggeredEvent(gameState, doors);
        DeletePreviouslyPickedUpItems(gameState, items);
    }

    private static void DestroyPreviouslyKilledEnemies(DataSaver.GameState gameState, DamageHandler[] enemies)
    {
        foreach (var damageHandler in enemies)
        {
            if (damageHandler.EnemyId != 0 && gameState.DeadEnemies.Contains(damageHandler.EnemyId))
                damageHandler.ForceDead();
        }
    }

    private static void UnlockPreviouslyUnlockedDoors(DataSaver.GameState gameState, Door[] doors)
    {
        foreach (var door in doors)
        {
            if(door.DoorId != 0 && gameState.DoorsUnlocked.Contains(door.DoorId))
                door.ForceUnlock();
        }
    }

    private static void DeletePreviouslyPickedUpItems(DataSaver.GameState gameState, Item[] items)
    {
        foreach (var item in items)
        {
            if(item.ItemId != 0 && gameState.GrabbedItems.Contains(item.ItemId))
                item.ForceDestroy();
        }
    }

    private static void ProcessPreviouslyTriggeredEvent(DataSaver.GameState gameState, Door[] doors)
    {
        foreach (var gameStateTriggeredEvent in gameState.TriggeredEvents)
        {
            // TODO: Eventually probably need to propagate these through more than just doors...
            foreach (var door in doors)
            {
                door.OnEvent((GlobalEvent)gameStateTriggeredEvent);
            }
        }
    }
}
