namespace Core.Extensions
{
    public static class AddressMappingExtensions
    {
        public static AddressDto ToDto(this Address address)
        {
            ArgumentNullException.ThrowIfNull(address);
            return new AddressDto(
                    address.Line1,
                    address.Line2,
                    address.City,
                    address.State,
                    address.PostalCode,
                     address.Country
                            );

        }

        public static Address ToEntity(this AddressDto addressDto)
        {
            ArgumentNullException.ThrowIfNull(addressDto);

            return new()
            {
                City = addressDto.City,
                Country = addressDto.Country,
                Line1 = addressDto.Line1,
                Line2 = addressDto.Line2,
                PostalCode = addressDto.PostalCode,
                State = addressDto.State,
            };
        }

        public static void UpdateFromDto(this Address address, AddressDto addressDto)
        {
            ArgumentNullException.ThrowIfNull(address);

            ArgumentNullException.ThrowIfNull(addressDto);

            address.City = addressDto.City;
            address.Country = addressDto.Country;
            address.Line1 = addressDto.Line1;
            address.Line2 = addressDto.Line2;
            address.PostalCode = addressDto.PostalCode;
            address.State = addressDto.State;

        }
    }
}
