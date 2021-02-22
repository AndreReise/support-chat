using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Models;

namespace TechnicalSupport
{
    public class AutoDialog
    {


        delegate Message DialogMetod (Message mes);
        private Dictionary<Guid, int> clientDictionary;
        private Dictionary<Guid, int> dialogOneState;
        private ChatContext _context;

        enum Dialogs
        {
            Booking = 1,
            Register,
            Return,
            Table,
            Employee,
            Cancel = 0
        }
        private Dictionary<Dialogs, string> DialogName;
        Dictionary<Dialogs, DialogMetod> DialogMetodDict;
        string DefaultTextMessage = "Напишіть: Бронювання квитків, Онлайн реєстрація, Повернення квитка, Онлайн Табло, Звязатися з оператором";


        string [] textArrDialogOne = { "Нове бронювання", "Змінити бронювання"};
      
        public AutoDialog(ChatContext context)
        {
            _context = context;
            clientDictionary = new Dictionary<Guid, int>();
            dialogOneState = new Dictionary<Guid, int>();

            DialogName = new Dictionary<Dialogs, string>() {
                { Dialogs.Booking, "Бронювання квитків" },
                { Dialogs.Register, "Онлайн реєстрація" },
                { Dialogs.Return, "Повернення квитка" },
                { Dialogs.Table, "Онлайн Табло" },
                { Dialogs.Employee, "Звязатися з оператором" },
                { Dialogs.Cancel, "Вийти" }
            };

            DialogMetodDict = new Dictionary<Dialogs, DialogMetod>() {
                {Dialogs.Booking,  DialogOne},
                {Dialogs.Register,  DialogTwo},
                {Dialogs.Employee,  DialogEmployee},

            };

         
        }






        public Message ReplyMessage (Message mes) 
        {


            if (!clientDictionary.ContainsKey(mes.DialogId))
            {

                if (DialogName.ContainsValue(mes.Text))
                {
                    var tupeDialog = DialogName.Where(w => w.Value.Contains(mes.Text)).FirstOrDefault().Key;
                    clientDictionary.Add(mes.DialogId, (int)tupeDialog);
                    DialogMetodDict[tupeDialog](mes);
                }
            

                if (!clientDictionary.ContainsKey(mes.DialogId))
                {
                    clientDictionary.Add(mes.DialogId, (int)Dialogs.Cancel);
                    mes.Text = DefaultTextMessage;

                }


            }
            else 
            {
                int tmp = clientDictionary[mes.DialogId];
                if ( tmp == 0)
                {
                    if(DialogName.ContainsValue(mes.Text))
                    {
                        var tupeDialog = DialogName.Where(w => w.Value.Contains(mes.Text)).FirstOrDefault().Key;
                            clientDictionary[mes.DialogId] = (int)tupeDialog;
                            DialogMetodDict[tupeDialog](mes);
                    }
                  
                    else
                    {
                        mes.Text = clientDictionary[mes.DialogId] == 0 ? DefaultTextMessage : mes.Text;
                    }

                }
                else
                {

                    DialogMetodDict[(Dialogs)clientDictionary[mes.DialogId]](mes);         
                }

            }
        
            return mes;

        }



        public Message DialogOne(Message mes)
        {

            if (dialogOneState.ContainsKey(mes.DialogId))
            {
                var state = dialogOneState[mes.DialogId];
                if (textArrDialogOne.Length > state)
                {
                    mes.Text = textArrDialogOne[state];
                    dialogOneState[mes.DialogId]++;

                }

                if (textArrDialogOne.Length <= dialogOneState[mes.DialogId])
                {
                    clientDictionary[mes.DialogId] = 0;
                    dialogOneState[mes.DialogId] = 0;
                }
              

            }
            else
            {
                dialogOneState.Add(mes.DialogId, 0);
                    mes.Text = textArrDialogOne[0];
                dialogOneState[mes.DialogId]++;

            }


            return mes;
        }

        public Message DialogTwo(Message mes)
        {







            return mes;
        }
        public Message DialogThree(Message mes)
        {
            return mes;
        }


        
             public Message DialogEmployee(Message mes)
        {
           Employee emp = _context.Employees.Where(w => w.StatusOnline == true & w.Email != "Bot@mail.com").FirstOrDefault();

            if(emp !=null)
            {
               Dialog thisDialog =  _context.Dialogs.Where(w => w.DialogId == mes.DialogId).FirstOrDefault();
                thisDialog.EmployeeId = emp.Id;
                _context.SaveChanges();
                mes.Text = $"Перемикаю на оператора {emp.Name}";
            }
            else
            {
                mes.Text = "На даний момент немає доступних працівників";
            }

           

            return mes;
        }


    }
}
