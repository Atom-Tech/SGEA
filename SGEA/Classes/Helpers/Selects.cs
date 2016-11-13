using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static SGEA.Connect;

namespace SGEA
{
    public static class Selects
    {
        public static DataGrid SelectProjetoAtrasado(this DataGrid dg)
        {
            dg.DataContext = LiteConnection(
                "select * from vwProjetoAtrasado");
            return dg;
        }
        public static DataGrid SelectHistoricoHoje(this DataGrid dg)
        {
            dg.DataContext = LiteConnection(
                "select * from vwHistoricoHoje");
            dg.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            return dg;
        }
        public static DataGrid SelectNotificacaoProjetos(this DataGrid dg)
        {
            dg.DataContext = LiteConnection(
                "select cdOrcamento, date(dtValidade) from tbOrcamento "+
                "where date(dtValidade) < date('now') and idExecucao = 0");
            return dg;
        }
        public static string SelectUsuarioLogin(int cd) =>
            "select login from tbUsuario where cdUsuario = " + cd;
        public static DataGrid SelectInformacoesUsuario(this DataGrid dg, string login, string senha)
        {
            dg.DataContext = LiteConnection(
                "Select cdUsuario, login, senha, email, grupo, nmUsuario, " +
                " cep, bairro, rua, telFixo, telCel, sexo from tbUsuario " +
                "where login = '" + login + "' and senha = '" + senha + "'");
            return dg;
        }
        public static DataGrid SelectDadosUsuario(this DataGrid dg, string cd)
        {
            dg.DataContext = LiteConnection(
                "select login, nmUsuario, cep, bairro, rua, telFixo,"+
                " telCel from tbUsuario where login = '" + cd + "'");
            return dg;
        }
        public static DataGrid SelectVisitasAntigas(this DataGrid dg)
        {
            dg.DataContext = LiteConnection(
                "select * from tbAgenda where dtVisita < date('now') and idExecucao = 0");
            return dg;
        }
        public static DataGrid SelectServico(this DataGrid dg)
        {
            dg.DataContext = LiteConnection("Select cdServico 'Código', nmServico 'Serviço', dsServico 'Descrição' from tbServico order by nmServico asc");
            return dg;
        }
        public static DataGrid SelectProduto(this DataGrid dg, string cdP)
        {
            dg.DataContext = LiteConnection("Select cdProduto 'Código', nmProduto 'Produto', dsProduto 'Descrição', imagem, nmTipo, preco 'Preço m²' from tbProduto, tbTipo where tpProduto = cdTipo and tpProduto = " + cdP + " order by nmProduto asc");
            dg.Columns[3].Visibility = Visibility.Collapsed;
            dg.Columns[4].Visibility = Visibility.Collapsed;
            return dg;
        }
        public static DataGrid SelectUltimoOrcamento(this DataGrid dg)
        {
            dg.DataContext = LiteConnection("Select cdOrcamento from tbOrcamento order by cdOrcamento desc");
            return dg;
        }
    }
}
