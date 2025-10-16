using System.ComponentModel;

namespace MES.WPF.Client.Models;

public class Bank
{
    [DisplayName("Name")]
    [Description("Name of the bank.")]
    public string Name
    {
        get;
        set;
    }

    [DisplayName("Customer ID")]
    [Description("Customer ID used for Net Banking.")]
    public string CustomerID
    {
        get;

        set;
    }

    [Description("Password used for Net Banking.")]
    [ReadOnly(true)]
    public string Password
    {
        get;

        set;
    }

    [DisplayName("Account Number")]
    [Description("Bank Account Number.")]
    public long AccountNumber
    {
        get;

        set;
    }


    [Description("Address of Bank.")]
    [ReadOnly(true)]
    public Address Address
    {
        get;

        set;
    }
    public override string ToString()
    {
        return Name;
    }
}

public enum Gender
{
    Male,

    Female
}

public enum Country
{
    USA,

    Germany,

    Canada,

    Mexico,

    Alaska,

    Japan,

    China,

    Peru,

    Chile,

    Argentina,

    Brazil

}