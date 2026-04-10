using npost.Data;

namespace npost.Core;

public class Constraint
{
    public static string Description(string ConstantName)
    {
        var msg = "Erro em Constraint não especificada.";
        var field = typeof(EnumConstraints).GetFields().Where(x => x.Name == ConstantName).FirstOrDefault();

        if (field != null)
        {
            var attribute = (ConstraintDescriptionAttribute)field.GetCustomAttributes(typeof(ConstraintDescriptionAttribute), true)[0];
            if (attribute != null)
            {
                msg = attribute.Message;
            }
        }

        return msg;
    }
}
