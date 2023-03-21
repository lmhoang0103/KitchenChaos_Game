using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //No KitchenObject in this Counter
            if (player.HasKitchenObject())
            {   
                //Player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
                } else
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //Counter is holding a Plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            } else
            {
                //Player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}

