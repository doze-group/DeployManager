using DeployManager.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using System.Windows.Forms;

namespace DeployManager.App
{
    public partial class Form : System.Windows.Forms.Form
    {
        Controller App = new Controller();        
        public Form()
        {
            InitializeComponent();
            CenterToScreen();
            App.Pre_Config(tbl_paths);
            tbl_paths.Columns[0].Width = 250;
            tbl_paths.Columns[1].Width = 150;
        }
       

        public void Clear_txtBox()
        {
            txt_name.Text = "";
            txt_path.Text = "";
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            bool Empty = !String.IsNullOrEmpty(txt_name.Text) && !String.IsNullOrEmpty(txt_path.Text);

            if (Empty)
            {
                var appexec = new AppExec()
                {
                    Name = txt_name.Text,
                    Path = txt_path.Text
                };

                App.Create(appexec);
                App.getAppItem_List().Serialize();
                tbl_paths.Rows.Add(App.rowBase(appexec));
                
            }
            Clear_txtBox();
        }



        private void tbl_paths_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            var item_col = App.tbl_colItem(e, tbl_paths);           

            switch (item_col.Name)
            {
                case "Tag":
                    App.NameEvent(e, tbl_paths);
                    break;
                case "Deploy":
                    App.DeployEvent(e, tbl_paths);
                    break;
                case "Path":
                    App.PathEvent(e, tbl_paths);
                    break;
                case "Delete":
                    App.DeleteEvent(e, tbl_paths);
                    break;
                case "Preview":              
                    App.PreviewEvent(e, tbl_paths);
                    break;
                case "ID":
                    App.IdEvent(e, tbl_paths);
                    break;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            App.findPath(txt_path);
        }
    }
}
