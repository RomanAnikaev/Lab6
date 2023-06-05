using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Lab6Anikaev
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IService1Callback))]
    public interface IService1
    {
        [OperationContract]
        int Connect(string name);

        [OperationContract]
        void Disconnect(int id);

        [OperationContract]
        List<Product> ObjServ(int id);

        [OperationContract]
        int Ordering_Serv(string prod, int prod_quant, int id);

        [OperationContract(IsOneWay = true)]
        void SendMsg(string msg, int id);
    }
    
    public interface IService1Callback
    {
        [OperationContract(IsOneWay = true)]
        void MsgCallback(string msg);
    }
}
