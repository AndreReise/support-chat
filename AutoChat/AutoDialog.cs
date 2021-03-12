using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TechnicalSupport.AutoChat;
using TechnicalSupport.Data;
using TechnicalSupport.Models;

namespace TechnicalSupport
{
    public class AutoDialog
    {
        delegate Message DialogMetod (Message mes);

        protected Dictionary<Guid, int> clientState;
        private Dictionary<Guid, int> dialogBookingState;
        private Dictionary<Guid, int> dialogNewBookingState;
        private Dictionary<Guid, int> dialogEditBookingState;
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


        string[] textArrDialogRegister;
        string[][] tableflight;
        string[] textArrDialogEditBooking;

        public AutoDialog(ChatContext context)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("dialogsettings.json");
            IConfigurationRoot config = builder.Build();
           
            textArrDialogRegister = config.GetSection("textArrDialogRegister").Get<string[]>();
            tableflight = config.GetSection("tableflight").Get<string[][]>();
            textArrDialogEditBooking = config.GetSection("textArrDialogEditBooking").Get<string[]>();

            _context = context;

            clientState = new Dictionary<Guid, int>();
            dialogBookingState = new Dictionary<Guid, int>();
            dialogNewBookingState = new Dictionary<Guid, int>();
            dialogEditBookingState = new Dictionary<Guid, int>();
            dialogRegState = new Dictionary<Guid, int>();

            UserDataDictionary = new Dictionary<Guid, Dictionary<string, string>>();

            DialogName = new Dictionary<Dialogs, string>() {
                { Dialogs.Booking, "Бронювання квитків" },
                { Dialogs.Register, "Онлайн реєстрація" },
                { Dialogs.Employee, "Звязатися з оператором" }
            };

          

            DialogMetodDict = new Dictionary<Dialogs, DialogMetod>() {
                {Dialogs.Booking,  DialogBookingMain},
                {Dialogs.Register,  DialogRegister},
                {Dialogs.Employee,  EmployeeMethod},

            };

            DefaultTextMessage = ButtonToJson(5, "Оберіть сервіс:", DialogName.Values.ToArray());

        }


