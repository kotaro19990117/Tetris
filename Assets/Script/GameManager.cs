using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//シーン遷移のライブラリ


public class GameManager : MonoBehaviour
{
    //変数の作成//
    Spawner spawner;//スポナー
    Block activeBlock;//生成されたブロック格納でBlockスクリプトを適用している

    [SerializeField]
    private float dropInterval = 0.25f;//次に落ちてくるまでのインターバル
    float nextdropTimer;

    //ボードのスクリプトを格納
    board board;

    //入力受付タイマー
    float nextKeyDownTimer, nextKeyLeftRightTimer, nextKeyRotateTimer;
    //入力インターバル
    [SerializeField]//Editorから修正できるように夏
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;

    [SerializeField]
    private GameObject gameOverPanel;//パネルの格納

    bool gameOver; //ゲームオーバー判定

    private void Start()
    {
        //スポナーオブジェクトをスポナー変数に格納するコードの記述
        spawner = GameObject.FindObjectOfType<Spawner>();//Sceneから取り出してそのスクリプトを読み込んでいる
        //ビードオブジェクトをボード変数に格納するコードの記述
        board = GameObject.FindObjectOfType<board>();

        spawner.transform.position = Rounding.Round(spawner.transform.position);

        //タイマーの初期設定
        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

        //スポナークラスからブロック生成関数を呼んで変数に格納する
        if(!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
        }

        //ゲームオーベーパネルの非表示設定
        if(gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if(gameOver)
        {
            return;//PlayerInputの処理にこなくなる
        }
        PlayerInput();
        
    }

    void PlayerInput()
    {
        if(Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTimer) 
        || Input.GetKeyDown(KeyCode.D) )//指定のボタンが判定されているか？
        {
            activeBlock.MoveRight();
            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if(!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        else if(Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTimer) 
        || Input.GetKeyDown(KeyCode.A))
        {
            activeBlock.MoveLeft();
            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if(!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        else if(Input.GetKey(KeyCode.E) && (Time.time > nextKeyRotateTimer) 
        || Input.GetKeyDown(KeyCode.E))
        {
            activeBlock.RotateRight();
            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;
            if(!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateLeft();
            }
        }
        else if(Input.GetKey(KeyCode.S) && (Time.time > nextKeyDownTimer) 
        || (Time.time > nextdropTimer))
        {
            activeBlock.MoveDown();
            nextKeyDownTimer = Time.time + nextKeyDownInterval;
            nextdropTimer = Time.time + dropInterval;
            if(!board.CheckPosition(activeBlock))
            {
                    if(board.OverLimit(activeBlock))
                    {
                        GameOver();
                    }
                    else
                    {
                        BottomBoard();
                    }
            }
        }
    }

    void BottomBoard()
    {
        activeBlock.MoveUp();
        board.SaveBlockInGrid(activeBlock);
        activeBlock = spawner.SpawnBlock();

        nextKeyDownTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;

        board.ClearAllRows();
    }

    //ゲームオーバーになったらパネルを表示する
    void GameOver()
    {
        activeBlock.MoveUp();//BottomBoard関数で同じようなことしているため
        if(!gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(true);
        }

        gameOver = true;
    }

    //シーンを再読み込みする
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    

}
