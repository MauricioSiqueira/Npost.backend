namespace npost.Core;

public class ConstraintDescriptionAttribute : Attribute
{
    public ConstraintDescriptionAttribute(string message)
    {
        Message = message;
    }

    public string Message { get; set; }
}