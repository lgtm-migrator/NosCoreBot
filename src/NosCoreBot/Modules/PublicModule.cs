using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using NosCoreBot.Enumerations;

namespace NosCoreBot.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        [Command("delete-message")]
        [Name("delete <amount>")]
        [Summary("Deletes a specified amount of messages")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        public async Task Delete(int amount)
        {
            var messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
            await ((SocketTextChannel)Context.Channel).DeleteMessagesAsync(messages);
        }

        [Command("translation-message")]
        [Name("translation <language>")]
        [Summary("Give the translator roles")]
        public async Task Translation(RegionType region)
        {
            if (Context.Channel.Name == "translation-info")
            {
                var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == $"Translator({region.ToString()})");
                if (role == null)
                {
                    await ReplyAsync("the role doesn't exist.");
                    return;
                }

                var user = (SocketGuildUser)Context.User;
                if (role.Members.Any(r => r.Username == user.Username))
                {
                    await user.RemoveRoleAsync(role);
                    await ReplyAsync($"{user.Username} lost the role {role.Name}.");
                }
                else
                {
                    await user.AddRoleAsync(role);
                    await ReplyAsync($"{user.Username} got the role {role.Name}.");
                }
            }
        }
    }
}
