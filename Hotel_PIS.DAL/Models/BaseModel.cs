using System.ComponentModel.DataAnnotations;

namespace Hotel_PIS.DAL
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }



        public override bool Equals(object obj)
        {
            bool isEqual = false;

            if (obj == null || GetType() != obj.GetType())
            {
                return isEqual;
            }


            var properties = this.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var objProperty = obj.GetType().GetProperty(prop.Name);
                isEqual = isEqual && (objProperty == prop);
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            var props = this.GetType().GetProperties();
            foreach (var p in props)
                hash = hash * 7 + p.GetHashCode();

            return hash;
        }
    }
}