using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System;
using System.Data.SQLite;

namespace SGEA.Classes
{
    public class ClasseUsuario
    {
        private int cdUsuario;

        public ClasseUsuario(int cdUsuario)
        {
            this.cdUsuario = cdUsuario;
        }

        /// <summary>
        /// Método para Cadastrar Usuário
        /// </summary>
        /// <param name="nome">Nome</param>
        /// <param name="cep">Endereço</param>
        /// <param name="email">Email</param>
        /// <param name="sexo">Sexo</param>
        /// <param name="telFixo">Telefone Fixo</param>
        /// <param name="telCel">Celular</param>
        /// <param name="login">Login</param>
        /// <param name="senha">Senha</param>
        /// <param name="grupo">Grupo</param>
        public bool CadastrarUsuario(string nome, string cep, string bairro, string rua, string email, string sexo, string telFixo,
            string telCel, string login, string senha, string grupo)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    DataTable ds = new DataTable();
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        if (nome != "" && login != "" && senha != "" && email != "")
                        {
                            if (telFixo != "(00)0000-0000" || telCel != "(00)00000-0000")
                            {
                                string cmdText = "insert into tbUsuario values (null,@nm,@login,@pass,@email,@sexo,@grupo,@end,@bairro,@rua,@fixo,@cel)";
                                SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                                cmd.Parameters.AddWithValue("@login", login);
                                cmd.Parameters.AddWithValue("@nm", nome);
                                cmd.Parameters.AddWithValue("@end", cep);
                                cmd.Parameters.AddWithValue("@bairro", bairro);
                                cmd.Parameters.AddWithValue("@rua", rua);
                                cmd.Parameters.AddWithValue("@pass", senha);
                                cmd.Parameters.AddWithValue("@email", email);
                                cmd.Parameters.AddWithValue("@sexo", sexo);
                                cmd.Parameters.AddWithValue("@fixo", telFixo);
                                cmd.Parameters.AddWithValue("@cel", telCel);
                                cmd.Parameters.AddWithValue("@grupo", grupo);
                                cmd.ExecuteNonQuery();
                                Xceed.Wpf.Toolkit.MessageBox.Show("Usuário Cadastrado Com Sucesso!");
                                Historico.AdicionarHistorico(cdUsuario, "cadastrou", "um", "usuario");
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
                Xceed.Wpf.Toolkit.MessageBox.Show("Login e/ou E-Mail cadastrado");
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        /// <summary>
        /// Método para Alterar Usuário
        /// </summary>
        /// <param name="nome">Nome</param>
        /// <param name="cep">Endereço</param>
        /// <param name="telFixo">Telefone Fixo</param>
        /// <param name="telCel">Celular</param>
        /// <param name="loginN">Login Novo</param>
        /// <param name="loginA">Login Antigo</param>
        /// <param name="email">Email</param>
        /// <param name="sexo">Sexo</param>
        /// <param name="check">CheckBox Mudar Senha</param>
        /// <param name="senhaA">Senha Antiga</param>
        /// <param name="senhaN">Senha Nova</param>
        /// <param name="grupo">Grupo</param>
        /// <param name="ver">Verificação</param>
        public bool AlterarUsuario(string nome, string cep, string bairro, string rua, string telFixo, string telCel, string loginN, string loginA,
            string email, string sexo, bool? check, string senhaA, string senhaN, string grupo, bool ver)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    DataTable ds = new DataTable();
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        if (check == false)
                        {
                            if (loginN != "" && email != "" && nome != "")
                            {
                                if (telFixo != "(00)0000-0000" || telCel != "(00)00000-0000")
                                {
                                    string cmdText = "update tbUsuario set login = @login, sexo = @sexo, email = @email, nmUsuario = @nome, cep = @end, bairro = @bairro, rua = @rua, telFixo = @fixo, telCel = @cel, grupo = @grupo where login = '" + loginA + "'";
                                    SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                                    cmd.Parameters.AddWithValue("@login", loginN);
                                    cmd.Parameters.AddWithValue("@sexo", sexo);
                                    cmd.Parameters.AddWithValue("@nome", nome);
                                    cmd.Parameters.AddWithValue("@end", cep);
                                    cmd.Parameters.AddWithValue("@bairro", bairro);
                                    cmd.Parameters.AddWithValue("@rua", rua);
                                    cmd.Parameters.AddWithValue("@fixo", telFixo);
                                    cmd.Parameters.AddWithValue("@cel", telCel);
                                    cmd.Parameters.AddWithValue("@email", email);
                                    cmd.Parameters.AddWithValue("@grupo", grupo);
                                    cmd.ExecuteNonQuery();
                                    Xceed.Wpf.Toolkit.MessageBox.Show("Usuário Alterado Com Sucesso!");
                                    Historico.AdicionarHistorico(cdUsuario, "alterou", "o", "usuario");
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
                        else if (check == true)
                        {
                            if (nome != "" && email != "" && loginN != "" && senhaA != "" && senhaN != "")
                            {
                                if (ver == true)
                                {
                                    Criptografar c = new Criptografar();
                                    string senha = c.EncryptToString(senhaN);
                                    string cmdText = "update tbUsuario set login = @login, email = @email, nmUsuario = @nome, cep = @end, bairro = @bairro, rua = @rua, telFixo = @fixo, telCel = @cel, senha = @pass where login = '" + loginA + "'";
                                    SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                                    cmd.Parameters.AddWithValue("@login", loginN);
                                    cmd.Parameters.AddWithValue("@nome", nome);
                                    cmd.Parameters.AddWithValue("@pass", senha);
                                    cmd.Parameters.AddWithValue("@end", cep);
                                    cmd.Parameters.AddWithValue("@rua", rua);
                                    cmd.Parameters.AddWithValue("@bairro", bairro);
                                    cmd.Parameters.AddWithValue("@fixo", telFixo);
                                    cmd.Parameters.AddWithValue("@cel", telCel);
                                    cmd.Parameters.AddWithValue("@email", email);
                                    cmd.ExecuteNonQuery();
                                    Xceed.Wpf.Toolkit.MessageBox.Show("Usuário Alterado Com Sucesso!");
                                }
                                else
                                {
                                    Xceed.Wpf.Toolkit.MessageBox.Show("Senha incorreta");
                                }
                            }
                            else
                            {
                                Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter campos vazios");
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("UNIQUE"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Login e/ou E-Mail cadastrado");
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        public bool AlterarUsuario(string nome, string cep, string bairro, string rua, string telFixo, string telCel, string loginN, string loginA,
            string email, string sexo, bool? check, string senhaA, string senhaN, bool ver)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    DataTable ds = new DataTable();
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        if (check == false)
                        {
                            if (loginN != "" && email != "" && nome != "")
                            {
                                if (telFixo != "(00)0000-0000" || telCel != "(00)00000-0000")
                                {
                                    string cmdText = "update tbUsuario set login = @login, sexo = @sexo, email = @email, nmUsuario = @nome, cep = @end, bairro = @bairro, rua = @rua, telFixo = @fixo, telCel = @cel where login = '" + loginA + "'";
                                    SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                                    cmd.Parameters.AddWithValue("@login", loginN);
                                    cmd.Parameters.AddWithValue("@sexo", sexo);
                                    cmd.Parameters.AddWithValue("@nome", nome);
                                    cmd.Parameters.AddWithValue("@end", cep);
                                    cmd.Parameters.AddWithValue("@bairro", bairro);
                                    cmd.Parameters.AddWithValue("@rua", rua);
                                    cmd.Parameters.AddWithValue("@fixo", telFixo);
                                    cmd.Parameters.AddWithValue("@cel", telCel);
                                    cmd.Parameters.AddWithValue("@email", email);
                                    cmd.ExecuteNonQuery();
                                    Xceed.Wpf.Toolkit.MessageBox.Show("Usuário Alterado Com Sucesso!");
                                    Historico.AdicionarHistorico(cdUsuario, "alterou", "o", "usuario");
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
                        else if (check == true)
                        {
                            if (nome != "" && email != "" && loginN != "" && senhaA != "" && senhaN != "")
                            {
                                if (ver == true)
                                {
                                    Criptografar c = new Criptografar();
                                    string senha = c.EncryptToString(senhaN);
                                    string cmdText = "update tbUsuario set login = @login, email = @email, nmUsuario = @nome, cep = @end, bairro = @bairro, rua = @rua, telFixo = @fixo, telCel = @cel, senha = @pass where login = '" + loginA + "'";
                                    SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                                    cmd.Parameters.AddWithValue("@login", loginN);
                                    cmd.Parameters.AddWithValue("@nome", nome);
                                    cmd.Parameters.AddWithValue("@pass", senha);
                                    cmd.Parameters.AddWithValue("@end", cep);
                                    cmd.Parameters.AddWithValue("@bairro", bairro);
                                    cmd.Parameters.AddWithValue("@rua", rua);
                                    cmd.Parameters.AddWithValue("@fixo", telFixo);
                                    cmd.Parameters.AddWithValue("@cel", telCel);
                                    cmd.Parameters.AddWithValue("@email", email);
                                    cmd.ExecuteNonQuery();
                                    Xceed.Wpf.Toolkit.MessageBox.Show("Usuário Alterado Com Sucesso!");
                                }
                                else
                                {
                                    Xceed.Wpf.Toolkit.MessageBox.Show("Senha incorreta");
                                }
                            }
                            else
                            {
                                Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter campos vazios");
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("UNIQUE"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Login e/ou E-Mail cadastrado");
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        /// <summary>
        /// Método para Deletar Usuário
        /// </summary>
        /// <param name="c1">Código do Usuário</param>
        public void DeletarUsuario(int c1)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        MessageBoxResult dialogResult = MessageBox.Show("Você vai deletar um funcionário. Tem certeza?", "Aviso", MessageBoxButton.YesNo);
                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            string cmdText = "delete from tbUsuario where cdUsuario = @cd";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@cd", c1);
                            cmd.ExecuteNonQuery();
                            Xceed.Wpf.Toolkit.MessageBox.Show("Funcionário deletado.");
                            Historico.AdicionarHistorico(cdUsuario, "deletou", "o", "usuário", c1);
                        }
                    }
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("FOREIGN"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Esse usuário não pode ser deletado");
            }
        }
    }
}
