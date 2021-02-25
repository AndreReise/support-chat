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
            if(employees != null)
                EmploDictionar =  employees.ToDictionary(s => s.Email);

            var clients = _chatContext.Clients.Where(x => x.Email != null);
            if(clients != null)
                UserDictionar =  clients.ToDictionary(s => s.Email);
            //_chatContext.Employees.Select(o => new DictionaryEntry
            //{
            //    Key = o.Id,
            //    Value = o.Email
            //}).ToList();

        }

        public virtual string GetUserId(HubConnectionContext connection)
        {
            Guid id = default(Guid);

            if (connection.User?.Identity.Name != null)
            {

             
              //  var userDichtionary = HomeController.useremail;

                // string t = userDichtionary.Keys.FirstOrDefault(s => s.Contains(connection.User?.Identity.Name));
                // string t = MessageHub._context.Users.FirstOrDefault(s => s.Email == connection.User.Identity.Name.ToString()).Id.ToString();
                // string t = userDichtionary.Where((d, v) => d.Key.Contains(connection.User.Identity.Name.ToString())).Select(v => v.Values).ToList();
                
                
              //  var user = _chatContext.Users.FirstOrDefault(s => s.Email == connection.User.Identity.Name);


               if ( EmploDictionar.ContainsKey(connection.User.Identity.Name))
                { 
               id = EmploDictionar[connection.User.Identity.Name].EmployeeGuid;
                
                }

              

                if (UserDictionar.ContainsKey(connection.User.Identity.Name))
                {
                  id = UserDictionar[connection.User.Identity.Name].ClientGuid;
                }

                id = id != null ? id : Guid.NewGuid();


                // Guid id = user?.Id ?? _chatContext.Employees.FirstOrDefault(s => s.Email == connection.User.Identity.Name).Id;


                //  string t = userDichtionary[connection.User?.Identity.Name].ToString();

                return id.ToString();

            }


            else return Guid.NewGuid().ToString();
            // или так
            //return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
        }




    }
}