using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class IKJoint
{
    // la position modifiée par l'algo : en fait la somme des positions des sous-branches. 
    // _weight comptera le nombre de sous-branches ayant touchées cette articulation.
    private Vector3 _position;

    // un lien vers le Transform de l'arbre d'Unity
    private Transform _transform;

    // un poids qui indique combien de fois ce point a été bougé par l'algo.
    public float _weight = 0.0f;

    public string name
    {
        get
        {
            return _transform.name;
        }
    }
    public Vector3 position     // la position moyenne
    {
        get
        {
            if (_weight == 0.0f) return positionTransform;
            else return _position / _weight;
        }
    }

    public Vector3 positionTransform
    {
        get
        {
            return _transform.position;
        }
    }

    public Transform transform
    {
        get
        {
            return _transform;
        }
    }

    public Vector3 positionOrigParent
    {
        get
        {
            return _transform.parent.position;
        }
    }

    public IKJoint(Transform t)
    {
        // TODO : initialise _position, _weight
        _transform = t;
        _position = new Vector3(0.0f, 0.0f, 0.0f);
        _weight = 0.0f;
    }

    public void SetPosition(Vector3 p)
    {
        _position = p;
        _weight = 1.0f;
    }

    public void AddPosition(Vector3 p)
    {
        // TODO : ajoute une position à 'position' et incrémente '_weight'
        _position += p;
        _weight += 1.0f;
    }

    public void ToTransform()
    {
        _transform.position = position;
        _position = new Vector3(0.0f, 0.0f, 0.0f);
        _weight = 0.0f;
    }

    public void Solve(IKJoint anchor, float l)
    {
        // TODO : ajoute une position (avec AddPosition) qui repositionne _position à la distance l
        // en restant sur l'axe entre la position et la position de anchor
        Vector3 dirPointToAnchor = Vector3.Normalize(position - anchor.position);
        AddPosition((dirPointToAnchor * l) + anchor.position);
    }
}