using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IKChain
{
    // Quand la chaine comporte une cible pour la racine. 
    // Ce sera le cas que pour la chaine comportant le root de l'arbre.
    private IKJoint rootTarget = null;

    // Quand la chaine à une cible à atteindre, 
    // ce ne sera pas forcément le cas pour toutes les chaines.
    private IKJoint endTarget = null;

    // Toutes articulations (IKJoint) triées de la racine vers la feuille. N articulations.
    private List<IKJoint> joints = new List<IKJoint>();

    // Contraintes pour chaque articulation : la longueur (à modifier pour 
    // ajouter des contraintes sur les angles). N-1 contraintes.
    private List<float> constraints = new List<float>();


    // Un cylndre entre chaque articulation (Joint). N-1 cylindres.
    //private List<GameObject> cylinders = new List<GameObject>();    


    // Créer la chaine d'IK en partant du noeud endNode et en remontant jusqu'au noeud plus haut, ou jusqu'à la racine

    public IKChain(Transform _endNode, Transform _rootTarget = null, Transform _endTarget = null)
    {
        // Debug.Log("BEGIN=== IKChain::createChain: ===");
        // TODO : construire la chaine allant de _endNode vers _rootTarget en remontant dans l'arbre. 
        // Chaque Transform dans Unity a accès à son parent 'tr.parent'
        endTarget = new IKJoint(_endTarget);
        rootTarget = new IKJoint(_rootTarget);
        rootTarget.SetPosition(_rootTarget.transform.position);

        bool after_parent = false;
        for (Transform t = _endNode; !after_parent && t != null; t = t.parent)
        {
            joints.Add(new IKJoint(t));
            if (t.name == _rootTarget.name)
                after_parent = true;

        }
        joints[joints.Count - 1] = rootTarget;

        for (int i = 1; i < joints.Count; ++i)
            constraints.Add((joints[i].position - joints[i - 1].position).magnitude);

    }

    public IKJoint First() { return joints[0]; }
    public IKJoint Last() { return joints[joints.Count - 1]; }

    // public void Merge(IKJoint j)
    public void Merge(IKChain c)
    {

        // TODO-2 : fusionne les noeuds carrefour quand il y a plusieurs chaines cinématiques
        // Dans le cas d'une unique chaine, ne rien faire pour l'instant.

        if (First().name == c.joints[0].name)
        {
            joints[0] = c.joints[0];
        }
        if (Last().name == c.joints[0].name)
        {
            joints[joints.Count - 1] = c.joints[0];
        }

    }

    public void Backward()
    {
        // TODO : une passe remontée de FABRIK. Placer le noeud N-1 sur la cible, 
        // puis on remonte du noeud N-2 au noeud 0 de la liste 
        // en résolvant les contraintes avec la fonction Solve de IKJoint.

        if (endTarget.transform != null)
            First().SetPosition(endTarget.position);

        for (int i = 1; i < joints.Count; ++i)
            joints[i].Solve(joints[i - 1], constraints[i - 1]);

    }

    public void Forward()
    {
        // TODO : une passe descendante de FABRIK. Placer le noeud 0 sur son origine puis on descend.
        // Codez et deboguez déjà Backward avant d'écrire celle-ci.

        Last().SetPosition(rootTarget.position);

        for (int i = joints.Count - 2; i >= 0; --i)
            joints[i].Solve(joints[i + 1], constraints[i]);
    }

    public void ToTransform()
    {
        // TODO : pour tous les noeuds de la liste appliquer la position au transform : voir ToTransform de IKJoint
        for (int i = joints.Count - 1; i >= 0; --i)
            joints[i].ToTransform();
    }

    public void Check()
    {
        // TODO : des Debug.Log pour afficher le contenu de la chaine (ne sert que pour le debug)
        Debug.Log("RootTarget : " + rootTarget.transform);
        Debug.Log("EndTarget : " + endTarget.transform);

        foreach (IKJoint j in joints)
            Debug.Log(j.name + " : " + j.positionTransform);
        

    }

}