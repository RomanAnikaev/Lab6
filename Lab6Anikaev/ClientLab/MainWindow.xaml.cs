using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using System.Xml.Serialization;

namespace ClientLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, Service1.IService1Callback
    {
        bool isConnected = false;
        public static Random r = new Random();
        //обьект типа нашего хоста
        Service1.Service1Client client;
        public List<Store> stores = new List<Store>();
        public List<Product> products1 = new List<Product>();//обьект для получения
        public static string[] names_prod = { "Стілець", "Стіл кухонний", "Обідній стіл", "Крісло", "Шафа",
            "Раковина", "Ванна", "Тумба", "Комп'ютерне крісло", "Підвісне крісло"};
        public int ID;
        //FileStream file;
        public MainWindow()
        {
            InitializeComponent();
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
        public class Store
        {
            public string name_store; //Назва магазину
            public int number_of_buyers; //Кількість покупців в магазині
            public int cash_register; //Касса магазину
            public List<Product> products = new List<Product>();

            public Store()
            {
                Name_store = "Меблевий магазин";//назва магазину
                Number_of_buyers = 0;//кількість покупців
                Cash_register = 500000;
                for (int i = 0; i < 10; i++)
                {
                    int temp1 = r.Next(0, 100);
                    products.Add(new Product()
                    {
                        id = i,
                        name_product = names_prod[i], //Назва продукту
                        quantity_of_goods = temp1, //кількість товару
                    });
                }
            }
            public string Name_store
            {
                get => this.name_store;
                set => this.name_store = value;
            }
            public int Number_of_buyers
            {
                get => this.number_of_buyers;
                set => this.number_of_buyers = value;
            }
            public int Cash_register
            {
                get => this.cash_register;
                set => this.cash_register = value;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            stores.Add(new Store() { });//обьект класса клиента, stores[0]
        }

        void ConnectUser()
        {
            try
            {
                if (!isConnected)
                {
                    client = new Service1.Service1Client(new System.ServiceModel.InstanceContext(this));
                    ID = client.Connect("Roman");

                    bConnDicon.Content = "Disconnect";
                    isConnected = true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Form Closing");
            }
        }
        
        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID);
                client = null;
                bConnDicon.Content = "Connect";
                isConnected = false;
            }
        }

        //Отримати інформацію про кількість товарів
        private void OutputServ_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                try
                {
                 
                    var objSer = client.ObjServ(ID);//кількість на складі
                    dataGrid2.ItemsSource = objSer;
                    for (int i = 0; i < 10; i++)
                    {
                        stores[0].products[i].price = objSer[i].price;
                        stores[0].products[i].purchase_price = objSer[i].purchase_price;
                    }
                    dataGrid1.ItemsSource = stores[0].products;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Немає зєднання зі сервером");
            }
        }
        //Замовити доставку продукції.
        private void OutputServ_Click2(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                try
                {
                    //var prod1 = client.ObjServ(ID);
                    string prod2 = CBClient.Text;//Предмет
                    int index_prod = CBClient.SelectedIndex;//індекс продукту
                    int quant_pr = Convert.ToInt32(TbProd.Text);//кількість одиниць товару для замовлення
                    int quant2_ord = client.Ordering_Serv(prod2, quant_pr, ID);
                    //System.Windows.MessageBox.Show(Convert.ToString(quant2_ord));
                    if (quant2_ord > 0)
                    {
                        System.Windows.MessageBox.Show("Замовлено!");
                        //System.Windows.MessageBox.Show(Convert.ToString(stores[0].products[index_prod].quantity_of_goods));
                        stores[0].products[index_prod].quantity_of_goods += quant2_ord;
                        //System.Windows.MessageBox.Show(Convert.ToString(stores[0].products[index_prod].quantity_of_goods));
                        dataGrid1.ItemsSource = stores[0].products;
                        dataGrid1.Items.Refresh();
                        var objSer = client.ObjServ(ID);//кількість на складі
                        dataGrid2.ItemsSource = objSer;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Недостатня кількість на складі!");
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Немає зєднання зі сервером");
            }
        }
        //"Сформувати магазин та підєднатися до складу ->"
        private void bConnDicon_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                
                ConnectUser();
                dataGrid1.ItemsSource = stores;
            }
        }
        public void MsgCallback(string msg) 
        {
            lb.Items.Add(msg);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

    }
}
