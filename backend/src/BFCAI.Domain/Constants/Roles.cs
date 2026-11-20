namespace BFCAI.Domain.Constants;

/// <summary>Identity role names (same values as the legacy StaticData.role_*).</summary>
public static class Roles
{
    public const string Admin = "Admin";
    public const string Instructor = "Instructor";
    public const string Student = "Student";
    public const string Customer = "Customer";

    public static readonly string[] All = [Admin, Instructor, Student];
}
