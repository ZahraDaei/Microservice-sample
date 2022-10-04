using System;
using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceProvider serviceProvider,IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper=mapper;
        }
        public void ProcessEvent(string message)
        {
            var result = GetEventType(message);
            var platformReadDto = JsonSerializer.Deserialize<PlatformReadDto>(message);
            Console.WriteLine($"result,plafromReadDto.name       {result},{platformReadDto.Name}");

            if (result)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                    var isPlatformExist = service.ExternalPlatformExist(platformReadDto.Id);
                    var platform=new Platform(){ExternalID=platformReadDto.Id, Name=platformReadDto.Name};
                    service.CreatePlatform(platform);
                    service.SaveChanges();
                }
            }

        }


        public bool GetEventType(string message)
        {
            var _event = JsonSerializer.Deserialize<EventDto>(message);
Console.WriteLine($"event {_event.Event}");
            switch (_event.Event)
            {
                case "Platform_Publishe": return true;
                default: return false;
            }
        }

    }

    public enum EventType
    {
        Platform_Published,
        Undetermind
    }
}