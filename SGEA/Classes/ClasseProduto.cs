using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Data.SQLite;
using SGEA.Classes;
using System.Collections.Generic;
using System.Diagnostics;

namespace SGEA
{
    public class ClasseProduto
    {
        private int cdUsuario;

        public string Nome { get; set; }

        public string Imagem { get; set; }

        public string Tipo { get; set; }

        public string Dimensao { get; set; }

        public double PrecoU { get; set; }

        public double PrecoT { get; set; }

        public string Descricao { get; set; }

        public int Quantidade { get; set; }

        public ClasseProduto()
        {

        }

        public ClasseProduto(int cdUsuario)
        {
            this.cdUsuario = cdUsuario;
        }
        
        /// <summary>
        /// Método para Cadastrar Produto sem Imagem
        /// </summary>
        /// <param name="nome">Nome do Produto</param>
        /// <param name="desc">Descrição do Produto</param>
        /// <param name="tipo">Tipo do Produto</param>
        /// <param name="preco">Preço por Metro Quadrado</param>
        /// <returns>Returna true se foi executado com sucesso</returns>
        public bool CadastrarProduto(string nome, string desc, string tipo, double preco)
        {
            try
            {
                using (var mConn = Connect.LiteString()) //Conecta no BD
                {
                    mConn.Open(); //Abre a Conexão
                    if (mConn.State == ConnectionState.Open) //Se a Conexão estiver aberta
                    {
                        //Checa para ver se os campos obrigatórios não estão nulos
                        if (nome != "" && desc != "")
                        {
                            string img = "Sem Imagem"; //Cadastra produto sem imagem
                            //Query de Insert
                            string cmdText = "insert into tbProduto values (null,@nome, @desc,@tipo,@img,@preco)";
                            //Instancia um Comando SQLite
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@nome", nome); //Insere nome
                            cmd.Parameters.AddWithValue("@desc", desc); //Insere descrição
                            cmd.Parameters.AddWithValue("@tipo", tipo); //Insere tipo
                            cmd.Parameters.AddWithValue("@img", img); //Insere imagem
                            cmd.Parameters.AddWithValue("@preco", preco); //Insere preço
                            cmd.ExecuteNonQuery(); //Executa o comando
                            //Exibe mensagem de inserido com sucesso
                            Xceed.Wpf.Toolkit.MessageBox.Show("Produto Inserido Com Sucesso!");
                            //Adiciona no Histórico
                            Historico.AdicionarHistorico(cdUsuario, "inseriu", "um", "produto");
                            //Retorna true
                            return true;
                        }
                        else
                        {
                            //Se algum campo obrigatório estiver nulo, ignora o insert
                            Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter campos vazios");
                        }
                    }
                }
            }
            //Se o produto já existir, ignora o insert
            catch (SQLiteException ex) when (ex.Message.Contains("UNIQUE"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter mais de um produto com o mesmo nome");
            }
            //Retorna false
            return false;
        }

        /// <summary>
        /// Método para Cadastrar Produto com Imagem
        /// </summary>
        /// <param name="nome">Nome</param>
        /// <param name="desc">Descrição</param>
        /// <param name="tipo">Tipo</param>
        /// <param name="imagem">Imagem</param>
        /// <param name="c">Caminho completo da Imagem</param>
        /// <param name="i">Nome da Imagem</param>
        public bool CadastrarProduto(string nome, string desc, string tipo,
            string imagem, string c, string i, double preco)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        if (nome != "" && desc != "")
                        {
                            string e = i.Substring((i.Length) - 4, 4);
                            string caminho = c + nome + e;
                            File.Copy(imagem, caminho, true);
                            string cmdText = "insert into tbProduto values (null,@nome, @desc,@tipo,@img,@preco)";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@nome", nome); //Insere nome
                            cmd.Parameters.AddWithValue("@desc", desc); //Insere descrição
                            cmd.Parameters.AddWithValue("@img", caminho); //Insere imagem
                            cmd.Parameters.AddWithValue("@tipo", tipo); //Insere tipo
                            cmd.Parameters.AddWithValue("@preco", preco); //Insere preço
                            cmd.ExecuteNonQuery();
                            Xceed.Wpf.Toolkit.MessageBox.Show("Produto Inserido Com Sucesso!");
                            Historico.AdicionarHistorico(cdUsuario, "inseriu", "um", "produto");
                            return true;
                        }
                        else
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter campos vazios");
                        }
                    }
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("UNIQUE"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter mais de um produto com o mesmo nome");
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        /// <summary>
        /// Método para Alterar Produto com Imagem
        /// </summary>
        /// <param name="index">Código do Produto</param>
        /// <param name="nome">Nome do Produto</param>
        /// <param name="text">Descrição do Produto</param>
        /// <param name="tipo">Tipo</param>
        /// <param name="imagem">Imagem</param>
        /// <param name="c">Caminho completo da imagem</param>
        /// <param name="i">Nome da Imagem</param>
        public bool AlterarProduto(long index, string nome, string text, string tipo, string imagem, string c, string i, double preco)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        if (nome != "" && text != "")
                        {
                            string e = i.Substring((i.Length) - 4, 4);
                            string caminho = c + nome + e;
                            if (imagem != caminho)
                            {
                                Wait.Waiting();
                                File.Copy(imagem, caminho, true);
                            }
                            string cmdText = "update tbProduto set nmProduto = @nome, dsProduto = @desc, imagem = @img, tpProduto = @tipo, preco = @preco where cdProduto = @cd";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@nome", nome);
                            cmd.Parameters.AddWithValue("@cd", index);
                            cmd.Parameters.AddWithValue("@desc", text);
                            cmd.Parameters.AddWithValue("@img", caminho);
                            cmd.Parameters.AddWithValue("@tipo", tipo);
                            cmd.Parameters.AddWithValue("@preco", preco);
                            cmd.ExecuteNonQuery();
                            Xceed.Wpf.Toolkit.MessageBox.Show("Produto Alterado Com Sucesso!");
                            Historico.AdicionarHistorico(cdUsuario, "alterou", "o", "produto", Convert.ToInt32(index));
                            return true;
                        }
                        else
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter campos vazios");
                        }
                    }
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("UNIQUE"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter mais de um produto com o mesmo nome");
            }
            return false;
        }
        
        /// <summary>
        /// Método para Alterar Produto sem Imagem
        /// </summary>
        /// <param name="index">Código do Produto</param>
        /// <param name="nome">Nome</param>
        /// <param name="text">Descrição</param>
        /// <param name="tipo">Tipo</param>
        public bool AlterarProduto(long index, string nome, string text, string tipo, double preco)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        if (nome != "" && text != "")
                        {
                            string imagem = "Sem Imagem";
                            string cmdText = "update tbProduto set nmProduto = @nome, dsProduto = @desc, imagem = @img, tpProduto = @tipo, preco = @preco where cdProduto = @cd";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@nome", nome);
                            cmd.Parameters.AddWithValue("@cd", index);
                            cmd.Parameters.AddWithValue("@desc", text);
                            cmd.Parameters.AddWithValue("@img", imagem);
                            cmd.Parameters.AddWithValue("@tipo", tipo);
                            cmd.Parameters.AddWithValue("@preco", preco);
                            cmd.ExecuteNonQuery();
                            Xceed.Wpf.Toolkit.MessageBox.Show("Produto Alterado Com Sucesso!");
                            Historico.AdicionarHistorico(cdUsuario, "alterou", "o", "produto", Convert.ToInt32(index));
                            return true;
                        }
                        else
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter campos vazios");
                        }
                    }
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("UNIQUE"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter mais de um produto com o mesmo nome");
            }
            return false;
        }

        /// <summary>
        /// Método para Deletar Produto
        /// </summary>
        /// <param name="index">Código do Produto</param>
        public void DeletarProduto(long index)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        //Deleta o produto selecionado
                        string cmdText = "delete from tbProduto where cdProduto = @cd";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@cd", index);
                        cmd.ExecuteNonQuery();
                        Xceed.Wpf.Toolkit.MessageBox.Show("Produto deletado com sucesso");
                        Historico.AdicionarHistorico(cdUsuario, "deletou", "o", "produto", Convert.ToInt32(index));
                    }
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("FOREIGN"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Tem orçamento e/ou perfil cadastrado com esse produto");
            }
        }

    }
}
