using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum MsgIDDefine
{
    /// <summary>
    /// 上行协议的编号
    /// </summary>
    C2S_OperationMsgID = 10001,

    ///下行协议。 
    S2C_ConnectResponseMsgID = 20001,
    S2C_FameMsgID = 20003,


    ///不是网络消息编号  ，用于内部模块间的事件派发。
    ClientConnectedID = 30001,  //当有客户的链接进来
    ClientClosedID = 30002,

    ///当客户端从服务器端掉线了
}

