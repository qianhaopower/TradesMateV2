

using EF.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataService.Models
{
  

    public class PropertyModel
    {
        //public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public int Id { get; set; }

        public string Condition { get; set; }

        public string Narrative { get; set; }
        public string Comment { get; set; }

        public string AddressDisplay { get; set; }

        public int ClientId { get; set; }

        public AddressModel Address { get; set; }

        public DefaultPropertySection DefaultSections { get; set; }
        public string PropertyLogoUrl { get; set; }



    }


    public class DefaultPropertySection
    {
        public short BedroomNumber { get; set; }
        public short LivingRoomNumber { get; set; }
        public short BathroomNumber { get; set; }
        public short KitchenNumber { get; set; }
        public short LaundryRoomNumber { get; set; }
        public short HallWayNumber { get; set; }
        public short DeckNumber { get; set; }
        public short BasementNumber { get; set; }
        public short GardenNumber { get; set; }
        public short GarageNumber { get; set; }

    }


}