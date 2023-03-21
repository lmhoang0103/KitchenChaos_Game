using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    public event EventHandler OnPlatesSpawned;
    public event EventHandler OnPlatesRemoved;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnAmount;
    private int platesSpawnAmountMax;
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax) 
        {
            spawnPlateTimer = 0f;
            if (KitchenGameManager.Instance.IsGamePlaying() && platesSpawnAmount < spawnPlateTimerMax) 
            {
            platesSpawnAmount += 1;
            OnPlatesSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpawnAmount > 0)
            {
                platesSpawnAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlatesRemoved?.Invoke(this, EventArgs.Empty);
            }
        } else 
        {
        }
    }
}
