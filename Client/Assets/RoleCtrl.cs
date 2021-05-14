using MyGame;
using UnityEngine;

public class RoleCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    private float rate = 0;
    private bool left;
    private bool up;
    private bool down;
    private bool right;
    void Start()
    {
        this.transform.position = Vector3.zero;
        rate = 0.4f;
    }

    /// <summary>
    /// 每一个逻辑调用。
    /// </summary>
    /// <param name="item"></param>
    public void logicUpdate(C2S_OperationMsg item)
    {
        if (item==null)  //消息里没有操作，空帧的时候。
        {
            if (left)
            {
                this.transform.position = this.transform.position + rate * Vector3.left;
            }
            if (right)
            {
                this.transform.position = this.transform.position + rate * Vector3.right;
            }
            if(down)
            {
                this.transform.position = this.transform.position + rate * Vector3.back;
            }

            if (up)
            {
                this.transform.position = this.transform.position + rate * Vector3.forward;
            }
        }
        else  
        {
            if (item.Down == 1)
            {
                down = true;
                this.transform.position = this.transform.position + rate * Vector3.back;
            }
            if (item.Down == 0)
            {
                down = false;
                
            }
            if (item.Up == 1)
            {
                up = true;
                this.transform.position = this.transform.position + rate * Vector3.forward;
            }
            if (item.Up == 0)
            {
                up = false;
               
            }

            if (item.Left == 1)
            {
                left = true;
                this.transform.position = this.transform.position + rate * Vector3.left;
            }
            if (item.Left == 0)
            {
                left = false;
                
            }

            if (item.Right == 1)
            {
                right = true;
                this.transform.position = this.transform.position + rate * Vector3.right;
            }
            if (item.Right == 0)
            {
                right = false;
            }
        }



    }
}
