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
    #endregion

    public void CacheComponent()
    {
        animator = GetComponent<Animator>();
        txtTitle = GetComponentInChildren<Text>();
        image = GetComponent<Image>();
    }

    #region PUBLIC 
    public void SetChoiceText(string strText)
    {
        txtTitle.text = strText;
    }

    public void SetOpacityTheChoice(float value)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, value);
    }

    public void SetCorrectChoice()
    {
        animator.SetTrigger("Correct");
        image.color = Color.green;
    }

    public void SetFalseChoice()
    {
        animator.SetTrigger("Wrong");
        image.color = Color.red;
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
