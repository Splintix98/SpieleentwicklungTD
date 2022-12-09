using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{

    // TODO: maybe build own datatype for waves so that theres additional options like delays between waves and so on?

    public List<List<int>> wave_1 = new List<List<int>>();

    private void Start()
    {
        // refers to list of EnemyPrefabs passed to the EnemyManager-Gameobject and handled in the EnemySpawner-Script
        //      if Element 0 of that EnemyPrefabs-List is a turtle, than first 10 turtles are being spawned

        // { EnemyPrefabs-Index, Amount to spawn, Milliseconds between spawns }
        wave_1.Add(new List<int>() { 0, 10, 1000 });
        wave_1.Add(new List<int>() { 1, 15, 500 });
        wave_1.Add(new List<int>() { 0, 10, 1000 });
        wave_1.Add(new List<int>() { 0, 10, 500 });
        wave_1.Add(new List<int>() { 1, 10, 300 });
    }
}
