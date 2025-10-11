﻿namespace MES.WPF.Client.Models
{
    public class EmployeeInfo
    {
        int _id;
        string _firstName;
        string _lastName;
        private string _title;
        double? _salary;
        int _reportsTo;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public double? Salary
        {
            get { return _salary; }
            set { _salary = value; }
        }

        public int ReportsTo
        {
            get { return _reportsTo; }
            set { _reportsTo = value; }
        }
    }
}
