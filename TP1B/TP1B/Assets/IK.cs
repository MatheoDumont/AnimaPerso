using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PairSrcTarget
{
    public Transform src;
    public Transform tgt;
}

public class IK : MonoBehaviour
{
    // Le transform (noeud) racine de l'arbre, 
    // le constructeur créera une sphère sur ce point pour en garder une copie visuelle.
    public GameObject rootNode = null;
    public Vector3 rootNodeOrigin;

    // Un transform (noeud) (probablement une feuille) qui devra arriver sur targetNode 
    // public Transform srcNode = null;

    // Le transform (noeud) cible pour srcNode
    // public Transform targetNode = null;

    // Si vrai, recréer toutes les chaines dans Update
    public bool createChains = true;

    // Toutes les chaines cinématiques 
    public List<IKChain> chains = new List<IKChain>();

    // Nombre d'itération de l'algo à chaque appel
    public int nb_ite;

    public bool compute = false;

    // la liste de tous les couples <noeud source, noeud cible>
    public List<PairSrcTarget> srcTargetNodes;

    void Start()
    {
        if (createChains)
        {
            // rootNode.SetActive(false);   
            // Debug.Log("(Re)Create CHAIN");
            // Debug.Log("src:" + srcNode + " root: " + rootNode);

            rootNodeOrigin = rootNode.transform.position;
            // TODO : 
            // Création des chaines : une chaine cinématique est un chemin entre deux nœuds carrefours.
            // Dans la 1ere question, une unique chaine sera suffisante entre srcNode et rootNode.
            // chains.Add(new IKChain(srcNode, rootNode.transform, targetNode));

            // TODO-2 : Pour parcourir tous les transform d'un arbre d'Unity vous pouvez faire une fonction récursive
            // ou utiliser GetComponentInChildren comme ceci :
            // foreach (Transform tr in gameObject.GetComponentsInChildren<Transform>())
            //     Debug.Log(tr.name + " : " + tr.childCount);

            // creation de l'arbre en recursif + merge
            recursiveChainsCreation(rootNode.transform);

            for (int i = 0; i < chains.Count; ++i)
            {
                Debug.Log("CHAIN [" + i + "]");
                chains[i].Check();
            }

            // TODO-2 : Dans le cas où il y a plusieurs chaines, fusionne les IKJoint entre chaque articulation.
        }
        createChains = false;
    }

    Transform findTarget(Transform node)
    {
        for (int i = 0; i < srcTargetNodes.Count; ++i)
            if (srcTargetNodes[i].src.name == node.name)
                return srcTargetNodes[i].tgt;
        return null;
    }

    // retourn l'indice de la chaine cree
    int recursiveChainsCreation(Transform parent)
    {
        Transform child = parent;
        while (child.childCount == 1)
        {
            child = child.GetChild(0);
        }

        Transform parent_sup;
        if (parent.parent == null)
            parent_sup = parent;
        else
            parent_sup = parent.parent;

        // for (int i = 0; i < srcTargetNodes.Count; ++i)
        //     Debug.Log(srcTargetNodes[i].src.name + "paired with " + srcTargetNodes[i].tgt.name);
        // Debug.Log("child : " + child.name + " | parent : " + parent_sup.name);

        Transform t = findTarget(child);
        // Debug.Log(child + " paired with " + t);
        // Debug.Log("child " + child + " | parent : " + parent_sup + " | target : " + t);
        chains.Add(new IKChain(child, parent_sup, t));

        int idx = chains.Count - 1;

        if (child.childCount > 0)
        {
            for (int i = 0; i < child.childCount; ++i)
            {   
                int idx_sub_chain = recursiveChainsCreation(child.GetChild(i));
                chains[idx_sub_chain].Merge(chains[idx]);
            }
        }

        return idx;
    }


    void Update()
    {
        if (createChains)
            Start();

        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     IKOneStep(true);
        // }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Chains count=" + chains.Count);
            foreach (IKChain ch in chains)
                ch.Check();
        }
        // if (compute == true)
        // {
        //     compute = false;
        // }
        IKOneStep(true);

    }


    void IKOneStep(bool down)
    {

        for (int j = 0; j < nb_ite; ++j)
        {

            // TODO : IK Backward (remontée), appeler la fonction Backward de IKChain 
            // sur toutes les chaines cinématiques.
            foreach (IKChain chain in chains)
                chain.Backward();

            // TODO : appliquer les positions des IKJoint aux transform en appelant ToTransform de IKChain
            foreach (IKChain chain in chains)
                chain.ToTransform();

            // IK Forward (descente), appeler la fonction Forward de IKChain 
            // sur toutes les chaines cinématiques.
            foreach (IKChain chain in chains)
                chain.Forward();

            // TODO : appliquer les positions des IKJoint aux transform en appelant ToTransform de IKChain
            foreach (IKChain chain in chains)
                chain.ToTransform();
            Debug.Log(chains[0].First().name + " => " + chains[0].First().position);
            Debug.Log(chains[0].First().name + " => " + chains[1].First().position);
            Debug.Log(chains[0].First().name + " => " + chains[2].First().position);
            Debug.Log("passe!");
        }
    }
}