using AtividadeCity.Models;
using MySql.Data.MySqlClient;
using System.Data;
namespace AtividadeCity.Repositorio
{
    public class ProdutoRepositorio(IConfiguration configuration)
    {
        
            private readonly string _conexaoMySql = configuration.GetConnectionString("conexaoMySql");

            public void Cadastrar(Produto produto)
            {
                using (var conexao = new MySqlConnection(_conexaoMySql))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("insert into tbProdutos(Nome, Descricao, Preco, Quantidade) values(@nome, @descricao, @preco, @quantidade)", conexao);
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.Nome;
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.Preco;
                    cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                    cmd.ExecuteNonQuery();
                    conexao.Close();
                }
            }

            public bool Atualizar(Produto produto)
            {
                try
                {
                    using (var conexao = new MySqlConnection(_conexaoMySql))
                    {
                        conexao.Open();
                        MySqlCommand cmd = new MySqlCommand("update tbProdutos set Nome=@nome, Descricao=@descricao, Preco=@preco, Quantidade=@quantidade" + " where Id = @id ", conexao);
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = produto.Id;
                        cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.Nome;
                        cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                        cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.Preco;
                        cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                        int linhasAfetadas = cmd.ExecuteNonQuery();
                        return linhasAfetadas > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao Atualizar o Produto: {ex.Message}");
                    return false;
                }
            }

            public IEnumerable<Produto> TodosProdutos()
            {
                List<Produto> produtos = new List<Produto>();

                using (var conexao = new MySqlConnection(_conexaoMySql))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("Select * from tbProdutos", conexao);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    conexao.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                        produtos.Add(
                                new Produto
                                {
                                    Id = Convert.ToInt32(dr["Id"]),
                                    Nome = ((string)dr["Nome"]),
                                    Descricao = ((string)dr["Descricao"]),
                                    Preco = Convert.ToDecimal(dr["Preco"]),
                                    Quantidade = Convert.ToInt32(dr["Quantidade"])
                                });
                    }
                    return produtos;
                }
            }

            public Produto ObterProduto(int id)
            {
                using (var conexao = new MySqlConnection(_conexaoMySql))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("Select * from tbProdutos where Id = @id", conexao);
                    cmd.Parameters.AddWithValue("id", id);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    MySqlDataReader dr;
                    Produto produto = new Produto();

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        produto.Id = Convert.ToInt32(dr["Id"]);
                        produto.Nome = (string)(dr["Nome"]);
                        produto.Descricao = (string)(dr["Descricao"]);
                        produto.Preco = Convert.ToDecimal(dr["Preco"]);
                        produto.Quantidade = Convert.ToInt32(dr["Quantidade"]);
                    }
                    return produto;
                }
            }

            public void Excluir(int Id)
            {
                using (var conexao = new MySqlConnection(_conexaoMySql))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("delete from tbProdutos where Id = @id", conexao);
                    cmd.Parameters.AddWithValue("id", Id);
                    int i = cmd.ExecuteNonQuery();
                    conexao.Close();
                }
            }
        }
    }