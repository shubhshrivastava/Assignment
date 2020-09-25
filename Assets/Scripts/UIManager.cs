using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using GoogleARCore.Examples.ObjectManipulation;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [FormerlySerializedAs("m_backButton")]
    [SerializeField]
    public Button _backButton;

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
    

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDatacurrentPosition = new PointerEventData(EventSystem.current);
        eventDatacurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDatacurrentPosition, results);
        return results.Count > 0;
    }
  
}
