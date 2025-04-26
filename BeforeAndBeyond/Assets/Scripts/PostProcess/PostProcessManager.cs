using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PostProcessManager : MonoBehaviour
{
  [SerializeField] private Material postProcessMat;

  private EventBinding<CharacterSwap> characterSwapEventBinding;
  
  private void OnEnable()
  {
    characterSwapEventBinding = new EventBinding<CharacterSwap>((e) =>
    {
      if (e.CharacterType == CharacterType.Knight)
      {
        postProcessMat.SetFloat("_ColorSelection", 1);
      }
      else
      {
        postProcessMat.SetFloat("_ColorSelection", 2);
      }
    });
    EventBus<CharacterSwap>.Register(characterSwapEventBinding);
  }

  private void OnDisable()
  {
    EventBus<CharacterSwap>.Deregister(characterSwapEventBinding);
  }
}
