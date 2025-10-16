namespace App.Core.Security;

public static class Permissions
{
    public const string User = "User";
    public const string UserCreate = $"{User}.Create";
    public const string UserUpdate = $"{User}.Update";
    public const string UserDelete = $"{User}.Delete";
}