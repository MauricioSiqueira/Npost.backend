using System.Text;

namespace npost.Core;

public static class Secret
{
    public static string GetConnectionStringDb(string pgdatabase)
    {
        var pghost = GetEnvironmentVariable("PGHOST");
        var pgport = GetEnvironmentVariable("PGPORT");
        var pgpass = GetEnvironmentVariable("PGPASSWORD");
        var pguser = GetEnvironmentVariable("PGUSER");
        return $"Host={pghost};Port={pgport};Database={pgdatabase};Username={pguser};Password={pgpass};";
    }

    public static string GetConnectionStringAws()
    {//TODO: Refatorar para pegar as variaveis de ambiente do AWS
        var AccountName = GetEnvironmentVariable("AZUREBLOB_ACCOUNTNAME");
        var AccountKey = GetEnvironmentVariable("AZUREBLOB_ACCOUNTKEY");
        var EndpointSuffix = GetEnvironmentVariable("AZUREBLOB_ENDPOINTSUFFIX");
        return $"DefaultEndpointsProtocol=https;AccountName={AccountName};AccountKey={AccountKey};EndpointSuffix={EndpointSuffix}";
    }

    public static byte[] GetJWTEncodedSecretKeyToken()
    {
        var Secret = GetEnvironmentVariable("JWT_SECRETKEY") ?? String.Empty;
        return Encoding.ASCII.GetBytes(Secret);
    }

    public static string GetEnvironmentVariable(string variable)
    {
        return Environment.GetEnvironmentVariable(variable) ?? throw new InvalidCastException($"Variável de ambiente {variable} não definida.");
    }
}