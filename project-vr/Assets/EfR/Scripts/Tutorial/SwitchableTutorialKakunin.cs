using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableTutorialKakunin : MonoBehaviour, ISwitchableObject
{
    [SerializeField]
    bool yes = true;

    [SerializeField]
    public TutorialKakunin kakuninPanel;

    public void OnAction()
    {
        if (yes)
        {
            TVSwitch.IsOn = true;
            GameCoordinator.GetInstance().ChangeStage("tutorial");
        }
        kakuninPanel.SetActiveThisObj(false);
    }

    public void OffAction()
    {

    }
}
