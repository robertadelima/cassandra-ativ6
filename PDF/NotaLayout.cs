using System;
using System.Collections.Generic;

namespace cassandra_ativ6.Infra
{
    public class NotaLayout
    {
        public order nf { get; set; }

        public readonly string layout_items_nf = @"<tr class='item'><td>{{serviceDescription}}</td><td>{{quantity}}</td><td>{{unitValue}}</td>
            <td>{{resourceName}}</td><td>{{qualificationName}}</td><td>{{taxes}}</td><td>{{discount}}</td><td>{{subtotal}}</td></tr>";

        public readonly string layout_nf = @"
            <!doctype html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Invoice #: {{orderNo}}</title>
    
                <style>
                .invoice-box {
                    max-width: 800px;
                    margin: auto;
                    padding: 30px;
                    border: 1px solid #eee;
                    box-shadow: 0 0 10px rgba(0, 0, 0, .15);
                    font-size: 16px;
                    line-height: 24px;
                    font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
                    color: #555;
                }
    
                .invoice-box table {
                    width: 100%;
                    line-height: inherit;
                    text-align: left;
                }
    
                .invoice-box table td {
                    padding: 5px;
                    vertical-align: top;
                }
    
                .invoice-box table tr td:nth-child(2) {
                    text-align: right;
                }
    
                .invoice-box table tr.top table td {
                    padding-bottom: 20px;
                }
    
                .invoice-box table tr.top table td.title {
                    font-size: 45px;
                    line-height: 45px;
                    color: #333;
                }
    
                .invoice-box table tr.information table td {
                    padding-bottom: 40px;
                }
    
                .invoice-box table tr.heading td {
                    background: #eee;
                    border-bottom: 1px solid #ddd;
                    font-weight: bold;
                }
    
                .invoice-box table tr.details td {
                    padding-bottom: 20px;
                }
    
                .invoice-box table tr.item td{
                    border-bottom: 1px solid #eee;
                }
    
                .invoice-box table tr.item.last td {
                    border-bottom: none;
                }
    
                .invoice-box table tr.total td:nth-child(2) {
                    border-top: 2px solid #eee;
                    font-weight: bold;
                }
    
                @media only screen and (max-width: 600px) {
                    .invoice-box table tr.top table td {
                        width: 100%;
                        display: block;
                        text-align: center;
                    }
        
                    .invoice-box table tr.information table td {
                        width: 100%;
                        display: block;
                        text-align: center;
                    }
                }
    
                /** RTL **/
                .rtl {
                    direction: rtl;
                    font-family: Tahoma, 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
                }
    
                .rtl table {
                    text-align: right;
                }
    
                .rtl table tr td:nth-child(2) {
                    text-align: left;
                }
                </style>
            </head>

            <body>
                <div class='invoice-box'>
                    <table cellpadding = '0' cellspacing='0'>
                        <tr class='top'>
                            <td colspan = '8'>
                                <table>
                                    <tr>
                                        <td class='title'>
                                            <!-- logo da empresa -->
                                            <img src = 'https://www.sparksuite.com/images/logo.png' style='width:100%; max-width:300px;'>
                                        </td>
                            
                                        <td>
                                            Invoice #: {{orderNo}}<br>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
            
                        <tr class='information'>
                            <td colspan = '8' >
                                <table>
                                    <tr>
                                        <td>
                                            SegSoft, Inc.<br>
                                            Itajai, SC, n. 150<br>
                                            Brasil
                                        </td>
                            
                                        <td>
                                            {{customerName}}<br>
                                            {{customerAddress}}
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
            
            
                        <!-- ////// -->
                        <!-- cabecalho -->
                        <!-- ////// -->

                        <tr class='heading' >
                            <td>
                                Descrição
                            </td>
                            <td>
                                Qtd.
                            </td>
                            <td>
                                $ unit.
                            </td>
                            <td>
                                Recurso
                            </td>
                            <td>
                                Função
                            </td>
                            <td>
                                Taxas
                            </td>
                            <td>
                                Disc.
                            </td>
                            <td>
                                Subtot.
                            </td>
                        </tr>
            
                        <!-- ////// -->
                        <!-- itens -->
                        <!-- ////// -->
            
                        {{items}}
            
                        <!-- ////// -->
                        <!-- total -->
                        <!-- ////// -->

                        <tr class='total' colspan='8' align='left'>
                            <td>
                               Total: {{totalAmount}}
                            </td>
                        </tr>
                    </table>
                </div>
            </body>
            </html>";


        public class order
        {
            /// <summary>
            /// Numero da nota
            /// </summary>
            public int InvoiceNumber { get; set; }
            /// <summary>
            /// Nome do cliente
            /// </summary>
            public string CustomerName { get; set; }
            /// <summary>
            /// Endereço do cliente
            /// </summary>
            public string CustomerAddress { get; set; }
            /// <summary>
            /// Itens da nota
            /// </summary>
            public List<item> Items { get; set; }
            /// <summary>
            /// Valor total da nota
            /// </summary>
            public double InvoiceTotal { get; set; }
        }

        public class item
        {
            /// <summary>
            /// Número do item
            /// </summary>
            public int InvoiceItemID { get; set; }
            /// <summary>
            /// Descrição do serviço
            /// </summary>
            public string ServiceDescription { get; set; }
            /// <summary>
            /// Quantidade
            /// </summary>
            public int InvoiceItemQuantity { get; set; }
            /// <summary>
            /// Valor unitário
            /// </summary>
            public double InvoiceItemUnitValue { get; set; }
            /// <summary>
            /// Nome do recurso que prestou o serviço
            /// </summary>
            public string ResourceName { get; set; }
            /// <summary>
            /// Função do recurso que prestou o serviço
            /// </summary>
            public string QualificationName { get; set; }
            /// <summary>
            /// Taxa/impostos
            /// </summary>
            public double InvoiceItemTaxValue { get; set; }
            /// <summary>
            /// Desconto
            /// </summary>
            public double InvoiceItemDiscount { get; set; }
            /// <summary>
            /// Subtotal
            /// </summary>
            public double InvoiceItemSubTotal { get; set; }
        }
    }
}
