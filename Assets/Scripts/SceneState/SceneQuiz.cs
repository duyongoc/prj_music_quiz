using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneQuiz : StateScene
{
    
    #region FIELDS
    //
    //  private 
    //
    public Image bgImage;
    #endregion

    #region STATE
    public override void StartState()
    {
        base.EndState();
        Owner.SetActivePanelScene(this.name);
        
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
