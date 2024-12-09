using UnityEngine;

public class PopUpController : BasePopUp, IPopUpController
{
    
}

public interface IPopUpController
{
    Transform transform { get; }
}