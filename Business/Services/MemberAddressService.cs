//using Business.Dtos;
//using Business.Models;
//using Data.Interfaces;
//using Data.Repositories;
//using Domain.Models;
//using Microsoft.IdentityModel.Tokens;

//namespace Business.Services;

//public class MemberAddressService(IMemberAddressRepository memberAddressRepository)
//{
//    private readonly IMemberAddressRepository _memberAddressRepository = memberAddressRepository;

//    // CREATE
//    public async Task<MemberAddressResult<MemberAddressModel>> CreateMemberAddressAsync(MemberAddressForm form, string userId)
//    {
//        if (form == null || userId.IsNullOrEmpty())
//            return MemberAddressResult<MemberAddressModel>.BadRequest("Parameters cannot be null/empty.");

//        var exists = (await _memberAddressRepository.ExistsAsync(x => x.UserId == userId));
//        if (exists.StatusCode == 204)
//        {
//            Console.WriteLine($"Address for member with userid {userId} alredy exists.");
//        }
//    }
//}
