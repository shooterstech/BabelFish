using System.Text;

namespace Scopos.BabelFish.DataModel.AttributeValue {

    [Serializable]
    public class ValidateUserID : BaseClass {
        public ValidateUserID() { }

        /// <summary>
        /// UserID
        /// </summary>
        public string UserID { get; set; } = string.Empty;

        public bool Valid{ get; set; } = false;

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("UserID status for ");
            foo.Append(UserID);
            return foo.ToString();
        }

    }
}