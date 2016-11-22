﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Globalization;
using System.Diagnostics;
using WPFCustomMessageBox;
using System.Data.SQLite;
using SGEA.Classes;
using SGEA.Properties;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Reflection;
using iTextSharp.text.pdf.draw;
using System.Linq;

namespace SGEA
{
    public class ClasseOrcamento
    {
        private int cdUsuario;
        private List<string> nome = new List<string>(), desc = new List<string>(), tipo = new List<string>(), imagem = new List<string>();
        private List<double> larg = new List<double>(), alt = new List<double>(), preco = new List<double>();
        private List<int> quant = new List<int>(), cd = new List<int>();

        public ClasseOrcamento(int cdUsuario)
        {
            this.cdUsuario = cdUsuario;
        }

        /// <summary>
        /// Método para Armazenar o Orçamento
        /// </summary>
        /// <param name="index">Código do Orçamento</param>
        /// <param name="q">Quantidade de Produtos/Serviços</param>
        public void ArmazenarRelatorio(List<int> cd, List<string> nome, List<string> desc, List<string> tipo, List<string> imagem,
            List<double> larg, List<double> alt, List<int> quant, List<double> preco, string obs, int index, int q)
        {
            int x = 0;
            using (var mConn = Connect.LiteString())
            {
                mConn.Open();
                if (mConn.State == ConnectionState.Open)
                {
                    for (int i = 0; i < q; i++)
                    {
                        if (larg[i] == 0 || alt[i] == 0)
                        {
                            x++;
                        }
                    }
                    if (x == 0)
                    {
                        for (int i = 0; i < q; i++)
                        {
                            string cmdText = "INSERT INTO tbItemNota VALUES (null,@id,@cdP,@cdS,@larg,@alt,@quant,@preco)";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@id", index);
                            if (tipo[i] == "Serviço")
                            {
                                cmd.Parameters.AddWithValue("@cdP", null);
                                cmd.Parameters.AddWithValue("@cdS", cd[i]);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@cdS", null);
                                cmd.Parameters.AddWithValue("@cdP", cd[i]);
                            }
                            cmd.Parameters.AddWithValue("@larg", larg[i]);
                            cmd.Parameters.AddWithValue("@alt", alt[i]);
                            cmd.Parameters.AddWithValue("@quant", quant[i]);
                            cmd.Parameters.AddWithValue("@preco", preco[i]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Medidas não podem ser 0");
                    }
                }
            }
        }

        /// <summary>
        /// Método para Exportar Orçamento
        /// </summary>
        /// <param name="q">Quantidade de Produtos/Serviços</param>
        public void ExportarRelatorio(List<ClasseProduto> produtos, List<Servicos> servicos, DateTime data,
            DateTime dataV, string cd, string cliente, string obs, string login, int q)
        {
            var con = Connect.LiteConnection("select * from tbCliente where cdCliente = " + cliente);
            var row = con.Rows[0];
            string nmCliente = row.ItemArray[1].ToString();
            string cidade = row.ItemArray[4].ToString().getCidade();
            string bairro = row.ItemArray[5].ToString();
            string rua = row.ItemArray[6].ToString() + " nº " + row.ItemArray[7].ToString();
            string tel = row.ItemArray[8].ToString() + " / " + row.ItemArray[9].ToString();
            string c = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SGEA\\Orçamentos\\";
            Directory.CreateDirectory(c);
            double precoT = 0;
            string dia = DateTime.Now.Day.ToString();
            string mes = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DateTime.Now.ToString("MMMM"));
            string ano = DateTime.Now.Year.ToString();
            string hora = DateTime.Now.Hour.ToString();
            string min = DateTime.Now.Minute.ToString();
            SaveFileDialog save = new SaveFileDialog();
            string caminho = c + mes + " de " + ano + "\\";
            Directory.CreateDirectory(caminho);
            save.FileName = "Orçamento Dia " + dia + " " + hora + "_" + min + ".pdf";
            save.Filter = "PDF Files| *.pdf";
            string caminhoC = caminho + save.FileName;
            Document pdf = new Document(PageSize.A4, 10, 10, 130, 35);
            PdfWriter w = PdfWriter.GetInstance(pdf, new FileStream(caminhoC, FileMode.Create));
            w.PageEvent = new HeaderFooter(data.Date.ToShortDateString());
            pdf.Open();
            //PDF ABERTO
            PdfPTable table = new PdfPTable(2);
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            //TABELA
            table.AddCell("Orçamento nº " + cd);
            table.AddCell("");
            table.AddCell("Data de Validade: ");
            table.AddCell(dataV.Date.ToShortDateString());
            table.AddCell("Usuário: ");
            table.AddCell(login);
            table.AddCell("Cliente: ");
            table.AddCell(nmCliente);
            table.AddCell("Cidade: ");
            table.AddCell(cidade);
            table.AddCell("Bairro: ");
            table.AddCell(bairro);
            table.AddCell("Rua: ");
            table.AddCell(rua);
            table.AddCell("Telefone: ");
            table.AddCell(tel);
            pdf.Add(table);
            var tP = new PdfPTable(1);
            tP.WidthPercentage = 100;
            var qtdProd = produtos.Count;
            if (qtdProd > 0)
            {
                Phrase phr = new Phrase("Produtos");
                phr.Font.Size = 14;
                var cellP = new PdfPCell(phr);
                cellP.Colspan = 7;
                cellP.Border = 0;
                cellP.HorizontalAlignment = Element.ALIGN_CENTER;
                var cellB = new PdfPCell();
                cellB.Colspan = 7;
                cellB.Border = 0;
                //TABELA PRODUTO
                var produto = new PdfPTable(7);
                produto.SpacingBefore = 10f;
                produto.WidthPercentage = 100;
                produto.AddCell(cellP);
                produto.AddCell(cellB);
                produto.AddCell("Produto");
                produto.AddCell("Tipo");
                produto.AddCell("Dimensão");
                produto.AddCell("Quantidade");
                produto.AddCell("Preço Unitário");
                produto.AddCell("Preço Total");
                produto.AddCell("Descrição");
                for (int i = 0; i < qtdProd; i++)
                {
                    var pP = new PdfPCell();
                    if (produtos[i].Imagem == "Sem Imagem")
                    {
                        pP.AddElement(new Paragraph("Sem Imagem"));
                    }
                    else
                    {
                        System.Drawing.Image sdi = System.Drawing.Image.FromFile(produtos[i].Imagem);
                        Image img = Image.GetInstance(sdi,ImageFormat.Jpeg);
                        img.ScaleAbsolute(64, 64);
                        pP.AddElement(img);
                    }
                    pP.AddElement(new Paragraph(""));
                    pP.AddElement(new Paragraph(produtos[i].Nome));
                    produto.AddCell(pP);
                    produto.AddCell(produtos[i].Tipo);
                    produto.AddCell(produtos[i].Dimensao);
                    produto.AddCell(produtos[i].Quantidade.ToString());
                    produto.AddCell(produtos[i].PrecoU.ToString());
                    produto.AddCell(produtos[i].PrecoT.ToString());
                    produto.AddCell(produtos[i].Descricao);
                }
                pdf.Add(produto);
            }
            //PDF FECHADO
            pdf.Close();
            MessageBoxResult r = CustomMessageBox.ShowYesNoCancel("PDF foi gerado na pasta \n" + caminho + "\nVocê deseja abrir o arquivo ou a pasta?", "Orçamento", "Arquivo", "Pasta", "Nenhum", MessageBoxImage.Question);
            switch (r)
            {
                case MessageBoxResult.Yes:
                    Process.Start(caminhoC);
                    break;
                case MessageBoxResult.No:
                    Process.Start(caminho);
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        /// <summary>
        /// Método para Deletar Orçamento
        /// </summary>
        /// <param name="cd">Código do Orçamento</param>
        public void DeletarOrcamento(int cd)
        {
            try
            {
                DeletarFK(cd);
                DeletarFK2(cd);
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "delete from tbOrcamento where cdOrcamento = @cd";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@cd", cd);
                        cmd.ExecuteNonQuery();
                        Xceed.Wpf.Toolkit.MessageBox.Show("Orçamento Deletado Com Sucesso!");
                        Historico.AdicionarHistorico(cdUsuario, "deletou", "o", "orcamento", cd);
                    }
                }
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        /// <summary>
        /// Método Auxiliar do Método Deletar Orçamento
        /// </summary>
        /// <param name="cd">Código do Orçamento</param>
        public void DeletarFK(int cd)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "delete from tbItemNota where idOrcamento = @cd";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@cd", cd);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        /// <summary>
        /// Método Auxiliar do Método Deletar Orçamento
        /// </summary>
        /// <param name="cd">Código do Orçamento</param>
        public void DeletarFK2(int cd)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "delete from tbProjeto where idOrcamento = @cd";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@cd", cd);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

    }
}
