using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public const int NumberNPCs = 5;
    private List<GameObject> _npcList = new List<GameObject>(NumberNPCs);

    private void Start()
    {
        //Create Obstacles
        
        //Initialise NPCs
        var startPtList = new Point[NumberNPCs];
        for (int i = 0; i < NumberNPCs; i++)
        {
            var p = Point.GetRandomPoint();
            //TODO make sure random points are not in obstacles
            while(startPtList.Contains(p)) p = Point.GetRandomPoint();
            startPtList[i] = p;
            
            var npc = Instantiate(Resources.Load("NPC")) as GameObject;
            var controller = npc.GetComponent<NPCControler>();
            
            controller.Init(p);
            npc.SetActive(true);
            
            _npcList.Add(npc);
        }
    }
}