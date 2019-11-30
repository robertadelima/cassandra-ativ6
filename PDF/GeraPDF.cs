using Cassandra;
using System;
using System.Collections.Generic;
using System.Text;
using static cassandra_ativ6.Infra.NotaLayout;

namespace cassandra_ativ6.Infra.PDF
{
    public static class GeraPDF
    {
        public static void Gerar(int id)
        {
            

            IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();

            // Render an HTML document or snippet as a string
            //var PDF = Renderer.RenderHtmlAsPdf("<h1>Hello World</h1>");
            var PDF = Renderer.RenderHtmlAsPdf(PreencheDados(id));
            PDF.SaveAs("nota.pdf");


            //// Advanced: 
            //// Set a "base url" or file path so that images, javascript and CSS can be loaded  
            //var PDF = Renderer.RenderHtmlAsPdf("<img src='icons/iron.png'>", @"C:\site\assets\");
            //PDF.SaveAs("html-with-assets.pdf");
        }

        private static NotaLayout BuscaNota(int idNota)
        {
            var myCassandraCluster = Cluster.Builder().AddContactPoints("localhost")
            .WithDefaultKeyspace("cassandraativ6").Build();

            var session = myCassandraCluster.ConnectAndCreateDefaultKeyspaceIfNotExists();

            NotaLayout nota = new NotaLayout();

                var query = $"SELECT * FROM invoice where InvoiceNumber = {idNota} ;";
                //var query = "SELECT now() agora FROM system.local;"
                var rs = session.Execute(query);

                short contador = 1;
                foreach (var row in rs)
                {
                    if (contador == 1)
                    {
                        nota.nf = new order
                        {
                            CustomerAddress = row.GetValue<string>("customeraddress"),
                            CustomerName = row.GetValue<string>("customername"),
                            InvoiceNumber = row.GetValue<int>("invoicenumber"),
                            InvoiceTotal = row.GetValue<double>("invoicetotal"),
                            Items = new List<item>()
                        };
                    }
                    // Adiciona Itens
                    nota.nf.Items.Add(new item
                    {
                        InvoiceItemDiscount = row.GetValue<double>("invoiceitemdiscountvalue"),
                        QualificationName = row.GetValue<string>("qualificationname"),
                        InvoiceItemQuantity = row.GetValue<int>("invoiceitemquantity"),
                        ResourceName = row.GetValue<string>("resourcename"),
                        ServiceDescription = row.GetValue<string>("servicedescription"),
                        InvoiceItemSubTotal = row.GetValue<double>("invoiceitemsubtotal"),
                        InvoiceItemTaxValue = row.GetValue<double>("invoiceitemtaxvalue"),
                        InvoiceItemUnitValue = row.GetValue<double>("invoiceitemunitvalue")
                    });
                    contador++;
                }
            

            return nota;
        }

        private static string PreencheDados(int idNota)
        {
            //Busca nota pelo ID no CASSANDRA
            var nota = BuscaNota(idNota);
            
            // RENOMEIA OS CAMPOS NO TEXTO DO HTML
            string layout_item_pronto = string.Empty;

            foreach (var i in nota.nf.Items)
            {
                layout_item_pronto += nota.layout_items_nf
                    .Replace("{{serviceDescription}}", i.ServiceDescription).Trim()
                    .Replace("{{quantity}}", i.InvoiceItemQuantity.ToString("C")).Trim()
                    .Replace("{{unitValue}}", i.InvoiceItemUnitValue.ToString()).Trim()
                    .Replace("{{resourceName}}", i.ResourceName).Trim()
                    .Replace("{{qualificationName}}", i.QualificationName).Trim()
                    .Replace("{{qualificationName}}", i.QualificationName).Trim()
                    .Replace("{{taxes}}", i.InvoiceItemTaxValue.ToString("C")).Trim()
                    .Replace("{{discount}}", i.InvoiceItemDiscount.ToString("C")).Trim()
                    .Replace("{{subtotal}}", i.InvoiceItemSubTotal.ToString("C")).Trim()
                    .Replace(Environment.NewLine, "").Trim();
            }

            string layout_nf_pronto = nota.layout_nf
                .Replace("{{items}}", layout_item_pronto)
                .Replace("{{orderNo}}", nota.nf.InvoiceNumber.ToString())
                .Replace("{{customerName}}", nota.nf.CustomerName)
                .Replace("{{customerAddress}}", nota.nf.CustomerAddress)
                .Replace("{{totalAmount}}", nota.nf.InvoiceTotal.ToString("C"))
                .Replace(Environment.NewLine, "").Trim();

            return layout_nf_pronto;
        }

    }
}

