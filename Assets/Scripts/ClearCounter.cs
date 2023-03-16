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
            } else
            {
                //Player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}

