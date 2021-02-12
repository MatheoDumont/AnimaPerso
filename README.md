# Animation de personnage
Etudiant : Mathéo Dumont
TP d'Alexandre Meyer [énoncés](https://perso.liris.cnrs.fr/alexandre.meyer/public_html/www/doku.php?id=master_charanim_tp_m2_unity)


Vous pouvez trouver la vidéo de présentation [ici](https://youtu.be/HcCChaSXCns).

## Organisation du répertoire

Le TP est divisé en deux catégories :
- [TP1A](TP1A)
- [TP1B](TP1B/TP1B)

## TP 1A
Le fichier [perso]() contient les assets de Mixamo ainsi que les scripts d'IK, d'animation et l'animator controller.

### BlendTree et Machine à états finis

Cette partie anime le personnage à l'aide des données de [Mixamo](https://www.mixamo.com/).
J'utilise un BlendTree pour les animations de marche et de courses dans leurs différents degrés (marcher, marche à droite, courir à droite, ...).
Les paramètres HSpeed et VSpeed sont utilisées pour "mélanger" les animations.
Ce BlendTree est contenu dans une machine à état qui permet de passer de la marche au saut quand la touche espace est préssée.
Le [script](TP1A/Assets/perso/MyAnimConScript.cs) décrit le comportement adopté.

### IK Unity 

J'ai implémenté l'IK avec Unity en suivant [ce tutoriel](https://www.youtube.com/watch?v=EggUxC5_lGE), le script résultant se trouve [ici](https://github.com/MatheoDumont/AnimaPerso/blob/master/TP1A/Assets/perso/IKScript.cs).
Après avoir fait quelques paramètrages, pour activer l'IK avec l'animator, il faut créer des "courbes" pour les différentes animations et leurs attacher un paramètre.
Une courbe décrit pour un paramètre son évolution en parallèle de l'animation, cela est utile pour savoir quand un pied touche le sol, et alors activer l'IK à ce moment précis.

Le script lance un rayon pour récupérer la position et la rotation à laquelle le pied va être mise pour s'adapter le plus "naturellement" au sol.

J'utilise l'IK sur les animations d'idle et de walk.

### Physique et Ragdoll

j'ai installé un ragdoll sur le personnage, cela permet d'obtenir des collisions plus réalistes qu'avec une simple capsule.
La contrepartis est cela perturbe l'utilisation de l'IK, pour l'intersection de rayon avec le sol.
Une solution est d'utiliser les LayerMask de Unity pour ne prendre en compte que certains objets, dans mon cas, ceux qui ne sont pas de type *Characters*.

### Plus
J'utilise les textures fournis dans le pack de Mixamo !

## TP 1B

### Inverse Kinematics : Fabrik

L'implémentation de Fabril permet d'avoir une solution d'IK simple et temps réel en comparaison à d'autre solution comme la Jacobienne inversée ou CCD.
Cette solution est approchée et donc fonctionnelle pour des cas comme des jeux, mais pas adaptée pour des simulations physiques fidèles.

Mon implémentation fonctionne avec une seule chaine et presque avec un arbre, les évolutions des points sont "bloquées" à la jointure des deux sous-chaines 
et la chaine parent ne bouge pas librement.

### Comparaison Fabrik / IK Unity
L'IK de Unity fonctionne sur une seule chaine, et non un arbre, il ne prend pas en compte de limitation avec les rotations, mais le fait 
pour les distances entres les jointures.
Si l'on veut des contraintes sur les rotations, il faut les ajouter dans un script.

Fabrik peut utiliser un arbre plutôt qu'une seule chaine, peut avoir des contraintes sur les distances entre les jointures, et des contraintes sur les angles
de rotation des jointures.

Dans les deux cas il faut proposer une target pour les joints en bout de chaine qu'on veut placer.
Unity permet de mélanger de l'IK et de l'animation avec des machines à états finis et BlendTree, à l'aide de graphe de fonction qui font évoluer des paramètres
au cours du temps, cela pour chaque animation séparement. L'inconvénient est qu'il faut éditer manuellement ces graphes pour obtenir les effets désirés,
comme avoir un paramètre qui vaut 1 quand les pieds d'une animationt de course touchent le sol et pas à un autre moment.














