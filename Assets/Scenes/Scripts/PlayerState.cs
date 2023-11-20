using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    //------ Player Health --------//

    public float currentHealth;
    public float maxHealth;


    //--------Player Calories ------ //
    public float currentCalories;
    public float maxCalories;

    //-------Player Hydration-------//
    public float currentHydrationPercent;
    public float maxHydrationPercent;



    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }



    void start()
    {
        currentHealth = maxHealth;

    }


    void Update()
    {
        
    }
}
