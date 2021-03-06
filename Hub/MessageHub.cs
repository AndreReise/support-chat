using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Security.Principal;
using TechnicalSupport.Data;
using Microsoft.EntityFrameworkCore;
using TechnicalSupport.Models;

namespace TechnicalSupport
{
    public class MessageHub : Hub
    {
        private ChatContext _context;
        AutoDialog _autoDialog;
       
        public MessageHub (ChatContext context, AutoDialog auto)
        {
            _context = context;
            _autoDialog = auto;
      

        }


        // [Authorize]
        public async Task Send(Message message)
        {

            var dialog = _context.Dialogs.FirstOrDefault(em => em.UserId.ToString() == Context.UserIdentifier);

            if (dialog != null)
            {



                message.SenderType = "in";
                message.DialogId = dialog.DialogId;
                message.ClientId = dialog.UserId;
                await Clients.User(Context.UserIdentifier.ToString()).SendAsync("Receive", message);

                if (dialog.EmployeeId == Guid.Parse("a839ea3e-1c14-45c4-95bb-529b7cad712b"))
                {

                    Message repmes = _autoDialog.ReplyMessage(message);

                    message.SenderType = "out";

                    await Clients.User(Context.UserIdentifier.ToString()).SendAsync("Receive", repmes);
                }
                else
                {
                    message.SenderType = "out";
                    await Clients.User(dialog.EmployeeId.ToString()).SendAsync("Receive", message);
                }




            }
            else
            {

                Guid dialogId = Guid.NewGuid();

                Dialog temp = new Dialog() { UserId = Guid.Parse(Context.UserIdentifier), DialogId = dialogId, EmployeeId = Guid.Parse("a839ea3e-1c14-45c4-95bb-529b7cad712b") };
                _context.Dialogs.Add(temp);
                _context.SaveChanges();

                message.DialogId = dialogId;


                message.SenderType = "in";

                await Clients.User(Context.UserIdentifier.ToString()).SendAsync("Receive", message);

                Message repmes = _autoDialog.ReplyMessage(message);

                message.SenderType = "out";

                await Clients.User(Context.UserIdentifier.ToString()).SendAsync("Receive", repmes);



            }







        }

        public async Task SendTechnical(Message message)
        {

         

        }


        [Authorize(Roles = "EMPLOYEE")]
        public async Task SendAdmin (Message message)
        {
            if (Context.User.Identity.IsAuthenticated)

            {
                message.SenderType = "in";
                await Clients.User(Context.UserIdentifier.ToString()).SendAsync("Receive", message);
                var dialog = _context.Dialogs.Find(message.DialogId);
                if (dialog != null)
                {
                    message.SenderType = "out";
                    await Clients.User(dialog.UserId.ToString()).SendAsync("Receive", message);
               
                }

            }
        }

       
        public override async Task OnConnectedAsync()
        {

            if (Context.User.Identity.IsAuthenticated && Context.User.HasClaim(c => c.Value == "EMPLOYEE"))
            {
                Employee employee = await _context.Employees
                  //.Include(u => u.Role)
                  .SingleOrDefaultAsync(u => u.FirstName+u.LastName == Context.User.Identity.Name);

                if (employee != null)
                {
                    employee.StatusOnline = true;
                    await Clients.User(employee.EmployeeId.ToString()).SendAsync("Receive", new Message() { Name = employee.FirstName, Text = "Hello Admin" });

                }


            }
            else
            {
                    if (Context.User.Identity.IsAuthenticated)
                    {

                    User user = await _context.Users
                    .Include(u => u.Role)
                    .SingleOrDefaultAsync(u => u.UserGuid == Guid.Parse(Context.UserIdentifier));

                            if (user != null)
                            {

                               Guid guidDialog = Guid.NewGuid();
                              
                               await Clients.User(Context.UserIdentifier.ToString()).SendAsync("Receive", new Message() { Name = "UserDefault", Text = " Hello user", DialogId = guidDialog });


                            }
                           
                     }
                     else
                     {
                        Guid Id = Guid.Parse(Context.UserIdentifier);
                        _context.Users.Add(new User() { UserGuid = Id });
                        Guid dialogId = Guid.NewGuid();
                        Dialog temp = new Dialog() { UserId = Guid.Parse(Context.UserIdentifier), DialogId = dialogId, EmployeeId = Guid.Parse("a839ea3e-1c14-45c4-95bb-529b7cad712b") };
                        _context.Dialogs.Add(temp);
                        await Clients.User(Id.ToString()).SendAsync("Receive", new Message() { Name = "UserDefault", Text = "Hello user", DialogId = dialogId });

                     
                    }
                 


            }



            _context.SaveChanges();
            await base.OnConnectedAsync();
        }
      
        public override async Task OnDisconnectedAsync(Exception exception)
        {

            if (Context.User.Identity.IsAuthenticated && Context.User.HasClaim(c => c.Value == "EMPLOYEE"))
            {
                Employee employee = await _context.Employees
              .SingleOrDefaultAsync(u => u.EmployeeGuid == Guid.Parse(Context.UserIdentifier));

                if (employee != null)
                {
                    employee.StatusOnline = false;
             
                }

            }
            else
            {

                User user =  _context.Users
                .Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.UserGuid == Guid.Parse(Context.UserIdentifier)).Result;


                if (user != null)
                {
                    var dialog = _context.Dialogs.Where(w => w.UserId == Guid.Parse(Context.UserIdentifier)).FirstOrDefault();


                    if(dialog != null)
                    {
                        await Clients.User(dialog.EmployeeId.ToString()).SendAsync("Receive", new Message() { Name = "Disconected", Text = $"Disconected {Context.UserIdentifier}" });
                        _context.Dialogs.Remove(_context.Dialogs.FirstOrDefault(f => f.UserId == Guid.Parse(Context.UserIdentifier)));

                      if (user.Email == null)  _context.Users.Remove(user);
                    }



                }

          

            }
            _context.SaveChanges();
            //  await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
            await base.OnDisconnectedAsync(exception);
        }

    }
}
