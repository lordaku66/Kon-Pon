using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusumuTeleport : MonoBehaviour
{
    FetchQuestManager fetchQuest;
    InputManager inputManager;
    int questID;

    bool movedAway = false;
    bool questStart = false;

    public GameObject owari;

    private void Awake()
    {
        fetchQuest = FindObjectOfType<FetchQuestManager>();
        inputManager = FindObjectOfType<InputManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        questID = fetchQuest.GetActiveQuestID();

        // Check for starter quest
        if (questID == 0)
        {
            questStart = true; 
        }

        // Check when the quest ends, and then teleport Susumu when Dialogue Exit
        if(questStart)
        {
            if (questID == -1 && !movedAway)
            {
                if(inputManager.DialogueExit())
                {
                    transform.position = owari.transform.position + new Vector3(0f, 0f, 0.5f);
                    transform.eulerAngles = owari.transform.eulerAngles;
                    movedAway = true;
                }
            }
        }
    }
}
