using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.GRPC
{
    public class GRPCService : PlatformsService.PlatformsServiceBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;

        public GRPCService(IPlatformRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public override Task<PlatformResponse> GetAllPlatforms(PlatformRequest request, ServerCallContext context)
        {

            var platforms = _repository.GetAllPlatforms();
            var platformResponse = new PlatformResponse();
            foreach (var item in platforms)
            {
                platformResponse.Platforms.Add( _mapper.Map<PlatformModel>(item));
            }

            return Task.FromResult(platformResponse);
        }

    }
}