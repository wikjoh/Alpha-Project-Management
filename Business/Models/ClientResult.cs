using Domain.Models;

namespace Business.Models;

public class ClientResult : ServiceResult
{
    public IEnumerable<ClientModel>? Data { get; set; }
}
