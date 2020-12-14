using System.ComponentModel.DataAnnotations;

namespace Fanap.DataLabeling.Wallet
{
    public class ConfirmTransferToUserInput
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
