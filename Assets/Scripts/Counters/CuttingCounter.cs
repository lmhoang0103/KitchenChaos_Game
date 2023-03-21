
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CuttingCounter : BaseCounter, IHasProgress
{
    private int cuttingProgress;


    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler Oncut;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //No KitchenObject in this Counter
            if (player.HasKitchenObject())
            {
                //Player is carrying something
                //Only allow to place on this counter if this kitchen Object can be cut
                if( HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                
                player.GetKitchenObject().SetKitchenObjectParent(this);
                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                cuttingProgress = 0;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                    progressNormalize = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            } else
            {
                //Player not carrying anything
            }
        } else
        {
            //Has KitchenObject in this Counter
            if (player.HasKitchenObject())
            {
                //Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {

                        GetKitchenObject().DestroySelf();
                    }
                }
            } else
            {
                //Player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
                cuttingProgress = 0;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalize = 0f
                });


            }
        }
    }

    public override void InteractAlternate(Player player)
    {


        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttingProgress += 1;

            Oncut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalize = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);

            }


        }

    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        } else
        {
            return null;
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }

        return null;
    }
}
