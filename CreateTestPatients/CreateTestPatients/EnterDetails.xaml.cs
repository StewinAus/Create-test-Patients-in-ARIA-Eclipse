using Newtonsoft.Json;
using services.varian.com.AriaConnect.PatientSelect.Contract._1._01;
using services.varian.com.AriaWebConnect.Link;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EnterDets
{
    /// <summary>
    /// Interaction logic for EnterDetails.xaml
    /// </summary>
    public partial class EnterDetails : Window
    {
        
        public EnterDetails()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            ////************API Key downloaded from MyVarian
            string apiKey = "050abb56-906c-4b46-83ef-c99b1b628026";
            ////************Constants for the creation of the patient, + firstname from the popup
            string patientId = "zzzTestPatient%", id2 = "TEST", firstName = FirstName.Text;
            ////************Check to get the details of the last patient created using the GetPatientsRequest and the Id as above with a wildcard
            string request_ptInfo = "{\"__type\":\"GetPatientsRequest:http://services.varian.com/AriaWebConnect/Link\",\"Attributes\":[],\"PatientId1\":{\"Value\":\"" + patientId + "\"}}";
            string response_ptInfo = SendData(request_ptInfo, true, apiKey);
            var ptInfo = JsonConvert.DeserializeObject<GetPatientsResponse>(response_ptInfo);

            //foreach (var ptinfo in ptInfo.Patients)
            //{
            //    Console.WriteLine(ptinfo.LastName.Value);
            //}
            //Console.ReadLine();

            ////************variable for the last patient entered
            var lastTestPatientID = ptInfo.Patients.LastOrDefault();
            ////************variable for the PatientId for the last 4 characters of the patient entered -> this will be incremented by 1 for the new patient
            var lastFour = lastTestPatientID.PatientId1.Value.Substring(lastTestPatientID.PatientId1.Value.Length - 4, 4);
            ////************ convert last 4 characters to number
            int i = int.Parse(lastFour);
            ////************increment by 1
            string newpostfix = string.Format("{0:0000}", i + 1);
            ////************add new postfix to standardised name
            string newPatient = "zzzTestPatient" + newpostfix;

            ////************same process for home phone number
            var lastHomePhNumber = lastTestPatientID.HomePhoneNumber.Value;
            int j = int.Parse(lastHomePhNumber);
            string newhomePhNumber = string.Format("{0:00000000}", j + 1);

            ////************Constatnts for assigning the correct department
            string hospName = "Las Vegas General";
            string deptId = "Radiation Therap";//16 Characters Max 

            ////************constant if DOB has not been entered in popup
            string dobNotEntered = "1895-11-08";
            ////************Check to see if DOB has been entered in popup
            DateTime? dob = DOB.SelectedDate;
            if (dob.HasValue)
            {
                dobNotEntered = dob.Value.ToString("yyyy-MM-dd");
            }

            ////************alternate sex for each patient
            string lastSex = lastTestPatientID.Sex.Value;
            Console.WriteLine(lastSex);
            if (lastSex == "Male") { lastSex = "Female"; } 
            if (lastSex == "Female") { lastSex = "Male"; }
            if (lastSex == "Unknown") { lastSex = "Male"; }
            if (lastSex is null) { lastSex = "Female"; }
            if (lastSex == "") { lastSex = "Male"; }





            string create_pt = "{\"__type\":\"CreatePatientRequest:http://services.varian.com/AriaWebConnect/Link\",\"Attributes\":[],\"PatientId1\":{\"Value\":\"" + newPatient + "\"},\"LastName\":{\"Value\":\"" + newPatient + "\"},\"PatientId2\":{\"Value\":\"" + id2 + "\"},\"FirstName\":{\"Value\":\"" + firstName + "\"},\"Birthdate\":{\"Value\":\"" + dobNotEntered.Substring(0, 10) + "\"},\"DepartmentId\":{\"Value\":\"" + deptId + "\"},\"HospitalName\":{\"Value\":\"" + hospName + "\"},\"HomePhoneNumber\":{\"Value\":\"" + newhomePhNumber + "\"},\"Sex\":{\"Value\":\"" + lastSex + "\"}}";
            string response_create_ptInfo = SendData(create_pt, true, apiKey);
            //Console.WriteLine(response_create_ptInfo);
            //Console.ReadLine();
            this.Close();
        }

        public static string SendData(string request, bool bIsJson, string apiKey)
        {
            var sMediaTYpe = bIsJson ? "application/json" :
           "application/xml";
            var sResponse = System.String.Empty;
            using (var c = new HttpClient(new
           HttpClientHandler()
            { UseDefaultCredentials = true }))
            {
                if (c.DefaultRequestHeaders.Contains("ApiKey"))
                {
                    c.DefaultRequestHeaders.Remove("ApiKey");
                }
                c.DefaultRequestHeaders.Add("ApiKey", apiKey);
                //in App.Config, change this to the Resource ID for your REST Service.
                var task =
               c.PostAsync(ConfigurationManager.AppSettings["GatewayRestUrl"],
                new StringContent(request, Encoding.UTF8,
               sMediaTYpe));
                Task.WaitAll(task);
                var responseTask =
               task.Result.Content.ReadAsStringAsync();
                Task.WaitAll(responseTask);
                sResponse = responseTask.Result;
            }
            return sResponse;
        }

        private void courseId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
