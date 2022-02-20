using AutoMapper;
using estoque_tek.Models;
using estoque_tek.Web.Dtos;

namespace estoque_tek.Web.AutoMapper
{
    public class ContractorProfile : Profile
    {
        public ContractorProfile()
        {
            CreateMap<ContractorOutputModel, Contractor>();
            CreateMap<ContractorInputModel, Contractor>();
        }
    }
}
