using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
    
namespace HostOne
{
    class Program
    {
        static void Main(string[] args)
        {
            
            using (var host = new ServiceHost(typeof(Lab6Anikaev.Service1)))
            {
                host.Open();
                Console.WriteLine("Хост розпочав роботу!");
                Console.ReadLine();
            }
        }
    }
}
