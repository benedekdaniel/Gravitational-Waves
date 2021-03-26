using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyOnClick : MonoBehaviour
{
    public Image img;

    public void OnClick()
    {
        Destroy(gameObject);
    }
   
}
