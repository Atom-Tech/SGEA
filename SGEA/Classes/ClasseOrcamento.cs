using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Win32;
using System.Globalization;
using System.Diagnostics;
using WPFCustomMessageBox;
using System.Data.SQLite;
using SGEA.Classes;

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
        public void ExportarRelatorio(List<string> nome, List<string> desc, List<string> tipo, List<string> imagem,
            List<double> larg, List<double> alt, List<int> quant, List<double> preco, string obs, string login,int q)
        {
            int v = 1;
            for (int i = 0; i < q; i++)
            {
                if (larg[i] == 0 || alt[i] == 0)
                {
                    v = 0;
                }
            }
            if (v == 1)
            {
                string c = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Orçamentos\\";
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
                Document pdf = new Document(PageSize.A4, 10, 10, 42, 35);
                PdfWriter.GetInstance(pdf, new FileStream(caminhoC, FileMode.Create));
                pdf.Open();
                PdfPTable table = new PdfPTable(2);
                pdf.Add(new Paragraph("Funcionário:   \t" + login));
                for (int i = 0; i < q; i++)
                {
                    PdfPCell cellBlankRow = new PdfPCell(new Phrase(" "));
                    cellBlankRow.Colspan = 3;
                    cellBlankRow.HorizontalAlignment = 1;
                    cellBlankRow.Border = 0;
                    table.AddCell(cellBlankRow);
                    if (imagem[i] != "Sem Imagem")
                    {
                        try
                        {
                            System.Drawing.Image im = System.Drawing.Image.FromFile(imagem[i]);
                            iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(im, System.Drawing.Imaging.ImageFormat.Jpeg);
                            pic.ScaleAbsolute(121, 121);
                            PdfPCell cell = new PdfPCell(pic);
                            cell.Colspan = 2;
                            cell.HorizontalAlignment = 1;
                            table.AddCell(cell);
                        }
                        catch
                        {
                            PdfPCell cell = new PdfPCell(new Phrase("Sem imagem"));
                            cell.Colspan = 2;
                            cell.HorizontalAlignment = 1;
                            table.AddCell(cell);
                        }
                    }
                    else
                    {
                        PdfPCell cell = new PdfPCell(new Phrase("Sem imagem"));
                        cell.Colspan = 2;
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);
                    }
                    table.AddCell(new Paragraph("Produto: "));
                    table.AddCell(new Paragraph(nome[i]));
                    table.AddCell(new Paragraph("Quantidade: "));
                    table.AddCell(new Paragraph("" + quant[i]));
                    table.AddCell(new Paragraph("Descrição: "));
                    table.AddCell(new Paragraph(desc[i]));
                    table.AddCell(new Paragraph("Tipo: "));
                    table.AddCell(new Paragraph(tipo[i]));
                    table.AddCell(new Paragraph("Medidas: "));
                    table.AddCell(new Paragraph(larg[i] + "x" + alt[i] + "m²"));
                    table.AddCell(new Paragraph("Preço Unitário: "));
                    table.AddCell(new Paragraph("" + preco[i]));
                    table.AddCell(new Paragraph("Preço Total do Produto: "));
                    table.AddCell(new Paragraph("" + (preco[i] * quant[i]) + "\n"));
                    precoT += (preco[i] * quant[i]);
                }
                pdf.Add(table);
                pdf.Add(new Paragraph("Preço Total do Orçamento:   \t" + precoT));
                pdf.Add(new Paragraph("Observações:   \t" + obs));
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
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Medidas não podem ser 0");
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
                        Historico.AdicionarHistorico(cdUsuario, "deletou", "o", "orcamento",cd);
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
