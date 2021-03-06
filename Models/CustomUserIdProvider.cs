using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using TechnicalSupport.Data;
using TechnicalSupport.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace TechnicalSupport.Models
{


    public class CustomUserIdProvider : IUserIdProvider
    {
        private readonly ChatContext _chatContext;

        Dictionary<string, Employee> EmploDictionar;
        Dictionary<string, Client> UserDictionar;

        public CustomUserIdProvider (ChatContext db)
        {
            
            _chatContext = db;


            var employees = _chatContext.Employees.Where(x => x.Email != null).Select(x => x).ToList();
            if (employees != null)
                EmploDictionar = employees.ToDictionary(s => s.FirstName+s.LastName);

            var clients = _chatContext.Clients.Where(x => x.Email != null);
            if (clients != null)
                UserDictionar = clients.ToDictionary(s => s.FirstName + s.LastName);


        }

        public virtual string GetUserId(HubConnectionContext connection)
        {
            Guid id = default(Guid);

            if (connection.User?.Identity.Name != null)
            {


               if ( EmploDictionar.ContainsKey(connection.User.Identity.Name))
                { 
               id = EmploDictionar[connection.User.Identity.Name].EmployeeGuid;
                
                }

              

                if (UserDictionar.ContainsKey(connection.User.Identity.Name))
                {
                  id = UserDictionar[connection.User.Identity.Name].ClientGuid;
                }

                id = id != null ? id : Guid.NewGuid();

                return id.ToString();

            }


            else return Guid.NewGuid().ToString();
            // или так
            //return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
        }




    }
}