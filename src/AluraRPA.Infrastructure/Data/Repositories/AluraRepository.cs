using System.Data;
using System.Data.SqlClient;

namespace AluraRPA.Infrastructure.Data.Repositories;
public class AluraRepository : Repository,IAluraRepository
{
    public AluraRepository(IConfiguration configuration) : base(configuration) { }
   
}