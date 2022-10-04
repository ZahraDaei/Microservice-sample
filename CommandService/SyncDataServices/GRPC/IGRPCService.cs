using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandService.Models;

namespace CommandService.SyncDataServices.GRPC
{
    public interface IGRPCService
    {
     Task<List<Platform>> GetAllPlatforms();   
    }
}