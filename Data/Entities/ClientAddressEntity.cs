using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ClientAddressEntity
{
    [Key]
    public int ClientId { get; set; }
    public ClientEntity? Client { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string StreetAddress { get; set; } = null!;

    [ProtectedPersonalData]
    public int PostalCode { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(20)")]
    public string City { get; set; } = null!;

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(60)")]
    public string Country { get; set; } = null!;
}
