
using AutoMapper;
using CustomerManagement.Core.Requests.Customer;
using CustomerManagement.Models;

namespace CustomerManagement.Core.Mappers;

public class CustomerMapper : Profile
{
    public CustomerMapper()
    {
        CreateMap<CreateCustomerRequest, Customer>();

        //CreateMap<PaginatedList<Person>, PaginatedList<PersonDto>>()
        //   .ForMember(dest => dest.List, opt => opt.MapFrom(src => src));
    }
}
