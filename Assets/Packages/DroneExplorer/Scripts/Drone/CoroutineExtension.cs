using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class CoroutineExtension
{

    // для отслеживания используем словарь <название группы, количество работающих корутинов>
    static private bool runner;
    // MonoBehaviour нам нужен для запуска корутина в контексте вызывающего класса
    public static void ParallelCoroutines(IEnumerator coroutine, MonoBehaviour parent)
    {

        runner = true;
        parent.StartCoroutine(DoParallel(coroutine, parent));
    }


    static IEnumerator DoParallel(IEnumerator coroutine, MonoBehaviour parent)
    {
        yield return parent.StartCoroutine(coroutine);
        runner = false;
    }

    // эту функцию используем, что бы узнать, есть ли в группе незавершенные корутины
    public static bool Processing()
    {
        return runner;
    }
}

