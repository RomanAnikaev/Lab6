using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Lab6Anikaev
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]//все клиенты которые будут подключаться к нашему хосту, будут работать именно с нашем сервисов
    public class Service1 : IService1
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;//кількість кліентів
        string[] names_prod = { "Стілець", "Стіл кухонний", "Обідній стіл", "Крісло", "Шафа",
            "Раковина", "Ванна", "Тумба", "Комп'ютерне крісло", "Підвісне крісло"};
        List<Product> products = new List<Product>();
        Random r = new Random();

        public Service1() 
        {
            for (int i = 0; i < 10; i++)
            {
                int temp1 = r.Next(1000, 2300);
                int temp2 = r.Next(400, 5000);
                int temp3;
                if (temp2 <= 1000) temp3 = temp2 - 300;
                else if (temp2 > 1000 && temp2 <= 3000) temp3 = temp2 - 700;
                else temp3 = temp2 - 1200;
                products.Add(new Product()
                {
                    id = i,
                    name_product = names_prod[i], //Назва продукту
                    quantity_of_goods = temp1, //кількість товару
                    price = temp2, //товар для продажу
                    purchase_price = temp3
                });
            }
        }
        public int Connect(string name)//підключення серверу
        {
            ServerUser user = new ServerUser()
            {
                ID = nextId++,
                Name = name,

                operationContext = OperationContext.Current
            };
            
            //var serializer = new XmlSerializer(typeof(List<Product>));
            //using (Filestream file = new FileStream("file1.xml", FileMode.Create))
            //{
            //    serializer.Serialize(file, user.products);
            //}
            //file = new FileStream("file1.xml", FileMode.Create);
            SendMsg(" " + user.Name + " підєднався до чату!", 0);
            users.Add(user);
            return user.ID;
        }
        public List<Product> ObjServ(int id)//возвращает данные сервиса в виде файла
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            //file = null;
            if (user != null)
            {
                return products;
                //    file = new FileStream("file1.xml", FileMode.Open);
            }
            return null;

        }
        public int Ordering_Serv(string prod, int prod_quant, int id)//возвращает данные сервиса в виде файла
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            //file = null;
            if (user != null)
            {
                var obj = products.FirstOrDefault(x => x.name_product == prod);
                int j = obj.id;
                if (products[j].quantity_of_goods >= prod_quant)
                {
                    products[j].quantity_of_goods -= prod_quant;
                    return prod_quant;
                }
                else return 0;
                //    file = new FileStream("file1.xml", FileMode.Open);
            }
            return 0;

        }
        public void Disconnect(int id)//відєднати сервер
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if (user != null) 
            {
                users.Remove(user);
                SendMsg(" " + user.Name + " покинув чат!", 0);
            }
        }

        public void SendMsg(string msg, int id)//відповідь від сервера від користувачам
        {
            foreach(var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();

                var user = users.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    answer += ": " + user.Name + " ";
                }
                answer += msg;
                item.operationContext.GetCallbackChannel<IService1Callback>().MsgCallback(answer);  
            }
        }
    }
}
