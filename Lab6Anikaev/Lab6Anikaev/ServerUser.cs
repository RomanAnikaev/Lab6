using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Lab6Anikaev
{
    internal class ServerUser
    {
        public int ID { get; set; } //Id кліента
        public string Name { get; set; } //Імя кліента

        
        public OperationContext operationContext { get; set; }
    }
    public class Product
    {
        public int id { get; set; } //Id
        public string name_product { get; set; } //Назва продукту
        public int quantity_of_goods { get; set; } //кількість товару
        public int price { get; set; } //вартість продукту для продажу
        public int purchase_price { get; set; } //закупівельна ціна
        public Product() { }
    }
}
