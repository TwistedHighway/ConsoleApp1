using static System.String;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = Contacts.Split(Environment.NewLine); 
            var lineCount = 0;
            var contactList = new List<Contact>();

            foreach (var i in lines)
            {
                lineCount++;
                var item = i.Split(',');
                
                if (!Guid.TryParse(item[0], out Guid validId)) continue;
                
                var fName = CleanName(item[1]);
                var lName = CleanName(item[2]);
                var phone = CleanPhone(item[3]);
                // Might be another item, we can handle it differently

                if (IsNullOrEmpty(phone)) continue;
                
                var newContact = new Contact()
                {
                    Id = validId,
                    FirstName = fName,
                    LastName = lName,
                    Phone = phone,
                };

                if(!contactList.Contains(newContact))
                    contactList.Add(newContact);
                Console.WriteLine($"Contact: {fName} {lName} {phone}");
            }

            Console.WriteLine($"Total Contacts: {contactList.Count}");
            Console.WriteLine($"Total rows parsed: {lineCount-1}");// don't count the first one
        }
        

        private static string CleanName(string name)
        {
            var contactName = name.Trim();
            return !IsNullOrEmpty(contactName) ? contactName : "***";
        }
        
        private static string CleanPhone(string phoneNumber)
        {
            var phone = phoneNumber.Trim();
            phone = phone.Replace(" ", "");
            phone = phone.Replace("(", "");
            phone = phone.Replace(")", "");
            phone = phone.Replace("-", "");
            
            // Could easily be made to handle the extra name. 
            // TODO: add code to append name together. 

            if(IsNullOrEmpty(phone) || phone.Length < 10) return "";
            
            // might not be a number
            var nanAreaCode = int.TryParse(phone.Substring(0, 3), out int areaCode);
            var nanPrefix = int.TryParse(phone.Substring(3, 3), out int prefix);
            var nanRemainder = int.TryParse(phone.Substring(6, 4), out int remainder);

            if (!nanAreaCode || !nanPrefix || !nanRemainder) return "";

            var formattedPhone = $"({areaCode}) {prefix}-{remainder}" ;
            
            return !IsNullOrEmpty(phone) ? formattedPhone : "";
        }
        
        /// <summary>
        /// Contact - complete with first name, last name and phone number
        /// </summary>
        public class Contact
        {
            public string FirstName { get; set; } = Empty;
            public string LastName { get; set; } = Empty;
            public string Phone { get; set; } = Empty;
            public Guid Id { get; set; }

        }
        
        private static readonly string Contacts = @"Id,FirstName,LastName,PhoneNumber 
                72F740AD-D949-41FE-945E-8801AAD04748,Jessica,Avery,202-555-0149
                48EDF105-6610-44E6-A680-5D3503EAF4A2,Benjamin,Hemmings,2025550129
                2A97E6D8-D0D7-4B2E-9B54-1908C96A9FE4,Grace,,202-555-0130
                9F937783-DC28-421C-A937-A0C201643AAE,Oliver,Wendell,202-555-3484
                4463CE30-041E-4EBF-8231-63443E36E281,Emily,Smith,   
                2CE36345-65A3-401A-B504-A3290162041,Joseph,Lummings,202-555-9351
                A203D9EA-79EF-4274-435F-C1CDABADD908,Robert,Ebert,(202) 555-0149
                0AF60567-DD6E-40D6-1FBE-417A25D1D908,,,202-555-2173
                7745FA91-E7F6-47D5-AB87-0349445D5F0F,Tim,Payne,
                834EF63A-DA43-4721-9DCA-15468ABC129E, ,Johnson,202-555-3484
                57F9A815-6044-4768-943D-8F7BD1D9CAE2,Elyse,Jenkins,202-555-0129
                B8E6C4AD-EC28-42FD-53FB-1C233F80DA08,Jennifer,Coolidge,Kirkland,202-555-7654";
    }


}
