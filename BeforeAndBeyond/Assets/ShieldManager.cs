using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    [SerializeField] private float healthChange = 25; 
    private EventBinding<DecreasePlayerHealth> playerDecreaseHealthEventBinding;

    private void OnEnable()
    {
        playerDecreaseHealthEventBinding = new EventBinding<DecreasePlayerHealth>((e) =>
        {
            EventBus<IncreasePlayerHealth>.Raise(new IncreasePlayerHealth()
            {
                healthChange = healthChange
            });
        });
        EventBus<DecreasePlayerHealth>.Register(playerDecreaseHealthEventBinding);
    }

    private void OnDisable()
    {
        EventBus<DecreasePlayerHealth>.Deregister(playerDecreaseHealthEventBinding);
    }
}
