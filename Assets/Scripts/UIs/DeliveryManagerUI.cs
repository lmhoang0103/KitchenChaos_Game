using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawn += DeliveryManager_OnRecipeSpawn;
        DeliveryManager.Instance.OnRecipeComplete += DeliveryManager_OnRecipeComplete;
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeComplete(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpawn(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (RecipeSO  recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
           Transform recipeTransform = Instantiate(recipeTemplate, container);
           recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSOText(recipeSO);
           recipeTransform.gameObject.SetActive(true);
            
        }
    }
}
