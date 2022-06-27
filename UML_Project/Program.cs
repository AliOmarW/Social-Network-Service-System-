using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// SSN  Project
namespace Social_NetWork_Service
{
    [Serializable]
    public class Person
    {
        private string first_name, last_name, location;
        private int age;
        public Person(string first_name, string last_name, string location, int age)
        {
            this.first_name = first_name;
            this.last_name = last_name;
            this.location = location;
            this.age = age;
        }
        public void Set_firstname(string s)
        {
            first_name = s;
        }
        public string Get_firstname()
        {
            return first_name;
        }
        public void Set_lastname(string s)
        {
            last_name = s;
        }
        public string Get_lastname()
        {
            return last_name;
        }
        public void Set_location(string s)
        {
            location = s;
        }
        public string Get_location()
        {
            return location;
        }
        public void Set_Age(int a)
        {
            age = a;
        }
        public int Get_Age()
        {
            return age;
        }
    }
    [Serializable]
    public class User_Account : Person 
    {
        private string password, username, status;
        List<string> friends_usernames = new List<string>();
        private int num_of_times_reported = 0;

        public User_Account(string username, string first_name, string last_name, string location, string status, string password, int age, string[] friends_usernames, int length) : base(first_name, last_name, location, age)
        {
            this.username = username;
            this.status = status;
            this.password = password;
            for (int i = 0; i < length; i++)
            {
                this.friends_usernames.Add(friends_usernames[i]);
            }

            BinaryFormatter bf = new BinaryFormatter();

            FileStream UserAccounts_File = null;
            try
            {                      
                UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Append, FileAccess.Write);
                bf.Serialize(UserAccounts_File, this);
                
            }
            catch (Exception FileNotFoundException) 
            {
                UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Create, FileAccess.Write);
                bf.Serialize(UserAccounts_File, this);
            }
            finally
            {
                try
                {
                    UserAccounts_File.Close();
                }
                catch (Exception NullReferenceException) { }
            }


        }

        public void Set_username(string s)
        {
            username = s;
        }
        public string Get_username()
        {
            return username;
        }
        public void Set_password(string p)
        {
            password = p;
        }
        public string Get_password()
        {
            return password;
        }
        public void Set_Status(string s)
        {
            this.status = s;
        }
        public string Get_Status()
        {
            return status;
        }
        public void Set_Number_of_times_reported(int s)
        {
            num_of_times_reported = s;
        }
        public int Get_Number_of_times_reported()
        {
            return num_of_times_reported;
        }
        public List<string> Get_friends_usernames()
        {
            return friends_usernames;
        }



        public void Set_Post(string username, string content, string Category)  
        {
            Post post = new Post(username, content, Category);
            BinaryFormatter bf = new BinaryFormatter();

            // Username posts
            {
                // Read all data of username_posts
                FileStream username_posts_file = null;
                try
                {
                    username_posts_file = new FileStream(username + "POSTS" + ".txt", FileMode.Append, FileAccess.Write);
                    bf.Serialize(username_posts_file, post);
                }
                catch (Exception FileNotFoundException)
                {
                    username_posts_file = new FileStream(username + "POSTS" + ".txt", FileMode.Create, FileAccess.Write);
                    bf.Serialize(username_posts_file, post);
                }
                finally
                {
                    try
                    {
                        username_posts_file.Close();
                    }
                    catch (Exception NullReferenceException) { }
                }

            }
            // Database
            {
                // Read all data of database
                FileStream database_posts_file = null;
                try
                {
                    database_posts_file = new FileStream("Posts_File.txt", FileMode.Append, FileAccess.Write);
                    bf.Serialize(database_posts_file, post);
                }
                catch (Exception FileNotFoundException)
                {
                    database_posts_file = new FileStream("Posts_File.txt", FileMode.Create, FileAccess.Write);
                    bf.Serialize(database_posts_file, post);
                }
                finally
                {
                    try
                    {
                        database_posts_file.Close();
                    }
                    catch (Exception NullReferenceException) { }
                }

            }
        }   

        // User Functions 
        public void Post_new_content() 
        {
            Console.WriteLine("[Posting New Content]");
            Console.Write("-Please Enter Post Content : ");
            string Content = Console.ReadLine();
            Console.Write("-Please Enter Post Category : ");
            string Category = Console.ReadLine();
            Set_Post(username, Content, Category);
            Console.WriteLine("You have successfully posted a new content!");
        }  
        public void Send_message() 
        {
            // Displaying all the active useraccounts 
            Table Active_Table  = new Table(); 
            Active_Table.SetHeaders("Active Accounts");

            BinaryFormatter bf = new BinaryFormatter();

            FileStream UserAccounts_File = null;
            List<string> active_accounts = new List<string>();
            List<string> all_accounts = new List<string>();
            int num = 1;
            try
            {
                UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Open, FileAccess.Read);
                while (UserAccounts_File.Position < UserAccounts_File.Length)
                {
                    User_Account account = (User_Account)bf.Deserialize(UserAccounts_File);
                    if (account.Get_Status() == "Active" || account.Get_Status() == "active")
                    {
                        Active_Table.AddRow(account.Get_username());
                        active_accounts.Add(account.Get_username());
                        num++;
                    }
                    all_accounts.Add((account.Get_username()));
                }
            }
            catch (Exception FileNotFoundException)
            {
                Console.WriteLine("There are no active accounts.");
            }
            finally
            {
                try
                {
                    UserAccounts_File.Close();
                }
                catch (Exception NullReferenceException) { }
            }
            //...........
            if (num != 1)
            {
                Active_Table.SetHeaders("Active Accounts");
                Console.WriteLine(Active_Table.ToString());
            }

            // Getting input
            Console.WriteLine("[Sending New Message]");
            Console.Write("[1]- Please enter the reciever's User Name: ");
            string reciever = Console.ReadLine();
            bool found = false;
            while (!found)
            {
                bool Is_User = false;
                foreach(string account in all_accounts)
                {
                    if (account == reciever)
                    {
                        Is_User = true;
                    }
                }
                if (!Is_User) // if not correct user account
                {
                    Console.WriteLine("The account with username {0} doesn't exist!", reciever);
                    Console.Write("-Re-enter the reciever's User Name: ");
                    reciever = Console.ReadLine();
                    continue;
                }

                foreach (string name in active_accounts)
                {
                    if (reciever == name)
                    {
                        found = true;
                    }
                }

                if (!found) // if not active user account
                {
                    Console.WriteLine("The account with username {0} is not active account!", reciever);
                    Console.Write("-Re-enter the reciever's User Name: ");
                    reciever = Console.ReadLine();
                }
            }
           
            Console.Write("[2]- Please enter the subjects: ");
            string subject = Console.ReadLine();

            Console.Write("[3]- Please enter the priority: ");
            string priority = Console.ReadLine();
            while (true)
            {
                if (priority == "High" || priority == "high" || priority == "low" || priority == "Low") break;
                Console.WriteLine("incorrect priority, the priority should be High/Low.");
                Console.Write("-Please re-enter the priority: ");
                priority = Console.ReadLine();
            }

            Console.Write("[4]- Please enter the text: ");
            string text = Console.ReadLine();

            Message message = new Message(username, reciever, text, priority, subject); // Target
            message.Set_Time(DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss")); // Time Sent (used for priority)

            //..........

            // saving message to database_messages
            FileStream Message_File = null;
            try
            {
                Message_File = new FileStream("Message_File.txt", FileMode.Append, FileAccess.Write);
                bf.Serialize(Message_File, message);
            }
            catch (Exception FileNotFoundException)
            {
                Message_File = new FileStream("Message_File.txt", FileMode.Create, FileAccess.Write);
                bf.Serialize(Message_File, message);
            }
            finally
            {
                try
                {
                    Message_File.Close();
                }
                catch (Exception NullReferenceException) { }
            }
            //.............


            // Saving sent_message to reciever_messsages
            FileStream Reciever_messages = null;
            try
            {
                Reciever_messages = new FileStream(reciever + "RecievedMESSAGES" + ".txt", FileMode.Append, FileAccess.Write);
                bf.Serialize(Reciever_messages, message);
            }
            catch (Exception FileNotFoundException)
            {
                Reciever_messages = new FileStream(reciever + "RecievedMESSAGES" + ".txt", FileMode.Create, FileAccess.Write);
                bf.Serialize(Reciever_messages, message);
            }
            finally
            {
                try
                {
                    Reciever_messages.Close();
                }
                catch (Exception NullReferenceException) { }
            }
            //.......


            // Saving sent_message to sender_messages
            FileStream username_messages = null;
            try
            {
                username_messages = new FileStream(username + "SENTMESSAGES" + ".txt", FileMode.Append, FileAccess.Write);
                bf.Serialize(username_messages, message);
            }
            catch (Exception FileNotFoundException)
            {
                username_messages = new FileStream(username + "SENTMESSAGES" + ".txt", FileMode.Create, FileAccess.Write);
                bf.Serialize(username_messages, message);
            }
            finally
            {
                try
                {
                    username_messages.Close();
                }
                catch (Exception NullReferenceException) { }
            }

            Console.WriteLine("A message to {0} has been successfully sent!", reciever);
            //.............
        }  
        public void View_all_my_posts()
        {
            Table Posts_table = new Table();
            Posts_table.SetHeaders("Content", "Category");
            bool posted = false;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream username_posts = null;
            try
            {
                username_posts = new FileStream(username + "POSTS" + ".txt", FileMode.Open, FileAccess.Read);
                while (username_posts.Position < username_posts.Length)
                {
                    Post post = (Post)bf.Deserialize(username_posts);
                    Posts_table.AddRow(post.Get_content(), post.Get_category());
                    posted = true;
                }
            }
            catch (Exception FileNotFoundException)
            {
                Console.WriteLine("You haven't posted anything yet.");
            }
            finally
            {
                try
                {
                    username_posts.Close();
                }
                catch (Exception NullReferenceException) { }
            }

            if (posted)
            {
                Console.WriteLine("[Viewing all {0} posts]", this.Get_username());
                Console.WriteLine(Posts_table.ToString());
            }
        }  
        public void View_all_recieved_messages() 
        {
            Table Messages_table = new Table();
            Messages_table.SetHeaders("Priority","First Name", "Last Name", "Subject", "Text", "Time");

            BinaryFormatter bf = new BinaryFormatter();
            FileStream Recieved_messages = null;
            // Null reference exception
            try
            {
                Recieved_messages = new FileStream(this.Get_username() + "RecievedMESSAGES" + ".txt", FileMode.Open, FileAccess.Read);

                List<Message> messages = new List<Message>();
                while (Recieved_messages.Position < Recieved_messages.Length)
                {
                    messages.Add((Message)bf.Deserialize(Recieved_messages));
                }

                // Sorting all Recieved messages based on their Priority (high/low) and Time
                Comparison<Message> comparison = new Comparison<Message>(main.ComparePriority);
                messages.Sort(comparison);

                foreach (Message msg in messages)
                {
                    string Sender_user_name = msg.Get_sender_username();
                    FileStream UserAccounts_File = null;
                    try
                    {
                        UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Open, FileAccess.Read);
                        while (UserAccounts_File.Position < UserAccounts_File.Length)
                        {
                            User_Account user_account = (User_Account)bf.Deserialize(UserAccounts_File);
                            if (user_account.Get_username() == Sender_user_name)
                            {
                                Messages_table.AddRow(msg.Get_priority(), user_account.Get_firstname(), user_account.Get_lastname(), msg.Get_subject(), msg.Get_text(), msg.Get_Time());
                            }
                        }
                    }
                    catch (Exception FileNotFoundException) { }
                    finally
                    {
                        try
                        {
                            UserAccounts_File.Close();
                        }
                        catch (Exception NullReferenceException) { }
                    }
                }
                Console.WriteLine("[Viewing {0} Recieved Messages]",this.Get_username());
                Console.WriteLine(Messages_table.ToString());
            }
            catch (Exception FileNotFoundException)
            {
                Console.WriteLine("No Messages Recieved Yet...");
            }
            finally
            {
                try
                {
                    Recieved_messages.Close();
                }
                catch (Exception NullReferenceException) { }
            }

        }  
        public void View_my_last_updated_wall() 
        {
            // Displays all posts from all friends of the current logged in user account...
            foreach (string username in friends_usernames)
            {
                string fn = "", ln = "";

                BinaryFormatter bf = new BinaryFormatter();
                FileStream UserAccounts_File = null;
                try
                {
                    UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Open, FileAccess.Read);
                    while (UserAccounts_File.Position < UserAccounts_File.Length)
                    {
                        User_Account user_account = (User_Account)bf.Deserialize(UserAccounts_File);
                        if (user_account.Get_username() == username)
                        {
                            fn = user_account.Get_firstname();
                            ln = user_account.Get_lastname();
                        }
                    }
                }
                catch (Exception FileNotFoundException)
                { }
                finally
                {
                    try
                    {
                        UserAccounts_File.Close();
                    }
                    catch (Exception NullReferenceException) { }
                }
                Table ithFriend_Post_Table = new Table();

                bool found = false;
                FileStream friend_posts = null;
                try
                {
                    friend_posts = new FileStream(username + "POSTS" + ".txt", FileMode.Open, FileAccess.Read);
                    while (friend_posts.Position < friend_posts.Length)
                    {
                        Post post = (Post)bf.Deserialize(friend_posts);
                        ithFriend_Post_Table.AddRow(fn,ln,post.Get_content(), post.Get_category());
                        found = true;
                    }
                }
                catch (Exception FileNotFoundException) { }
                finally
                {
                    try
                    {
                        friend_posts.Close();
                    }
                    catch (Exception NullReferenceException) { }
                }
                if (found)
                {
                    Console.WriteLine("[Displaying {0} Posts]", username);
                    ithFriend_Post_Table.SetHeaders("First Name", "Last Name", "Content", "Category");
                    Console.WriteLine(ithFriend_Post_Table.ToString());
                    Console.WriteLine();
                }

            }

        }  
        public void Filter_My_Wall() 
        {
            // 1. Displays all posts
            this.View_my_last_updated_wall();
            Console.WriteLine();

            // 2. Enter Category 
            Console.Write("-Please enter the category: ");
            string Category = Console.ReadLine();

            // 3. Displays all posts from all friends basd on the entered category in the same way as case 9
            foreach (string username in friends_usernames)
            {
                string fn = "", ln = "";

                BinaryFormatter bf = new BinaryFormatter();
                FileStream UserAccounts_File = null;
                try
                {
                    UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Open, FileAccess.Read);
                    while (UserAccounts_File.Position < UserAccounts_File.Length)
                    {
                        User_Account user_account = (User_Account)bf.Deserialize(UserAccounts_File);
                        if (user_account.Get_username() == username)
                        {
                            fn = user_account.Get_firstname();
                            ln = user_account.Get_lastname();
                        }
                    }
                }
                catch (Exception FileNotFoundException) { }
                finally
                {
                    try
                    {
                        UserAccounts_File.Close();
                    }
                    catch (Exception NullReferenceException) { }
                }

                bool found = false;
                Table ithFriend_Post_Table = new Table();
                FileStream friend_posts = null;
                try
                {
                    friend_posts = new FileStream(username + "POSTS" + ".txt", FileMode.Open, FileAccess.Read);
                    while (friend_posts.Position < friend_posts.Length)
                    {
                        Post post = (Post)bf.Deserialize(friend_posts);
                        if (post.Get_category() == Category)
                        {
                            ithFriend_Post_Table.AddRow(fn, ln, post.Get_content(), post.Get_category());
                            found = true;
                        }
                    }
                }
                catch (Exception FileNotFoundException) { }
                finally
                {
                    try
                    {
                        friend_posts.Close();
                    }
                    catch (Exception NullReferenceException) { }
                }

                if (found)
                {
                    Console.WriteLine("[Displaying {0} {1} Posts]", username, Category);
                    ithFriend_Post_Table.SetHeaders("First Name", "Last Name", "Content", "Category");
                    Console.WriteLine(ithFriend_Post_Table.ToString());
                    Console.WriteLine();
                }
            }

        } 
        public void Send_Report_to_administrator() 
        {
            // 1. Displays all active user accounts 
            BinaryFormatter bf = new BinaryFormatter();
            List<string> user_accounts = new List<string>();
            {
                Table Active_Table = new Table();
                Active_Table.SetHeaders("Active Accounts");
                FileStream UserAccounts_File = null;
                int cnt = 1;
                try
                {
                    UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Open, FileAccess.Read);
                    Console.WriteLine("Displaying the usernames of Active User Accounts: ");
                    while (UserAccounts_File.Position < UserAccounts_File.Length)
                    {
                        User_Account account = (User_Account)bf.Deserialize(UserAccounts_File);
                        if (account.Get_Status() == "Active" || account.Get_Status() == "active")
                        {
                            Active_Table.AddRow(account.Get_username());
                            cnt++;
                        }
                        user_accounts.Add(account.Get_username());
                    }
                }
                catch (Exception FileNotFoundException)
                {
                    Console.WriteLine("There are no user accounts.");
                }
                finally
                {
                    try
                    {
                        UserAccounts_File.Close();
                    }
                    catch (Exception NullReferenceException) { }
                }

                if (cnt != 1)
                {
                    Active_Table.SetHeaders("Active Accounts");
                    Console.WriteLine(Active_Table.ToString());
                }
            }
            //........


            // 2. Enter user_name of the account to report
            Console.WriteLine("[Sending Report To Administrator]");
            Console.Write("-Please enter the user name of the account you want to report: ");
            string user_name = Console.ReadLine();
            bool Valid_User_Name = false;
            while (!Valid_User_Name)
            {
                foreach (string account in user_accounts)
                {
                    if (account == user_name)
                    {
                        Valid_User_Name = true;
                    }
                }
                if (Valid_User_Name == true) continue;
                Console.WriteLine("An account with user name {0} doesn't exist!", user_name);
                Console.Write("-Re-enter the user name of the account you want to report: ");
                user_name = Console.ReadLine();
            }

            Report report = new Report(user_name, this.Get_username());
            //.......

            bool Suspended = false;
            {
                // Getting all new data to accounts list
                List<User_Account> accounts = new List<User_Account>();
                FileStream UserAccounts_File = null;
                try
                {
                    UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Open, FileAccess.Read);
                    while (UserAccounts_File.Position < UserAccounts_File.Length)
                    {
                        User_Account user_account = (User_Account)bf.Deserialize(UserAccounts_File);
                        if (user_account.Get_username() == user_name)
                        {
                            if (user_account.Get_Status() == "Suspended" || user_account.Get_Status() == "suspended")
                            {
                                Suspended = true;
                            } 
                            else
                            {
                                user_account.Set_Number_of_times_reported(user_account.Get_Number_of_times_reported() + 1);
                            }
                        }
                        accounts.Add(user_account);
                    }
                }
                catch (Exception FileNotFoundException) { }
                finally {
                    try
                    {
                        UserAccounts_File.Close();
                    }
                    catch (Exception NullReferenceException) { }
                }


                // Adding all new data
                FileStream File = new FileStream("UserAccounts_File.txt", FileMode.Create, FileAccess.Write);
                foreach (User_Account acc in accounts)
                {
                    bf.Serialize(File, acc);
                }
                File.Close();
            }

            if (Suspended)
            {
                Console.WriteLine("The account with user name {0} is already suspended.", user_name);
                return;
            }

            // Add report to reports database
            FileStream Reports = null;
            try
            {
                Reports = new FileStream("Reports_file.txt", FileMode.Append, FileAccess.Write);
                bf.Serialize(Reports, report);
            }
            catch (Exception FileNotFoundException)
            {
                Reports = new FileStream("Reports_file.txt", FileMode.Create, FileAccess.Write);
                bf.Serialize(Reports, report);
            }
            finally
            {
                try
                {
                    Reports.Close();
                }
                catch (Exception NullReferenceException) { }
            }

            Console.WriteLine("The account {0} has been reported successfully!", user_name);
        } 
    }
    public class Admin_Account : Person 
    {
       
        private string username, password;
        public Admin_Account(string username, string password) : base("", "", "", 0)
        {
            this.username = username;
            this.password = password;
        }
        public void Set_username(string s)
        {
            username = s;
        }
        public string Get_username()
        {
            return username;
        }
        public void Set_password(string p)
        {
            password = p;
        }
        public string Get_password()
        {
            return password;
        }


        public void Register_new_user_account()  
        {
            Console.WriteLine("Please Fill The Following Information: ");

            List<string> user_accounts = new List<string>();
            BinaryFormatter bf = new BinaryFormatter();

            FileStream UserAccounts_File = null;
            try
            {
                UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Open, FileAccess.Read);
                while (UserAccounts_File.Position < UserAccounts_File.Length)
                {
                    User_Account account = (User_Account)bf.Deserialize(UserAccounts_File);
                    user_accounts.Add(account.Get_username());
                }
            }
            catch (Exception FileNotFoundException)
            {}
            finally
            {
                try
                {
                    UserAccounts_File.Close();
                }
                catch (Exception NullReferenceException) { }
            }

            Console.Write("1- Username : ");
            string username = Console.ReadLine();
            while (true)
            {
                bool unique = true;
                foreach (string account in user_accounts)
                {
                    if (account == username) unique = false;
                }
                if (!unique)
                {
                    Console.WriteLine("an account with user {0} already exists!", username);
                    Console.Write("-Re-enter Username : ");
                    username = Console.ReadLine();
                }
                else break;
            } // checking uniqueness


            Console.Write("2- Password : ");
            string password = Console.ReadLine();

            Console.Write("3- Status : ");
            string status = Console.ReadLine();
            bool valid_status = false;
            while (true)
            {
                if (status == "Active" || status == "active" || status == "suspended" || status == "Suspended") break;
                Console.WriteLine("Invalid status, status should be Active/Suspended!");
                Console.Write("-Please re-enter the status: ");
                status = Console.ReadLine();
            }

            Console.Write("4- First Name : ");
            string First_Name = Console.ReadLine();

            Console.Write("5- Last Name : ");
            string Last_Name = Console.ReadLine();

            Console.Write("6- Location : ");
            string Location = Console.ReadLine();


            Console.Write("7- Age : ");
            string input = Console.ReadLine();
            int age = -1;
            while (true)
            {
                try // handles correct input (integer not string)
                {

                    try // handles overflow
                    {
                        age = Int32.Parse(input);
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("Woah! are you sure he is that old..? i don't think so...");
                        Console.Write("-Re-enter age: ");
                        input = Console.ReadLine();
                        continue;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("invalid age, [{0}] is not an integer!", input);
                    Console.Write("-Re-enter age: ");
                    input = Console.ReadLine();
                    continue;
                }
                if (age < 0)
                {
                    Console.WriteLine("invalid age, the age must be positive!");
                    Console.Write("-Re-enter age: ");
                    input = Console.ReadLine();
                }
                else break;
            } 

            Console.Write("8- Enter the number of friends : ");
            string number = Console.ReadLine();
            int num = -1;
            while (true)
            {
                try
                {
                    try // handles overflow
                    {
                        num = Int32.Parse(number);
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("Woah! that's so many friends, i don't believe you!");
                        Console.Write("-Re-enter the number of friends: ");
                        number = Console.ReadLine();
                        continue;
                    }
                }
                catch (FormatException) // handles correct input
                {
                    Console.WriteLine("invalid number, [{0}] is not an integer!", number);
                    Console.Write("-Re-enter the number of friends: ");
                    number = Console.ReadLine();
                    continue;
                }
                if (num < 0)
                {
                    int copy = num;
                    int num_of_digits = 0;
                    while(copy > 0)
                    {
                        num_of_digits++;
                        copy /= 10;
                    }
                    if (num_of_digits > 6)
                    {
                        Console.WriteLine("Woah! that's so many friends, i don't believe you!");
                        Console.Write("-Re-enter the number of friends: ");
                        number = Console.ReadLine();
                        continue;
                    }
                    Console.WriteLine("invalid number, the number of friends must be positive!");
                    Console.Write("-Re-enter the number of friends: ");
                    number = Console.ReadLine();
                }
                else break;
            }

            string[] friends = new string[num];
            for (int i = 0; i < num; i++)
            {
                Console.Write("{0}- Enter User Name : ", i + 1);
                friends[i] = Console.ReadLine();
            }
            User_Account New_User = new User_Account(username, First_Name, Last_Name, Location, status, password, age, friends, num);
            Console.WriteLine("The account with user name {0} has been successfully registered!", username);
        } 
        public void View_All_User_Accounts() 
        {
            var table = new Table();
            List<User_Account> accounts = new List<User_Account>();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream UserAccounts_File = null;
            try
            {
                UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Open, FileAccess.Read);
                while (UserAccounts_File.Position < UserAccounts_File.Length)
                {
                    User_Account USER = (User_Account)bf.Deserialize(UserAccounts_File);
                    accounts.Add(USER);
                }
            }
            catch (Exception FileNotFoundException)
            { }
            finally
            {
                try
                {
                    UserAccounts_File.Close();
                }
                catch (Exception NullReferenceException) { }
            }

            bool has = false;
            foreach (User_Account account in accounts)
            {
                has = true;
                string all_friends = "";
                int n = account.Get_friends_usernames().Count;
                int i = 0;
                foreach (string friend in account.Get_friends_usernames())
                {
                    all_friends += friend;
                    if (i != n - 1)
                    {
                        all_friends += ',';
                    }
                    i++;
                }
                string q = account.Get_Age().ToString();
                table.AddRow(account.Get_username(), account.Get_password(), account.Get_Status(), account.Get_firstname(), account.Get_lastname(), account.Get_location(),q, all_friends);
            }
            if (!has)
            {
                Console.WriteLine("No User Accounts has been registered yet..");
            } else
            {
                table.SetHeaders("Username", "password", "Status", "First Name", "Last Name", "Location", "Age", "Friends");
                Console.WriteLine("[Viewing All Registered User Accounts]");
                Console.WriteLine(table.ToString());
            }

        } 
        public void Suspend_User_Account() 
        {
            BinaryFormatter bf = new BinaryFormatter();
            // 1. Displaying All reports: 
            {
                Table Reports_Table = new Table();
                int num = 1;
                FileStream Reports_file = null;
                try
                {
                    Reports_file = new FileStream("Reports_file.txt", FileMode.Open, FileAccess.Read);
                    while (Reports_file.Position < Reports_file.Length)
                    {
                        Report report = (Report)bf.Deserialize(Reports_file);
                        string add = "The User " + report.Get_reporter_username() + " Has Reported The User " + report.Get_reported_user_account();
                        string x = num.ToString();
                        Reports_Table.AddRow(x, add);
                        num++;
                    }
                }
                catch (Exception FileNotFoundException)
                {
                    Console.WriteLine("No Reports were found...");
                }
                finally
                {
                    try
                    {
                        Reports_file.Close();
                    }
                    catch (Exception NullReferenceException)
                    { }
                }

                if (num != 1)
                {
                    Reports_Table.SetHeaders("Number","             All Reports");
                    Console.WriteLine(Reports_Table.ToString());
                }
            }

            List<User_Account> accounts_reported_2above = new List<User_Account>();
            bool ret = false;
            // 2. Displaying Usernames that have been reported at least 2 times
            {
                Table Two_Times_Reports_Table = new Table();
                FileStream UserAccounts_File = null;
                try
                {
                    UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Open, FileAccess.Read);
                    int num = 1;
                    while (UserAccounts_File.Position < UserAccounts_File.Length)
                    {
                        User_Account user_Account = (User_Account)bf.Deserialize(UserAccounts_File);
                        if (user_Account.Get_Number_of_times_reported() >= 2)
                        {
                            string x = num.ToString();
                            Two_Times_Reports_Table.AddRow(x, user_Account.Get_username());
                            accounts_reported_2above.Add(user_Account);
                            num++;
                        }
                    }
                    if (num == 1)
                    {
                        Console.WriteLine("There were no accounts reported at least 2 times...");
                        ret = true;
                    } else
                    {
                        Console.WriteLine("Displaying User Names of the Accounts that have been reported at least 2 times: ");
                        Two_Times_Reports_Table.SetHeaders("Number", "      Username");
                        Console.WriteLine(Two_Times_Reports_Table.ToString());
                    }
                }
                catch (Exception FileNotFoundException)
                { }
                finally
                {
                    try
                    {
                        UserAccounts_File.Close();
                    }
                    catch (Exception NullReferenceException) { }
                }
                //...
            }
            if (ret)
            {
                return;
            }

            // 3. Enter User Name
            Console.Write("-Please enter the user name of the account to be suspended: ");
            string User_Name = Console.ReadLine();
            //....

            bool Valid_User_Name = false;
            bool Suspended = false;
            // 4. Change the status to suspended (Change User_Accounts Database)
            {
                // Changing all Data 
                List<User_Account> accounts = new List<User_Account>();
                FileStream UserAccounts_File = null;
                try
                {
                    UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Open, FileAccess.Read);
                    while (UserAccounts_File.Position < UserAccounts_File.Length)
                    {
                        User_Account user_account = (User_Account)bf.Deserialize(UserAccounts_File);
                        if (user_account.Get_username() == User_Name)
                        {
                            if (user_account.Get_Status() == "Suspended" || user_account.Get_Status() == "suspended")
                            {
                                Suspended = true;
                            }
                            else
                            {
                                user_account.Set_Status("Suspended");
                            }
                            Valid_User_Name = true;
                        }
                        accounts.Add(user_account);
                    }
                }
                catch (Exception FileNotFoundException) { }
                finally {
                    try
                    {
                        UserAccounts_File.Close();
                    }
                    catch (Exception NullReferenceException) { }
                }



                // Adding all new data
                FileStream File = new FileStream("UserAccounts_File.txt", FileMode.Create, FileAccess.Write);
                foreach (User_Account acc in accounts)
                {
                    bf.Serialize(File, acc);
                }
                File.Close();
            }


            // Should be 2 times to suspend
            if (!Valid_User_Name)
            {
                Console.WriteLine("There's no user account with user name {0}", User_Name);
                return;
            }
            if (Suspended)
            {
                Console.WriteLine("The account with user name {0} is already suspended.", User_Name);
                return;
            }

            bool in_the_list = false;
            foreach (User_Account account in accounts_reported_2above)
            {
                if (account.Get_username() == User_Name)
                {
                    in_the_list = true;
                }
            }
            if (!in_the_list)
            {
                Console.WriteLine("The account with user name {0} hasn't been reported 2 times or more.", User_Name);
                return;
            }

            // 5. Remove reports of the Selected user_account from database (Change Reports Database)
            {
                // Changing all data
                List<Report> all_reports = new List<Report>();
                FileStream Reports_file = null;
                try
                {
                    Reports_file = new FileStream("Reports_file.txt", FileMode.Open, FileAccess.Read);
                    while (Reports_file.Position < Reports_file.Length)
                    {
                        Report report = (Report)bf.Deserialize(Reports_file);
                        if (report.Get_reporter_username() == User_Name)
                        {
                            continue;
                        }
                        all_reports.Add(report);
                    }
                }
                catch (Exception FileNotFoundException) { }
                finally {
                    try
                    {
                        Reports_file.Close();
                    }
                    catch (Exception NullReferenceException) { }
                }


                // Adding all new data
                FileStream File = new FileStream("Reports_file.txt", FileMode.Create, FileAccess.Write);
                foreach (Report report in all_reports)
                {
                    bf.Serialize(File, report);
                }
                File.Close();
            }

            Console.WriteLine("The user account with user name {0} has been suspended successfully!", User_Name);

            Console.WriteLine();
        }  
        public void Activate_User_Accounts()  
        {
            BinaryFormatter bf = new BinaryFormatter();
            List<User_Account> Suspended_Accounts = new List<User_Account>();
            // 1- Display user names of all the suspended accounts
            {
                FileStream UserAccounts_File = null;
                Table Suspended_Table = new Table();
                bool ret = false;
                try
                {
                    UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Open, FileAccess.Read);
                    int num = 1;
                    while (UserAccounts_File.Position < UserAccounts_File.Length)
                    {
                        User_Account USER = (User_Account)bf.Deserialize(UserAccounts_File);
                        if (USER.Get_Status() == "Suspended" || USER.Get_Status() == "suspended")
                        {
                            Suspended_Accounts.Add(USER);
                            string x = num.ToString();
                            Suspended_Table.AddRow(x, USER.Get_username());
                            num++;
                        }
                    }
                    if (num == 1)
                    {
                        Console.WriteLine("There are no suspended accounts to activate..");
                        ret = true;
                    } else
                    {
                        Suspended_Table.SetHeaders("Number","Suspended Accounts");
                        Console.WriteLine(Suspended_Table.ToString());
                    }
                }
                catch (Exception FileNotFoundException)
                { }
                finally
                {
                    try
                    {
                        UserAccounts_File.Close();
                    }
                    catch (Exception NullReferenceException)
                    { }
                }
                if (ret)
                {
                    return;
                }
            }
            //....


            // 2- Entering User_name
            Console.Write("-Please enter the user name of the account to activate: ");
            string User_Name = Console.ReadLine();
            //....
            bool Valid_User_Name = false;
            bool Active = false;

            // 3- Change the status to active (Changing Data Base)
            {
                // Changing all Data 
                List<User_Account> accounts = new List<User_Account>();
                FileStream UserAccounts_File = null;
                try
                {
                    UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Open, FileAccess.Read);
                    while (UserAccounts_File.Position < UserAccounts_File.Length)
                    {
                        User_Account user_account = (User_Account)bf.Deserialize(UserAccounts_File);
                        if (user_account.Get_username() == User_Name)
                        {
                            if (user_account.Get_Status() == "Active" || user_account.Get_Status() == "active")
                            {
                                Active = true;
                            }
                            else 
                            {
                                user_account.Set_Status("Active");                 
                            }
                            Valid_User_Name = true;
                        }
                        accounts.Add(user_account);
                    }
                }
                catch (Exception FileNotFoundException) { }
                finally {
                    try
                    {
                        UserAccounts_File.Close();
                    }
                    catch (Exception NullReferenceException) { }
                }



                // Adding all new data
                FileStream File = new FileStream("UserAccounts_File.txt", FileMode.Create, FileAccess.Write);
                foreach (User_Account acc in accounts)
                {
                    bf.Serialize(File, acc);
                }
                File.Close();
            }
            //....
            if (!Valid_User_Name)
            {
                Console.WriteLine("There's no user account with user name {0}", User_Name);
                return;
            }
            if (Active)
            {
                Console.WriteLine("The account with user name {0} is already Active.", User_Name);
                return;
            }
            bool in_the_list = false;
            foreach (User_Account account in Suspended_Accounts)
            {
                if (account.Get_username() == User_Name)
                {
                    in_the_list = true;
                }
            }
            if (!in_the_list)
            {
                Console.WriteLine("The account with user name {0} isn't a part of the suspended accounts.", User_Name);
                return;
            }

            Console.WriteLine("The account with user name {0} has been successfully activated!", User_Name);
        }  
    }

    [Serializable]
    public class Post  
    {
        private string poster_name, content, category;
        public Post(string poster_name, string content, string category)
        {
            this.poster_name = poster_name;
            this.content = content;
            this.category = category;
        }
        public void Set_poster_user_name(string s)
        {
            poster_name = s;
        }
        public string Get_poster_user_name()
        {
            return poster_name;
        }
        public void Set_content(string s)
        {
            content = s;
        }
        public string Get_content()
        {
            return content;
        }
        public void Set_category(string s)
        {
            category = s;
        }
        public string Get_category()
        {
            return category;
        }
    }
    [Serializable]
    public class Message 
    {
        private string sender, reciever, text, priority, subject, time;
        public Message(string sender, string reciever, string text, string priority, string subject)
        {
            this.sender = sender;
            this.reciever = reciever;
            this.text = text;
            this.priority = priority;
            this.subject = subject;
        }
        public void Set_sender_username(string s)
        {
            sender = s;
        }
        public string Get_sender_username()
        {
            return sender;
        }

        public void Set_reciever_username(string s)
        {
            reciever = s;
        }
        public string Get_reciever_username()
        {
            return reciever;
        }

        public void Set_text(string s)
        {
            text = s;
        }
        public string Get_text()
        {
            return text;
        }

        public void Set_priority(string s)
        {
            priority = s;
        }
        public string Get_priority()
        {
            return priority;
        }

        public void Set_subject(string s)
        {
            subject = s;
        }
        public string Get_subject()
        {
            return subject;
        }

        public void Set_Time(string n)
        {
             time = n;
        }
        public string Get_Time()
        {
            return time;
        }

    }
    [Serializable]
    public class Report 
    {
        private string reported_user_account, reporter_user_name;
        public Report(string reported_user_account, string reporter_user_name)
        {
            this.reported_user_account = reported_user_account;
            this.reporter_user_name = reporter_user_name;
        }
        public void Set_reported_user_account(string s)
        {
            reported_user_account = s;
        }
        public string Get_reported_user_account()
        {
            return reported_user_account;
        }

        public void Set_reporter_user_name(string s)
        {
            reporter_user_name = s;
        }
        public string Get_reporter_username()
        {
            return reporter_user_name;
        }
    }

    

    public class Table // table class from https://github.com/BrunoVT1992/ConsoleTable (is used to print the data in table format)
    {
        private const string TopLeftJoint = "┌";
        private const string TopRightJoint = "┐";
        private const string BottomLeftJoint = "└";
        private const string BottomRightJoint = "┘";
        private const string TopJoint = "┬";
        private const string BottomJoint = "┴";
        private const string LeftJoint = "├";
        private const string MiddleJoint = "┼";
        private const string RightJoint = "┤";
        private const char HorizontalLine = '─';
        private const string VerticalLine = "│";

        private string[] _headers;
        private List<string[]> _rows = new List<string[]>();
        public int Padding { get; set; } = 1;
        public bool HeaderTextAlignRight { get; set; }
        public bool RowTextAlignRight { get; set; }

        public void SetHeaders(params string[] headers)
        {
            _headers = headers;
        }

        public void AddRow(params string[] row)
        {
            _rows.Add(row);
        }

        public void ClearRows()
        {
            _rows.Clear();
        }

        private int[] GetMaxCellWidths(List<string[]> table)
        {
            var maximumColumns = 0;
            foreach (var row in table)
            {
                if (row.Length > maximumColumns)
                    maximumColumns = row.Length;
            }

            var maximumCellWidths = new int[maximumColumns];
            for (int i = 0; i < maximumCellWidths.Count(); i++)
                maximumCellWidths[i] = 0;

            var paddingCount = 0;
            if (Padding > 0)
            {
                //Padding is left and right
                paddingCount = Padding * 2;
            }

            foreach (var row in table)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    var maxWidth = row[i].Length + paddingCount;

                    if (maxWidth > maximumCellWidths[i])
                        maximumCellWidths[i] = maxWidth;
                }
            }

            return maximumCellWidths;
        }

        private StringBuilder CreateTopLine(int[] maximumCellWidths, int rowColumnCount, StringBuilder formattedTable)
        {
            for (int i = 0; i < rowColumnCount; i++)
            {
                if (i == 0 && i == rowColumnCount - 1)
                    formattedTable.AppendLine(string.Format("{0}{1}{2}", TopLeftJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), TopRightJoint));
                else if (i == 0)
                    formattedTable.Append(string.Format("{0}{1}", TopLeftJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
                else if (i == rowColumnCount - 1)
                    formattedTable.AppendLine(string.Format("{0}{1}{2}", TopJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), TopRightJoint));
                else
                    formattedTable.Append(string.Format("{0}{1}", TopJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
            }

            return formattedTable;
        }

        private StringBuilder CreateBottomLine(int[] maximumCellWidths, int rowColumnCount, StringBuilder formattedTable)
        {
            for (int i = 0; i < rowColumnCount; i++)
            {
                if (i == 0 && i == rowColumnCount - 1)
                    formattedTable.AppendLine(string.Format("{0}{1}{2}", BottomLeftJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), BottomRightJoint));
                else if (i == 0)
                    formattedTable.Append(string.Format("{0}{1}", BottomLeftJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
                else if (i == rowColumnCount - 1)
                    formattedTable.AppendLine(string.Format("{0}{1}{2}", BottomJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), BottomRightJoint));
                else
                    formattedTable.Append(string.Format("{0}{1}", BottomJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
            }

            return formattedTable;
        }

        private StringBuilder CreateValueLine(int[] maximumCellWidths, string[] row, bool alignRight, StringBuilder formattedTable)
        {
            int cellIndex = 0;
            int lastCellIndex = row.Length - 1;

            var paddingString = string.Empty;
            if (Padding > 0)
                paddingString = string.Concat(Enumerable.Repeat(' ', Padding));

            foreach (var column in row)
            {
                var restWidth = maximumCellWidths[cellIndex];
                if (Padding > 0)
                    restWidth -= Padding * 2;

                var cellValue = alignRight ? column.PadLeft(restWidth, ' ') : column.PadRight(restWidth, ' ');

                if (cellIndex == 0 && cellIndex == lastCellIndex)
                    formattedTable.AppendLine(string.Format("{0}{1}{2}{3}{4}", VerticalLine, paddingString, cellValue, paddingString, VerticalLine));
                else if (cellIndex == 0)
                    formattedTable.Append(string.Format("{0}{1}{2}{3}", VerticalLine, paddingString, cellValue, paddingString));
                else if (cellIndex == lastCellIndex)
                    formattedTable.AppendLine(string.Format("{0}{1}{2}{3}{4}", VerticalLine, paddingString, cellValue, paddingString, VerticalLine));
                else
                    formattedTable.Append(string.Format("{0}{1}{2}{3}", VerticalLine, paddingString, cellValue, paddingString));

                cellIndex++;
            }

            return formattedTable;
        }

        private StringBuilder CreateSeperatorLine(int[] maximumCellWidths, int previousRowColumnCount, int rowColumnCount, StringBuilder formattedTable)
        {
            var maximumCells = Math.Max(previousRowColumnCount, rowColumnCount);

            for (int i = 0; i < maximumCells; i++)
            {
                if (i == 0 && i == maximumCells - 1)
                {
                    formattedTable.AppendLine(string.Format("{0}{1}{2}", LeftJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), RightJoint));
                }
                else if (i == 0)
                {
                    formattedTable.Append(string.Format("{0}{1}", LeftJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
                }
                else if (i == maximumCells - 1)
                {
                    if (i > previousRowColumnCount)
                        formattedTable.AppendLine(string.Format("{0}{1}{2}", TopJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), TopRightJoint));
                    else if (i > rowColumnCount)
                        formattedTable.AppendLine(string.Format("{0}{1}{2}", BottomJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), BottomRightJoint));
                    else if (i > previousRowColumnCount - 1)
                        formattedTable.AppendLine(string.Format("{0}{1}{2}", MiddleJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), TopRightJoint));
                    else if (i > rowColumnCount - 1)
                        formattedTable.AppendLine(string.Format("{0}{1}{2}", MiddleJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), BottomRightJoint));
                    else
                        formattedTable.AppendLine(string.Format("{0}{1}{2}", MiddleJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), RightJoint));
                }
                else
                {
                    if (i > previousRowColumnCount)
                        formattedTable.Append(string.Format("{0}{1}", TopJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
                    else if (i > rowColumnCount)
                        formattedTable.Append(string.Format("{0}{1}", BottomJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
                    else
                        formattedTable.Append(string.Format("{0}{1}", MiddleJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
                }
            }

            return formattedTable;
        }

        public override string ToString()
        {
            var table = new List<string[]>();

            var firstRowIsHeader = false;
            if (_headers?.Any() == true)
            {
                table.Add(_headers);
                firstRowIsHeader = true;
            }

            if (_rows?.Any() == true)
                table.AddRange(_rows);

            if (!table.Any())
                return string.Empty;

            var formattedTable = new StringBuilder();

            var previousRow = table.FirstOrDefault();
            var nextRow = table.FirstOrDefault();

            int[] maximumCellWidths = GetMaxCellWidths(table);

            formattedTable = CreateTopLine(maximumCellWidths, nextRow.Count(), formattedTable);

            int rowIndex = 0;
            int lastRowIndex = table.Count - 1;

            for (int i = 0; i < table.Count; i++)
            {
                var row = table[i];

                var align = RowTextAlignRight;
                if (i == 0 && firstRowIsHeader)
                    align = HeaderTextAlignRight;

                formattedTable = CreateValueLine(maximumCellWidths, row, align, formattedTable);

                previousRow = row;

                if (rowIndex != lastRowIndex)
                {
                    nextRow = table[rowIndex + 1];
                    formattedTable = CreateSeperatorLine(maximumCellWidths, previousRow.Count(), nextRow.Count(), formattedTable);
                }

                rowIndex++;
            }

            formattedTable = CreateBottomLine(maximumCellWidths, previousRow.Count(), formattedTable);

            return formattedTable.ToString();
        }
    } 

    public class main 
    {
        public static int ComparePriority(Message a, Message b) 
        {

            int comp = Char.ToLower(a.Get_priority()[0]).CompareTo(Char.ToLower((b.Get_priority()[0])));
            if (comp == 0)
            {
                string a_time = a.Get_Time();
                int a_hour = (a_time[23] - '0') * 10 + a_time[24] - '0';
                int a_min = (a_time[26] - '0') * 10 + a_time[27] - '0';
                int a_sec = (a_time[29] - '0') * 10 + a_time[30] - '0';

                string b_time = b.Get_Time();
                int b_hour = (b_time[23] - '0') * 10 + (b_time[24] - '0');
                int b_min = (b_time[26] - '0') * 10 + b_time[27] - '0';
                int b_sec = (b_time[29] - '0') * 10 + b_time[30] - '0';

                if (a_hour > b_hour) return -1;
                else if (a_hour < b_hour) return 1;
                else
                {
                    if (a_min > b_min) return -1;
                    else if (a_min < b_min) return 1;
                    else
                    {
                        if (a_sec > b_sec) return -1;
                        else if (a_sec < b_sec) return 1;
                        else return 0;
                    }
                }

            }
            else
            {
                return comp;
            }
        } 
        static void Get_data()
        {
            string[] UN1_Friends = new string[2];
            UN1_Friends[0] = "UN2";
            UN1_Friends[1] = "UN3";
            User_Account UN1 = new User_Account("UN1", "Zaid", "Ahmad", "Irbid", "Active", "11", 28, UN1_Friends, 2);

            string[] UN2_Friends = new string[3];
            UN2_Friends[0] = "UN1";
            UN2_Friends[1] = "UN3";
            UN2_Friends[2] = "UN5";
            User_Account UN2 = new User_Account("UN2", "Omar", "Farook", "Irbid", "Active", "22", 30, UN2_Friends, 3);


            string[] UN3_Friends = new string[3];
            UN3_Friends[0] = "UN2";
            UN3_Friends[1] = "UN4";
            UN3_Friends[2] = "UN6";
            User_Account UN3 = new User_Account("UN3", "Maha", "Hani", "Amman", "Active", "33", 42, UN3_Friends, 3);

            string[] UN4_Friends = new string[2];
            UN4_Friends[0] = "UN5";
            UN4_Friends[1] = "UN6";
            User_Account UN4 = new User_Account("UN4", "Hamzah", "Ali", "Zarqa", "Active", "44", 37, UN4_Friends, 2);

            string[] UN5_Friends = new string[3];
            UN5_Friends[0] = "UN1";
            UN5_Friends[1] = "UN3";
            UN5_Friends[2] = "UN4";
            User_Account UN5 = new User_Account("UN5", "Salma", "Waleed", "Jerash", "Active", "55", 40, UN5_Friends, 3);

            string[] UN6_Friends = new string[2];
            UN6_Friends[0] = "UN1";
            UN6_Friends[1] = "UN2";
            User_Account UN6 = new User_Account("UN6", "Ali", "Khaled", "Amman", "Suspended", "66", 26, UN6_Friends, 2);

            UN1.Set_Post("UN1", "Liverpool beats Man. City 2-1", "Sport");
            UN1.Set_Post("UN1", "Apple expects to release iPhone 14 in October", "News");
            UN2.Set_Post("UN2", "Expect snow next Sunday", "Weather");
            UN3.Set_Post("UN3", "Italy fails to qualify for the World Cup", "Sport");
            UN3.Set_Post("UN3", "The deficit exceeds 2 million dollars", "Economy");
            UN5.Set_Post("UN5", "The minimum wage has been raised to 300 dinars", "Economy");
        }
        static void Login_Menu()
        {
            Console.WriteLine("[Login Menu]");
            Console.WriteLine("[1]- Login as administrator");
            Console.WriteLine("[2]- Login as user");
            Console.WriteLine("[3]- Exit");
        }
        static void Admin_Menu()
        {
            Console.WriteLine("[Admin Menu]");
            Console.WriteLine("[1]- Register new user account");
            Console.WriteLine("[2]- View all user accounts");
            Console.WriteLine("[3]- Suspend user account");
            Console.WriteLine("[4]- Activate user account");
            Console.WriteLine("[5]- Back to login screen");
        }
        static void User_Menu()
        {
            Console.WriteLine("[User Menu]");
            Console.WriteLine("[1]- Post new content");
            Console.WriteLine("[2]- Send a message");
            Console.WriteLine("[3]- View all my posts");
            Console.WriteLine("[4]- View all received messages");
            Console.WriteLine("[5]- View my last-updated wall");
            Console.WriteLine("[6]- Filter my wall");
            Console.WriteLine("[7]- Send report to administrator");
            Console.WriteLine("[8]- Back to login screen");
        }
        static void Main(string[] args)
        {
            BinaryFormatter bf = new BinaryFormatter();
            Login_Menu();
            Console.Write("-Please Enter Your Choice: ");
            string choice = Console.ReadLine();
            while (choice != "3")
            {   
                if (choice != "1" && choice != "2" && choice != "3")
                {
                    Console.WriteLine("Please enter a valid choice!");
                    Console.Write("-Re-enter your choice: ");
                    choice = Console.ReadLine();
                    continue;
                }
                if (choice == "1")
                {
                    Console.WriteLine();
                    Console.WriteLine("[]- Logging in as admin:");
                    Console.Write("-Enter Username : ");
                    string username = Console.ReadLine();

                    Console.Write("-Enter Password : ");
                    string password = Console.ReadLine();

                    if (username == "admin" && password == "0")
                    {
                        Admin_Account admin = new Admin_Account("admin", "0");
                        Console.WriteLine("Login Successful.");
                        Console.WriteLine();
                        Admin_Menu();
                        Console.Write("-Please Enter Your Choice: ");
                        string C = Console.ReadLine();
                        while (C != "5")
                        {
                            if (C != "1" && C != "2" && C != "3" && C != "4" && C != "5")
                            {
                                Console.WriteLine("Please enter a valid choice!");
                                Console.Write("-Re-enter your choice: ");
                                C = Console.ReadLine();
                                continue;
                            }
                            if (C == "1")
                            {
                                Console.WriteLine();
                                admin.Register_new_user_account();
                                Console.WriteLine();
                            }
                            if (C == "2")
                            {
                                Console.WriteLine();
                                admin.View_All_User_Accounts();
                                Console.WriteLine();
                            }
                            if (C == "3")
                            {
                                Console.WriteLine();
                                admin.Suspend_User_Account();
                                Console.WriteLine();
                            }
                            if (C == "4")
                            {
                                Console.WriteLine();
                                admin.Activate_User_Accounts();
                                Console.WriteLine();
                            }
                            Admin_Menu();
                            Console.Write("-Please Enter Your Choice: ");
                            C = Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect Login Information, Please Try Again..");
                    }
                    Console.WriteLine();
                }
                if (choice == "2")
                {
                    Console.WriteLine();
                    Console.WriteLine("[]- Logging in as user:");
                    Console.Write("-Enter Username : ");
                    string username = Console.ReadLine();

                    Console.Write("-Enter Password : ");
                    string password = Console.ReadLine();


                    FileStream UserAccounts_File = null;
                    List<User_Account> LoggedIN_User = new List<User_Account>();
                    bool found = false, suspended = false;
                    try
                    {
                        UserAccounts_File = new FileStream("UserAccounts_File.txt", FileMode.Open, FileAccess.Read);
                        while (UserAccounts_File.Position < UserAccounts_File.Length)
                        {
                            User_Account USER = (User_Account)bf.Deserialize(UserAccounts_File);
                            if (USER.Get_username() == username && USER.Get_password() == password)
                            {
                                if (USER.Get_Status() == "Suspended" || USER.Get_Status() == "suspended")
                                {
                                    suspended = true;
                                }
                                else
                                {
                                    found = true;
                                    LoggedIN_User.Add(USER);
                                }
                            }
                        }
                    }
                    catch (Exception FileNotFoundException)
                    {
                        Console.WriteLine("Incorrect Login Information, Please Try Again..");
                        Console.WriteLine();
                    }
                    finally
                    {
                        try
                        {
                            UserAccounts_File.Close();
                        }
                        catch (Exception NullReferenceException) { }
                    }

                    if (suspended)
                    {
                        Console.WriteLine("Sorry, this account is suspended.");
                        Console.WriteLine();

                        Login_Menu();
                        Console.Write("-Please Enter Your Choice: ");
                        choice = Console.ReadLine();

                        continue;
                    }
                    if (found)
                    {
                        Console.WriteLine("Welcome {0} {1}. ", LoggedIN_User[0].Get_firstname(), LoggedIN_User[0].Get_lastname());
                        Console.WriteLine();
                        User_Menu();
                        Console.WriteLine();
                        Console.Write("-Please Enter Your Choice: ");
                        string choose = Console.ReadLine();
                        while (choose != "8")
                        {
                            if (choose != "1" && choose != "2" && choose != "3" && choose != "4" && choose != "5" && choose != "6" && choose != "7" && choose != "8")
                            {
                                Console.WriteLine("Please enter a valid choice!");
                                Console.Write("-Re-enter your choice: ");
                                choose = Console.ReadLine();
                                continue;
                            }
                            if (choose == "1")
                            {
                                Console.WriteLine();
                                LoggedIN_User[0].Post_new_content();
                                Console.WriteLine();
                            }
                            if (choose == "2")
                            {
                                Console.WriteLine();
                                LoggedIN_User[0].Send_message();
                                Console.WriteLine();
                            }
                            if (choose == "3")
                            {
                                Console.WriteLine();
                                LoggedIN_User[0].View_all_my_posts();
                                Console.WriteLine();
                            }
                            if (choose == "4")
                            {
                                Console.WriteLine();
                                LoggedIN_User[0].View_all_recieved_messages();
                                Console.WriteLine();
                            }
                            if (choose == "5")
                            {
                                Console.WriteLine();
                                LoggedIN_User[0].View_my_last_updated_wall();
                                Console.WriteLine();
                            }
                            if (choose == "6")
                            {
                                Console.WriteLine();
                                LoggedIN_User[0].Filter_My_Wall();
                                Console.WriteLine();
                            }
                            if (choose == "7")
                            {
                                Console.WriteLine();
                                LoggedIN_User[0].Send_Report_to_administrator();
                                Console.WriteLine();
                            }
                            User_Menu();
                            Console.WriteLine();
                            Console.Write("-Please Enter Your Choice: ");
                            choose = Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect Login Information, Please Try Again..");
                        Console.WriteLine();
                    }
                }

                //
                Login_Menu();
                Console.Write("-Please Enter Your Choice: ");
                choice = Console.ReadLine();
            }

        }
    }
}