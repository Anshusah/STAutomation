using AutoMapper;
using Cicero.Data.Entities.JazzCash;
using Cicero.Service.Helpers;
using Cicero.Service.Models.JazzCash;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service
{
    public class JazzCashModelProfile : Profile
    {
        public JazzCashModelProfile()
        {
            CreateMap<Payee, PayeeViewModel>()
           .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
           .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<PayeeViewModel, Payee>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));
        }
    }
}
