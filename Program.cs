using System.Threading;
using System;
using System.Linq;
using Cassandra;
using cassandra_ativ6.Models;
using System.Globalization;
using cassandra_ativ6.Infra.PDF;

namespace cassandra_ativ6
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            InvoicesDbContext mySQLContext = new InvoicesDbContext();
            var invoiceItens = mySQLContext.InvoiceItem.Where(p => true).ToList();
            // foreach (var item in xInvoiceList)
            // {
            //     System.Console.WriteLine(item.ToString());
            // }
            var cassandraContext = Cluster.Builder().AddContactPoints("localhost")
            .WithDefaultKeyspace("cassandraativ6").Build();

            var cassandraSession = cassandraContext.ConnectAndCreateDefaultKeyspaceIfNotExists();

            cassandraSession.Execute(@"
                CREATE TABLE IF NOT EXISTS invoice ( 
                    InvoiceNumber  int,   
                    CustomerName text,         
                    CustomerAddress text, 
                    InvoiceItemID int,
                    ServiceDescription text, 
                    InvoiceItemQuantity int, 
                    InvoiceItemUnitValue double, 
                    ResourceName text, 
                    QualificationName text, 
                    InvoiceItemTaxValue double, 
                    InvoiceItemDiscountValue double, 
                    InvoiceItemSubtotal double, 
                    InvoiceTotal double, 
                    PRIMARY KEY ((InvoiceNumber), InvoiceItemID) 
                ) ;
            ");

            foreach (var invoiceItem in invoiceItens)
            {
                cassandraSession.Execute(getCassandraInsertFromInvoiceItem(invoiceItem));
            }
            
            System.Console.WriteLine("--------------------------");
            System.Console.WriteLine("Insira o número da nota:");
            System.Console.WriteLine("--------------------------");
            var id = Convert.ToInt32(Console.ReadLine());
            System.Console.WriteLine("-- Gerando PDF...       --");
            GeraPDF.Gerar(id);

            System.Console.WriteLine("--------------------------");
            System.Console.WriteLine("--- EXECUÇÃO FINALIZADA --");
            System.Console.WriteLine("--------------------------");

        }

        public static string getCassandraInsertFromInvoiceItem(InvoiceItem pInvoiceItem){
            var specifier = "F";
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            var taxValue = pInvoiceItem.InvoiceItemTaxValue.HasValue ? pInvoiceItem.InvoiceItemTaxValue.Value : 0.0;
            var discountValue = pInvoiceItem.InvoiceItemDiscountValue.HasValue ? pInvoiceItem.InvoiceItemDiscountValue.Value : 0.0;
            var invoiceTotal = pInvoiceItem.InvoiceTotal.HasValue ? pInvoiceItem.InvoiceTotal.Value : 0.0;
            var insert = $@"
                INSERT INTO invoice (
                    InvoiceNumber,   
                    CustomerName,         
                    CustomerAddress, 
                    InvoiceItemID,
                    ServiceDescription, 
                    InvoiceItemQuantity, 
                    InvoiceItemUnitValue, 
                    ResourceName, 
                    QualificationName, 
                    InvoiceItemTaxValue, 
                    InvoiceItemDiscountValue, 
                    InvoiceItemSubtotal, 
                    InvoiceTotal
                ) VALUES ({pInvoiceItem.InvoiceNumber},
                '{pInvoiceItem.CustomerName}',
                '{pInvoiceItem.CustomerAddress}',
                {pInvoiceItem.InvoiceItemId},
                '{pInvoiceItem.ServiceDescription}',
                {pInvoiceItem.InvoiceItemQuantity},
                {pInvoiceItem.InvoiceItemUnitValue},
                '{pInvoiceItem.ResourceName}',
                '{pInvoiceItem.QualificationName}',
                {taxValue.ToString(specifier, culture)},
                {discountValue.ToString(specifier, culture)},
                {pInvoiceItem.InvoiceItemSubtotal.ToString(specifier, culture)},
                {invoiceTotal.ToString(specifier, culture)});
            "
                .Replace("\n", "")
                .Replace("\r", "");
            return insert;
        }
    }
}
