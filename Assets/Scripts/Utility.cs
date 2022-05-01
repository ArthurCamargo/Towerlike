using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {
    public enum Rewards {
        NONE,
        ELEMENT_ITEM,
        EFFECT_ITEM,
        STAT_ITEM,
        SOCKET_ITEM,
        COMBAT_ITEM,
        TOWER_ITEM,
        BASE_HEAL,
    }

    public enum Difficulty {
        VERY_LOW,
        LOW,
        MEDIUM,
        HIGH,
        VER_HIGH,
    }

    public static T[] ShuffleArray<T>(T[] array, int seed) {
        System.Random prng = new System.Random(seed);

        for(int i = 0; i < array.Length - 1; i++) {
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
            if(collider != null) {
                if(collider.tag == "Enemy") {
                    collider.GetComponent<Enemy>().TakeAttack(new Attack(damage, element, effects));
                }
            }
        }
    }

    internal static float AdaptToDifficulty(Difficulty difficulty, float value) {
        float adaptedValue = value;

        switch(difficulty) {
            case Difficulty.VERY_LOW:
                adaptedValue = value * 0.20f;
                break;
            case Difficulty.LOW:
                adaptedValue = value * 0.60f;
                break;
            case Difficulty.MEDIUM:
                adaptedValue = value;
                break;
            case Difficulty.HIGH:
                adaptedValue = value * 1.40f;
                break;
            case Difficulty.VER_HIGH:
                adaptedValue = value * 1.80f;
                break;
        }

        if(adaptedValue < 1) {
            adaptedValue = 1;
        }

        return adaptedValue;
    }

    internal static float DifficultyIncreaseFunction(int waveNum, float value, float multiplyer) {
        return value * Mathf.Pow(multiplyer, waveNum - 1);
    }
}
