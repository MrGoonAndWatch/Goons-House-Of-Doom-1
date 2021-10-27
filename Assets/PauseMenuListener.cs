using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
[SerializeField] private GameObject menuPanel;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
          if(menuPanel!=null)
          {
            menuPanel.SetActive(true);
          }
        }
    }
}
