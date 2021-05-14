using System.Collections.Generic;
using System.Threading;

public class SyncPostitionManager : Singleton<SyncPostitionManager>
{

    private Dictionary<int, Client> allClients = new Dictionary<int, Client>();
    private object obj = new object();

    private int clientID = 0;

    /// <summary>
    /// 每一个逻辑帧，装所有的客户端的操作的集合。
    /// </summary>
    private List<MyGame.C2S_OperationMsg> operationMsgList = new List<MyGame.C2S_OperationMsg>();

    private int frameID = 0; //逻辑帧的帧号。
    public void Init()
    {

        Message_manager.GetInstance().Addlistener((int)MsgIDDefine.ClientConnectedID, ClientConnect);
        Message_manager.GetInstance().Addlistener((int)MsgIDDefine.C2S_OperationMsgID, operationMsgHandler);
    }

    private void operationMsgHandler(Notification obj)
    {
        MyGame.C2S_OperationMsg msg = MyGame.C2S_OperationMsg.Parser.ParseFrom(obj.content);
        operationMsgList.Add(msg);
    }

    private void ClientConnect(Notification obj)
    {
        clientID++;
        allClients.Add(clientID, obj.client);

        if (clientID==2)
        {
            ///开启一个50ms执行一次的逻辑
            ThreadPool.QueueUserWorkItem(sendFrameLoop);
        }
        ///发消息告诉客户端，你的ID是什么
        MyGame.S2C_ConnectResponseMsg m = new MyGame.S2C_ConnectResponseMsg();
        m.Userid = clientID;
        NetManager.GetInstance().sendMsgToClient(MsgIDDefine.S2C_ConnectResponseMsgID, m, obj.client);
    }

    private void sendFrameLoop(object state)
    {
        ///每50毫秒执行。
        while (true)
        {
            broadcastOperationMsg();
            Thread.Sleep(100);
        }
    }

    private void broadcastOperationMsg()
    {
        MyGame.S2C_FameMsg m = new MyGame.S2C_FameMsg();
        lock (obj)
        {
            frameID++;
            m.FrameID = frameID;
            foreach (var item in operationMsgList)
            {
                m.OperationList.Add(item);
            }
            operationMsgList.Clear();
        }
        ///广播给所有客户端，操作的消息。
        foreach (var item in allClients.Values)
        {
            NetManager.GetInstance().sendMsgToClient(MsgIDDefine.S2C_FameMsgID, m, item);
        }
        //清空操作列表。
    }
}

