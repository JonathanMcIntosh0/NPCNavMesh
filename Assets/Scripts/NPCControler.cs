using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCControler : MonoBehaviour
{
    [NonSerialized] public Queue<Node> Path = new Queue<Node>();
    private int _curStep = 0; 
    private Node _prevNode;
    
    public Point Destination;
    
    //Number of steps computed per fixed update.
    //By default fixedUpdate happens every 0.02s thus to traverse a slopped bridge (25 steps) it takes 0.5s
    //Note for ease of computation we have defined speed in terms of steps and not actual time thus our actual speed is
    //  dependent on how our PF alg defines steps. In the case of Reservation based we will allow diagonal and straight
    //  moves to take 1 step only!
    public int speed = 1;   

    // Start is called before the first frame update
    void Start()
    {
        //Set first destination point
        Destination = Point.GetRandomPoint();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        while (Path.Count > 0 && Path.Peek().step <= _curStep)
        {
            _prevNode = Path.Dequeue();
        }

        if (Path.Count == 0 || _prevNode.step == _curStep)
        {
            transform.position = _prevNode.pt.Pos;
        }
        else //Interpolate position between prevNode and Path.peek()
        {
            var nextNode = Path.Peek();
            var t = (_curStep - _prevNode.step) / (float) (nextNode.step - _prevNode.step);
            transform.position = Vector3.Lerp(_prevNode.pt.Pos, Path.Peek().pt.Pos, t);
        }
        _curStep += speed;
    }

    public void Init(Point start)
    {
        //Set random colour
        Random.InitState(GetInstanceID());
        GetComponent<Renderer>().material.color = Random.ColorHSV();
        
        //Move to start point
        transform.position = start.Pos;
        
        //Add to path start point and a 50 step wait (~1s) to allow time for PF alg
        Path.Enqueue(new Node(start, 0));
        Path.Enqueue(new Node(start, 50));
    }
}
