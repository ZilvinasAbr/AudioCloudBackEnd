namespace SaitynoProjektasBackEnd.RequestModels
{
    public class EditPlaylistRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsPublic { get; set; }
    }
}
