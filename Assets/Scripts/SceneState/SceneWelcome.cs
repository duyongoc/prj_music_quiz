using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneWelcome : StateScene
{
    
    public AudioClip audi;

    #region STATE
    public override void StartState()
    {
        base.EndState();
        Owner.SetActivePanelScene(this.name);

        SoundManger.GetInstance().PlaySound(audi);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        
    }

    public override void EndState()
    {
        base.EndState();

       
    }
    #endregion
}
