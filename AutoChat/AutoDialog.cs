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
        private Dictionary<Guid, int> clientState;
        private Dictionary<Guid, int> dialogBookingState;
        private Dictionary<Guid, int> dialogRegState;
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
        string DefaultTextMessage;

      

        string [] textArrDialogBooking = { "Нове бронювання", "Змінити бронювання"};
        string[] textArrDialogRegister = { "Для регістрації введіть номер бронювання", " Оберіть номер місця в діапазоні 0-44", "Ви зареестровані!" };
        string[] arrNumberBooking = {"BA5555","BB444","BC3333", "AA1111" };




        public AutoDialog(ChatContext context)
        {
            _context = context;
            clientState = new Dictionary<Guid, int>();
            dialogBookingState = new Dictionary<Guid, int>();
            dialogRegState = new Dictionary<Guid, int>();
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
                {Dialogs.Booking,  DialogBooking},
                {Dialogs.Register,  DialogRegister},
                {Dialogs.Employee,  DialogEmployee},

            };


            DefaultTextMessage = ButtonToJson(5, "Оберіть сервіс:", new string[5] { "Бронювання квитків", "Онлайн реєстрація", "Повернення квитка", "Онлайн Табло", "Звязатися з оператором" });

          

        }






        public Message ReplyMessage (Message mes) 
        {

          

          

            if (!clientState.ContainsKey(mes.DialogId))
            {

                if (DialogName.ContainsValue(mes.Text))
                {
                    var tupeDialog = DialogName.Where(w => w.Value.Contains(mes.Text)).FirstOrDefault().Key;
                    clientState.Add(mes.DialogId, (int)tupeDialog);
                    mes = DialogMetodDict[tupeDialog](mes);
                }
                  else
                {
                    clientState.Add(mes.DialogId, (int)Dialogs.Cancel);
                    mes.TextTupe = "json";
                    mes.Text = DefaultTextMessage;

                }


            }
            else 
            {
                int tmp = clientState[mes.DialogId];
                if ( tmp == 0)
                {
                    if(DialogName.ContainsValue(mes.Text))
                    {
                        var tupeDialog = DialogName.Where(w => w.Value.Contains(mes.Text)).FirstOrDefault().Key;
                            clientState[mes.DialogId] = (int)tupeDialog;
                            mes = DialogMetodDict[tupeDialog](mes);
                    }
                    else
                    {
                        mes.TextTupe = "json";
                        mes.Text = clientState[mes.DialogId] == 0 ? DefaultTextMessage : mes.Text;
                    }

                }
                else
                {

                  mes = DialogMetodDict[(Dialogs)clientState[mes.DialogId]](mes);         
                }

            }
        
            return mes;

        }



        public Message DialogBooking(Message mes)
        {
            string ButtoBookingOne = ButtonToJson(2, "Оберіть дію:", new string[2] { "Нове бронювання", "Змінити бронювання" });


            if (dialogBookingState.ContainsKey(mes.DialogId))
            {
                var state = dialogBookingState[mes.DialogId];

                if (textArrDialogBooking.Length > state && state > 0)
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
                        mes.Text = "Не вірно введені дані " + textArrDialogBooking[--state];
                        return mes;

                    }

                    mes.Text = textArrDialogRegister[state];
                    dialogRegState[mes.DialogId]++;

                    if (textArrDialogRegister.Length <= dialogRegState[mes.DialogId])
                    {
                        clientState[mes.DialogId] = 0;
                        dialogRegState[mes.DialogId] = 0;

                    }
                    return mes;
















               

                }



                if (textArrDialogBooking.Length <= dialogBookingState[mes.DialogId])
                {
                    clientState[mes.DialogId] = 0;
                    dialogBookingState[mes.DialogId] = 0;
                }
              

            }
            else
            {
                dialogBookingState.Add(mes.DialogId, 0);
                mes.TextTupe = "json";
                mes.Text = ButtoBookingOne;
                dialogBookingState[mes.DialogId]++;

            }


            return mes;


           bool ValidationOne(Message mes)
            {


                return false;
            }
            bool ValidationTwo(Message mes)
            {


                return false;
            }
            bool ValidationThree(Message mes)
            {


                return false;
            }




        }

        public Message DialogRegister(Message mes)
        {


            
            if (dialogRegState.ContainsKey(mes.DialogId))
            {

                var state = dialogRegState[mes.DialogId];

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

                      //  dialogRegState[mes.DialogId]--;
                        return mes;

                    }
                    mes.Text = textArrDialogRegister[state];
                    dialogRegState[mes.DialogId]++;

                    if (textArrDialogRegister.Length <= dialogRegState[mes.DialogId])
                    {
                        clientState[mes.DialogId] = 0;
                        dialogRegState[mes.DialogId] = 0;
                      
                    }
                    return mes;

                }

            }
            else
            {
                dialogRegState.Add(mes.DialogId, 0);
                
          

            }
            mes.Text = textArrDialogRegister[dialogRegState[mes.DialogId]];
            dialogRegState[mes.DialogId]++;

            return mes;


            bool ValidationOne (Message mes)
            {
                if(dialogRegState[mes.DialogId] ==1)
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
                if (dialogRegState[mes.DialogId] == 2)
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
                if (dialogRegState[mes.DialogId] == 3)
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
            Employee emp = _context.Employees.Where(w => w.StatusOnline == true & w.Email != "Bot@mail.com").FirstOrDefault();

            if(emp !=null)
            {
                Dialog thisDialog =  _context.Dialogs.Where(w => w.DialogId == mes.DialogId).FirstOrDefault();
                thisDialog.EmployeeId = emp.EmployeeGuid;
                _context.SaveChanges();
                mes.Text = $"Перемикаю на оператора {emp.SecondName}";
            }
            else
            {
                mes.Text = "На даний момент немає доступних працівників";
                clientState[mes.DialogId] = 0;

            }
        return mes;
        }

        public string ButtonToJson (int i, string text, string [] buttontext)
        {
            var buttons = new
            {
                buttoncount = i,
                text = text,
                textbutton = buttontext

            };
            string json = JsonSerializer.Serialize(buttons);
            return json;
        }




    }
}
