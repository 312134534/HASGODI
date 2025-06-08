using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TriggerSelect : MonoBehaviour, IPointerEnterHandler
{
    public GameObject ManagerSelect;
    public Role role;

    private DetermineChoose determine;
    private void Awake()
    {
        determine = ManagerSelect.GetComponent<DetermineChoose>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        determine.ShowTechie(role);
    }

}
