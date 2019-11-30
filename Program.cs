using System;
using System.Linq;
using cassandra_ativ6.Models;

namespace cassandra_ativ6
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            InvoicesDbContext dbContext = new InvoicesDbContext();
            var xInvoiceList = dbContext.InvoiceItem.Where(p => true).ToList();
            foreach (var item in xInvoiceList)
            {
                System.Console.WriteLine(item.ToString());
            }
        }
    }
}
