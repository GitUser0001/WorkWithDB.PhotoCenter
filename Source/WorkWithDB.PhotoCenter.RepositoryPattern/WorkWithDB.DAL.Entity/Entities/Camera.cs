using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class Camera : BaseEntity<int>
    {
        public DateTime DateOfIssue { get; set; }
        public string Resolution { get; set; }
        public string Model { get; set; }
        public float CostHour { get; set; }
        public string Firm { get; set; }
        public string MadeIn { get; set; }

        public Camera Clone()
        {
            return (Camera)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "Camera(Id: {1}, DateOfIssue: {2}, Resolution: {3}, Model: {4}, CostHour: {5}, Firm: {6}, MadeIn: {7})", 
                Id, 
                DateOfIssue, 
                Resolution,
                Model, 
                CostHour, 
                Firm, 
                MadeIn);
        }
    }
}
