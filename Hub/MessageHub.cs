using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Hub;
using TechnicalSupport.Models;

namespace TechnicalSupport
{
    public class MessageHub : Hub<IMessageHub>
    {
        private ChatContext _context;
        private AutoDialog _autoDialog;
        private Dictionary<Guid, Dialog> _usersDialog;
        private readonly Guid _botId = default(Guid);

        public MessageHub(ChatContext context, AutoDialog auto)
        {
            _context = context;
            _autoDialog = auto;
            _usersDialog = new Dictionary<Guid, Dialog>();
            _usersDialog = _context.Dialogs?.ToDictionary(k => k.DialogId);
        }

        public async Task Send(Message message)
        {
            var dialog = _usersDialog
                .FirstOrDefault(x => x.Value.ClientGuid
                .ToString() == Context.UserIdentifier || x.Value.EmployeeGuid.ToString() == Context.UserIdentifier)
                .Value;

            if (dialog != null)
            {
                message.SenderType = "in";
                message.DialogId = dialog.DialogId;
                message.ClientId = dialog.ClientGuid;

                await Clients.User(Context.UserIdentifier).Receive(message);

                if (dialog.EmployeeGuid == _botId)
                {
                    await Clients.User(Context.UserIdentifier.ToString()).Receive(_autoDialog.ReplyMessage(message));
                }
                else
                {
                    message.SenderType = "out";
                    await Clients.User(dialog.EmployeeGuid.ToString()).Receive(message);
                }
            }
            else
            {
                Guid dialogId = Guid.NewGuid();

                Dialog newdialog = new Dialog()
                {
                    ClientGuid = Guid.Parse(Context.UserIdentifier),
                    DialogId = dialogId,
                    EmployeeGuid = _botId
                };

                _context.Dialogs.Add(newdialog);
                _usersDialog.Add(key: newdialog.DialogId, value: newdialog);
                _context.SaveChanges();

                message.DialogId = dialogId;

                await Clients.User(Context.UserIdentifier.ToString()).Receive(message);
                await Clients.User(Context.UserIdentifier.ToString()).Receive(_autoDialog.ReplyMessage(message));
            }
        }

        [Authorize(Roles = "EMPLOYEE")]
        public async Task SendAdmin(Message message)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                await Clients.User(Context.UserIdentifier.ToString()).Receive(message);

                if (_usersDialog.ContainsKey(message.DialogId))
                {
                    message.SenderType = "out";
                    await Clients.User(_usersDialog[message.DialogId].ClientGuid.ToString()).Receive(message);
                }
            }
        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var email = Context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Email).Value;

                var user = await _context.Users.Include(i => i.Role)
                    .FirstOrDefaultAsync(x => x.Email == email);

                if (user != null)
                {
                    var mes = new Message() { Name = user.FirstName, Text = $"Hello {user.FirstName}" };

                    if (user.Role.Name == "EMPLOYEE")
                    {
                        _context.Employees.FirstOrDefault(f => f.UserGuid == user.UserGuid).StatusOnline = true;
                    }
                    else
                    {
                        var dialog = _usersDialog
                        .FirstOrDefault(x => x.Value.ClientGuid.ToString() == Context.UserIdentifier || x.Value.EmployeeGuid
                        .ToString() == Context.UserIdentifier)
                        .Value;

                        Dialog dialogResalt;

                        if (dialog == null)
                        {
                            dialogResalt = new Dialog()
                            {
                                ClientGuid = Guid.Parse(Context.UserIdentifier),
                                DialogId = Guid.NewGuid(),
                                EmployeeGuid = _botId
                            };

                            _context.Dialogs.Add(dialogResalt);
                            _usersDialog.Add(key: dialogResalt.DialogId, value: dialogResalt);
                        }
                        else
                        {
                            dialogResalt = dialog;
                        }
                        mes.DialogId = dialogResalt.DialogId;
                    }

                    await Clients.User(Context.UserIdentifier.ToString()).Receive(mes);
                }
            }
            else
            {
                Guid Id = Guid.Parse(Context.UserIdentifier);

                Guid dialogId = Guid.NewGuid();
                Dialog dialog = new Dialog()
                {
                    ClientGuid = Id,
                    DialogId = dialogId,
                    EmployeeGuid = _botId
                };

                _context.Dialogs.Add(dialog);
                _usersDialog.Add(key: dialog.DialogId, value: dialog);

                Message mes = new Message()
                {
                    Name = "UserDefault",
                    Text = "Hello User",
                    DialogId = dialogId
                };

                await Clients.User(Id.ToString()).Receive(mes);
            }
            _context.SaveChanges();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            User user = await _context.Users.Include(i => i.Role)
            .FirstOrDefaultAsync(u => u.UserGuid == Guid.Parse(Context.UserIdentifier));

            var dialog = _context.Dialogs
            .FirstOrDefault(f => f.ClientGuid == Guid.Parse(Context.UserIdentifier)
            || f.EmployeeGuid == Guid.Parse(Context.UserIdentifier));

            if (dialog != null)
            {
                var userOnline = dialog.ClientGuid.ToString() == Context.UserIdentifier ? dialog.EmployeeGuid : dialog.ClientGuid;

                if (userOnline != _botId)
                {
                    await Clients.User(userOnline.ToString()).Receive(new Message() { Text = "Disconected", DialogId = dialog.DialogId });
                }

                _context.Remove(dialog);
                _usersDialog.Remove(dialog.DialogId);
            }

            if (user != null && user.Role.Name == "EMPLOYEE")
            {
                _context.Employees.FirstOrDefault(f => f.UserGuid == user.UserGuid).StatusOnline = false;
            }

            _context.SaveChanges();

            await base.OnDisconnectedAsync(exception);
        }
    }
}