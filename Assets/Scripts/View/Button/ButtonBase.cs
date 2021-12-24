using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBase<T> : MonoBehaviour, IPointerClickHandler
    where T : IEventSystemHandler
{
    protected GameObject targetObj = null;
    public void OnPointerClick(PointerEventData eventData)
    {
        ExecuteEvents.Execute<T>(
            target: targetObj == null ? gameObject : targetObj,
            eventData: null,
            functor: (controller, data) => Execute(controller)
        );
    }

    public virtual void Execute(T controller)
    {

    }
}
