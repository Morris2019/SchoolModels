using System;
namespace GhanaModels
{
    public class UsePostModel
    {
        public string PostId { get; set; }
        public string User_Id { get; set; }
        public string PostCaption { get; set; }
        public string PostImage { get; set; }
        public string DateCreated { get; set; }

        public UsePostModel()
        {
        }
    }
}
