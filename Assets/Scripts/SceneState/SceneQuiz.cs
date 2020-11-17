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

//==

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

//==

    #region PUBLIC FUNCTION
    public void GetRandomBackroundColor()
    {
        var randR = Random.Range(50, 150);
        var randB = Random.Range(50, 150);
        var randG = Random.Range(50, 150);

        Color randColor = new Color( randR/255f, randB/ 255f, randG/255f, 1f);
        bgImage.color = randColor;
    }
    #endregion

}
