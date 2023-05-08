using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace RainbowKittenBot
{
    public class BaseModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping(SocketUser user) 
        {
            await ReplyAsync($"{user.Mention}\n{user.Mention}\n{user.Mention}\n{user.Mention}\n{user.Mention}\n");
        }
        [Command("roles")]
        public async Task ListRoles(SocketGuildUser user) 
        {
            var guildUser = user;

            var roleList = string.Join(",\n", guildUser.Roles.Where(x => !x.IsEveryone).Select(x => x.Mention));

            var embedBuiler = new EmbedBuilder()
                .WithAuthor(guildUser.ToString(), guildUser.GetAvatarUrl() ?? guildUser.GetDefaultAvatarUrl())
                .WithTitle("Roles")
                .WithDescription(roleList)
                .WithColor(Color.Green)
                .WithCurrentTimestamp();

            await ReplyAsync(embed: embedBuiler.Build());
        }
    }
}
