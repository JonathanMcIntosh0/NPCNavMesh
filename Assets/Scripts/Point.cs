using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public readonly struct Point
{
    private readonly Vector2 _coords;
    private readonly Vector3 _pos;
    
    public readonly Layer Layer;
    public float x => _coords.x;
    public float y => _coords.y;
    public Vector2 Coords => new Vector2(_coords.x, _coords.y);
    public Vector3 Pos => new Vector3(_pos.x, _pos.y, _pos.z);


    public Point(Layer layer, float x, float y) : this()
    {
        this.Layer = layer;
        this._coords = new Vector2(x, y);
        
        //Calculate real world pos
        _pos = CalcPos();
    }

    public Point(Layer layer, Vector2 coords) : this(layer, coords.x, coords.y)
    {
    }

    private Vector3 CalcPos()
    {
        var npcOffset = new Vector3(0.5f, 1f, 0.5f);
        var layerOffset = Layer switch
        {
            Layer.MainFloor => new Vector3(0, 0, 0),
            Layer.BotFloor => new Vector3(0, 0, 50),
            Layer.TopFloor => new Vector3(0, 15, 50),
            Layer.LeftBridge => new Vector3(15, 0, 30),
            Layer.MidBridge => new Vector3(25, 0, 30),
            Layer.RightBridge => new Vector3(35, 0, 30),
            _ => throw new ArgumentOutOfRangeException()
        };
        var vertOffset = (Layer == Layer.LeftBridge || Layer == Layer.RightBridge) ? 
            (y + 1) * 3 / 5 * Vector3.up : Vector3.zero;
        var coordOffset = new Vector3(x, 0, y);

        return npcOffset + layerOffset + vertOffset + coordOffset;
    }

    //TODO move to a model class that contains list of obstacles
    //  Get random point (away from waiting areas)
    public static Point GetRandomPoint()
    {
        Random.InitState(DateTime.Now.Millisecond);
        //We will first choose the layer using a distribution approx according to area of the layers
        var r = Random.Range(0, 4570);
        Layer l;
        if (r < 1500) l = Layer.MainFloor;
        else if (r < 3000) l = Layer.TopFloor;
        else if (r < 4500) l = Layer.BotFloor;
        else if (r < 4525) l = Layer.LeftBridge;
        else if (r < 4550) l = Layer.RightBridge;
        else l = Layer.MidBridge;

        //Then choose a point randomly within the layer
        var p = l switch
        {
            Layer.MainFloor => new Vector2(Random.Range(0, 50), Random.Range(0, 30)),
            Layer.BotFloor => GetRandomFloorPoint(),
            Layer.TopFloor => GetRandomFloorPoint(),
            Layer.LeftBridge => new Vector2(0, Random.Range(0, 25)),
            Layer.RightBridge => new Vector2(0, Random.Range(0, 25)),
            Layer.MidBridge => new Vector2(0, Random.Range(0, 20)),
            _ => throw new ArgumentOutOfRangeException()
        };

        return new Point(l, p);
    }

    private static Vector2 GetRandomFloorPoint()
    {
        var p = new Vector2(Random.Range(0, 50), Random.Range(0, 30));
        while (14 <= p.y && p.y <= 16 && ((14 <= p.x && p.x <= 16) || (34 <= p.x && p.x <= 36)))
        {
            p = new Vector2(Random.Range(0, 50), Random.Range(0, 30));
        }
        return p;
    }
    
    
    // public List<Point> GetNeighbours()
    // {
    //     var l = new List<Point>()
    //     {
    //         new Point(Layer, x+1, y+1),
    //         new Point(Layer, x+1, y),
    //         new Point(Layer, x+1, y-1),
    //         new Point(Layer, x, y+1),
    //         new Point(Layer, x, y-1),
    //         new Point(Layer, x, y),
    //     };
    //     switch (Layer)
    //     {
    //         case Layer.MainFloor:
    //             break;
    //         case Layer.TopFloor:
    //             break;
    //         case Layer.BotFloor:
    //             break;
    //         case Layer.MidBridge:
    //             break;
    //         case Layer.LeftBridge:
    //             break;
    //         case Layer.RightBridge:
    //             break;
    //         default:
    //             throw new ArgumentOutOfRangeException();
    //     }
    // }

    public bool Equals(Point other)
    {
        return _coords.Equals(other._coords) && _pos.Equals(other._pos) && Layer == other.Layer;
    }

    public override bool Equals(object obj)
    {
        return obj is Point other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = _coords.GetHashCode();
            hashCode = (hashCode * 397) ^ _pos.GetHashCode();
            hashCode = (hashCode * 397) ^ (int) Layer;
            return hashCode;
        }
    }
}