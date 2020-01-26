using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using System.Windows.Controls;
using System.ComponentModel;
using DataSources;
using System.Windows;
using System.Data;
using System.Windows.Data;

namespace TabsTest.ViewModels

{
    class TabsViewModel : BindableBase
    {
        private string _realUserName;
        private int _roleID;
        private int _userID;
        private string _phoneNumber;
        private bool _isValid;
        private Visibility _canShowAllTabs;


        public TabsViewModel()
        {
            _booleanToVisibilityConverter = new BooleanToVisibilityConverter();
            _canShowAllTabs = Visibility.Collapsed;

            _isValid = false;
            UserLoginPlease = new DelegateCommand<PasswordBox>(Execute, CanExecute);
            AddAccounts = new DelegateCommand(ExecuteAddAccounts, CanExecuteAddAccounts);
            StartSearch = new DelegateCommand(ExecuteStartSearch, CanExecuteSearch);
            TryToRegisterNewUser = new DelegateCommand<PasswordBox>(ExecuteRegisterNewUser, CanExecute);
            AddOR = new DelegateCommand(ExecuteAddOR, CanExecuteAddOR);
            SubmitKW = new DelegateCommand(ExecuteGetKW, CanExecuteAny);
            SubmitEvent = new DelegateCommand(ExecuteNewEvent, CanExecuteOrg);
            UserManUpdateUser = new DelegateCommand(ExecuteUserManUpdateUser, CanExecuteAlways);

        }

        private bool CanExecuteAny() { return true; }
        private bool CanExecuteOrg() { return true; }



        //Create the function for create accounts

        private string _nameUser;
        private string _phonenumber;
        private string _roleid;
        private string _password;
        private string _enabledUser;
        private string _nameOr;
        private string _groupID;
        private string _inFor;

        public string CreateNewOR
        {
            get
            {
                return _nameOr;
            }
            set
            {
                SetProperty(ref _nameOr, value);
            }
        }



        public string CreateGroupID
        {
            get
            {
                return _groupID;
            }
            set
            {
                SetProperty(ref _groupID, value);
            }
        }
        public string Createinfor
        {
            get
            {
                return _inFor;
            }
            set
            {
                SetProperty(ref _inFor, value);
            }
        }
        public string CreateNewNameUser
        {
            get
            {
                return _nameUser;
            }
            set
            {
                SetProperty(ref _nameUser, value);
            }
        }

        public string UserPhoneNumber
        {
            get
            {
                return _phonenumber;
            }
            set
            {
                SetProperty(ref _phonenumber, value);
            }
        }
        public string RoleIFOfUser
        {
            get
            {
                return _roleid;
            }
            set
            {
                SetProperty(ref _roleid, value);
            }
        }

        public string UserPassword
        {
            get
            {
                return _password;
            }
            set
            {
                SetProperty(ref _password, value);
            }
        }

        public string EnableUser
        {
            get
            {
                return _enabledUser;
            }
            set
            {
                SetProperty(ref _enabledUser, value);
            }
        }

        

        public DelegateCommand AddAccounts
        {
            get; private set;
        }
        private bool CanExecuteAddAccounts()
        {
            return true;
        }

        private void ExecuteAddAccounts()
        {
            Connection.InsertAccounts(_nameUser, _phonenumber, SelectOR, _password, _enabledUser);
            System.Windows.MessageBox.Show("Added account");
        }


        public DelegateCommand AddOR
        {
            get; private set;
        }
        private bool CanExecuteAddOR()
        {
            return true;
        }

        private void ExecuteAddOR()
        {
            Connection.InsertOR(_nameOr, _groupID, _inFor);
            System.Windows.MessageBox.Show("Added New Organization");
        }

        //Login function
        public string RealUserName
        {
            get
            {
                return _realUserName;
            }

            set
            {
                SetProperty(ref _realUserName, value);
            }
        }

        public int RoleID
        {
            get
            {
                return _roleID;
            }
            set
            {
                SetProperty(ref _roleID, value);
            }
        }

