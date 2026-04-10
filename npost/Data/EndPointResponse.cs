namespace npost.Data;

public class EndPointResponse
{
    public EndPointResponse(string message, int rows)
    {
        Msg =  message;
        RowsAffected = rows;
    }

    public EndPointResponse()
    { }

    public string? Msg { get; set; } = "";
    public int RowsAffected { get; set; }
}