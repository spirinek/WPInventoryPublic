using System;

namespace WPInventory.Data.Models.Entities
{
    public class VideoCard : IEquatable<VideoCard>
    {

        //[Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Computer Computer { get; set; }
        public int ComputerId { get; set; }
        public string  CardModel { get; set; }
       
        
        public bool Equals(VideoCard other)
        {
            if (this.CardModel==other.CardModel)
                return true;
            return false;
        }
    }
}
