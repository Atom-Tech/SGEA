using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Data.SqlClient;
using System;
using System.Data.SQLite;

namespace SGEA.Classes
{
    public class ClassePerfil
    {
        private int cdUsuario;

        public ClassePerfil(int cdUsuario)
        {
            this.cdUsuario = cdUsuario;
        }

        /// <summary>
        /// Método para Adicionar Perfil
        /// </summary>
        /// <param name="nome">Nome do Perfil</param>
        /// <param name="desc">Descrição do Perfil</param>
        public bool AdicionarPerfil(string nome, string desc)
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
                            string cmdText = "INSERT INTO tbPerfil VALUES (null,@nome, @desc)";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@nome", nome);
                            cmd.Parameters.AddWithValue("@desc", desc);
                            cmd.ExecuteNonQuery();
                            Xceed.Wpf.Toolkit.MessageBox.Show("Perfil Inserido Com Sucesso!");
                            Historico.AdicionarHistorico(cdUsuario, "inseriu", "um", "perfil");
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
                Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter mais de um perfil com o mesmo nome");
            }            
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        public void AdicionarPerfilProduto(int cdPerfil, string cdProduto)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "INSERT INTO tbPerfilProduto VALUES (null,@cdPerfil, @cdProduto)";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@cdPerfil", cdPerfil);
                        cmd.Parameters.AddWithValue("@cdProduto", cdProduto);
                        cmd.ExecuteNonQuery();
                        Historico.AdicionarHistorico(cdUsuario, "adicionou", "o", "perfil nº"+cdPerfil+" ao produto nº"+cdProduto);
                    }
                }
            }
            
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }
        public void AdicionarPerfilForn(int cdPerfil, string cdForn)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "INSERT INTO tbPerfilForn VALUES (null,@cdPerfil, @cdForn)";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@cdPerfil", cdPerfil);
                        cmd.Parameters.AddWithValue("@cdForn", cdForn);
                        cmd.ExecuteNonQuery();
                        Historico.AdicionarHistorico(cdUsuario, "adicionou", "o", "perfil nº" + cdPerfil + " ao fornecedor nº" + cdForn);
                    }
                }
            }
            
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        /// <summary>
        /// Método para Adicionar Perfil
        /// </summary>
        /// <param name="nome">Nome do Perfil</param>
        /// <param name="desc">Descrição do Perfil</param>
        public bool AlterarPerfil(int id, string nome, string desc)
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
                            string cmdText = "update tbPerfil set nmPerfil = @nome, dsPerfil = @desc where cdPerfil = " + id;
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@nome", nome);
                            cmd.Parameters.AddWithValue("@desc", desc);
                            cmd.ExecuteNonQuery();
                            Xceed.Wpf.Toolkit.MessageBox.Show("Perfil Alterado Com Sucesso!");
                            Historico.AdicionarHistorico(cdUsuario, "alterou", "o", "perfil", id);
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
                Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter mais de um perfil com o mesmo nome");
            }            
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        /// <summary>
        /// Método para Deletar Perfil do Fornecedor
        /// </summary>
        /// <param name="id">Código do Fornecedor</param>
        public void DeletarPerfilForn(int id)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "delete from tbPerfilForn where cdPF = @id";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        Historico.AdicionarHistorico(cdUsuario, "deletou", "um", "perfil de um fornecedor");
                    }
                }
            }
            
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        /// <summary>
        /// Método para Deletar Perfil do Produto
        /// </summary>
        /// <param name="id">Código do Produto</param>
        public void DeletarPerfilProduto(int id)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "delete from tbPerfilProduto where cdPP = @id";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        Historico.AdicionarHistorico(cdUsuario, "deletou", "um", "perfil de um produto");
                    }
                }
            }
            
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }


        /// <summary>
        /// Método para Deletar Perfil
        /// </summary>
        /// <param name="index">Código do Perfil</param>
        public void DeletarPerfil(int index)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "delete from tbPerfil where cdPerfil = @cd";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@cd", index);
                        cmd.ExecuteNonQuery();
                        Xceed.Wpf.Toolkit.MessageBox.Show("Perfil deletado com sucesso");
                        Historico.AdicionarHistorico(cdUsuario, "deletou", "o", "perfil", index);
                    }
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("FOREIGN"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Tem fornecedor e/ou produto cadastrado com esse perfil");
            }
            
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

    }
}
