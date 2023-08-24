using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Domain.ViewModels.Task
{
    public class TaskViewModel
    {
        public long Id { get; set; }
       

        [Display(Name ="name")]
        public string Name { get; set; }

        [Display(Name = "isDone")]
        public string IsDone { get; set; }

        [Display(Name = "priority")]
        public string Priority { get; set; }
        [Display(Name = "description")]
        public string Description { get; set; }
        [Display(Name = "date of created")]
        public string Created { get; set; }

    }
}
