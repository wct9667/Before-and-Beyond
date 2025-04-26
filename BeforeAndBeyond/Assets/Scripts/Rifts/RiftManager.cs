using System.Data;
using UnityEngine;

public class RiftManager : MonoBehaviour
{
   [Header("Rifts to close")]
   [SerializeField] private int riftsToWin = 3;

   private int riftsClosed;

   private EventBinding<RiftClosed> riftClosedEventBinding;
   private void OnEnable()
   {
      riftClosedEventBinding = new EventBinding<RiftClosed>(() =>
      {
         riftsClosed++;
         CheckWin();
      });
      
      EventBus<RiftClosed>.Register(riftClosedEventBinding);
   }

   private void OnDisable()
   {
      EventBus<RiftClosed>.Deregister(riftClosedEventBinding);
   }

   private void CheckWin()
   {
      if (riftsClosed >= riftsToWin)
      {
         Debug.Log("WIN");
         DataTracker.Instance.SaveToFile();
         SceneLoader.LoadScene(0);
      }
   }
}
