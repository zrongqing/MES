﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.RightsManagement;
using System.Windows.Media;

using MES.WPF.Client.Models;

using Syncfusion.Windows.PropertyGrid;

namespace MES.WPF.Client
{
    
    
        [PropertyGridAttribute(NestedPropertyDisplayMode = NestedPropertyDisplayMode.None,
                           PropertyName = "DOB,FavoriteColor")]
        [TypeConverter(typeof(ExpandableObjects))]
        [Editor("Mobile", typeof(MobileEditor))]
        public class Person
        {
            [Display(Order = 1)]
            [CategoryAttribute("Identity")]
            [DisplayNameAttribute("First Name")]
            [DescriptionAttribute("First Name of the actual person.")]
            public string FirstName { get; set; }

            [Display(Order = 2)]
            [CategoryAttribute("Identity")]
            [DisplayNameAttribute("Last Name")]
            [DescriptionAttribute("Last Name of the actual person.")]
            public string LastName { get; set; }

            [Display(Order = 6)]
            [Browsable(false)]
            public string MaritalStatus { get; set; }

            [Display(Order = 9)]
            [CategoryAttribute("Additional Info")]
            [DescriptionAttribute("Bank in which the person has account.")]
            [ReadOnly(true)]
            public Bank Bank { get; set; }

            [Display(Order = 11)]
            [CategoryAttribute("Additional Info")]
            [DisplayNameAttribute("Email ID")]
            [Mask(MaskAttribute.EmailId)]
            [DescriptionAttribute("Email address of the actual person.")]
            public string Email { get; set; }

            [Display(Order = 5)]
            [CategoryAttribute("Identity")]
            [DescriptionAttribute("Age of the actual person.")]
            public int Age { get; set; }

            [Display(Order = 3)]
            [CategoryAttribute("Identity")]
            [DisplayNameAttribute("Date of Birth")]
            [DescriptionAttribute("Birth date of the actual person.")]
            public DateTime DOB { get; set; }

            [Display(Order = 4)]
            [CategoryAttribute("Identity")]
            [DescriptionAttribute("Gender information of the actual person.")]
            public Gender Gender { get; set; }

            [Display(Order = 7)]
            [CategoryAttribute("Additional Info")]
            [DisplayNameAttribute("Favorite Color")]
            [DescriptionAttribute("Favorite color of the actual person.")]
            public Brush FavoriteColor { get; set; }

            [Display(Order = 8)]
            [CategoryAttribute("Additional Info")]
            [DisplayNameAttribute("Permanent Employee")]
            [DescriptionAttribute("Determines whether the person is permanent or not.")]
            public bool IsPermanent { get; set; }

            [Display(Order = 10)]
            [CategoryAttribute("Additional Info")]
            [DescriptionAttribute("License key for the person.")]
            [Bindable(false)]
            public string Key { get; set; }

            [Display(Order = 0)]
            [ReadOnly(true)]
            [CategoryAttribute("Identity")]
            [DescriptionAttribute("ID of the actual person.")]
            public string ID { get; set; }

            [Display(Order = 11)]
            [CategoryAttribute("Additional Info")]
            [DescriptionAttribute("Country where the actual person is located.")]
            public Country Country { get; set; }

            [Display(Order = 13)]
            [CategoryAttribute("Additional Info")]
            [DisplayNameAttribute("Mobile Number")]
            [DescriptionAttribute("Contact Number of the actual person.")]
            public object Mobile { get; set; }

        public Person()
        {
            FirstName = "Carl";
            LastName = "Johnson";
            Age = 30;
            Mobile = 91983467382;
            Email = "carljohnson@gmail.com";
            ID = "0005A";
            FavoriteColor = Brushes.Gray;
            IsPermanent = true;
            DOB = new DateTime(1987, 10, 16);
            Key = "dasd798@79hiujodsa';psdoiu9*(Uj0JK)(";
            Bank = new Bank()
            {
                Name = "Centura Banks",
                AccountNumber = 00987453721,
                CustomerID = "carljohnson",
                Password = "nuttertools",
                Address = new Address()
                {
                    State = "New Yark",
                    DoorNo = 87,
                    StreetName = "Martin street"
                }
            };
        }
        
    }
}
