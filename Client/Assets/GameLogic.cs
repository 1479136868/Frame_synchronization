using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    // Start is called before the first frame update
    private int userid;
    private int crtFrameID;
    private bool canSend = false;
    private Dictionary<int, GameObject> allCLientDic = new Dictionary<int, GameObject>();

    private MyGame.C2S_OperationMsg m = new MyGame.C2S_OperationMsg();
    void Start()
    {
        NetManager.GetInstance().Connect("127.0.0.1", 10086);

        Message_manager.GetInstance().Addlistener((int)MsgIDDefine.S2C_ConnectResponseMsgID, connecthandler);
        Message_manager.GetInstance().Addlistener((int)MsgIDDefine.S2C_FameMsgID, logicUpdate);
    }

    /// <summary>
    /// 这个函数的频率是服务器端控制的逻辑帧的帧频。
    /// </summary>
    /// <param name="obj"></param>
    private void logicUpdate(Notification obj)
    {
        MyGame.S2C_FameMsg m = MyGame.S2C_FameMsg.Parser.ParseFrom(obj.content);
        crtFrameID = m.FrameID;

        if (m.OperationList.Count>0)  //不是空帧
        {
            foreach (var operation in m.OperationList)
            {
                if (allCLientDic.ContainsKey(operation.Userid))
                {
                    RoleCtrl ctrl = allCLientDic[operation.Userid].GetComponent<RoleCtrl>();
                    ctrl.logicUpdate(operation);
                }
                else
                {
                    GameObject prefab = Resources.Load<GameObject>("role");
                    GameObject go = GameObject.Instantiate<GameObject>(prefab);
                    allCLientDic.Add(operation.Userid, go);

                    RoleCtrl ctrl = go.GetComponent<RoleCtrl>();
                    ctrl.logicUpdate(operation);
                }
            }
        }
        else //空帧 情况下；
        {
            foreach (var go in allCLientDic.Values)
            {
                RoleCtrl ctrl = go.GetComponent<RoleCtrl>();
                ctrl.logicUpdate(null);
            }
        }



    }

    private void connecthandler(Notification obj)
    {
        MyGame.S2C_ConnectResponseMsg m = MyGame.S2C_ConnectResponseMsg.Parser.ParseFrom(obj.content);
        this.userid = m.Userid;

        m.Userid = this.userid;

    }

    // Update is called once per frame
    void Update()
    {
        NetManager.GetInstance().Update();

        ///获取输入法消息给服务器。
        m.Userid = this.userid;
        if (Input.GetKeyDown(KeyCode.A))
        {
            m.Left = 1;
            canSend = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            m.Up = 1;
            canSend = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            m.Down = 1;
            canSend = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            m.Right = 1;
            canSend = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            m.Left = 0;
            canSend = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            m.Up = 0;
            canSend = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            m.Down = 0;
            canSend = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            m.Right = 0;
            canSend = true;
        }

        if (canSend)
        {
            NetManager.GetInstance().sendMsgToServer(MsgIDDefine.C2S_OperationMsgID, m);
            canSend = false;
        }
    }
}
