using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace RainbowKittenBot
{
    public class SlashCommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public SlashCommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            _commands = commands;
            _client = client;
            _services = services;
        }
        public async Task CommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "roles":
                    await HandleListRoleCommand(command);
                    break;
                case "mult":
                    await Multiply(command);
                    break;
                case "disconnect":
                    await DisconnectClient(command);
                    break;
                case "ping":
                    await PingUser(command); 
                    break;
            }
        }
        private async Task HandleListRoleCommand(SocketSlashCommand command)
        {
            var guildUser = (SocketGuildUser)command.Data.Options.First().Value;

            //remove the everyone role and select the mention of each role.
            var roleList = string.Join(",\n", guildUser.Roles.Where(x => !x.IsEveryone).Select(x => x.Mention));

            var embedBuiler = new EmbedBuilder()
                .WithAuthor(guildUser.ToString(), guildUser.GetAvatarUrl() ?? guildUser.GetDefaultAvatarUrl())
                .WithTitle("Roles")
                .WithDescription(roleList)
                .WithColor(Color.Green)
                .WithCurrentTimestamp();

            // Response
            await command.RespondAsync(embed: embedBuiler.Build());
        }
        private async Task Multiply(SocketSlashCommand command)
        {
            var number1 = Convert.ToInt32(command.Data.Options.ToList()[0].Value);
            var number2 = Convert.ToInt32(command.Data.Options.ToList()[1].Value);
            var result = number1 * number2;
            await command.RespondAsync($"{result}");
        }
        private async Task DisconnectClient(SocketSlashCommand command) 
        {
            await command.RespondAsync("Desconectando");
            await _client.StopAsync();
            Environment.Exit(757);
        }
        private async Task PingUser(SocketSlashCommand command) 
        {
            var guildUser = (SocketUser)command.Data.Options.First().Value;
            
            await command.RespondAsync($"{guildUser.Mention}\n{guildUser.Mention}\n{guildUser.Mention}\n{guildUser.Mention}\n{guildUser.Mention}\n");
        }
        public async Task Client_Ready()
        {
            // Note: Names have to be all lowercase and match the regular expression ^[\w-]{3,32}$
            // Descriptions can have a max length of 100.

            //Server guild id goes here
            var guild = _client.GetGuild(00000000000);

            // Guild Commands
            var guildCommand = new SlashCommandBuilder();

            guildCommand.WithName("mult").WithDescription("multiply two numbers").AddOption("first", ApplicationCommandOptionType.Integer, "number1", isRequired: true)
            .AddOption("second", ApplicationCommandOptionType.Integer, "number2", isRequired: true);

            var guildCommand2 = new SlashCommandBuilder();
            guildCommand2.WithName("roles").WithDescription("List all roles of a user").AddOption("user", ApplicationCommandOptionType.User, "The users whos roles you want to be listed", isRequired: true);

            var guildCommand3 = new SlashCommandBuilder();
            guildCommand3.WithName("disconnect").WithDescription("Disconnects the bot");

            var guildCommand4 = new SlashCommandBuilder();
            guildCommand4.WithName("ping").WithDescription("Ping a user").AddOption("user", ApplicationCommandOptionType.Mentionable, "The users who you want to ping", isRequired: true);

            //Global Commands
            //var globalCommand = new SlashCommandBuilder();





            try
            {
            
                await guild.CreateApplicationCommandAsync(guildCommand.Build());
                await guild.CreateApplicationCommandAsync(guildCommand2.Build());
                await guild.CreateApplicationCommandAsync(guildCommand3.Build());
                await guild.CreateApplicationCommandAsync(guildCommand4.Build());

                //Global commands don't need the guild.
                //await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());

                // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
                // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
            }
            catch (HttpException exception)
            {
                //If a command is invalid, returns the error
                var json = JsonConvert.SerializeObject(exception.Message, Formatting.Indented);

                //Where the error will appear
                Console.WriteLine(json);
            }
        }
    }
}
