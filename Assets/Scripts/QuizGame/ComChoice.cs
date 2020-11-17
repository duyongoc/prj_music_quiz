using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComChoice : MonoBehaviour
{
    
    #region FIELDS
    //
    //  private
    //
    private Animator animator;
    private Text txtTitle;
    private Image image;

    //set border up
    private ComBorder border;
    #endregion

    public void CacheComponent()
    {
        animator = GetComponent<Animator>();
        txtTitle = GetComponentInChildren<Text>();
        image = GetComponent<Image>();

        border = GetComponentInChildren<ComBorder>();
    }

    #region PUBLIC 
    public void SetActiveBorder(bool value)
    {
        border.gameObject.GetComponent<Image>().enabled = value;
    }

    public void SetTitleText(string strText)
    {
        txtTitle.text = strText;
    }

    public void SetCorrectChoice()
    {
        if(animator != null)
            animator.SetTrigger("Correct");

        image.color = Color.green;
    }

    public void SetFalseChoice()
    {
        if(animator != null)
            animator.SetTrigger("Wrong");
            
        image.color = Color.red;
    }

    public void SetOpacity(float value)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, value);
    }

    public void SetColor(Color col)
    {
        image.color = col;
    }

    public void Reset()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }
    #endregion

}
