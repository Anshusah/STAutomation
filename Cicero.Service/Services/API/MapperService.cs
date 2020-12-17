using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cicero.Service.Models.SimpleTransfer.Onfido;
using Cicero.Service.Models;

namespace Cicero.Service.Services.API
{
    public interface IMapperService
    {
        CiceroStandardResponse MapToCiceroStandardResponse(GetApplicantsResponseViewModel response);
        CiceroStandardResponse MapToCiceroStandardResponse(dynamic response);

        dynamic Map<T>(dynamic model);
    }

    public class MapperService : IMapperService
    {
        private readonly IMapper mapper;
        public MapperService(IMapper _mapper)
        {
            mapper = _mapper;
        }

        public CiceroStandardResponse MapToCiceroStandardResponse(GetApplicantsResponseViewModel model)
        {
            return (mapper.Map<GetApplicantsResponseViewModel, CiceroStandardResponse>(model));

        }
        public CiceroStandardResponse MapToCiceroStandardResponse(dynamic model)
        {
            return (mapper.Map<dynamic, CiceroStandardResponse>(model));

        }

        public dynamic Map<T>(dynamic model)
        {
            return (mapper.Map<dynamic, T>(model));
        }
    }
}