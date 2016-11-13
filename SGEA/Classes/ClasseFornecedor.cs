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
    public class ClasseFornecedor
    {
        private int cdUsuario;

        public ClasseFornecedor(int cdUsuario)
        {
            this.cdUsuario = cdUsuario;
        }

        public bool CadastrarFornecedor(string n, string e, string b, string r, string m, string f, string c, string cn, string rs)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        if (n != "" && e != "")
                        {
                            if (f != "(00)0000-0000" || c != "(00)00000-0000")
                            {
                                string cmdText = "INSERT INTO tbFornecedor VALUES (null,@cnpj,@nome,@rs, @email,@end,@bairro,@rua,@fixo,@cel)";
                                SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                                cmd.Parameters.AddWithValue("@cnpj", cn); //Insere CNPJ
                                cmd.Parameters.AddWithValue("@nome", n); //Insere nome
                                cmd.Parameters.AddWithValue("@rs", rs); //Insere Razão Social
                                cmd.Parameters.AddWithValue("@email", m); //Insere email
                                cmd.Parameters.AddWithValue("@end", e); //Insere cep
                                cmd.Parameters.AddWithValue("@bairro", b); //Insere bairro
                                cmd.Parameters.AddWithValue("@rua", r); //Insere rua
                                cmd.Parameters.AddWithValue("@fixo", f); //Insere telefone fixo
                                cmd.Parameters.AddWithValue("@cel", c); //Insere celular
                                cmd.ExecuteNonQuery();
                                Xceed.Wpf.Toolkit.MessageBox.Show("Fornecedor Inserido Com Sucesso!");
                                Historico.AdicionarHistorico(cdUsuario, "inseriu", "um", "fornecedor");
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
            catch (SQLiteException ex) when (ex.Message.Contains("NULL"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter campos vazios");
            }
            catch (SQLiteException ex) when (ex.Message.Contains("UNIQUE"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Nome Fantasia, CNPJ e/ou E-Mail repetido");
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        /// <summary>
        /// Método para Alterar Fornecedor
        /// </summary>
        /// <param name="cd">Código do Fornecedor</param>
        /// <param name="cnpj">CNPJ do Fornecedor</param>
        /// <param name="n">Nome Fantasia</param>
        /// <param name="rs">Razão Social</param>
        /// <param name="email">Email do Fornecedor</param>
        /// <param name="end">Endereço</param>
        /// <param name="tel">Telefone Fixo</param>
        /// <param name="cel">Celular</param>
        public bool AlterarFornecedor(int cd, string cnpj, string n, string rs, string email, string cep, string bairro, string rua, string tel, string cel)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        if (n != "" && email != "")
                        {
                            if (tel != "(00)0000-0000" || cel != "(00)00000-0000")
                            {
                                string cmdText = "update tbFornecedor set nmFornecedor = @nome, email = @email, cep = @cep, bairro = @bairro, rua = @rua, telFixo = @fixo, telCel = @cel, cnpj = @cnpj, razaoSocial = @rs where cdFornecedor = @cd";
                                SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                                cmd.Parameters.AddWithValue("@nome", n);
                                cmd.Parameters.AddWithValue("@cd", cd);
                                cmd.Parameters.AddWithValue("@email", email);
                                cmd.Parameters.AddWithValue("@cep", cep);
                                cmd.Parameters.AddWithValue("@bairro", bairro);
                                cmd.Parameters.AddWithValue("@rua", rua);
                                cmd.Parameters.AddWithValue("@fixo", tel);
                                cmd.Parameters.AddWithValue("@cel", cel);
                                cmd.Parameters.AddWithValue("@cnpj", cnpj);
                                cmd.Parameters.AddWithValue("@rs", rs);
                                cmd.ExecuteNonQuery();
                                Xceed.Wpf.Toolkit.MessageBox.Show("Fornecedor Alterado Com Sucesso!");
                                Historico.AdicionarHistorico(cdUsuario, "alterou", "o", "fornecedor", cd);
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
                Xceed.Wpf.Toolkit.MessageBox.Show("Nome Fantasia, CNPJ e/ou email repetido");
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        /// <summary>
        /// Método para Deletar Fornecedor
        /// </summary>
        /// <param name="c1">Código do Fornecedor</param>
        public void DeletarFornecedor(int c1)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Você vai deletar um Fornecedor. Tem certeza?", "Aviso", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                try
                {
                    using (var mConn = Connect.LiteString())
                    {
                        mConn.Open();
                        if (mConn.State == ConnectionState.Open)
                        {
                            string cmdText = "delete from tbFornecedor where cdFornecedor = @cd";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@cd", c1);
                            cmd.ExecuteNonQuery();
                            Xceed.Wpf.Toolkit.MessageBox.Show("Fornecedor deletado com sucesso");
                            Historico.AdicionarHistorico(cdUsuario, "deletou", "o", "fornecedor", c1);
                        }
                    }
                }
                catch (SQLiteException ex) when (ex.Message.Contains("FOREIGN"))
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Tem Perfil cadastrado com esse Fornecedor");
                }

                catch (Exception ex)
                {
                    Error.Erro(ex);
                }
            }
        }
    }
}
