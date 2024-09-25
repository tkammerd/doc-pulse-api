namespace Doc.Pulse.Contracts.Communications.V1.Programs;

public class ProgramContracts_v1_0
{
    public const string VERSION = "v1.0";
    public const string CONTROLLER_NAME = "Programs";

    public const string CMD_UPDATE = $"/api/{VERSION}/{CONTROLLER_NAME}/Update";
    public const string CMD_CREATE = $"/api/{VERSION}/{CONTROLLER_NAME}/Create";
    public const string CMD_HARDDELETE = $"/api/{VERSION}/{CONTROLLER_NAME}/HardDelete";

    public const string QRY_LIST = $"/api/{VERSION}/{CONTROLLER_NAME}/List";
    public const string QRY_PAGINATEDLIST = $"/api/{VERSION}/{CONTROLLER_NAME}/ListPaginated";
    public const string QRY_GETBYID = $"/api/{VERSION}/{CONTROLLER_NAME}/GetById/{{0}}";
    public const string QRY_GETBYIDWOEXT = $"/api/{VERSION}/{CONTROLLER_NAME}/GetById_WithoutExtensions/{{0}}";    
}
