using UnityEngine;

public class PopUpController : MonoBehaviour, IPopUpController
{
    
}

public interface IPopUpController
{
    Transform transform { get; }
}