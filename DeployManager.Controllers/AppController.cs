using DeployManager.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DeployManager
{
    public class Controller
    {

        OpenFileDialog ofd = new OpenFileDialog();

        List<AppExec> list = new List<AppExec>();      
        const string FILE_NAME = "Cache.data";
        const string Title_form = "DeployManager";

        public void Create(AppExec app)
        {
            list.Add(app);
        }

        public string getFILE_NAME() => FILE_NAME;
        public List<AppExec> getList() => list;

        public void Pre_Config(DataGridView tbl)
        {
            ofd.Filter = "batch files (*.bat, *.exe)|*.bat; *.exe";
            ListAll_tbl(tbl);
        }

        //this method works to get column value of row, My method
        public string tbl_rowItem(DataGridViewCellEventArgs e, String Column, DataGridView tbl) => tbl.Rows[e.RowIndex].Cells[Column].FormattedValue.ToString();
        //this method works to get row value of column, My method
        public DataGridViewColumn tbl_colItem(DataGridViewCellEventArgs e, DataGridView tbl) => tbl.Columns[e.ColumnIndex];

        public string InputBox(string Prompt, string Title, string DefaultResponse) 
            => Microsoft.VisualBasic.Interaction.InputBox(Prompt, Title, DefaultResponse);

        public object[] rowBase(AppExec item) => new object[] { item.Name, item.Path, "", "", "", item.Id};
        
        private void ListAll_tbl(DataGridView tbl)
        {
            try
            {
                tbl.Rows.Clear();
                list = list.Deserialize<AppExec>();
                list.ForEach(item =>
                {
                    tbl.Rows.Add(rowBase(item));
                });
            }
            catch (Exception e)
            {
                if (list.Count == 0) list.Serialize();
            }
          
        }

        public void NameEvent(DataGridViewCellEventArgs e, DataGridView tbl)
        {
            string input = InputBox("Insert the new TAG", Title_form, tbl_rowItem(e, "Tag", tbl));

            if (!input.Equals(""))
            {
                DialogResult dr = MessageBox.Show(
               "Are you shure that you wanna change item's TAG?",
               Title_form, MessageBoxButtons.YesNoCancel,
               MessageBoxIcon.Information);

                if (dr == DialogResult.Yes)
                {
                    var obj = FindIndex(e, tbl);
                    AppExec temp = list[obj];
                    temp.Name = input;
                    list[obj] = temp;
                    list.Serialize();
                    ListAll_tbl(tbl);
                }
            }
        }

        public void PathEvent(DataGridViewCellEventArgs e, DataGridView tbl)
        {
            DialogResult dr1 = MessageBox.Show(
                        "Are you shure that you wanna change item's PATH?",
                        Title_form, MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Information);

            if (dr1 == DialogResult.Yes)
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var obj = FindIndex(e, tbl);
                    AppExec temp = list[obj];
                    temp.Path = ofd.FileName;
                    list[obj] = temp;
                    list.Serialize();
                    ListAll_tbl(tbl);
                }
            }
        }

        public void DeleteEvent(DataGridViewCellEventArgs e, DataGridView tbl)
        {
            DialogResult del = MessageBox.Show(
                       "Are you shure that you wanna delete this item?",
                       Title_form, MessageBoxButtons.YesNoCancel,
                       MessageBoxIcon.Information);

            if (del == DialogResult.Yes)
            {
                var obj = FindIndex(e, tbl);
                AppExec result = list[obj];
                list.Remove(result);
                list.Serialize();
                ListAll_tbl(tbl);
            }
        }

        public int FindIndex(DataGridViewCellEventArgs e,DataGridView tbl)
        {            
            return list.FindIndex(x => x.Id == tbl_rowItem(e, "ID", tbl));
        }

        public void DeployEvent(DataGridViewCellEventArgs e, DataGridView tbl)
        {
            var file_path = tbl_rowItem(e, "Path", tbl);
            var dir_path = new FileInfo(file_path).Directory.FullName;
            Process batch_exec = new Process();
            batch_exec.StartInfo.FileName = file_path;
            batch_exec.StartInfo.WorkingDirectory = dir_path;
            batch_exec.Start();
        }

        public void PreviewEvent(DataGridViewCellEventArgs e, DataGridView tbl)
        {
            string fullPath = tbl_rowItem(e, "Path", tbl);
            var FileInfo = new FileInfo(fullPath);
            // opens the folder in explorer
            Process.Start(@FileInfo.Directory.FullName);
        }

        public void IdEvent(DataGridViewCellEventArgs e, DataGridView tbl)
        {
            
            string fullPath = tbl_rowItem(e, "Path", tbl);
            var FileInfo = new FileInfo(fullPath);

            MessageBox.Show(String.Format(
                "[ Name File: ] \n{0}\n\n[ Directory: ] \n>{1}\n\n[ FullPath: ] \n{2}\n\n[ ID: ] \n{3}\n\n[ Description: ] \n{4}",
                FileInfo.Name,
                FileInfo.Directory.FullName,
                fullPath,
                tbl_rowItem(e, "ID", tbl),
                tbl_rowItem(e, "Tag", tbl)
                ), Title_form);
        }

        public void findPath(TextBox txt)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txt.Text = ofd.FileName;
            }
        }
    }
}
