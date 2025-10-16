using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using Syncfusion.Windows.PropertyGrid;

namespace MES.WPF.Client.Models;

[PropertyGrid(NestedPropertyDisplayMode = NestedPropertyDisplayMode.None,
              PropertyName = "DOB,FavoriteColor")]
[TypeConverter(typeof(ExpandableObjects))]
[Editor("Mobile", typeof(MobileEditor))]
public class Person
{
    [Display(Order = 1)]
    [Category("Identity")]
    [DisplayName("First Name")]
    [Description("First Name of the actual person.")]
    public string FirstName { get; set; }

    [Display(Order = 2)]
    [Category("Identity")]
    [DisplayName("Last Name")]
    [Description("Last Name of the actual person.")]
    public string LastName { get; set; }

    [Display(Order = 6)]
    [Browsable(false)]
    public string MaritalStatus { get; set; }

    [Display(Order = 9)]
    [Category("Additional Info")]
    [Description("Bank in which the person has account.")]
    [ReadOnly(true)]
    public Bank Bank { get; set; }

    [Display(Order = 11)]
    [Category("Additional Info")]
    [DisplayName("Email ID")]
    [Mask(MaskAttribute.EmailId)]
    [Description("Email address of the actual person.")]
    public string Email { get; set; }

    [Display(Order = 5)]
    [Category("Identity")]
    [Description("Age of the actual person.")]
    public int Age { get; set; }

    [Display(Order = 3)]
    [Category("Identity")]
    [DisplayName("Date of Birth")]
    [Description("Birth date of the actual person.")]
    public DateTime DOB { get; set; }

    [Display(Order = 4)]
    [Category("Identity")]
    [Description("Gender information of the actual person.")]
    public Gender Gender { get; set; }

    [Display(Order = 7)]
    [Category("Additional Info")]
    [DisplayName("Favorite Color")]
    [Description("Favorite color of the actual person.")]
    public Brush FavoriteColor { get; set; }

    [Display(Order = 8)]
    [Category("Additional Info")]
    [DisplayName("Permanent Employee")]
    [Description("Determines whether the person is permanent or not.")]
    public bool IsPermanent { get; set; }

    [Display(Order = 10)]
    [Category("Additional Info")]
    [Description("License key for the person.")]
    [Bindable(false)]
    public string Key { get; set; }

    [Display(Order = 0)]
    [ReadOnly(true)]
    [Category("Identity")]
    [Description("ID of the actual person.")]
    public string ID { get; set; }

    [Display(Order = 11)]
    [Category("Additional Info")]
    [Description("Country where the actual person is located.")]
    public Country Country { get; set; }

    [Display(Order = 13)]
    [Category("Additional Info")]
    [DisplayName("Mobile Number")]
    [Description("Contact Number of the actual person.")]
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