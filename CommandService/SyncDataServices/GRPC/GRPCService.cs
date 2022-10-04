using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace CommandService.SyncDataServices.GRPC
{
    public class GRPCService : PlatformsService.PlatformsServiceClient,IGRPCService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public GRPCService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<List<Platform>> GetAllPlatforms()
        {

            // The port number must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress(_configuration["GRPCServerUrl"]);
            var client = new PlatformsService.PlatformsServiceClient(channel);
            var reply = await client.GetAllPlatformsAsync(new PlatformRequest());
            Console.WriteLine($"reply {reply}");

            var plat = new List<Platform>();
            foreach (var item in reply.Platforms)
            {
                plat.Add(_mapper.Map<Platform>(item));
            }

            return plat;
        }
    }
}