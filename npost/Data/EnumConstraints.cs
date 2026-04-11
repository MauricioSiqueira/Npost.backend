using npost.Core;

namespace npost.Data;

public enum EnumConstraints
{
    pkUsuario,
    pkNotation,

    [ConstraintDescription("Erro de chave estrangeira: A notação deve estar associada a um usuário existente.")]
    fkNotationUsuario
}
