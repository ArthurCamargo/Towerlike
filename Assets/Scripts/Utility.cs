using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random prng = new System.Random(seed);

        for (int i = 0; i < array.Length - 1; i++)
        {
            int randomIndex = prng.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }

        return array;
    }

    public static bool ProcTest(float procPercentage) {
        System.Random rand = new System.Random();
        return rand.NextDouble() < procPercentage / 100.0;
    }

    public static void Explode(Vector3 pos, float range, float damage, Attributes.Elements element, List<Effect> effects) {
        Collider[] colliders = Physics.OverlapSphere(pos, range);
        foreach(Collider collider in colliders) {
            if(collider.tag == "Enemy") {
                collider.GetComponent<Enemy>().TakeAttack(new Attack(damage, element, effects));
            }
        }
    }
}
