using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        private Dictionary<Guid, int> dialogTwoState;
        Dictionary<Guid, Dictionary<string, string>> UserDataDictionary;
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
        string[] textArrDialogRegister = { "Для регістрації введіть номер бронювання", " Оберіть номер місця в діапазоні 0-44", "Ви зареестровані!" };
        string[] arrNumberBooking = {"BA5555","BB444","BC3333", "AA1111" };


        public AutoDialog(ChatContext context)
        {
            _context = context;
            clientDictionary = new Dictionary<Guid, int>();
            dialogOneState = new Dictionary<Guid, int>();
            dialogTwoState = new Dictionary<Guid, int>();
            UserDataDictionary = new Dictionary<Guid, Dictionary<string, string>>();
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

            var v = new
            {
                buttoncount = 5,
                text = "Оберіть сервіс:",
                textbutton = new string[5] { "Бронювання квитків", "Онлайн реєстрація", "Повернення квитка", "Онлайн Табло", "Звязатися з оператором" }
                
            };

            string json = JsonSerializer.Serialize(v);


            mes.SenderType = "text";


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
                    mes.TextTupe = "json";
                    // mes.Text = DefaultTextMessage;
                    mes.Text = json;

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
                        mes.TextTupe = "json";
                        mes.Text = clientDictionary[mes.DialogId] == 0 ? json : mes.Text;
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


            
            if (dialogTwoState.ContainsKey(mes.DialogId))
            {

                var state = dialogTwoState[mes.DialogId];

                if (textArrDialogRegister.Length > state && state > 0)
                {
                    bool valid = false;
                    switch (state)
                    {
                        case 1:
                            valid = ValidationOne(mes);
                            break;
                        case 2:
                            valid = ValidationTwo(mes);
                            break;
                        case 3:
                            valid = ValidationThree(mes);
                            break;

                    }
                    if (!valid)
                    {
                        mes.Text = "Не вірно введені дані "+textArrDialogRegister[--state];

                      //  dialogTwoState[mes.DialogId]--;
                        return mes;

                    }
                    mes.Text = textArrDialogRegister[state];
                    dialogTwoState[mes.DialogId]++;

                    if (textArrDialogRegister.Length <= dialogTwoState[mes.DialogId])
                    {
                        clientDictionary[mes.DialogId] = 0;
                        dialogTwoState[mes.DialogId] = 0;
                      
                    }
                    return mes;

                }



              


            }
            else
            {
                dialogTwoState.Add(mes.DialogId, 0);
                
          

            }
            mes.Text = textArrDialogRegister[dialogTwoState[mes.DialogId]];
            dialogTwoState[mes.DialogId]++;

            return mes;


            bool ValidationOne (Message mes)
            {
                if(dialogTwoState[mes.DialogId] ==1)
                {
                    var numberBooking = mes.Text;
                   if(arrNumberBooking.Contains(numberBooking))
                    {
                       if(!UserDataDictionary.ContainsKey(mes.DialogId))
                        {
                            UserDataDictionary.Add(mes.DialogId, new Dictionary<string, string>() { { "NumBooking", mes.Text } });
                            return true;
                        }
                       else
                        {
                            if(UserDataDictionary[mes.DialogId].ContainsKey("NumBooking"))
                            {
                                UserDataDictionary[mes.DialogId].Add("NumBooking", mes.Text);
                            }
                            else
                            {
                                UserDataDictionary[mes.DialogId]["NumBooking"] = mes.Text;
                            } 
                           
                        }
                       
                    }
                    else
                    {
                        return false;

                    }

                }

                return false;

            }

            bool ValidationTwo(Message mes)
            {
                if (dialogTwoState[mes.DialogId] == 2)
                {
                    
                    int numberseat;
                    
                    if (int.TryParse(mes.Text, out numberseat) && numberseat<=44 && numberseat>0)
                    {
                        if (UserDataDictionary[mes.DialogId].ContainsKey("NumBooking"))
                        {
                            UserDataDictionary[mes.DialogId].Add("NumSeat", mes.Text);
                        }
                        else
                        {
                            UserDataDictionary[mes.DialogId]["NumSeat"] = mes.Text;
                        }
                       
                        
                        return true;
                    }
                    else
                    {
                        return false;

                    }

                }

                return false;

            }

            bool ValidationThree(Message mes)
            {
                if (dialogTwoState[mes.DialogId] == 3)
                {

                 

                    if (UserDataDictionary[mes.DialogId]["NumBooking"] != null && UserDataDictionary[mes.DialogId]["NumSeat"] != null)
                    {

                      
                        return true;
                    }
                    else
                    {
                        return false;

                    }

                }

                return false;

            }


        }
        public Message DialogThree(Message mes)
        {
            return mes;
        }


        
             public Message DialogEmployee(Message mes)
        {
           User emp = _context.Users.Where(w=>w.Role.Name == "EMPLOYEE").FirstOrDefault();

            if(emp !=null)
            {
               Dialog thisDialog =  _context.Dialogs.Where(w => w.DialogId == mes.DialogId).FirstOrDefault();
                thisDialog.EmployeeId = emp.UserGuid;
                _context.SaveChanges();
                mes.Text = $"Перемикаю на оператора";
            }
            else
            {
                mes.Text = "На даний момент немає доступних працівників";
                clientDictionary[mes.DialogId] = 0;

            }

           

            return mes;
        }


    }
}
