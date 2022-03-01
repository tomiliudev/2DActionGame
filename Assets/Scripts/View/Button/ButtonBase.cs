using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBase<T> : MonoBehaviour
    , IPointerClickHandler
    , IPointerDownHandler
    , IPointerUpHandler
    where T : IEventSystemHandler
{
    [SerializeField] protected GameObject targetObj = null;
    
    public virtual void Click(T controller)
    {

    }

    public virtual void Down(T controller)
    {

    }

    public virtual void Up(T controller)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ExecuteEvents.Execute<T>(
            target: targetObj == null ? GameManager.Instance.baseController.gameObject : targetObj,
            eventData: null,
            functor: (controller, data) => Click(controller)
        );
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ExecuteEvents.Execute<T>(
            target: targetObj == null ? GameManager.Instance.baseController.gameObject : targetObj,
            eventData: null,
            functor: (controller, data) => Down(controller)
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ExecuteEvents.Execute<T>(
            target: targetObj == null ? GameManager.Instance.baseController.gameObject : targetObj,
            eventData: null,
            functor: (controller, data) => Up(controller)
        );
    }
}
