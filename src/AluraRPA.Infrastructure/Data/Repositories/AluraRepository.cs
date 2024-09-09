using AluraRPA.Domain.Entities;
using System.Data;
using System.Data.SqlClient;

namespace AluraRPA.Infrastructure.Data.Repositories;
public class AluraRepository : Repository, IAluraRepository
{
    private ILogger<AluraRepository> _logger { get; set; }
    public AluraRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<bool> InsertData(List<DataExtracted> dataExtracted, CancellationToken ct = default)
    {
        try
        {

            using (var conn = new SqlConnection(ConnectionString))
            {
                var sql = $@"insert into 
                                	[DESAFIO].[TB_DADOS]
                                	(
                                		[vcTitulo]
                                		,[vcProfessor]
                                		,[vcCargaHoraria]
                                		,[vcDescricao]
                                	
                                	)
                                values
                                	(
                                		@vcTitulo --[vcTitulo]
                                		,@vcProfessor --,[vcProfessor]
                                		,@vcCargaHoraria --,[vcCargaHoraria]
                                		,@vcDescricao --,[vcDescricao]
                                	)";


                conn.Open();

                SqlCommand commandInsert = new SqlCommand(sql, conn);

                commandInsert.Parameters.Add("@vcTitulo", SqlDbType.VarChar);
                commandInsert.Parameters.Add("@vcProfessor", SqlDbType.VarChar);
                commandInsert.Parameters.Add("@vcCargaHoraria", SqlDbType.VarChar);
                commandInsert.Parameters.Add("@vcDescricao", SqlDbType.VarChar);

                foreach (var item in dataExtracted)
                {
                    commandInsert.Parameters["@vcTitulo"].Value = item.titulo.ToString();
                    commandInsert.Parameters["@vcProfessor"].Value = item.professor.ToString();
                    commandInsert.Parameters["@vcCargaHoraria"].Value = item.cargaHoraria.ToString();
                    commandInsert.Parameters["@vcDescricao"].Value = item.descricao.ToString();
                }

                await commandInsert.ExecuteScalarAsync(ct);

                conn.Close();

                return true;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;

        }
    }

    public async Task<Credential> GetCredential(CancellationToken ct = default)
    {
        try
        {
            var sql = $@"select 
                            	vcUsuario
                            	,vcSenha
                            from 
                            	[DESAFIO].[TB_ACESSO]
                            where
                            	biAtivo = 1";

            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(sql, connection);

            await connection.OpenAsync(ct);
            using var reader = await command.ExecuteReaderAsync(ct);

            if (reader.Read())
                return new
                (
                    User: reader[0].ToString(),
                    Password: reader[1].ToString()
                );
            return null;
        }
        catch (Exception ex)
        {

            return null;
        }
    }

  
}