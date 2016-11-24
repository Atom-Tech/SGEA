using iTextSharp.text;
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
            table.AddCell("Preço Total: ");
            double soma = produtos.Sum(p => p.PrecoT) +
                servicos.Sum(s => s.PrecoT);
            table.AddCell("R$" + string.Format("{0:N2}", soma));
            pdf.Add(table);
            var tP = new PdfPTable(1);
            tP.WidthPercentage = 100;
            var qtdProd = produtos.Count;
            var qtdSer = servicos.Count;
            if (qtdProd > 0)
            {
                Phrase phr = new Phrase("Produtos");
                phr.Font.Size = 18;
                var cellP = new PdfPCell(phr);
                cellP.Phrase.Font.SetStyle(Font.BOLD);
                cellP.Colspan = 7;
                cellP.Border = 0;
                cellP.HorizontalAlignment = Element.ALIGN_CENTER;
                var cellB = new PdfPCell(new Phrase(" "));
                cellB.Colspan = 7;
                cellB.HorizontalAlignment = 1;
                cellB.Border = 0;
                //TABELA PRODUTO
                var produto = new PdfPTable(7);
                produto.SpacingBefore = 10f;
                produto.WidthPercentage = 100;
                produto.AddCell(cellP);
                produto.AddCell(cellB);
                var head = new List<string>
                {
                    "Produto", "Tipo", "Dimensão", "Quantidade",
                    "Preço Unitário", "Preço Total", "Descrição"
                };
                foreach (var h in head)
                {
                    Phrase p1 = new Phrase(h);
                    p1.Font.SetStyle(Font.BOLD);
                    var cellH = new PdfPCell(p1);
                    cellH.BorderColor = new BaseColor(123, 160, 205);
                    cellH.BackgroundColor = new BaseColor(211, 223, 238);
                    produto.AddCell(cellH);
                }
                for (int i = 0; i < qtdProd; i++)
                {
                    var pP = new PdfPCell();
                    pP.BorderColor = new BaseColor(123, 160, 205);
                    pP.BackgroundColor = new BaseColor(167, 191, 222);
                    if (produtos[i].Imagem == "Sem Imagem")
                    {
                        pP.AddElement(new Paragraph("Sem Imagem"));
                    }
                    else
                    {
                        System.Drawing.Image sdi = System.Drawing.Image.FromFile(produtos[i].Imagem);
                        Image img = Image.GetInstance(sdi, ImageFormat.Jpeg);
                        img.ScaleAbsolute(64, 64);
                        pP.AddElement(img);
                    }
                    pP.AddElement(new Paragraph(""));
                    pP.AddElement(new Paragraph(produtos[i].Nome));
                    produto.AddCell(pP);
                    var celulas = new List<string>(){
                        produtos[i].Tipo,
                        produtos[i].Dimensao,
                        produtos[i].Quantidade.ToString(),
                        "R$" + string.Format("{0:N2}",produtos[i].PrecoU),
                        "R$" + string.Format("{0:N2}",produtos[i].PrecoT),
                        produtos[i].Descricao
                    };
                    foreach (var ce in celulas)
                    {
                        pP = new PdfPCell(new Phrase(ce));
                        pP.BorderColor = new BaseColor(123, 160, 205);
                        pP.BackgroundColor = new BaseColor(167, 191, 222);
                        produto.AddCell(pP);
                    }
                }
                var pP2 = new PdfPCell(new Phrase("TOTAL: "));
                pP2.HorizontalAlignment = Element.ALIGN_RIGHT;
                pP2.Phrase.Font.SetStyle(Font.BOLD);
                pP2.Colspan = 6;
                pP2.BorderColor = new BaseColor(123, 160, 205);
                pP2.BackgroundColor = new BaseColor(167, 191, 222);
                produto.AddCell(pP2);
                pP2.Phrase = new Phrase("R$" + string.Format("{0:N2}",
                    produtos.Sum(p => p.PrecoT)));
                produto.AddCell(pP2);
                pdf.Add(produto);
            }
            //SERVIÇOS
            if (qtdSer > 0)
            {
                Phrase phr = new Phrase("Serviços");
                phr.Font.Size = 18;
                var cellS = new PdfPCell(phr);
                cellS.Phrase.Font.SetStyle(Font.BOLD);
                cellS.Colspan = 3;
                cellS.Border = 0;
                cellS.HorizontalAlignment = Element.ALIGN_CENTER;
                var cellB = new PdfPCell(new Phrase(" "));
                cellB.Colspan = 3;
                cellB.HorizontalAlignment = 1;
                cellB.Border = 0;
                //TABELA PRODUTO
                var servico = new PdfPTable(3);
                servico.SpacingBefore = 10f;
                servico.WidthPercentage = 100;
                servico.AddCell(cellS);
                servico.AddCell(cellB);
                var head = new List<string>
                {
                    "Serviço", "Descrição", "Preço Total"
                };
                foreach (var h in head)
                {
                    Phrase p1 = new Phrase(h);
                    p1.Font.SetStyle(Font.BOLD);
                    var cellH = new PdfPCell(p1);
                    cellH.BorderColor = new BaseColor(123, 160, 205);
                    cellH.BackgroundColor = new BaseColor(211, 223, 238);
                    servico.AddCell(cellH);
                }
                for (int i = 0; i < qtdSer; i++)
                {
                    var celulas = new List<string>(){
                        servicos[i].Nome,
                        servicos[i].Descricao,
                        "R$" + string.Format("{0:N2}",servicos[i].PrecoT)
                    };
                    foreach (var ce in celulas)
                    {
                        var pP = new PdfPCell(new Phrase(ce));
                        pP.BorderColor = new BaseColor(123, 160, 205);
                        pP.BackgroundColor = new BaseColor(167, 191, 222);
                        servico.AddCell(pP);
                    }
                }
                var pP2 = new PdfPCell(new Phrase("TOTAL: "));
                pP2.HorizontalAlignment = Element.ALIGN_RIGHT;
                pP2.Phrase.Font.SetStyle(Font.BOLD);
                pP2.Colspan = 2;
                pP2.BorderColor = new BaseColor(123, 160, 205);
                pP2.BackgroundColor = new BaseColor(167, 191, 222);
                servico.AddCell(pP2);
                pP2.Phrase = new Phrase("R$" + string.Format("{0:N2}",
                    servicos.Sum(p => p.PrecoT)));
                pP2.HorizontalAlignment = Element.ALIGN_LEFT;
                servico.AddCell(pP2);
                pdf.Add(servico);
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
