using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIManager : MonoBehaviour
{
    public GameObject ConnectOptionPanelGameObject;
    public GameObject ConnectWithNameGameObject;

    #region Unity methods
    // Start is called before the first frame update
    void Start()
    {
        ConnectOptionPanelGameObject.SetActive(true);
        ConnectWithNameGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
}
