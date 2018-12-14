using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployManager.Entities
{
    [Serializable]
    public class AppExec
    {
        public AppExec() {
            this.Id = Guid.NewGuid().ToString();            
        }

        public readonly string Id;
        public string Name { get; set; }
        public string Path { get; set; }       

    }
}
