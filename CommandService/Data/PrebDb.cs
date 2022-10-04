using System;
using CommandService.Data;
using CommandService.SyncDataServices.GRPC;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CommandService.Data
{
    public static class PrebDb
    {
       

        public static async void RegisterPlatforms(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
              var _grpcService  =scope.ServiceProvider.GetRequiredService<IGRPCService>();
              var platforms=await _grpcService.GetAllPlatforms();

              foreach (var item in platforms)
              {
                  RegisterAndCheckPlatform(app,item);
              }                
            };
            
        }

        private static void RegisterAndCheckPlatform(IApplicationBuilder app, Models.Platform item)
        {
            using(var scope=app.ApplicationServices.CreateScope()){
                var _repository=scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                if(!_repository.ExternalPlatformExist(item.ExternalID)){
                    _repository.CreatePlatform(item);
                    _repository.SaveChanges();
                }            
            }
        }
    }
}