using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using GoogleARCore.Examples.ObjectManipulation;
public class UIManager : MonoBehaviour
{
    [FormerlySerializedAs("m_backButton")]
    [SerializeField]
    public Button _backButton;

    [FormerlySerializedAs("m_object1")]
    [SerializeField]
    public Button _object1;

    [FormerlySerializedAs("m_object2")]
    [SerializeField]
    public Button _object2;

    int sceneIndex;

    public PawnManipulator pawnspawn;

    // Start is called before the first frame update
    void Start()
    {
        _backButton.onClick.AddListener(onBackButtonClicked);
     

        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(sceneIndex - 1);
        }
    }

    public void onBackButtonClicked()
    {
         SceneManager.LoadScene(sceneIndex - 1);
    }

  
}
