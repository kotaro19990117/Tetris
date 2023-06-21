using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //配列の作成（生成するブロック全てを格納する）
    [SerializeField]
    Block[] Blocks;

    //関数の作成//
    //ランダムなブロックを一つ選ぶ関数
    Block GetRandomBlock()
    {
        int i = Random.Range(0, Blocks.Length);//０以上７未満から数値が選ばれる

        if(Blocks[i])
        {
            return Blocks[i];
        }
        else
        {
            return null;
        }

    }
    //選ばれたブロックを生成する関数
    public Block SpawnBlock()
    {
        Block block = Instantiate(GetRandomBlock(), transform.position,
            Quaternion.identity);

        if(block)
        {
            return block;
        }
        else
        {
            return null;
        }
    }
}