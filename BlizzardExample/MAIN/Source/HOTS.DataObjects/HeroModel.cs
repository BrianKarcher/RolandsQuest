using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOTS.DataObjects
{
    public class HeroModel : IValidatableObject,  ICloneable
    {
        [Required]
        [RegularExpression("^[A-Za-z ]+$")]
        public string Name { get; set; }
        public HeroType Type { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [Range(5, 25)]
        public float Price { get; set; }
        public bool IsFreePromo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            Validator.TryValidateProperty(this.Name,
                new ValidationContext(this, null, null) { MemberName = "Name" },
                results);
            Validator.TryValidateProperty(this.Price,
                new ValidationContext(this, null, null) { MemberName = "Price" },
                results);

            return results;
        }

        public object Clone()
        {
            return new HeroModel()
            {
                Name = this.Name,
                DateCreated = this.DateCreated,
                IsActive = this.IsActive,
                IsDeleted = this.IsDeleted,
                IsFreePromo = this.IsFreePromo,
                Price = this.Price,
                Type = this.Type
            };
        }
    }
}
