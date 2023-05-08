using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowKittenBot
{
    public static class BaseService
    {
        public static IServiceProvider ConfigureServices()
        {
            var map = new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()));
            map.AddSingleton(new CommandService(new CommandServiceConfig()));
            return map.BuildServiceProvider();
        }
    }
}