        public bool UserIsValid
        {
            get
            {
                if (_isValid != null)
                {
                    return _isValid;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                CanShowTabs = Visibility.Visible;
                SetProperty(ref _isValid, value);
            }
        }

        public string PossibleUserName { get; set; }




        public DelegateCommand<PasswordBox> UserLoginPlease
        {
            get; private set;
        }


        private void Execute(PasswordBox passwordbox)
        {
            Connection.LogActivity("Trying to Authenticate...", "Now");
            string pass = passwordbox.Password.ToString();
            TryToAuthenticateUser(PossibleUserName, pass);
            Connection.LogActivity("Something happened.", "...");
        }

        private void TryToAuthenticateUser(string possibleUserName, string password)
        {
            ConnectivityStatus = "Connecting...";

            Connection.LogActivity("Trying this:", possibleUserName + "::" + password);

            string role = "none";

            role = Connection.GetUserRoleID(possibleUserName, password).ToString();

            if (role == null)
            {
                ConnectivityStatus = "Login failed";
                UserIsValid = false;
                Connection.LogActivity("User Authentication1", "Failed");
                return;
            }

            Connection.LogActivity("role5:", role);

            int possibleRole = 404404404;
            int.TryParse(role, out possibleRole);

            if (possibleRole < 1 || possibleRole > 2000)
            {
                ConnectivityStatus = "Login failed";
                UserIsValid = false;
                Connection.LogActivity("User Authentication2", "Failed");
                return;
            }

            ConnectivityStatus = "Connected OK";
            UserIsValid = true;

            RealUserName = possibleUserName;
            RoleID = possibleRole;

            Connection.LogActivity("User Authentication3", "SUCCESS");
        }



        private bool CanExecute(PasswordBox args)
        {
            return true;
        }

        public DelegateCommand StartSearch
        {
            get; private set;
        }


















        // ------------------ Janne's code begins
        private string _date1;
        private string _date2;

        public string Date1
        {
            get
            {
                return _date1;
            }
            set
            {
                Connection.LogActivity("Passingdate1:", ConvertToISODate(value.ToString()));
                SetProperty(ref _date1, ConvertToISODate(value.ToString()));
            }
        }
        public string Date2
        {
            get
            {
                return _date2;
            }

            set
            {
                Connection.LogActivity("Passingdate2:", ConvertToISODate(value.ToString()));
                SetProperty(ref _date2, ConvertToISODate(value.ToString()));
            }
        }

        private DataTable _eventsGotten;

        public DataTable EventsGotten
        {

            get
            {

                return _eventsGotten;
            }
            set
            {
                SetProperty(ref _eventsGotten, value);
            }

        }


        private void ExecuteStartSearch()
        {
            EventsGotten = Connection.GetDates(Date1, Date2);
        }

        private bool CanExecuteSearch()
        {
            return true;
        }

        // ------------------ Janne's code ends



        // Keywords Joona

        DataTable _GetOrgsKW = Connection.GetOrg();
        DataTable _Table1KW; DataTable _Table2KW;


        public DelegateCommand SubmitKW { get; set; }
        public int SelectOrgKW { get; set; }
        public DataTable GetOrgsKW { get { return _GetOrgsKW; } }
        public DataTable Table1KW { get { return _Table1KW; } set { SetProperty(ref _Table1KW, value); } }
        public DataTable Table2KW { get { return _Table2KW; } set { SetProperty(ref _Table2KW, value); } }

        private void ExecuteGetKW()
        {
            int next = SelectOrgKW;
            if (next > 0)
            {
                var arr = Connection.GetAllKeywords(next);
                Table1KW = arr[0];
                Table2KW = arr[1];
            }
        }

        // Add Events Joona
        DataTable _GetET = Connection.GetET();
        DataTable _GetLocationEvent = Connection.GetLocationEvent();
        DateTime _SelectDateEvent = DateTime.Now;
        DateTime[] dateArr;

        //button
        public DelegateCommand SubmitEvent { get; set; }
        //Drop down selection (one taken feature above)
        public DataTable GetET { get { return _GetET; } }
        public DataTable GetLocationEvent { get { return _GetLocationEvent; } }

        //inputs
        public int SelectOrgEvent { get; set; }
        public int SelectLocationEvent { get; set; }
        public int SelectETEvent { get; set; }
        public string InputNameEvent { get; set; }
        public string InputLimitEvent { get; set; }
        public string InputHoursEvent { get; set; }
        public DateTime SelectDateEvent { get { return _SelectDateEvent; } set { SetProperty(ref _SelectDateEvent, value); } }

        public string InputCountEvent { get; set; } //not included if empty
        public string InputFreqEvent { get; set; } //not included if empty

        //insert event
        public void EventInsert(DateTime d)
        {
            string date = d.ToString("yyyy-MM-dd");
            if (Connection.InsertEvent(SelectETEvent, SelectOrgEvent, InputNameEvent, date, SelectLocationEvent, InputLimitEvent, InputHoursEvent))
            {
                MessageBox.Show("Insert complete");
            }
            else
            {
                MessageBox.Show("Something went wrong (╯°□°)╯︵ ┻━┻");
            }
        }

        public int ValidCount()
        {
            string count = InputCountEvent; 
            if (count == "" || count == "1" || count == null) return 0; // Is empty or 1
            else
            {
                try { Convert.ToInt32(count); return 1; } // Is int 
                catch (Exception) { MessageBox.Show("Count takes prime number"); return -1; } // Invalid!
            }
        }

        //Validate Freq
        public int ValidFreq()
        {
            string freq = InputFreqEvent;
            if (freq == "" || freq == "None" || freq == null) return 0; // Is empty or None
            else if (freq == "Daily" || freq == "Weekly" || freq == "Monthly" || freq == "Yearly" ) return 1; // Preset chosen
            else
            {
                try { Convert.ToInt32(freq); return 2; } // Days chosen 
                catch (Exception) { MessageBox.Show("Frequenzy takes either preset or number of days"); return -1; } // Invalid!
            }
        }

        //Try int
        public bool TryInt(string str, string name)
        {
            try { Convert.ToInt32(str); return true; } catch (Exception) { MessageBox.Show(name + "Needs to be prime number"); return false; }
        }

        static DateTime[] SetDatesDefault(DateTime start, int count, string str)
        {
            DateTime[] arr = new DateTime[count];
            arr[0] = start;
            for (int i = 1; i < count; i++)
            {
                if (str == "Daily") { arr[i] = start.AddDays(1); start = start.AddDays(1); }

                if (str == "Weekly") { arr[i] = start.AddDays(7); start = start.AddDays(7); }

                if (str == "Monthly") { arr[i] = start.AddMonths(1); start = start.AddMonths(1); }

                if (str == "Yearly") { arr[i] = start.AddYears(1); start = start.AddYears(1); }
            }
            return arr;
        }
        static DateTime[] SetDatesCustom(DateTime start, int count, int days)
        {
            DateTime[] arr = new DateTime[count];
            arr[0] = start;
            for (int i = 1; i < count; i++)
            {
                arr[i] = start.AddDays(days);
                start = start.AddDays(days);
            }
            return arr;
        }

        //function on click
        private void ExecuteNewEvent()
        {
            if (TryInt(InputLimitEvent, "Participant limit" ) && TryInt(InputHoursEvent, "Duration")) // Needs to be valid in every case
            {
                DateTime eventDate = SelectDateEvent;
                int countE = -10; countE = ValidCount();
                if (countE == 0) EventInsert(eventDate); // No optional count = insert single
                else if (countE != -1) // Not single, but not invalid = continue testing
                {
                    int freqE = -10; freqE = ValidFreq();
                    int count = Convert.ToInt32(InputCountEvent);
                    if (freqE == 1 || freqE == 2) // Preset or days chosen
                    {
                        try
                        {
                            dateArr = new DateTime[count];
                            if (freqE == 1) // insert with preset
                            {
                                dateArr = SetDatesDefault(eventDate, count, InputFreqEvent);
                                foreach (DateTime d in dateArr)
                                {
                                    Connection.InsertEvent(SelectETEvent, SelectOrgEvent, InputNameEvent, d.ToString("yyyy-MM-dd"), SelectLocationEvent, InputLimitEvent, InputHoursEvent);
                                }
                            }
                            else if (freqE == 2) // insert with custom days
                            {
                                int days = Convert.ToInt32(InputFreqEvent);
                                dateArr = SetDatesCustom(eventDate, count, days);
                                foreach (DateTime d in dateArr)
                                {
                                    Connection.InsertEvent(SelectETEvent, SelectOrgEvent, InputNameEvent, d.ToString("yyyy-MM-dd"), SelectLocationEvent, InputLimitEvent, InputHoursEvent);
                                }
                            }
                            MessageBox.Show("Everything has been inserted");
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Insertion Failed ┻━┻ ︵ \\(°□°)/ ︵ ┻━┻" + e.ToString());
                        }
                    }
                    if (freqE == 0) MessageBox.Show("Frequenzy is required when inserting multiple"); //can't insert multiple while missing freq
                }
               

            }

        }











        // ----------------------------- Antti's code starts

        private string _connectivityStatus;

        public string ConnectivityStatus
        {
            get
            {
                return _connectivityStatus;
            }

            set
            {
                SetProperty(ref _connectivityStatus, value);
            }
        }

        public DelegateCommand<PasswordBox> TryToRegisterNewUser { get; private set; }

        private bool CanExecuteAlways()
        {
            return true;
        }

        private BooleanToVisibilityConverter _booleanToVisibilityConverter;

        private string _registerNewName;
        private string _registerPhoneNumber;

        public string RegisterNewName
        {
            get
            {
                return _registerNewName;
            }

            set
            {
                SetProperty(ref _registerNewName, value);
            }
        }

        public string RegisterPhoneNumber
        {
            get
            {
                return _registerPhoneNumber;
            }

            set
            {
                SetProperty(ref _registerPhoneNumber, value);
            }
        }


        public Visibility CanShowTabs
        {
            get
            {
                return _canShowAllTabs;
            }

            set
            {
                SetProperty(ref _canShowAllTabs, value);
            }
        }

        private string _registrationStatus;
        public string RegistrationStatus
        {
            get
            {
                return _registrationStatus;
            }

            set
            {
                SetProperty(ref _registrationStatus, value);
            }
        }

        private void ExecuteRegisterNewUser(PasswordBox passwordbox)
        {
            Connection.LogActivity("Trying to Register New User...", "Now");
            string pass = passwordbox.Password.ToString();

            Connection.RegisterNewUser(RegisterNewName, RegisterPhoneNumber, pass);
            RegistrationStatus = "New User Registered";
        }

        private DataTable _getAllUsersTable;

        public DataTable GetAllUsersTable
        {
            get
            {
                _getAllUsersTable = Connection.GetAllUsers();
                return _getAllUsersTable;
            }

            set
            {
                SetProperty(ref _getAllUsersTable, value);
            }
        }

        private DataTable _getAllPublicEvents;

        public DataTable GetAllPublicEvents
        {
            get
            {
                _getAllPublicEvents = Connection.GetAllPublicEventsTable();
                return _getAllPublicEvents;
            }

            set
            {
                SetProperty(ref _getAllPublicEvents, value);
            }
        }


        public static string ConvertToISODate(string dateToConvert)
        {
            var arr = dateToConvert.ToCharArray();
            var corrected = new char[arr.Length];

            Array.Copy(arr, 6, corrected, 0, 4);
            corrected[4] = '-';
            Array.Copy(arr, 3, corrected, 5, 2);
            corrected[7] = '-';
            Array.Copy(arr, 0, corrected, 8, 2);

            string result = new string(corrected);

            return result;
        }


        private string _selectedUserMan;

        public string SelectedUserMan
        {
            get
            {
                return _selectedUserMan;
            }

            set
            {
                SetProperty(ref _selectedUserMan, value);
            }
        }

        public DelegateCommand ChangeUserMan { get; private set; }

        private object _selectedManUser;
        public object SelectedManUser
        {
            get
            {
                return _selectedManUser;
            }
            set
            {
                SetProperty(ref _selectedManUser, value);
            }
        }

        public DelegateCommand UserManUpdateUser { get; private set; }


        private void ExecuteUserManUpdateUser()
        {






        }


        public void UpdateUserInformationToDatabase(string userName, string phoneNumber, string roleID, string organization, string enabled)
        {

            Connection.UpdateCurrentUserInformation(userName, phoneNumber, roleID, organization, enabled);
            GetAllUsersTable = Connection.GetAllUsers();

        }

        private DataTable _getAllLogs;

        public DataTable GetAllLogs
        {
            get
            {
                if (_getAllLogs == null)
                {
                    _getAllLogs = Connection.GetAllLogs();
                }

                return _getAllLogs;
            }

            set
            {
                SetProperty(ref _getAllLogs, value);
            }
        }


        public void DeleteUserCompletely(string userName)
        {
            Connection.DeleteUserCompletely(userName);
            GetAllUsersTable = Connection.GetAllUsers();
            GetAllLogs = Connection.GetAllLogs();
        }


        // ----------------------------- Antti's code ends















        public string SelectOR { get; set; }
        DataTable _GetOR = Connection.GetOR();
        public DataTable GetOR { get { return _GetOR; } }
    }


}