        public Message ReplyMessage (Message mes) 
        {


            mes.TextTupe = "text";
            mes.Name = "Bot";

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
                    if(mes.Text.Contains("Вийти") )
                    {
                        clientState[mes.DialogId] = (int)Dialogs.Cancel;
                        mes.TextTupe = "json";
                        mes.Text =  DefaultTextMessage;

                    }
                    else
                    {
                        mes = DialogMetodDict[(Dialogs)clientState[mes.DialogId]](mes);
                    }
                       
                }

            }
        
            return mes;

        }

        public Message DialogBookingMain (Message mes)
        {
            mes.TextTupe = "text";
            if (dialogBookingState.ContainsKey(mes.DialogId))
            {
                switch (dialogBookingState[mes.DialogId])
                {
                    case 2:
                        mes = DialogBookingNew(mes);
                        return mes;
                    case 3:
                        mes = DialogBookingEdit(mes);
                        return mes;

                }

                var textBooking = mes.Text;
                if (dialogBookingState[mes.DialogId] == 1 && textBooking == "Нове бронювання")
                {
                    dialogBookingState[mes.DialogId] = 2;

                    mes = DialogBookingNew(mes);
                    return mes;
                }
                else
                {
                    if (dialogBookingState[mes.DialogId] == 1 && textBooking == "Змінити бронювання")
                    {
                        dialogBookingState[mes.DialogId] = 3;

                        mes = DialogBookingEdit(mes);
                        return mes;
                    }
                    else
                    {
                        dialogBookingState[mes.DialogId] = 0;
                      
                    }
                }
            }
            else
            {

                dialogBookingState.Add(mes.DialogId, 0);
               

            }  


            string ButtoBookingOne = ButtonToJson(2, "Оберіть дію:", new string[2] { "Нове бронювання", "Змінити бронювання" });

            mes.Text = ButtoBookingOne;
            dialogBookingState[mes.DialogId]++;
            mes.TextTupe = "json";
            return mes;

           

        }

        public Message DialogBookingNew(Message mes)
        {

            string ButtoBookingOne = ButtonToJson(4, "Оберіть пункт відправлення:", new string[4] { "Київ", "Харків", "Львів", "Одеса" });

            string ButtoBookingTwo = ButtonToJson(4, "Оберіть пункт призначення:", new string[4] { "Київ", "Харків", "Львів", "Одеса" });

            string textnumberseat = ButtonToJson(0, "Введіть номер місця з 1 до 22");
            string flight;
            string numBooking;
            if (UserDataDictionary.ContainsKey(mes.DialogId) && UserDataDictionary[mes.DialogId].ContainsKey("Flight")) { flight = UserDataDictionary[mes.DialogId]["Flight"]; }else { flight = ""; }
            if (UserDataDictionary.ContainsKey(mes.DialogId) && UserDataDictionary[mes.DialogId].ContainsKey("NumBooking")) { numBooking = UserDataDictionary[mes.DialogId]["NumBooking"]; } else { numBooking = ""; }

            string textBookingConfirm = ButtonToJson(2, $"Підтвердіть бронювання за маршрутом  {flight}", new string[2] { "Yes", "No" });
            
            string[] textArrDialogNewBooking = { ButtoBookingOne, ButtoBookingTwo, "Оберіть рейс", textnumberseat, textBookingConfirm, "" };



            mes.TextTupe = "json";


            if (dialogNewBookingState.ContainsKey(mes.DialogId))
            {
                var state = dialogNewBookingState[mes.DialogId];

                if (textArrDialogNewBooking.Length > state && state >= 0)
                {
                    bool valid = false;
                    switch (state)
                    {
                        case 0: valid = true;
                            break;
                        case 1:
                            valid = ValidationOne(mes);
                            break;
                        case 2:
                            valid = ValidationTwo(mes);
                            break;
                        case 3:
                            valid = ValidationThree(mes);
                            break;
                        case 4:
                            valid = ValidationFour(mes);
                            break;
                        case 5:
                            {
                                valid = ValidationFive(mes);
                                textArrDialogNewBooking[5] = valid ? ButtonToJson(0, $"Дякуємо! Номер бронювання: {UserDataDictionary[mes.DialogId]["NumBooking"]}"):"";
                                break;
                            }
                            

                    }
                    if (!valid)
                    {
                       
                        mes.Text = textArrDialogNewBooking[--state];
                        return mes;

                    }

                    if (dialogNewBookingState[mes.DialogId] == 2)
                    {
                        List<string> arr = new List<string>();

                        string from = UserDataDictionary[mes.DialogId]["From"];
                        string to = UserDataDictionary[mes.DialogId]["To"];

                        for (int i = 0; i < tableflight.Length; i++)
                        {

                            if (tableflight[i][0] == from && tableflight[i][1] == to)
                            {

                                arr.Add(tableflight[i][0] + "-" + tableflight[i][1] + " " + tableflight[i][2]);

                            }

                        }

                        if (arr.Count() > 0)
                        {

                            mes.Text = ButtonToJson(arr.Count(), "Оберіть маршрут:", arr.ToArray());
                            dialogNewBookingState[mes.DialogId]++;
                        }
                        else
                        {
                            mes.TextTupe = "text";
                            mes.Text = "Маршрутів не знайдено! Змініть пункт призначення.";
                            dialogNewBookingState[mes.DialogId]--;
                        }

                    }
                    else
                    {
                        mes.Text = textArrDialogNewBooking[state];
                        dialogNewBookingState[mes.DialogId]++;


                    }





                    if (textArrDialogNewBooking.Length <= dialogNewBookingState[mes.DialogId])
                    {
                        clientState[mes.DialogId] = 0;
                        dialogBookingState[mes.DialogId] = 0;
                        dialogNewBookingState[mes.DialogId] = 0;
                       
                    }
                    return mes;

                }
              
            }
            else
            {
                
                dialogNewBookingState.Add(mes.DialogId, 0);
                mes.Text = textArrDialogNewBooking[0];
                dialogNewBookingState[mes.DialogId]++;

            }


            return mes;


            bool ValidationOne(Message mes)
            {
                if (dialogNewBookingState[mes.DialogId] == 1 && tableflight.Any(x => x.Contains(mes.Text)))
                {

                    if (!UserDataDictionary.ContainsKey(mes.DialogId))
                    {
                        UserDataDictionary.Add(mes.DialogId, new Dictionary<string, string>() { { "From", mes.Text } });

                    }
                    else
                    {
                        if (UserDataDictionary[mes.DialogId].ContainsKey("From"))
                        {
                            UserDataDictionary[mes.DialogId]["From"] = mes.Text;

                        }
                        else
                        {
                            UserDataDictionary[mes.DialogId].Add("From", mes.Text);
                        }

                    }
                    return true;

                }

                return false;
            }

            bool ValidationTwo(Message mes)
            {
                if (dialogNewBookingState[mes.DialogId] == 2 && tableflight.Any(x => x.Contains(mes.Text)))
                {

                    if (!UserDataDictionary.ContainsKey(mes.DialogId))
                    {
                        UserDataDictionary.Add(mes.DialogId, new Dictionary<string, string>() { { "To", mes.Text } });

                    }
                    else
                    {
                        if (UserDataDictionary[mes.DialogId].ContainsKey("To"))
                        {
                            UserDataDictionary[mes.DialogId]["To"] = mes.Text;

                        }
                        else
                        {
                            UserDataDictionary[mes.DialogId].Add("To", mes.Text);
                        }

                    }

                    return true;

                }
                return false;
            }

            bool ValidationThree(Message mes)
            {
                List<string> arr = new List<string>();

                for (int i = 0; i < tableflight.Length; i++)
                {
                    arr.Add(tableflight[i][0] + "-" + tableflight[i][1] + " " + tableflight[i][2]);
                }


                if (dialogNewBookingState[mes.DialogId] == 3 && arr.Contains(mes.Text))
                {
                    if (!UserDataDictionary.ContainsKey(mes.DialogId))
                    {
                        UserDataDictionary.Add(mes.DialogId, new Dictionary<string, string>() { { "Flight", mes.Text } });

                    }
                    else
                    {
                        if (UserDataDictionary[mes.DialogId].ContainsKey("Flight"))
                        {
                            UserDataDictionary[mes.DialogId]["Flight"] = mes.Text;

                        }
                        else
                        {
                            UserDataDictionary[mes.DialogId].Add("Flight", mes.Text);
                        }

                    }
                    return true;
                }

                return false;
            }

            bool ValidationFour(Message mes)
            {
                if (dialogNewBookingState[mes.DialogId] == 4)
                {

                    int numberseat;

                    if (int.TryParse(mes.Text, out numberseat) && numberseat <= 22 && numberseat > 0)
                    {
                        if (UserDataDictionary[mes.DialogId].ContainsKey("NumSeat"))
                        {
                            UserDataDictionary[mes.DialogId]["NumSeat"] = mes.Text;
                        }
                        else
                        {
                            UserDataDictionary[mes.DialogId].Add("NumSeat", mes.Text);
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

            bool ValidationFive(Message mes)
            {
                if (dialogNewBookingState[mes.DialogId] == 5)
                {


                    if (mes.Text == "Yes")
                    {
                        char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                        string numbooking = "";
                        Random rand = new Random();

                        for (int j = 0; j <= 6; j++)
                        {
                           
                            if (j == 0 || j == 1)
                            {
                                int num = rand.Next(0, letters.Length - 1);
                                numbooking += letters[num];
                            }
                            else
                            {
                                int num = rand.Next(0, 9);
                                numbooking += num;

                            }

                        }

                        if (!UserDataDictionary.ContainsKey(mes.DialogId))
                        {
                            UserDataDictionary.Add(mes.DialogId, new Dictionary<string, string>() { { "NumBooking", numbooking } });
                            return true;
                        }
                        else
                        {

                            if (UserDataDictionary[mes.DialogId].ContainsKey("NumBooking"))
                            {
                                UserDataDictionary[mes.DialogId]["NumBooking"] = numbooking;
                            }
                            else
                            {
                                UserDataDictionary[mes.DialogId].Add("NumBooking", numbooking);
                            }
                        }


                        return true;


                    }
                    else
                    {

                        clientState[mes.DialogId] = 0;
                        dialogBookingState[mes.DialogId] = 0;
                        dialogNewBookingState[mes.DialogId] = 0;

                        return false;

                    }
                }
                return false;
            }

        }

        public Message DialogBookingEdit (Message mes)
        {

            if (dialogEditBookingState.ContainsKey(mes.DialogId))
            {
                var state = dialogEditBookingState[mes.DialogId];

                if (textArrDialogEditBooking.Length > state && state >= 0)
                {
                    bool valid = false;
                    switch (state)
                    {
                        case 0:
                            valid = true;
                            break;
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
                       
                        mes.Text = textArrDialogEditBooking[--state];
                        return mes;

                    }

                    if (dialogEditBookingState[mes.DialogId] == 1)
                    {
                        var booking = UserDataDictionary.Values.Where(v => v.ContainsValue(mes.Text)).FirstOrDefault();
                       
                        if (!UserDataDictionary.ContainsKey(mes.DialogId))
                        {
                            UserDataDictionary.Add(mes.DialogId, booking );
                           
                        }
                        else
                        {
                           
                         UserDataDictionary[mes.DialogId] = booking;

                          
                        }

                        string textBooking = ButtonToJson(2, $"Змінити бронювання за маршрутом  {booking["Flight"]} Місце :{booking["NumSeat"]} ", new string[2] { "Відмінити", "Змінити місце" });
                        mes.TextTupe = "json";
                        textArrDialogEditBooking[state] = textBooking;
                    }
                    if (dialogEditBookingState[mes.DialogId] == 2)
                    {

                        if (mes.Text == "Відмінити")
                        {

                            UserDataDictionary[mes.DialogId].Remove("NumBooking");
                            mes.Text = "Бронювання відмінено!";
                            mes.TextTupe = "text";
                            clientState[mes.DialogId] = 0;
                            dialogBookingState[mes.DialogId] = 0;
                            dialogEditBookingState[mes.DialogId] = 0;

                            return mes;

                        }
                      

                    }

                    mes.Text = textArrDialogEditBooking[state];
                    dialogEditBookingState[mes.DialogId]++;


                    if (textArrDialogEditBooking.Length <= dialogEditBookingState[mes.DialogId])
                    {
                        clientState[mes.DialogId] = 0;
                        dialogBookingState[mes.DialogId] = 0;
                        dialogEditBookingState[mes.DialogId] = 0;

                    }
                    return mes;

                }
            }
            else
            {
                dialogEditBookingState.Add(mes.DialogId, 0);
                mes.Text = textArrDialogEditBooking[0];
                dialogEditBookingState[mes.DialogId]++;

            }


            return mes;

            bool ValidationOne (Message mes)
           {

                if (dialogEditBookingState[mes.DialogId] == 1)
                {
                    var numberBooking = mes.Text;
                    if (UserDataDictionary.Values.Any(v=>v.ContainsValue(numberBooking)))
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
            bool ValidationTwo(Message mes)
            {

                if (dialogEditBookingState[mes.DialogId] == 2 && ( mes.Text == "Відмінити" || mes.Text == "Змінити місце") )
                {

                    return true;


                }

                return false;

            }
            bool ValidationThree(Message mes)
            {
                if (dialogEditBookingState[mes.DialogId] == 3)
                {

                    int numberseat;

                    if (int.TryParse(mes.Text, out numberseat) && numberseat <= 44 && numberseat > 0)
                    {
                        if (UserDataDictionary[mes.DialogId].ContainsKey("NumSeat"))
                        {
                            UserDataDictionary[mes.DialogId]["NumSeat"] = mes.Text;
                        }
                        else
                        {
                            UserDataDictionary[mes.DialogId].Add("NumSeat", mes.Text);
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



        }

        public Message DialogRegister(Message mes)
        {


            
            if (dialogRegState.ContainsKey(mes.DialogId))
            {

                var state = dialogRegState[mes.DialogId];

                if (textArrDialogRegister.Length > state && state >= 0)
                {
                    bool valid = false;
                    switch (state)
                    {
                        case 0:
                            valid = true;
                            break;

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
                   if(UserDataDictionary.Values.Any(v => v.ContainsValue(numberBooking)))
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
                                UserDataDictionary[mes.DialogId]["NumBooking"] = mes.Text;
                            }
                            else
                            {
                                UserDataDictionary[mes.DialogId].Add("NumBooking", mes.Text);
                               
                            }
                            return true;
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
                        if (UserDataDictionary[mes.DialogId].ContainsKey("NumSeat"))
                        {
                            UserDataDictionary[mes.DialogId]["NumSeat"] = mes.Text;
                        }
                        else
                        {
                            UserDataDictionary[mes.DialogId].Add("NumSeat", mes.Text); 
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

        public Message EmployeeMethod(Message mes)
        {
            var emp = _context.Employees.Where(w => w.StatusOnline == true).FirstOrDefault();

            if (emp != null)
            {
                Dialog thisDialog = _context.Dialogs.Where(w => w.DialogId == mes.DialogId).FirstOrDefault();
                thisDialog.EmployeeUserUserId = emp.UserUserId;
                _context.SaveChanges();
                mes.Text = $"Перемикаю на оператора {emp.User.FirstName}";
            }
            else
            {
                mes.Text = "На даний момент немає доступних працівників";
                clientState[mes.DialogId] = 0;

            }
            return mes;
        }
        public string ButtonToJson (int i, string text, string [] buttontext = null)
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
