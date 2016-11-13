using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Data.SQLite;

namespace SGEA.Classes
{
    public class ClasseCliente
    {
        private int cdUsuario;

        public ClasseCliente(int cdUsuario)
        {
            this.cdUsuario = cdUsuario;
        }

        public bool CadastrarCliente(string n, string cep, string b, string r, string m, string s, string f, string c)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        if (f != "(00)0000-0000" || c != "(00)00000-0000")
                        {
                            string cmdText = "INSERT INTO tbCliente VALUES (null,@nome, @email,@sexo,@end,@bairro,@rua,@fixo,@cel)";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@nome", n); //Insere nome
                            cmd.Parameters.AddWithValue("@email", m); //Insere email
                            cmd.Parameters.AddWithValue("@end", cep); //Insere cep
                            cmd.Parameters.AddWithValue("@bairro", b); //Insere bairro
                            cmd.Parameters.AddWithValue("@rua", r); //Insere rua
                            cmd.Parameters.AddWithValue("@sexo", s); //insere sexo
                            cmd.Parameters.AddWithValue("@fixo", f); //Insere telefone fixo
                            cmd.Parameters.AddWithValue("@cel", c); //Insere celular
                            cmd.ExecuteNonQuery();
                            Xceed.Wpf.Toolkit.MessageBox.Show("Cliente Inserido Com Sucesso!");
                            Historico.AdicionarHistorico(cdUsuario, "inseriu", "um", "cliente");
                            return true;
                        }
                        else
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("É necessário pelo menos um telefone");
                        }
                    }
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("UNIQUE"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Nome de cliente e/ou e-mail repetido");
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        /// <summary>
        /// Método para Alterar Cliente
        /// </summary>
        /// <param name="cd">Código do Cliente</param>
        /// <param name="n">Nome do Cliente</param>
        /// <param name="email">Email do Cliente</param>
        /// <param name="sexo">Sexo do Cliente</param>
        /// <param name="end">Endereço do Cliente</param>
        /// <param name="tel">Telefone Fixo do Cliente</param>
        /// <param name="cel">Celular do Cliente</param>
        public bool AlterarCliente(int cd, string n, string email, string sexo, string cep, string b, string r, string tel, string cel)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        if (n != "")
                        {
                            if (tel != "(00)0000-0000" || cel != "(00)00000-0000")
                            {
                                string cmdText = "update tbCliente set nmCliente = @nome, email = @email, sexo = @sexo, cep = @end, bairro = @bairro, rua = @rua, telFixo = @fixo, telCel = @cel where cdCliente = @cd";
                                SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                                cmd.Parameters.AddWithValue("@nome", n);
                                cmd.Parameters.AddWithValue("@cd", cd);
                                cmd.Parameters.AddWithValue("@email", email);
                                cmd.Parameters.AddWithValue("@sexo", sexo);
                                cmd.Parameters.AddWithValue("@end", cep);
                                cmd.Parameters.AddWithValue("@bairro", b);
                                cmd.Parameters.AddWithValue("@rua", r);
                                cmd.Parameters.AddWithValue("@fixo", tel);
                                cmd.Parameters.AddWithValue("@cel", cel);
                                cmd.ExecuteNonQuery();
                                Xceed.Wpf.Toolkit.MessageBox.Show("Cliente Alterado Com Sucesso!");
                                Historico.AdicionarHistorico(cdUsuario, "alterou", "o", "cliente", cd);
                                return true;
                            }
                            else
                            {
                                Xceed.Wpf.Toolkit.MessageBox.Show("É necessário pelo menos um telefone");
                            }
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
                Xceed.Wpf.Toolkit.MessageBox.Show("Nome do cliente e/ou email repetido");
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        /// <summary>
        /// Método para Deletar Cliente
        /// </summary>
        /// <param name="c1">Código do Cliente</param>
        public void DeletarCliente(int c1)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Você vai deletar um cliente. Tem certeza?", "Aviso", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                try
                {
                    using (var mConn = Connect.LiteString())
                    {
                        mConn.Open();
                        if (mConn.State == ConnectionState.Open)
                        {
                            string cmdText = "delete from tbCliente where cdCliente = @cd";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@cd", c1);
                            cmd.ExecuteNonQuery();
                            Xceed.Wpf.Toolkit.MessageBox.Show("Cliente deletado com sucesso");
                            Historico.AdicionarHistorico(cdUsuario, "deletou", "o", "cliente", c1);
                        }
                    }
                }
                catch (SQLiteException ex) when (ex.Message.Contains("FOREIGN"))
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Tem Orçamento ou Visita agendada com esse cliente");
                }

                catch (Exception ex)
                {
                    Error.Erro(ex);
                }
            }
        }

    }
}
