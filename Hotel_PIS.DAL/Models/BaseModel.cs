using System.ComponentModel.DataAnnotations;

namespace Hotel_PIS.DAL
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }



        public override bool Equals(object obj)
        {
            bool isEqual = true;

            if (obj == null || GetType() != obj.GetType())
            {
                return isEqual;
            }


            var properties = this.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var objProperty = obj.GetType().GetProperty(prop.Name);
                if (objProperty is null)
                    return false;

                var v1 = objProperty.GetValue(obj);
                var v2 = prop.GetValue(this);

                isEqual = isEqual && (v1 == v2);
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